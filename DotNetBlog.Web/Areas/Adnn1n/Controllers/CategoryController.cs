using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using Microsoft.Net.Http.Headers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using DotNetBlog.Dal;
using DotNetBlog.Model;

namespace DotNetBlog.Web.Areas.Controllers
{
    [Area("Adnn1n")]
    public class CategoryController : Controller
    {
        CategoryDal dal;

        public CategoryController(CategoryDal cadal)
        {
            this.dal = cadal;
        }
        public IActionResult Index()
        {
            ViewBag.calist = dal.GetList("");
            ViewBag.nodejson = dal.getTreeJson();
            return View();
        }

        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Del(int id)
        {
            bool b = dal.Delete(id);
            if (b)
            {
                return Content("Delete Success！");
            }
            return Content("Delete falied！");
        }
        /// <summary>
        /// add
        /// </summary>
        /// <param name="pid"></param>
        /// <param name="caname"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Add(int pid, string caname)
        {
            caname = Tool.GetSafeSQL(caname);
            string pnum = "0";
            if (pid != 0)
            {
                Category pca = dal.GetModel(pid);
                if (pca != null)
                {
                    if (pca.ParNum != "0")
                    {
                        return Json(new { status = "n", info = "Can only have 2 tier of category!" });
                    }
                    pnum = pca.Num;
                    if (dal.CalcCount($"parnum='{pca.Num}' and caname='{caname}'") > 0)
                    {
                        return Json(new { status = "n", info = "Cannot have same name with other category!" });
                    }
                }
            }
            else
            {
                if (dal.CalcCount($"parnum='0' and caname='{caname}'") > 0)
                {
                    return Json(new { status = "n", info = "Cannot have same name with other category!" });
                }
            }
            string num = dal.GenBH(pnum, 2);
            dal.Insert(new Model.Category() { CaName = caname, ParNum = pnum, Num = num, });
            return Json(new { status = "y", info = "Create new Category success!" });

        }

        /// <summary>
        /// update
        /// </summary>
        /// <param name="pid"></param>
        /// <param name="caname"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Mod(int pid, string caname, int id)
        {
            Model.Category ca = dal.GetModel(id);
            if (ca == null)
            {
                return Json(new { status = "n", info = "wrong category id!" });
            }

            string num = ca.Num;
            string parnum = ca.ParNum;

            int source_pid = ca.ParNum == "0" ? 0 : dal.GetModelByNum(ca.ParNum).Id;//Original parentid

            Model.Category pca = dal.GetModel(pid);
            if (pca != null)
            {
                if (pca.Id != source_pid)
                {
                    //generate new number for parent number
                    parnum = pca.Num;
                    num = dal.GenBH(parnum, 2);
                }
            }
            else if (pid == 0)
            {
                //make it on top category
                parnum = "0";
                num = dal.GenBH(parnum, 2);
            }

            ca.CaName = caname;
            ca.Num = num;
            ca.ParNum = parnum;
            bool b = dal.Update(ca);
            if (b)
            {
                return Json(new { status = "y", info = "category update success！" });
            }
            else
            {
                return Json(new { status = "n", info = "category update falied！" });
            }
        }
    }

}
