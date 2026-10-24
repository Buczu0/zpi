namespace WebStore.ViewModels.VM;

public class InvoiceVm
{
    public int Id { get; set; }
    public string Number { get; set; } = "";
    public DateTime IssueDate { get; set; }
    public decimal TotalAmount { get; set; }
    public List<int> OrderIds { get; set; } = [];
}
