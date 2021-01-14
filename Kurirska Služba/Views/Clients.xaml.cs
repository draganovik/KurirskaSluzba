using Kurirska_Služba.Forms;
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
    /// Interaction logic for Clients.xaml
    /// </summary>
    public partial class Clients : UserControl
    {
        public Clients()
        {
            InitializeComponent();
            ShowData();
        }
        public void ShowData()
        {
            string sqlSelect = @"select '@' + KorisnickoIme as 'Korisničko ime', Ime + ' ' +Prezime as 'Ime', Grad, Adresa as Ulica, TelefonskiBroj as 'Broj telefona' from tblKlijent";
            DataTable dataTable = DatabaseConnection.GetTable(sqlSelect);
            dgClients.ItemsSource = dataTable.DefaultView;
        }

        private void btnChange_Click(object sender, RoutedEventArgs e)
        {
            if (dgClients.SelectedItem != null)
            {
                DataRowView selectedRow = (DataRowView)dgClients.SelectedItems[0];
                WindowClient window = new WindowClient(selectedRow["Korisničko ime"].ToString()[1..]) { Owner = Application.Current.MainWindow };
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
            if (dgClients.SelectedItem != null)
            {
                DataRowView id = (DataRowView)dgClients.SelectedItems[0];
                DatabaseConnection.DeleteByValue(id["Korisničko ime"].ToString()[1..], "KorisnickoIme", "tblKlijent");
                ShowData();
            }
            else
            {
                MessageBox.Show("Morate selektovati vrednost iz liste kako bi je obrisali", "Element nije izabran", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void dgClients_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgClients.SelectedItem != null)
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
