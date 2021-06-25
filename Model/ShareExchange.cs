using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Completist.Model
{
    public class ShareExchange : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string info)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(info));

        }

        string shareName;
        double sharePrice;
        int shareIndex;
        string shareIndicatorColor;
        public override string ToString()
        {
            return ShareName;
        }
        public string ShareName
        {
            get
            {
                return shareName;
            }
            set
            {
                shareName = value;
                NotifyPropertyChanged("EventName");
            }
        }
        public int ShareIndex
        {
            get
            {
                return shareIndex;
            }
            set
            {
                shareIndex = value;
                NotifyPropertyChanged("ShareIndex");
            }
        }
        public double SharePrice
        {
            get
            {
                return sharePrice;
            }
            set
            {
                sharePrice = value;
                NotifyPropertyChanged("SharePrice");
            }
        }
        public string ShareIndicatorColor
        {
            get
            {
                return shareIndicatorColor;
            }
            set
            {
                shareIndicatorColor = value;
                NotifyPropertyChanged("ShareIndicatorColor");
            }
        }
    }
}
