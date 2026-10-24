using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using WebStore.Services.ConcreteServices;
using WebStore.Services.Configuration.Profiles;
using WebStore.Services.Interfaces;

namespace WebStore.Services.Configuration;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddWebStoreServices(this IServiceCollection services)
    {
        services.TryAddSingleton<TimeProvider>(TimeProvider.System);
        services.AddLogging();
        services.AddSingleton<IMapper>(provider =>
        {
            var configuration = new MapperConfiguration(c =>
            {
                c.AddProfile<MainProfile>();
                var license = Environment.GetEnvironmentVariable("AUTOMAPPER_LICENSE_KEY");
                if (!string.IsNullOrWhiteSpace(license)) c.LicenseKey = license;
            }, provider.GetRequiredService<ILoggerFactory>());
            configuration.AssertConfigurationIsValid();
            return configuration.CreateMapper();
        });
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<IOrderService, OrderService>();
        services.AddScoped<IInvoiceService, InvoiceService>();
        services.AddScoped<IStoreService, StoreService>();
        services.AddScoped<IAddressService, AddressService>();
        return services;
    }
}
