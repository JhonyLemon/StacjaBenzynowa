using Caliburn.Micro;
using System;
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
                if (Regex.IsMatch(ClientName, @"^[a-zA-Z]+$") == true && Regex.IsMatch(ClientSurname, @"^[a-zA-Z]+$") == true)
                    return true;
                else
                    return false;
            }
         
        }

        public void AddClient()
        { 

        }
    }
}
