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
    public class frmPriorityVM : INotifyPropertyChanged
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
        public RelayCommand lstSelectionChanged_Command { get; private set; }

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

        Controller.Controller con;
        public frmPriorityVM()
        {
            Loaded_Command = new RelayCommand(Loaded_Method);
            lstSelectionChanged_Command = new RelayCommand(lstSelectionChanged_Method);
            con = new Controller.Controller();
            
        }

        private void lstSelectionChanged_Method()
        {

            if (selectedPriority == null) { return; }

            SystemVars.SelectedPriority = selectedPriority;
            SystemVars.FrmPriorityWindow.Close();
        }

        private void Loaded_Method()
        {
            listOfPriorities = con.returnAllPriorities(" where sts=0");
        }
    }
}
