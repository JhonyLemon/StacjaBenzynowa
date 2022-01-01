using StacjaBenzynowaMVVM.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StacjaBenzynowaMVVM.Helpers
{
    public class ProductsListHelper
    {
        public static BindingList<Product> GetProductsList(List<Dictionary<string, string>> products)
        {
            BindingList<Product> Products = new BindingList<Product>();
            foreach(Dictionary<string,string> product in products)
            {
                Product Product = new Product();
                foreach(KeyValuePair<string,string> valuePair in product)
                {
                    if(valuePair.Value!="")
                    switch(valuePair.Key)
                    {
                        case "NAZWA":
                            {
                                Product.Name = valuePair.Value;
                                break;
                            }
                        case "CENA":
                            {
                                Product.Price = Convert.ToDouble(valuePair.Value);
                                break;
                            }
                        case "ID_PRODUKTU":
                            {
                                Product.ProductID = Convert.ToInt32(valuePair.Value);
                                break;
                            }
                        case "DATA_DOSTAWY":
                            {
                                Product.DeliveryDate = Convert.ToDateTime(valuePair.Value);
                                break;
                            }
                        case "DATA_WAZNOSCI":
                            {
                                Product.ExpirationDate = Convert.ToDateTime(valuePair.Value);
                                break;
                            }
                        case "ILOSC":
                            {
                                Product.Amount = Convert.ToInt32(valuePair.Value);
                                break;
                            }
                        case "ID_DOSTAWCY":
                            {
                                Product.SupplierID = Convert.ToInt32(valuePair.Value);
                                break;
                            }
                    }
                }
                Products.Add(Product);
            }
            return Products;
        }
    }
}
