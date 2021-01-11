using System;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

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

        public CardPackage(int id, String name, int weight, String routeFrom, String routeTo, String status)
        {
            InitializeComponent();
            Update(id, name, weight, routeFrom, routeTo, status);
        }

        public void Update(int id, String name, int weight, String routeFrom, String routeTo, String status)
        {
            lbName.Content = name;
            lbRouteFrom.Content = routeFrom;
            lbRouteTo.Content = routeTo;
            lbStatus.Content = status;
            setID(id);
            setImage(weight);
        }

        public void setImage(int weight)
        {
            // TODO: Image setup logic
            BitmapImage bitmapImage;
            switch (weight)
            {
                case <= 600:
                    bitmapImage = new BitmapImage(new Uri(@"pack://application:,,/Assets\letter_small.png", UriKind.Absolute));
                    break;
                case <= 1200:
                    bitmapImage = new BitmapImage(new Uri(@"pack://application:,,/Assets\letter_large.png", UriKind.Absolute));
                    break;
                case <= 5000:
                    bitmapImage = new BitmapImage(new Uri(@"pack://application:,,/Assets\box_small.png", UriKind.Absolute));
                    break;
                default:
                    bitmapImage = new BitmapImage(new Uri(@"pack://application:,,/Assets\box_large.png", UriKind.Absolute));
                    break;
            }
            brdImage.Background = new ImageBrush(bitmapImage);
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
