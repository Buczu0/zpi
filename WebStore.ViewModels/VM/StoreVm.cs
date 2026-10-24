namespace WebStore.ViewModels.VM;

public class StoreVm
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public List<AddressVm> Addresses { get; set; } = [];
    public List<int> EmployeeIds { get; set; } = [];
}
