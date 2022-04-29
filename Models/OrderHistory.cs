namespace Models;


public class OrderHistory
{
    public int StoreFront {get; set;}
    public int OrderId {get; set;}
    public decimal TotalPrice {get; set;}
    public int ArtSupplyQty {get; set;}
    public decimal ArtSupplyPrice {get;set; }
    public DateTime DateOrdered {get; set;}
    public Customer customer = new Customer();
    public StoreFront storeFront = new StoreFront();
    public string ProductName { get; set; } = "";
}