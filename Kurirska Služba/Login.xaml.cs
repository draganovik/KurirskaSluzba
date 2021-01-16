using Kurirska_Služba.Controllers;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Input;

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
            var adminID = ValidateLoginGetUserID();
            if (adminID != "")
            {
                MainWindow main = new MainWindow(adminID);
                main.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Podaci nisu validni", "Prijava nije moguća", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

        }

        private string ValidateLoginGetUserID()
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
                        id = reader["MenadzerID"].ToString();
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

        private void tbxUsername_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = InputValidation.IsTextASCII(e.Text);
        }
    }
}
