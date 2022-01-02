using Caliburn.Micro;
using StacjaBenzynowaMVVM.EventModels;
using StacjaBenzynowaMVVM.Helpers.Classes;
using StacjaBenzynowaMVVM.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StacjaBenzynowaMVVM.ViewModels
{
    class CheckOutViewModel:Screen, IHandle<ConfirmSale>
    {
        private BindingList<Product> _cartItems;
        private Client _clientClass;

        public Client ClientClass
        {
            get { return _clientClass; }
            set { _clientClass = value; }
        }

        public BindingList<Product> CartItems 
        {
            get { return _cartItems; }
            set { _cartItems = value; }
        }

        public void Handle(ConfirmSale message)
        {
            CartItems = message.cartItems;
            RecalculatePrice();
        }

        private double _discount;

        public double Discount
        {
            get { return _discount; }
            set
            {
                _discount = value;
                NotifyOfPropertyChange(() => Discount);
            }
        }

        private double _price;

        public double Price
        {
            get { return _price; }
            set
            {
                _price = value;
                NotifyOfPropertyChange(() => Price);
            }
        }

        private double _sum;

        public double Sum
        {
            get { return _sum; }
            set
            {
                _sum = value;
                NotifyOfPropertyChange(() => Sum);
            }
        }

        private string _client;

        public string Client
        {
            get { return _client; }
            set
            {
                _client = value;
                NotifyOfPropertyChange(() => Client);
                NotifyOfPropertyChange(() => CanConfirmCart);
            }
        }

        public void RecalculatePrice()
        {
            double temp2 = 0;
            double temp = 0;
            foreach (Product p in CartItems)
            {
                temp += p.Price * p.Discount * p.Amount;
                temp2 += p.Price * p.Amount;
            }
            if(ClientClass!=null && ClientClass.Points>=100)
            {
                temp = temp * 0.9;
            }
            Discount = temp2 - temp;
            Price = temp2;
            Sum = temp;
        }

        public bool CanConfirmCart
        {
            get
            {
                bool check = false;
                if (Client.Length == 16)
                {
                    ClientClass = DatabaseDataHelper.GetClient(Client.Replace('0', ' ').Trim());
                    if (ClientClass != null)
                    {
                        check = true;
                        RecalculatePrice();
                    }
                }
                else if(ClientClass!=null)
                {
                    ClientClass = null;
                    RecalculatePrice();
                }
                return check;
            }
        }

        public void ConfirmCart()
        {

        }


    }
}
