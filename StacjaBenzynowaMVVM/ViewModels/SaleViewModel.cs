using Caliburn.Micro;
using StacjaBenzynowaMVVM.Models;
using StacjaBenzynowaMVVM.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using StacjaBenzynowaLibrary;
using System.Text;
using System.Threading.Tasks;

namespace StacjaBenzynowa.ViewModels
{
    class SaleViewModel:Screen
    {
        private List<Product> _products;

        public List<Product> Products 
        {
            get { return _products; }
            set { _products = value; } 
        }

        public SaleViewModel()
        {
            List<string> values = new List<string> { "NAZWA","CENA","DATA_DOSTAWY","DATA_WAZNOSCI","ILOSC" };
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>();
            List<Dictionary<string, string>> answer = DataBaseAccess.GetImportedData("SELECT NAZWA,CENA,DATA_DOSTAWY,DATA_WAZNOSCI,ILOSC FROM PRODUKTY", parameters, values);
            Products = ProductsListHelper.GetList(answer);
        }
    }
}
