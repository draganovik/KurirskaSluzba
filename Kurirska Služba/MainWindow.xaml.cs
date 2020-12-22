using Kurirska_Služba.Forms;
using System;
using System.ComponentModel;
using System.Reflection;
using System.Windows;

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
            try
            {
                WindowPackage windowPackage = new("add")
                {
                    Owner = this
                };
                windowPackage.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Nastao je problem pri postavci okruženja: \n" + ex.Message, "Neuspešno otvaranje prozora", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void miNewClient_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                WindowClient windowClient = new("add")
                {
                    Owner = this
                };
                windowClient.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Nastao je problem pri postavci okruženja: \n" + ex.Message, "Neuspešno otvaranje prozora", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void miNewCourier_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                WindowCourier windowCourier = new("add")
                {
                    Owner = this
                };
                windowCourier.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Nastao je problem pri postavci okruženja: \n" + ex.Message, "Neuspešno otvaranje prozora", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void miNewMenadžer_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                WindowManager windowManager = new("add")
                {
                    Owner = this
                };
                windowManager.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Nastao je problem pri postavci okruženja: \n" + ex.Message, "Neuspešno otvaranje prozora", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void miNewPrice_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                WindowPrice windowPrice = new("add")
                {
                    Owner = this
                };
                windowPrice.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Nastao je problem pri postavci okruženja: \n" + ex.Message, "Neuspešno otvaranje prozora", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
