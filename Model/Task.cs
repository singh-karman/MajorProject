using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Completist.Model
{
    //class for tasks objects
    public class Task : INotifyPropertyChanged
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

        string name; //string name of task
        string content; //description
        DateTime due; //due date using System.DateTime
        string strDue; //date but in words. limited for a small range, most times will be numbers
        Priority priority;
        ObservableCollection<Tag> tagList; //since you can have more than one tag, I have used an observable collection which makes it easier for data binding more than one element
        string strTag; //array for tags, will be seperated by ;
        int status;
        string strStatus;
        int taskID;

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

        public string Content
        {
            get
            {
                return content;
            }

            set
            {
                content = value;
                NotifyPropertyChanged("Content");
            }
        }

        public DateTime Due
        {
            get
            {
                return due;
            }

            set
            {
                due = value;
                NotifyPropertyChanged("Due");
            }
        }

        public string StrDue
        {
            get
            {
                return strDue;
            }

            set
            {
                strDue = value;
                NotifyPropertyChanged("StrDue");
            }
        }

        public Priority Priority
        {
            get
            {
                return priority;
            }

            set
            {
                priority = value;
                NotifyPropertyChanged("Priority");
            }
        }

        public ObservableCollection<Tag> TagList
        {
            get
            {
                return tagList;
            }

            set
            {
                tagList = value;
                NotifyPropertyChanged("TagList");
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

        public string StrTag
        {
            get
            {
                return strTag;
            }

            set
            {
                strTag = value;
                NotifyPropertyChanged("StrTag");
            }
        }

        public int TaskID
        {
            get
            {
                return taskID;
            }
            set
            {
                taskID = value;
                NotifyPropertyChanged("TaskID");
            }
        }
    }
}
