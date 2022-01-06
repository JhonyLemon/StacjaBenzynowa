using Caliburn.Micro;
using StacjaBenzynowaMVVM.EventModels;
using StacjaBenzynowaMVVM.Helpers.Classes;
using StacjaBenzynowaMVVM.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StacjaBenzynowaMVVM.ViewModels
{
    class CheckOutViewModel:Screen
    {
        private ObservableCollection<Product> _cartItems;
        private Client _clientClass;
        private IEventAggregator _eventAggregator;
        private Employee _employee;


        public CheckOutViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
        }

        public Client ClientClass
        {
            get { return _clientClass; }
            set { _clientClass = value; }
        }

        public Employee Employee
        {
            get { return _employee; }
            set { _employee = value; }
        }

        public ObservableCollection<Product> CartItems 
        {
            get { return _cartItems; }
            set 
            {
                _cartItems = value;
                RecalculatePrice();
            }
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
            double withOutDiscount = 0;
            double withDiscount = 0;
            foreach (Product p in CartItems)
            {
                withDiscount += p.Price * (1-p.Discount) * p.Amount;

                withOutDiscount += p.Price * p.Amount;
            }
            if(ClientClass!=null && ClientClass.Points>=10 && withDiscount>=200)
            {
                ClientClass.Discount = 0.1;
                withDiscount = withDiscount * (1-ClientClass.Discount);
            }
            Discount = withOutDiscount - withDiscount;
            Price = withOutDiscount;
            Sum = withDiscount;
        }

        public bool CanConfirmCart
        {
            get
            {
                bool check = true;
                if (Client!=null && Client.Length == 16)
                {
                    ClientClass = DatabaseDataHelper.GetClient(Client.Replace('0', ' ').Trim());
                    if (ClientClass != null)
                    {
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
            if(ClientClass==null)
            {
                ClientClass = new Client();
            }
            if (DatabaseDataHelper.SetSale(ClientClass, CartItems, Employee, Sum) == 0)
            {
                Client = "";
                _eventAggregator.PublishOnUIThread(new SoldOnEvent());
                ClientClass = null;
            }
        }

        public void Return()
        {
            _eventAggregator.PublishOnUIThread(new ReturnOnEvent());
        }
    }
}
