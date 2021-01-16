using Kurirska_Služba.Controllers;
using Kurirska_Služba.Forms;
using System.ComponentModel;
using System.Data;
using System.Windows;
using System.Windows.Controls;

namespace Kurirska_Služba.Views
{
    /// <summary>
    /// Interaction logic for PackageHistory.xaml
    /// </summary>
    public partial class PackageHistory : UserControl
    {
        public PackageHistory()
        {
            InitializeComponent();
            ShowData();
        }

        public void ShowData()
        {
            string sqlSelect = @"select StanjePosiljkeID as 'ID stanja', FORMAT(tblStanjePosiljke.Vreme, 'HH:mm / dd.MM.yyyy.', 'sr-Latn-RS') as 'Vreme promene', SVP.NazivStanja as 'Ime trenutnog procesa',
            '#' + CONVERT(nvarchar,posiljka.PosiljkaID) +' - '+ Posiljka.Naziv as 'Ime pošiljke',
            ISNULL(Komentar, 'Nema') as Komentar
            from tblStanjePosiljke
                left join tblPosiljka as Posiljka on tblStanjePosiljke.PosiljkaID = Posiljka.PosiljkaID
                left join tblVrstaStanjaPosiljke as SVP on tblStanjePosiljke.VrstaStanjaID = SVP.VrstaStanjaID";
            DataTable dataTable = DatabaseConnection.GetTable(sqlSelect);
            dgPackageHistory.ItemsSource = dataTable.DefaultView;
            dgPackageHistory.Items.SortDescriptions.Add(new SortDescription("ID stanja", ListSortDirection.Descending));
        }

        private void btnShowPackage_Click(object sender, RoutedEventArgs e)
        {
            if (dgPackageHistory.SelectedItem != null)
            {
                DataRowView item = (DataRowView)dgPackageHistory.SelectedItems[0];
                WindowManagePackage window = new WindowManagePackage(item) { Owner = Application.Current.MainWindow };
                window.ShowDialog();
                ShowData();
            }
            else
            {
                MessageBox.Show("Morate selektovati paket iz liste", "Paket nije izabran", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void dgPackageHistory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgPackageHistory.SelectedItem != null)
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
