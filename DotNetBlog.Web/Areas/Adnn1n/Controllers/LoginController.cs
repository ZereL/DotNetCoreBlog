using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using DotNetBlog.Model;
using DotNetBlog.Dal;

namespace DotNetBlog.Web.Areas.Controllers
{
    [Area("Adnn1n")]
    public class LoginController : Controller
    {
        AdminDal aDal;

        public LoginController(AdminDal adal)
        {
            this.aDal = adal;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(string username, string password)
        {
            username = Tool.GetSafeSQL(username);
            password = Tool.MD5Hash(password);

            Admin a = aDal.Login(username, password);
            if (a == null)
            {
                return Content("Login Error,wrong username or password！");
            }
            HttpContext.Session.SetInt32("adminid", a.Id);
            HttpContext.Session.SetString("adminusername", a.UserName);
            return Redirect("/Adnn1n/Home/Index");
        }
    }
}
