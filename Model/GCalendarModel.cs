using System.ComponentModel;

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
        string eventTime;
        public override string ToString()
        {
            return EventName;
        }
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
        public string EventTime
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
