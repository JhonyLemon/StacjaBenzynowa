using StacjaBenzynowaLibrary;
using StacjaBenzynowaMVVM.Helpers.Interfaces;
using StacjaBenzynowaMVVM.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StacjaBenzynowaMVVM.Helpers.Classes
{
    class DatabaseDataHelper : IDatabaseDataHelper
    {
        public BindingList<Product> Products { get; set; }
        public DatabaseDataHelper()
        {
            List<string> values = new List<string> { "ID_PRODUKTU","NAZWA", "CENA", "DATA_DOSTAWY", "DATA_WAZNOSCI", "ILOSC" };
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>();
            List<Dictionary<string, string>> answer = DataBaseAccess.GetImportedData("SELECT * FROM DOSTEPNE_PRODUKTY", parameters, values);
            Products = ProductsListHelper.GetProductsList(answer);
        }
    }

}
