using System.ComponentModel.DataAnnotations;
using Models;
    

    public class StoreFront 
    {
        public int Id{get; set;}
        public string Name{get; set;} = "";
        public string Address{get; set;} = "";
        public List<Product> StoreFrontInv = new List<Product>();

        public void DisplayStoreFrontInv()
        {
            int i = 1;
            foreach(Product product in StoreFrontInv)
            {
                Console.WriteLine($"{i} {product.Name} | {product.Price} | {product.Quantity} QTY.{product.Details}");
            }              
        }

    public static implicit operator int(StoreFront v)
    {
        throw new NotImplementedException();
    }
}

