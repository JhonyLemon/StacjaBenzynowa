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

namespace StacjaBenzynowaMVVM.ViewModels
{
    class EmployeeListViewModel : Screen
    {
        private Employee _employee;
        public Employee employee;
        private ObservableCollection<Employee> _employees;
        private IEventAggregator _eventAggregator;

        public EmployeeListViewModel(IEventAggregator eventAggregator)
        {
            UpdateList();
            _eventAggregator = eventAggregator;
        }

        public void UpdateList()
        {
            _employees = DatabaseDataHelper.GetEmployees();
        }

        public Employee Employee
        {
            get { return _employee; }
            set 
            { 
                _employee = value;
                NotifyOfPropertyChange(()=>Employee);
                NotifyOfPropertyChange(()=>CanDeleteEmployee);
            }
        }

        public ObservableCollection<Employee> Employees
        {
            get { return _employees; }
            set 
            { 
                _employees = value;
                NotifyOfPropertyChange(() => Employees);
            }
        }

        public bool CanDeleteEmployee
        {
            get
            {
                bool check = false;
                if (Employee != null && Employee.ID_PRACOWNIKA!=employee.ID_PRACOWNIKA)
                    check = true;
                return check;
            }
        }

        public void DeleteEmployee()
        {
            _eventAggregator.PublishOnUIThread(new DeleteEmployeeOnEventModel(1));
        }

    }
}
