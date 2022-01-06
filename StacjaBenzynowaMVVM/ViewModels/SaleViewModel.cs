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
            _products = DatabaseDataHelper.GetProducts();
            _eventAggregator = eventAggregator;
            _cartItems = new ObservableCollection<Product>();
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
                if (Amount>0 && Product!=null && Product.Amount>=Amount)
                    check = true;
                return check;
            }

        }

        public void AddToCart()
        {
            Product.Amount -= Amount;
            Product product = new Product(Product);
            product = new Product(product);
            product.Amount = Amount;
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
                if (p.ProductID == CartItem.ProductID)
                {
                    p.Amount += CartItem.Amount;
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
                    if (p.ProductID == c.ProductID)
                    {
                        p.Amount += c.Amount;                        
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
                        if (p.ProductID == CartItem.ProductID)
                        {
                            i = Products.IndexOf(p);
                            break;
                        }
                    }
                    if (Amount > 0 && Products.ElementAt(i).Amount >= Amount - CartItem.Amount)
                        check = true;
                }
                return check;
            }
        }
        public void ChangeAmount()
        {
            foreach (Product p in Products)
            {
                if (p.ProductID == CartItem.ProductID)
                {
                    p.Amount += CartItem.Amount;
                    p.Amount -= Amount;
                    CartItem.Amount = Amount;
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
            _eventAggregator.PublishOnUIThread(new ConfirmSale(CartItems));
        }

        public void RemoveZeroItems()
        {
            foreach (Product p in Products.ToList())
            {
                if(p.Amount==0)
                    Products.Remove(p);
            }
        }

    }
}
