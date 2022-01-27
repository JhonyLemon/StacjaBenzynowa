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
using System.Windows.Media;
using System.Windows.Threading;

namespace StacjaBenzynowa.ViewModels
{
    class DeliveriesViewModel:Screen
    {
        private ObservableCollection<Product> _products;
        private Product _product;
        private int _ammount;
        private Brush _messageColor;
        private double _price;
        private string _name="";
        private DateTime _expDate = DateTime.Today;
        private Supplier _supplier;
        private string _message="";
        private ObservableCollection<Supplier> _suppliers;
        DispatcherTimer _dispatcherTimer = new DispatcherTimer();
        private IEventAggregator _eventAggregator;
           
        public DeliveriesViewModel(IEventAggregator eventAggregator)
        {
            _products = new ObservableCollection<Product>();
            GetSuppliers();
            _dispatcherTimer.Interval = new TimeSpan(0, 0, 10);
            _dispatcherTimer.Tick += new EventHandler(TimerTick);
            _eventAggregator = eventAggregator;
        }

        public void GetSuppliers()
        {
            _suppliers = DatabaseDataHelper.GetSuppliers();
        }

        public ObservableCollection<Product> Products
        {
            get { return _products; }
            set
            {
                _products = value;
            }
        }

        public ObservableCollection<Supplier> Suppliers
        {
            get { return _suppliers; }
            set
            {
                _suppliers = value;
            }
        }

        public Product Product
        {
            get { return _product; }
            set
            {
                _product = value;
                NotifyOfPropertyChange(() => Product);
                NotifyOfPropertyChange(()=>CanDelete);
            }
        }
        public Brush MessageColor
        {
            get { return _messageColor; }
            set
            {
                _messageColor = value;
                NotifyOfPropertyChange(() => MessageColor);
            }
        }

        public int Amount
        {
            get { return _ammount; }
            set
            {
                _ammount = value;
                NotifyOfPropertyChange(() => Amount);
                NotifyOfPropertyChange(() => CanChangeAmount);
                NotifyOfPropertyChange(() => CanAddToCart);
            }
        }

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                NotifyOfPropertyChange(() => Name);
                NotifyOfPropertyChange(() => CanAddToCart);
            }
        }
        public string Message
        {
            get { return _message; }
            set
            {
                _message = value;
                NotifyOfPropertyChange(() => Message);
            }
        }

        public DateTime ExpDate
        {
            get { return _expDate; }
            set
            {
                _expDate = value;
                NotifyOfPropertyChange(() => ExpDate);
                NotifyOfPropertyChange(() => CanAddToCart);
            }
        }

        public Supplier Supplier
        {
            get { return _supplier; }
            set
            {
                _supplier = value;
                NotifyOfPropertyChange(() => Supplier);
                NotifyOfPropertyChange(() => CanAddToCart);
            }
        }

        public double Price
        {
            get { return _price; }
            set
            {
                _price = value;
                NotifyOfPropertyChange(() => Price);
                NotifyOfPropertyChange(() => CanAddToCart);
            }
        }

        public bool CanChangeAmount
        {
            get
            {
                bool check = false;
                if (Product != null)
                {
                    if (Amount > 0)
                        check = true;
                }
                return check;
            }
        }
        public void ChangeAmount()
        {
            Product.ILOSC = Amount;
            Amount = 0;
            Product = null;
        }

        public bool CanAddToCart
        {
            get
            {
                bool check = false;
                if (Amount > 0 && Name!="" && Supplier!=null && Price>0)
                    check = true;
                return check;
            }

        }

        public void AddToCart()
        {
            Product product = new Product();
            product.NAZWA = Name;
            product.ILOSC = Amount;
            product.ID_DOSTAWCY = Supplier.ID_DOSTAWCY;
            product.CENA = Price;
            product.DATA_DOSTAWY = DateTime.Now;
            if (ExpDate > DateTime.Today)
            {
                product.DATA_WAZNOSCI = ExpDate;
            }
            Products.Add(product);
            Amount = 0;
            Supplier = null;
            Price = 0;
            ExpDate = DateTime.Today;
            Name = "";
            NotifyOfPropertyChange(() => CanConfirmDelivery);
        }

        public bool CanDelete
        {
            get
            {
                bool check = false;
                if (Products.Count > 0 && Product != null)
                    check = true;
                return check;
            }
        }

        public void Delete()
        {
            Products.Remove(Product);
            NotifyOfPropertyChange(() => CanConfirmDelivery);
        }

        public bool CanConfirmDelivery
        {
            get
            {
                bool check = false;
                if (Products.Count > 0)
                    check = true;
                return check;
            }
        }
        public void ConfirmDelivery()
        {

            if (DatabaseDataHelper.SetProducts(Products) == 0)
            {
                Product = null;
                Products.Clear();
                Amount = 0;
                Supplier = null;
                Price = 0;
                ExpDate = DateTime.Today;
                Name = "";
                Message = "Dodano nowe produkty";
                MessageColor = Brushes.LimeGreen;
                _dispatcherTimer.Start();
                _eventAggregator.PublishOnUIThread(new UpdateProductsOnEvent());
            }
            else
            {
                Message = "Coś poszło nie tak";
                MessageColor = Brushes.Red;
                _dispatcherTimer.Start();
            }
            NotifyOfPropertyChange(() => CanConfirmDelivery);
        }
        public void TimerTick(object sender, EventArgs e)
        {
            _dispatcherTimer.Stop();
            Message = "";
        }

    }
}
