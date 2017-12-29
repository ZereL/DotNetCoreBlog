using Dapper;
using DotNetBlog.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotNetBlog.Dal
{
    public class AdminDal
    {
        /// <summary>
        /// Connection String,from appsettings.json
        /// </summary>
        public string ConnStr { get; set; }

        public Admin Login(string username, string password)
        {
            string sql = "select * from admin where username=@username and password=@password";
            using (var connection = ConnectionFactory.GetOpenConnection(ConnStr))
            {
                var m = connection.Query<Model.Admin>(sql, new { username = username, password = password }).FirstOrDefault();
                return m;
            }
        }

        public Model.Admin GetModel(int id)
        {
            using (var connection = ConnectionFactory.GetOpenConnection(ConnStr))
            {

                var m = connection.Query<Model.Admin>("select * from admin where id = @id",

                  new { id = id }).FirstOrDefault();
                return m;
            }
        }

        public bool UpdatePwd(string pwd, int id)
        {
            using (var connection = ConnectionFactory.GetOpenConnection(ConnStr))
            {

                int res = connection.Execute(@"UPDATE [admin]
                                                       SET [password]=@password
                                                     WHERE Id=@Id", new { password = pwd, id = id });

                if (res > 0)
                {
                    return true;
                }
                return false;
            }
        }
    }
}
