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
        public CardPackage()
        {
            InitializeComponent();
        }

        public CardPackage(String name, String routeFrom, String routeTo, String status)
        {
            InitializeComponent();
            Update(name, routeFrom,routeTo, status);
        }

        public void Update(String name, String routeFrom, String routeTo, String status)
        {
            lbName.Content = name;
            lbRouteFrom.Content = routeFrom;
            lbRouteTo.Content = routeTo;
            lbStatus.Content = status;
            setImage();
        }

        private void setImage()
        {
            // TODO: Image setup logic
        }
    }
}
