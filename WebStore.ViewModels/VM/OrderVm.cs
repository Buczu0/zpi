namespace WebStore.ViewModels.VM;

public class OrderVm
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public DateTime OrderDate { get; set; }
    public DateTime DeliveryDate { get; set; }
    public long TrackingNumber { get; set; }
    public decimal TotalAmount { get; set; }
    public int? InvoiceId { get; set; }
    public List<OrderProductVm> Items { get; set; } = [];
}
