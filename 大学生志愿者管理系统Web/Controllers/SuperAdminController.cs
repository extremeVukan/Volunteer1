using System;
using System.Web.Mvc;
using BLL;

namespace 大学生志愿者管理系统Web.Controllers
{
    public class SuperAdminController : Controller
    {
        private UserService _userService;

        public SuperAdminController()
        {
            _userService = new UserService();
        }

        // GET: SuperAdmin
        public ActionResult Index()
        {
            // 检查是否已登录
            if (Session["Username"] == null)
                return RedirectToAction("Login", "Account");

            // 检查是否是平台管理员
            if ((int)Session["UserType"] != 2)
                return RedirectToAction("AccessDenied", "Account");

            ViewBag.Username = Session["Username"]; // 传递用户名到视图
            return View();
        }
    }
}