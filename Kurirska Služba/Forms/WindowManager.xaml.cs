using System;
using System.Collections.Generic;
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
    /// Interaction logic for WindowManager.xaml
    /// </summary>
    public partial class WindowManager : Window
    {
        SqlConnection sqlConnection = new();
        public WindowManager()
        {
            InitializeComponent();
        }
        public WindowManager(String windowType)
        {
            InitializeComponent();
            setType(windowType);
        }

        private void setType(String type)
        {
            switch (type)
            {
                case "edit":
                    this.Title = "Izmena podataka menadžera";
                    btnApply.Content = "Sačuvaj";
                    break;
                case "add":
                    this.Title = "Dodavanje novog menadžera";
                    btnApply.Content = "Napravi nalog";
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
                command.Parameters.Add("@Lokacija", System.Data.SqlDbType.NVarChar).Value = tbxLocation.Text;
                command.Parameters.Add("@KIme", System.Data.SqlDbType.NVarChar).Value = tbxUsername.Text;
                command.Parameters.Add("@KLozinka", System.Data.SqlDbType.NVarChar).Value = tbxPassword.Password;
                command.CommandText = @"Insert into tblMenadzer(Ime, Prezime, TelefonskiBroj, Lokacija, KorisnickaLozinka, KorisnickoIme) values(@Ime, @Prezime, @Telefon, @Lokacija, @KLozinka, @KIme)";
                command.ExecuteNonQuery();
                command.Dispose();
                MessageBox.Show("Operacija uspešno izvršena", "Promena uspešna", MessageBoxButton.OK, MessageBoxImage.Asterisk);
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
    }
}
