namespace WebStore.ViewModels.VM;

public class CustomerAddressesVm
{
    public int CustomerId { get; set; }
    public AddressVm? BillingAddress { get; set; }
    public AddressVm? ShippingAddress { get; set; }
}
