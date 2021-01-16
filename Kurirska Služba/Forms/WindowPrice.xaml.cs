using Kurirska_Služba.Controllers;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Input;

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
                    tbxPrice.Text = reader["Cena"].ToString()[0..^2];
                    tbxType.Text = reader["Opis"].ToString();
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

        private bool hasValidValues()
        {
            if (tbxPrice.Text != "" &&
            tbxType.Text != "")
            {
                return true;
            }
            MessageBox.Show("Morate popuniti sve informacije.", "Operacija nije sporovedena", MessageBoxButton.OK, MessageBoxImage.Warning);
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
                    command.Parameters.Add("@Cena", System.Data.SqlDbType.Money).Value = tbxPrice.Text.Trim();
                    command.Parameters.Add("@Opis", System.Data.SqlDbType.NVarChar).Value = tbxType.Text.Trim();
                    if (isEdit)
                    {
                        command.Parameters.Add("@id", System.Data.SqlDbType.NVarChar).Value = selectedID;
                        command.CommandText = @"Update tblCenovnik set Opis = @Opis, Cena = @Cena where CenaID = @id";
                    }
                    else
                    {
                        command.CommandText = @"Insert into tblCenovnik(Opis,Cena) values(@Opis, @Cena)";
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
            tbxType.Text = "";
            tbxPrice.Text = "";
        }

        private void tbxPrice_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = InputValidation.IsNumeric(e.Text);
        }
    }
}
