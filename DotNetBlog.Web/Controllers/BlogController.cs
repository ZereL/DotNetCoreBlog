using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using DotNetBlog.Dal;

namespace DotNetBlog.Web.Controllers
{
    /// <summary>
    /// Frontend blog controller
    /// </summary>
    public class BlogController : Controller
    {
        BlogDal dal;
        CategoryDal cadal;

        public BlogController(BlogDal bdal, CategoryDal cadal)
        {
            this.dal = bdal;
            this.cadal = cadal;
        }

        /// <summary>
        /// Get sql
        /// </summary>
        /// <returns></returns>
        public string GetCond(string key, string month, string caNum)
        {

            string cond = "1=1";
            if (!string.IsNullOrEmpty(key))
            {
                key = Tool.GetSafeSQL(key);
                cond += $" and title like '%{key}%'";
            }
            if (!string.IsNullOrEmpty(month))
            {
                DateTime d;
                if (DateTime.TryParse(month + "-01", out d))
                {
                    cond += $" and createdate>='{d.ToString("yyyy-MM-dd")}' and createdate<'{d.AddMonths(1).ToString("yyyy-MM-dd")}'";
                }
            }
            if (!string.IsNullOrEmpty(caNum))
            {
                caNum = Tool.GetSafeSQL(caNum);
                cond += $" and caNum='{caNum}'";
            }
            return cond;
        }

        /// <summary>
        /// count blog number
        /// </summary>
        /// <returns></returns>
        public IActionResult GetTotalCount(string key, string month, string caNum)
        {
            int totalcount = dal.CalcCount(GetCond(key, month, caNum));
            return Content(totalcount.ToString());
        }


        /// <summary>
        /// get pageindex and size，return JSON
        /// </summary>
        /// <param name="pageindex"></param>
        /// <param name="pagesize"></param>
        /// <returns></returns>
        public IActionResult List(int pageindex, int pagesize, string key, string month, string caNum)
        {
            List<Model.Blog> list = dal.GetList("sort asc,id desc", pagesize, pageindex, GetCond(key, month, caNum));
            ArrayList arr = new ArrayList();
            foreach (var item in list)
            {
                arr.Add(new
                {
                    id = item.Id,
                    title = item.Title,
                    createDate = item.CreateDate.ToString("yyyy-MM-dd HH:mm"),
                    caName = item.CaName,
                    desc = Tool.StringTruncat(Tool.GetNoHTMLString(item.Body), 60, "..."),
                });
            }
            return Json(arr);
        }


        /// <summary>
        /// show blog
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult Show(int id)
        {
            ViewBag.blogDal = dal;
            ViewBag.calist = cadal.GetList("");
            ViewBag.blogmonth = dal.GetBlogMonth();

            Model.Blog b = dal.GetModel(id);
            if (b == null)
            {
                return Content("Not Found！");
            }
            return View(b);
        }

    }
}
