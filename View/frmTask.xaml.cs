using Completist.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Completist.View
{
    /// <summary>
    /// Interaction logic for frmTask.xaml
    /// </summary>
    public partial class frmTask : Window
    {
        frmTaskVM vm;
        Model.Task taskToEdit;
        public frmTask(Model.Task taskToEdit)
        {
            InitializeComponent();
            this.taskToEdit = taskToEdit;
            vm = new frmTaskVM(taskToEdit);
            this.DataContext = vm;
        }
    }
}
