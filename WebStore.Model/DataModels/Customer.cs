namespace WebStore.Model.DataModels;

public class Customer : User
{
    public int? BillingAddressId { get; set; }
    public Address? BillingAddress { get; set; }
    public int? ShippingAddressId { get; set; }
    public Address? ShippingAddress { get; set; }
    public ICollection<Address> Addresses { get; set; } = new List<Address>();
    public ICollection<Order> Orders { get; set; } = new List<Order>();
}
