using KurirskaSluzba.Controllers;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Windows;
using System.Windows.Documents;

namespace KurirskaSluzba.Forms
{
    /// <summary>
    /// Interaction logic for WindowManagePackage.xaml
    /// </summary>
    public partial class WindowManagePackage : Window
    {
        private SqlConnection sqlConnection = new();
        private int selectedID;
        private string currentStateName;

        public WindowManagePackage()
        {
            InitializeComponent();
            rtbComment.Focus();
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
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item), "Passed DataGridView in a null");
            }
            string packageName = item["Ime pošiljke"].ToString();
            selectedID = Convert.ToInt32(packageName.Substring(1, packageName.IndexOf(' ', StringComparison.Ordinal)), new CultureInfo("en-US", false));
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
                cbxPackageState.ItemsSource = dtState.DefaultView;
                dtState.Dispose();
                sdaCouriers.Dispose();
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
                {
                    sqlConnection.Close();
                }
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
                    cbxPackageState.SelectedValue = reader["VrstaStanjaID"];
                    currentStateName = cbxPackageState.Text;
                }
                if (cbxPackageState.SelectedIndex < cbxPackageState.Items.Count - 2 && cbxPackageState.IsEnabled)
                {
                    cbxPackageState.SelectedIndex++;
                }
                command.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Nije moguće očitati vrednosti elementa " + ex.Message, "Upravljanje nije moguće", MessageBoxButton.OK, MessageBoxImage.Error);
                Close();
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
                    ccManagedCardPackage.Update(
                        Convert.ToInt32(reader["PosiljkaID"], new CultureInfo("en-US", false)),
                        reader["Ime"].ToString(),
                        Convert.ToInt32(reader["Tezina"], new CultureInfo("en-US", false)),
                        reader["Preuzimanje"].ToString(),
                        reader["Dostava"].ToString(),
                        currentStateName);
                    Title = "Pošiljka: " + reader["Ime"].ToString();
                }
                command.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Nije moguće očitati vrednosti elementa " + ex.Message, "Upravljanje nije moguće", MessageBoxButton.OK, MessageBoxImage.Error);
                Close();
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
                command.Parameters.Add("@Posiljka", SqlDbType.Int).Value = selectedID;
                command.Parameters.Add("@Vreme", SqlDbType.DateTime).Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CurrentCulture);
                command.Parameters.Add("@Stanje", SqlDbType.Int).Value = cbxPackageState.SelectedValue;
                command.Parameters.Add("@Komentar", SqlDbType.NVarChar).Value = new TextRange(rtbComment.Document.ContentStart, rtbComment.Document.ContentEnd).Text.Trim();
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
                sqlConnection.Dispose();
                if (sqlConnection != null)
                {
                    sqlConnection.Close();
                    Close();
                }
            }
        }
    }
}
