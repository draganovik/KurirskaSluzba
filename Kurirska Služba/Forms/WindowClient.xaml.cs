using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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
    /// Interaction logic for WindowClient.xaml
    /// </summary>
    public partial class WindowClient : Window
    {
        SqlConnection sqlConnection = new();
        bool isEdit = false;
        string selectedID;
        public WindowClient()
        {
            InitializeComponent();
            setType("add");

        }

        public WindowClient(String selectedID)
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
                    this.Title = "Izmena podataka klijenta";
                    btnApply.Content = "Sačuvaj";
                    isEdit = true;
                    break;
                case "add":
                    this.Title = "Dodavanje novog klijenta";
                    btnApply.Content = "Napravi nalog";
                    isEdit = false;
                    break;
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
                command.Parameters.Add("@Ime", System.Data.SqlDbType.NVarChar).Value = tbxName.Text;
                command.Parameters.Add("@Prezime", System.Data.SqlDbType.NVarChar).Value = tbxSurname.Text;
                command.Parameters.Add("@Telefon", System.Data.SqlDbType.NVarChar).Value = tbxPhoneNumber.Text;
                command.Parameters.Add("@Grad", System.Data.SqlDbType.NVarChar).Value = tbxCity.Text;
                command.Parameters.Add("@Adresa", System.Data.SqlDbType.NVarChar).Value = tbxAddress.Text;
                command.Parameters.Add("@KIme", System.Data.SqlDbType.NVarChar).Value = tbxUsername.Text;
                if (isEdit)
                {
                    command.Parameters.Add("@ImeID", System.Data.SqlDbType.NVarChar).Value = this.selectedID;
                    command.CommandText = @"Update tblKlijent set Ime = @Ime, Prezime = @Prezime, TelefonskiBroj = @Telefon, Grad = @Grad, Adresa = @Adresa, KorisnickoIme = @KIme where KorisnickoIme = @ImeID";
                }
                else
                {
                    command.Parameters.Add("@KLozinka", System.Data.SqlDbType.NVarChar).Value = PasswordHasher.Encode(tbxPassword.Password);
                    command.CommandText = @"Insert into tblKlijent(Ime, Prezime, TelefonskiBroj, Grad, Adresa, KorisnickaLozinka, KorisnickoIme) values(@Ime, @Prezime, @Telefon, @Grad, @Adresa, @KLozinka, @KIme)";
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

        private void ResetInput()
        {
            tbxName.Text = "";
            tbxSurname.Text = "";
            tbxPhoneNumber.Text = "";
            tbxCity.Text = "";
            tbxAddress.Text = "";
            tbxUsername.Text = "";
            tbxPassword.Password = "";
        }
    }
}
