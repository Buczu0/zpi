using AutoMapper;
using WebStore.Model.DataModels;
using WebStore.ViewModels.VM;

namespace WebStore.Services.Configuration.Profiles;

public class MainProfile : Profile
{
    public MainProfile()
    {
        CreateMap<AddOrUpdateProductVm, Product>(MemberList.None).ForMember(x => x.Id, o => o.Ignore());
        CreateMap<AddOrUpdateStoreVm, StationaryStore>(MemberList.None).ForMember(x => x.Id, o => o.Ignore());
        CreateMap<AddOrUpdateAddressVm, Address>(MemberList.None).ForMember(x => x.Id, o => o.Ignore());
        CreateMap<Product, ProductVm>().ForMember(x => x.Quantity, o => o.MapFrom(x => x.Stocks.Sum(s => s.Quantity)));
        CreateMap<OrderProduct, OrderProductVm>();
        CreateMap<Order, OrderVm>().ForMember(x => x.Items, o => o.MapFrom(x => x.OrderProducts.OrderBy(p => p.ProductId)));
        CreateMap<Invoice, InvoiceVm>().ForMember(x => x.OrderIds, o => o.MapFrom(x => x.Orders.OrderBy(p => p.Id).Select(p => p.Id)));
        CreateMap<Address, AddressVm>();
        CreateMap<StationaryStore, StoreVm>()
            .ForMember(x => x.Addresses, o => o.MapFrom(x => x.Addresses.OrderBy(a => a.Id)))
            .ForMember(x => x.EmployeeIds, o => o.MapFrom(x => x.Employees.OrderBy(e => e.Id).Select(e => e.Id)));
    }
}
