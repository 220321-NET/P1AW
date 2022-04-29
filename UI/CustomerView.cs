using BL;
using Models;

namespace UI;

public class CustomerView
{
    private ShopCart ShopCart = new ShopCart();
    private StoreFront currentStoreFront = new StoreFront();
    private readonly IAsbl _bl;
    private Customer _customer = new Customer();

    public CustomerView(BL.IAsbl bl, Customer customer)
    {
        _bl = bl;
        _customer = customer;
    }

    public void ArtHome()
    {
        List<StoreFront> StoreFronts = _bl.GetAllStoreFronts();

        StoreFrontLocation:
        Console.WriteLine("Select a StoreFront to start shopping or View your order history"); 

        int i = 1;
        foreach (StoreFront StoreFront in StoreFronts)
        {
            Console.WriteLine($"[{i}] {StoreFront.Name} | {StoreFront.Address}");
            i++;
        }

        Console.WriteLine($"[{i}] View Order History");
        Console.WriteLine($"[x] Logout");

        string StoreFrontAnswer = Console.ReadLine().Trim();
    
        if (StoreFrontAnswer == "1")
        {
            currentStoreFront = StoreFronts[0];
        }
        else if (StoreFrontAnswer == "2")
        {
            currentStoreFront = StoreFronts[1];
        
        }
        else if(StoreFrontAnswer == "3")
        {
            ViewOrderHistory();
            goto StoreFrontLocation;
        }
        else if (StoreFrontAnswer.ToLower() == "x")
        {
            return;
        }
        else
        {
            Console.WriteLine("Invalid Input");
            goto StoreFrontLocation;
        }
        
        currentStoreFront = _bl.getStoreFrontInv(currentStoreFront);
        string result = Home();

        if (result == "7")
        {
            goto StoreFrontLocation;
        }
    }

    private string Home()
    {
        Options:
        Console.WriteLine("[1] Add product to ShopCart");
        Console.WriteLine("[2] View ShopCart");
        Console.WriteLine("[3] Checkout");
        Console.WriteLine("[4] Change StoreFront location.");
        Console.WriteLine("[x] Logout");

        string Option = Console.ReadLine().Trim().ToLower();

        switch (Option)
        {
            case "1":
                AddProduct();
                break;

            case "2":
                Checkout();

                break;                

            case "3":
                if (ShopCart.IsShopCartEmpty())
                {
                    Console.WriteLine("ShopCart is empty.");
                }  
                else
                {
                    bool CheckingOut = Checkout();

                    if (CheckingOut)
                    {
                        return "x";
                    }
                }
                break;

            case "9":
                if (ShopCart.IsShopCartEmpty())
                {
                    return Option;
                }
                else
                {
                    DifferentSF:
                    Console.WriteLine("Are you sure you want to change to a different location? All products that are in your ShopCart will be emptied. [Y/N]");
                    string Cresponse = Console.ReadLine().Trim().ToUpper();

                    if (Cresponse == "Y")
                    {
                        ShopCart.IsShopCartEmpty();
                        Console.WriteLine("Your ShopCart is now empty.");
                        
                        return Option;
                    }
                    else if (Cresponse == "N")
                    {
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Invalid Input");
                        goto DifferentSF;
                    }
                }

            case "x":
                return Option;

            default:
                Console.WriteLine("Invalid Input");
                break;
        }
        goto Options;
    }
    private void AddProduct()
    {
        ArtSupplyToAdd:
        StoreFrontInv();

        Console.WriteLine("Which Art Supply would you like to add:");
        string Option = Console.ReadLine().Trim();
        int productId;

        try
        {
            productId = Convert.ToInt32(Option);
        }
        catch (Exception)
        {
            Console.WriteLine($"Invalid Input");
            goto ArtSupplyToAdd;
        }
        
        if (productId > currentStoreFront.StoreFrontInv.Count || productId < 0)
        {
            Console.WriteLine("Invalid Input");
            goto ArtSupplyToAdd;
        }

        foreach (Product product in currentStoreFront.StoreFrontInv)
        {
            if (product.Id == productId)
            {
            QuantityArt:
                Console.WriteLine("How many would you like:");
                int qty;

                try
                {
                    qty = Convert.ToInt32(Console.ReadLine());
                }
                catch (Exception)
                {
                    Console.WriteLine($"Invalid Input");
                    goto QuantityArt;
                }
                if (qty > product.Quantity)
                {
                    Console.WriteLine("[The current StoreFront has sold out.");
                    goto QuantityArt;
                }

                Product ArtSupply = new Product();

                ArtSupply.Id = product.Id;
                ArtSupply.Name = product.Name;
                ArtSupply.Price = product.Price;
                ArtSupply.Details = product.Details;
                ArtSupply.Quantity = qty;

                for (int i = 0; i < currentStoreFront.StoreFrontInv.Count; i++)
                {
                    if (productId == currentStoreFront.StoreFrontInv[i].Id)
                    {
                        currentStoreFront.StoreFrontInv[i].Quantity -= qty;
                    }
                }

                ShopCart.AddArtSupply(ArtSupply);
            }
        }
    }

    private bool Checkout()
    {
        CheckoutOption:
        ShopCart.ShopCartContents();

        Console.WriteLine("Are you sure you wish to checkout? [Y/N]");
        string Option = Console.ReadLine().Trim().ToUpper();

        if (Option == "Y")
        {
            Console.WriteLine("Placing Order");
            Order order = new Order();
            order.customer = _customer;
            order.ShopCart = ShopCart;
            order.StoreFront = currentStoreFront;
            _bl.addToOrder(order);
            return true;
        }
        else if (Option == "N")
        {
            return false;
        }
        else
        {
            Console.WriteLine("Invalid Input");
            goto CheckoutOption;
        }
    }

    private void StoreFrontInv()
    {
        currentStoreFront.DisplayStoreFrontInv();
    }
        private void ViewOrderHistory()
    {
        List<OrderHistory> OrderHistoryC = _bl.GetOrderHistoryC(_customer);

        if (OrderHistoryC.Count == 0)
        {
            Console.WriteLine("Empty Order History.");
            return;
        }
    }
}
