using DL;

namespace BL;

public class Asbl : IAsbl
{
    private readonly IRepository _repo;

    public Asbl(IRepository repo)
    {
        _repo = repo;
    }

    public List<Customer> GetAllCustomers()
    {
        return _repo.GetAllCustomers();
    }

    public Customer AddCustomer(Customer customerAdd)
    {
        return _repo.CreateCustomer(customerAdd);
    }

    public void AddOrder(Order order)
    {
        _repo.CreateOrder(order);
    }

    public List<StoreFront> GetAllStoreFronts()
    {
        return _repo.GetAllStoreFronts();
    }
    
    public StoreFront GetStoreFrontInv(StoreFront currentStoreFront)
    {
        return _repo.GetStoreFrontInv(currentStoreFront);
    }

    public List<OrderHistory> GetOrderHistorySF(StoreFront _StoreFront)
    {
        List<OrderHistory> OrderHistorySF = _repo.GetOrderHistorySF(_StoreFront);
        return OrderHistorySF;
    }

    public List<OrderHistory> GetOrderHistoryC(Customer customer)
    {
        List<OrderHistory> OrderHistoryC = _repo.GetOrderHistoryC(customer);
        return OrderHistoryC;
    }

    public Customer addCustomer(Customer customerAdd)
    {
        return _repo.CreateCustomer(customerAdd);
    }

    public void addToOrder(Order order)
    {
        _repo.CreateOrder(order);
    }

    public StoreFront getStoreFrontInv(StoreFront currentStoreFront)
    {
        throw new NotImplementedException();
    }
}

