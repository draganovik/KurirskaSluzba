using KurirskaSluzba.Controllers;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Threading;
using System.Windows;
using System.Windows.Documents;

namespace KurirskaSluzba.Forms
{
    /// <summary>
    /// Interaction logic for WindowPackage.xaml
    /// </summary>
    public partial class WindowPackage : Window
    {
        private SqlConnection sqlConnection = new();
        private bool isEdit;
        private int selectedID;
        private readonly int managerID;

        public WindowPackage()
        {
            InitializeComponent();
            tbxName.Focus();
            EnvironmentSetup();
            setType("add");
        }

        public WindowPackage(string managerID) : this()
        {
            this.managerID = Convert.ToInt32(managerID, new CultureInfo("en-US", false));
        }

        public WindowPackage(int selectedID) : this()
        {
            setType("edit");
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
                command.CommandText = @"Select * from tblPosiljka where PosiljkaID= @id";
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    tbxName.Text = reader["Naziv"].ToString();
                    tbxWeight.Text = reader["Tezina"].ToString();
                    cbxCourier.SelectedValue = reader["DodeljenKurirID"];
                    cbxSender.SelectedValue = reader["DodeljenPosiljalacID"];
                    cbxReceiver.SelectedValue = reader["DodeljenPrimalacID"];
                    tbxPickupCity.Text = reader["GradPreuzimanja"].ToString();
                    tbxPickupAddress.Text = reader["AdresaPreuzimanja"].ToString();
                    tbxDropoffCity.Text = reader["GradDostave"].ToString();
                    tbxDropoffAddress.Text = reader["AdresaDostave"].ToString();
                    cbxPostage.SelectedValue = reader["PostarinaID"];
                    tbxRansom.Text = reader["DoplataZaPaket"].ToString()[0..^2];
                    dpDropoffDate.SelectedDate = (DateTime)reader["VremeDostave"];
                    rtbComment.SelectAll();
                    rtbComment.Selection.Text = reader["Napomena"].ToString();
                }
                command.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Nije moguće očitati vrednosti elementa " + ex.Message, "Izmena nije moguća", MessageBoxButton.OK, MessageBoxImage.Error);
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

        private void EnvironmentSetup()
        {
            try
            {
                sqlConnection = DatabaseConnection.CreateConnection();
                sqlConnection.Open();

                // Load Courier Combo Box items
                string sqlCallCouriers = @"select KurirID, Ime + ' ' + Prezime + ' - ' + Lokacija as Kurir from tblKurir";
                DataTable dataTable = new();
                SqlDataAdapter dataAdapter = new(sqlCallCouriers, sqlConnection);
                dataAdapter.Fill(dataTable);
                cbxCourier.ItemsSource = dataTable.DefaultView;
                dataTable.Dispose();
                dataAdapter.Dispose();

                // Load Sender Combo Box items
                string sqlCallSender = @"select KlijentID, Ime + ' ' + Prezime + ' [@' + KorisnickoIme + ']' as Klijent from tblKlijent";
                dataTable = new();
                dataAdapter = new(sqlCallSender, sqlConnection);
                dataAdapter.Fill(dataTable);
                cbxSender.ItemsSource = dataTable.DefaultView;
                dataTable.Dispose();
                dataAdapter.Dispose();

                // Load Reciever Combo Box items
                string sqlCallReceiver = @"select KlijentID, Ime + ' ' + Prezime + ' [@' + KorisnickoIme + ']' as Klijent from tblKlijent";
                dataTable = new();
                dataAdapter = new(sqlCallReceiver, sqlConnection);
                dataAdapter.Fill(dataTable);
                cbxReceiver.ItemsSource = dataTable.DefaultView;
                dataTable.Dispose();
                dataAdapter.Dispose();

                // Load Postage Combo Box items
                string sqlCallPostage = @"select CenaID, Opis + ' - ' + CONVERT(nvarchar, Cena) + ' RSD' as Postarina from tblCenovnik";
                dataTable = new();
                dataAdapter = new(sqlCallPostage, sqlConnection);
                dataAdapter.Fill(dataTable);
                cbxPostage.ItemsSource = dataTable.DefaultView;
                dataTable.Dispose();
                dataAdapter.Dispose();
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

        private void setType(string type)
        {
            switch (type)
            {
                case "edit":
                    Title = "Izmena paketa";
                    btnApply.Content = "Sačuvaj";
                    isEdit = true;
                    break;
                case "add":
                default:
                    Title = "Dodavanje novog paketa";
                    btnApply.Content = "Napravi paket";
                    isEdit = false;
                    break;
            }
        }

        private bool hasValidValues()
        {
            if (!string.IsNullOrEmpty(tbxName.Text) &&
            !string.IsNullOrEmpty(tbxWeight.Text) &&
            cbxCourier.SelectedIndex != -1 &&
            cbxSender.SelectedIndex != -1 &&
            cbxReceiver.SelectedIndex != -1 &&
            !string.IsNullOrEmpty(tbxPickupCity.Text) &&
            !string.IsNullOrEmpty(tbxPickupAddress.Text) &&
            !string.IsNullOrEmpty(tbxDropoffCity.Text) &&
            !string.IsNullOrEmpty(tbxDropoffAddress.Text) &&
            cbxPostage.SelectedIndex != -1 &&
            dpDropoffDate.SelectedDate != null)
            {
                return true;
            }
            MessageBox.Show("Morate popuniti sve informacije osim napomene i naplate za paket.", "Operacija nije sporovedena", MessageBoxButton.OK, MessageBoxImage.Warning);
            return false;
        }

        private int getLatestPackageID()
        {
            int id = -1;
            try
            {
                sqlConnection = DatabaseConnection.CreateConnection();
                sqlConnection.Open();
                SqlCommand command = new SqlCommand
                {
                    Connection = sqlConnection
                };
                sqlConnection = DatabaseConnection.CreateConnection();
                sqlConnection.Open();
                command.CommandText = @"SELECT MAX(PosiljkaID) FROM tblPosiljka";
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    id = Convert.ToInt32(reader[0], new CultureInfo("en-US", false));
                }
                command.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Promene nisu sačuvane zbog sledećeg problema u izvršavanju operacije: \n" + ex.Message, "Operacija je neuspešna", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                if (sqlConnection != null)
                {
                    sqlConnection.Close();
                }
            }
            return id;
        }

        private void createInitialPackageState()
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
                command.Parameters.Add("@Stanje", SqlDbType.Int).Value = 1;
                command.Parameters.Add("@Komentar", SqlDbType.NVarChar).Value = "Paket je kreiran";
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
                {
                    sqlConnection.Close();
                }

                Close();
            }
        }

        private void btnApply_Click(object sender, RoutedEventArgs e)
        {
            if (hasValidValues())
            {
                bool isValid = false;
                try
                {
                    sqlConnection = DatabaseConnection.CreateConnection();
                    sqlConnection.Open();
                    SqlCommand command = new()
                    {
                        Connection = sqlConnection
                    };
                    command.Parameters.Add("@Naziv", SqlDbType.NVarChar).Value = tbxName.Text.Trim();
                    command.Parameters.Add("@Tezina", SqlDbType.Int).Value = tbxWeight.Text.Trim();
                    command.Parameters.Add("@Menadzer", SqlDbType.Int).Value = managerID;
                    command.Parameters.Add("@Kurir", SqlDbType.Int).Value = cbxCourier.SelectedValue;
                    command.Parameters.Add("@Posiljalac", SqlDbType.Int).Value = cbxSender.SelectedValue;
                    command.Parameters.Add("@Primalac", SqlDbType.Int).Value = cbxReceiver.SelectedValue;
                    command.Parameters.Add("@GradPreuzimanja", SqlDbType.NVarChar).Value = tbxPickupCity.Text.Trim();
                    command.Parameters.Add("@AdresaPreuzimanja", SqlDbType.NVarChar).Value = tbxPickupAddress.Text.Trim();
                    command.Parameters.Add("@GradDostave", SqlDbType.NVarChar).Value = tbxDropoffCity.Text.Trim();
                    command.Parameters.Add("@AdresaDostave", SqlDbType.NVarChar).Value = tbxDropoffAddress.Text.Trim();
                    command.Parameters.Add("@Postarina", SqlDbType.Int).Value = cbxPostage.SelectedValue;
                    command.Parameters.Add("@Doplata", SqlDbType.Money).Value = Convert.ToDouble(string.IsNullOrEmpty(tbxRansom.Text.Trim()) ? 0 : tbxRansom.Text.Trim(), new CultureInfo("en-US", false));
                    command.Parameters.Add("@VremeDostave", SqlDbType.DateTime).Value = ((DateTime)dpDropoffDate.SelectedDate).ToString("yyyy-MM-dd", CultureInfo.CurrentCulture);
                    command.Parameters.Add("@Napomena", SqlDbType.NVarChar).Value = new TextRange(rtbComment.Document.ContentStart, rtbComment.Document.ContentEnd).Text.Trim();
                    if (isEdit)
                    {
                        command.Parameters.Add("@id", SqlDbType.Int).Value = selectedID;
                        command.CommandText = @"update tblPosiljka set 
                        Naziv = @Naziv,
                        Tezina = @Tezina,
                        DodeljenPosiljalacID = @Posiljalac,
                        DodeljenPrimalacID = @Primalac,
                        DodeljenKurirID = @Kurir,
                        GradPreuzimanja = @GradPreuzimanja,
                        AdresaPreuzimanja = @AdresaPreuzimanja,
                        GradDostave = @GradDostave,
                        AdresaDostave = @AdresaDostave,
                        VremeDostave = @VremeDostave,
                        PostarinaID = @Postarina,
                        DoplataZaPaket = @Doplata,
                        Napomena = @Napomena
                        where PosiljkaID = @id";
                    }
                    else
                    {
                        command.CommandText =
                            @"insert tblPosiljka (
                        Naziv,
                        Tezina,
                        DodeljenMenadzerID,
                        DodeljenPosiljalacID,
                        DodeljenPrimalacID,
                        DodeljenKurirID,
                        GradPreuzimanja,
                        AdresaPreuzimanja,
                        GradDostave,
                        AdresaDostave,
                        VremeDostave,
                        PostarinaID,
                        DoplataZaPaket,
                        Napomena
                    )values(
                        @Naziv,
                        @Tezina,
                        @Menadzer,
                        @Posiljalac,
                        @Primalac,
                        @Kurir,
                        @GradPreuzimanja,
                        @AdresaPreuzimanja,
                        @GradDostave,
                        @AdresaDostave,
                        @VremeDostave,
                        @Postarina,
                        @Doplata,
                        @Napomena
                    )";
                    }
                    command.ExecuteNonQuery();
                    command.Dispose();
                    if (!isEdit)
                    {
                        isValid = true;
                    }

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
                        if (isValid)
                        {
                            Thread.Sleep(200);
                            selectedID = getLatestPackageID();
                            if (selectedID > -1)
                            {
                                createInitialPackageState();
                            }
                        }
                        Close();
                    }
                }
            }
        }

        private void cbxSender_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            try
            {
                sqlConnection = DatabaseConnection.CreateConnection();
                sqlConnection.Open();
                SqlCommand command = new SqlCommand
                {
                    Connection = sqlConnection
                };
                sqlConnection = DatabaseConnection.CreateConnection();
                sqlConnection.Open();
                command.CommandText = @"SELECT Grad, Adresa FROM tblKlijent where KlijentID =" + cbxSender.SelectedValue;
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    tbxPickupCity.Text = reader["Grad"].ToString();
                    tbxPickupAddress.Text = reader["Adresa"].ToString();

                }
                command.Dispose();
            }
            catch
            {
                MessageBox.Show("Automatsko popunjavanje adrese nije uspelo", "Problem sa bazom podataka", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void cbxReceiver_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            try
            {
                sqlConnection = DatabaseConnection.CreateConnection();
                sqlConnection.Open();
                SqlCommand command = new SqlCommand
                {
                    Connection = sqlConnection
                };
                sqlConnection = DatabaseConnection.CreateConnection();
                sqlConnection.Open();
                command.CommandText = @"SELECT Grad, Adresa FROM tblKlijent where KlijentID =" + cbxReceiver.SelectedValue;
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    tbxDropoffCity.Text = reader["Grad"].ToString();
                    tbxDropoffAddress.Text = reader["Adresa"].ToString();

                }
                command.Dispose();
            }
            catch
            {
                MessageBox.Show("Automatsko popunjavanje adrese nije uspelo", "Problem sa bazom podataka", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void tbxWeight_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            e.Handled = InputValidation.IsNumeric(e.Text);
        }

        private void tbxRansom_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            e.Handled = InputValidation.IsNumeric(e.Text);
        }
    }
}
