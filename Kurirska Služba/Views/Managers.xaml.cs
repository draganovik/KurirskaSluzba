using Kurirska_Služba.Controllers;
using Kurirska_Služba.Forms;
using System.Data;
using System.Windows;
using System.Windows.Controls;

namespace Kurirska_Služba.Views
{
    /// <summary>
    /// Interaction logic for Managers.xaml
    /// </summary>
    public partial class Managers : UserControl
    {
        public Managers()
        {
            InitializeComponent();
            ShowData();
        }
        public void ShowData()
        {
            string sqlSelect = @"select '@' + KorisnickoIme as 'Korisničko ime', Ime + ' ' + Prezime as Ime, Lokacija, TelefonskiBroj as 'Broj telefona' from tblMenadzer";
            DataTable dataTable = DatabaseConnection.GetTable(sqlSelect);
            dgManagers.ItemsSource = dataTable.DefaultView;
        }

        private void btnChange_Click(object sender, RoutedEventArgs e)
        {
            if (dgManagers.SelectedItem != null)
            {
                DataRowView selectedRow = (DataRowView)dgManagers.SelectedItems[0];
                WindowManager window = new WindowManager(selectedRow["Korisničko Ime"].ToString()[1..], "KorisnickoIme") { Owner = Application.Current.MainWindow };
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
            if (dgManagers.SelectedItem != null)
            {
                DataRowView item = (DataRowView)dgManagers.SelectedItems[0];
                DatabaseConnection.DeleteByValue(item["Korisničko Ime"].ToString()[1..], "KorisnickoIme", "tblMenadzer");
                ShowData();
            }
            else
            {
                MessageBox.Show("Morate selektovati vrednost iz liste kako bi je obrisali", "Element nije izabran", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void dgManagers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgManagers.SelectedItem != null)
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
