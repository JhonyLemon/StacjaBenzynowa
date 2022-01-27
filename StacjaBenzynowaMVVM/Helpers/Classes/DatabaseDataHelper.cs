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
            return DatabaseClassesHelper.GetModels<Product>
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

        public static ObservableCollection<Employee> GetEmployees()
        {
            return DatabaseClassesHelper.GetModels<Employee>
                (
                    DataBaseAccess.GetImportedData
                    (
                        "SELECT ID_PRACOWNIKA,IMIE,NAZWISKO,Z.STANOWISKO AS POZYCJA,ZATRUDNIONY FROM PRACOWNICY LEFT OUTER JOIN STANOWISKA Z ON PRACOWNICY.ID_STANOWISKA=Z.ID_STANOWISKA WHERE ZATRUDNIONY==1",
                        new List<KeyValuePair<KeyValuePair<string, string>, string>>
                        {
                            new KeyValuePair<KeyValuePair<string, string>, string>(new KeyValuePair<string, string>("ID_PRACOWNIKA",""),""),
                            new KeyValuePair<KeyValuePair<string, string>, string>(new KeyValuePair<string, string>("IMIE",""),""),
                            new KeyValuePair<KeyValuePair<string, string>, string>(new KeyValuePair<string, string>("NAZWISKO",""),""),
                            new KeyValuePair<KeyValuePair<string, string>, string>(new KeyValuePair<string, string>("POZYCJA",""),""),
                            new KeyValuePair<KeyValuePair<string, string>, string>(new KeyValuePair<string, string>("ZATRUDNIONY",""),""),
                        }
                    )
                );
        }

        public static Client GetClient(string cardCode)
        {
            Client client=null;
            List<Dictionary<string, object>> answer = DataBaseAccess.GetImportedData("SELECT * FROM KLIENCI WHERE ID_KLIENTA=@id", 
                new List<KeyValuePair<KeyValuePair<string, string>, string>>
                {
                    new KeyValuePair<KeyValuePair<string, string>, string>(new KeyValuePair<string, string>("ID_KLIENTA","@id"),cardCode),
                    new KeyValuePair<KeyValuePair<string, string>, string>(new KeyValuePair<string, string>("IMIE",""),""),
                    new KeyValuePair<KeyValuePair<string, string>, string>(new KeyValuePair<string, string>("NAZWISKO",""),""),
                    new KeyValuePair<KeyValuePair<string, string>, string>(new KeyValuePair<string, string>("NIP",""),"")
                });
            client= DatabaseClassesHelper.GetModel<Client>(answer);
            if(client!=null)
            {
                answer = DataBaseAccess.GetImportedData("SELECT SUM(IFNULL(PUNKTY,0)-(IFNULL(RABAT,0)*100)) AS PUNKTY FROM ZAMOWIENIA WHERE ID_KLIENTA=@id",
                    new List<KeyValuePair<KeyValuePair<string, string>, string>>
                    {
                        new KeyValuePair<KeyValuePair<string, string>, string>(new KeyValuePair<string, string>("PUNKTY",""),""),
                        new KeyValuePair<KeyValuePair<string, string>, string>(new KeyValuePair<string, string>("ID_KLIENTA","@id"),cardCode)
                    });
                client.PUNKTY = DatabaseClassesHelper.GetClientPoints(answer);
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

        public static int SetEmployee(string name, string surname, int position, int empoyed, string login, string password)
        {
            List<KeyValuePair<KeyValuePair<string, string>, string>> parameters = new List<KeyValuePair<KeyValuePair<string, string>, string>>
            {
                new KeyValuePair<KeyValuePair<string, string>, string>(new KeyValuePair<string, string>("IMIE", "@imie"), name),
                new KeyValuePair<KeyValuePair<string, string>, string>(new KeyValuePair<string, string>("NAZWISKO", "@nazwisko"), surname),
                new KeyValuePair<KeyValuePair<string, string>, string>(new KeyValuePair<string, string>("ID_STANOWISKA", "@pozycja"), position.ToString()),
                new KeyValuePair<KeyValuePair<string, string>, string>(new KeyValuePair<string, string>("ZATRUDNIONY", "@zatrudniony"), empoyed.ToString()),
                new KeyValuePair<KeyValuePair<string, string>, string>(new KeyValuePair<string, string>("LOGIN", "@login"), login),
                new KeyValuePair<KeyValuePair<string, string>, string>(new KeyValuePair<string, string>("HASLO", "@haslo"), password),
            };
            KeyValuePair<string, string> parameter = InsertParametersToString(parameters);
            return DataBaseAccess.SetData("INSERT INTO PRACOWNICY (" + parameter.Key + ") VALUES (" + parameter.Value + ")", parameters);
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
                parameter.Add(new KeyValuePair<KeyValuePair<string, string>, string>(new KeyValuePair<string, string>("NAZWA", "@id_produktu"), p.NAZWA));
                parameter.Add(new KeyValuePair<KeyValuePair<string, string>, string>(new KeyValuePair<string, string>("ILOSC", "@ilosc"), p.ILOSC.ToString()));
                parameter.Add(new KeyValuePair<KeyValuePair<string, string>, string>(new KeyValuePair<string, string>("CENA", "@cena"), p.CENA.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture)));
                parameter.Add(new KeyValuePair<KeyValuePair<string, string>, string>(new KeyValuePair<string, string>("DATA_DOSTAWY", "@deliverDate"), p.DATA_DOSTAWY.ToString()));
                if(p.DATA_WAZNOSCI.Equals(new DateTime()))
                    parameter.Add(new KeyValuePair<KeyValuePair<string, string>, string>(new KeyValuePair<string, string>("DATA_WAZNOSCI", "@expDate"), ""));
                else
                    parameter.Add(new KeyValuePair<KeyValuePair<string, string>, string>(new KeyValuePair<string, string>("DATA_WAZNOSCI", "@expDate"), p.DATA_WAZNOSCI.ToString()));
                parameter.Add(new KeyValuePair<KeyValuePair<string, string>, string>(new KeyValuePair<string, string>("ID_DOSTAWCY", "@id"), p.ID_DOSTAWCY.ToString()));
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
                new KeyValuePair<KeyValuePair<string, string>, string>(new KeyValuePair<string, string>("ID_PRACOWNIKA","@id_pracownika"),employee.ID_PRACOWNIKA.ToString()),
                new KeyValuePair<KeyValuePair<string, string>, string>(new KeyValuePair<string, string>("DATA_WYDANIA","@data"),DateTime.Today.ToString()),
                new KeyValuePair<KeyValuePair<string, string>, string>(new KeyValuePair<string, string>("ID_KLIENTA", "@id_klienta"), client.GetClientID()),
                new KeyValuePair<KeyValuePair<string, string>, string>(new KeyValuePair<string, string>("RABAT", "@rabat"), client.RABAT.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture)),
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
                parameter.Add(new KeyValuePair<KeyValuePair<string, string>, string>(new KeyValuePair<string, string>("ID_PRODUKTU", "@id_produktu"), p.ID_PRODUKTU.ToString()));
                parameter.Add(new KeyValuePair<KeyValuePair<string, string>, string>(new KeyValuePair<string, string>("ILOSC", "@ilosc"), p.ILOSC.ToString()));
                parameter.Add(new KeyValuePair<KeyValuePair<string, string>, string>(new KeyValuePair<string, string>("CENA", "@cena"), p.CENA.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture)));
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
                parameter.Add(new KeyValuePair<KeyValuePair<string, string>, string>(new KeyValuePair<string, string>("ID_PRODUKTU", "@id"), p.ID_PRODUKTU.ToString()));
                parameter.Add(new KeyValuePair<KeyValuePair<string, string>, string>(new KeyValuePair<string, string>("RABAT", "@discount"), p.RABAT.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture)));
                statement = "UPDATE PRODUKTY SET RABAT=@discount WHERE ID_PRODUKTU=@id";
                statements.Add(statement);
                parameters.Add(parameter);
            }

            return DataBaseAccess.SetDataTransaction(statements, parameters);
        }

        public static int UpdateEmployee(Employee employee)
        {
            List<KeyValuePair<KeyValuePair<string, string>, string>> parameters = new List<KeyValuePair<KeyValuePair<string, string>, string>>
            {
                new KeyValuePair<KeyValuePair<string, string>, string>(new KeyValuePair<string, string>("IMIE", "@imie"), employee.IMIE),
                new KeyValuePair<KeyValuePair<string, string>, string>(new KeyValuePair<string, string>("NAZWISKO", "@nazwisko"), employee.NAZWISKO),
                new KeyValuePair<KeyValuePair<string, string>, string>(new KeyValuePair<string, string>("ID_STANOWISKA", "@pozycja"), employee.POZYCJA),
                new KeyValuePair<KeyValuePair<string, string>, string>(new KeyValuePair<string, string>("ZATRUDNIONY", "@zatrudniony"), employee.ZATRUDNIONY.ToString()),
            };
            string parameter = UpdateParametersToString(parameters);
            parameters.Add(new KeyValuePair<KeyValuePair<string, string>, string>(new KeyValuePair<string, string>("ID_PRACOWNIKA", "@id"), employee.ID_PRACOWNIKA.ToString()));
            return DataBaseAccess.SetData("UPDATE PRACOWNICY SET "+parameter+ " WHERE ID_PRACOWNIKA=@id", parameters);
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
                parameter.Add(new KeyValuePair<KeyValuePair<string, string>, string>(new KeyValuePair<string, string>("ID_PRODUKTU", "@id_produktu"), p.ID_PRODUKTU.ToString()));
                parameter.Add(new KeyValuePair<KeyValuePair<string, string>, string>(new KeyValuePair<string, string>("ILOSC", "@ilosc"), p.ILOSC.ToString()));
                param = InsertParametersToString(parameter);
                statement = "INSERT INTO PRODUKTY_PO_TERMINIE (" + param.Key + ") VALUES (" + param.Value + ")";
                statements.Add(statement);
                parameters.Add(parameter);
            }

            return DataBaseAccess.SetDataTransaction(statements, parameters);
        }

        public static Employee GetEmployee(string userName, string password)
        {
            return DatabaseClassesHelper.GetModel<Employee>
                (
                    DataBaseAccess.GetImportedData
                    (
                        "SELECT ID_PRACOWNIKA,IMIE,NAZWISKO,S.STANOWISKO AS POZYCJA,ZATRUDNIONY FROM PRACOWNICY P LEFT OUTER JOIN STANOWISKA S ON P.ID_STANOWISKA=S.ID_STANOWISKA WHERE LOGIN=@login AND HASLO=@password",
                        new List<KeyValuePair<KeyValuePair<string, string>, string>>
                        {
                            new KeyValuePair<KeyValuePair<string, string>, string>(new KeyValuePair<string, string>("ID_PRACOWNIKA",""),""),
                            new KeyValuePair<KeyValuePair<string, string>, string>(new KeyValuePair<string, string>("IMIE",""),""),
                            new KeyValuePair<KeyValuePair<string, string>, string>(new KeyValuePair<string, string>("NAZWISKO",""),""),
                            new KeyValuePair<KeyValuePair<string, string>, string>(new KeyValuePair<string, string>("POZYCJA",""),""),
                            new KeyValuePair<KeyValuePair<string, string>, string>(new KeyValuePair<string, string>("ZATRUDNIONY",""),""),
                            new KeyValuePair<KeyValuePair<string, string>, string>(new KeyValuePair<string, string>("LOGIN","@login"),userName),
                            new KeyValuePair<KeyValuePair<string, string>, string>(new KeyValuePair<string, string>("HASLO","@password"),PasswordHashHelper.HashPassword(password))
                        }
                    )
               );
        }

        public static Employee GetEmployeeLogin(string userName)
        {
            return DatabaseClassesHelper.GetModel<Employee>
                (
                    DataBaseAccess.GetImportedData
                    (
                        "SELECT ID_PRACOWNIKA,IMIE,NAZWISKO,S.STANOWISKO AS POZYCJA,ZATRUDNIONY FROM PRACOWNICY P LEFT OUTER JOIN STANOWISKA S ON P.ID_STANOWISKA=S.ID_STANOWISKA WHERE LOGIN=@login",
                        new List<KeyValuePair<KeyValuePair<string, string>, string>>
                        {
                            new KeyValuePair<KeyValuePair<string, string>, string>(new KeyValuePair<string, string>("ID_PRACOWNIKA",""),""),
                            new KeyValuePair<KeyValuePair<string, string>, string>(new KeyValuePair<string, string>("IMIE",""),""),
                            new KeyValuePair<KeyValuePair<string, string>, string>(new KeyValuePair<string, string>("NAZWISKO",""),""),
                            new KeyValuePair<KeyValuePair<string, string>, string>(new KeyValuePair<string, string>("POZYCJA",""),""),
                            new KeyValuePair<KeyValuePair<string, string>, string>(new KeyValuePair<string, string>("ZATRUDNIONY",""),""),
                            new KeyValuePair<KeyValuePair<string, string>, string>(new KeyValuePair<string, string>("LOGIN","@login"),userName),
                            new KeyValuePair<KeyValuePair<string, string>, string>(new KeyValuePair<string, string>("HASLO","@password"), "")
                        }
                    )
               );
        }


        public static ObservableCollection<Supplier> GetSuppliers()
        {
            return DatabaseClassesHelper.GetModels<Supplier>
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

        public static ObservableCollection<Position> GetPositions()
        {
            return DatabaseClassesHelper.GetModels<Position>
                (
                    DataBaseAccess.GetImportedData
                    (
                        "SELECT * FROM STANOWISKA",
                        new List<KeyValuePair<KeyValuePair<string, string>, string>>
                        {
                            new KeyValuePair<KeyValuePair<string, string>, string>(new KeyValuePair<string, string>("ID_STANOWISKA",""),""),
                            new KeyValuePair<KeyValuePair<string, string>, string>(new KeyValuePair<string, string>("STANOWISKO",""),"")
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

        public static string UpdateParametersToString(List<KeyValuePair<KeyValuePair<string, string>, string>> parameters)
        {
            string s = "";

            foreach (KeyValuePair<KeyValuePair<string, string>, string> pair in parameters)
            {
                s += pair.Key.Key+"="+pair.Key.Value + ",";
            }
            s = s.Remove(s.Length - 1, 1);
            return s;
        }

    }

}
