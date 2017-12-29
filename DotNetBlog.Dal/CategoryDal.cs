using Dapper;
using DotNetBlog.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotNetBlog.Dal
{
    /// <summary>
    /// Category table Dal
    /// </summary>
    public class CategoryDal
    {
        /// <summary>
        /// Connection String,from appsettings.json
        /// </summary>
        public string ConnStr { get; set; }

        /// <summary>
        /// Generate numbers of category by parent number
        /// </summary>
        /// <param name="pnum">parent number</param>
        /// <param name="x">how many numbers in each tier</param>
        /// <returns></returns>
        public string GenBH(string pnum, int x)
        {
            string sql = "select right(max(num)," + x + ") from category where parNum=" + pnum;
            using (var connection = ConnectionFactory.GetOpenConnection(ConnStr))
            {

                string res = connection.ExecuteScalar<string>(sql);
                if (string.IsNullOrEmpty(res))
                {
                    int a = 1;
                    if (pnum != "0")
                    {
                        return pnum + a.ToString("d" + x);
                    }
                    return a.ToString("d" + x);
                }
                else
                {
                    int a = int.Parse(res) + 1;
                    int b = (int)Math.Pow(10, x);
                    if (a >= b)
                    {
                        throw new Exception("number is too big, need renew!");
                    }
                    if (pnum != "0")
                    {
                        return pnum + a.ToString("d" + x);
                    }
                    return a.ToString("d" + x);
                }

            }


        }

        /// <summary>
        /// Insert
        /// </summary>
        /// <returns></returns>
        public int Insert(Category category)
        {
            // Dapper - Insert
            using (var connection = ConnectionFactory.GetOpenConnection(ConnStr))
            {
                int resId = connection.Query<int>(@"INSERT INTO Category (CaName,Num,ParNum,Remark) values(@CaName,@Num,@ParNum,@Remark); SELECT @@IDENTITY;",
                  category).First();
                return resId;
            }
        }

        public dynamic getTreeJson()
        {
            List<Model.TreeNode_LayUI> list_return = new List<TreeNode_LayUI>();
            List<Model.Category> list = GetList("");
            var top = list.Where(a => a.ParNum == "0");
            foreach (var item in top)
            {
                Model.TreeNode_LayUI node = new TreeNode_LayUI() { id = item.Id, name = item.CaName, spread = true, pid = 0, };
                var sub = list.Where(a => a.ParNum == item.Num);
                List<Model.TreeNode_LayUI> list_sub = new List<TreeNode_LayUI>();
                foreach (var item2 in sub)
                {
                    Model.TreeNode_LayUI node2 = new TreeNode_LayUI() { id = item2.Id, name = item2.CaName, spread = true, pid = item.Id, };
                    list_sub.Add(node2);
                }
                node.children = list_sub;

                list_return.Add(node);
            }
            return Newtonsoft.Json.JsonConvert.SerializeObject(list_return);
        }

        public Category GetModelByNum(string caNum)
        {
            using (var connection = ConnectionFactory.GetOpenConnection(ConnStr))
            {

                var m = connection.Query<Category>("select * from category where Num=@Num",

                  new { Num = caNum }).FirstOrDefault();
                return m;
            }
        }

        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Delete(int id)
        {
            // Dapper - Delete
            using (var connection = ConnectionFactory.GetOpenConnection(ConnStr))
            {
                int res = connection.Execute(@"delete from Category where id = @id", new { id = id });
                if (res > 0)
                {
                    return true;
                }
                return false;
            }
        }

        /// <summary>
        /// GetList
        /// </summary>
        /// <param name="cond"></param>
        /// <returns></returns>
        public List<Category> GetList(string cond)
        {
            // Dapper – Simple List
            using (var connection = ConnectionFactory.GetOpenConnection(ConnStr))
            {
                string sql = "select * from Category";
                if (!string.IsNullOrEmpty(cond))
                {
                    sql += $" where {cond}";
                }
                var List = connection.Query<Category>(sql).ToList();
                return List;
            }
        }

        /// <summary>
        /// GetModel
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Category GetModel(int id)
        {
            // Dapper - Model
            using (var connection = ConnectionFactory.GetOpenConnection(ConnStr))
            {
                var model = connection.Query<Category>("select * from category where id = @id",
                  new { id = id }).First();
                return model;
            }
        }

        ///
        /// <summary>
        /// Update
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        public bool Update(Category category)
        {
            // Dapper - Update
            using (var connection = ConnectionFactory.GetOpenConnection(ConnStr))
            {
                int res = connection.Execute(@"UPDATE [Category]
                                               SET [CaName] = @CaName
                                                  ,[Num] = @Num
                                                  ,[ParNum] = @ParNum
                                                  ,[Remark] = @Remark
                                             WHERE Id = @id",
                  category);
                if (res > 0)
                {
                    return true;
                }
                return false;
            }
        }

        /// <summary>
        /// Count row number
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public int CalcCount(string cond)
        {
            string sql = "select count(1) from category";
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
    }
}
