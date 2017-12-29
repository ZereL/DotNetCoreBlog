using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DotNetBlog.Dal;

namespace DotNetBlog.Web.Controllers
{
    public class TestController : Controller
    {
        public IActionResult Index()
        {
            string str = "";
            Random r = new Random();
            BlogDal blogdal = new BlogDal();
            CategoryDal cadal = new CategoryDal();

            List<Model.Category> list_ca = cadal.GetList("");

            for (int i = 0; i < 102; i++)
            {
                string title = $"新闻标题{i}";
                string body = title + "的内容";
                Model.Category ca = list_ca[r.Next(0, list_ca.Count)];
                string cabh = ca.Num,
                    caname = ca.CaName;

                blogdal.Insert(new Model.Blog
                {
                    Title = title,
                    Body = body,
                    VisitNum = r.Next(100, 999),
                    CaNum = cabh,
                    CaName = caname,
                });
            }
            str = "添加102条测试新闻成功！";

            return Content(str);
        }
    }
}
