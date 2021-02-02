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
    /// Interaction logic for frmData.xaml
    /// </summary>
    public partial class frmData : Window
    {
        frmDataVM vm;
        public frmData()
        {
            vm = new frmDataVM();
            this.DataContext = vm;
            InitializeComponent();
        }
    }
}
