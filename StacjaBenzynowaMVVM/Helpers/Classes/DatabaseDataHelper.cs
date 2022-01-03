using StacjaBenzynowaLibrary;
using StacjaBenzynowaMVVM.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StacjaBenzynowaMVVM.Helpers.Classes
{
    class DatabaseDataHelper
    {


        public static BindingList<Product> GetProducts()
        {
            return DatabaseClassesHelper.GetProductsList
                (
                    DataBaseAccess.GetImportedData
                    (
                        "SELECT * FROM DOSTEPNE_PRODUKTY",
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
                    new KeyValuePair<KeyValuePair<string, string>, string>(new KeyValuePair<string, string>("ID_KLIENTA",""),""),
                    new KeyValuePair<KeyValuePair<string, string>, string>(new KeyValuePair<string, string>("IMIE",""),""),
                    new KeyValuePair<KeyValuePair<string, string>, string>(new KeyValuePair<string, string>("NAZWISKO",""),""),
                    new KeyValuePair<KeyValuePair<string, string>, string>(new KeyValuePair<string, string>("NIP","@id"),cardCode)
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
                if(answer.Count!=0)
                {
                    if (answer[0]["PUNKTY"] == "")
                        client.Points = 0;
                    else
                        client.Points = Convert.ToInt32(answer[0]["PUNKTY"]);
                }
                
            }
            return client;
        }

        public static int SetSale(Client client, BindingList<Product> products, Employee employee, double price)
        {
            List<KeyValuePair<KeyValuePair<string, string>, string>> parameters = new List<KeyValuePair<KeyValuePair<string, string>, string>>
            {
                new KeyValuePair<KeyValuePair<string, string>, string>(new KeyValuePair<string, string>("ID_PRACOWNIKA","@id_pracownika"),employee.EmployeeID.ToString()),
                new KeyValuePair<KeyValuePair<string, string>, string>(new KeyValuePair<string, string>("DATA_WYDANIA","@data"),DateTime.Today.ToString())
            };

            if (client == null)
                parameters.Add(new KeyValuePair<KeyValuePair<string, string>, string>(new KeyValuePair<string, string>("ID_KLIENTA", "@id_klienta"), null));
            else
            {
                parameters.Add(new KeyValuePair<KeyValuePair<string, string>, string>(new KeyValuePair<string, string>("ID_KLIENTA", "@id_klienta"), client.ClientID.ToString()));
                if (client.Points >= 10 && price/0.9>=200)
                    parameters.Add(new KeyValuePair<KeyValuePair<string, string>, string>(new KeyValuePair<string, string>("RABAT", "@rabat"), (0.1).ToString()));
                parameters.Add(new KeyValuePair<KeyValuePair<string, string>, string>(new KeyValuePair<string, string>("PUNKTY", "@punkty"), (price / 100).ToString()));
            }
            KeyValuePair<string, string> parameter = InsertParametersToString(parameters);
            long answer = DataBaseAccess.SetData("INSERT INTO ZAMOWIENIA (" + parameter.Key + ") VALUES (" + parameter.Value + ")", parameters);

            foreach (Product p in products)
            {
                parameters.Clear();
                parameters.Add(new KeyValuePair<KeyValuePair<string, string>, string>(new KeyValuePair<string, string>("ID_ZAMOWIENIA", "@id_zamowienia"), answer.ToString()));
                parameters.Add(new KeyValuePair<KeyValuePair<string, string>, string>(new KeyValuePair<string, string>("ID_PRODUKTU", "@id_produktu"), p.ProductID.ToString()));
                parameters.Add(new KeyValuePair<KeyValuePair<string, string>, string>(new KeyValuePair<string, string>("ID_ZAMOWIENIA", "@id_zamowienia"), answer.ToString()));
                parameters.Add(new KeyValuePair<KeyValuePair<string, string>, string>(new KeyValuePair<string, string>("ILOSC", "@ilosc"), p.Amount.ToString()));
                parameters.Add(new KeyValuePair<KeyValuePair<string, string>, string>(new KeyValuePair<string, string>("CENA", "@cena"), p.Price.ToString()));
                parameters.Add(new KeyValuePair<KeyValuePair<string, string>, string>(new KeyValuePair<string, string>("RABAT", "@rabat"), p.Discount.ToString()));
                parameter = InsertParametersToString(parameters);
                DataBaseAccess.SetData("INSERT INTO OPISY_ZAMOWIEN (" + parameter.Key + ") VALUES (" + parameter.Value + ")", parameters);
            }

            return 0;
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
