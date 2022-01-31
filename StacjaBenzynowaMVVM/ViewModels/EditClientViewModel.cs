using Caliburn.Micro;
using StacjaBenzynowaMVVM.EventModels;
using StacjaBenzynowaMVVM.Helpers.Classes;
using StacjaBenzynowaMVVM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Media;

namespace StacjaBenzynowaMVVM.ViewModels
{
    class EditClientViewModel : Screen

    {
        private Client _client;
        private string _clientName;
        private string _clientSurname;
        private string _clientNIP;
        private string _clientTelephoneNumber;
        private string _message;
        private Brush _messageColor;
        private IEventAggregator _eventAggregator;

        public EditClientViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
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

        public Client Client
        {
            get { return _client; }
            set
            {
                _client = value;
            }
        }

        public string ClientTelephoneNumber
        {
            get { return _clientTelephoneNumber; }
            set
            {
                _clientTelephoneNumber = value;
                NotifyOfPropertyChange(() => ClientTelephoneNumber);
                NotifyOfPropertyChange(() => CanEditClient);
            }
        }

        public string ClientNIP
        {
            get { return _clientNIP; }
            set
            {
                _clientNIP = value;
                NotifyOfPropertyChange(() => ClientNIP);
                NotifyOfPropertyChange(() => CanEditClient);
            }
        }


        public string ClientName
        {
            get { return _clientName; }
            set
            {
                _clientName = value;
                NotifyOfPropertyChange(() => ClientName);
                NotifyOfPropertyChange(() => CanEditClient);
            }
        }

        public string ClientSurname
        {
            get { return _clientSurname; }
            set
            {
                _clientSurname = value;
                NotifyOfPropertyChange(() => ClientSurname);
                NotifyOfPropertyChange(() => CanEditClient);
            }
        }
        public bool CanEditClient
        {
            get
            {
                Message = "";
                if (!CheckName() || !CheckSurname() || !CheckNIP() || !CheckTelephoneNumber())
                    return false;
                return true;
            }
        }

        public void EditClient()
        {
            Client temp = DatabaseDataHelper.GetClientNIP(ClientNIP);
            Client temp1 = DatabaseDataHelper.GetClientPhone(ClientTelephoneNumber);
            if (ClientNIP.Length != 0)
            {
                if (temp.ID_KLIENTA != 0)
                {
                    if (temp.ID_KLIENTA != Client.ID_KLIENTA)
                    {
                        MessageColor = Brushes.Red;
                        Message = "Istnieje już klient z podanym numerem NIP";
                    }
                    else
                    {
                        AddToBase();
                    }
                }
                else
                {
                    AddToBase();
                }
            }
            else if(ClientTelephoneNumber.Length != 0)
            {
                if (temp1.ID_KLIENTA != 0)
                {
                    if(temp1.ID_KLIENTA != Client.ID_KLIENTA)
                    {
                        MessageColor = Brushes.Red;
                        Message = "Istnieje już klient z podanym numerem telefonu";
                    }
                    else
                    {
                        AddToBase();
                    }
                }
                else
                {
                    AddToBase();
                }
            }
            else
            {
                AddToBase();
            }
        }

        public void Return()
        {
            _eventAggregator.PublishOnUIThread(new EditClientOnEventModel(0));
        }

        public void AddToBase()
        {
                Client.IMIE = ClientName;
                Client.NAZWISKO = ClientSurname;
                Client.NIP = ClientNIP;
                Client.NUMER_TELEFONU = ClientTelephoneNumber;
                Client.AKTYWNY = 1;
                DatabaseDataHelper.UpdateClient(Client);
                _eventAggregator.PublishOnUIThread(new EditClientOnEventModel(2));
        }

        public bool CheckName()
        {
            if (ClientName == null)
                ClientName = "";
            else if  (Regex.IsMatch(ClientName, @"^[0-9]+$"))
                return false;
            return true;
        }

        public bool CheckSurname()
        {
            if (ClientSurname == null)
                ClientSurname = "";
            else if (Regex.IsMatch(ClientSurname, @"^[0-9]+$"))
                return false;
            return true;
        }

        public bool CheckNIP()
        {
            if (ClientNIP == null)
            {
                ClientNIP = "";
            }
            if (ClientNIP.Length == 0)
            {
                return true;
            }
            else if (ClientNIP.Length == 10)
            {
                if (Regex.IsMatch(ClientNIP, @"^[0-9]+$"))
                    return true;
            }
            return false;
        }

        public bool CheckTelephoneNumber()
        {
            if (ClientTelephoneNumber == null)
                ClientTelephoneNumber = "";
            if (ClientTelephoneNumber.Length == 9)
            {
                if (Regex.IsMatch(ClientTelephoneNumber, @"^[0-9]+$"))
                    return true;
            }
            return false;
        }
    }
}
