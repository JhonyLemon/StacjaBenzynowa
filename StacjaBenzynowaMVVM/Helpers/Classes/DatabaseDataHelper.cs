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
            List<string> values = new List<string> { "ID_PRODUKTU", "NAZWA", "CENA", "DATA_DOSTAWY", "DATA_WAZNOSCI", "ILOSC", "RABAT" };
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>();
            List<Dictionary<string, string>> answer = DataBaseAccess.GetImportedData("SELECT * FROM DOSTEPNE_PRODUKTY", parameters, values);
            return DatabaseClassesHelper.GetProductsList(answer);
        }

        public static Client GetClient(string cardCode)
        {
            Client client=null;
            List<string> values = new List<string> { "ID_KLIENTA","IMIE","NAZWISKO","NIP" };
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>{ new KeyValuePair<string, string>("@id",cardCode)};
            List<Dictionary<string, string>> answer = DataBaseAccess.GetImportedData("SELECT * FROM KLIENCI WHERE ID_KLIENTA=@id", parameters, values);
            client= DatabaseClassesHelper.GetClientClass(answer);
            if(client!=null)
            {
                values = new List<string> { "PUNKTY" };
                parameters = new List<KeyValuePair<string, string>> { new KeyValuePair<string, string>("@id", cardCode) };
                answer = DataBaseAccess.GetImportedData("SELECT SUM(IFNULL(PUNKTY,0)-(IFNULL(RABAT,0)*100)) AS PUNKTY FROM ZAMOWIENIA WHERE ID_KLIENTA=@id", parameters, values);
                if(answer.Count!=0)
                client.Points = Convert.ToInt32(answer[0]["PUNKTY"]);
            }
            return client;
        }

        public static bool SetSale(Client client, BindingList<Product> products)
        {
            //zapis do bazy danych
            return true;
        }

        public static List<Dictionary<string, string>> GetLogData(string userName, string password)
        {
            List<string> values = new List<string> { "ID_PRACOWNIKA" };
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>> { new KeyValuePair<string, string>("@login", userName), new KeyValuePair<string, string>("@password", PasswordHashHelper.HashPassword(password)) };
            List<Dictionary<string, string>> answer = DataBaseAccess.GetImportedData("SELECT ID_PRACOWNIKA FROM PRACOWNICY WHERE LOGIN=@login AND HASLO=@password", parameters, values);
            return answer;
        }

    }

}
