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

public class OrderService(ApplicationDbContext db, IMapper mapper, ILogger<OrderService> logger, TimeProvider clock)
    : BaseService(db, mapper, logger), IOrderService
{
    private IQueryable<Order> Query => DbContext.Orders.AsNoTracking().Include(x => x.OrderProducts);

    public OrderVm AddOrUpdateOrder(AddOrUpdateOrderVm model)
    {
        ArgumentNullException.ThrowIfNull(model);
        Guard.Id(model.Id);
        Guard.Utc(model.DeliveryDate, nameof(model.DeliveryDate));
        if (model.TrackingNumber < 0) throw new ArgumentException("Numer śledzenia nie może być ujemny.", nameof(model.TrackingNumber));
        if (model.Items is null || model.Items.Count == 0 || model.Items.Any(x => x is null || x.Quantity <= 0))
            throw new ArgumentException("Zamówienie wymaga pozycji z dodatnimi ilościami.", nameof(model.Items));
        if (model.Items.Select(x => x.ProductId).Distinct().Count() != model.Items.Count)
            throw new ArgumentException("Produkt może wystąpić w zamówieniu tylko raz.", nameof(model.Items));

        var entity = Write(() =>
        {
            var order = model.Id.GetValueOrDefault() == 0
                ? new Order { OrderDate = clock.GetUtcNow().UtcDateTime }
                : DbContext.Orders.Include(x => x.OrderProducts).SingleOrDefault(x => x.Id == model.Id)
                    ?? throw new KeyNotFoundException("Nie znaleziono zamówienia.");
            if (order.InvoiceId.HasValue) throw new InvalidOperationException("Nie można edytować zafakturowanego zamówienia.");
            if (model.DeliveryDate < order.OrderDate)
                throw new ArgumentException("Dostawa nie może poprzedzać utworzenia zamówienia.", nameof(model.DeliveryDate));
            if (!DbContext.Customers.Any(x => x.Id == model.CustomerId))
                throw new KeyNotFoundException("Nie znaleziono klienta.");
            var ids = model.Items.Select(x => x.ProductId).ToArray();
            var products = DbContext.Products.AsNoTracking().Where(x => ids.Contains(x.Id)).ToDictionary(x => x.Id);
            if (products.Count != ids.Length) throw new KeyNotFoundException("Nie znaleziono jednego z produktów.");
            var total = Guard.SumMoney(model.Items.Select(x => products[x.ProductId].Price * x.Quantity));

            // Zmiany dopiero po sprawdzeniu całego modelu.
            order.CustomerId = model.CustomerId;
            order.DeliveryDate = model.DeliveryDate;
            order.TrackingNumber = model.TrackingNumber;
            order.TotalAmount = total;
            foreach (var removed in order.OrderProducts.Where(x => !ids.Contains(x.ProductId)).ToList())
            {
                DbContext.OrderProducts.Remove(removed);
                order.OrderProducts.Remove(removed);
            }
            foreach (var item in model.Items)
            {
                var line = order.OrderProducts.SingleOrDefault(x => x.ProductId == item.ProductId);
                if (line is null)
                {
                    line = new OrderProduct { ProductId = item.ProductId };
                    order.OrderProducts.Add(line);
                }
                line.Quantity = item.Quantity;
                line.UnitPrice = products[item.ProductId].Price;
            }
            if (model.Id.GetValueOrDefault() == 0) DbContext.Orders.Add(order);
            return order;
        });
        return Mapper.Map<OrderVm>(entity);
    }

    public OrderVm? GetOrder(Expression<Func<Order, bool>> filter) => ReadOne<Order, OrderVm>(Query, filter);
    public IReadOnlyList<OrderVm> GetOrders(Expression<Func<Order, bool>>? filter = null) => ReadMany<Order, OrderVm>(Query, filter);
}
