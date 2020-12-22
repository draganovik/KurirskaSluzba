using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
