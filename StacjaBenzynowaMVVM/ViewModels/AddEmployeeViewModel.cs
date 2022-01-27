using Caliburn.Micro;
using System;
using StacjaBenzynowaMVVM.EventModels;
using StacjaBenzynowaMVVM.Helpers.Classes;
using StacjaBenzynowaMVVM.Helpers;
using StacjaBenzynowaMVVM.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace StacjaBenzynowaMVVM.ViewModels
{
    class AddEmployeeViewModel:Screen
    {
        private string _employeeName = "";
        private string _employeeSurname = "";
        private string _employeeLogin;
        private Position _employeePosition;
        private string _employeePassword;
        private string _message;
        private ObservableCollection<Position> _positions;
        private IEventAggregator _eventAggregator;

        public AddEmployeeViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            _positions = DatabaseDataHelper.GetPositions();
        }


        public ObservableCollection<Position> Positions
        {
            get { return _positions; }
            set 
            { 
                _positions = value; 
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



        public string EmployeePassword
        {
            get { return _employeePassword; }
            set 
            { 
                _employeePassword = value;
                NotifyOfPropertyChange(() => CanAddEmployee);
                NotifyOfPropertyChange(() => EmployeePassword);
            }
        }


        public Position EmployeePosition
        {
            get { return _employeePosition; }
            set 
            {
                _employeePosition = value;
                NotifyOfPropertyChange(() => EmployeePosition);
                NotifyOfPropertyChange(() => CanAddEmployee);
            }
        }


        public string EmployeeLogin
        {
            get { return _employeeLogin; }
            set 
            { 
                _employeeLogin = value;
                NotifyOfPropertyChange(() => CanAddEmployee);
                NotifyOfPropertyChange(() => EmployeeLogin);
            }
        }


        public string EmployeeSurname
        {
            get { return _employeeSurname; }
            set 
            {
                _employeeSurname = value;
                NotifyOfPropertyChange(() => CanAddEmployee);
                NotifyOfPropertyChange(() => EmployeeSurname);
            }
        }

        public string EmployeeName
        {
            get { return _employeeName; }
            set 
            { 
                _employeeName = value;
                NotifyOfPropertyChange(() => CanAddEmployee);
                NotifyOfPropertyChange(() => EmployeeName);
            }
        }

        public bool CanAddEmployee
        {
            get
            {
                Message = "";
                bool output = false;

                if (EmployeeLogin != null && EmployeeLogin.Length > 0 && EmployeePassword != null && EmployeePassword.Length > 0
                    && EmployeePosition != null && Regex.IsMatch(EmployeeSurname, @"^[a-zA-Z-żŻźŹąĄęĘśŚóÓłŁćĆńŃ]+$") && Regex.IsMatch(EmployeeSurname, @"^[a-zA-Z-żŻźŹąĄęĘśŚóÓłŁćĆńŃ]+$"))
                    output = true;
                return output;
            }
        }

        public void AddEmployee()
        {
            DatabaseDataHelper.SetEmployee(EmployeeName, EmployeeSurname, EmployeePosition.ID_STANOWISKA,1, EmployeeLogin, PasswordHashHelper.HashPassword(EmployeePassword));
            EmployeeName = "";
            EmployeeSurname = "";
            EmployeePosition = null;
            EmployeeLogin = null;
            EmployeePassword = null;
            Message = "Pracownik zostal dodany do bazy danych";
        }
    }
}
