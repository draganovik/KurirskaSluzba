using System;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Documents;

namespace Kurirska_Služba.Forms
{
    /// <summary>
    /// Interaction logic for WindowPackage.xaml
    /// </summary>
    public partial class WindowPackage : Window
    {
        SqlConnection sqlConnection = new();
        bool isEdit = false;
        int selectedID;
        public WindowPackage()
        {
            InitializeComponent();
            tbxName.Focus();
            EnvironmentSetup();
            setType("add");
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
            }
            catch (Exception ex)
            {
                MessageBox.Show("Nije moguće očitati vrednosti elementa " + ex.Message, "Izmena nije moguća", MessageBoxButton.OK, MessageBoxImage.Error);
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

        private void EnvironmentSetup()
        {
            try
            {
                sqlConnection = DatabaseConnection.CreateConnection();
                sqlConnection.Open();
                // Load Courier Combo Box items
                string sqlCallCouriers = @"select KurirID, Ime + ' ' + Prezime + ' - ' + Lokacija as Kurir from tblKurir";
                DataTable dtCouriers = new();
                SqlDataAdapter sdaCouriers = new(sqlCallCouriers, sqlConnection);
                sdaCouriers.Fill(dtCouriers);
                cbxCourier.ItemsSource = dtCouriers.DefaultView;
                // Load Sender Combo Box items
                string sqlCallSender = @"select KlijentID, Ime + ' ' + Prezime + ' [@' + KorisnickoIme + ']' as Klijent from tblKlijent";
                DataTable dtSender = new();
                SqlDataAdapter sdaSender = new(sqlCallSender, sqlConnection);
                sdaSender.Fill(dtSender);
                cbxSender.ItemsSource = dtSender.DefaultView;
                // Load Reciever Combo Box items
                string sqlCallReceiver = @"select KlijentID, Ime + ' ' + Prezime + ' [@' + KorisnickoIme + ']' as Klijent from tblKlijent";
                DataTable dtReceiver = new();
                SqlDataAdapter sdaReceiver = new(sqlCallReceiver, sqlConnection);
                sdaReceiver.Fill(dtReceiver);
                cbxReceiver.ItemsSource = dtReceiver.DefaultView;
                // Load Postage Combo Box items
                string sqlCallPostage = @"select CenaID, Opis + ' - ' + CONVERT(nvarchar, Cena) + ' RSD' as Postarina from tblCenovnik";
                DataTable dtPostage = new();
                SqlDataAdapter sdaPostage = new(sqlCallPostage, sqlConnection);
                sdaPostage.Fill(dtPostage);
                cbxPostage.ItemsSource = dtPostage.DefaultView;
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
        private void setType(String type)
        {
            switch (type)
            {
                case "edit":
                    this.Title = "Izmena paketa";
                    btnApply.Content = "Sačuvaj";
                    isEdit = true;
                    break;
                case "add":
                    this.Title = "Dodavanje novog paketa";
                    btnApply.Content = "Napravi paket";
                    isEdit = false;
                    break;
            }
        }

        private void btnApply_Click(object sender, RoutedEventArgs e)
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
                command.Parameters.Add("@Naziv", System.Data.SqlDbType.NVarChar).Value = tbxName.Text;
                command.Parameters.Add("@Tezina", System.Data.SqlDbType.Int).Value = tbxWeight.Text;
                // TODO: Implement ManagerID
                command.Parameters.Add("@Menadzer", System.Data.SqlDbType.Int).Value = 1;
                command.Parameters.Add("@Kurir", System.Data.SqlDbType.Int).Value = cbxCourier.SelectedValue;
                command.Parameters.Add("@Posiljalac", System.Data.SqlDbType.Int).Value = cbxSender.SelectedValue;
                command.Parameters.Add("@Primalac", System.Data.SqlDbType.Int).Value = cbxReceiver.SelectedValue;
                command.Parameters.Add("@GradPreuzimanja", System.Data.SqlDbType.NVarChar).Value = tbxPickupCity.Text;
                command.Parameters.Add("@AdresaPreuzimanja", System.Data.SqlDbType.NVarChar).Value = tbxPickupAddress.Text;
                command.Parameters.Add("@GradDostave", System.Data.SqlDbType.NVarChar).Value = tbxDropoffCity.Text;
                command.Parameters.Add("@AdresaDostave", System.Data.SqlDbType.NVarChar).Value = tbxDropoffAddress.Text;
                command.Parameters.Add("@Postarina", System.Data.SqlDbType.Int).Value = cbxPostage.SelectedValue;
                command.Parameters.Add("@Doplata", System.Data.SqlDbType.Money).Value = Convert.ToDouble(tbxRansom.Text == "" ? 0 : tbxRansom.Text);
                // TODO: Implement DateTime Picker
                command.Parameters.Add("@VremeDostave", System.Data.SqlDbType.DateTime).Value = ((DateTime)dpDropoffDate.SelectedDate).ToString("yyyy-MM-dd", CultureInfo.CurrentCulture);
                command.Parameters.Add("@Napomena", System.Data.SqlDbType.NVarChar).Value = new TextRange(rtbComment.Document.ContentStart, rtbComment.Document.ContentEnd).Text.Trim();
                if (isEdit)
                {
                    command.Parameters.Add("@id", System.Data.SqlDbType.Int).Value = selectedID;
                    command.CommandText = @"Update tblPosiljka set 
                        Naziv = @Naziv,
                        Tezina = @Tezina,
                        DodeljenMenadzerID = @Menadzer,
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
                if (sqlConnection != null)
                {
                    sqlConnection.Close();
                    if (isValid)
                    {
                        Thread.Sleep(200);
                        this.selectedID = getLatestPackageID();
                        if (selectedID > -1)
                            createInitialPackageState();
                    }
                }
                this.Close();
            }
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
                command.Parameters.Add("@Posiljka", System.Data.SqlDbType.Int).Value = selectedID;
                command.Parameters.Add("@Vreme", System.Data.SqlDbType.DateTime).Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CurrentCulture);
                command.Parameters.Add("@Stanje", System.Data.SqlDbType.Int).Value = 1;
                command.Parameters.Add("@Komentar", System.Data.SqlDbType.NVarChar).Value = "Paket je kreiran";
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
                    id = Convert.ToInt32(reader[0]);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Promene nisu sačuvane zbog sledećeg problema u izvršavanju operacije: \n" + ex.Message, "Operacija je neuspešna", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                if (sqlConnection != null)
                    sqlConnection.Close();
            }
            return id;
        }

        private void ResetInput()
        {
            tbxName.Text = "";
            tbxWeight.Text = "";
            cbxCourier.SelectedIndex = -1;
            cbxSender.SelectedIndex = -1;
            cbxReceiver.SelectedIndex = -1;
            tbxPickupCity.Text = "";
            tbxPickupAddress.Text = "";
            tbxDropoffCity.Text = "";
            tbxDropoffAddress.Text = "";
            cbxPostage.SelectedIndex = -1;
            tbxRansom.Text = "";
            dpDropoffDate.SelectedDate = null;
            rtbComment.Document = new();
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
            }
            catch
            {
                MessageBox.Show("Automatsko popunjavanje adrese nije uspelo", "Problem sa bazom podataka", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
