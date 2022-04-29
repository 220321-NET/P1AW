namespace Models;

public class ShopCart
{

    private List<Product> Details {get; set;} = new List<Product>() { };

    private decimal TotalPrice { get; set; } = 0.00M;

    public void AddArtSupply(Product ArtSupply)
    {
        decimal price = ArtSupply.Price * ArtSupply.Quantity;
        Details.Add(ArtSupply);
        TotalPrice += price;
    }

    public Product RemoveArtSupply(int ArtSupply)
    {
        int totalArtSupply = Details[ArtSupply].Quantity;
        TotalPrice -= (Details[ArtSupply].Price * totalArtSupply);
        Product product = Details[ArtSupply];
        Details.RemoveAt(ArtSupply);

        return product;
    }

    public decimal GetTotalPrice()
    {
        return TotalPrice;
    }

    public List<Product> AllProducts()
    {
        return Details;
    }

    public void ClearShopCart()
    {
        Details = new List<Product>() { };
        TotalPrice = 0.00M;
    }

    public bool IsShopCartEmpty()
    {
        bool isEmpty;

        if (Details.Count > 0)
        {
            isEmpty = false;
        }
        else
        {
            isEmpty = true;
        }

        return isEmpty;
    }

    public void ShopCartDetails()
    {
        int i = 1;
        foreach (Product product in Details)
        {
            Console.WriteLine($"[{i}] {product.Name} | ${product.Price} | {product.Quantity} QTY.");
            i++;
        }
        Console.WriteLine($"Total Price: ${TotalPrice}");
    }

    public void ShopCartContents()
    {
        throw new NotImplementedException();
    }
}