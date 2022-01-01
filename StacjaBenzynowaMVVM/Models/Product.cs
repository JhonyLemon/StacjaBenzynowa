using System;
using Caliburn.Micro;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StacjaBenzynowaMVVM.Models
{
    public class Product
    {



        private int _productID;
        public int ProductID
        {
            get { return _productID; }
            set { _productID = value; }
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
        }

        public DateTime ExpirationDate
        {
            get { return _expirationDate; }
            set { _expirationDate = value; }
        }

        public string NameAndAmount
        {
            get { return Name + " " + Amount; }
        }

    }
}
