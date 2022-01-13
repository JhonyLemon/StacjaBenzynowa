using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StacjaBenzynowaMVVM.Models
{
    public class Employee:BaseModel
    {
        private int _employeeID;
        public int ID_PRACOWNIKA
        {
            get { return _employeeID; }
            set { _employeeID = value; }
        }

        private string _firstName;

        public string IMIE
        {
            get { return _firstName; }
            set { _firstName = value; }
        }

        private string _surName;

        public string NAZWISKO
        {
            get { return _surName; }
            set { _surName = value; }
        }

        private string _position;

        public string POZYCJA
        {
            get { return _position; }
            set { _position = value; }
        }

        public Employee()
        {
        }
    }
}
