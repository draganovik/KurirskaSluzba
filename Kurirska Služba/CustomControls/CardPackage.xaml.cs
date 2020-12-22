using System;
using System.Windows.Controls;

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
            Update(id, name, routeFrom, routeTo, status);
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
