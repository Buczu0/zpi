using WebStore.Tests.Infrastructure;
using WebStore.ViewModels.VM;

namespace WebStore.Tests.UnitTests;

public sealed class AddressServiceTests : IDisposable
{
    private readonly TestEnvironment env = new();

    [Fact]
    public void Create_update_and_queries_preserve_owner()
    {
        var customer = env.Addresses.AddOrUpdateAddress(TestEnvironment.Address());
        var store = env.Addresses.AddOrUpdateAddress(TestEnvironment.Address(null, 1));
        var update = TestEnvironment.Address();
        update.Id = customer.Id;
        update.City = "Kraków";
        var result = env.Addresses.AddOrUpdateAddress(update);
        Assert.Equal(customer.Id, result.Id);
        Assert.Equal(1, result.CustomerId);
        Assert.Null(result.ApartmentNumber);
        Assert.Equal("Kraków", env.InNewContext(db => db.Addresses.Single(x => x.Id == customer.Id).City));
        Assert.Equal(new[] { customer.Id, store.Id }, env.Addresses.GetAddresses().Select(x => x.Id));
        Assert.Equal(customer.Id, env.Addresses.GetAddress(x => true)!.Id);
        Assert.Single(env.Addresses.GetAddresses(x => x.StationaryStoreId == 1));
        Assert.Null(env.Addresses.GetAddress(x => x.Id == 999));
        Assert.Empty(env.Addresses.GetAddresses(x => x.Id == 999));
    }

    [Theory]
    [InlineData("street")]
    [InlineData("building")]
    [InlineData("postal")]
    [InlineData("city")]
    [InlineData("country")]
    [InlineData("no-owner")]
    [InlineData("two-owners")]
    [InlineData("negative-id")]
    public void Invalid_values_are_rejected(string field)
    {
        var input = TestEnvironment.Address();
        switch (field)
        {
            case "street": input.Street = " "; break;
            case "building": input.BuildingNumber = ""; break;
            case "postal": input.PostalCode = ""; break;
            case "city": input.City = ""; break;
            case "country": input.Country = ""; break;
            case "no-owner": input.CustomerId = null; break;
            case "two-owners": input.StationaryStoreId = 1; break;
            case "negative-id": input.Id = -1; break;
        }
        Assert.Throws<ArgumentException>(() => env.Addresses.AddOrUpdateAddress(input));
        Assert.Empty(env.Addresses.GetAddresses());
    }

    [Theory]
    [InlineData("id")]
    [InlineData("customer")]
    [InlineData("store")]
    [InlineData("wrong-type")]
    public void Missing_reference_is_rejected(string field)
    {
        var input = TestEnvironment.Address();
        switch (field)
        {
            case "id": input.Id = 999; break;
            case "customer": input.CustomerId = 999; break;
            case "store": input.CustomerId = null; input.StationaryStoreId = 999; break;
            case "wrong-type": input.CustomerId = 3; break;
        }
        Assert.Throws<KeyNotFoundException>(() => env.Addresses.AddOrUpdateAddress(input));
        Assert.Empty(env.Addresses.GetAddresses());
    }

    [Fact]
    public void Owner_cannot_be_changed()
    {
        var saved = env.Addresses.AddOrUpdateAddress(TestEnvironment.Address());
        var input = TestEnvironment.Address(2);
        input.Id = saved.Id;
        Assert.Throws<InvalidOperationException>(() => env.Addresses.AddOrUpdateAddress(input));
        Assert.Equal(1, env.Addresses.GetAddress(x => x.Id == saved.Id)!.CustomerId);
    }

    [Fact]
    public void Customer_can_use_same_address_twice_and_clear_either_reference()
    {
        var own = env.Addresses.AddOrUpdateAddress(TestEnvironment.Address());
        var assigned = env.Addresses.SetCustomerAddresses(1, own.Id, own.Id);
        Assert.Equal(own.Id, assigned.BillingAddress!.Id);
        Assert.Equal(own.Id, assigned.ShippingAddress!.Id);
        var cleared = env.Addresses.SetCustomerAddresses(1, null, own.Id);
        Assert.Null(cleared.BillingAddress);
        Assert.Equal(own.Id, cleared.ShippingAddress!.Id);
        env.Addresses.SetCustomerAddresses(1, null, null);
        Assert.Null(env.InNewContext(db => db.Customers.Single(x => x.Id == 1).ShippingAddressId));
    }

    [Fact]
    public void Invalid_second_reference_does_not_partially_change_first()
    {
        var own = env.Addresses.AddOrUpdateAddress(TestEnvironment.Address());
        var other = env.Addresses.AddOrUpdateAddress(TestEnvironment.Address(2));
        var store = env.Addresses.AddOrUpdateAddress(TestEnvironment.Address(null, 1));
        Assert.Throws<InvalidOperationException>(() => env.Addresses.SetCustomerAddresses(1, own.Id, other.Id));
        Assert.Throws<InvalidOperationException>(() => env.Addresses.SetCustomerAddresses(1, own.Id, store.Id));
        Assert.Throws<KeyNotFoundException>(() => env.Addresses.SetCustomerAddresses(1, own.Id, 999));
        Assert.Throws<KeyNotFoundException>(() => env.Addresses.SetCustomerAddresses(999, null, null));
        Assert.Null(env.InNewContext(db => db.Customers.Single(x => x.Id == 1).BillingAddressId));
        Assert.Null(env.InNewContext(db => db.Customers.Single(x => x.Id == 1).ShippingAddressId));
    }

    [Fact]
    public void Null_arguments_are_rejected()
    {
        Assert.Throws<ArgumentNullException>(() => env.Addresses.AddOrUpdateAddress(null!));
        Assert.Throws<ArgumentNullException>(() => env.Addresses.GetAddress(null!));
    }

    public void Dispose() => env.Dispose();
}
