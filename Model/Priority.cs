using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Completist.Model
{
    //class for priorities
    public class Priority : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }

        public override string ToString()
        {
            return Name;
        }
        string strName;
        string name;
        string color;
        int status; // three statuses represented by integers 0,1,9 
        string strStatus; 

        public string StrName
        {
            get
            {
                return strName;
            }

            set
            {
                strName = value;
                NotifyPropertyChanged("StrName");
            }
        }

        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                name = value;
                NotifyPropertyChanged("Name");
            }
        }

        public string Color
        {
            get
            {
                return color;
            }

            set
            {
                color = value;
                NotifyPropertyChanged("Color");
            }
        }

        public int Status
        {
            get
            {
                return status;
            }

            set
            {
                status = value;
                NotifyPropertyChanged("Status");
            }
        }

        public string StrStatus
        {
            get
            {
                return strStatus;
            }

            set
            {
                strStatus = value;
                NotifyPropertyChanged("StrStatus");
            }
        }
    }
}
