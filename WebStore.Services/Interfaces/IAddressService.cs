using System.Linq.Expressions;
using WebStore.Model.DataModels;
using WebStore.ViewModels.VM;

namespace WebStore.Services.Interfaces;

public interface IAddressService
{
    AddressVm AddOrUpdateAddress(AddOrUpdateAddressVm model);
    AddressVm? GetAddress(Expression<Func<Address, bool>> filter);
    IReadOnlyList<AddressVm> GetAddresses(Expression<Func<Address, bool>>? filter = null);
    CustomerAddressesVm SetCustomerAddresses(int customerId, int? billingAddressId, int? shippingAddressId);
}
