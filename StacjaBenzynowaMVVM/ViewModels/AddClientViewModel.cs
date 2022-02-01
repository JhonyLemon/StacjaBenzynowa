using Caliburn.Micro;
using System;
using StacjaBenzynowaMVVM.EventModels;
using StacjaBenzynowaMVVM.Helpers.Classes;
using StacjaBenzynowaMVVM.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Windows.Media;

namespace StacjaBenzynowa.ViewModels
{
    class AddClientViewModel : Screen
    {
        private string _clientName = "";
        private string _clientSurname = "";
        private string _clientNIP = "";
        private string _message;
        private Brush _messageColor;
        private string _clientTelephoneNumber = "";

        public string ClientTelephoneNumber
        {
            get { return _clientTelephoneNumber; }
            set 
            {
                _clientTelephoneNumber = value;
                NotifyOfPropertyChange(() => ClientTelephoneNumber);
                NotifyOfPropertyChange(() => CanAddClient);
            }
        }

        public string Message
        {
            get { return _message; }
            set 
            { 
                _message = value;
                NotifyOfPropertyChange(() => Message);
            }
        }

        public Brush MessageColor
        {
            get { return _messageColor; }
            set
            {
                _messageColor = value;
                NotifyOfPropertyChange(() => MessageColor);
            }
        }

        public string ClientNIP
        {
            get { return _clientNIP; }
            set 
            {
                _clientNIP = value;
                NotifyOfPropertyChange(() => ClientNIP);
                NotifyOfPropertyChange(() => CanAddClient);
            }
        }


        public string ClientName
        {
            get { return _clientName; }
            set 
            {
                _clientName = value;
                NotifyOfPropertyChange(() => ClientName);
                NotifyOfPropertyChange(() => CanAddClient);
            }
        }

        public string ClientSurname
        {
            get { return _clientSurname; }
            set
            {
                _clientSurname = value;
                NotifyOfPropertyChange(() => ClientSurname);
                NotifyOfPropertyChange(() => CanAddClient);
            }
        }

        public bool CanAddClient
        {
            get{
                Message = "";
                if (!CheckName() || !CheckSurname() || !CheckNIP() || !CheckTelephoneNumber())
                    return false;
                return true;
            }
        }

        public void AddClient()
        {
            Client temp = DatabaseDataHelper.GetClientNIP(ClientNIP);
            Client temp1 = DatabaseDataHelper.GetClientPhone(ClientTelephoneNumber);
            if (ClientNIP.Length != 0)
            {
                if (temp.ID_KLIENTA != 0)
                {
                    MessageColor = Brushes.Red;
                    Message = "Istnieje już klient z podanym numerem NIP";
                }
            }
            else if (ClientTelephoneNumber.Length != 0)
            {
                if (temp1.ID_KLIENTA != 0)
                {
                    MessageColor = Brushes.Red;
                    Message = "Istnieje już klient z podanym numerem telefonu";
                }
            }
            else if(DatabaseDataHelper.SetClient(ClientName, ClientSurname, ClientNIP, ClientTelephoneNumber, 1) == 1)
            {
                ClientName = "";
                ClientSurname = "";
                ClientNIP = "";
                ClientTelephoneNumber = "";
                MessageColor = Brushes.Green;
                Message = "Klient zostal dodany do bazy danych";
            }
        }

        public bool CheckName()
        {
            if (ClientName == null)
                ClientName = "";
            else if (ClientName.Length == 0 || ClientName.Any(char.IsDigit) == true)
                return false;
            return true;
        }

        public bool CheckSurname()
        {
            if (ClientSurname == null)
                ClientSurname = "";
            else if (ClientName.Length == 0 || ClientSurname.Any(char.IsDigit) == true)
                return false;
            return true;
        }

        public bool CheckNIP()
        {
            if (ClientNIP.Length == 0)
            {
                return true;
            }
            else if(ClientNIP.Length == 10)
            {
                if (Regex.IsMatch(ClientNIP, @"^[0-9]+$"))
                    return true;
            }
            return false;
        }

        public bool CheckTelephoneNumber()
        {
            if (ClientTelephoneNumber.Length == 9)
            {
                if (Regex.IsMatch(ClientTelephoneNumber, @"^[0-9]+$"))
                    return true;
            }    
            return false;
        }

    }
}
