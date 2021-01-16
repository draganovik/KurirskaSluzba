using KurirskaSluzba.Controllers;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Input;

namespace KurirskaSluzba
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

        private string ValidateLoginGetUserID()
        {
            SqlConnection sqlConnection = null;
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
                command.Dispose();
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

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            string adminID = ValidateLoginGetUserID();
            if (!string.IsNullOrEmpty(adminID))
            {
                MainWindow main = new MainWindow(adminID);
                main.Show();
                Close();
            }
            else
            {
                MessageBox.Show("Podaci nisu validni", "Prijava nije moguća", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

        }

        private void tbxUsername_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = InputValidation.IsTextASCII(e.Text);
        }
    }
}
