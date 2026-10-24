namespace WebStore.Model.DataModels;

public class StationaryStore
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public ICollection<Address> Addresses { get; set; } = new List<Address>();
    public ICollection<StationaryStoreEmployee> Employees { get; set; } = new List<StationaryStoreEmployee>();
}
