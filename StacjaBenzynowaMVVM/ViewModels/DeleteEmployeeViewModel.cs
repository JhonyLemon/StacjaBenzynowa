using Caliburn.Micro;
using StacjaBenzynowaMVVM.EventModels;
using StacjaBenzynowaMVVM.Helpers.Classes;
using StacjaBenzynowaMVVM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StacjaBenzynowaMVVM.ViewModels
{
    class DeleteEmployeeViewModel : Screen
    {
        private Employee _employee;
        private IEventAggregator _eventAggregator;

        public DeleteEmployeeViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
        }
        public Employee Employee
        {
            get { return _employee; }
            set
            {
                _employee = value;
            }
        }
        public void DeleteEmployee()
        {
            Employee.ZATRUDNIONY = 0;
            DatabaseDataHelper.UpdateEmployee(Employee);
            _eventAggregator.PublishOnUIThread(new DeleteEmployeeOnEventModel(2));
        }
        public void Return()
        {
            _eventAggregator.PublishOnUIThread(new DeleteEmployeeOnEventModel(0));
        }
    }
}
