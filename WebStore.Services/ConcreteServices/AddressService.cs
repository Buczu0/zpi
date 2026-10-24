using System.Linq.Expressions;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WebStore.DAL.EF;
using WebStore.Model.DataModels;
using WebStore.Services.Interfaces;
using WebStore.Services.Validation;
using WebStore.ViewModels.VM;

namespace WebStore.Services.ConcreteServices;

public class AddressService(ApplicationDbContext db, IMapper mapper, ILogger<AddressService> logger)
    : BaseService(db, mapper, logger), IAddressService
{
    private IQueryable<Address> Query => DbContext.Addresses.AsNoTracking();

    public AddressVm AddOrUpdateAddress(AddOrUpdateAddressVm model)
    {
        ArgumentNullException.ThrowIfNull(model);
        Guard.Id(model.Id);
        Guard.Text(model.Street, nameof(model.Street));
        Guard.Text(model.BuildingNumber, nameof(model.BuildingNumber));
        Guard.Text(model.PostalCode, nameof(model.PostalCode));
        Guard.Text(model.City, nameof(model.City));
        Guard.Text(model.Country, nameof(model.Country));
        if (model.CustomerId.HasValue == model.StationaryStoreId.HasValue)
            throw new ArgumentException("Adres musi mieć dokładnie jednego właściciela.", nameof(model));

        var entity = Write(() =>
        {
            if (model.CustomerId.HasValue && !DbContext.Customers.Any(x => x.Id == model.CustomerId))
                throw new KeyNotFoundException("Nie znaleziono klienta.");
            if (model.StationaryStoreId.HasValue && !DbContext.Stores.Any(x => x.Id == model.StationaryStoreId))
                throw new KeyNotFoundException("Nie znaleziono sklepu.");
            var address = model.Id.GetValueOrDefault() == 0
                ? new Address()
                : DbContext.Addresses.SingleOrDefault(x => x.Id == model.Id)
                    ?? throw new KeyNotFoundException("Nie znaleziono adresu.");
            if (model.Id.GetValueOrDefault() > 0 &&
                (address.CustomerId != model.CustomerId || address.StationaryStoreId != model.StationaryStoreId))
                throw new InvalidOperationException("Nie można zmienić właściciela adresu.");
            Mapper.Map(model, address);
            if (model.Id.GetValueOrDefault() == 0) DbContext.Addresses.Add(address);
            return address;
        });
        return Mapper.Map<AddressVm>(entity);
    }

    public CustomerAddressesVm SetCustomerAddresses(int customerId, int? billingAddressId, int? shippingAddressId)
    {
        var customer = Write(() =>
        {
            var owner = DbContext.Customers.SingleOrDefault(x => x.Id == customerId)
                ?? throw new KeyNotFoundException("Nie znaleziono klienta.");
            var billing = OwnedAddress(billingAddressId, customerId);
            var shipping = OwnedAddress(shippingAddressId, customerId);
            owner.BillingAddress = billing;
            owner.BillingAddressId = billingAddressId;
            owner.ShippingAddress = shipping;
            owner.ShippingAddressId = shippingAddressId;
            return owner;
        });
        return new CustomerAddressesVm
        {
            CustomerId = customer.Id,
            BillingAddress = Mapper.Map<AddressVm?>(customer.BillingAddress),
            ShippingAddress = Mapper.Map<AddressVm?>(customer.ShippingAddress)
        };
    }

    private Address? OwnedAddress(int? id, int customerId)
    {
        if (!id.HasValue) return null;
        var address = DbContext.Addresses.SingleOrDefault(x => x.Id == id)
            ?? throw new KeyNotFoundException("Nie znaleziono adresu.");
        if (address.CustomerId != customerId)
            throw new InvalidOperationException("Adres nie należy do wskazanego klienta.");
        return address;
    }

    public AddressVm? GetAddress(Expression<Func<Address, bool>> filter) => ReadOne<Address, AddressVm>(Query, filter);
    public IReadOnlyList<AddressVm> GetAddresses(Expression<Func<Address, bool>>? filter = null) => ReadMany<Address, AddressVm>(Query, filter);
}
