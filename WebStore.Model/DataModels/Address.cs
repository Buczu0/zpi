namespace WebStore.Model.DataModels;

public class Address
{
    public int Id { get; set; }
    public string Street { get; set; } = "";
    public string BuildingNumber { get; set; } = "";
    public string? ApartmentNumber { get; set; }
    public string PostalCode { get; set; } = "";
    public string City { get; set; } = "";
    public string Country { get; set; } = "";
    public int? CustomerId { get; set; }
    public Customer? Customer { get; set; }
    public int? StationaryStoreId { get; set; }
    public StationaryStore? StationaryStore { get; set; }
}
