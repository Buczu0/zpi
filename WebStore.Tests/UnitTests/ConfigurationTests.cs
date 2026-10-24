using Microsoft.AspNetCore.Identity;
using WebStore.Model.DataModels;
using WebStore.Services.ConcreteServices;
using WebStore.Services.Interfaces;
using WebStore.Tests.Infrastructure;

namespace WebStore.Tests.UnitTests;

public sealed class ConfigurationTests
{
    [Fact]
    public void All_services_resolve_and_mapper_configuration_is_valid()
    {
        using var env = new TestEnvironment();
        env.Mapper.ConfigurationProvider.AssertConfigurationIsValid();
        Assert.IsAssignableFrom<BaseService>(env.Get<IProductService>());
        Assert.IsAssignableFrom<BaseService>(env.Get<IOrderService>());
        Assert.IsAssignableFrom<BaseService>(env.Get<IInvoiceService>());
        Assert.IsAssignableFrom<BaseService>(env.Get<IStoreService>());
        Assert.IsAssignableFrom<BaseService>(env.Get<IAddressService>());
        Assert.NotNull(env.Get<UserManager<User>>());
        Assert.NotNull(env.Get<RoleManager<IdentityRole<int>>>());
    }

    [Fact]
    public void Public_view_models_do_not_expose_credentials()
    {
        var forbidden = new[] { "Password", "PasswordHash", "SecurityStamp", "ConcurrencyStamp" };
        foreach (var type in typeof(WebStore.ViewModels.VM.ProductVm).Assembly.GetTypes())
            Assert.DoesNotContain(type.GetProperties(), p => forbidden.Contains(p.Name));
    }
}
