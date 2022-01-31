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
using System.Windows.Threading;
using System.Windows.Media;

namespace StacjaBenzynowaMVVM.ViewModels
{
    class ShellViewModel:Conductor<object>,IHandle<LogOnEvent>, IHandle<LogOutOnEvent>, IHandle<ReturnOnEvent>, IHandle<ConfirmSaleOnEventModel>,IHandle<SoldOnEvent>,IHandle<AddNotificationsOnEvent>,IHandle<UpdateProductsOnEvent>,IHandle<UpdateSuppliersOnEvent>,IHandle<NotificationCheckOnEvent>, IHandle<DeleteEmployeeOnEventModel>, IHandle<DeleteClientOnEventModel>, IHandle<EditClientOnEventModel>, IHandle<EditEmployeeOnEventModel>, IHandle<ChangePasswordOnEventModel>
    {

        private DispatcherTimer _expirationDateChecker = new DispatcherTimer();
        private Brush _notificationColor = Brushes.Black;
        private Visibility _menuVisibility= Visibility.Hidden;
        private Visibility _deliveriesVisibility = Visibility.Collapsed;
        private Visibility _addClientVisibility = Visibility.Collapsed;
        private Visibility _addEmployeeVisibility = Visibility.Collapsed;
        private LoginViewModel _loginViewModel;
        private SaleViewModel _saleViewModel;
        private AddClientViewModel _addClientViewModel;
        private LogoutViewModel _logoutViewModel;
        private DeliveriesViewModel _deliveriesViewModel;
        private CheckOutViewModel _checkOutViewModel;
        private AddSupplierViewModel _addSupplierViewModel;
        private AddEmployeeViewModel _addEmployeeViewModel;
        private NotificationsViewModel _notificationsViewModel;
        private DeleteEmployeeViewModel _deleteEmployeeViewModel;
        private EmployeeListViewModel _employeeListViewModel;
        private ClientListViewModel _clientListViewModel;
        private DeleteClientViewModel _deleteClientViewModel;
        private EditClientViewModel _editClientViewModel;
        private EditEmployeeViewModel _editEmployeeViewModel;
        private ChangePasswordViewModel _changePasswordViewModel;
        private SaleListViewModel _saleListViewModel;
        private IEventAggregator _eventAggregator;
        private Screen previouslyActive;
        public ShellViewModel(LoginViewModel loginViewModel, IEventAggregator eventAggregator,SaleViewModel saleViewModel,AddClientViewModel addClientViewModel, LogoutViewModel logoutViewModel, DeliveriesViewModel deliveriesViewModel,CheckOutViewModel checkOutViewModel, AddSupplierViewModel addSupplierViewModel, AddEmployeeViewModel addEmployeeViewModel, NotificationsViewModel notificationsViewModel, DeleteEmployeeViewModel deleteEmployeeViewModel, EmployeeListViewModel employeeListViewModel, ClientListViewModel clientListViewModel, DeleteClientViewModel deleteClientViewModel, EditClientViewModel editClientViewModel, EditEmployeeViewModel editEmployeeViewModel, ChangePasswordViewModel changePasswordViewModel, SaleListViewModel saleListViewModel)
        {
            _loginViewModel = loginViewModel;
            _eventAggregator = eventAggregator;
            _saleViewModel = saleViewModel;
            _addClientViewModel = addClientViewModel;
            _logoutViewModel = logoutViewModel;
            _deliveriesViewModel = deliveriesViewModel;
            _checkOutViewModel = checkOutViewModel;
            _addSupplierViewModel = addSupplierViewModel;
            _addEmployeeViewModel = addEmployeeViewModel;
            _notificationsViewModel = notificationsViewModel;
            _deleteEmployeeViewModel = deleteEmployeeViewModel;
            _employeeListViewModel = employeeListViewModel;
            _clientListViewModel = clientListViewModel;
            _deleteClientViewModel = deleteClientViewModel;
            _editClientViewModel = editClientViewModel;
            _editEmployeeViewModel = editEmployeeViewModel;
            _changePasswordViewModel = changePasswordViewModel;
            _saleListViewModel = saleListViewModel;
            _expirationDateChecker.Interval = TimeSpan.FromHours(1);
            _expirationDateChecker.Tick += _dispatcherTimer_Tick;
            _eventAggregator.Subscribe(this);
            ActivateItem(_loginViewModel);
            _expirationDateChecker.Start();
        }

        private void _dispatcherTimer_Tick(object sender, EventArgs e)
        {
            _saleViewModel.UpdateDiscounts();
        }

        private void NotificationsCheck()
        {
            if (_notificationsViewModel.CheckIfUnread())
                NotificationColor = Brushes.Red;
            else
                NotificationColor = Brushes.Black;
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

        public Visibility DeliveriesVisibility
        {
            get { return _deliveriesVisibility; }
            set
            {
                _deliveriesVisibility = value;
                NotifyOfPropertyChange(() => DeliveriesVisibility);
            }
        }

        public Visibility AddClientVisibility
        {
            get { return _addClientVisibility; }
            set
            {
                _addClientVisibility = value;
                NotifyOfPropertyChange(() => AddClientVisibility);
            }
        }

        public Visibility AddEmployeeVisibility
        {
            get { return _addEmployeeVisibility; }
            set
            {
                _addEmployeeVisibility = value;
                NotifyOfPropertyChange(() => AddEmployeeVisibility);
            }
        }

        public Brush NotificationColor
        {
            get { return _notificationColor; }
            set
            {
                _notificationColor = value;
                NotifyOfPropertyChange(()=>NotificationColor);
            }
        }
        public void Handle(LogOnEvent message)
        {
            previouslyActive = (Screen)ActiveItem;
            DeactivateItem(_loginViewModel, false);
            MenuVisibility = Visibility.Visible;
            switch (_loginViewModel.Employee.POZYCJA)
            {
                case "Właściciel":
                    {
                        DeliveriesVisibility = Visibility.Visible;
                        AddClientVisibility = Visibility.Visible;
                        AddEmployeeVisibility = Visibility.Visible;
                        break;
                    }
                case "Kierownik":
                    {
                        DeliveriesVisibility = Visibility.Visible;
                        AddClientVisibility = Visibility.Visible;
                        AddEmployeeVisibility = Visibility.Visible;
                        break;
                    }
                case "Kasjer":
                    {
                        DeliveriesVisibility = Visibility.Visible;
                        AddClientVisibility = Visibility.Collapsed;
                        AddEmployeeVisibility = Visibility.Collapsed;
                        break;
                    }
                case "Pracownik podjazdu":
                    {
                        DeliveriesVisibility = Visibility.Collapsed;
                        AddClientVisibility = Visibility.Collapsed;
                        AddEmployeeVisibility = Visibility.Collapsed;
                        break;
                    }
            }
            ActivateItem(_saleViewModel);
            _dispatcherTimer_Tick(this, null);
        }

        public void Sale()
        {
            ClearCart();
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

        public void AddSupplier()
        {
            previouslyActive = (Screen)ActiveItem;
            ActivateItem(_addSupplierViewModel);
        }

        public void Notifications()
        {
            previouslyActive = (Screen)ActiveItem;
            ActivateItem(_notificationsViewModel);
        }

        public void EmployeeList()
        {
            previouslyActive = (Screen)ActiveItem;
            _employeeListViewModel.employee = _loginViewModel.Employee;
            _employeeListViewModel.Employee = null;
            _employeeListViewModel.UpdateList();
            ActivateItem(_employeeListViewModel);
        }

        public void ClientList()
        {
            previouslyActive = (Screen)ActiveItem;
            _clientListViewModel.Client = null;
            _clientListViewModel.UpdateList();
            ActivateItem(_clientListViewModel);
        }

        public void SaleList()
        {
            previouslyActive = (Screen)ActiveItem;
            _saleListViewModel.UpdateList();
            _saleListViewModel.SaleDetails = null;
            ActivateItem(_saleListViewModel);
        }
        public void AddEmployee()
        {
            previouslyActive = (Screen)ActiveItem;
            ActivateItem(_addEmployeeViewModel);
        }

        public void Handle(LogOutOnEvent message)
        {
            previouslyActive = (Screen)ActiveItem;
            MenuVisibility = Visibility.Hidden;
            ActivateItem(_loginViewModel);
        }

        public void Handle(ReturnOnEvent message)
        {
            ActivateItem(previouslyActive);
        }

        public void Handle(ConfirmSaleOnEventModel message)
        {
            previouslyActive = (Screen)ActiveItem;
            ActivateItem(_checkOutViewModel);
            _checkOutViewModel.Employee = _loginViewModel.Employee;
            _checkOutViewModel.CartItems = message.cartItems;
        }

        public void ClearCart()
        {
            if (_saleViewModel.CartItems != null)
                _saleViewModel.ClearCart();
        }

        public void Handle(SoldOnEvent message)
        {
            _saleViewModel.CartItems.Clear();
            _saleViewModel.RemoveZeroItems();
            _checkOutViewModel.Message = "";
            ActivateItem(_saleViewModel);
        }
        public void Handle(AddNotificationsOnEvent message)
        {
            _notificationsViewModel.AddNotifications(message.notifications);
            NotificationsCheck();
        }

        public void Handle(UpdateSuppliersOnEvent message)
        {
            _deliveriesViewModel.GetSuppliers();
        }
        public void Handle(UpdateProductsOnEvent message)
        {
            _saleViewModel.GetProducts();
        }

        public void Handle(NotificationCheckOnEvent message)
        {
            NotificationsCheck();
        }

        public void Handle(DeleteEmployeeOnEventModel message)
        {
            if (message.State == 1)
            {
                previouslyActive = (Screen)ActiveItem;
                _deleteEmployeeViewModel.Employee = _employeeListViewModel.Employee;
                ActivateItem(_deleteEmployeeViewModel);
            }
            else if(message.State == 2)
            {
                _employeeListViewModel.Employee = null;
                _deleteEmployeeViewModel.Employee = null;
                _employeeListViewModel.UpdateList();
                ActivateItem(previouslyActive);
            }
            else if(message.State == 0)
            {
                ActivateItem(previouslyActive);
            }
        }

        public void Handle(DeleteClientOnEventModel message)
        {
            if(message.State == 1)
            {
                previouslyActive = (Screen)ActiveItem;
                _deleteClientViewModel.Client = _clientListViewModel.Client;
                ActivateItem(_deleteClientViewModel);
            }
            else if(message.State == 2)
            {
                _clientListViewModel.Client = null;
                _deleteClientViewModel.Client = null;
                _clientListViewModel.UpdateList();
                ActivateItem(previouslyActive);
            }
            else if(message.State == 0)
            {
                ActivateItem(previouslyActive);
            }
        }

        public void Handle(EditEmployeeOnEventModel message)
        {
            if(message.State == 1)
            {
                previouslyActive = (Screen)ActiveItem;
                _editEmployeeViewModel.Employee = _employeeListViewModel.Employee;
                _editEmployeeViewModel.EmployeeName = _employeeListViewModel.Employee.IMIE;
                _editEmployeeViewModel.EmployeeSurname = _employeeListViewModel.Employee.NAZWISKO;
                _editEmployeeViewModel.EmployeeLogin = _employeeListViewModel.Employee.LOGIN;
                ActivateItem(_editEmployeeViewModel);
            }
            if(message.State == 2)
            {
                _employeeListViewModel.Employee = null;
                _editEmployeeViewModel.Employee = null;
                _editEmployeeViewModel.EmployeePosition = null;
                _employeeListViewModel.UpdateList();
                ActivateItem(previouslyActive);
            }
            if(message.State == 0)
            {
                ActivateItem(previouslyActive);
            }
        }

        public void Handle(EditClientOnEventModel message)
        {
            if (message.State == 1)
            {
                previouslyActive = (Screen)ActiveItem;
                _editClientViewModel.Client = _clientListViewModel.Client;
                _editClientViewModel.ClientName = _clientListViewModel.Client.IMIE;
                _editClientViewModel.ClientSurname = _clientListViewModel.Client.NAZWISKO;
                _editClientViewModel.ClientNIP = _clientListViewModel.Client.NIP;
                _editClientViewModel.ClientTelephoneNumber = _clientListViewModel.Client.NUMER_TELEFONU;
                ActivateItem(_editClientViewModel);
            }
            else if (message.State == 2)
            {
                _clientListViewModel.Client = null;
                _editClientViewModel.Client = null;
                _clientListViewModel.UpdateList();
                ActivateItem(previouslyActive);
            }
            else if (message.State == 0)
            {
                ActivateItem(previouslyActive);
            }
        }

        public void Handle(ChangePasswordOnEventModel message)
        {
            if(message.State == 1)
            {
                previouslyActive = (Screen)ActiveItem;
                _changePasswordViewModel.Employee = _employeeListViewModel.Employee;
                ActivateItem(_changePasswordViewModel);
            }
            else if(message.State == 2)
            {
                _clientListViewModel.Client = null;
                _editClientViewModel.Client = null;
                _clientListViewModel.UpdateList();
                ActivateItem(previouslyActive);
            }
            else if(message.State == 0)
            {
                ActivateItem(previouslyActive);
            }
        }
    }
}
