using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WebStore.Model.DataModels;

namespace WebStore.DAL.EF;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : IdentityDbContext<User, IdentityRole<int>, int>(options)
{
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Supplier> Suppliers => Set<Supplier>();
    public DbSet<StationaryStoreEmployee> Employees => Set<StationaryStoreEmployee>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<ProductStock> ProductStocks => Set<ProductStock>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderProduct> OrderProducts => Set<OrderProduct>();
    public DbSet<Invoice> Invoices => Set<Invoice>();
    public DbSet<Address> Addresses => Set<Address>();
    public DbSet<StationaryStore> Stores => Set<StationaryStore>();

    protected override void OnModelCreating(ModelBuilder b)
    {
        base.OnModelCreating(b);

        b.Entity<User>().ToTable("Users", t =>
        {
            t.HasCheckConstraint("CK_Users_FirstName", "LEN(LTRIM(RTRIM([FirstName]))) > 0");
            t.HasCheckConstraint("CK_Users_LastName", "LEN(LTRIM(RTRIM([LastName]))) > 0");
            t.HasCheckConstraint("CK_Users_EmployeeStore", "[UserType] <> 'Employee' OR [StationaryStoreId] IS NOT NULL");
        });
        b.Entity<User>().HasDiscriminator<string>("UserType")
            .HasValue<User>("User").HasValue<Customer>("Customer")
            .HasValue<Supplier>("Supplier").HasValue<StationaryStoreEmployee>("Employee");
        b.Entity<Customer>().HasMany(x => x.Addresses).WithOne(x => x.Customer).HasForeignKey(x => x.CustomerId);
        b.Entity<Customer>().HasOne(x => x.BillingAddress).WithMany().HasForeignKey(x => x.BillingAddressId);
        b.Entity<Customer>().HasOne(x => x.ShippingAddress).WithMany().HasForeignKey(x => x.ShippingAddressId);
        b.Entity<Customer>().HasMany(x => x.Orders).WithOne(x => x.Customer).HasForeignKey(x => x.CustomerId);
        b.Entity<Supplier>().HasMany(x => x.Products).WithOne(x => x.Supplier).HasForeignKey(x => x.SupplierId);

        b.Entity<Product>().ToTable("Products", t =>
        {
            t.HasCheckConstraint("CK_Products_Price", "[Price] >= 0");
            t.HasCheckConstraint("CK_Products_Weight", "[Weight] >= 0");
            t.HasCheckConstraint("CK_Products_Name", "LEN(LTRIM(RTRIM([Name]))) > 0");
            t.HasCheckConstraint("CK_Products_Description", "LEN(LTRIM(RTRIM([Description]))) > 0");
            t.HasCheckConstraint("CK_Products_Image", "DATALENGTH([ImageBytes]) > 0");
        });
        b.Entity<Product>().HasMany(x => x.Stocks).WithOne(x => x.Product).HasForeignKey(x => x.ProductId);
        b.Entity<Category>().HasMany(x => x.Products).WithOne(x => x.Category).HasForeignKey(x => x.CategoryId);
        b.Entity<Category>().ToTable("Categories", t => t.HasCheckConstraint("CK_Categories_Name", "LEN(LTRIM(RTRIM([Name]))) > 0"));
        b.Entity<ProductStock>().ToTable("ProductStocks", t => t.HasCheckConstraint("CK_ProductStocks_Quantity", "[Quantity] >= 0"));

        b.Entity<Order>().ToTable("Orders", t =>
        {
            t.HasCheckConstraint("CK_Orders_TotalAmount", "[TotalAmount] >= 0");
            t.HasCheckConstraint("CK_Orders_DeliveryDate", "[DeliveryDate] >= [OrderDate]");
            t.HasCheckConstraint("CK_Orders_TrackingNumber", "[TrackingNumber] >= 0");
        });
        b.Entity<Order>().Property(x => x.InvoiceId).IsConcurrencyToken();
        b.Entity<Order>().Property(x => x.TotalAmount).IsConcurrencyToken();
        b.Entity<OrderProduct>().HasKey(x => new { x.OrderId, x.ProductId });
        b.Entity<OrderProduct>().HasOne(x => x.Order).WithMany(x => x.OrderProducts).HasForeignKey(x => x.OrderId);
        b.Entity<OrderProduct>().HasOne(x => x.Product).WithMany(x => x.OrderProducts).HasForeignKey(x => x.ProductId);
        b.Entity<OrderProduct>().ToTable("OrderProducts", t =>
        {
            t.HasCheckConstraint("CK_OrderProducts_Quantity", "[Quantity] > 0");
            t.HasCheckConstraint("CK_OrderProducts_UnitPrice", "[UnitPrice] >= 0");
        });

        b.Entity<Invoice>().HasMany(x => x.Orders).WithOne(x => x.Invoice).HasForeignKey(x => x.InvoiceId);
        b.Entity<Invoice>().Property(x => x.Number).HasMaxLength(64);
        b.Entity<Invoice>().HasIndex(x => x.Number).IsUnique();
        b.Entity<Invoice>().ToTable("Invoices", t =>
        {
            t.HasCheckConstraint("CK_Invoices_Number", "LEN(LTRIM(RTRIM([Number]))) > 0");
            t.HasCheckConstraint("CK_Invoices_TotalAmount", "[TotalAmount] >= 0");
        });

        b.Entity<StationaryStore>().ToTable("Stores", t => t.HasCheckConstraint("CK_Stores_Name", "LEN(LTRIM(RTRIM([Name]))) > 0"));
        b.Entity<StationaryStore>().HasMany(x => x.Employees).WithOne(x => x.StationaryStore).HasForeignKey(x => x.StationaryStoreId);
        b.Entity<StationaryStore>().HasMany(x => x.Addresses).WithOne(x => x.StationaryStore).HasForeignKey(x => x.StationaryStoreId);
        b.Entity<Address>().ToTable("Addresses", t =>
        {
            t.HasCheckConstraint("CK_Addresses_Owner", "([CustomerId] IS NOT NULL AND [StationaryStoreId] IS NULL) OR ([CustomerId] IS NULL AND [StationaryStoreId] IS NOT NULL)");
            foreach (var field in new[] { "Street", "BuildingNumber", "PostalCode", "City", "Country" })
                t.HasCheckConstraint($"CK_Addresses_{field}", $"LEN(LTRIM(RTRIM([{field}]))) > 0");
        });

        var utc = new ValueConverter<DateTime, DateTime>(v => v, v => DateTime.SpecifyKind(v, DateTimeKind.Utc));
        foreach (var entity in b.Model.GetEntityTypes())
        {
            foreach (var fk in entity.GetForeignKeys())
                fk.DeleteBehavior = DeleteBehavior.NoAction;
            foreach (var property in entity.GetProperties())
            {
                if (property.ClrType == typeof(decimal))
                {
                    property.SetPrecision(18);
                    property.SetScale(2);
                }
                if (property.ClrType == typeof(DateTime))
                    property.SetValueConverter(utc);
            }
        }
    }
}
