using Kurirska_Služba.CustomControls;
using System;
using System.Data;
using System.Data.SqlClient;
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
        public void ShowData()
        {
            // TODO: Implement latest package status in the list UI
            string sqlSelectCouriers = @"select PosiljkaID, '#' + CONVERT(nvarchar, PosiljkaID) + ' - ' + Naziv as Ime, Tezina, GradPreuzimanja + ', ' + AdresaPreuzimanja as Preuzimanje, GradDostave + ', ' + AdresaDostave as Dostava from tblPosiljka";
            DataTable dtCouriers = DatabaseConnection.GetTable(sqlSelectCouriers);
            foreach (DataRow row in dtCouriers.Rows)
            {
                lvPackages.Items.Add(new CardPackage(Convert.ToInt32(row["PosiljkaID"].ToString()), row["Ime"].ToString(), Convert.ToInt32(row["Tezina"].ToString()), row["Preuzimanje"].ToString(), row["Dostava"].ToString(), "TODO"));
            }
        }
    }
}
