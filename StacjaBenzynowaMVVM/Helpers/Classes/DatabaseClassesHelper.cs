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
        public static Client GetClientClass(List<Dictionary<string, object>> client)
        {
            ObservableCollection<BaseModel> models = GetModel(client, typeof(Client));

            return (Client)models[0];
            //Client Client = new Client();
            //if (client.Count == 1)
            //{
            //    foreach (Dictionary<string, object> c in client)
            //    {
            //        foreach (KeyValuePair<string, object> valuePair in c)
            //        {
            //            if ((string)valuePair.Value != "")
            //                switch (valuePair.Key)
            //                {
            //                    case "ID_KLIENTA":
            //                        {
            //                            Client.ID_KLIENTA =(int)valuePair.Value;
            //                            break;
            //                        }
            //                    case "IMIE":
            //                        {
            //                            Client.IMIE = (string)valuePair.Value;
            //                            break;
            //                        }
            //                    case "NAZWISKO":
            //                        {
            //                            Client.NAZWISKO = (string)valuePair.Value;
            //                            break;
            //                        }
            //                    case "NIP":
            //                        {
            //                            Client.NIP = (string)valuePair.Value;
            //                            break;
            //                        }
            //                }
            //        }
            //    }
            //    return Client;
            //}
            //return null;
        }

        public static ObservableCollection<Product> GetProductsList(List<Dictionary<string, object>> products)
        {
            ObservableCollection<BaseModel> models = GetModel(products, typeof(Product));
            ObservableCollection<Product> Products = new ObservableCollection<Product>();
            foreach (Product p in models)
            {
                Products.Add(p);
            }
            return Products;
            //ObservableCollection<Product> Products = new ObservableCollection<Product>();
            //foreach(Dictionary<string, object> product in products)
            //{
            //    Product Product = new Product();
            //    foreach(KeyValuePair<string, object> valuePair in product)
            //    {
            //        if((string)valuePair.Value!="")
            //        switch(valuePair.Key)
            //        {
            //            case "NAZWA":
            //                {
            //                    Product.NAZWA = (string)valuePair.Value;
            //                    break;
            //                }
            //            case "CENA":
            //                {
            //                    Product.CENA = Convert.ToDouble(valuePair.Value);
            //                    break;
            //                }
            //            case "ID_PRODUKTU":
            //                {
            //                    Product.ID_PRODUKTU = Convert.ToInt32(valuePair.Value);
            //                    break;
            //                }
            //            case "DATA_DOSTAWY":
            //                {
            //                    Product.DATA_DOSTAWY = Convert.ToDateTime(valuePair.Value);
            //                    break;
            //                }
            //            case "DATA_WAZNOSCI":
            //                {
            //                    Product.DATA_WAZNOSCI = Convert.ToDateTime(valuePair.Value);
            //                    break;
            //                }
            //            case "ILOSC":
            //                {
            //                    Product.ILOSC = Convert.ToInt32(valuePair.Value);
            //                    break;
            //                }
            //            case "ID_DOSTAWCY":
            //                {
            //                    Product.ID_DOSTAWCY = Convert.ToInt32(valuePair.Value);
            //                    break;
            //                }
            //            case "RABAT":
            //                {
            //                    Product.RABAT = Convert.ToDouble(valuePair.Value);
            //                    break;
            //                }
            //        }
            //    }
            //    Products.Add(Product);
            //}
            //return Products;
        }

        public static Employee GetEmployee(List<Dictionary<string, object>> data)
        {
            ObservableCollection<BaseModel> models = GetModel(data, typeof(Employee));
            return (Employee)models[0];

            //Employee employee = new Employee();
            //foreach (Dictionary<string, object> d in data)
            //{
            //    foreach (KeyValuePair<string, object> valuePair in d)
            //    {
            //        if ((string)valuePair.Value != "")
            //            switch (valuePair.Key)
            //            {
            //                case "NAZWISKO":
            //                    {
            //                        employee.NAZWISKO = (string)valuePair.Value;
            //                        break;
            //                    }
            //                case "IMIE":
            //                    {
            //                        employee.IMIE = (string)valuePair.Value;
            //                        break;
            //                    }
            //                case "ID_PRACOWNIKA":
            //                    {
            //                        employee.ID_PRACOWNIKA=Convert.ToInt32(valuePair.Value);
            //                        break;
            //                    }
            //                case "POZYCJA":
            //                    {
            //                        employee.POZYCJA = (string)valuePair.Value;
            //                        break;
            //                    }
            //            }
            //    }
            //}
            //if (employee.ID_PRACOWNIKA==0)
            //    return null;
            //else
            //    return employee;
        }

        public static int GetClientPoints(List<Dictionary<string, object>> data)
        {

            int points = 0;
            if (data.Count != 0)
            {
                if ((string)data[0]["PUNKTY"] != "")
                    points = Convert.ToInt32(data[0]["PUNKTY"]);
            }
            return points;
        }

        public static ObservableCollection<Supplier> GetSuppliers(List<Dictionary<string, object>> suppliers)
        {
            ObservableCollection<BaseModel> models = GetModel(suppliers, typeof(Supplier));
            ObservableCollection<Supplier> Suppliers = new ObservableCollection<Supplier>();
            foreach(Supplier s in models)
            {
                Suppliers.Add(s);
            }
            return Suppliers;
            //ObservableCollection<Supplier> Suppliers = new ObservableCollection<Supplier>();
            //foreach (Dictionary<string, object> supplier in suppliers)
            //{
            //    Supplier Supplier = new Supplier();
            //    foreach (KeyValuePair<string, object> valuePair in supplier)
            //    {
            //        if ((string)valuePair.Value != "")
            //            switch (valuePair.Key)
            //            {
            //                case "NAZWA_FIRMY":
            //                    {
            //                        Supplier.NAZWA_FIRMY = (string)valuePair.Value;
            //                        break;
            //                    }
            //                case "ID_DOSTAWCY":
            //                    {
            //                        Supplier.ID_DOSTAWCY = Convert.ToInt32(valuePair.Value);
            //                        break;
            //                    }
            //            }
            //    }
            //    Suppliers.Add(Supplier);
            //}
            //return Suppliers;
        }

        public static ObservableCollection<BaseModel> GetModel(List<Dictionary<string, object>> data, Type type)
        {

            ObservableCollection<BaseModel> models = new ObservableCollection<BaseModel>();
            foreach (Dictionary<string, object> d in data)
            {
                BaseModel model = (BaseModel)Activator.CreateInstance(type);
                foreach (KeyValuePair<string, object> valuePair in d)
                {
                    if((string)valuePair.Value!="")
                    model.GetType().GetProperty(valuePair.Key).SetValue(model, Convert.ChangeType(valuePair.Value, model.GetType().GetProperty(valuePair.Key).PropertyType));
                }
                models.Add(model);
            }
            return models;
        }
    }
}
