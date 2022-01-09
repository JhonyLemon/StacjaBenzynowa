using StacjaBenzynowaMVVM.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StacjaBenzynowaMVVM.EventModels
{
    public class AddNotificationsOnEvent
    {
        public ObservableCollection<Notification> notifications { get; set; }

        public AddNotificationsOnEvent(ObservableCollection<Notification> notifications)
        {
            this.notifications = notifications;
        }
    }
}
