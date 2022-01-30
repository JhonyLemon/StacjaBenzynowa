using Caliburn.Micro;
using StacjaBenzynowaMVVM.EventModels;
using StacjaBenzynowaMVVM.Helpers.Classes;
using StacjaBenzynowaMVVM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace StacjaBenzynowaMVVM.ViewModels
{
    class ChangePasswordViewModel : Screen
    {
        private Employee _employee;
        private string _employeeOldPassword;
        private string _employeeNewPassword;
        private string _employeeNewPasswordCheck;
        private string _message;
        private Brush _messageColor;
        public Employee Employee
        {
            get { return _employee; }
            set
            {
                _employee = value;
                NotifyOfPropertyChange(() => Employee);
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
        public string EmployeeOldPassword
        {
            get { return _employeeOldPassword; }
            set
            {
                _employeeOldPassword = value;
                NotifyOfPropertyChange(() => EmployeeOldPassword);
                NotifyOfPropertyChange(() => CanChangePassword);
            }
        }
        public string EmployeeNewPassword
        {
            get { return _employeeNewPassword; }
            set
            {
                _employeeNewPassword = value;
                NotifyOfPropertyChange(() => EmployeeNewPassword);
                NotifyOfPropertyChange(() => CanChangePassword);
            }
        }
        public string EmployeeNewPasswordCheck
        {
            get { return _employeeNewPasswordCheck; }
            set
            {
                _employeeNewPasswordCheck = value;
                NotifyOfPropertyChange(() => EmployeeNewPasswordCheck);
                NotifyOfPropertyChange(() => CanChangePassword);
            }
        }

        private IEventAggregator _eventAggregator;

        public ChangePasswordViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
        }

        public void Return()
        {
            _eventAggregator.PublishOnUIThread(new ChangePasswordOnEventModel(0));
        }

        public bool CanChangePassword
        {
            get
            {
                Message = "";
                return true;
            }
        }

        public void ChangePassword()
        {
            Employee temp = DatabaseDataHelper.GetEmployee(Employee.LOGIN, EmployeeOldPassword);
            if (temp.ID_PRACOWNIKA == 0)
            {
                MessageColor = Brushes.Red;
                Message = "Stare hasło jest błędne";
            }
            else if (temp.ID_PRACOWNIKA != 0)
            {
                if (temp.ID_PRACOWNIKA != Employee.ID_PRACOWNIKA)
                {
                    MessageColor = Brushes.Red;
                    Message = "Stare hasło jest błędne";
                }
                else if (EmployeeNewPassword != EmployeeNewPasswordCheck || EmployeeNewPassword == null || EmployeeNewPasswordCheck == null)
                {
                    MessageColor = Brushes.Red;
                    Message = "Hasła się nie zgadzają";
                }
                else
                {
                    DatabaseDataHelper.UpdateEmployeePassword(EmployeeNewPassword, Employee.ID_PRACOWNIKA.ToString());
                    EmployeeOldPassword = null;
                    EmployeeNewPassword = null;
                    EmployeeNewPasswordCheck = null;
                    _eventAggregator.PublishOnUIThread(new ChangePasswordOnEventModel(2));
                }
            }
        }
    }
}
