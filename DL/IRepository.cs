using Models;
namespace DL;

public interface IRepository
{
    List<Customer> GetAllCustomers();
    Customer CreateCustomer(Customer customerAdd);
    void CreateOrder(Order Order);
    List<StoreFront> GetAllStoreFronts();
    StoreFront GetStoreFrontInv(StoreFront currentStoreFront);
    List<OrderHistory> GetOrderHistorySF(StoreFront _StoreFront);
    List<OrderHistory> GetOrderHistoryC(Customer Customer);
}

