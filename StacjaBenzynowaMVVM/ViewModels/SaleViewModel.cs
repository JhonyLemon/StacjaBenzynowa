using Caliburn.Micro;
using StacjaBenzynowaMVVM.Models;
using StacjaBenzynowaMVVM.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using StacjaBenzynowaLibrary;
using System.Text;
using System.Threading.Tasks;
using StacjaBenzynowaMVVM.Helpers.Classes;
using System.ComponentModel;
using StacjaBenzynowaMVVM.EventModels;
using System.Collections.ObjectModel;

namespace StacjaBenzynowa.ViewModels
{
    class SaleViewModel:Screen
    {
        ObservableCollection<Product> _products;
        ObservableCollection<Product> _cartItems;
        private int _ammount;
        private Product _product;
        private Product _cartItem;
        private IEventAggregator _eventAggregator;

        public SaleViewModel(IEventAggregator eventAggregator)
        {
            GetProducts();
            _eventAggregator = eventAggregator;
            _cartItems = new ObservableCollection<Product>();
        }

        public void GetProducts()
        {
            _products = DatabaseDataHelper.GetProducts();
            RemoveZeroItems();
        }

        public Product Product
        {
            get { return _product; }
            set
            {
                _product = value;
                NotifyOfPropertyChange(() => Product);
                NotifyOfPropertyChange(() => CanAddToCart);
            }
        }

        public Product CartItem
        {
            get { return _cartItem; }
            set
            {
                _cartItem = value;
                NotifyOfPropertyChange(() => CartItem);
                NotifyOfPropertyChange(() => CanDeleteFromCart);
                NotifyOfPropertyChange(() => CanChangeAmount);
            }
        }

        public ObservableCollection<Product> Products
        {
            get { return _products; }
            set 
            { 
                _products = value;
            }
        }

        public ObservableCollection<Product> CartItems
        {
            get { return _cartItems; }
            set
            {
                _cartItems = value;
            }
        }

        public int Amount
        {
            get { return _ammount; }
            set
            {
                if(value.ToString().Length <= 30)
                    _ammount = value;
                NotifyOfPropertyChange(() => Amount);
                NotifyOfPropertyChange(() => CanAddToCart);
                NotifyOfPropertyChange(() => CanChangeAmount);
            }
        }

        public bool CanAddToCart
        {
            get
            {
                bool check = false;
                if (Amount>0 && Product!=null && Product.ILOSC>=Amount)
                    check = true;
                return check;
            }

        }

        public void AddToCart()
        {
            Product.ILOSC -= Amount;
            Product product = new Product(Product);
            product = new Product(product);
            product.ILOSC = Amount;
                CartItems.Add(product);
                Amount = 0;
                Product = null;
            NotifyOfPropertyChange(() => CanConfirmCart);
        }

        public bool CanDeleteFromCart
        {
            get
            {
                bool check = false;
                if (CartItems.Count>0 && CartItem!=null)
                    check = true;
                return check;
            }
        }

        public void DeleteFromCart()
        {
            foreach (Product p in Products)
            {
                if (p.ID_PRODUKTU == CartItem.ID_PRODUKTU)
                {
                    p.ILOSC += CartItem.ILOSC;
                    CartItems.Remove(CartItem);
                    break;
                }
            }
            NotifyOfPropertyChange(() => CanConfirmCart);
        }

        public void ClearCart()
        {
            foreach (Product c in CartItems)
            {
                foreach (Product p in Products)
                {
                    if (p.ID_PRODUKTU == c.ID_PRODUKTU)
                    {
                        p.ILOSC += c.ILOSC;                        
                        break;
                    }
                }
            }
            CartItems.Clear();
            NotifyOfPropertyChange(() => CanConfirmCart);
        }

        public bool CanChangeAmount
        {
            get 
            {
                int i = 0;
                bool check = false;
                if (CartItem != null)
                {
                    foreach (Product p in Products)
                    {
                        if (p.ID_PRODUKTU == CartItem.ID_PRODUKTU)
                        {
                            i = Products.IndexOf(p);
                            break;
                        }
                    }
                    if (Amount > 0 && Products.ElementAt(i).ILOSC >= Amount - CartItem.ILOSC)
                        check = true;
                }
                return check;
            }
        }
        public void ChangeAmount()
        {
            foreach (Product p in Products)
            {
                if (p.ID_PRODUKTU == CartItem.ID_PRODUKTU)
                {
                    p.ILOSC += CartItem.ILOSC;
                    p.ILOSC -= Amount;
                    CartItem.ILOSC = Amount;
                    break;
                }
            }
        }

        public bool CanConfirmCart
        {
            get
            {
                bool check = false;
                if (CartItems.Count>0)
                check = true;
                return check;
            }
        }
        public void ConfirmCart()
        {
            _eventAggregator.PublishOnUIThread(new ConfirmSaleOnEventModel(CartItems));
        }

        public void RemoveZeroItems()
        {
            ObservableCollection<Notification> notifications = new ObservableCollection<Notification>();
            foreach (Product p in Products.ToList())
            {
                if (p.ILOSC == 0)
                {
                    notifications.Add(new Notification("Wyprzedzano", p.NAZWA));
                    Products.Remove(p);
                }
            }
            if (notifications.Count > 0 && _eventAggregator!=null)
                _eventAggregator.PublishOnUIThread(new AddNotificationsOnEvent(notifications));
        }

        public void UpdateDiscounts()
        {
            ObservableCollection<Notification> notifications = new ObservableCollection<Notification>();
            int i = 0;
            List<Product> expieredProducts = new List<Product>();
            List<Product> discountChangedProducts = new List<Product>();
            foreach(Product p in Products)
            {
                i = p.CheckExpDate();
               if (i==-1)
               {
                    expieredProducts.Add(p);
                    notifications.Add(new Notification("Koniec Daty Ważności", p.NAZWA));
                }
               else if(i==1)
               {
                    discountChangedProducts.Add(p);
                    notifications.Add(new Notification("Zmiana ceny", p.NAZWA));
                }
            }
            if (expieredProducts.Count > 0)
                DatabaseDataHelper.SetExpiredProducts(expieredProducts);
            if (discountChangedProducts.Count > 0)
                DatabaseDataHelper.UpdateProducts(discountChangedProducts);
            if (notifications.Count > 0)
            {
                GetProducts();
                _eventAggregator.PublishOnUIThread(new AddNotificationsOnEvent(notifications));
            }
        }

    }
}
