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
using Completist.ViewModel;

namespace Completist.View
{
    /// <summary>
    /// Interaction logic for frmDate.xaml
    /// </summary>
    public partial class frmDate : Window
    {
        frmDateVM vm;
        public frmDate()
        {
            vm = new frmDateVM();
            this.DataContext = vm;
            InitializeComponent();
        }
    }
}
