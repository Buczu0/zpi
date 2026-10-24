using System.Linq.Expressions;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WebStore.DAL.EF;
using WebStore.Model.DataModels;
using WebStore.Services.Interfaces;
using WebStore.Services.Validation;
using WebStore.ViewModels.VM;

namespace WebStore.Services.ConcreteServices;

public class InvoiceService(ApplicationDbContext db, IMapper mapper, ILogger<InvoiceService> logger)
    : BaseService(db, mapper, logger), IInvoiceService
{
    private IQueryable<Invoice> Query => DbContext.Invoices.AsNoTracking().Include(x => x.Orders);

    public InvoiceVm AddOrUpdateInvoice(AddOrUpdateInvoiceVm model)
    {
        ArgumentNullException.ThrowIfNull(model);
        Guard.Id(model.Id);
        Guard.Text(model.Number, nameof(model.Number));
        var number = model.Number.Trim().ToUpperInvariant();
        if (number.Length > 64) throw new ArgumentException("Numer faktury może mieć najwyżej 64 znaki.", nameof(model.Number));
        Guard.Utc(model.IssueDate, nameof(model.IssueDate));
        if (model.OrderIds is null || model.OrderIds.Count == 0 || model.OrderIds.Distinct().Count() != model.OrderIds.Count)
            throw new ArgumentException("Wymagana jest niepusta lista różnych zamówień.", nameof(model.OrderIds));

        var entity = Write(() =>
        {
            var invoice = model.Id.GetValueOrDefault() == 0
                ? new Invoice()
                : DbContext.Invoices.Include(x => x.Orders).SingleOrDefault(x => x.Id == model.Id)
                    ?? throw new KeyNotFoundException("Nie znaleziono faktury.");
            if (DbContext.Invoices.Any(x => x.Number == number && x.Id != model.Id.GetValueOrDefault()))
                throw new InvalidOperationException("Faktura o tym numerze już istnieje.");

            var orders = DbContext.Orders.Where(x => model.OrderIds.Contains(x.Id)).OrderBy(x => x.Id).ToList();
            if (orders.Count != model.OrderIds.Count) throw new KeyNotFoundException("Nie znaleziono jednego z zamówień.");
            if (orders.Select(x => x.CustomerId).Distinct().Count() != 1)
                throw new InvalidOperationException("Faktura może obejmować zamówienia tylko jednego klienta.");
            if (model.IssueDate < orders.Max(x => x.OrderDate))
                throw new ArgumentException("Wystawienie faktury nie może poprzedzać zamówienia.", nameof(model.IssueDate));

            if (model.Id.GetValueOrDefault() == 0)
            {
                if (orders.Any(x => x.InvoiceId.HasValue))
                    throw new InvalidOperationException("Jedno z zamówień jest już zafakturowane.");
            }
            else if (!invoice.Orders.Select(x => x.Id).ToHashSet().SetEquals(model.OrderIds))
                throw new InvalidOperationException("Nie można zmienić zestawu zamówień istniejącej faktury.");

            var total = Guard.SumMoney(orders.Select(x => x.TotalAmount));
            invoice.Number = number;
            invoice.IssueDate = model.IssueDate;
            invoice.TotalAmount = total;
            if (model.Id.GetValueOrDefault() == 0)
            {
                foreach (var order in orders) invoice.Orders.Add(order);
                DbContext.Invoices.Add(invoice);
            }
            return invoice;
        });
        return Mapper.Map<InvoiceVm>(entity);
    }

    public InvoiceVm? GetInvoice(Expression<Func<Invoice, bool>> filter) => ReadOne<Invoice, InvoiceVm>(Query, filter);
    public IReadOnlyList<InvoiceVm> GetInvoices(Expression<Func<Invoice, bool>>? filter = null) => ReadMany<Invoice, InvoiceVm>(Query, filter);
}
