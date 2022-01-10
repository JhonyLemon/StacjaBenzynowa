using StacjaBenzynowaLibrary;
using StacjaBenzynowaMVVM.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StacjaBenzynowaMVVM.Helpers.Classes
{
    class DatabaseDataHelper
    {


        public static ObservableCollection<Product> GetProducts()
        {
            return DatabaseClassesHelper.GetProductsList
                (
                    DataBaseAccess.GetImportedData
                    (
                        "SELECT * FROM DOSTEPNE_PRODUKTY WHERE ILOSC>0",
                        new List<KeyValuePair<KeyValuePair<string, string>, string>>
                        {
                            new KeyValuePair<KeyValuePair<string, string>, string>(new KeyValuePair<string, string>("ID_PRODUKTU",""),""),
                            new KeyValuePair<KeyValuePair<string, string>, string>(new KeyValuePair<string, string>("NAZWA",""),""),
                            new KeyValuePair<KeyValuePair<string, string>, string>(new KeyValuePair<string, string>("CENA",""),""),
                            new KeyValuePair<KeyValuePair<string, string>, string>(new KeyValuePair<string, string>("DATA_DOSTAWY",""),""),
                            new KeyValuePair<KeyValuePair<string, string>, string>(new KeyValuePair<string, string>("DATA_WAZNOSCI",""),""),
                            new KeyValuePair<KeyValuePair<string, string>, string>(new KeyValuePair<string, string>("ILOSC",""),""),
                            new KeyValuePair<KeyValuePair<string, string>, string>(new KeyValuePair<string, string>("RABAT",""),"")
                        }
                    )
                );
        }

        public static Client GetClient(string cardCode)
        {
            Client client=null;
            List<Dictionary<string, string>> answer = DataBaseAccess.GetImportedData("SELECT * FROM KLIENCI WHERE ID_KLIENTA=@id", 
                new List<KeyValuePair<KeyValuePair<string, string>, string>>
                {
                    new KeyValuePair<KeyValuePair<string, string>, string>(new KeyValuePair<string, string>("ID_KLIENTA","@id"),cardCode),
                    new KeyValuePair<KeyValuePair<string, string>, string>(new KeyValuePair<string, string>("IMIE",""),""),
                    new KeyValuePair<KeyValuePair<string, string>, string>(new KeyValuePair<string, string>("NAZWISKO",""),""),
                    new KeyValuePair<KeyValuePair<string, string>, string>(new KeyValuePair<string, string>("NIP",""),"")
                });
            client= DatabaseClassesHelper.GetClientClass(answer);
            if(client!=null)
            {
                answer = DataBaseAccess.GetImportedData("SELECT SUM(IFNULL(PUNKTY,0)-(IFNULL(RABAT,0)*100)) AS PUNKTY FROM ZAMOWIENIA WHERE ID_KLIENTA=@id",
                    new List<KeyValuePair<KeyValuePair<string, string>, string>>
                    {
                        new KeyValuePair<KeyValuePair<string, string>, string>(new KeyValuePair<string, string>("PUNKTY",""),""),
                        new KeyValuePair<KeyValuePair<string, string>, string>(new KeyValuePair<string, string>("ID_KLIENTA","@id"),cardCode)
                    });
                client.Points = DatabaseClassesHelper.GetClientPoints(answer);
            }
            return client;
        }

        public static int SetClient(string name, string surname, string nip)
        {
            List<KeyValuePair<KeyValuePair<string, string>, string>> parameters = new List<KeyValuePair<KeyValuePair<string, string>, string>>
            {
            new KeyValuePair<KeyValuePair<string, string>, string>(new KeyValuePair<string, string>("IMIE", "@imie"), name),
            new KeyValuePair<KeyValuePair<string, string>, string>(new KeyValuePair<string, string>("NAZWISKO", "@nazwisko"), surname),
            new KeyValuePair<KeyValuePair<string, string>, string>(new KeyValuePair<string, string>("NIP", "@nip"), nip)
            };
            KeyValuePair<string, string> parameter = InsertParametersToString(parameters);
            return DataBaseAccess.SetData("INSERT INTO KLIENCI (" + parameter.Key + ") VALUES (" + parameter.Value + ")", parameters);
        }

        public static int SetSupplier(string name)
        {
            List<KeyValuePair<KeyValuePair<string, string>, string>> parameters = new List<KeyValuePair<KeyValuePair<string, string>, string>>
            {
            new KeyValuePair<KeyValuePair<string, string>, string>(new KeyValuePair<string, string>("NAZWA_FIRMY", "@nazwa"), name),
            };
            KeyValuePair<string, string> parameter = InsertParametersToString(parameters);
            return DataBaseAccess.SetData("INSERT INTO DOSTAWCY (" + parameter.Key + ") VALUES (" + parameter.Value + ")", parameters);
        }

        public static int SetProducts(ObservableCollection<Product> products)
        {
            List<string> statements = new List<string>();
            List<List<KeyValuePair<KeyValuePair<string, string>, string>>> parameters = new List<List<KeyValuePair<KeyValuePair<string, string>, string>>>();
            List<KeyValuePair<KeyValuePair<string, string>, string>> parameter;
            string statement = "";
            KeyValuePair<string, string> param;
            foreach (Product p in products)
            {
                parameter = new List<KeyValuePair<KeyValuePair<string, string>, string>>();
                parameter.Add(new KeyValuePair<KeyValuePair<string, string>, string>(new KeyValuePair<string, string>("NAZWA", "@id_produktu"), p.Name));
                parameter.Add(new KeyValuePair<KeyValuePair<string, string>, string>(new KeyValuePair<string, string>("ILOSC", "@ilosc"), p.Amount.ToString()));
                parameter.Add(new KeyValuePair<KeyValuePair<string, string>, string>(new KeyValuePair<string, string>("CENA", "@cena"), p.Price.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture)));
                parameter.Add(new KeyValuePair<KeyValuePair<string, string>, string>(new KeyValuePair<string, string>("DATA_DOSTAWY", "@deliverDate"), p.DeliveryDate.ToString()));
                parameter.Add(new KeyValuePair<KeyValuePair<string, string>, string>(new KeyValuePair<string, string>("DATA_WAZNOSCI", "@expDate"), p.ExpirationDate.ToString()));
                parameter.Add(new KeyValuePair<KeyValuePair<string, string>, string>(new KeyValuePair<string, string>("ID_DOSTAWCY", "@id"), p.SupplierID.ToString()));
                param = InsertParametersToString(parameter);
                statement = "INSERT INTO PRODUKTY (" + param.Key + ") VALUES (" + param.Value + ")";
                statements.Add(statement);
                parameters.Add(parameter);
            }

            return DataBaseAccess.SetDataTransaction(statements, parameters);
        }

        public static int SetSale(Client client, ObservableCollection<Product> products, Employee employee, double price)
        {
            List<string> statements = new List<string>();
            List<List<KeyValuePair<KeyValuePair<string, string>, string>>> parameters = new List<List<KeyValuePair<KeyValuePair<string, string>, string>>>();
            List<KeyValuePair<KeyValuePair<string, string>, string>> parameter = new List<KeyValuePair<KeyValuePair<string, string>, string>>
            {
                new KeyValuePair<KeyValuePair<string, string>, string>(new KeyValuePair<string, string>("ID_PRACOWNIKA","@id_pracownika"),employee.EmployeeID.ToString()),
                new KeyValuePair<KeyValuePair<string, string>, string>(new KeyValuePair<string, string>("DATA_WYDANIA","@data"),DateTime.Today.ToString()),
                new KeyValuePair<KeyValuePair<string, string>, string>(new KeyValuePair<string, string>("ID_KLIENTA", "@id_klienta"), client.GetClientID()),
                new KeyValuePair<KeyValuePair<string, string>, string>(new KeyValuePair<string, string>("RABAT", "@rabat"), client.Discount.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture)),
                new KeyValuePair<KeyValuePair<string, string>, string>(new KeyValuePair<string, string>("PUNKTY", "@punkty"), (price / 100).ToString("0.00", System.Globalization.CultureInfo.InvariantCulture))
            };
            KeyValuePair<string, string> param = InsertParametersToString(parameter);
            string statement = "INSERT INTO ZAMOWIENIA (" + param.Key + ") VALUES (" + param.Value + ")";

            statements.Add(statement);
            parameters.Add(parameter);

            foreach (Product p in products)
            {
                parameter = new List<KeyValuePair<KeyValuePair<string, string>, string>>();
                parameter.Add(new KeyValuePair<KeyValuePair<string, string>, string>(new KeyValuePair<string, string>("ID_ZAMOWIENIA", "@FIRSTINSERTEDROW"), ""));
                parameter.Add(new KeyValuePair<KeyValuePair<string, string>, string>(new KeyValuePair<string, string>("ID_PRODUKTU", "@id_produktu"), p.ProductID.ToString()));
                parameter.Add(new KeyValuePair<KeyValuePair<string, string>, string>(new KeyValuePair<string, string>("ILOSC", "@ilosc"), p.Amount.ToString()));
                parameter.Add(new KeyValuePair<KeyValuePair<string, string>, string>(new KeyValuePair<string, string>("CENA", "@cena"), p.Price.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture)));
                param = InsertParametersToString(parameter);
                statement="INSERT INTO OPISY_ZAMOWIEN (" + param.Key + ") VALUES (" + param.Value + ")";
                statements.Add(statement);
                parameters.Add(parameter);
            }

            return DataBaseAccess.SetDataTransaction(statements, parameters);
        }

        public static int UpdateProducts(List<Product> products)
        {
            List<string> statements = new List<string>();
            List<List<KeyValuePair<KeyValuePair<string, string>, string>>> parameters = new List<List<KeyValuePair<KeyValuePair<string, string>, string>>>();
            List<KeyValuePair<KeyValuePair<string, string>, string>> parameter;
            string statement = "";
            foreach (Product p in products)
            {
                parameter = new List<KeyValuePair<KeyValuePair<string, string>, string>>();
                parameter.Add(new KeyValuePair<KeyValuePair<string, string>, string>(new KeyValuePair<string, string>("ID_PRODUKTU", "@id"), p.ProductID.ToString()));
                parameter.Add(new KeyValuePair<KeyValuePair<string, string>, string>(new KeyValuePair<string, string>("RABAT", "@discount"), p.Discount.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture)));
                statement = "UPDATE PRODUKTY SET RABAT=@discount WHERE ID_PRODUKTU=@id";
                statements.Add(statement);
                parameters.Add(parameter);
            }

            return DataBaseAccess.SetDataTransaction(statements, parameters);
        }

        public static int SetExpiredProducts(List<Product> products)
        {
            List<string> statements = new List<string>();
            List<List<KeyValuePair<KeyValuePair<string, string>, string>>> parameters = new List<List<KeyValuePair<KeyValuePair<string, string>, string>>>();
            List<KeyValuePair<KeyValuePair<string, string>, string>> parameter;
            string statement = "";
            KeyValuePair<string, string> param;
            foreach (Product p in products)
            {
                parameter = new List<KeyValuePair<KeyValuePair<string, string>, string>>();
                parameter.Add(new KeyValuePair<KeyValuePair<string, string>, string>(new KeyValuePair<string, string>("ID_PRODUKTU", "@id_produktu"), p.ProductID.ToString()));
                parameter.Add(new KeyValuePair<KeyValuePair<string, string>, string>(new KeyValuePair<string, string>("ILOSC", "@ilosc"), p.Amount.ToString()));
                param = InsertParametersToString(parameter);
                statement = "INSERT INTO PRODUKTY_PO_TERMINIE (" + param.Key + ") VALUES (" + param.Value + ")";
                statements.Add(statement);
                parameters.Add(parameter);
            }

            return DataBaseAccess.SetDataTransaction(statements, parameters);
        }

        public static Employee GetEmployee(string userName, string password)
        {
            return DatabaseClassesHelper.GetEmployee
                (
                    DataBaseAccess.GetImportedData
                    (
                        "SELECT ID_PRACOWNIKA,IMIE,NAZWISKO,POZYCJA FROM PRACOWNICY WHERE LOGIN=@login AND HASLO=@password",
                        new List<KeyValuePair<KeyValuePair<string, string>, string>>
                        {
                            new KeyValuePair<KeyValuePair<string, string>, string>(new KeyValuePair<string, string>("ID_PRACOWNIKA",""),""),
                            new KeyValuePair<KeyValuePair<string, string>, string>(new KeyValuePair<string, string>("IMIE",""),""),
                            new KeyValuePair<KeyValuePair<string, string>, string>(new KeyValuePair<string, string>("NAZWISKO",""),""),
                            new KeyValuePair<KeyValuePair<string, string>, string>(new KeyValuePair<string, string>("POZYCJA",""),""),
                            new KeyValuePair<KeyValuePair<string, string>, string>(new KeyValuePair<string, string>("LOGIN","@login"),userName),
                            new KeyValuePair<KeyValuePair<string, string>, string>(new KeyValuePair<string, string>("HASLO","@password"),PasswordHashHelper.HashPassword(password))
                        }
                    )
               );
        }

        public static ObservableCollection<Supplier> GetSuppliers()
        {
            return DatabaseClassesHelper.GetSuppliers
                (
                    DataBaseAccess.GetImportedData
                    (
                        "SELECT * FROM DOSTAWCY",
                        new List<KeyValuePair<KeyValuePair<string, string>, string>>
                        {
                            new KeyValuePair<KeyValuePair<string, string>, string>(new KeyValuePair<string, string>("ID_DOSTAWCY",""),""),
                            new KeyValuePair<KeyValuePair<string, string>, string>(new KeyValuePair<string, string>("NAZWA_FIRMY",""),"")
                        }
                    )
               );
        }

        public static KeyValuePair<string,string> InsertParametersToString(List<KeyValuePair<KeyValuePair<string, string>, string>> parameters)
        {
            string key="";
            string value="";

            foreach(KeyValuePair<KeyValuePair<string, string>, string> pair in parameters)
            {
                key += pair.Key.Key+",";
                value += pair.Key.Value+",";
            }
            key = key.Remove(key.Length - 1, 1);
            value = value.Remove(value.Length - 1, 1);
            return new KeyValuePair<string, string>(key,value);
        }

    }

}
