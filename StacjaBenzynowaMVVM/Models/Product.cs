using System;
using Caliburn.Micro;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace StacjaBenzynowaMVVM.Models
{
    public class Product:BaseModel,INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private int _productID;
        public int ID_PRODUKTU
        {
            get { return _productID; }
            set { 
                _productID = value; 
                
            }
        }

        private int _supplierID;
        public int ID_DOSTAWCY
        {
            get { return _supplierID; }
            set { _supplierID = value; }
        }

        private int _amount;
        public int ILOSC
        {
            get { return _amount; }
            set 
            { 
                _amount = value;
                NotifyPropertyChanged();
            }
        }
        private string _name;

        public string NAZWA
        {
            get { return _name; }
            set { _name = value; }
        }
        private double _price;

        public double CENA
        {
            get { return _price; }
            set { _price = value; }
        }

        private double _discount;

        public double RABAT
        {
            get { return _discount; }
            set { _discount = value; }
        }

        private DateTime _deliveryDate;

        public DateTime DATA_DOSTAWY
        {
            get { return _deliveryDate; }
            set { _deliveryDate = value; }
        }
        private DateTime _expirationDate;

        public Product()
        {
        }

        public Product(Product product)
        {
            ID_PRODUKTU = product.ID_PRODUKTU;
            ID_DOSTAWCY = product.ID_DOSTAWCY;
            ILOSC = product.ILOSC;
            NAZWA = product.NAZWA;
            CENA = product.CENA;
            DATA_DOSTAWY = product.DATA_DOSTAWY;
            DATA_WAZNOSCI = product.DATA_WAZNOSCI;
            RABAT = product.RABAT;
        }

        public DateTime DATA_WAZNOSCI
        {
            get { return _expirationDate; }
            set { _expirationDate = value; }
        }

        public string FinalPrice
        {
            get { return (CENA * (1 - RABAT)*ILOSC).ToString("0.00", System.Globalization.CultureInfo.InvariantCulture); }
        }



        public string PricePerOne
        {
            get { return (CENA * (1 - RABAT)).ToString("0.00", System.Globalization.CultureInfo.InvariantCulture); }
        }

        public int CheckExpDate()
        {
            if(DATA_WAZNOSCI.CompareTo(DateTime.Today)>0)
            {
                TimeSpan timeLeft = DATA_WAZNOSCI.Subtract(DateTime.Today);
                int days = (timeLeft.Days/7)*5;
                if (RABAT == ((double)((100 - days))) / 100)
                    return 0;
                else
                {
                    if (100 - days > 0)
                        RABAT = ((double)((100 - days)))/100;
                    else
                        return 0;
                }
                return 1;
            }
            else
            {
                return -1;
            }
        }

    }
}
