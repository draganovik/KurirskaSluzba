using Kurirska_Služba.Forms;
using System;
using System.ComponentModel;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;

namespace Kurirska_Služba
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static MainWindow()
        {
            _menuDropAlignmentField = typeof(SystemParameters).GetField("_menuDropAlignment", BindingFlags.NonPublic | BindingFlags.Static);
            System.Diagnostics.Debug.Assert(_menuDropAlignmentField != null);

            EnsureStandardPopupAlignment();
            SystemParameters.StaticPropertyChanged += SystemParameters_StaticPropertyChanged;
        }

        #region Menubar fix
        private static readonly FieldInfo _menuDropAlignmentField;

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
        #endregion

        private void displayWindow(Window window)
        {
            try
            {
                window.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Nastao je problem pri postavci okruženja: \n" + ex.Message, "Neuspešno otvaranje prozora", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            updateViews();
        }

        private void miNewPackage_Click(object sender, RoutedEventArgs e)
        {
            displayWindow(new WindowPackage() { Owner = this });
        }

        private void miNewClient_Click(object sender, RoutedEventArgs e)
        {
            displayWindow(new WindowClient() { Owner = this });
        }

        private void miNewCourier_Click(object sender, RoutedEventArgs e)
        {
            displayWindow(new WindowCourier() { Owner = this });
        }

        private void miNewMenadžer_Click(object sender, RoutedEventArgs e)
        {
            displayWindow(new WindowManager() { Owner = this });
        }

        private void miNewPrice_Click(object sender, RoutedEventArgs e)
        {
            displayWindow(new WindowPrice() { Owner = this });
        }
        private void updateViews()
        {
            viewPackageHistory.ShowData();
            viewPackages.ShowData();
            viewCouriers.ShowData();
            viewClients.ShowData();
            viewManagers.ShowData();
            viewPrices.ShowData();
        }

        private void tcMainContent_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (e.Source is TabControl)
            {
                updateViews();
            }
        }
    }
}
