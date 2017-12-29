using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Text;

namespace DotNetBlog.Dal
{
    /// <summary>
    /// Database connection Factory
    /// </summary>
    public class ConnectionFactory
    {
        public static DbConnection GetOpenConnection(string connstr)
        {
            var connection = new SqlConnection(connstr);
            connection.Open();

            return connection;
        }
    }
}
