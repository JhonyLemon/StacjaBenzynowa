using Caliburn.Micro;
using StacjaBenzynowaLibrary;
using StacjaBenzynowaMVVM.EventModels;
using StacjaBenzynowaMVVM.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace StacjaBenzynowa.ViewModels
{
    class LoginViewModel:Screen
    {
        private string _userName;
        private string _password;
        private string _errorMessage;
        private IEventAggregator _eventAggregator;

        public LoginViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
        }

        public string UserName
        {
            get { return _userName; }
            set 
            {
                _userName = value;
                NotifyOfPropertyChange(()=> UserName);
                NotifyOfPropertyChange(() => CanLogIn);
            }
        }

        public string ErrorMessage
        {
            get { return _errorMessage; }
            set
            {
                _errorMessage = value;
                NotifyOfPropertyChange(() => ErrorMessage);
            }
        }

        public string Password
        {
            get { return _password; }
            set 
            { 
                _password = value;
                NotifyOfPropertyChange(() => Password);
                NotifyOfPropertyChange(() => CanLogIn);
            }
        }
        
        public bool CanLogIn
        {
            get
            {
                ErrorMessage = "";
                bool check = false;
                if (Password !=null && UserName!=null && UserName.Length > 0 && Password.Length > 0)
                    check = true;
                return check;
            }

        }

        public void LogIn()
        {
            List<string> values = new List<string> { "ID_PRACOWNIKA" };
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>> { new KeyValuePair<string, string>("@login", UserName), new KeyValuePair<string, string>("@password", PasswordHashHelper.HashPassword(Password)) };
            List<Dictionary<string, string>> answer = DataBaseAccess.GetImportedData("SELECT ID_PRACOWNIKA FROM PRACOWNICY WHERE LOGIN=@login AND HASLO=@password", parameters, values);
            if (answer.Count == 0)
            {
                ErrorMessage = "Błędny login lub hasło";
            }
            else
            {
                _eventAggregator.PublishOnUIThread(new LogOnEvent());
                UserName = "";
                Password = "";
            }
        }

    }
}
