using Kurirska_Služba.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
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
using System.Windows.Shell;

namespace Kurirska_Služba
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static readonly FieldInfo _menuDropAlignmentField;
        static MainWindow()
        {
            _menuDropAlignmentField = typeof(SystemParameters).GetField("_menuDropAlignment", BindingFlags.NonPublic | BindingFlags.Static);
            System.Diagnostics.Debug.Assert(_menuDropAlignmentField != null);

            EnsureStandardPopupAlignment();
            SystemParameters.StaticPropertyChanged += SystemParameters_StaticPropertyChanged;
        }

        private static void SystemParameters_StaticPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            EnsureStandardPopupAlignment();
        }

        private static void EnsureStandardPopupAlignment()
        {
            if (SystemParameters.MenuDropAlignment && _menuDropAlignmentField != null)
            {
                _menuDropAlignmentField.SetValue(null, false);
            }
        }

        private void miNewPackage_Click(object sender, RoutedEventArgs e)
        {
            WindowPackage windowPackage = new("add")
            {
                Owner = this
            };
            windowPackage.ShowDialog();
        }

        private void miNewClient_Click(object sender, RoutedEventArgs e)
        {
            WindowClient windowClient = new("add")
            {
                Owner = this
            };
            windowClient.ShowDialog();
        }

        private void miNewCourier_Click(object sender, RoutedEventArgs e)
        {
            WindowCourier windowCourier = new("add")
            {
                Owner = this
            };
            windowCourier.ShowDialog();
        }

        private void miNewMenadžer_Click(object sender, RoutedEventArgs e)
        {
            WindowManager windowManager = new("add")
            {
                Owner = this
            };
            windowManager.ShowDialog();
        }

        private void miNewPrice_Click(object sender, RoutedEventArgs e)
        {
            WindowPrice windowPrice = new("add")
            {
                Owner = this
            };
            windowPrice.ShowDialog();
        }
    }
}
