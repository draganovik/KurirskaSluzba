using System;
using System.Collections.Generic;
using System.Data;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Kurirska_Služba.Views
{
    /// <summary>
    /// Interaction logic for Couriers.xaml
    /// </summary>
    public partial class Couriers : UserControl
    {
        public Couriers()
        {
            InitializeComponent();
            ShowData();
        }
        public void ShowData()
        {
            string sqlSelect = @"select Ime + ' ' + Prezime as Ime, Lokacija from tblKurir";
            DataTable dataTable = DatabaseConnection.GetTable(sqlSelect);
            dgCouriers.ItemsSource = dataTable.DefaultView;
        }
    }
}
