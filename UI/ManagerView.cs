using BL;
using DL;
using Models;

namespace UI;

public class ManagerView
{
    private readonly IAsbl _bl;
    private readonly Manager _manager;
    private StoreFront currentStoreFront = null;

    public ManagerView(IAsbl bl, Manager manager)
    {
        _bl = bl;
        _manager = manager;
    }

    public void ManagerHome()
    {
        Console.WriteLine("Manager View");

        ManagerOptions:
        if (currentStoreFront != null)
        {
            Console.WriteLine($"Selected StoreFront:{currentStoreFront.Name} | {currentStoreFront.Address}");
        }

        Console.WriteLine("[1] Select a StoreFront");
        if (currentStoreFront != null)
        {
            Console.WriteLine("[2] View StoreFront Order History");
            Console.WriteLine("[3] Add an Art Supply");
        }
        Console.WriteLine("[x] Logout");

        string ManagerOption = Console.ReadLine().Trim().ToLower();

        switch (ManagerOption)
        {
            case "1":
                SelectStoreFront();
                break;

            case "2":
                OrderHistorySF();
                break;

            case "3":
                object value = ManagerView.AddProduct();
                break;

            case "x":
                return;

            default:
                Console.WriteLine("Invalid Input");
                goto ManagerOptions;
        }

        goto ManagerOptions;
    }

    private static object AddProduct()
    {
        throw new NotImplementedException();
    }

    private void SelectStoreFront()
    {
        List<StoreFront> allStoreFronts = _bl.GetAllStoreFronts();

        StoreFrontDecision:
        int i = 1;
        foreach (StoreFront StoreFront in allStoreFronts)
        {
            Console.WriteLine($"[{i}] {StoreFront.Name} | {StoreFront.Address}");
            i++;
        }

        Console.WriteLine("Select a StoreFront:");
        string StoreFrontSelection = Console.ReadLine().Trim();

        if (StoreFrontSelection == "1")
        {
            currentStoreFront = allStoreFronts[1];
        }
        else if (StoreFrontSelection == "2")
        {
            currentStoreFront = allStoreFronts[2];
        }
        else
        {
            Console.WriteLine("Invalid Input");
            goto StoreFrontDecision;
        }
    }

    private void OrderHistorySF()
    {
        List<OrderHistory> OrderHistorySF = _bl.GetOrderHistorySF(currentStoreFront);

        foreach (OrderHistory order in OrderHistorySF)
        {
            Console.WriteLine(value: $"{order.StoreFront} | {order.DateOrdered} |${order.ArtSupplyPrice} | {order.ProductName} | {order.ArtSupplyQty} Qty.");
        }
    }
}
