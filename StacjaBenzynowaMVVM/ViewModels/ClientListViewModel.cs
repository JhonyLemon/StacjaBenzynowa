using Caliburn.Micro;
using System;
using StacjaBenzynowaMVVM.EventModels;
using StacjaBenzynowaMVVM.Helpers.Classes;
using StacjaBenzynowaMVVM.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace StacjaBenzynowaMVVM.ViewModels
{
    class ClientListViewModel : Screen
    {
        private ObservableCollection<Client> _clients;
        private Client _client;
        private IEventAggregator _eventAggregator;

        public ClientListViewModel(IEventAggregator eventAggregator)
        {
            UpdateList();
            _eventAggregator = eventAggregator;
        }

        public Client Client
        {
            get { return _client; }
            set
            {
                _client = value;
                NotifyOfPropertyChange(() => Client);
                NotifyOfPropertyChange(() => CanDeleteClient);
                NotifyOfPropertyChange(() => CanEditClient);
            }
        }
        public ObservableCollection<Client> Clients
        {
            get { return _clients; }
            set
            {
                _clients = value;
                NotifyOfPropertyChange(() => Clients);
            }
        }

        public void UpdateList()
        {
            _clients = DatabaseDataHelper.GetClients();
        }

        public bool CanEditClient
        {
            get
            {
                bool check = false;
                if (Client != null)
                    check = true;
                return check;
            }
        }

        public void EditClient()
        {
            _eventAggregator.PublishOnUIThread(new EditClientOnEventModel(1));
        }

        public bool CanDeleteClient
        {
            get 
            {
                bool check = false;
                if (Client != null)
                    check = true;
                return check;
            }
        }

        public void DeleteClient()
        {
            _eventAggregator.PublishOnUIThread(new DeleteClientOnEventModel(1));
        }
    }
}
