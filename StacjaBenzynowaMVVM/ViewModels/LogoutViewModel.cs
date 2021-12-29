using Caliburn.Micro;
using StacjaBenzynowaMVVM.EventModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StacjaBenzynowa.ViewModels
{
    class LogoutViewModel:Screen
    {
        private IEventAggregator _eventAggregator;
        public LogoutViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
        }

        public void LogOut()
        {
            _eventAggregator.PublishOnUIThread(new LogOutOnEvent());
        }
        public void Return()
        {
            _eventAggregator.PublishOnUIThread(new ReturnOnEvent());
        }
    }
}
