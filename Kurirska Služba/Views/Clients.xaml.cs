using System;
using System.Collections.Generic;
using System.Data;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Kurirska_Služba.Views
{
    /// <summary>
    /// Interaction logic for Clients.xaml
    /// </summary>
    public partial class Clients : UserControl
    {
        public Clients()
        {
            InitializeComponent();
            ShowData();
        }
        public void ShowData()
        {
            string sqlSelect = @"select '@' + KorisnickoIme as 'Korisničko ime', Ime + ' ' +Prezime as 'Ime', Grad, Adresa as Ulica, TelefonskiBroj as Telefon from tblKlijent";
            DataTable dataTable = DatabaseConnection.GetTable(sqlSelect);
            dgClients.ItemsSource = dataTable.DefaultView;
        }
    }
}
