using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StacjaBenzynowaMVVM.Models
{
    public class Client
    {
        private int _clientID;
        public int ClientID
        {
            get { return _clientID; }
            set { _clientID = value; }
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

        private string _nip;

        public string NIP
        {
            get { return _nip; }
            set { _nip = value; }
        }

        private int _points;

        public int Points
        {
            get { return _points; }
            set { _points = value; }
        }

    }
}
