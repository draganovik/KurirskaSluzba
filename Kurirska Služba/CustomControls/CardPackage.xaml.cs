using System;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace KurirskaSluzba.CustomControls
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

        public int ID { get; private set; } = -1;

        public CardPackage(int id, string name, int weight, string routeFrom, string routeTo, string status)
        {
            InitializeComponent();
            Update(id, name, weight, routeFrom, routeTo, status);
        }

        public void Update(int id, string name, int weight, string routeFrom, string routeTo, string status)
        {
            lbName.Content = name;
            lbRouteFrom.Content = routeFrom;
            lbRouteTo.Content = routeTo;
            lbStatus.Content = status;
            ID = id;
            SetImage(weight);
        }

        public void SetImage(int weight)
        {
            BitmapImage bitmapImage = weight switch
            {
                <= 600 => new BitmapImage(new Uri(@"pack://application:,,/Assets\letter_small.png", UriKind.Absolute)),
                <= 1200 => new BitmapImage(new Uri(@"pack://application:,,/Assets\letter_large.png", UriKind.Absolute)),
                <= 5000 => new BitmapImage(new Uri(@"pack://application:,,/Assets\box_small.png", UriKind.Absolute)),
                _ => new BitmapImage(new Uri(@"pack://application:,,/Assets\box_large.png", UriKind.Absolute)),
            };
            brdImage.Background = new ImageBrush(bitmapImage);
        }
    }
}
