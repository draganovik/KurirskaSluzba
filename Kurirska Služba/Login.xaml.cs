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

namespace Kurirska_Služba
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            var adminID = ValidateLoginGetID();
            if (adminID != "")
            {
                MainWindow main = new MainWindow(adminID);
                main.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Podaci nisu validni", "Prijava nije moguća",MessageBoxButton.OK,MessageBoxImage.Exclamation);
            }
            
        }

        private string ValidateLoginGetID()
        {
            SqlConnection sqlConnection = new();
            string id = "";
            try
            {
                sqlConnection = DatabaseConnection.CreateConnection();
                sqlConnection.Open();
                SqlCommand command = new SqlCommand
                {
                    Connection = sqlConnection
                };
                command.Parameters.Add("@Username", SqlDbType.NVarChar).Value = tbxUsername.Text;
                command.Parameters.Add("@Password", SqlDbType.NVarChar).Value = PasswordHasher.Encode(tbxPassword.Password); ;
                command.CommandText = @"select * from tblMenadzer
                                        where KorisnickoIme = @Username and KorisnickaLozinka = @Password";
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    if (reader.HasRows)
                    {
                        id = reader["KorisnickoIme"].ToString();
                    }
                }
            }
            finally
            {
                sqlConnection.Dispose();
                if (sqlConnection != null)
                {
                    sqlConnection.Close();
                }
            }
            return id;
        }
    }
}
