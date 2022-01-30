using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StacjaBenzynowaMVVM.Models
{
    public class Employee
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

        private int _employed;
        public int ZATRUDNIONY
        {
            get { return _employed; }
            set { _employed = value; }
        }

        private string _login;
        public string LOGIN
        {
            get { return _login; }
            set { _login = value; }
        }
        
        public Employee()
        {
        }
    }
}
