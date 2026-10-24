namespace WebStore.ViewModels.VM;

public class ProductVm
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
    public byte[] ImageBytes { get; set; } = [];
    public decimal Price { get; set; }
    public float Weight { get; set; }
    public int CategoryId { get; set; }
    public int SupplierId { get; set; }
    public int Quantity { get; set; }
}
