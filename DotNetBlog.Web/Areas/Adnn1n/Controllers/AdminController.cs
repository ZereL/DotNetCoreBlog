using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using DotNetBlog.Dal;

namespace DotNetBlog.Web.Areas.Controllers
{
    [Area("Adnn1n")]
    public class AdminController : Controller
    {
        AdminDal adal;

        public AdminController(AdminDal adal)
        {
            this.adal = adal;
        }

        public IActionResult ModPwd() { return View(); }

        [AutoValidateAntiforgeryToken()]
        [HttpPost]
        public IActionResult ModPwd(string oldpwd, string newpwd, string newpwd2)
        {

            if (string.IsNullOrEmpty(oldpwd) || string.IsNullOrEmpty(newpwd) || string.IsNullOrEmpty(newpwd2))
            {
                return Content("<script>alert('Fill all the blank！');location.href='/Adnn1n/Admin/ModPwd'</script>", "text/html");
            }


            if (newpwd != newpwd2)
            {
                return Content("<script>alert('type new passwords are different！');location.href='/Adnn1n/Admin/ModPwd'</script>", "text/html");
            }

            oldpwd = Tool.MD5Hash(oldpwd);




            int? adminid = HttpContext.Session.GetInt32("adminid");
            if (adminid == null)
            {
                //Not Login
                return Redirect("/Adnn1n/Login/");
            }

            Model.Admin a = adal.GetModel(adminid.Value);
            if (a == null)
            {
                return Content("<script>alert('adminid not exist！');location.href='/Adnn1n/Admin/ModPwd'</script>", "text/html");
            }
            if (a.Password != oldpwd)
            {
                return Content("<script>alert('Old password wrong！');location.href='/Adnn1n/Admin/ModPwd'</script>", "text/html");
            }

            a.Password = Tool.MD5Hash(newpwd);
            bool b = adal.UpdatePwd(a.Password, a.Id);

            if (b)
            {
                return Content("<script>alert('success！');parent.location.href='/Adnn1n/Login'</script>", "text/html");
            }
            else
            {
                return Content("<script>alert('Failed！');location.href='/Adnn1n/Admin/ModPwd'</script>", "text/html");
            }



        }

    }
}
