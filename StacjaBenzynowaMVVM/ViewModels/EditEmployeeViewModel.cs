using Caliburn.Micro;
using StacjaBenzynowaMVVM.EventModels;
using StacjaBenzynowaMVVM.Helpers.Classes;
using StacjaBenzynowaMVVM.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace StacjaBenzynowaMVVM.ViewModels
{
    class EditEmployeeViewModel : Screen
    {
        private Employee _employee;
        private string _employeeName = "";
        private string _employeeSurname = "";
        private string _employeeLogin;
        private Position _employeePosition;
        private string _message;
        private Brush _messageColor;
        private IEventAggregator _eventAggregator;
        private ObservableCollection<Position> _positions;

        public EditEmployeeViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            _positions = DatabaseDataHelper.GetPositions();
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

        public Employee Employee
        {
            get { return _employee; }
            set
            {
                _employee = value;
                NotifyOfPropertyChange(() => Employee);
            }
        }
        public ObservableCollection<Position> Positions
        {
            get { return _positions; }
            set
            {
                _positions = value;
            }
        }

        public Position EmployeePosition
        {
            get { return _employeePosition; }
            set
            {
                _employeePosition = value;
                NotifyOfPropertyChange(() => EmployeePosition);
                NotifyOfPropertyChange(() => CanEditEmployee);
            }
        }


        public string EmployeeLogin
        {
            get { return _employeeLogin; }
            set
            {
                _employeeLogin = value;
                NotifyOfPropertyChange(() => EmployeeLogin);
                NotifyOfPropertyChange(() => CanEditEmployee);
            }
        }


        public string EmployeeSurname
        {
            get { return _employeeSurname; }
            set
            {
                _employeeSurname = value;
                NotifyOfPropertyChange(() => EmployeeSurname);
                NotifyOfPropertyChange(() => CanEditEmployee);
            }
        }

        public string EmployeeName
        {
            get { return _employeeName; }
            set
            {
                _employeeName = value;
                NotifyOfPropertyChange(() => EmployeeName);
                NotifyOfPropertyChange(() => CanEditEmployee);
            }
        }

        public bool CanEditEmployee
        {
            get
            {
                Message = "";
                return true;
            }
        }

        public void EditEmployee()
        {
            
            if (EmployeePosition == null)
            {
                MessageColor = Brushes.Red;
                Message = "Wybierz pozycje";
            }
            else if(CheckLogin() == false)
            {
                MessageColor = Brushes.Red;
                Message = "Podany login jest już zajęty";
            }
            else 
            {
                Employee.IMIE = EmployeeName;
                Employee.NAZWISKO = EmployeeSurname;
                Employee.POZYCJA = EmployeePosition.ID_STANOWISKA.ToString();
                Employee.LOGIN = EmployeeLogin;
                DatabaseDataHelper.UpdateEmployee(Employee);
                _eventAggregator.PublishOnUIThread(new EditEmployeeOnEventModel(2));
            }
        }

        public void Return()
        {
            _eventAggregator.PublishOnUIThread(new EditEmployeeOnEventModel(0));
        }

        public bool CheckLogin()
        {
            if(EmployeeLogin == Employee.LOGIN)
            {
                return true;
            }
            else 
            {
                Employee temp = DatabaseDataHelper.GetEmployeeLogin(EmployeeLogin);
                if(temp.ID_PRACOWNIKA != 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }
    }
}
