using Caliburn.Micro;
using StacjaBenzynowa.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using StacjaBenzynowaLibrary;
using StacjaBenzynowaMVVM.EventModels;
using System.Windows;

namespace StacjaBenzynowaMVVM.ViewModels
{
    class ShellViewModel:Conductor<object>,IHandle<LogOnEvent>, IHandle<LogOutOnEvent>, IHandle<ReturnOnEvent>
    {

        private Visibility _menuVisibility= Visibility.Hidden;
        private LoginViewModel _loginViewModel;
        private SaleViewModel _saleViewModel;
        private AddClientViewModel _addClientViewModel;
        private LogoutViewModel _logoutViewModel;
        private DeliveriesViewModel _deliveriesViewModel;
        private IEventAggregator _eventAggregator;
        private Screen previouslyActive;
        public ShellViewModel(LoginViewModel loginViewModel, IEventAggregator eventAggregator,SaleViewModel saleViewModel,AddClientViewModel addClientViewModel, LogoutViewModel logoutViewModel, DeliveriesViewModel deliveriesViewModel)
        {
            _loginViewModel = loginViewModel;
            _eventAggregator = eventAggregator;
            _saleViewModel = saleViewModel;
            _addClientViewModel = addClientViewModel;
            _logoutViewModel = logoutViewModel;
            _deliveriesViewModel = deliveriesViewModel;
            _eventAggregator.Subscribe(this);
            ActivateItem(_loginViewModel);
        }

        public Visibility MenuVisibility
        {
            get { return _menuVisibility; }
            set
            { 
                _menuVisibility = value;
                NotifyOfPropertyChange(()=>MenuVisibility);
            }
        }

        public void Handle(LogOnEvent message)
        {
            previouslyActive = (Screen)ActiveItem;
            DeactivateItem(_loginViewModel, false);
            ActivateItem(_saleViewModel);
            MenuVisibility = Visibility.Visible;
        }

        public void Sale()
        {
            previouslyActive = (Screen)ActiveItem;
            ActivateItem(_saleViewModel);
        }
        public void AddClient()
        {
            previouslyActive = (Screen)ActiveItem;
            ActivateItem(_addClientViewModel);
        }
        public void Logout()
        {
            previouslyActive = (Screen)ActiveItem;
            ActivateItem(_logoutViewModel);
        }
        public void Deliveries()
        {
            previouslyActive = (Screen)ActiveItem;
            ActivateItem(_deliveriesViewModel);
        }

        public void Handle(LogOutOnEvent message)
        {
            MenuVisibility = Visibility.Hidden;
            ActivateItem(_loginViewModel);
        }

        public void Handle(ReturnOnEvent message)
        {
            ActivateItem(previouslyActive);
        }
    }
}
