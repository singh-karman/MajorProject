using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Completist.ViewModel
{

    public class frmDateVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }

        DateTime _selectedDueDate;
        public DateTime selectedDueDate
        {
            get 
            {
                return _selectedDueDate;
            }
            set 
            {
                _selectedDueDate = value;
                NotifyPropertyChanged("selectedDueDate");
            }
        }

        public RelayCommand SelectDate_Command { get; set; }

        public frmDateVM()
        {
            SelectDate_Command = new RelayCommand(SelectDate_Method);
        }

        private void SelectDate_Method()
        {
            if (selectedDueDate != null) 
            {
                //preserves values lost after window is closed
                SystemVars.SelectedDateX = selectedDueDate;
                SystemVars.FrmDateWindow.Close();
            }
        }
    }
}
