using StacjaBenzynowaMVVM.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StacjaBenzynowaMVVM.Helpers.Interfaces
{
    interface IDatabaseDataHelper
    {
        BindingList<Product> Products { get; set; }
    }
}
