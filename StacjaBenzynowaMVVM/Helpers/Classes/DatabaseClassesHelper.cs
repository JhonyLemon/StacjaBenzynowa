using StacjaBenzynowaMVVM.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StacjaBenzynowaMVVM.Helpers
{
    public class DatabaseClassesHelper
    {
        public static Client GetClientClass(List<Dictionary<string, string>> client)
        {
            Client Client = new Client();
            if (client.Count == 1)
            {
                foreach (Dictionary<string, string> c in client)
                {
                    foreach (KeyValuePair<string, string> valuePair in c)
                    {
                        if (valuePair.Value != "")
                            switch (valuePair.Key)
                            {
                                case "ID_KLIENTA":
                                    {
                                        Client.ClientID = Convert.ToInt32(valuePair.Value);
                                        break;
                                    }
                                case "IMIE":
                                    {
                                        Client.FirstName = valuePair.Value;
                                        break;
                                    }
                                case "NAZWISKO":
                                    {
                                        Client.SurName = valuePair.Value;
                                        break;
                                    }
                                case "NIP":
                                    {
                                        Client.NIP = valuePair.Value;
                                        break;
                                    }
                            }
                    }
                }
                return Client;
            }
            return null;
        }

        public static ObservableCollection<Product> GetProductsList(List<Dictionary<string, string>> products)
        {
            ObservableCollection<Product> Products = new ObservableCollection<Product>();
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
                        case "RABAT":
                            {
                                Product.Discount = Convert.ToDouble(valuePair.Value);
                                break;
                            }
                    }
                }
                Products.Add(Product);
            }
            return Products;
        }

        public static Employee GetEmployee(List<Dictionary<string, string>> data)
        {
            Employee employee = new Employee();
            foreach (Dictionary<string, string> d in data)
            {
                foreach (KeyValuePair<string, string> valuePair in d)
                {
                    if (valuePair.Value != "")
                        switch (valuePair.Key)
                        {
                            case "NAZWISKO":
                                {
                                    employee.SurName = valuePair.Value;
                                    break;
                                }
                            case "IMIE":
                                {
                                    employee.FirstName = valuePair.Value;
                                    break;
                                }
                            case "ID_PRACOWNIKA":
                                {
                                    employee.EmployeeID=Convert.ToInt32(valuePair.Value);
                                    break;
                                }
                            case "POZYCJA":
                                {
                                    employee.Position = valuePair.Value;
                                    break;
                                }
                        }
                }
            }
            if (employee.EmployeeID==0)
                return null;
            else
                return employee;
        }

        public static int GetClientPoints(List<Dictionary<string, string>> data)
        {
            int points = 0;
            if (data.Count != 0)
            {
                if (data[0]["PUNKTY"] != "")
                    points = Convert.ToInt32(data[0]["PUNKTY"]);
            }
            return points;
        }

        public static ObservableCollection<Supplier> GetSuppliers(List<Dictionary<string, string>> suppliers)
        {
            ObservableCollection<Supplier> Suppliers = new ObservableCollection<Supplier>();
            foreach (Dictionary<string, string> supplier in suppliers)
            {
                Supplier Supplier = new Supplier();
                foreach (KeyValuePair<string, string> valuePair in supplier)
                {
                    if (valuePair.Value != "")
                        switch (valuePair.Key)
                        {
                            case "NAZWA_FIRMY":
                                {
                                    Supplier.SupplierName = valuePair.Value;
                                    break;
                                }
                            case "ID_DOSTAWCY":
                                {
                                    Supplier.SupplierID = Convert.ToInt32(valuePair.Value);
                                    break;
                                }
                        }
                }
                Suppliers.Add(Supplier);
            }
            return Suppliers;
        }

    }
}
