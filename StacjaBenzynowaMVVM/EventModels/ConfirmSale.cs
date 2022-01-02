using StacjaBenzynowaMVVM.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StacjaBenzynowaMVVM.EventModels
{
    class ConfirmSale
    {
        public BindingList<Product> cartItems;

        public ConfirmSale(BindingList<Product> cartItems)
        {
            this.cartItems = cartItems;
        }
    }
}
