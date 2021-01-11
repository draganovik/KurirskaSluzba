﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
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
            string sqlSelect = @"select CenaID as 'Cena ID', Opis, Tezina, Cena from tblCenovnik";
            DataTable dataTable = DatabaseConnection.GetTable(sqlSelect);
            dgPrices.ItemsSource = dataTable.DefaultView;
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            DataRowView id = (DataRowView)dgPrices.SelectedItems[0];
            DatabaseConnection.DeleteById(id["Cena ID"].ToString(),"CenaID", "tblCenovnik");
            ShowData();
        }
    }
}
