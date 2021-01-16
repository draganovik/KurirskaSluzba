using KurirskaSluzba.Controllers;
using KurirskaSluzba.CustomControls;
using KurirskaSluzba.Forms;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace KurirskaSluzba.Views
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

        public void ShowData()
        {
            string sqlSelect = @"select PosiljkaID, '#' + CONVERT(nvarchar, PosiljkaID) + ' - ' + Naziv as Ime, Tezina, GradPreuzimanja + ', ' + AdresaPreuzimanja as Preuzimanje, GradDostave + ', ' + AdresaDostave as Dostava from tblPosiljka";
            DataTable dataTable = DatabaseConnection.GetTable(sqlSelect);
            lvPackages.Items.Clear();
            foreach (DataRow row in dataTable.Rows)
            {
                lvPackages.Items.Insert(0, new CardPackage(Convert.ToInt32(row["PosiljkaID"].ToString(), new CultureInfo("en-US", false)), row["Ime"].ToString(), Convert.ToInt32(row["Tezina"].ToString(), new CultureInfo("en-US", false)), row["Preuzimanje"].ToString(), row["Dostava"].ToString(), getPackageState(Convert.ToInt32(row["PosiljkaID"], new CultureInfo("en-US", false)))));
            }
            dataTable.Dispose();
        }

        private static string getPackageState(int id)
        {
            SqlConnection sqlConnection = null;
            string historyValue = "NULL";
            try
            {
                sqlConnection = DatabaseConnection.CreateConnection();
                sqlConnection.Open();
                SqlCommand command = new SqlCommand
                {
                    Connection = sqlConnection
                };
                command.Parameters.Add("@id", SqlDbType.Int).Value = id;
                command.CommandText = @"select tblVrstaStanjaPosiljke.NazivStanja from tblStanjePosiljke
                                        join tblVrstaStanjaPosiljke on tblStanjePosiljke.VrstaStanjaID = tblVrstaStanjaPosiljke.VrstaStanjaID
                                        where PosiljkaID = @id";
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    historyValue = reader["NazivStanja"].ToString();
                }
                command.Dispose();
            }
            finally
            {
                sqlConnection.Dispose();
                if (sqlConnection != null)
                {
                    sqlConnection.Close();
                }
            }
            return historyValue;
        }

        private static void eraceStateHistory(int packageID)
        {
            DatabaseConnection.DeleteByValue(packageID.ToString(new CultureInfo("en-US", false)), "PosiljkaID", "tblStanjePosiljke");
        }

        private void btnManage_Click(object sender, RoutedEventArgs e)
        {
            if (lvPackages.SelectedItem != null)
            {
                CardPackage selectedPackage = (CardPackage)lvPackages.SelectedItems[0];
                WindowManagePackage window = new WindowManagePackage(selectedPackage.ID) { Owner = Application.Current.MainWindow };
                window.ShowDialog();
                ShowData();
            }
            else
            {
                MessageBox.Show("Morate selektovati paket iz liste", "Paket nije izabran", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void btnChange_Click(object sender, RoutedEventArgs e)
        {
            if (lvPackages.SelectedItem != null)
            {
                CardPackage selectedPackage = (CardPackage)lvPackages.SelectedItems[0];
                WindowPackage window = new WindowPackage(selectedPackage.ID) { Owner = Application.Current.MainWindow };
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
            CardPackage selectedCard = (CardPackage)lvPackages.SelectedItems[0];
            int id = selectedCard.ID;
            eraceStateHistory(id);
            DatabaseConnection.DeleteByIdUnsafe(id.ToString(new CultureInfo("en-US", false)), "PosiljkaID", "tblPosiljka");
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

        #region Mouse wheel scroll in list view
        private void svContainer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            ScrollViewer scv = (ScrollViewer)sender;
            scv.ScrollToVerticalOffset(scv.VerticalOffset - e.Delta);
            e.Handled = true;
        }
        #endregion
    }
}
