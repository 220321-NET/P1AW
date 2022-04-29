
namespace Models;
using System.Text.RegularExpressions;


public class Manager : Customer
{
    public string ProductName { get; private set; }

    public void AddProduct()
    {
        ProductName:
        Product newProduct = new Product();
        string regularExpression = "^[a-zA-Z ]+$";
        Regex regex = new Regex(regularExpression);

        Console.WriteLine($"What is the name of the art supply you would like to add?");

        if (regex.IsMatch(ProductName))
        {
            newProduct.Name = ProductName;
        }
        else
        {
            Console.WriteLine("Invalid Input, only letters allowed");
            goto ProductName;
        }

        ProductDetails:
        Console.WriteLine("Give a Details of the product:");
        string productDetails = Console.ReadLine();

        if (regex.IsMatch(productDetails))
        {
            newProduct.Details = productDetails;
        }
        else
        {
            Console.WriteLine("Invalid Input, only letters allowed");
            goto ProductDetails;
        }

    ProductPrice:
        Console.WriteLine("How much will this product cost:");
        try
        {
            double productPrice = (double)Convert.ToDecimal(Console.ReadLine());
        }
        catch (Exception)
        {
            goto ProductPrice;
        }
    }

    public void RemoveProduct()
    {
        // Completely remove a product from the store
    }

    public void AdjustProductQuantity()
    {
        // Search through products to adjust quantity of a specific product
    }
}
