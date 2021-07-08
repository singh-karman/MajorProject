using System;
using System.Windows;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Completist.ViewModel
{
    public class frmDataVM : INotifyPropertyChanged //Property change interface to update XAML render
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }

        public RelayCommand Loaded_Command { get; private set; } //when event loads
        public RelayCommand Priorities_SelectionChanged_Command { get; private set; } //relay command for when a different priority is selected from the listview items
        public RelayCommand Tags_SelectionChanged_Command { get; private set; } //relay command enforced when a change in tag selection is witnessed
        public RelayCommand AddNew_Command { get; private set; } //relay command to add a new priority, initiated through the 'editing' window
        public RelayCommand AddNewTag_Command { get; private set; } //relay command to add a new tag, initiated through the 'editing' window
        public RelayCommand Edit_Command { get; private set; } //relay command to edit an existing priority, initiated through the 'editing' window
        public RelayCommand EditTag_Command { get; private set; } //relay command to edit an existing priority, initiated through the 'editing' window
        public RelayCommand Remove_Command { get; private set; } //relay command to delete an existing priority, initiated through the 'editing' window
        public RelayCommand RemoveTag_Command { get; private set; } //relay command to delete an existing tag, initiated through the 'editing' window

        #region because headache and because observablecollections & models do not need to be tampered with! click to unravel...
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

        ObservableCollection<string> _listOfStatuses;
        public ObservableCollection<string> listOfStatuses 
        {
            get 
            {
                return _listOfStatuses;
            }
            set 
            {
                _listOfStatuses = value;
                NotifyPropertyChanged("listOfStatuses");
            }
        }

        string _selectedPriorityName;
        public string selectedPriorityName
        {
            get
            {
                return _selectedPriorityName;
            }
            set
            {
                _selectedPriorityName = value;
                NotifyPropertyChanged("selectedPriorityName");
            }
        }

        string _selectedTagName;
        public string selectedTagName
        {
            get
            {
                return _selectedTagName;
            }
            set
            {
                _selectedTagName = value;
                NotifyPropertyChanged("selectedTagName");
            }
        }

        string _selectedStatus;
        public string selectedStatus 
        {
            get 
            {
                return _selectedStatus;
            }
            set 
            {
                _selectedStatus = value;
                NotifyPropertyChanged("selectedStatus");
            }
        }
        string _selectedStatus_Tag;
        public string selectedStatus_Tag
        {
            get
            {
                return _selectedStatus_Tag;
            }
            set
            {
                _selectedStatus_Tag = value;
                NotifyPropertyChanged("selectedStatus_Tag");
            }
        }
        Model.AllColors _selectedColor_Tag;
        public Model.AllColors selectedColor_Tag
        {
            get
            {
                return _selectedColor_Tag;
            }
            set
            {
                _selectedColor_Tag = value;
                NotifyPropertyChanged("selectedColor_Tag");
            }
        }
        Model.AllColors _selectedColor;
        public Model.AllColors selectedColor
        {
            get
            {
                return _selectedColor;
            }
            set
            {
                _selectedColor = value;
                NotifyPropertyChanged("selectedColor");
            }
        }
        Model.Priority _selectedPriority;
        public Model.Priority selectedPriority 
        {
            get 
            {
                return _selectedPriority;
            }
            set 
            {
                _selectedPriority = value;
                NotifyPropertyChanged("selectedPriority");
            }
        }
        Model.Tag _selectedTag;
        public Model.Tag selectedTag
        {
            get
            {
                return _selectedTag;
            }
            set
            {
                _selectedTag = value;
                NotifyPropertyChanged("selectedTag");
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

        ObservableCollection<Model.AllColors> _listOfColors;
        public ObservableCollection<Model.AllColors> listOfColors
        {
            get
            {
                return _listOfColors;
            }
            set
            {
                _listOfColors = value;
                NotifyPropertyChanged("listOfColors");
            }
        }
        #endregion
        
        
        Controller.Controller con; //actually important - initialises controller class for DB communication 
        //We can make this read-only

        public frmDataVM()
        {
             
            con = new Controller.Controller();
            
            //Simplifing commands for later - making my code 'atomic' and efficient 
            Loaded_Command = new RelayCommand(Loaded_Method);
            Priorities_SelectionChanged_Command = new RelayCommand(Priorities_SelectionChanged);
            Tags_SelectionChanged_Command = new RelayCommand(Tags_SelectionChanged);

            //Pretty self explanatory how/what the commands are assigned as
            AddNew_Command = new RelayCommand(AddNew);
            AddNewTag_Command = new RelayCommand(AddNewTag);
            Edit_Command = new RelayCommand(Edit);
            EditTag_Command = new RelayCommand(EditTag);
            Remove_Command = new RelayCommand(Remove);
            RemoveTag_Command = new RelayCommand(RemoveTag);
    }
        //important for when the software comes across errors/contradictions in the user's logic
        private void RemoveTag()
        {
            if (selectedTag == null || String.IsNullOrEmpty(selectedTag.Name)) //if statement for null pointer check
            {
                MessageBox.Show("You need to select a tag to remove!"); //ugly box telling you that you missed selecting a tag for removal 
                return;
            }
            if (MessageBox.Show("Remove [" + selectedTag + "]?", "", MessageBoxButton.YesNo) == MessageBoxResult.No) { return; }//confirming with the user that the selected tag is for removal
            bool result = con.removeTag(selectedTag); //addressing the controller and sending the tag selected as a parameter so that it can be deleted from the DB and view 
            if (!result) { MessageBox.Show("Error!"); return; }
            listOfTags = con.returnAllTags("");
        }

        private void Remove()
        {
            if (selectedPriority == null || String.IsNullOrEmpty(selectedPriority.Name)) //if statement for null pointer check
            {
                MessageBox.Show("You need to select a priority to remove!"); //ugly box telling you that you missed selecting a priority for removal 
                return;
            }
            if (MessageBox.Show("Remove [" + selectedPriorityName + "]?","",MessageBoxButton.YesNo) == MessageBoxResult.No) { return; }//confirming with the user that the selected tag is for removal
            bool result = con.removePriority(selectedPriority); //addressing the controller and sending the priority selected as a parameter so that it can be deleted from the DB and view by using the remove command defined earlier 
            if (!result) { MessageBox.Show("Error!"); return; }
            listOfPriorities = con.returnAllPriorities("");
        }

        private void EditTag()
        {
            if (selectedTag == null || String.IsNullOrEmpty(selectedTag.Name))
            {
                MessageBox.Show("You need to select a tag to change!");
                return;
            }

            Model.Tag myTag = new Model.Tag();
            myTag.Name = selectedTagName;
            myTag.Color = selectedColor_Tag.Color;
            myTag.Status = selectedStatus_Tag == "Active" ? myTag.Status = 0 : myTag.Status = 1;

            bool result = con.editTag(myTag);
            if (!result) { MessageBox.Show("Error!"); return; }
            listOfTags = con.returnAllTags("");
        }

        private void Edit()
        {
            if (selectedPriority == null || String.IsNullOrEmpty(selectedPriority.Name)) 
            {
                MessageBox.Show("You need to select a priority to change!");
                return;
            }

            Model.Priority myPriority = new Model.Priority();
            myPriority.Name = selectedPriorityName;
            myPriority.Color = selectedColor.Color;
            myPriority.Status = selectedStatus == "Active" ? myPriority.Status = 0 : myPriority.Status = 1;

            bool result = con.editPriority(myPriority);
            if (!result) { MessageBox.Show("Error!"); return; }
            listOfPriorities = con.returnAllPriorities("");
        }

        private void AddNewTag()
        {
            Model.Tag myTag = new Model.Tag();
            if (!String.IsNullOrWhiteSpace(selectedTagName))
            {
                myTag.Name = selectedTagName;
                myTag.Color = selectedColor_Tag.Color;
                myTag.Status = selectedStatus_Tag == "Active" ? myTag.Status = 0 : myTag.Status = 1;

                bool result = con.createTag(myTag);
                if (!result) { MessageBox.Show("Tag Name Already Exists!"); return; }
                listOfTags = con.returnAllTags("");
            }
            else
            {
                MessageBox.Show("Tag needs to have a name");
            }
        }

        private void AddNew()
        {
            Model.Priority myPriority = new Model.Priority();
            myPriority.Name = selectedPriorityName;
            myPriority.Color = selectedColor.Color;
            myPriority.Status = selectedStatus == "Active" ? myPriority.Status = 0 : myPriority.Status = 1;

            bool result = con.createPriority(myPriority);
            if (!result) { MessageBox.Show("Priority Name Already Exists!"); return; }
            listOfPriorities = con.returnAllPriorities("");
        }

        private void Loaded_Method()
        {
            start_Ini();
        }

        private async void start_Ini()//asynchronous code preventing work queued to halt/hang the software
        {
            await System.Threading.Tasks.Task.Run(() => start());//queues work on thread pool
        }

        private void start()
        {
            try
            {
                listOfPriorities = con.returnAllPriorities("");
                listOfTags = con.returnAllTags("");
                listOfColors = con.returnColors();
                listOfStatuses = new ObservableCollection<string>();
                string s1 = "Active";//string for option
                string s2 = "Inactive"; //string for second option if the user wants to disable the tag/priority
                listOfStatuses.Add(s1);
                listOfStatuses.Add(s2);

                selectedPriority = new Model.Priority(); //when the priority window is called upon, a new instance is initiated, previously selected priority is wiped
                selectedTag = new Model.Tag(); //when the tag window is called upon, a new instance is initiated, previously selected tag(s) is wiped
                selectedColor = new Model.AllColors();
                selectedStatus = "";
            }
            catch (Exception ex)
            {
                string exMessage = ex.Message;
            }
        }

        private void Tags_SelectionChanged()
        {
            if (selectedTag != null)
            {
                selectedTagName = selectedTag.Name;
                selectedColor_Tag = con.returnColor("where COLOR='" + selectedTag.Color + "'");
                selectedStatus_Tag = selectedTag.StrStatus;
            }
        }

        private void Priorities_SelectionChanged()
        {
            if (selectedPriority != null) 
            {
                selectedPriorityName = selectedPriority.Name; //equating priority to that in the priority model
                selectedColor = con.returnColor("where COLOR='" + selectedPriority.Color + "'");
                selectedStatus = selectedPriority.StrStatus; //updated status of priority
            }
        }
    }
}
