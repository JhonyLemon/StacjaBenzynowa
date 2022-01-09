using Caliburn.Micro;
using StacjaBenzynowaMVVM.EventModels;
using StacjaBenzynowaMVVM.Helpers.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Threading;

namespace StacjaBenzynowaMVVM.ViewModels
{
    class AddSupplierViewModel:Screen
    {
        private string _message = "";
        private Brush _messageColor;
        private string _name = "";
        private DispatcherTimer _dispatcherTimer = new DispatcherTimer();
        private IEventAggregator _eventAggregator;
   
        public AddSupplierViewModel(IEventAggregator eventAggregator)
        {
            _dispatcherTimer.Interval = new TimeSpan(0, 0, 10);
            _dispatcherTimer.Tick += new EventHandler(TimerTick);
            _eventAggregator = eventAggregator;
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

        public string Message
        {
            get { return _message; }
            set
            {
                _message = value;
                NotifyOfPropertyChange(() => Message);
            }
        }

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                NotifyOfPropertyChange(() => Name);
                NotifyOfPropertyChange(() => CanAddSupplier);
            }
        }

        public bool CanAddSupplier
        {
            get
            {
                bool check = false;
                if (Name!="")
                {
                    check = true;
                }
                return check;
            }
        }

        public void AddSupplier()
        {
            if (DatabaseDataHelper.SetSupplier(Name) > 0)
            {
                Name = "";
                Message = "Dodano nowego dostawce";
                MessageColor = Brushes.LimeGreen;
                _dispatcherTimer.Start();
                _eventAggregator.PublishOnUIThread(new UpdateSuppliersOnEvent());
            }
            else
            {
                Message = "Coś poszło nie tak";
                MessageColor = Brushes.Red;
                _dispatcherTimer.Start();
            }
            NotifyOfPropertyChange(() => CanAddSupplier);
        }
        public void TimerTick(object sender, EventArgs e)
        {
            _dispatcherTimer.Stop();
            Message = "";
        }

    }
}
