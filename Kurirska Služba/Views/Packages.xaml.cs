using Kurirska_Služba.CustomControls;
using Kurirska_Služba.Forms;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Kurirska_Služba.Views
{
    /// <summary>
    /// Interaction logic for Packages.xaml
    /// </summary>
    public partial class Packages : UserControl
    {
        public Packages()
        {
            InitializeComponent();
            ShowData();
        }

        #region Mouse wheel scroll in list view
        private void svContainer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            ScrollViewer scv = (ScrollViewer)sender;
            scv.ScrollToVerticalOffset(scv.VerticalOffset - e.Delta);
            e.Handled = true;
        }
        #endregion

        public void ShowData()
        {
            // TODO: Implement latest package status in the list UI
            string sqlSelectCouriers = @"select PosiljkaID, '#' + CONVERT(nvarchar, PosiljkaID) + ' - ' + Naziv as Ime, Tezina, GradPreuzimanja + ', ' + AdresaPreuzimanja as Preuzimanje, GradDostave + ', ' + AdresaDostave as Dostava from tblPosiljka";
            DataTable dtCouriers = DatabaseConnection.GetTable(sqlSelectCouriers);
            lvPackages.Items.Clear();
            foreach (DataRow row in dtCouriers.Rows)
            {
                lvPackages.Items.Insert(0, new CardPackage(Convert.ToInt32(row["PosiljkaID"].ToString()), row["Ime"].ToString(), Convert.ToInt32(row["Tezina"].ToString()), row["Preuzimanje"].ToString(), row["Dostava"].ToString(), "TODO"));
            }
        }

        private void eraceStateHistory(int packageID)
        {
            DatabaseConnection.DeleteByValue(packageID.ToString(), "PosiljkaID", "tblStanjePosiljke");
        }
        private void btnChange_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (lvPackages.SelectedItem != null)
            {
                CardPackage selectedPackage = (CardPackage)lvPackages.SelectedItems[0];
                WindowPackage window = new WindowPackage(selectedPackage.getID()) { Owner = Application.Current.MainWindow };
                window.ShowDialog();
                ShowData();
            }
            else
            {
                MessageBox.Show("Morate selektovati vrednost iz liste kako bi je menjali", "Element nije izabran", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void btnDelete_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            CardPackage card = (CardPackage)lvPackages.SelectedItems[0];
            int id = card.getID();
            eraceStateHistory(id);
            DatabaseConnection.DeleteByIdUnsafe(id.ToString(), "PosiljkaID", "tblPosiljka");
            ShowData();
        }

        private void lvPackages_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lvPackages.SelectedItem != null)
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

        private void btnManage_Click(object sender, RoutedEventArgs e)
        {
            if (lvPackages.SelectedItem != null)
            {
                CardPackage selectedPackage = (CardPackage)lvPackages.SelectedItems[0];
                WindowManagePackage window = new WindowManagePackage(selectedPackage.getID()) { Owner = Application.Current.MainWindow };
                window.ShowDialog();
                ShowData();
            }
            else
            {
                MessageBox.Show("Morate selektovati paket iz liste", "Paket nije izabran", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
