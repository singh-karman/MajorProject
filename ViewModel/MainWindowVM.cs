using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using Completist.Model;
using System.Windows;

namespace Completist.ViewModel
{
    public class MainWindowVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private int count = 0;
        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }

        #region Commands
        public RelayCommand Loaded_Command { get; private set; }
        public RelayCommand NewTask_Command { get; private set; }
        public RelayCommand Refresh_Command { get; private set; }
        public RelayCommand Search_Command { get; private set; }
        public RelayCommand History_Command { get; private set; }
        public RelayCommand Data_Command { get; private set; }
        public RelayCommand GCalendar_Command { get; private set; }
        public RelayCommand CompleteTask_Command { get; private set; }
        public RelayCommand RemoveTask_Command { get; private set; }
        public RelayCommand EditTask_Command { get; private set; }
        public RelayCommand lstSelectionChanged_Command { get; private set; }
        public RelayCommand ChangePriority_Command { get; private set; }
        public RelayCommand ChangeTag_Command { get; private set; }
        public RelayCommand SearchTaskText_Command { get; private set; }
        #endregion

        #region Variables

        bool searchIsActive;

        Model.Tag _selectedTag_Search;
        public Model.Tag selectedTag_Search
        {
            get
            {
                return _selectedTag_Search;
            }
            set
            {
                _selectedTag_Search = value;
                NotifyPropertyChanged("selectedTag_Search");
            }
        }

        Model.Priority _selectedPriority_Search;
        public Model.Priority selectedPriority_Search
        {
            get
            {
                return _selectedPriority_Search;
            }
            set
            {
                _selectedPriority_Search = value;
                NotifyPropertyChanged("selectedPriority_Search");
            }
        }

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

        double _searchHeight;
        public double searchHeight 
        {
            get 
            {
                return _searchHeight;
            }
            set 
            {
                _searchHeight = value;
                NotifyPropertyChanged("searchHeight");
            }
        }
        string _lastSelectedName;
        public string lastSelectedName 
        {
            get 
            {
                return _lastSelectedName;
            }
            set 
            {
                _lastSelectedName = value;
                NotifyPropertyChanged("lastSelectedName");
            }
        }
        ObservableCollection<Model.Priority> _listOfPriorities;
        public ObservableCollection<Model.Priority> listOfPriorities
        {
            get
            {
                return _listOfPriorities;
            }
            set
            {
                _listOfPriorities = value;
                NotifyPropertyChanged("listOfPriorities");
            }
        }

        ObservableCollection<Model.Tag> _listOfTags;
        public ObservableCollection<Model.Tag> listOfTags
        {
            get
            {
                return _listOfTags;
            }
            set
            {
                _listOfTags = value;
                NotifyPropertyChanged("listOfTags");
            }
        }
        double _myHeight;
        public double myHeight
        {
            get
            {
                return _myHeight;
            }
            set
            {
                _myHeight = value;
                NotifyPropertyChanged("myHeight");
            }
        }
        ObservableCollection<Model.Task> _myContent;
        public ObservableCollection<Model.Task> myContent
        {
            get
            {
                return _myContent;
            }
            set
            {
                _myContent = value;
                NotifyPropertyChanged("myContent");
            }
        }

        Model.Task _selectedTask;
        public Model.Task selectedTask
        {
            get
            {

                return _selectedTask;
            }
            set
            {
                _selectedTask = value;
                NotifyPropertyChanged("selectedTask");
            }
        }

        string _myCondition_Name;
        public string myCondition_Name
        {
            get 
            {
                return _myCondition_Name;
            }
            set 
            {
                _myCondition_Name = value;
                NotifyPropertyChanged("myCondition_Name");
            }
        }

        string _myCondition_Tag;
        public string myCondition_Tag
        {
            get
            {
                return _myCondition_Tag;
            }
            set
            {
                _myCondition_Tag = value;
                NotifyPropertyChanged("myCondition_Tag");
            }
        }

        string _myCondition_Priority;
        public string myCondition_Priority
        {
            get
            {
                return _myCondition_Priority;
            }
            set
            {
                _myCondition_Priority = value;
                NotifyPropertyChanged("myCondition_Priority");
            }
        }

        string _title;
        public string title
        {
            get
            {
                return _title;
            }
            set
            {
                _title = value;
                NotifyPropertyChanged("title");
            }
        }

        ICollectionView _myContent_View;
        public ICollectionView myContent_View
        {
            get
            {
                return _myContent_View;
            }
            set
            {
                _myContent_View = value;
                NotifyPropertyChanged("myContent_View");
            }
        }

        string _visibleNew;
        public string visibleNew 
        {
            get 
            {
                return _visibleNew;
            }
            set 
            {
                _visibleNew = value;
                NotifyPropertyChanged("visibleNew");
            }
        }

        string _visibleExit;
        public string visibleExit
        {
            get
            {
                return _visibleExit;
            }
            set
            {
                _visibleExit = value;
                NotifyPropertyChanged("visibleExit");
            }
        }

        string _complete;

        public string complete
        {
            get
            {
                return _complete;
            }
            set
            {
                _complete = value;
                NotifyPropertyChanged("complete");
            }
        }

        Controller.Controller con;

        #endregion

        public MainWindowVM()
        {
            con = new Controller.Controller();
            searchIsActive = false;

            selectedPriority_Search = new Priority();
            selectedTag_Search = new Tag();

            #region Command to methods
            Loaded_Command = new RelayCommand(Loaded_Method);
            NewTask_Command = new RelayCommand(NewTask_Method);
            Refresh_Command = new RelayCommand(Refresh_Method);
            Search_Command = new RelayCommand(Search_Method);
            History_Command = new RelayCommand(History_Method);
            Data_Command = new RelayCommand(Data_Method);
            GCalendar_Command = new RelayCommand(GCalendar_Method);
            RemoveTask_Command = new RelayCommand(RemoveTask_Method);
            CompleteTask_Command = new RelayCommand(CompleteTask_Method);
            EditTask_Command = new RelayCommand(EditTask_Method);
            lstSelectionChanged_Command = new RelayCommand(lstSelectionChanged_Method);
            ChangePriority_Command = new RelayCommand(ChangePriority_Method);
            ChangeTag_Command = new RelayCommand(ChangeTag_Method);
            SearchTaskText_Command = new RelayCommand(SearchTaskText_Method); 
            #endregion
        }

        private void Refresh_Method()
        {
            Loaded_Method();
        }

        private void SearchTaskText_Method()
        {
            filterMe();
        }

        public void filterMe()
        {
            //building the connection, the necessary condition for filter, afterwhich, we call controller and the DB
            string condition = " where STS=0 and name like '%" + filterText + "%'";

            if (selectedTag_Search != null) 
            {
                condition += " AND TAGLIST LIKE '%" + selectedTag_Search.Name + "%'";
            }

            if (selectedPriority_Search != null)
            {
                condition += " AND PRIORITY LIKE '%" + selectedPriority_Search.Name + "%'";
            }
            myContent = con.returnAllTasks(condition);
            myContent = new ObservableCollection<Model.Task>(myContent.OrderBy(x => x.Due).ThenBy(v => v.Name));
        }

        private void ChangeTag_Method()
        {
            if (selectedTask == null)
            {
                MessageBox.Show("First click on task in order to select it");
                return;
            }
            //Opens tags window for changes 
            View.frmTag window = new View.frmTag();
            SystemVars.FrmTagWindow = window;
            window.ShowDialog();

            selectedTask.TagList = SystemVars.SelectedTagList;
            SystemVars.FrmTagWindow = null;
        }

        private void ChangePriority_Method()
        {
            if (selectedTask == null) 
            {
                MessageBox.Show("First click on task in order to select it");
                return; 
            }
            //Opens priority window for changes 
            View.frmPriority window = new View.frmPriority();
            SystemVars.FrmPriorityWindow = window;
            window.ShowDialog();

            selectedTask.Priority = SystemVars.SelectedPriority;
            SystemVars.FrmPriorityWindow = null;
        }
        
        private void lstSelectionChanged_Method()
        {
            //if there are no selected tasks then the menu bar says 'Inbox'
            if (selectedTask == null || String.IsNullOrEmpty(selectedTask.Name)) { selectedTask = new Model.Task(); title = "Inbox"; return; }
            //lastSelectedName = selectedTask.Name;
            title = "Editing [" + selectedTask.Name + "]";
            //if there is a task selected the menu bar says 'Editing'
            Application.Current.MainWindow.Opacity = 0.5; //background opacity change - I am aiming to mirror the prototype I created for the requirements doc
            View.frmTask window = new View.frmTask(selectedTask);
            SystemVars.FrmTaskWindow = window;
            window.ShowDialog();
            Application.Current.MainWindow.Opacity = 1; //return to normal
            title = "Inbox"; //menu bar header returns to 'Inbox'
            SystemVars.FrmTaskWindow = null;
            myContent = con.returnAllTasks(" where STS=0");
            myContent = new ObservableCollection<Model.Task>(myContent.OrderBy(x => x.Due).ThenBy(v => v.Name)); //LINQ sorting, sort by due date then by name

        }

        private void History_Method()
        {
            View.frmHistory window = new View.frmHistory();
            window.ShowDialog();
        }

        private void CompleteTask_Method()
        {
            if (selectedTask == null)
            {
                MessageBox.Show("Task is not selected ", "", MessageBoxButton.OK);
                return;
            }

            if (MessageBox.Show("Complete task [" + selectedTask.Name + "]?", "", MessageBoxButton.YesNo) == MessageBoxResult.No) { return; }

            if (con.handleTask(selectedTask, "COMPLETE", selectedTask.Name)) { myContent = con.returnAllTasks("where STS=0"); title = "Inbox"; count = count + 1; complete = count.ToString(); }
        }

        private void RemoveTask_Method()
        {
            if (selectedTask == null)
            {
                MessageBox.Show("Task is not selected ", "", MessageBoxButton.OK);
                return;
            }

            if (MessageBox.Show("Remove task [" + selectedTask.Name + "]?", "", MessageBoxButton.YesNo) == MessageBoxResult.No) { return; }

            if (con.handleTask(selectedTask, "REMOVE", selectedTask.Name)) { myContent = con.returnAllTasks("where STS=0"); title = "Inbox"; }
        }

        private void EditTask_Method()
        {
            if (selectedTask == null) { return; }
            if (String.IsNullOrEmpty(lastSelectedName)) { lastSelectedName = selectedTask.Name; }
            if (con.handleTask(selectedTask, "EDIT", lastSelectedName)) { myContent = con.returnAllTasks("where STS=0"); selectedTask = new Model.Task(); title = "Inbox"; }
        }

        private async void start_Ini()
        {
            await System.Threading.Tasks.Task.Run(() => start());
        }
        private void start() 
        {
            try
            {
                complete = con.TaskCounter().ToString(); //count.ToString();
                title = "Inbox"; //initial values
                visibleExit = "Hidden";
                visibleNew = "Visible";
                searchHeight = 0;
                myCondition_Name = "";
                myCondition_Priority = "";
                myContent = con.returnAllTasks(" where STS=0");
                myContent = new ObservableCollection<Model.Task>(myContent.OrderBy(x => x.Due).ThenBy(v => v.Name));
                listOfPriorities = con.returnAllPriorities("where STS=0");
                listOfTags = con.returnAllTags("where STS=0");
            }
            catch (Exception ex)
            {
                string exMessage = ex.Message;
            }
        }
        private void Data_Method()
        {
            View.frmData window = new View.frmData();
            window.ShowDialog();
        }

        private void GCalendar_Method()
        {
            View.GCalendar window = new View.GCalendar();
            SystemVars.FrmGCalendar = window;
            window.ShowDialog();
        }


        private void Search_Method()
        {
            filterText = "";
            if (searchIsActive)
            {
                title = "Inbox";
                visibleExit = "Hidden";
                visibleNew = "Visible";
                
                searchHeight = 0;
                searchIsActive = false;
            }
            else 
            {
                title = "Search";
                visibleExit = "Hidden";
                visibleNew = "Visible";

                searchHeight = 280;
                searchIsActive = true;
            }
        }

        private void NewTask_Method()
        {
            //method for creating a new task
            title = "New Task";
            Application.Current.MainWindow.Opacity = 0.5;
            View.frmTask window = new View.frmTask(null);
            SystemVars.FrmTaskWindow = window;
            window.ShowDialog();
            Application.Current.MainWindow.Opacity = 1;
            title = "Inbox";
            SystemVars.FrmTaskWindow = null;
            myContent = con.returnAllTasks(" where STS=0");
            myContent = new ObservableCollection<Model.Task>(myContent.OrderBy(x => x.Due).ThenBy(v => v.Name));
        }

        private void Loaded_Method()
        {
            start_Ini();
        }

    }
}
