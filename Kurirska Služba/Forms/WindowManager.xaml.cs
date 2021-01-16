using KurirskaSluzba.Controllers;
using System;
using System.Data.SqlClient;
using System.Globalization;
using System.Windows;
using System.Windows.Input;

namespace KurirskaSluzba.Forms
{
    /// <summary>
    /// Interaction logic for WindowManager.xaml
    /// </summary>
    public partial class WindowManager : Window
    {
        private SqlConnection sqlConnection = new();
        private bool isEdit;
        private readonly string selectedID;

        public WindowManager()
        {
            InitializeComponent();
            tbxName.Focus();
            setType("add");
        }

        public WindowManager(string selectedID, string idName)
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
                command.CommandText = @"Select * from tblMenadzer where " + idName + " = '" + selectedID + "'";
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    this.selectedID = reader["MenadzerID"].ToString();
                    tbxName.Text = reader["Ime"].ToString();
                    tbxSurname.Text = reader["Prezime"].ToString();
                    tbxPhoneNumber.Text = reader["TelefonskiBroj"].ToString();
                    tbxLocation.Text = reader["Lokacija"].ToString();
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
                    Title = "Izmena podataka menadžera";
                    btnApply.Content = "Sačuvaj";
                    grdPassword.Visibility = Visibility.Collapsed;
                    tbxUsername.IsEnabled = false;
                    isEdit = true;
                    break;
                case "add":
                default:
                    Title = "Dodavanje novog menadžera";
                    btnApply.Content = "Napravi nalog";
                    isEdit = false;
                    break;
            }

        }

        private bool hasValidValues()
        {
            bool hasValidUsername = DatabaseConnection.IsUniqueValue(tbxUsername.Text, "KorisnickoIme", "tblMenadzer");
            if (!string.IsNullOrEmpty(tbxName.Text.Trim()) &&
            !string.IsNullOrEmpty(tbxSurname.Text.Trim()) &&
            !string.IsNullOrEmpty(tbxPhoneNumber.Text.Trim()) &&
            !string.IsNullOrEmpty(tbxLocation.Text.Trim()) &&
            !string.IsNullOrEmpty(tbxUsername.Text) && (hasValidUsername || isEdit))
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
                    command.Parameters.Add("@Ime", System.Data.SqlDbType.NVarChar).Value = tbxName.Text.Trim();
                    command.Parameters.Add("@Prezime", System.Data.SqlDbType.NVarChar).Value = tbxSurname.Text.Trim();
                    command.Parameters.Add("@Telefon", System.Data.SqlDbType.NVarChar).Value = tbxPhoneNumber.Text.Trim();
                    command.Parameters.Add("@Lokacija", System.Data.SqlDbType.NVarChar).Value = tbxLocation.Text.Trim();
                    command.Parameters.Add("@KIme", System.Data.SqlDbType.NVarChar).Value = tbxUsername.Text.Trim();
                    if (isEdit)
                    {
                        command.Parameters.Add("@id", System.Data.SqlDbType.Int).Value = Convert.ToInt32(selectedID, new CultureInfo("en-US", false));
                        command.CommandText = @"Update tblMenadzer set Ime = @Ime, Prezime = @Prezime, TelefonskiBroj = @Telefon, Lokacija = @Lokacija, KorisnickoIme = @KIme where MenadzerID = @id";
                    }
                    else
                    {
                        command.Parameters.Add("@KLozinka", System.Data.SqlDbType.NVarChar).Value = PasswordHasher.Encode(tbxPassword.Password);
                        command.CommandText = @"Insert into tblMenadzer(Ime, Prezime, TelefonskiBroj, Lokacija, KorisnickaLozinka, KorisnickoIme) values(@Ime, @Prezime, @Telefon, @Lokacija, @KLozinka, @KIme)";
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
