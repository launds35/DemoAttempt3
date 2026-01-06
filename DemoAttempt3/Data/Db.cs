using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoAttempt3.Data
{
    internal static class Db
    {
        private static readonly string _connectString = @"
            Server=localhost\SQLEXPRESS;
            Database=at3;
            Trusted_connection=true;
            TrustServerCertificate=true;
        ";

        public static SqlConnection GetConnection()
        {
            return new SqlConnection(_connectString);
        }
    }
}
