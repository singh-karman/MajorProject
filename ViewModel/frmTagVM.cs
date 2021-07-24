using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Completist.ViewModel
{
    //this is the ViewModel for the tag class
    //contributes soley to populate observable collection of tags so that it can be accessed by other ViewModels
    public class frmTagVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }

        public RelayCommand Loaded_Command { get; private set; }

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

        ObservableCollection<Model.Tag> _selectedTags;
        public ObservableCollection<Model.Tag> selectedTags
        {
            get
            {
                return _selectedTags;
            }
            set
            {
                _selectedTags = value;
                NotifyPropertyChanged("selectedTags");
            }
        }

        Controller.Controller con;

        public frmTagVM()
        {
            Loaded_Command = new RelayCommand(Loaded_Method);
            con = new Controller.Controller();
        }

        public void myMethod(ObservableCollection<Model.Tag> mySelectedItems)
        {

            if (mySelectedItems == null) { return; }

            SystemVars.SelectedTagList = mySelectedItems;
            SystemVars.FrmTagWindow.Close();
        }

        private void Loaded_Method()
        {
            listOfTags = con.returnAllTags(" where sts=0"); //override listOfTags by returned observable collection of tags retrieved from DB
        }
    }
}
