using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;

namespace KurirskaSluzba.Controllers
{
    internal class DatabaseConnection
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
            DataTable dataTable = null;
            SqlConnection sqlConnection = null;
            try
            {
                sqlConnection = CreateConnection();
                sqlConnection.Open();
                SqlDataAdapter sqlDataAdapter = new(selectCommandText, sqlConnection);
                dataTable = new();
                sqlDataAdapter.Fill(dataTable);
                sqlDataAdapter.Dispose();
            }
            finally
            {
                sqlConnection.Dispose();
                if (sqlConnection != null)
                {
                    sqlConnection.Close();
                }
            }
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
                    command.Dispose();
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
                command.Dispose();
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
            SqlConnection connection = CreateConnection();
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
                    command.Dispose();
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
        public static bool IsUniqueValue(string id, string idName, string tableName)
        {
            bool isUnique = false;
            SqlConnection sqlConnection = new();
            try
            {
                sqlConnection = CreateConnection();
                sqlConnection.Open();
                SqlCommand command = new SqlCommand
                {
                    Connection = sqlConnection
                };
                command.CommandText = @"select * from " + tableName + " where " + idName + " = '" + id + "'";
                SqlDataReader reader = command.ExecuteReader();
                if (!reader.HasRows)
                {
                    isUnique = true;
                }
                command.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Nije moguće uporediti vrednosti sa bazom podataka " + ex.Message, "Problem u konekciji!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                sqlConnection.Dispose();
                if (sqlConnection != null)
                {
                    sqlConnection.Close();
                }
            }
            return isUnique;
        }
    }
}
