using WebStore.Tests.Infrastructure;
using WebStore.ViewModels.VM;

namespace WebStore.Tests.UnitTests;

public sealed class OrderServiceTests : IDisposable
{
    private readonly TestEnvironment env = new();

    [Fact]
    public void Create_calculates_snapshot_and_does_not_change_stock()
    {
        var order = env.Orders.AddOrUpdateOrder(TestEnvironment.Order());
        Assert.Equal(25m, order.TotalAmount);
        Assert.Equal(TestEnvironment.Now, order.OrderDate);
        Assert.Equal(DateTimeKind.Utc, order.OrderDate.Kind);
        Assert.Equal(new[] { 10m, 5m }, order.Items.Select(x => x.UnitPrice));
        Assert.Equal(5, env.InNewContext(db => db.ProductStocks.Sum(x => x.Quantity)));
        var product = TestEnvironment.Product(1);
        product.Price = 20m;
        env.Products.AddOrUpdateProduct(product);
        Assert.Equal(25m, env.Orders.GetOrder(x => x.Id == order.Id)!.TotalAmount);
        Assert.Equal(10m, env.Orders.GetOrder(x => x.Id == order.Id)!.Items[0].UnitPrice);
    }

    [Fact]
    public void Update_replaces_lines_and_reprices_without_resetting_date()
    {
        var saved = env.Orders.AddOrUpdateOrder(TestEnvironment.Order());
        var product = TestEnvironment.Product(1);
        product.Price = 12m;
        env.Products.AddOrUpdateProduct(product);
        var update = TestEnvironment.Order(saved.Id);
        update.Items = [new() { ProductId = 1, Quantity = 3 }];
        update.TrackingNumber = 12345;
        var result = env.Orders.AddOrUpdateOrder(update);
        Assert.Equal(36m, result.TotalAmount);
        Assert.Single(result.Items);
        Assert.Equal(12m, result.Items[0].UnitPrice);
        Assert.Equal(saved.OrderDate, result.OrderDate);
        Assert.Equal(12345, result.TrackingNumber);
        Assert.Equal(1, env.InNewContext(db => db.OrderProducts.Count()));
        Assert.Equal(1, env.InNewContext(db => db.Orders.Count()));
    }

    [Theory]
    [InlineData("empty")]
    [InlineData("duplicate")]
    [InlineData("quantity")]
    [InlineData("date")]
    [InlineData("date-kind")]
    [InlineData("tracking")]
    [InlineData("negative-id")]
    public void Invalid_update_is_atomic(string field)
    {
        var saved = env.Orders.AddOrUpdateOrder(TestEnvironment.Order());
        var input = TestEnvironment.Order(saved.Id);
        input.TrackingNumber = 100;
        switch (field)
        {
            case "empty": input.Items = []; break;
            case "duplicate": input.Items.Add(new() { ProductId = 1, Quantity = 1 }); break;
            case "quantity": input.Items[0].Quantity = 0; break;
            case "date": input.DeliveryDate = TestEnvironment.Now.AddDays(-1); break;
            case "date-kind": input.DeliveryDate = DateTime.SpecifyKind(input.DeliveryDate, DateTimeKind.Unspecified); break;
            case "tracking": input.TrackingNumber = -1; break;
            case "negative-id": input.Id = -1; break;
        }
        Assert.Throws<ArgumentException>(() => env.Orders.AddOrUpdateOrder(input));
        Assert.Equal(25m, env.InNewContext(db => db.Orders.Single().TotalAmount));
        Assert.Equal(0, env.InNewContext(db => db.Orders.Single().TrackingNumber));
    }

    [Theory]
    [InlineData("id")]
    [InlineData("customer")]
    [InlineData("product")]
    [InlineData("wrong-type")]
    public void Missing_reference_does_not_save(string field)
    {
        var input = TestEnvironment.Order();
        switch (field)
        {
            case "id": input.Id = 999; break;
            case "customer": input.CustomerId = 999; break;
            case "product": input.Items[1].ProductId = 999; break;
            case "wrong-type": input.CustomerId = 3; break;
        }
        Assert.Throws<KeyNotFoundException>(() => env.Orders.AddOrUpdateOrder(input));
        Assert.Equal(0, env.InNewContext(db => db.Orders.Count()));
        Assert.Equal(0, env.InNewContext(db => db.OrderProducts.Count()));
    }

    [Fact]
    public void Amount_overflow_is_rejected()
    {
        var product = TestEnvironment.Product(1);
        product.Price = 9999999999999999.99m;
        env.Products.AddOrUpdateProduct(product);
        Assert.Throws<ArgumentException>(() => env.Orders.AddOrUpdateOrder(TestEnvironment.Order()));
        Assert.Empty(env.Orders.GetOrders());
    }

    [Fact]
    public void Invoiced_order_cannot_be_edited()
    {
        var order = env.Orders.AddOrUpdateOrder(TestEnvironment.Order());
        env.Invoices.AddOrUpdateInvoice(TestEnvironment.Invoice(order.Id));
        Assert.Throws<InvalidOperationException>(() => env.Orders.AddOrUpdateOrder(TestEnvironment.Order(order.Id)));
        Assert.Equal(25m, env.Orders.GetOrder(x => x.Id == order.Id)!.TotalAmount);
    }

    [Fact]
    public void Read_contract_and_null_arguments()
    {
        var first = env.Orders.AddOrUpdateOrder(TestEnvironment.Order());
        var second = env.Orders.AddOrUpdateOrder(TestEnvironment.Order(customerId: 2));
        Assert.Equal(new[] { first.Id, second.Id }, env.Orders.GetOrders().Select(x => x.Id));
        Assert.Equal(first.Id, env.Orders.GetOrder(x => true)!.Id);
        Assert.Single(env.Orders.GetOrders(x => x.CustomerId == 2));
        Assert.Null(env.Orders.GetOrder(x => x.Id == 999));
        Assert.Empty(env.Orders.GetOrders(x => x.Id == 999));
        Assert.Throws<ArgumentNullException>(() => env.Orders.GetOrder(null!));
        Assert.Throws<ArgumentNullException>(() => env.Orders.AddOrUpdateOrder(null!));
    }

    public void Dispose() => env.Dispose();
}
