using Dapper;
using DotNetBlog.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotNetBlog.Dal
{
    /// <summary>
    /// Blog Table Dal
    /// </summary>
    public class BlogDal
    {
        /// <summary>
        /// Connection String,from appsettings.json
        /// </summary>
        public string ConnStr { get; set; }

        /// <summary>
        /// Get Created Date
        /// </summary>
        /// <returns></returns>
        public List<string> GetBlogMonth()
        {
            string sql = "select left( CONVERT(varchar(100), CreateDate, 23),7) as aaa from blog group by left( CONVERT(varchar(100), CreateDate, 23),7) order by aaa desc";
            using (var connection = ConnectionFactory.GetOpenConnection(ConnStr))
            {
                var list = connection.Query<string>(sql).ToList();
                return list;
            }
        }

        /// <summary>
        ///offset pager
        /// </summary> 
        /// <param name="orderstr">如：yydate desc,yytime asc,id desc,必须形成唯一性</param>
        /// <param name="PageSize">page size</param>
        /// <param name="PageIndex">page index</param>
        /// <param name="strWhere">where condition</param>
        /// <returns></returns>
        public List<Blog> GetList(string orderstr, int PageSize, int PageIndex, string strWhere)
        {
            if (!string.IsNullOrEmpty(strWhere))
            {
                strWhere = " where " + strWhere;
            }
            string sql = string.Format(
                    "select * from [blog] {0} order by {1} offset {2} rows fetch next {3} rows only",
                    strWhere,
                    orderstr,
                    (PageIndex - 1) * PageSize,
                    PageSize
                );
            List<Blog> list = new List<Blog>();
            using (var connection = ConnectionFactory.GetOpenConnection(ConnStr))
            {
                list = connection.Query<Blog>(sql).ToList();
            }
            return list;
        }

        /// <summary>
        /// Insert
        /// </summary>
        /// <returns></returns>
        public int Insert(Model.Blog m)
        {
            using (var connection = ConnectionFactory.GetOpenConnection(ConnStr))
            {
                int resid = connection.Query<int>(
                    @"INSERT INTO [dbo].[Blog]
                       ( [Title]
                       ,[Body]
                       ,[Body_md]
                       ,[VisitNum]
                       ,[CaNum]
                       ,[CaName]
                       ,[Remark]
                       ,[Sort])
                 VALUES
                       ( @Title
                       ,@Body
                       ,@Body_md
                       ,@VisitNum
                       ,@CaNum
                       ,@CaName
                       ,@Remark
                       ,@Sort);SELECT @@IDENTITY;",
                    m).First();
                return resid;
            }
        }

        /// <summary>
        /// Count row number
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public int CalcCount(string cond)
        {
            string sql = "select count(1) from blog";
            if (!string.IsNullOrEmpty(cond))
            {
                sql += $" where {cond}";
            }
            using (var connection = ConnectionFactory.GetOpenConnection(ConnStr))
            {

                int res = connection.ExecuteScalar<int>(sql);
                return res;
            }
        }

        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Delete(int id)
        {
            using (var connection = ConnectionFactory.GetOpenConnection(ConnStr))
            {

                int res = connection.Execute(@"delete from Blog where id = @id", new { id = id });
                if (res > 0)
                {
                    return true;
                }
                return false;
            }
        }

        /// <summary>
        /// Read
        /// </summary>
        /// <param name="cond"></param>
        /// <returns></returns>
        public List<Model.Blog> GetList(string cond)
        {
            using (var connection = ConnectionFactory.GetOpenConnection(ConnStr))
            {
                string sql = "select * from Blog";
                if (!string.IsNullOrEmpty(cond))
                {
                    sql += $" where {cond}";
                }
                var list = connection.Query<Model.Blog>(sql).ToList();
                return list;

            }
        }

        /// <summary>
        /// Get Model
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Model.Blog GetModel(int id)
        {
            using (var connection = ConnectionFactory.GetOpenConnection(ConnStr))
            {

                var m = connection.Query<Model.Blog>("select * from Blog where id = @id",

                  new { id = id }).First();
                return m;
            }
        }

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        public bool Update(Model.Blog m)
        {
            using (var connection = ConnectionFactory.GetOpenConnection(ConnStr))
            {

                int res = connection.Execute(@"UPDATE [dbo].[Blog]
                                                       SET  [Title] = @Title
                                                          ,[Body] = @Body
                                                          ,[Body_md] = @body_md
                                                          ,[VisitNum] =@VisitNum
                                                          ,[CaNum] = @CaNum
                                                          ,[CaName] = @CaName
                                                          ,[Remark] =@Remark
                                                          ,[Sort] = @Sort
                                                     WHERE Id=@Id", m);

                if (res > 0)
                {
                    return true;
                }
                return false;
            }
        }
    }
}
