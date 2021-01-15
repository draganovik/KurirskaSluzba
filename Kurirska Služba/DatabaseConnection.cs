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
        public static void DeleteById(string id, string idName, string tableName)
        {
            SqlConnection connection = CreateConnection(); ;
            try
            {
                connection.Open();
                MessageBoxResult result = MessageBox.Show("Da li ste sigurni?", "Upozorenje", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    SqlCommand command = new SqlCommand
                    {
                        Connection = connection
                    };
                    command.Parameters.Add("@id", SqlDbType.Int).Value = id;
                    command.CommandText = @"Delete from " + tableName + " where " + idName + "= " + "@id";
                    command.ExecuteNonQuery();
                }
            }
            catch (SqlException)
            {
                MessageBox.Show("Postoje povezani podaci u drugim tabelama", "Obaveštenje!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                if (connection != null)
                {
                    connection.Close();
                }
            }
        }
        public static bool DeleteByIdUnsafe(string id, string idName, string tableName)
        {
            bool isComleated = false;
            SqlConnection connection = CreateConnection(); ;
            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand
                {
                    Connection = connection
                };
                command.Parameters.Add("@id", SqlDbType.Int).Value = id;
                command.CommandText = @"Delete from " + tableName + " where " + idName + "= " + "@id";
                command.ExecuteNonQuery();
                isComleated = true;
            }
            catch (SqlException)
            {
                MessageBox.Show("Postoje povezani podaci u drugim tabelama", "Obaveštenje!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                if (connection != null)
                {
                    connection.Close();
                }
            }
            return isComleated;
        }
        public static void DeleteByValue(string id, string idName, string tableName)
        {
            SqlConnection connection = CreateConnection(); ;
            try
            {
                connection.Open();
                MessageBoxResult result = MessageBox.Show("Da li ste sigurni?", "Upozorenje", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    SqlCommand command = new SqlCommand
                    {
                        Connection = connection
                    };
                    command.Parameters.Add("@id", SqlDbType.NVarChar).Value = id;
                    command.CommandText = @"Delete from " + tableName + " where " + idName + "= " + "@id";
                    command.ExecuteNonQuery();
                }
            }
            catch (SqlException)
            {
                MessageBox.Show("Postoje povezani podaci u drugim tabelama", "Obaveštenje!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                if (connection != null)
                {
                    connection.Close();
                }
            }
        }
    }
}
