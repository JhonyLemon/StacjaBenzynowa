using StacjaBenzynowaMVVM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StacjaBenzynowaMVVM.Helpers
{
    public class ProductsListHelper
    {
        public static List<Product> GetList(List<Dictionary<string, string>> products)
        {
            List<Product> Products = new List<Product>();
            foreach(Dictionary<string,string> product in products)
            {
                Product Product = new Product(); ;
                foreach(KeyValuePair<string,string> valuePair in product)
                {
                    Product.GetType().GetProperty(valuePair.Key).SetValue(Product, valuePair.Value);
                }
                Products.Add(Product);
            }
            return Products;
        }
    }
}
