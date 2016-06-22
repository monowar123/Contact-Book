using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace Contract_Book
{
    public class DBConnection
    {
        private string connectionString = null;
        private SqlConnection sqlConn = null;

        public DBConnection()
        {
            connectionString = @"Data Source=MONOWAR-PC; Initial Catalog=Contact Book DB; User ID=sa; Password=cse";
            sqlConn = new SqlConnection(connectionString);
        }

        public SqlConnection GetConnection
        {
            get
            {
                return sqlConn;
            }
            private set
            {
            }
        }
    }
}
