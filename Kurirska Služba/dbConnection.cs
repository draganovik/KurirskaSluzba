using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kurirska_Služba
{
    class dbConnection
    {
        public SqlConnection sqlConnection()
        {
            SqlConnectionStringBuilder stringBuilder = new SqlConnectionStringBuilder()
            {
                DataSource = @"DESKTOP-NQOQKUE/SQLEXPRESS",
                InitialCatalog = "SistemKurirskeSluzbe",
                IntegratedSecurity = true
            };
            string responce = stringBuilder.ToString();
            return new SqlConnection(responce);
        }
    }
}
