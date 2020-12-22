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
        SqlConnection sqlConnection = new();
        public Packages()
        {
            InitializeComponent();
            ShowData();
        }

        private void svContainer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            ScrollViewer scv = (ScrollViewer)sender;
            scv.ScrollToVerticalOffset(scv.VerticalOffset - e.Delta);
            e.Handled = true;
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
        public void ShowData()
        {
            sqlConnection = DatabaseConnection.CreateConnection();
            sqlConnection.Open();
            // TODO: Implement latest package status in the list UI
            string sqlCallCouriers = @"select PosiljkaID, '#' + CONVERT(nvarchar, PosiljkaID) + ' - ' + Naziv as Ime, GradPreuzimanja + ', ' + AdresaPreuzimanja as Preuzimanje, GradDostave + ', ' + AdresaDostave as Dostava from tblPosiljka";
            DataTable dtCouriers = new();
            SqlDataAdapter sdaCouriers = new(sqlCallCouriers, sqlConnection);
            sdaCouriers.Fill(dtCouriers);
            foreach (DataRow row in dtCouriers.Rows)
            {
                lvPackages.Items.Add(new CardPackage(Convert.ToInt32(row["PosiljkaID"].ToString()), row["Ime"].ToString(), row["Preuzimanje"].ToString(), row["Dostava"].ToString(), "TODO"));
            }
            sqlConnection.Dispose();
            sqlConnection.Close();
        }
    }
}
