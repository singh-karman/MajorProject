//Garbage and now depreciated feature 

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Completist.Model;

namespace Completist.ViewModel
{
    public class frmHistoryVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }

       
        Controller.Controller con;
        public RelayCommand Search_Command { get; private set; }

        string _filterText;
        public string filterText
        {
            get
            {
                return _filterText;
            }
            set
            {
                _filterText = value;
                NotifyPropertyChanged("filterText");
            }
        }

        string _filterTag;
        public string filterTag
        {
            get
            {
                return _filterTag;
            }
            set
            {
                _filterTag = value;
                NotifyPropertyChanged("filterTag");
            }
        }

        ObservableCollection<Model.Task> _listOfTasks;
        public ObservableCollection<Model.Task> listOfTasks
        {
            get
            {
                return _listOfTasks;
            }
            set
            {
                _listOfTasks = value;
                NotifyPropertyChanged("listOfTasks");
            }
        }

        DateTime _filterDate;
        public DateTime filterDate
        {
            get
            {
                return _filterDate;
            }
            set
            {
                _filterDate = value;
                NotifyPropertyChanged("filterDate");
            }
        }

        public frmHistoryVM()
        {
            Search_Command = new RelayCommand(Search_Method);
            con = new Controller.Controller();
        }

        private void Search_Method()
        {
            try
            {
                string condition = "where (NAME LIKE '%" + filterText + "%' OR CONTENT LIKE '%" + filterText + "%') AND TAGLIST LIKE '%" + filterTag + "%'";

                listOfTasks = con.returnAllTasks(condition);
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Error/" + ex.Message);
            }

        }
    }
}
