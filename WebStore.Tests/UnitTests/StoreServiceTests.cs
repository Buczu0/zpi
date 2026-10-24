using WebStore.Tests.Infrastructure;
using WebStore.ViewModels.VM;

namespace WebStore.Tests.UnitTests;

public sealed class StoreServiceTests : IDisposable
{
    private readonly TestEnvironment env = new();

    [Fact]
    public void Create_update_and_queries_include_addresses_and_staff()
    {
        var created = env.Stores.AddOrUpdateStore(new() { Name = "Nowy" });
        Assert.Empty(created.Addresses);
        Assert.Empty(created.EmployeeIds);
        var updated = env.Stores.AddOrUpdateStore(new() { Id = created.Id, Name = "Nowa nazwa" });
        Assert.Equal(created.Id, updated.Id);
        Assert.Equal("Nowa nazwa", env.InNewContext(db => db.Stores.Single(x => x.Id == created.Id).Name));
        env.Addresses.AddOrUpdateAddress(TestEnvironment.Address(null, 1));
        var first = env.Stores.GetStore(x => x.Id == 1)!;
        Assert.Single(first.Addresses);
        Assert.Equal(new[] { 4 }, first.EmployeeIds);
        Assert.Equal(new[] { 1, 2, created.Id }, env.Stores.GetStores().Select(x => x.Id));
        Assert.Equal(1, env.Stores.GetStore(x => true)!.Id);
        Assert.Single(env.Stores.GetStores(x => x.Name == "Północ"));
        Assert.Null(env.Stores.GetStore(x => x.Id == 999));
        Assert.Empty(env.Stores.GetStores(x => x.Id == 999));
    }

    [Fact]
    public void Employee_is_moved_between_stores()
    {
        env.Stores.AssignEmployee(4, 2);
        Assert.Empty(env.Stores.GetStore(x => x.Id == 1)!.EmployeeIds);
        Assert.Equal(new[] { 4 }, env.Stores.GetStore(x => x.Id == 2)!.EmployeeIds);
        env.Stores.AssignEmployee(4, 2);
        Assert.Single(env.Stores.GetStore(x => x.Id == 2)!.EmployeeIds);
    }

    [Theory]
    [InlineData(999, 2)]
    [InlineData(1, 2)]
    [InlineData(3, 2)]
    [InlineData(5, 2)]
    [InlineData(4, 999)]
    public void Invalid_assignment_does_not_move_employee(int employee, int store)
    {
        Assert.Throws<KeyNotFoundException>(() => env.Stores.AssignEmployee(employee, store));
        Assert.Equal(1, env.InNewContext(db => db.Employees.Single().StationaryStoreId));
    }

    [Fact]
    public void Invalid_write_and_null_arguments_are_rejected()
    {
        Assert.Throws<ArgumentException>(() => env.Stores.AddOrUpdateStore(new() { Name = " " }));
        Assert.Throws<ArgumentException>(() => env.Stores.AddOrUpdateStore(new() { Id = -1, Name = "Nowy" }));
        Assert.Throws<KeyNotFoundException>(() => env.Stores.AddOrUpdateStore(new() { Id = 999, Name = "Nowy" }));
        Assert.Throws<ArgumentNullException>(() => env.Stores.AddOrUpdateStore(null!));
        Assert.Throws<ArgumentNullException>(() => env.Stores.GetStore(null!));
        Assert.Equal(2, env.InNewContext(db => db.Stores.Count()));
    }

    public void Dispose() => env.Dispose();
}
