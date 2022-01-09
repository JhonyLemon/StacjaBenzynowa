using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace StacjaBenzynowaMVVM.Models
{
    public class Notification: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public Notification(string name, string description)
        {
            Name = name;
            Description = description;
            AddDate = DateTime.Now;
            IsRead = Brushes.LightGray;
        }

        public Notification()
        {
            Name = "";
            Description = "";
            AddDate = DateTime.Now;
            IsRead = Brushes.LightGray;
        }
        private Brush _isRead;
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime AddDate { get; set; }
        public Brush IsRead 
        {
            get { return _isRead; }
            set
            {
                _isRead = value;
                NotifyPropertyChanged();
            } 
        }

        public void Read()
        {
            IsRead = Brushes.Transparent;
        }
        public bool CheckRead
        {
            get
            {
                if (IsRead == Brushes.Transparent)
                    return false;
                else
                    return true;
            }
        }
    }
}
