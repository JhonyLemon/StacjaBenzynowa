using StacjaBenzynowaMVVM.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StacjaBenzynowaMVVM.EventModels
{
    class ConfirmSaleOnEventModel
    {
        public ObservableCollection<Product> cartItems;

        public ConfirmSaleOnEventModel(ObservableCollection<Product> cartItems)
        {
            this.cartItems = cartItems;
        }
    }
}
