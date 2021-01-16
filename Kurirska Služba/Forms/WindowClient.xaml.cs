using KurirskaSluzba.Controllers;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Input;

namespace KurirskaSluzba.Forms
{
    /// <summary>
    /// Interaction logic for WindowClient.xaml
    /// </summary>
    public partial class WindowClient : Window
    {
        private SqlConnection sqlConnection = new();
        private bool isEdit;
        private readonly string selectedID;

        public WindowClient()
        {
            InitializeComponent();
            tbxName.Focus();
            setType("add");

        }

        public WindowClient(string selectedID)
        {
            InitializeComponent();
            setType("edit");
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
                command.CommandText = @"Select * from tblKlijent where KorisnickoIme= @id";
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    tbxName.Text = reader["Ime"].ToString();
                    tbxSurname.Text = reader["Prezime"].ToString();
                    tbxPhoneNumber.Text = reader["TelefonskiBroj"].ToString();
                    tbxCity.Text = reader["Grad"].ToString();
                    tbxAddress.Text = reader["Adresa"].ToString();
                    tbxUsername.Text = reader["KorisnickoIme"].ToString();
                    tbxPassword.Password = "";
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

        private void setType(string type)
        {
            switch (type)
            {
                case "edit":
                    Title = "Izmena podataka klijenta";
                    btnApply.Content = "Sačuvaj";
                    grdPassword.Visibility = Visibility.Collapsed;
                    tbxUsername.IsEnabled = false;
                    isEdit = true;
                    break;
                case "add":
                default:
                    Title = "Dodavanje novog klijenta";
                    btnApply.Content = "Napravi nalog";
                    isEdit = false;
                    break;
            }

        }

        private bool hasValidValues()
        {
            bool hasValidUsername = DatabaseConnection.IsUniqueValue(tbxUsername.Text, "KorisnickoIme", "tblKlijent");
            if (!string.IsNullOrEmpty(tbxName.Text.Trim()) &&
            !string.IsNullOrEmpty(tbxSurname.Text.Trim()) &&
            !string.IsNullOrEmpty(tbxPhoneNumber.Text.Trim()) &&
            !string.IsNullOrEmpty(tbxCity.Text.Trim()) &&
            !string.IsNullOrEmpty(tbxAddress.Text.Trim()) &&
            !string.IsNullOrEmpty(tbxUsername.Text.Trim()) && (hasValidUsername || isEdit))
            {
                return true;
            }
            if (hasValidUsername || string.IsNullOrEmpty(tbxUsername.Text.Trim()) || isEdit)
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
                    command.Parameters.Add("@Ime", SqlDbType.NVarChar).Value = tbxName.Text.Trim();
                    command.Parameters.Add("@Prezime", SqlDbType.NVarChar).Value = tbxSurname.Text.Trim();
                    command.Parameters.Add("@Telefon", SqlDbType.NVarChar).Value = tbxPhoneNumber.Text.Trim();
                    command.Parameters.Add("@Grad", SqlDbType.NVarChar).Value = tbxCity.Text.Trim();
                    command.Parameters.Add("@Adresa", SqlDbType.NVarChar).Value = tbxAddress.Text.Trim();
                    command.Parameters.Add("@KIme", SqlDbType.NVarChar).Value = tbxUsername.Text.Trim();
                    if (isEdit)
                    {
                        command.Parameters.Add("@ImeID", SqlDbType.NVarChar).Value = selectedID;
                        command.CommandText = @"Update tblKlijent set Ime = @Ime, Prezime = @Prezime, TelefonskiBroj = @Telefon, Grad = @Grad, Adresa = @Adresa, KorisnickoIme = @KIme where KorisnickoIme = @ImeID";
                    }
                    else
                    {
                        command.Parameters.Add("@KLozinka", SqlDbType.NVarChar).Value = PasswordHasher.Encode(tbxPassword.Password);
                        command.CommandText = @"Insert into tblKlijent(Ime, Prezime, TelefonskiBroj, Grad, Adresa, KorisnickaLozinka, KorisnickoIme) values(@Ime, @Prezime, @Telefon, @Grad, @Adresa, @KLozinka, @KIme)";
                    }
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
