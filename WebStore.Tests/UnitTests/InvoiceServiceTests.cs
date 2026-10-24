using WebStore.Tests.Infrastructure;
using WebStore.ViewModels.VM;

namespace WebStore.Tests.UnitTests;

public sealed class InvoiceServiceTests : IDisposable
{
    private readonly TestEnvironment env = new();

    private (int First, int Second) CreateOrders()
    {
        var first = env.Orders.AddOrUpdateOrder(TestEnvironment.Order()).Id;
        var input = TestEnvironment.Order();
        input.Items = [new() { ProductId = 2, Quantity = 3 }];
        return (first, env.Orders.AddOrUpdateOrder(input).Id);
    }

    [Fact]
    public void Groups_orders_normalizes_number_and_freezes_amount()
    {
        var ids = CreateOrders();
        var result = env.Invoices.AddOrUpdateInvoice(TestEnvironment.Invoice(ids.First, ids.Second));
        Assert.Equal("FV/01", result.Number);
        Assert.Equal(40m, result.TotalAmount);
        Assert.Equal(new[] { ids.First, ids.Second }, result.OrderIds);
        Assert.Equal(2, env.InNewContext(db => db.Orders.Count(x => x.InvoiceId == result.Id)));
        var product = TestEnvironment.Product(1);
        product.Price = 100;
        env.Products.AddOrUpdateProduct(product);
        Assert.Equal(40m, env.Invoices.GetInvoice(x => x.Id == result.Id)!.TotalAmount);
    }

    [Fact]
    public void Update_changes_only_header()
    {
        var ids = CreateOrders();
        var saved = env.Invoices.AddOrUpdateInvoice(TestEnvironment.Invoice(ids.First, ids.Second));
        var input = TestEnvironment.Invoice(ids.Second, ids.First);
        input.Id = saved.Id;
        input.Number = " fv/02 ";
        input.IssueDate = TestEnvironment.Now.AddDays(2);
        var result = env.Invoices.AddOrUpdateInvoice(input);
        Assert.Equal(saved.Id, result.Id);
        Assert.Equal("FV/02", result.Number);
        Assert.Equal(40m, result.TotalAmount);
        Assert.Equal(saved.OrderIds, result.OrderIds);
        Assert.Equal(1, env.InNewContext(db => db.Invoices.Count()));
    }

    [Theory]
    [InlineData("empty-number")]
    [InlineData("long-number")]
    [InlineData("date")]
    [InlineData("date-kind")]
    [InlineData("empty-orders")]
    [InlineData("duplicate-orders")]
    [InlineData("negative-id")]
    public void Invalid_values_leave_orders_unassigned(string field)
    {
        var order = env.Orders.AddOrUpdateOrder(TestEnvironment.Order());
        var input = TestEnvironment.Invoice(order.Id);
        switch (field)
        {
            case "empty-number": input.Number = " "; break;
            case "long-number": input.Number = new string('a', 65); break;
            case "date": input.IssueDate = TestEnvironment.Now.AddDays(-1); break;
            case "date-kind": input.IssueDate = DateTime.SpecifyKind(input.IssueDate, DateTimeKind.Unspecified); break;
            case "empty-orders": input.OrderIds = []; break;
            case "duplicate-orders": input.OrderIds.Add(order.Id); break;
            case "negative-id": input.Id = -1; break;
        }
        Assert.Throws<ArgumentException>(() => env.Invoices.AddOrUpdateInvoice(input));
        Assert.Empty(env.Invoices.GetInvoices());
        Assert.Null(env.Orders.GetOrder(x => x.Id == order.Id)!.InvoiceId);
    }

    [Fact]
    public void Different_customers_are_rejected_atomically()
    {
        var first = env.Orders.AddOrUpdateOrder(TestEnvironment.Order());
        var other = env.Orders.AddOrUpdateOrder(TestEnvironment.Order(customerId: 2));
        Assert.Throws<InvalidOperationException>(() => env.Invoices.AddOrUpdateInvoice(TestEnvironment.Invoice(first.Id, other.Id)));
        Assert.Empty(env.Invoices.GetInvoices());
        Assert.All(env.Orders.GetOrders(), x => Assert.Null(x.InvoiceId));
    }

    [Fact]
    public void Duplicate_number_and_repeated_invoicing_are_rejected()
    {
        var ids = CreateOrders();
        var original = env.Invoices.AddOrUpdateInvoice(TestEnvironment.Invoice(ids.First));
        Assert.Throws<InvalidOperationException>(() => env.Invoices.AddOrUpdateInvoice(TestEnvironment.Invoice(ids.Second)));
        var repeated = TestEnvironment.Invoice(ids.First, ids.Second);
        repeated.Number = "FV/INNA";
        Assert.Throws<InvalidOperationException>(() => env.Invoices.AddOrUpdateInvoice(repeated));
        Assert.Null(env.Orders.GetOrder(x => x.Id == ids.Second)!.InvoiceId);
        Assert.Equal(original.Id, Assert.Single(env.Invoices.GetInvoices()).Id);
    }

    [Fact]
    public void Existing_order_set_cannot_be_changed()
    {
        var ids = CreateOrders();
        var saved = env.Invoices.AddOrUpdateInvoice(TestEnvironment.Invoice(ids.First));
        var changed = TestEnvironment.Invoice(ids.Second);
        changed.Id = saved.Id;
        changed.Number = "ZMIENIONA";
        Assert.Throws<InvalidOperationException>(() => env.Invoices.AddOrUpdateInvoice(changed));
        Assert.Equal("FV/01", env.Invoices.GetInvoice(x => x.Id == saved.Id)!.Number);
        Assert.Null(env.Orders.GetOrder(x => x.Id == ids.Second)!.InvoiceId);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void Missing_invoice_or_order_does_not_create_data(bool missingInvoice)
    {
        var ids = CreateOrders();
        var input = TestEnvironment.Invoice(ids.First, missingInvoice ? ids.Second : 999);
        if (missingInvoice) input.Id = 999;
        Assert.Throws<KeyNotFoundException>(() => env.Invoices.AddOrUpdateInvoice(input));
        Assert.Empty(env.Invoices.GetInvoices());
        Assert.All(env.Orders.GetOrders(), x => Assert.Null(x.InvoiceId));
    }

    [Fact]
    public void Read_contract_and_null_arguments()
    {
        var ids = CreateOrders();
        var first = env.Invoices.AddOrUpdateInvoice(TestEnvironment.Invoice(ids.First));
        var input = TestEnvironment.Invoice(ids.Second);
        input.Number = "FV/02";
        var second = env.Invoices.AddOrUpdateInvoice(input);
        Assert.Equal(new[] { first.Id, second.Id }, env.Invoices.GetInvoices().Select(x => x.Id));
        Assert.Equal(first.Id, env.Invoices.GetInvoice(x => true)!.Id);
        Assert.Single(env.Invoices.GetInvoices(x => x.Number == "FV/02"));
        Assert.Null(env.Invoices.GetInvoice(x => x.Id == 999));
        Assert.Empty(env.Invoices.GetInvoices(x => x.Id == 999));
        Assert.Throws<ArgumentNullException>(() => env.Invoices.GetInvoice(null!));
        Assert.Throws<ArgumentNullException>(() => env.Invoices.AddOrUpdateInvoice(null!));
    }

    public void Dispose() => env.Dispose();
}
