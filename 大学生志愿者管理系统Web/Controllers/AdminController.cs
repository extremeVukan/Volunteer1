using System;
using System.Web.Mvc;
using BLL;

namespace 大学生志愿者管理系统Web.Controllers
{
    public class AdminController : Controller
    {
        private ActivityService _activityService;

        public AdminController()
        {
            _activityService = new ActivityService();
        }

        // GET: Admin
        public ActionResult Index()
        {
            // 检查是否已登录
            if (Session["Username"] == null)
                return RedirectToAction("Login", "Account");

            // 检查是否是管理员
            if ((int)Session["UserType"] != 1 && (int)Session["UserType"] != 2)
                return RedirectToAction("AccessDenied", "Account");

            ViewBag.Username = Session["Username"]; // 传递用户名到视图
            var activities = _activityService.GetAllActivities();
            return View(activities);
        }
    }
}