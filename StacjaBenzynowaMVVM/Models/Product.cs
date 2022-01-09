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
    public class Product: INotifyPropertyChanged
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
        public int ProductID
        {
            get { return _productID; }
            set { 
                _productID = value; 
                
            }
        }

        private int _supplierID;
        public int SupplierID
        {
            get { return _supplierID; }
            set { _supplierID = value; }
        }

        private int _amount;
        public int Amount
        {
            get { return _amount; }
            set 
            { 
                _amount = value;
                NotifyPropertyChanged();
            }
        }
        private string _name;

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        private double _price;

        public double Price
        {
            get { return _price; }
            set { _price = value; }
        }

        private double _discount;

        public double Discount
        {
            get { return _discount; }
            set { _discount = value; }
        }

        private DateTime _deliveryDate;

        public DateTime DeliveryDate
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
            ProductID = product.ProductID;
            SupplierID = product.SupplierID;
            Amount = product.Amount;
            Name = product.Name;
            Price = product.Price;
            DeliveryDate = product.DeliveryDate;
            ExpirationDate = product.ExpirationDate;
            Discount = product.Discount;
        }

        public DateTime ExpirationDate
        {
            get { return _expirationDate; }
            set { _expirationDate = value; }
        }

        public double FinalPrice
        {
            get { return Price * (1 - Discount)*Amount; }
        }
        public double PricePerOne
        {
            get { return Price * (1 - Discount); }
        }

        public int CheckExpDate()
        {
            if(ExpirationDate.CompareTo(DateTime.Today)<0)
            {
                TimeSpan timeLeft = DateTime.Today.Subtract(ExpirationDate);
                int days = (timeLeft.Days/7)*5;
                if (Discount == 100 - days)
                    return 0;
                else
                {
                    if (100 - days > 0)
                        Discount = 100 - days;
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
