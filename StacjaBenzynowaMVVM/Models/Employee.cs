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
        public int EmployeeID
        {
            get { return _employeeID; }
            set { _employeeID = value; }
        }

        private string _firstName;

        public string FirstName
        {
            get { return _firstName; }
            set { _firstName = value; }
        }

        private string _surName;

        public string SurName
        {
            get { return _surName; }
            set { _surName = value; }
        }

        private string _position;

        public string Position
        {
            get { return _position; }
            set { _position = value; }
        }

    }
}
