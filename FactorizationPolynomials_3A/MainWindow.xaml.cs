using System.Windows;
using System.ComponentModel;
using System.Windows.Controls;

namespace FactorizationPolynomials_3A
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private void DataGrid_Sorting(object sender, DataGridSortingEventArgs e)
        {
            //e.Column.SortDirection = ListSortDirection.Descending;
        }
    }
}
