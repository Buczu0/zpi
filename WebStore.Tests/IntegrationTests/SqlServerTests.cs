using System.Data.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging.Abstractions;
using WebStore.Model.DataModels;
using WebStore.Services.ConcreteServices;
using WebStore.Tests.Infrastructure;

namespace WebStore.Tests.IntegrationTests;

[Trait("Category", "SqlServer")]
public sealed class SqlServerTests
{
    [Fact]
    public void Migration_is_repeatable_and_roundtrips_tph_and_utc()
    {
        using var env = new TestEnvironment(sqlServer: true);
        Assert.Single(env.Db.Database.GetAppliedMigrations());
        env.Db.Database.Migrate();
        Assert.Single(env.Db.Database.GetAppliedMigrations());
        Assert.Equal(2, env.InNewContext(db => db.Products.Count()));
        Assert.IsType<Customer>(env.InNewContext(db => db.Users.Single(x => x.Id == 1)));
        Assert.IsType<Supplier>(env.InNewContext(db => db.Users.Single(x => x.Id == 3)));
        Assert.IsType<StationaryStoreEmployee>(env.InNewContext(db => db.Users.Single(x => x.Id == 4)));
        Assert.IsType<User>(env.InNewContext(db => db.Users.Single(x => x.Id == 5)));
        Assert.Equal(DateTimeKind.Utc, env.InNewContext(db => db.Users.Single(x => x.Id == 1).RegistrationDate).Kind);
        var types = env.Db.Database.SqlQueryRaw<string>("SELECT DISTINCT [UserType] AS [Value] FROM [Users]").ToArray();
        Assert.Equal(new[] { "Customer", "Employee", "Supplier", "User" }, types.OrderBy(x => x));
        var precision = env.Db.Database.SqlQueryRaw<int>("SELECT CAST([precision] AS int) AS [Value] FROM sys.columns WHERE [object_id] = OBJECT_ID('Products') AND [name] = 'Price'").Single();
        var scale = env.Db.Database.SqlQueryRaw<int>("SELECT CAST([scale] AS int) AS [Value] FROM sys.columns WHERE [object_id] = OBJECT_ID('Products') AND [name] = 'Price'").Single();
        Assert.Equal(18, precision);
        Assert.Equal(2, scale);
    }

    [Fact]
    public void Database_enforces_foreign_keys_checks_unique_keys_and_no_cascade()
    {
        using var env = new TestEnvironment(sqlServer: true);
        var input = TestEnvironment.Order();
        input.Items = [new() { ProductId = 1, Quantity = 1 }];
        var order = env.Orders.AddOrUpdateOrder(input);
        env.Invoices.AddOrUpdateInvoice(TestEnvironment.Invoice(order.Id));

        void Reject(Action<WebStore.DAL.EF.ApplicationDbContext> change)
        {
            env.InNewContext(db =>
            {
                change(db);
                Assert.Throws<DbUpdateException>(() => db.SaveChanges());
                return true;
            });
        }

        Reject(db => db.ProductStocks.Add(new() { ProductId = 999, Quantity = 1 }));
        Reject(db => db.ProductStocks.Add(new() { ProductId = 1, Quantity = -1 }));
        Reject(db => db.Products.Single(x => x.Id == 1).Price = -1);
        Reject(db => db.Products.Single(x => x.Id == 1).Weight = -1);
        Reject(db => db.Addresses.Add(new()
        {
            Street = "Polna", BuildingNumber = "1", City = "Warszawa", PostalCode = "00-001", Country = "Polska"
        }));
        Reject(db => db.Addresses.Add(new()
        {
            Street = "Polna", BuildingNumber = "1", City = "Warszawa", PostalCode = "00-001", Country = "Polska",
            CustomerId = 1, StationaryStoreId = 1
        }));
        Reject(db => db.Invoices.Add(new() { Number = "FV/01", IssueDate = TestEnvironment.Now, TotalAmount = 1 }));
        Reject(db => db.OrderProducts.Add(new() { OrderId = order.Id, ProductId = 1, Quantity = 1, UnitPrice = 10 }));
        Reject(db => db.OrderProducts.Add(new() { OrderId = order.Id, ProductId = 2, Quantity = 0, UnitPrice = 5 }));
        Reject(db => db.OrderProducts.Add(new() { OrderId = order.Id, ProductId = 2, Quantity = 1, UnitPrice = -1 }));
        Reject(db => db.Suppliers.Remove(db.Suppliers.Single(x => x.Id == 3)));
        Reject(db => db.Stores.Remove(db.Stores.Single(x => x.Id == 1)));
        Assert.Equal(2, env.InNewContext(db => db.Products.Count()));
        Assert.Equal(2, env.InNewContext(db => db.ProductStocks.Count()));
        Assert.Equal(1, env.InNewContext(db => db.OrderProducts.Count()));
        Assert.Equal(1, env.InNewContext(db => db.Invoices.Count()));
    }

    [Fact]
    public void Five_services_work_together_on_sql_server()
    {
        using var env = new TestEnvironment(sqlServer: true);
        var product = env.Products.AddOrUpdateProduct(TestEnvironment.Product());
        var productUpdate = TestEnvironment.Product(product.Id);
        productUpdate.Price = 15.75m;
        Assert.Equal(15.75m, env.Products.AddOrUpdateProduct(productUpdate).Price);
        Assert.Equal(5, env.Products.GetProduct(x => x.Id == 1)!.Quantity);
        Assert.Single(env.Products.GetProducts(x => x.Id == product.Id));

        var store = env.Stores.AddOrUpdateStore(new() { Name = "Zachód" });
        env.Stores.AssignEmployee(4, store.Id);
        env.Stores.AddOrUpdateStore(new() { Id = store.Id, Name = "Sklep Zachód" });
        Assert.Single(env.Stores.GetStores(x => x.Id == store.Id));
        Assert.Equal(new[] { 4 }, env.Stores.GetStore(x => x.Id == store.Id)!.EmployeeIds);
        var address = env.Addresses.AddOrUpdateAddress(TestEnvironment.Address());
        env.Addresses.AddOrUpdateAddress(TestEnvironment.Address(null, store.Id));
        env.Addresses.SetCustomerAddresses(1, address.Id, address.Id);
        Assert.Equal(address.Id, env.InNewContext(db => db.Customers.Single(x => x.Id == 1).BillingAddressId));
        Assert.Single(env.Stores.GetStore(x => x.Id == store.Id)!.Addresses);
        Assert.Single(env.Addresses.GetAddresses(x => x.CustomerId == 1));

        var first = env.Orders.AddOrUpdateOrder(TestEnvironment.Order());
        var update = TestEnvironment.Order(first.Id);
        update.Items = [new() { ProductId = 1, Quantity = 3 }];
        Assert.Equal(30m, env.Orders.AddOrUpdateOrder(update).TotalAmount);
        var secondInput = TestEnvironment.Order();
        secondInput.Items = [new() { ProductId = 2, Quantity = 3 }];
        var second = env.Orders.AddOrUpdateOrder(secondInput);
        var invoice = env.Invoices.AddOrUpdateInvoice(TestEnvironment.Invoice(first.Id, second.Id));
        Assert.Equal(45m, invoice.TotalAmount);
        Assert.Equal(2, env.Orders.GetOrders(x => x.InvoiceId == invoice.Id).Count);
        Assert.Equal(45m, Assert.Single(env.Invoices.GetInvoices(x => x.Id == invoice.Id)).TotalAmount);
        Assert.Throws<InvalidOperationException>(() => env.Orders.AddOrUpdateOrder(update));
        Assert.Equal(30m, env.Orders.GetOrder(x => x.Id == first.Id)!.TotalAmount);

        var third = env.Orders.AddOrUpdateOrder(TestEnvironment.Order());
        var invalid = TestEnvironment.Invoice(first.Id, third.Id);
        invalid.Number = "FV/POWTORNA";
        Assert.Throws<InvalidOperationException>(() => env.Invoices.AddOrUpdateInvoice(invalid));
        Assert.Null(env.Orders.GetOrder(x => x.Id == third.Id)!.InvoiceId);
        Assert.Equal(1, env.InNewContext(db => db.Invoices.Count()));
        Assert.Equal(5, env.InNewContext(db => db.ProductStocks.Sum(x => x.Quantity)));
    }

    [Fact]
    public void Failure_after_order_insert_rolls_back_entire_write_and_allows_retry()
    {
        var failure = new FailOrderLineInsert();
        using var env = new TestEnvironment(sqlServer: true, interceptor: failure);
        var exception = Assert.ThrowsAny<Exception>(() => env.Orders.AddOrUpdateOrder(TestEnvironment.Order()));
        Assert.Contains("Kontrolowana awaria zapisu pozycji", exception.ToString());
        Assert.True(failure.Triggered);
        Assert.Equal(0, env.InNewContext(db => db.Orders.Count()));
        Assert.Equal(0, env.InNewContext(db => db.OrderProducts.Count()));
        failure.Enabled = false;
        var result = env.Orders.AddOrUpdateOrder(TestEnvironment.Order());
        Assert.Equal(25m, result.TotalAmount);
        Assert.Equal(1, env.InNewContext(db => db.Orders.Count()));
        Assert.Equal(2, env.InNewContext(db => db.OrderProducts.Count()));
    }

    [Fact]
    public void Reused_context_blocks_edit_after_another_context_invoices_order()
    {
        using var env = new TestEnvironment(sqlServer: true);
        var order = env.Orders.AddOrUpdateOrder(TestEnvironment.Order());
        var invoice = env.InNewContext(db =>
            new InvoiceService(db, env.Mapper, NullLogger<InvoiceService>.Instance)
                .AddOrUpdateInvoice(TestEnvironment.Invoice(order.Id)));
        var edit = TestEnvironment.Order(order.Id);
        // Suma i nagłówek pozostają identyczne; zmieniają się tylko pozycje.
        edit.Items = [new() { ProductId = 1, Quantity = 1 }, new() { ProductId = 2, Quantity = 3 }];

        Assert.Throws<InvalidOperationException>(() => env.Orders.AddOrUpdateOrder(edit));

        var persisted = env.InNewContext(db => db.Orders.Include(x => x.OrderProducts).Single(x => x.Id == order.Id));
        Assert.Equal(invoice.Id, persisted.InvoiceId);
        Assert.Equal(25m, persisted.TotalAmount);
        Assert.Equal(new[] { 2, 1 }, persisted.OrderProducts.OrderBy(x => x.ProductId).Select(x => x.Quantity));
    }

    [Fact]
    public void Reused_context_rejects_invoice_after_another_context_changes_customer()
    {
        using var env = new TestEnvironment(sqlServer: true);
        var first = env.Orders.AddOrUpdateOrder(TestEnvironment.Order());
        var second = env.Orders.AddOrUpdateOrder(TestEnvironment.Order());
        env.InNewContext(db =>
            new OrderService(db, env.Mapper, NullLogger<OrderService>.Instance, TimeProvider.System)
                .AddOrUpdateOrder(TestEnvironment.Order(first.Id, customerId: 2)));

        Assert.Throws<InvalidOperationException>(() =>
            env.Invoices.AddOrUpdateInvoice(TestEnvironment.Invoice(first.Id, second.Id)));

        Assert.Equal(0, env.InNewContext(db => db.Invoices.Count()));
        var persisted = env.InNewContext(db => db.Orders.OrderBy(x => x.Id).ToArray());
        Assert.Equal(new[] { 2, 1 }, persisted.Select(x => x.CustomerId));
        Assert.All(persisted, order => Assert.Null(order.InvoiceId));
    }

    private sealed class FailOrderLineInsert : DbCommandInterceptor
    {
        public bool Enabled { get; set; } = true;
        public bool Triggered { get; private set; }

        public override InterceptionResult<DbDataReader> ReaderExecuting(
            DbCommand command, CommandEventData eventData, InterceptionResult<DbDataReader> result)
        {
            if (Enabled && command.CommandText.Contains("INSERT INTO [OrderProducts]", StringComparison.Ordinal))
            {
                Triggered = true;
                throw new InvalidOperationException("Kontrolowana awaria zapisu pozycji.");
            }
            return result;
        }
    }
}
