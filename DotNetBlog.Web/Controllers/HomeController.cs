using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DotNetBlog.Dal;

namespace DotNetBlog.Web.Controllers
{
    public class HomeController : Controller
    {
        CategoryDal caDal;
        BlogDal bDal;

        public HomeController(CategoryDal caDal, BlogDal bDal)
        {
            this.caDal = caDal;
            this.bDal = bDal;
        }

        public IActionResult Index(string key, string month, string caNum)
        {
            ViewBag.blogDal = bDal;
            ViewBag.calist = caDal.GetList("");
            ViewBag.blogmonth = bDal.GetBlogMonth();

            ViewBag.search_key = key;
            ViewBag.search_month = month;
            ViewBag.search_caNum = caNum;
            return View();
        }
    }
}
