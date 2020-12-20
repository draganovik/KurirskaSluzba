using System;
using System.Collections.Generic;
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

namespace Kurirska_Služba.CustomControls
{
    /// <summary>
    /// Interaction logic for CardPackage.xaml
    /// </summary>
    public partial class CardPackage : UserControl
    {
        private int PackageID = -1;
        public CardPackage()
        {
            InitializeComponent();
        }

        public CardPackage(int id, String name, String routeFrom, String routeTo, String status)
        {
            InitializeComponent();
            Update(id, name, routeFrom,routeTo, status);
        }

        public void Update(int id, String name, String routeFrom, String routeTo, String status)
        {
            lbName.Content = name;
            lbRouteFrom.Content = routeFrom;
            lbRouteTo.Content = routeTo;
            lbStatus.Content = status;
            setID(id);
            setImage();
        }

        public void setImage()
        {
            // TODO: Image setup logic
        }

        private void setID(int id)
        {
            PackageID = id;
        }

        public int getID()
        {
            return PackageID;
        }
    }
}
