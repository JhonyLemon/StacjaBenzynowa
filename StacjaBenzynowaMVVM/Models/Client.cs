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

        public int ID_KLIENTA
        {
            get { return _clientID; }
            set { _clientID = value; }
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

        private string _nip;

        public string NIP
        {
            get { return _nip; }
            set { _nip = value; }
        }

        private double _points;

        public double PUNKTY
        {
            get { return _points; }
            set { _points = value; }
        }

        private string _telephoneNumber;

        public string NUMER_TELEFONU
        {
            get { return _telephoneNumber; }
            set { _telephoneNumber = value; }
        }

        private int _active;

        public int AKTYWNY
        {
            get { return _active; }
            set { _active = value; }
        }

        public double RABAT { get; set; }

        public string GetClientID()
        {
            if (ID_KLIENTA == 0)
                return null;
            else
                return ID_KLIENTA.ToString();
        }
    }
}
