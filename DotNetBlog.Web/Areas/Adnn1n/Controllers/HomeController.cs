using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using Microsoft.Net.Http.Headers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace DotNetBlog.Web.Areas.Controllers
{
    [Area("Adnn1n")]
    public class HomeController : Controller
    {
        private IHostingEnvironment hostingEnv;
        public HomeController(IHostingEnvironment env)
        {
            hostingEnv = env;
        }
        public IActionResult Index()
        {
            int? adminid = HttpContext.Session.GetInt32("adminid");
            if (adminid == null)
            {
                //Not Login
                return Redirect("/Adnn1n/Login/");
            }
            return View();
        }
        public IActionResult Top()
        {
            return View();
        }
        public IActionResult Left()
        {
            return View();
        }
        public IActionResult Welcome()
        {
            return View();
        }
        public IActionResult ImgUpload()
        {
                        var imgFile = Request.Form.Files[0];
            if (imgFile != null && !string.IsNullOrEmpty(imgFile.FileName))
            {
                long size = 0;
                string tempname = "";
                var filename = ContentDispositionHeaderValue
                                .Parse(imgFile.ContentDisposition)
                                .FileName
                                .Trim('"');
                var extname = filename.Substring(filename.LastIndexOf("."), filename.Length - filename.LastIndexOf(".")); //extensions，.jpg

                #region Extension
                if (!extname.ToLower().Contains("jpg") && !extname.ToLower().Contains("png") && !extname.ToLower().Contains("gif"))
                {
                    return Json(new { code = 1, msg = "Only allow jpg,png,gif.", });
                }
                #endregion

                #region size
                long mb = imgFile.Length / 1024 / 1024; // MB
                if (mb>5)
                {
                    return Json(new { code = 1, msg = " 5MB Max size.", });
                }
                #endregion




                var filename1 = System.Guid.NewGuid().ToString().Substring(0, 6) + extname;
                tempname = filename1;
                var path = hostingEnv.WebRootPath;
                string dir = DateTime.Now.ToString("yyyyMMdd");
                if (!System.IO.Directory.Exists(hostingEnv.WebRootPath + $@"\upload\{dir}"))
                {
                    System.IO.Directory.CreateDirectory(hostingEnv.WebRootPath + $@"\upload\{dir}");
                }
                filename = hostingEnv.WebRootPath + $@"\upload\{dir}\{filename1}";
                size += imgFile.Length;
                using (FileStream fs = System.IO.File.Create(filename))
                {
                    imgFile.CopyTo(fs);
                    fs.Flush();
                }
                return Json(new { code = 0, msg = "Upload Success", data = new { src = $"/upload/{dir}/{filename1}", title = filename } });
            }
            return Json(new { code = 1, msg = "Upload Failed", });
        }      
    }
}
