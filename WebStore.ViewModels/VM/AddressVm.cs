namespace WebStore.ViewModels.VM;

public class AddressVm
{
    public int Id { get; set; }
    public string Street { get; set; } = "";
    public string BuildingNumber { get; set; } = "";
    public string? ApartmentNumber { get; set; }
    public string PostalCode { get; set; } = "";
    public string City { get; set; } = "";
    public string Country { get; set; } = "";
    public int? CustomerId { get; set; }
    public int? StationaryStoreId { get; set; }
}
