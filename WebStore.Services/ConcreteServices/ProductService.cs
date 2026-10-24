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

public class ProductService(ApplicationDbContext db, IMapper mapper, ILogger<ProductService> logger)
    : BaseService(db, mapper, logger), IProductService
{
    private IQueryable<Product> Query => DbContext.Products.AsNoTracking().Include(x => x.Stocks);

    public ProductVm AddOrUpdateProduct(AddOrUpdateProductVm model)
    {
        ArgumentNullException.ThrowIfNull(model);
        Guard.Id(model.Id);
        Guard.Text(model.Name, nameof(model.Name));
        Guard.Text(model.Description, nameof(model.Description));
        if (model.ImageBytes is null || model.ImageBytes.Length == 0)
            throw new ArgumentException("Obraz musi zawierać co najmniej jeden bajt.", nameof(model.ImageBytes));
        Guard.Money(model.Price, nameof(model.Price));
        if (!float.IsFinite(model.Weight) || model.Weight < 0)
            throw new ArgumentException("Waga musi być skończona i nieujemna.", nameof(model.Weight));

        var entity = Write(() =>
        {
            if (!DbContext.Categories.Any(x => x.Id == model.CategoryId))
                throw new KeyNotFoundException("Nie znaleziono kategorii.");
            if (!DbContext.Suppliers.Any(x => x.Id == model.SupplierId))
                throw new KeyNotFoundException("Nie znaleziono dostawcy.");
            var product = model.Id.GetValueOrDefault() == 0
                ? new Product()
                : DbContext.Products.Include(x => x.Stocks).SingleOrDefault(x => x.Id == model.Id)
                    ?? throw new KeyNotFoundException("Nie znaleziono produktu.");
            Mapper.Map(model, product);
            if (model.Id.GetValueOrDefault() == 0) DbContext.Products.Add(product);
            return product;
        });
        return Mapper.Map<ProductVm>(entity);
    }

    public ProductVm? GetProduct(Expression<Func<Product, bool>> filter) => ReadOne<Product, ProductVm>(Query, filter);
    public IReadOnlyList<ProductVm> GetProducts(Expression<Func<Product, bool>>? filter = null) => ReadMany<Product, ProductVm>(Query, filter);
}
