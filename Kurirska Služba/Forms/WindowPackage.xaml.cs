using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
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
    /// Interaction logic for WindowPackage.xaml
    /// </summary>
    public partial class WindowPackage : Window
    {
        SqlConnection sqlConnection = new();
        public WindowPackage()
        {
            InitializeComponent();
            tbxName.Focus();
            sqlConnection = DatabaseConnection.CreateConnection();
        }

        public WindowPackage(String windowType) : this()
        {
            setType(windowType);
        }

        private void setType(String type)
        {
            switch (type)
            {
                case "edit":
                    this.Title = "Izmena paketa";
                    btnApply.Content = "Sačuvaj";
                    break;
                case "add":
                    this.Title = "Dodavanje novog paketa";
                    btnApply.Content = "Napravi paket";
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
                command.Parameters.Add("@Naziv", System.Data.SqlDbType.NVarChar).Value = tbxName.Text;
                command.Parameters.Add("@Tezina", System.Data.SqlDbType.Int).Value = tbxWeight.Text;
                command.Parameters.Add("@Menadzer", System.Data.SqlDbType.Int).Value = 1;
                command.Parameters.Add("@Kurir", System.Data.SqlDbType.Int).Value = 1;
                command.Parameters.Add("@Posiljalac", System.Data.SqlDbType.Int).Value = 1;
                command.Parameters.Add("@Primalac", System.Data.SqlDbType.Int).Value = 1;
                command.Parameters.Add("@GradPreuzimanja", System.Data.SqlDbType.NVarChar).Value = tbxPickupCity.Text;
                command.Parameters.Add("@AdresaPreuzimanja", System.Data.SqlDbType.NVarChar).Value = tbxPickupAddress.Text;
                command.Parameters.Add("@GradDostave", System.Data.SqlDbType.NVarChar).Value = tbxDropoffCity.Text;
                command.Parameters.Add("@AdresaDostave", System.Data.SqlDbType.NVarChar).Value = tbxDropoffAddress.Text;
                command.Parameters.Add("@Postarina", System.Data.SqlDbType.Int).Value = 1;
                command.Parameters.Add("@Doplata", System.Data.SqlDbType.Money).Value = tbxRansom.Text;
                command.Parameters.Add("@VremeDostave", System.Data.SqlDbType.DateTime).Value = ((DateTime)dpDropoffDate.SelectedDate).ToString("yyyy-MM-dd",CultureInfo.CurrentCulture);
                command.Parameters.Add("@Napomena", System.Data.SqlDbType.NVarChar).Value =  new TextRange(rtbComment.Document.ContentStart, rtbComment.Document.ContentEnd).Text;
                command.CommandText = @"insert tblPosiljka (
                        Naziv,
                        Tezina,
                        DodeljenMenadzerID,
                        DodeljenPosiljalacID,
                        DodeljenPrimalacID,
                        DodeljenKurirID,
                        GradPreuzimanja,
                        AdresaPreuzimanja,
                        GradDostave,
                        AdresaDostave,
                        VremeDostave,
                        PostarinaID,
                        DoplataZaPaket,
                        Napomena)
                        values(@Naziv, @Tezina, @Menadzer, @Posiljalac, @Primalac, @Kurir, @GradPreuzimanja, @AdresaPreuzimanja, @GradDostave, @AdresaDostave, @VremeDostave, @Postarina, @Doplata, @Napomena)";
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
            tbxWeight.Text = "";
            cbxCourier.SelectedIndex = -1;
            cbxSender.SelectedIndex = -1;
            cbxReceiver.SelectedIndex = -1;
            tbxPickupCity.Text = "";
            tbxPickupAddress.Text = "";
            tbxDropoffCity.Text = "";
            tbxDropoffAddress.Text = "";
            cbxPostage.SelectedIndex = -1;
            tbxRansom.Text = "";
            dpDropoffDate.SelectedDate = null;
            rtbComment.Document = new();
        }
    }
}
