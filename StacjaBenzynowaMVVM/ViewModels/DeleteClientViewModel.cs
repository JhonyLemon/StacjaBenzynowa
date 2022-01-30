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
    class DeleteClientViewModel : Screen
    {
        private Client _client;
        private IEventAggregator _eventAggregator;

        public DeleteClientViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
        }

        public Client Client
        {
            get { return _client; }
            set
            {
                _client = value;
            }
        }

        public void Return()
        {
            _eventAggregator.PublishOnUIThread(new DeleteClientOnEventModel(0));
        }

        public void DeleteClient()
        {
            Client.AKTYWNY = 0;
            DatabaseDataHelper.UpdateClient(Client);
            _eventAggregator.PublishOnUIThread(new DeleteClientOnEventModel(2));
        }
    }
}
