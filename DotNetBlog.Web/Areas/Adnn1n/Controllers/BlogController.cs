using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DotNetBlog.Dal;
using DotNetBlog.Model;
using System.Collections;

namespace DotNetBlog.Web.Areas.Controllers
{
    [Area("Adnn1n")]
    public class BlogController : Controller
    {
        BlogDal dal;
        CategoryDal caDal;

        public BlogController(BlogDal bdal, CategoryDal cadal)
        {
            this.dal = bdal;
            this.caDal = cadal;
        }

        public IActionResult Index()
        {
            //List<Blog> list = dal.GetList("");
            ViewBag.calist = caDal.GetList("");
            //return View(list);
            return View();
        }
        /// <summary>
        /// get Search Conditions
        /// </summary>
        /// <param name="key"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="caNum"></param>
        /// <returns></returns>
        public string GetCond(string key, string start, string end, string caNum)
        {
            string cond = "1=1";
            if (!string.IsNullOrEmpty(key))
            {
                key = Tool.GetSafeSQL(key);
                cond += $" and title like '%{key}%'";
            }

            if (!string.IsNullOrEmpty(start))
            {
                DateTime d;
                if (DateTime.TryParse(start, out d))
                {
                    cond += $" and createdate >= '{d.ToString("yyyy-MM-dd")}'";
                }

            }

            if (!string.IsNullOrEmpty(end))
            {
                DateTime d;
                if (DateTime.TryParse(end, out d))
                {
                    cond += $" and createdate <= '{d.ToString("yyyy-MM-dd")}'";
                }

            }

            if (!string.IsNullOrEmpty(caNum))
            {
                caNum = Tool.GetSafeSQL(caNum);
                cond += $" and caNum = '{caNum}'";
            }

            return cond;
        }

        public IActionResult GetTotalCount(string key, string start, string end, string caNum)
        {

            int totalcount = dal.CalcCount(GetCond(key,start,end,caNum));
            return Content(totalcount.ToString());
        }

        public IActionResult List(int pageindex, int pagesize, string key, string start, string end, string caNum)
        {
            List<Blog> list = dal.GetList("sort asc, id desc", pagesize, pageindex, GetCond(key, start, end, caNum));
            ArrayList arr = new ArrayList();
            foreach (var item in list)
            {
                arr.Add(new
                {
                    id = item.Id,
                    title = item.Title,
                    createDate = item.CreateDate.ToString("yyyy-MM-dd HH:mm"),
                    visitNum = item.VisitNum,
                    caName = item.CaName,
                    sort = item.Sort,
                });
            }
            return Json(arr);
        }

        public IActionResult Add(int? id)
        {
            ViewBag.calist = caDal.GetList("");
            Blog m = new Blog();
            if (id!= null)
            {
                m = dal.GetModel(id.Value);
            }
            return View(m);
        }

        [AutoValidateAntiforgeryToken]
        [HttpPost]
        public IActionResult Add(Blog m)
        {
            Category ca = caDal.GetModelByNum(m.CaNum);
            if (ca!=null)
            {
                m.CaName = ca.CaName;
            }

            if (m.Id ==0)
            {
                dal.Insert(m);
            }
            else
            {
                dal.Update(m);
            }
            return Redirect("/Adnn1n/Blog/Index");
        }

        [HttpPost]
        public IActionResult Del(int id)
        {
            bool b = dal.Delete(id);
            if (b)
            {
                return Content("Deleted");
            }
            else
            {
                return Content("Deleted failed");
            }

        }
    }
}
