namespace WebStore.ViewModels.VM;

public class AddOrUpdateOrderVm
{
    public int? Id { get; set; }
    public int CustomerId { get; set; }
    public DateTime DeliveryDate { get; set; }
    public long TrackingNumber { get; set; }
    public List<AddOrderProductVm> Items { get; set; } = [];
}
