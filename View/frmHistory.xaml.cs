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
    /// Interaction logic for frmHistory.xaml
    /// </summary>
    public partial class frmHistory : Window
    {
        frmHistoryVM vm;
        public frmHistory()
        {
            InitializeComponent();
            vm = new frmHistoryVM();
            this.DataContext = vm;
        }
    }
}
