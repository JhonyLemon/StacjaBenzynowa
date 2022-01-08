using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StacjaBenzynowaMVVM.Models
{
    public class Supplier
    {
        private int _supplierID;
        public int SupplierID
        {
            get { return _supplierID; }
            set { _supplierID = value; }
        }
        private string _supplierName;
        public string SupplierName
        {
            get { return _supplierName; }
            set { _supplierName = value; }
        }
    }
}
