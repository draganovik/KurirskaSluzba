﻿using Kurirska_Služba.Forms;
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
        }

        private void miNewPackage_Click(object sender, RoutedEventArgs e)
        {
            displayWindow(new WindowPackage("add") { Owner = this });
        }

        private void miNewClient_Click(object sender, RoutedEventArgs e)
        {
            displayWindow(new WindowClient("add") { Owner = this });
        }

        private void miNewCourier_Click(object sender, RoutedEventArgs e)
        {
            displayWindow(new WindowCourier("add") { Owner = this });
        }

        private void miNewMenadžer_Click(object sender, RoutedEventArgs e)
        {
            displayWindow(new WindowManager("add") { Owner = this });
        }

        private void miNewPrice_Click(object sender, RoutedEventArgs e)
        {
            displayWindow(new WindowPrice("add") { Owner = this });
        }
    }
}
