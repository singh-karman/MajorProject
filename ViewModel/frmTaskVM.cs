using Completist.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Completist.ViewModel
{
    public class frmTaskVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }

        public RelayCommand ChooseDate_Command { get; private set; }
        public RelayCommand ChoosePriority_Command { get; private set; }
        public RelayCommand ChooseTags_Command { get; private set; }
        public RelayCommand SaveTask_Command { get; private set; }
        public RelayCommand ExitTask_Command { get; private set; }
        public RelayCommand ClearMeName_Command { get; private set; } //NEW METHOD REQUIRED
        public RelayCommand LostFocusName_Command { get; private set; } //NEW METHOD REQUIRED
        public RelayCommand ClearMeContent_Command { get; private set; } //NEW METHOD REQUIRED
        public RelayCommand LostFocusContent_Command { get; private set; } //NEW METHOD REQUIRED

        Model.Task _myTask;
        public Model.Task myTask
        {
            get
            {
                return _myTask;
            }
            set
            {
                _myTask = value;
                NotifyPropertyChanged("myTask");
            }
        }

        Controller.Controller con;
        string startingName;
        Model.Task taskToEdit;
        public frmTaskVM(Model.Task taskToEdit) //breakpoint here as this is a common point of complications
        {

            this.taskToEdit = taskToEdit;
            if (taskToEdit != null) 
            {
                startingName = taskToEdit.Name;
            }

            ChooseDate_Command = new RelayCommand(ChooseDate_Method);
            ChoosePriority_Command = new RelayCommand(ChoosePriority_Method);
            ChooseTags_Command = new RelayCommand(ChooseTags_Method);
            SaveTask_Command = new RelayCommand(SaveTask_Method);
            ExitTask_Command = new RelayCommand(ExitTask_Method);
            ClearMeName_Command = new RelayCommand(ClearMe_Method);
            LostFocusName_Command = new RelayCommand(LostFocus_Method);
            ClearMeContent_Command = new RelayCommand(ClearMeContent_Method);
            LostFocusContent_Command = new RelayCommand(LostFocusContent_Method);

            con = new Controller.Controller();

            //if this is a new task, taskToEdit will be null, therefore I have populated it placeholder text for ergonomics

            if (taskToEdit == null)
            {
                myTask = new Model.Task();
                myTask.Name = "New Task Name";
                myTask.Content = "Add Description";
                myTask.Due = DateTime.Now;
                myTask.StrDue = DateTime.Today.ToShortDateString();
                myTask.StrTag = ";";
                myTask.Priority = new Model.Priority();
                myTask.Priority.Name = "Priority";
            }
            else 
            {
                //While editing, we have taskToEdit which is part of the model the populates the appropriate field with content 
                myTask = taskToEdit;
                myTask.Name = taskToEdit.Name;
                myTask.Content = taskToEdit.Content;
                myTask.StrDue = taskToEdit.StrDue;
                myTask.StrTag = taskToEdit.StrTag;
                myTask.Priority = taskToEdit.Priority;

            }
        }

        //Will add more comments later - pretty self explanatory - most is just for an ergonomic design
        private void LostFocus_Method() //Needs refactoring
        {
            if (String.IsNullOrEmpty(myTask.Name)) { myTask.Name = "New Task Name"; }
        }

        private void ClearMe_Method()
        {
            if (myTask.Name == "New Task Name") { myTask.Name = ""; }
        }
        private void LostFocusContent_Method() //Needs refactoring
        {
            if (String.IsNullOrEmpty(myTask.Content)) { myTask.Content = "Add Description"; }
        }

        private void ClearMeContent_Method()
        {
            if (myTask.Content == "Add Description") { myTask.Content = ""; }
        }

        private void ExitTask_Method()
        {
            SystemVars.FrmTaskWindow.Close();
        }

        private void SaveTask_Method()
        {
            if (myTask == null || String.IsNullOrEmpty(myTask.Name)) 
            {
                MessageBox.Show("Task must have a name");
                return;
            }

            //if (myTask.Priority == null || String.IsNullOrEmpty(myTask.Priority.Name)) 
            //{
            //    MessageBox.Show("Task must have a priority level");
            //    return;
            //}

            if (myTask.Due == null)
            {
                MessageBox.Show("Task must have a due date");
                return;
            }

            if (taskToEdit == null)
            {
                if (!con.createTask(myTask))
                {
                    MessageBox.Show("A Task with this name already exists");
                    return;
                }
            }
            else 
            {
                if (!con.handleTask(myTask, "EDIT", startingName)) 
                {
                    MessageBox.Show("Error! Something has gone wrong during editing");
                    return;
                }
            }
            SystemVars.FrmTaskWindow.Close();
        }

        private void ChooseTags_Method()
        {
            if (myTask == null) { return; }

            View.frmTag window = new View.frmTag();
            SystemVars.FrmTagWindow = window;
            window.ShowDialog();

            myTask.TagList = SystemVars.SelectedTagList;
            myTask.StrTag = createArray(SystemVars.SelectedTagList);
            SystemVars.FrmTagWindow = null;
        }

        private string createArray(ObservableCollection<Tag> selectedTagList)
        {
            try
            {
                string result = "";
                if (selectedTagList != null)
                {
                    foreach (Model.Tag item in selectedTagList)
                    {
                    result += item.Name + ";";
                    }

                }
                

                return result;
            }
            catch (Exception)
            {
                return "";
            }

        }

        private void ChoosePriority_Method()
        {
            if (myTask == null) { return; }
            
            View.frmPriority window = new View.frmPriority();
            SystemVars.FrmPriorityWindow = window;
            window.ShowDialog();

            myTask.Priority = SystemVars.SelectedPriority;
            SystemVars.FrmPriorityWindow = null;
        }

        private void ChooseDate_Method()
        {
            if (myTask == null) { return; }

            View.frmDate window = new View.frmDate();
            SystemVars.FrmDateWindow = window;
            window.ShowDialog();

            myTask.Due = SystemVars.SelectedDateX;
            myTask.StrDue = myTask.Due.ToShortDateString();
            if (myTask.Due == DateTime.Today)
            {
                myTask.StrDue = "Today";
            }
            else if (myTask.Due == DateTime.Today.AddDays(-1))
            {
                myTask.StrDue = "Yesterday";
            }
            else if (myTask.Due == DateTime.Today.AddDays(1))
            {
                myTask.StrDue = "Tomorrow";
            }
            else if (myTask.Due == DateTime.Today.AddDays(2))
            {
                myTask.StrDue = "Day after tomorrow";
            }
            SystemVars.FrmDateWindow = null;
        }
    }
}
