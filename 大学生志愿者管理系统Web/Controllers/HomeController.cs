using System;
using System.Web.Mvc;
using BLL;
using DAL;

namespace 大学生志愿者管理系统Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "大学生志愿者管理系统Web版";
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "联系我们";
            return View();
        }
    }
}