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
    public class Task : INotifyPropertyChanged //class modifier that enables all attribute of an object to be updated in View when there is a change in data or if the Notify interface is called. i.e as soon as a new name for a task is SET, then the view render is told to update as well as the collection of tasks to reflect changes in the listView.
    {

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }

        public override string ToString() //Using polymorphism
        {
            return Name;
        }

        string name; //string name of task
        string content; //description
        DateTime due; //due date using System.DateTime
        string strDue; //date but in words. limited for a small range, most times will be numbers
        Priority priority; //field inheriting properties of priority class
        ObservableCollection<Tag> tagList; // Notify change is an inherent interface of observable. Changes will automatically change data rendered to View. since you can have more than one tag, I have used a collection which makes it easier for data binding more than one element [USED BY LISTVIEW]
        string strTag; //array for tags, will be seperated by ;
        int status; //integer value representing the status 
        string strStatus; //string name of integer status - check SQL table for three statuses
        int taskID; //PRIMARY KEY for SQL table: identifier for tasks.

        //The following are getter/setter methods for all properties
        public string Name
        {
            get //gets current value of respective property ["NAME"]
            {
                return name;
            }

            set //assigns a new value to the respective property ["NAME"]
            {
                name = value;
                NotifyPropertyChanged("Name");
            }
        }
        //the same applies for the following properties. Decision for this method of setting up a class is to promote the safety of data transactions and preventing the need for global fields
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
