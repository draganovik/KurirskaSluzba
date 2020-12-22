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
    /// Interaction logic for PackageHistory.xaml
    /// </summary>
    public partial class PackageHistory : UserControl
    {
        public PackageHistory()
        {
            InitializeComponent();
            ShowData();
        }
        public void ShowData()
        {
            string sqlSelect = @"select
            tblStanjePosiljke.Vreme,
            SVP.NazivStanja as 'Ime trenutnog procesa',
            '#' + CONVERT(nvarchar,posiljka.PosiljkaID) as 'ID posiljke',
            Posiljka.Naziv as 'Naziv posiljke',
            ISNULL(Komentar, 'Nema') as Komentar
            from tblStanjePosiljke
                join tblPosiljka as Posiljka on tblStanjePosiljke.PosiljkaID = Posiljka.PosiljkaID
                join tblVrstaStanjaPosiljke as SVP on tblStanjePosiljke.StanjePosiljkeID = SVP.VrstaStanjaID";
            DataTable dataTable = DatabaseConnection.GetTable(sqlSelect);
            dgPackageHistory.ItemsSource = dataTable.DefaultView;
        }
    }
}
