using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Completist.Model
{
    public class GCalendarModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }

        }

        string eventName;
        DateTime eventTime;

        public string EventName
        {
            get
            {
                return eventName;
            }
            set
            {
                eventName = value;
                NotifyPropertyChanged(EventName);
            }
        }
        public DateTime EventTime
        {
            get
            {
                return eventTime;
            }
            set
            {
                eventTime = value;
                NotifyPropertyChanged(EventTime.ToString());
            }
        }
    }
}
