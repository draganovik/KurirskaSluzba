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
    /// Interaction logic for WindowPrice.xaml
    /// </summary>
    public partial class WindowPrice : Window
    {
        SqlConnection sqlConnection = new();
        public WindowPrice()
        {
            InitializeComponent();
            tbxType.Focus();
            sqlConnection = DatabaseConnection.CreateConnection();
        }
        public WindowPrice(String windowType) : this()
        {
            setType(windowType);
        }

        private void setType(String type)
        {
            switch (type)
            {
                case "edit":
                    this.Title = "Izmena cene";
                    btnApply.Content = "Sačuvaj";
                    break;
                case "add":
                    this.Title = "Dodavanje nove cene";
                    btnApply.Content = "Dodaj cenu";
                    break;
            }

        }

        private void btnApply_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                sqlConnection.Open();
                SqlCommand command = new()
                {
                    Connection = sqlConnection
                };
                command.Parameters.Add("@Tezina", System.Data.SqlDbType.Int).Value = tbxWeight.Text;
                command.Parameters.Add("@Cena", System.Data.SqlDbType.Money).Value = tbxPrice.Text;
                command.Parameters.Add("@Opis", System.Data.SqlDbType.NVarChar).Value = tbxType.Text;
                command.CommandText = @"Insert into tblCenovnik(Opis,Tezina,Cena) values(@Opis, @Tezina, @Cena)";
                command.ExecuteNonQuery();
                command.Dispose();
                MessageBox.Show("Operacija uspešno izvršena", "Promena uspešna",MessageBoxButton.OK, MessageBoxImage.Asterisk);
                ResetInput();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Promene nisu sačuvane zbog sledećeg problema u izvršavanju operacije: \n" + ex.Message, "Operacija je neuspešna", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                if(sqlConnection != null)
                sqlConnection.Close();
            }


        }

        private void ResetInput()
        {
            tbxWeight.Text = "";
            tbxType.Text = "";
            tbxPrice.Text = "";
        }
    }
}
