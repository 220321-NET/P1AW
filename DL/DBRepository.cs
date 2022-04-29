using System.Data;
using Microsoft.Data.SqlClient;
using Models;

namespace DL;
public class DBRepository : IRepository
{
    private readonly string _connectionString;

    public DBRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    /// <summary>
    /// Creates Customer
    /// </summary>
    /// <param name="customerAdd"></param>
    /// <returns></returns>

    public Customer CreateCustomer(Customer customerAdd)
    {
        SqlConnection connection = new SqlConnection(_connectionString);
        connection.Open();

        SqlCommand cmd = new SqlCommand("INSERT INTO Customers(customerFname, customerCpass) OUTPUT INSERTED.Id VALUES(@cName, @Cpassword)", connection);

        cmd.Parameters.AddWithValue("@cName", customerAdd.cName);
        cmd.Parameters.AddWithValue("@Cpassword", customerAdd.Cpassword);

        try
        {
            customerAdd.Id = (int)cmd.ExecuteScalar();
        }
        catch (Exception z)
        {
            Console.WriteLine(z.Message);
        }

        connection.Close();

        return customerAdd;

    }
    
    public void CreateOrder(Order order)
    {
        DateTime currentDate = DateTime.Now;
        SqlConnection connection = new SqlConnection(_connectionString);
        connection.Open();

        SqlCommand cmd = new SqlCommand("INSERT INTO Orders(StoreFrontId, CustomerId, TotalPrice, DateOrdered) OUTPUT INSERTED.Id VALUES (@StoreFrontId, @customerId, @totalprice, @dateOrdered)", connection);

        cmd.Parameters.AddWithValue("@StoreFrontId", order.StoreFront.Id);
        cmd.Parameters.AddWithValue("@customerId", order.customer.Id);
        cmd.Parameters.AddWithValue("@totalprice", order.ShopCart.GetTotalPrice());
        cmd.Parameters.AddWithValue("@dateOrdered", currentDate);

        try
        {
            order.Id = (int)cmd.ExecuteScalar();
        }
        catch (Exception z)
        {
            Console.WriteLine(z.Message);
        }

        connection.Close();
        CreateShopCart(order);

    }

    public void CreateShopCart(Order order)
    {
        SqlConnection connection = new SqlConnection(_connectionString);
        connection.Open();
        SqlCommand cmd;

        foreach (Product product in order.ShopCart.AllProducts())
        {
            cmd = new SqlCommand("INSERT INTO ShopCart (ProductId, Quantity, OrderId) OUTPUT INSERTED.Id VALUES (@productId, @quantity, @orderId)", connection);
            cmd.Parameters.AddWithValue("@productId", product.Id);
            cmd.Parameters.AddWithValue("@quantity", product.Quantity);
            cmd.Parameters.AddWithValue("@orderId", order.Id);

            int id = (int)cmd.ExecuteScalar();
        }

        connection.Close();

        UpdateStoreFrontInv(order);

    }

    public List<Customer> GetAllCustomers()
    {
        
        List<Customer> Customers = new List<Customer>();

        SqlConnection connection = new SqlConnection(_connectionString);
        connection.Open();

        SqlCommand cmd = new SqlCommand("SELECT * FROM Customers", connection);
        SqlDataReader reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            int id = reader.GetInt32(0);
            string cName = reader.GetString(1);
            string Cpassword = reader.GetString(2);

            Customer Customer = new Customer
            {
                Id = id,
                cName = cName,
                Cpassword = Cpassword,
            };
            Customers.Add(Customer);
        }

        reader.Close();
        connection.Close();

        return Customers;
    }

    public List<StoreFront> GetAllStoreFronts()
    {
        List<StoreFront> StoreFronts = new List<StoreFront>();

        SqlConnection connection = new SqlConnection(_connectionString);
        connection.Open();

        SqlCommand cmd = new SqlCommand("SELECT * FROM StoreFront", connection);
        SqlDataReader reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            int id = reader.GetInt32(0);
            string name = reader.GetString(1);
            string address = reader.GetString(2);

            StoreFront StoreFront = new StoreFront
            {
                Id = id,
                Name = name,
                Address = address
            };

            StoreFronts.Add(StoreFront);
        }

        reader.Close();
        connection.Close();

        return StoreFronts;

    }

    public List<OrderHistory> GetOrderHistoryC(Customer Customer)
    {
        List<OrderHistory> OrderHistoryC = new List<OrderHistory>();
        List<StoreFront> StoreFronts = GetAllStoreFronts();

        SqlConnection connection = new SqlConnection(_connectionString);
        connection.Open();

        SqlCommand cmd = new SqlCommand("SELECT Orders.Id, StoreFrontId, CustomerId, DateOrdered, Quantity, Product.Name, Product.Price, TotalPrice FROM Orders JOIN ShopCart ON (ShopCart.OrderId = Orders.Id) JOIN StoreFront ON (StoreFront.Id = Orders.StoreFrontId) JOIN Product ON (Product.Id = Cart.ProductId) WHERE CustomerId = @customerId ORDER BY StoreFrontId;", connection);
        cmd.Parameters.AddWithValue("@customerId", Customer.Id);

        SqlDataReader reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            int id = reader.GetInt32(0);
            int StoreFrontId = reader.GetInt32(1);
            DateTime date = reader.GetDateTime(3);
            int ArtSupplyQty = reader.GetInt32(4);
            string ArtSupplyName = reader.GetString(5);
            double ArtSupplyPrice = reader.GetDouble(6);
            double totalPrice = reader.GetDouble(7);

            OrderHistory order = new OrderHistory
            {
                OrderId = id,
                ProductName = ArtSupplyName,
                StoreFront = StoreFrontId,
                TotalPrice = (decimal)totalPrice,
                ArtSupplyPrice = (decimal)ArtSupplyPrice,
                ArtSupplyQty = ArtSupplyQty,
                DateOrdered = date,
            };

            foreach (StoreFront StoreFront in StoreFronts)
            {
                if (StoreFront.Id == StoreFrontId)
                {
                    order.StoreFront = StoreFront;
                    break;
                }
            }

            OrderHistoryC.Add(order);
        }
        connection.Close();

        return OrderHistoryC;

    }

    public List<OrderHistory> GetOrderHistorySF(StoreFront _StoreFront)
    {
        List<OrderHistory> OrderHistorySF = new List<OrderHistory>();
        List<Customer> Customers = GetAllCustomers();

        SqlConnection connection = new SqlConnection(_connectionString);
        connection.Open();

        SqlCommand cmd = new SqlCommand("SELECT Orders.Id, CustomerId, DateOrdered, Quantity, Product.Name, TotalPrice, StoreFrontId, Price FROM Orders JOIN Cart ON (Cart.OrderId = Orders.Id) JOIN StoreFront ON (StoreFront.Id = Orders.StoreFrontId) JOIN Product ON (Product.Id = Cart.ProductId) WHERE StoreFrontId = @StoreFrontId ORDER BY StoreFrontId;", connection);
        cmd.Parameters.AddWithValue("@StoreFrontId", _StoreFront.Id);

        SqlDataReader reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            int id = reader.GetInt32(0);
            int customerId = reader.GetInt32(1);
            DateTime date = reader.GetDateTime(2);
            int ArtSupplyQty = reader.GetInt32(3);
            string ArtSupplyName = reader.GetString(4);
            double totalPrice = reader.GetDouble(5);
            int StoreFrontId = reader.GetInt32(6);
            double ArtSupplyPrice = reader.GetDouble(7);

            OrderHistory order = new OrderHistory
            {
                OrderId = id,
                ProductName = ArtSupplyName,
                StoreFront = StoreFrontId,
                TotalPrice = (decimal)totalPrice,
                ArtSupplyQty = ArtSupplyQty,
                DateOrdered = date,
                ArtSupplyPrice = (decimal)ArtSupplyPrice
            };

            foreach (Customer Customer in Customers)
            {
                if (customerId == Customer.Id)
                {
                    order.customer = Customer;
                    break;
                }
            }
            OrderHistorySF.Add(order);
        }

        reader.Close();
        connection.Close();

        return OrderHistorySF;

    }

    public StoreFront GetStoreFrontInv(StoreFront currentStoreFront)
    {
        List<Product> StoreFrontInv = new List<Product>();

        DataSet StoreFrontInvSet = new DataSet();

        SqlConnection connection = new SqlConnection(_connectionString);
        SqlCommand cmd = new SqlCommand("SELECT ProductId, StoreFrontId, Product.Name as ProductName, Details, Price, Quantity FROM StoreFrontInv JOIN Product ON (Product.Id = ProductId) JOIN StoreFront ON (StoreFront.Id = StoreFrontId) WHERE StoreFrontId = @id;", connection);
        cmd.Parameters.AddWithValue("@id", currentStoreFront.Id);

        SqlDataAdapter StoreFrontInvAdapter = new SqlDataAdapter(cmd);

        StoreFrontInvAdapter.Fill(StoreFrontInvSet, "StoreFrontInvTable");
        DataTable? StoreFrontInvTable = StoreFrontInvSet.Tables["StoreFrontInvTable"];
        if (StoreFrontInvTable != null && StoreFrontInvTable.Rows.Count > 0)
        {
            foreach (DataRow row in StoreFrontInvTable.Rows)
            {
                Product product = new Product();

                product.Id = (int)row["ProductId"];
                product.Name = (string)row["ProductName"];
                product.Details = (string)row["Details"];
                product.Price = (decimal)(double)row["Price"];
                product.Quantity = (int)row["Quantity"];

                StoreFrontInv.Add(product);
            }
        }
        currentStoreFront.StoreFrontInv = StoreFrontInv;

        return currentStoreFront;

    }

    public void UpdateStoreFrontInv(Order order)
    {
        SqlConnection connection = new SqlConnection(_connectionString);
        connection.Open();
        SqlCommand cmd;

        foreach (Product product in order.StoreFront.StoreFrontInv)
        {
            cmd = new SqlCommand("UPDATE StoreFrontInv SET Quantity = @quantity OUTPUT INSERTED.Id WHERE ProductId = @productId AND StoreFrontId = @StoreFront", connection);
            cmd.Parameters.AddWithValue("@quantity", product.Quantity);
            cmd.Parameters.AddWithValue("@productId", product.Id);
            cmd.Parameters.AddWithValue("@StoreFront", order.StoreFront.Id);

            int id = (int)cmd.ExecuteScalar();
        }

        connection.Close();
    }

    }

