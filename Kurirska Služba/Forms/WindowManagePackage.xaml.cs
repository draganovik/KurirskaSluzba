using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
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
using System.Windows.Shapes;

namespace Kurirska_Služba.Forms
{
    /// <summary>
    /// Interaction logic for WindowManagePackage.xaml
    /// </summary>
    public partial class WindowManagePackage : Window
    {
        SqlConnection sqlConnection = new();
        private int selectedID;
        private string currentStateName;

        public WindowManagePackage()
        {
            InitializeComponent();
        }

        public WindowManagePackage(int selectedID) : this()
        {
            this.selectedID = selectedID;
            EnvironmentSetup();
            FillDataPackageState(this.selectedID);
            FillDataPackage(this.selectedID);
        }

        public WindowManagePackage(DataRowView item) : this()
        {
            EnvironmentSetup();
            var packageName = item["Ime pošiljke"].ToString();
            this.selectedID = Convert.ToInt32(packageName.Substring(1, packageName.IndexOf(' ')));
            cbxPackageState.IsEnabled = false;
            FillDataPackageState(selectedID);
            FillDataPackage(selectedID);
            cbxPackageState.Text = item["Ime trenutnog procesa"].ToString();
            tbxStateDate.Text = item["Vreme promene"].ToString();
            grdTime.Visibility = Visibility.Visible;
            btnApply.Visibility = Visibility.Collapsed;
            lbState.Content = "Stanje pošiljke";
            
            rtbComment.SelectAll();
            rtbComment.Selection.Text = item["Komentar"].ToString();
            rtbComment.IsEnabled = false;
        }

        private void EnvironmentSetup()
        {
            try
            {
                sqlConnection = DatabaseConnection.CreateConnection();
                sqlConnection.Open();
                // Load State Combo Box items
                string sqlCallState = @"select * from tblVrstaStanjaPosiljke";
                DataTable dtState = new();
                SqlDataAdapter sdaCouriers = new(sqlCallState, sqlConnection);
                sdaCouriers.Fill(dtState);
                this.cbxPackageState.ItemsSource = dtState.DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Nije moguće povezati se sa bazom podataka: " + ex.Message, "Problem sa bazom podataka");
            }
            finally
            {
                // Dispose and Close connection
                sqlConnection.Dispose();
                if (sqlConnection != null)
                    sqlConnection.Close();
            }
        }

        private void FillDataPackageState(int selectedID)
        {
            try
            {
                sqlConnection = DatabaseConnection.CreateConnection();
                sqlConnection.Open();
                SqlCommand command = new SqlCommand
                {
                    Connection = sqlConnection
                };
                command.Parameters.Add("@id", SqlDbType.Int).Value = selectedID;
                this.selectedID = selectedID;
                command.CommandText = @"select * from tblStanjePosiljke where PosiljkaID = @id";
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    this.cbxPackageState.SelectedValue = reader["VrstaStanjaID"];
                    currentStateName = cbxPackageState.Text;
                }
                if (this.cbxPackageState.SelectedIndex < cbxPackageState.Items.Count - 2 && cbxPackageState.IsEnabled)
                {
                    this.cbxPackageState.SelectedIndex++;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Nije moguće očitati vrednosti elementa " + ex.Message, "Upravljanje nije moguće", MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
            }
            finally
            {
                if (sqlConnection != null)
                {
                    sqlConnection.Close();
                }
            }
        }

        public void FillDataPackage(int selectedID)
        {
            try
            {
                sqlConnection = DatabaseConnection.CreateConnection();
                sqlConnection.Open();
                SqlCommand command = new SqlCommand
                {
                    Connection = sqlConnection
                };
                command.Parameters.Add("@id", SqlDbType.Int).Value = selectedID;
                this.selectedID = selectedID;
                command.CommandText = @"select PosiljkaID, '#' + CONVERT(nvarchar, PosiljkaID) + ' - ' + Naziv as Ime, Tezina, GradPreuzimanja + ', ' + AdresaPreuzimanja as Preuzimanje, GradDostave + ', ' + AdresaDostave as Dostava from tblPosiljka where PosiljkaID= @id";
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    this.ccManagedCardPackage.Update(
                        Convert.ToInt32(reader["PosiljkaID"]),
                        reader["Ime"].ToString(),
                        Convert.ToInt32(reader["Tezina"]),
                        reader["Preuzimanje"].ToString(),
                        reader["Dostava"].ToString(),
                        currentStateName);
                    this.Title = "Pošiljka: " + reader["Ime"].ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Nije moguće očitati vrednosti elementa " + ex.Message, "Upravljanje nije moguće", MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
            }
            finally
            {
                if (sqlConnection != null)
                {
                    sqlConnection.Close();
                }
            }
        }

        private void btnApply_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                sqlConnection = DatabaseConnection.CreateConnection();
                sqlConnection.Open();
                SqlCommand command = new()
                {
                    Connection = sqlConnection
                };
                command.Parameters.Add("@Posiljka", System.Data.SqlDbType.Int).Value = selectedID;
                command.Parameters.Add("@Vreme", System.Data.SqlDbType.DateTime).Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CurrentCulture);
                command.Parameters.Add("@Stanje", System.Data.SqlDbType.Int).Value = cbxPackageState.SelectedValue;
                command.Parameters.Add("@Komentar", System.Data.SqlDbType.NVarChar).Value = new TextRange(rtbComment.Document.ContentStart, rtbComment.Document.ContentEnd).Text.Trim();
                command.CommandText =
                        @"insert tblStanjePosiljke (
                        PosiljkaID,
                        VrstaStanjaID,
                        Komentar,
                        Vreme
                    )values(
                        @Posiljka,
                        @Stanje,
                        @Komentar,
                        @Vreme
                    )";
                command.ExecuteNonQuery();
                command.Dispose();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Promene nisu sačuvane zbog sledećeg problema u izvršavanju operacije: \n" + ex.Message, "Operacija je neuspešna", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                if (sqlConnection != null)
                    sqlConnection.Close();
                this.Close();
            }
        }
    }
}
