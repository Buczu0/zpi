using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using WebStore.DAL.EF;
using WebStore.Model.DataModels;
using WebStore.Services.Configuration;
using WebStore.Services.Interfaces;
using WebStore.ViewModels.VM;

namespace WebStore.Tests.Infrastructure;

public sealed class TestEnvironment : IDisposable
{
    public static readonly DateTime Now = new(2026, 10, 24, 12, 0, 0, DateTimeKind.Utc);
    private readonly ServiceProvider provider;
    private readonly IServiceScope scope;
    private readonly string? ownedDatabase;
    private bool ownsSqlDatabase;
    public ApplicationDbContext Db { get; }
    public IProductService Products => Get<IProductService>();
    public IOrderService Orders => Get<IOrderService>();
    public IInvoiceService Invoices => Get<IInvoiceService>();
    public IStoreService Stores => Get<IStoreService>();
    public IAddressService Addresses => Get<IAddressService>();
    public IMapper Mapper => Get<IMapper>();

    public TestEnvironment(bool sqlServer = false, DbCommandInterceptor? interceptor = null)
    {
        var services = new ServiceCollection();
        var name = "WebStoreTests_" + Guid.NewGuid().ToString("N");
        if (sqlServer)
        {
            var cs = new SqlConnectionStringBuilder(
                Environment.GetEnvironmentVariable("WEBSTORE_TEST_SQL_CONNECTION")
                ?? @"Server=(localdb)\MSSQLLocalDB;Integrated Security=true;TrustServerCertificate=true");
            cs.InitialCatalog = name;
            cs.ConnectTimeout = 30;
            ownedDatabase = name;
            services.AddDbContext<ApplicationDbContext>(o =>
            {
                o.UseSqlServer(cs.ConnectionString, sql => sql.MaxBatchSize(1));
                if (interceptor is not null) o.AddInterceptors(interceptor);
            });
        }
        else services.AddDbContext<ApplicationDbContext>(o => o.UseInMemoryDatabase(name));

        services.AddIdentityCore<User>().AddRoles<IdentityRole<int>>().AddEntityFrameworkStores<ApplicationDbContext>();
        services.AddSingleton<TimeProvider>(new FixedClock());
        services.AddWebStoreServices();
        provider = services.BuildServiceProvider(new ServiceProviderOptions { ValidateScopes = true, ValidateOnBuild = true });
        scope = provider.CreateScope();
        Db = Get<ApplicationDbContext>();
        try
        {
            if (sqlServer)
            {
                if (Db.Database.CanConnect()) throw new InvalidOperationException("Losowa baza testowa już istnieje.");
                ownsSqlDatabase = true;
                Db.Database.Migrate();
            }
            else Db.Database.EnsureCreated();
            SeedAsync().GetAwaiter().GetResult();
        }
        catch
        {
            Dispose();
            throw;
        }
    }

    public T Get<T>() where T : notnull => scope.ServiceProvider.GetRequiredService<T>();

    public T InNewContext<T>(Func<ApplicationDbContext, T> action)
    {
        using var freshScope = provider.CreateScope();
        return action(freshScope.ServiceProvider.GetRequiredService<ApplicationDbContext>());
    }

    private async Task SeedAsync()
    {
        Db.Stores.AddRange(new StationaryStore { Name = "Centrum" }, new StationaryStore { Name = "Północ" });
        Db.Categories.Add(new Category { Name = "Elektronika", Tag = "#elektronika" });
        await Db.SaveChangesAsync();
        var users = Get<UserManager<User>>();
        User[] identities =
        [
            new Customer { UserName = "klient1", FirstName = "Anna", LastName = "Nowak" },
            new Customer { UserName = "klient2", FirstName = "Jan", LastName = "Kowalski" },
            new Supplier { UserName = "dostawca", FirstName = "Adam", LastName = "Dostawca" },
            new StationaryStoreEmployee { UserName = "pracownik", FirstName = "Ewa", LastName = "Pracownik", StationaryStoreId = 1 },
            new User { UserName = "uzytkownik", FirstName = "Piotr", LastName = "Użytkownik" }
        ];
        foreach (var user in identities)
        {
            user.RegistrationDate = Now.AddDays(-10);
            var result = await users.CreateAsync(user);
            if (!result.Succeeded) throw new InvalidOperationException(string.Join("; ", result.Errors.Select(x => x.Description)));
        }
        Db.Products.AddRange(
            new Product { Name = "Monitor", Description = "Monitor biurowy", ImageBytes = [1], Price = 10m, Weight = 1, SupplierId = 3, CategoryId = 1 },
            new Product { Name = "Mysz", Description = "Mysz komputerowa", ImageBytes = [2], Price = 5m, Weight = 0.5f, SupplierId = 3, CategoryId = 1 });
        await Db.SaveChangesAsync();
        Db.ProductStocks.AddRange(
            new ProductStock { ProductId = 1, Quantity = 2 },
            new ProductStock { ProductId = 1, Quantity = 3 });
        await Db.SaveChangesAsync();
        Db.ChangeTracker.Clear();
    }

    public static AddOrUpdateProductVm Product(int? id = null) => new()
    {
        Id = id, Name = "Klawiatura", Description = "Klawiatura komputerowa", ImageBytes = [3],
        Price = 12.50m, Weight = 0.75f, CategoryId = 1, SupplierId = 3
    };

    public static AddOrUpdateOrderVm Order(int? id = null, int customerId = 1) => new()
    {
        Id = id, CustomerId = customerId, DeliveryDate = Now.AddDays(3), TrackingNumber = 0,
        Items = [new() { ProductId = 1, Quantity = 2 }, new() { ProductId = 2, Quantity = 1 }]
    };

    public static AddOrUpdateInvoiceVm Invoice(params int[] ids) => new()
    {
        Number = " fv/01 ", IssueDate = Now.AddDays(1), OrderIds = ids.ToList()
    };

    public static AddOrUpdateAddressVm Address(int? customerId = 1, int? storeId = null) => new()
    {
        Street = "Polna", BuildingNumber = "7", PostalCode = "00-001", City = "Warszawa", Country = "Polska",
        CustomerId = customerId, StationaryStoreId = storeId
    };

    public void Dispose()
    {
        try
        {
            if (ownsSqlDatabase && ownedDatabase is not null)
            {
                var actual = Db.Database.GetDbConnection().Database;
                if (actual != ownedDatabase || !actual.StartsWith("WebStoreTests_", StringComparison.Ordinal))
                    throw new InvalidOperationException("Odmowa usunięcia bazy spoza testu.");
                Db.Database.EnsureDeleted();
            }
        }
        finally
        {
            scope.Dispose();
            provider.Dispose();
        }
    }

    private sealed class FixedClock : TimeProvider
    {
        public override DateTimeOffset GetUtcNow() => new(Now);
    }
}
