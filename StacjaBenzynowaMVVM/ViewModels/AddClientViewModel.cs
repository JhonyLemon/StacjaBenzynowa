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

namespace StacjaBenzynowa.ViewModels
{
    class AddClientViewModel : Screen
    {
        private string _clientName = "";
        private string _clientSurname = "";
        private string _clientNIP = "";
        private string _message;

        public string Message
        {
            get { return _message; }
            set 
            { 
                _message = value;
                NotifyOfPropertyChange(() => Message);
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
                if (Regex.IsMatch(ClientName, @"^[a-zA-Z]+$") == true && Regex.IsMatch(ClientSurname, @"^[a-zA-Z]+$") == true && 
                    Regex.IsMatch(ClientNIP, @"^[0-9]+$") == true && ClientNIP.Length == 10)
                    return true;
                else
                    return false;
            }
        }

        public void AddClient()
        {
            if(DatabaseDataHelper.SetClient(ClientName, ClientSurname, ClientNIP) == 1)
            {
                ClientName = "";
                ClientSurname = "";
                ClientNIP = "";
                Message = "Klient zostal dodany do bazy danych";
            }
        }
    }
}
