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
    /// Interaction logic for frmPriority.xaml
    /// </summary>
    public partial class frmPriority : Window
    {
        frmPriorityVM vm;
        public frmPriority()
        {
            InitializeComponent();
            vm = new frmPriorityVM();
            this.DataContext = vm;
        }
    }
}
