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

namespace StacjaBenzynowa.ViewModels
{
    class SaleViewModel:Screen
    {
        BindingList<Product> _products;
        BindingList<Product> _cartItems;
        private int _ammount;
        private Product _product;
        private Product _cartItem;
        private IEventAggregator _eventAggregator;

        public SaleViewModel(IEventAggregator eventAggregator)
        {
            _products = DatabaseDataHelper.GetProducts();
            _eventAggregator = eventAggregator;
            _cartItems = new BindingList<Product>();
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

        public BindingList<Product> Products
        {
            get { return _products; }
            set 
            { 
                _products = value;
                NotifyOfPropertyChange(() => Products);
            }
        }

        public BindingList<Product> CartItems
        {
            get { return _cartItems; }
            set
            {
                _cartItems = value;
                NotifyOfPropertyChange(() => CartItems);
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
            int i = Products.IndexOf(Product);
            Product.Amount -= Amount;
            Product product = new Product(Product);
            Products.Remove(Product);
            Products.Insert(i, product);
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
            int i=0;
            Product product = null;
            foreach (Product p in Products)
            {
                if (p.ProductID == CartItem.ProductID)
                {
                    i = Products.IndexOf(p);
                    product = new Product(Products.ElementAt(i));
                    Products.RemoveAt(i);
                    product.Amount += CartItem.Amount;
                    Products.Insert(i, product);
                    CartItems.Remove(CartItem);
                    break;
                }
            }
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
            int i = 0;
            Product product = null;
            foreach (Product p in Products)
            {
                if (p.ProductID == CartItem.ProductID)
                {
                    i = Products.IndexOf(p);
                    product = new Product(Products.ElementAt(i));
                    Products.RemoveAt(i);
                    product.Amount += CartItem.Amount-Amount;
                    Products.Insert(i, product);
                    i = CartItems.IndexOf(CartItem);
                    CartItems.Remove(CartItem);
                    product = new Product(product);
                    product.Amount = Amount;
                    CartItems.Insert(i, product);
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

    }
}
