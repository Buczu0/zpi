namespace WebStore.ViewModels.VM;

public class AddOrUpdateInvoiceVm
{
    public int? Id { get; set; }
    public string Number { get; set; } = "";
    public DateTime IssueDate { get; set; }
    public List<int> OrderIds { get; set; } = [];
}
