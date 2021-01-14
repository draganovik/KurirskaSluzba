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
    /// Interaction logic for WindowPrice.xaml
    /// </summary>
    public partial class WindowPrice : Window
    {
        SqlConnection sqlConnection = new();
        bool isEdit = false;
        int selectedID;
        public WindowPrice()
        {
            InitializeComponent();
            tbxType.Focus();
            setType("add");
        }
        public WindowPrice(int selectedID) : this()
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
                command.CommandText = @"Select * from tblCenovnik where CenaID= @id";
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    tbxPrice.Text = reader["Cena"].ToString();
                    tbxType.Text = reader["Opis"].ToString();
                    tbxWeight.Text = reader["Tezina"].ToString();
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
                    this.Title = "Izmena cene";
                    btnApply.Content = "Sačuvaj";
                    isEdit = true;
                    break;
                case "add":
                    this.Title = "Dodavanje nove cene";
                    btnApply.Content = "Dodaj cenu";
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
                command.Parameters.Add("@Tezina", System.Data.SqlDbType.Int).Value = tbxWeight.Text;
                command.Parameters.Add("@Cena", System.Data.SqlDbType.Money).Value = tbxPrice.Text;
                command.Parameters.Add("@Opis", System.Data.SqlDbType.NVarChar).Value = tbxType.Text;
                if (isEdit)
                {
                    command.Parameters.Add("@id", System.Data.SqlDbType.NVarChar).Value = selectedID;
                    command.CommandText = @"Update tblCenovnik set Opis = @Opis, Cena = @Cena, Tezina = @Tezina where CenaID = @id";
                }
                else
                {
                    command.CommandText = @"Insert into tblCenovnik(Opis,Tezina,Cena) values(@Opis, @Tezina, @Cena)";
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
            tbxWeight.Text = "";
            tbxType.Text = "";
            tbxPrice.Text = "";
        }
    }
}
