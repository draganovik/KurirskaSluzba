﻿using Kurirska_Služba.Controllers;
using Kurirska_Služba.Forms;
using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;

namespace Kurirska_Služba.Views
{
    /// <summary>
    /// Interaction logic for Prices.xaml
    /// </summary>
    public partial class Prices : UserControl
    {
        public Prices()
        {
            InitializeComponent();
            ShowData();
        }

        public void ShowData()
        {
            string sqlSelect = @"select CenaID as 'Cena ID', Opis, CAST(Cena as numeric(17,2)) from tblCenovnik";
            DataTable dataTable = DatabaseConnection.GetTable(sqlSelect);
            dgPrices.ItemsSource = dataTable.DefaultView;
        }

        private void btnChange_Click(object sender, RoutedEventArgs e)
        {
            if (dgPrices.SelectedItem != null)
            {
                DataRowView selectedRow = (DataRowView)dgPrices.SelectedItems[0];
                WindowPrice window = new WindowPrice(Convert.ToInt32(selectedRow["Cena ID"])) { Owner = Application.Current.MainWindow };
                window.ShowDialog();
                ShowData();
            }
            else
            {
                MessageBox.Show("Morate selektovati vrednost iz liste kako bi je menjali", "Element nije izabran", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (dgPrices.SelectedItem != null)
            {
                DataRowView id = (DataRowView)dgPrices.SelectedItems[0];
                DatabaseConnection.DeleteById(id["Cena ID"].ToString(), "CenaID", "tblCenovnik");
                ShowData();
            }
            else
            {
                MessageBox.Show("Morate selektovati vrednost iz liste kako bi je obrisali", "Element nije izabran", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void dgPrices_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgPrices.SelectedItem != null)
            {
                foreach (Button button in spControls.Children)
                {
                    button.IsEnabled = true;
                }
            }
            else
            {
                foreach (Button button in spControls.Children)
                {
                    button.IsEnabled = true;
                }
            }
        }
    }
}
