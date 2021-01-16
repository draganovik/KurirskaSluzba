using KurirskaSluzba.Controllers;
using KurirskaSluzba.Forms;
using System.Data;
using System.Windows;
using System.Windows.Controls;

namespace KurirskaSluzba.Views
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
            string sqlSelect = @"select '@' + KorisnickoIme as 'Korisničko ime', Ime + ' ' + Prezime as Ime, Lokacija, TelefonskiBroj as 'Broj telefona' from tblKurir";
            DataTable dataTable = DatabaseConnection.GetTable(sqlSelect);
            dgCouriers.ItemsSource = dataTable.DefaultView;
            dataTable.Dispose();
        }

        private void btnChange_Click(object sender, RoutedEventArgs e)
        {
            if (dgCouriers.SelectedItem != null)
            {
                DataRowView selectedRow = (DataRowView)dgCouriers.SelectedItems[0];
                WindowCourier window = new WindowCourier(selectedRow["Korisničko ime"].ToString()[1..]) { Owner = Application.Current.MainWindow };
                window.ShowDialog();
                ShowData();
            }
            else
            {
                MessageBox.Show("Morate selektovati vrednost iz liste kako bi je menjali", "Element nije izabran", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (dgCouriers.SelectedItem != null)
            {
                DataRowView selectedRow = (DataRowView)dgCouriers.SelectedItems[0];
                DatabaseConnection.DeleteByValue(selectedRow["Korisničko ime"].ToString()[1..], "KorisnickoIme", "tblKurir");
                ShowData();
            }
            else
            {
                MessageBox.Show("Morate selektovati vrednost iz liste kako bi je obrisali", "Element nije izabran", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void dgCouriers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgCouriers.SelectedItem != null)
            {
                foreach (Button button in spControls.Children)
                {
                    button.IsEnabled = true;
                }
            }
            else
            {
                foreach (Button button in spControls.Children)
                {
                    button.IsEnabled = true;
                }
            }
        }
    }
}
