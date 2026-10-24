using WebStore.Tests.Infrastructure;
using WebStore.ViewModels.VM;

namespace WebStore.Tests.UnitTests;

public sealed class ProductServiceTests : IDisposable
{
    private readonly TestEnvironment env = new();

    [Theory]
    [InlineData(null)]
    [InlineData(0)]
    public void Create_assigns_id_and_persists_all_fields(int? id)
    {
        var input = TestEnvironment.Product(id);
        var vm = env.Products.AddOrUpdateProduct(input);
        Assert.True(vm.Id > 2);
        Assert.Equal(3, env.InNewContext(db => db.Products.Count()));
        Assert.Equal(input.Name, env.InNewContext(db => db.Products.Single(x => x.Id == vm.Id).Name));
        Assert.Equal(input.ImageBytes, vm.ImageBytes);
        Assert.Equal(0, vm.Quantity);
    }

    [Fact]
    public void Update_preserves_id_count_and_stocks()
    {
        var input = TestEnvironment.Product(1);
        input.Price = 29.99m;
        var result = env.Products.AddOrUpdateProduct(input);
        Assert.Equal(1, result.Id);
        Assert.Equal(29.99m, result.Price);
        Assert.Equal(5, result.Quantity);
        Assert.Equal(2, env.InNewContext(db => db.Products.Count()));
        Assert.Equal(5, env.InNewContext(db => db.ProductStocks.Sum(x => x.Quantity)));
    }

    [Fact]
    public void Queries_filter_sort_and_return_null_or_empty()
    {
        Assert.Equal(new[] { 1, 2 }, env.Products.GetProducts().Select(x => x.Id));
        Assert.Equal(1, env.Products.GetProduct(x => true)!.Id);
        Assert.Equal(2, Assert.Single(env.Products.GetProducts(x => x.Price == 5)).Id);
        Assert.Equal(5, env.Products.GetProduct(x => x.Id == 1)!.Quantity);
        Assert.Equal(0, env.Products.GetProduct(x => x.Id == 2)!.Quantity);
        Assert.Null(env.Products.GetProduct(x => x.Id == 999));
        Assert.Empty(env.Products.GetProducts(x => x.Id == 999));
    }

    [Theory]
    [InlineData("name")]
    [InlineData("description")]
    [InlineData("image")]
    [InlineData("price-negative")]
    [InlineData("price-scale")]
    [InlineData("price-overflow")]
    [InlineData("weight-negative")]
    [InlineData("weight-nan")]
    [InlineData("weight-infinity")]
    [InlineData("id")]
    public void Invalid_values_do_not_modify_product(string field)
    {
        var input = TestEnvironment.Product(1);
        switch (field)
        {
            case "name": input.Name = " "; break;
            case "description": input.Description = ""; break;
            case "image": input.ImageBytes = []; break;
            case "price-negative": input.Price = -1; break;
            case "price-scale": input.Price = 1.001m; break;
            case "price-overflow": input.Price = 10000000000000000m; break;
            case "weight-negative": input.Weight = -1; break;
            case "weight-nan": input.Weight = float.NaN; break;
            case "weight-infinity": input.Weight = float.PositiveInfinity; break;
            case "id": input.Id = -1; break;
        }
        Assert.Throws<ArgumentException>(() => env.Products.AddOrUpdateProduct(input));
        Assert.Equal("Monitor", env.InNewContext(db => db.Products.Single(x => x.Id == 1).Name));
    }

    [Theory]
    [InlineData("id")]
    [InlineData("category")]
    [InlineData("supplier")]
    [InlineData("wrong-type")]
    public void Missing_reference_is_not_created(string field)
    {
        var input = TestEnvironment.Product(1);
        switch (field)
        {
            case "id": input.Id = 999; break;
            case "category": input.CategoryId = 999; break;
            case "supplier": input.SupplierId = 999; break;
            case "wrong-type": input.SupplierId = 1; break;
        }
        Assert.Throws<KeyNotFoundException>(() => env.Products.AddOrUpdateProduct(input));
        Assert.Equal(2, env.InNewContext(db => db.Products.Count()));
        Assert.Equal("Monitor", env.Products.GetProduct(x => x.Id == 1)!.Name);
    }

    [Fact]
    public void Null_arguments_are_rejected()
    {
        Assert.Throws<ArgumentNullException>(() => env.Products.AddOrUpdateProduct(null!));
        Assert.Throws<ArgumentNullException>(() => env.Products.GetProduct(null!));
    }

    public void Dispose() => env.Dispose();
}
