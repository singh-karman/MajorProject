using Completist.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for frmTag.xaml
    /// </summary>
    public partial class frmTag : Window
    {
        frmTagVM vm;
        public frmTag()
        {
            InitializeComponent();
            vm = new frmTagVM();
            this.DataContext = vm;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ObservableCollection<Model.Tag> mySelectedItems = new ObservableCollection<Model.Tag>();

            foreach (Model.Tag item in lst.SelectedItems)
            {
                mySelectedItems.Add(item);
            }

            vm.myMethod(mySelectedItems);
        }
    }
}
