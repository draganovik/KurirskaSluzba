using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Kurirska_Služba
{
    class DatabaseConnection
    {
        public static SqlConnection CreateConnection()
        {
            SqlConnectionStringBuilder stringBuilder = new SqlConnectionStringBuilder()
            {
                DataSource = @"DESKTOP-NQOQKUE",
                InitialCatalog = "SistemKurirskeSluzbe",
                IntegratedSecurity = true
            };
            string responce = stringBuilder.ToString();
            return new SqlConnection(responce);
        }
        public static DataTable GetTable(string selectCommandText)
        {
            SqlConnection sqlConnection = CreateConnection();
            sqlConnection.Open();
            SqlDataAdapter sdaCouriers = new(selectCommandText, sqlConnection);
            DataTable dataTable = new();
            sdaCouriers.Fill(dataTable);
            sqlConnection.Dispose();
            sqlConnection.Close();
            return dataTable;
        }
    }
}
