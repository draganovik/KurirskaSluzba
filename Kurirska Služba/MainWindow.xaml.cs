using KurirskaSluzba.Forms;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;

namespace KurirskaSluzba
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly string managerID = "";

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1810:Initialize reference type static fields inline", Justification = "<Pending>")]
        static MainWindow()
        {
            _menuDropAlignmentField = typeof(SystemParameters).GetField("_menuDropAlignment", BindingFlags.NonPublic | BindingFlags.Static);
            Debug.Assert(_menuDropAlignmentField != null);

            EnsureStandardPopupAlignment();
            SystemParameters.StaticPropertyChanged += SystemParameters_StaticPropertyChanged;
        }

        public MainWindow(string managerID) : base()
        {
            InitializeComponent();
            this.managerID = managerID;
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
            displayWindow(new WindowPackage(managerID) { Owner = this });
        }

        private void miNewClient_Click(object sender, RoutedEventArgs e)
        {
            displayWindow(new WindowClient() { Owner = this });
        }

        private void miNewCourier_Click(object sender, RoutedEventArgs e)
        {
            displayWindow(new WindowCourier() { Owner = this });
        }

        private void miNewManager_Click(object sender, RoutedEventArgs e)
        {
            displayWindow(new WindowManager() { Owner = this });
        }

        private void miNewPrice_Click(object sender, RoutedEventArgs e)
        {
            displayWindow(new WindowPrice() { Owner = this });
        }

        private void miAccount_Click(object sender, RoutedEventArgs e)
        {
            WindowManager account = new WindowManager(managerID, "MenadzerID") { Owner = this };
            account.ShowDialog();
        }

        private void miSignOut_Click(object sender, RoutedEventArgs e)
        {
            Login login = new Login();
            login.Show();
            Close();
        }

        private void miExitProgram_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void miNewVersion_Click(object sender, RoutedEventArgs e)
        {
            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = "https://github.com/draganovik/KurirskaSluzba",
                UseShellExecute = true
            };
            Process.Start(psi);
        }

        private void miSupport_Click(object sender, RoutedEventArgs e)
        {
            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = "https://draganovik.com/kontakt",
                UseShellExecute = true
            };
            Process.Start(psi);
        }

        private void miAbout_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Kurirska Služba - Verzija 1.0 \t\t\t\nVlasništvo: Mladen Draganović", "O aplikaciji", MessageBoxButton.OK, MessageBoxImage.Asterisk);
        }

        private void tcMainContent_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.Source is TabControl)
            {
                updateViews();
            }
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
    }
}
