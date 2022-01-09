using Caliburn.Micro;
using StacjaBenzynowaMVVM.EventModels;
using StacjaBenzynowaMVVM.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StacjaBenzynowaMVVM.ViewModels
{
    class NotificationsViewModel : Screen
    {
        private ObservableCollection<Notification> _notifications;
        private Notification _notification;
        private IEventAggregator _eventAggregator;

        public NotificationsViewModel(IEventAggregator eventAggregator)
        {
            _notifications = new ObservableCollection<Notification>();
            _eventAggregator = eventAggregator;
    }

        public ObservableCollection<Notification> Notifications
        {
            get { return _notifications; }
            set
            {
                _notifications = value;
                NotifyOfPropertyChange(() => CanDeleteAll);
                NotifyOfPropertyChange(() => CanReadAll);
            }
        }

        public Notification Notification
        {
            get { return _notification; }
            set
            {
                _notification = value;
                if(_notification!=null)
                    _notification.Read();
                _eventAggregator.PublishOnUIThread(new NotificationCheckOnEvent());
                NotifyOfPropertyChange(() => Notification);
                NotifyOfPropertyChange(() => CanDelete);
            }
        }

        public bool CanDelete
        {
            get
            {
                bool b = false;
                if (Notification != null && Notifications.Count>0)
                    b = true;
                return b;
            }
        }
        public bool CanDeleteAll
        {
            get
            {
                bool b = false;
                if (Notifications.Count > 0)
                    b = true;
                return b;
            }
        }
        public bool CanReadAll
        {
            get
            {
                bool b = false;
                if (Notifications.Count > 0)
                    b = true;
                return b;
            }
        }

        public void Delete()
        {
            Notifications.Remove(Notification);
            _eventAggregator.PublishOnUIThread(new NotificationCheckOnEvent());
        }
        public void DeleteAll()
        {
            Notifications.Clear();
            _eventAggregator.PublishOnUIThread(new NotificationCheckOnEvent());
        }
        public void ReadAll()
        {
            foreach(Notification n in Notifications)
            {
                n.Read();
            }
            _eventAggregator.PublishOnUIThread(new NotificationCheckOnEvent());
        }
        public bool CheckIfUnread()
        {
            foreach(Notification n in Notifications)
            {
                if (n.CheckRead == true)
                    return true;
            }
            return false;
        }

        public void AddNotifications(ObservableCollection<Notification> notifications)
        {
            foreach(Notification n in notifications)
            {
                Notifications.Add(n);
            }    
        }
    }
}
