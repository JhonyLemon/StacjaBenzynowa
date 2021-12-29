using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StacjaBenzynowaMVVM.Models
{
    public class Product
    {
        private int _amount;

        public int ILOSC
        {
            get { return _amount; }
            set { _amount = value; }
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
        private DateTime _deliveryDate;

        public DateTime DATA_DOSTAWY
        {
            get { return _deliveryDate; }
            set { _deliveryDate = value; }
        }
        private DateTime _expirationDate;

        public DateTime DATA_WAZNOSCI
        {
            get { return _expirationDate; }
            set { _expirationDate = value; }
        }

    }
}
