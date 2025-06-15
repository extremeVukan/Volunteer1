using System;
using System.Web.Mvc;
using BLL;
using DAL;

namespace 大学生志愿者管理系统Web.Controllers
{
    public class AccountController : Controller
    {
        private UserService _userService;

        public AccountController()
        {
            _userService = new UserService();
        }

        // GET: Account/Login
        public ActionResult Login()
        {
            return View();
        }

        // POST: Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string username, string password, int userType = 0)
        {
            try
            {
                var (success, type, userId, userName) = _userService.Login(username, password, userType);

                if (success)
                {
                    // 存储登录信息到Session
                    Session["Username"] = userName;
                    Session["UserType"] = type;
                    Session["UserId"] = userId;

                    // 根据用户类型重定向
                    switch (type)
                    {
                        case 0: // 志愿者
                            return RedirectToAction("Index", "Volunteer");
                        case 1: // 管理员
                            return RedirectToAction("Index", "Admin");
                        case 2: // 平台管理员
                            return RedirectToAction("Index", "SuperAdmin");
                        default:
                            return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ViewBag.ErrorMessage = "用户名或密码错误";
                    return View();
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"登录过程中出错: {ex.Message}";
                return View();
            }
        }

        // GET: Account/Register
        public ActionResult Register()
        {
            return View();
        }

        // POST: Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(string name, string phone, string email)
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(phone))
            {
                ViewBag.ErrorMessage = "用户名或密码不能为空";
                return View();
            }

            try
            {
                bool result = _userService.RegisterVolunteer(name, phone, email);

                if (result)
                {
                    ViewBag.SuccessMessage = "注册成功";
                    return RedirectToAction("Login");
                }
                else
                {
                    ViewBag.ErrorMessage = "用户名已存在";
                    return View();
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"注册过程中出错: {ex.Message}";
                return View();
            }
        }

        // GET: Account/Logout
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}