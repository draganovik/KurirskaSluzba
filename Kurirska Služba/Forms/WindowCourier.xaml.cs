using Kurirska_Služba.Controllers;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Input;

namespace Kurirska_Služba.Forms
{
    /// <summary>
    /// Interaction logic for WindowCourier.xaml
    /// </summary>
    public partial class WindowCourier : Window
    {
        SqlConnection sqlConnection = new();
        bool isEdit = false;
        string selectedID;
        public WindowCourier()
        {
            InitializeComponent();
            setType("add");
        }
        public WindowCourier(String selectedID)
        {
            InitializeComponent();
            setType("edit");
            tbxPassword.IsEnabled = false;
            try
            {
                sqlConnection = DatabaseConnection.CreateConnection();
                sqlConnection.Open();
                SqlCommand command = new SqlCommand
                {
                    Connection = sqlConnection
                };
                command.Parameters.Add("@id", SqlDbType.NVarChar).Value = selectedID;
                this.selectedID = selectedID;
                command.CommandText = @"Select * from tblKurir where KorisnickoIme= @id";
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    tbxName.Text = reader["Ime"].ToString();
                    tbxSurname.Text = reader["Prezime"].ToString();
                    tbxPhoneNumber.Text = reader["TelefonskiBroj"].ToString();
                    tbxLocation.Text = reader["Lokacija"].ToString();
                    tbxUsername.Text = reader["KorisnickoIme"].ToString();
                    tbxPassword.Password = "";
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

        private void setType(String type)
        {
            switch (type)
            {
                case "edit":
                    this.Title = "Izmena podataka kurira";
                    btnApply.Content = "Sačuvaj";
                    isEdit = true;
                    break;
                case "add":
                    this.Title = "Dodavanje novog kurira";
                    btnApply.Content = "Napravi nalog";
                    isEdit = false;
                    break;
            }

        }

        private bool hasValidValues()
        {
            bool hasValidUsername = DatabaseConnection.IsUniqueValue(tbxUsername.Text, "KorisnickoIme", "tblKurir");
            if (tbxName.Text.Trim() != "" &&
            tbxSurname.Text.Trim() != "" &&
            tbxPhoneNumber.Text.Trim() != "" &&
            tbxLocation.Text.Trim() != "" &&
            tbxUsername.Text != "" &&
            tbxPassword.Password != "" && hasValidUsername)
            {
                return true;
            }
            if (hasValidUsername || tbxUsername.Text.Trim() == "")
            {
                MessageBox.Show("Morate popuniti sve informacije.", "Operacija nije sporovedena", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                MessageBox.Show("Korisničko ime je zauzeto, probajte drugu kombinaciju.", "Operacija nije sporovedena", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            return false;
        }

        private void btnApply_Click(object sender, RoutedEventArgs e)
        {
            if (hasValidValues())
            {
                try
                {
                    sqlConnection = DatabaseConnection.CreateConnection();
                    sqlConnection.Open();
                    SqlCommand command = new()
                    {
                        Connection = sqlConnection
                    };
                    command.Parameters.Add("@Ime", System.Data.SqlDbType.NVarChar).Value = tbxName.Text.Trim();
                    command.Parameters.Add("@Prezime", System.Data.SqlDbType.NVarChar).Value = tbxSurname.Text.Trim();
                    command.Parameters.Add("@Telefon", System.Data.SqlDbType.NVarChar).Value = tbxPhoneNumber.Text.Trim();
                    command.Parameters.Add("@Lokacija", System.Data.SqlDbType.NVarChar).Value = tbxLocation.Text.Trim();
                    command.Parameters.Add("@KIme", System.Data.SqlDbType.NVarChar).Value = tbxUsername.Text.Trim();
                    if (isEdit)
                    {
                        command.Parameters.Add("@ImeID", System.Data.SqlDbType.NVarChar).Value = this.selectedID;
                        command.CommandText = @"Update tblKurir set Ime = @Ime, Prezime = @Prezime, TelefonskiBroj = @Telefon, Lokacija = @Lokacija, KorisnickoIme = @KIme where KorisnickoIme = @ImeID";
                    }
                    else
                    {
                        command.Parameters.Add("@KLozinka", System.Data.SqlDbType.NVarChar).Value = PasswordHasher.Encode(tbxPassword.Password);
                        command.CommandText = @"Insert into tblKurir(Ime, Prezime, TelefonskiBroj, Lokacija, KorisnickaLozinka, KorisnickoIme) values(@Ime, @Prezime, @Telefon, @Lokacija, @KLozinka, @KIme)";
                    }
                    command.ExecuteNonQuery();
                    command.Dispose();
                    if (!isEdit)
                    {
                        MessageBox.Show("Operacija uspešno izvršena", "Promena uspešna", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                    }
                    ResetInput();

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Promene nisu sačuvane zbog sledećeg problema u izvršavanju operacije: \n" + ex.Message, "Operacija je neuspešna", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                finally
                {
                    if (sqlConnection != null)
                        sqlConnection.Close();
                    if (isEdit)
                        this.Close();
                }
            }
        }

        private void ResetInput()
        {
            tbxName.Text = "";
            tbxSurname.Text = "";
            tbxPhoneNumber.Text = "";
            tbxLocation.Text = "";
            tbxUsername.Text = "";
            tbxPassword.Password = "";
        }

        private void tbxUsername_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = InputValidation.IsTextASCII(e.Text);
        }

        private void tbxPhoneNumber_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = InputValidation.IsPhone(e.Text);
        }
    }
}
