using Kurirska_Služba.Forms;
using System;
using System.ComponentModel;
using System.Diagnostics;
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
        string managerID = "";
        static MainWindow()
        {
            _menuDropAlignmentField = typeof(SystemParameters).GetField("_menuDropAlignment", BindingFlags.NonPublic | BindingFlags.Static);
            System.Diagnostics.Debug.Assert(_menuDropAlignmentField != null);

            EnsureStandardPopupAlignment();
            SystemParameters.StaticPropertyChanged += SystemParameters_StaticPropertyChanged;
        }
        public MainWindow(string managerID) : base()
        {
            InitializeComponent();
            this.managerID = managerID;
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

        private void miSignOut_Click(object sender, RoutedEventArgs e)
        {
            Login login = new Login();
            login.Show();
            this.Close();
        }

        private void miExitProgram_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void miAccount_Click(object sender, RoutedEventArgs e)
        {
            WindowManager account = new WindowManager(managerID) { Owner = this };
            account.ShowDialog();
        }

        private void miNewVersion_Click(object sender, RoutedEventArgs e)
        {
            var psi = new ProcessStartInfo
            {
                FileName = "https://github.com/draganovik/KurirskaSluzba",
                UseShellExecute = true
            };
            Process.Start(psi);
        }

        private void miSupport_Click(object sender, RoutedEventArgs e)
        {
            var psi = new ProcessStartInfo
            {
                FileName = "https://draganovik.com/kontakt",
                UseShellExecute = true
            };
            Process.Start(psi);
        }
    }
}
