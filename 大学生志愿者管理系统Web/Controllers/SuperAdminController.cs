using BLL;
using DAL;
using System;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace 大学生志愿者管理系统Web.Controllers
{
    public class SuperAdminController : Controller
    {
        private UserService _userService;
        private ActivityService _activityService;
        private IdentifyService _identifyService;
        private VolunteerService _volunteerService;
        private LogService _logService;

        public SuperAdminController()
        {
            _userService = new UserService();
            _activityService = new ActivityService();
            _identifyService = new IdentifyService();
            _volunteerService = new VolunteerService();
            _logService = new LogService();
        }

        // GET: SuperAdmin
        public ActionResult Index()
        {
            // 检查是否已登录
            if (Session["Username"] == null)
            {
                TempData["ErrorMessage"] = "请登录后访问";
                return RedirectToAction("Login", "Account");
            }

            // 检查是否是平台管理员
            if ((int)Session["UserType"] != 2)
            {
                return RedirectToAction("AccessDenied", "Account");
            }

            try
            {
                // 获取用户名
                ViewBag.Username = Session["Username"].ToString();

                // 获取管理员总数
                var admins = _userService.GetAllAdmins();
                ViewBag.AdminCount = admins.Count;

                // 获取志愿者总数
                var volunteers = _volunteerService.GetAllVolunteers();
                ViewBag.VolunteerCount = volunteers.Count;

                // 获取活动总数
                var activities = _activityService.GetAllActivities();
                ViewBag.ActivityCount = activities.Count;

                // 获取待审核的志愿者证申请数量
                var pendingIdentifies = _identifyService.GetPendingIdentifies();
                ViewBag.PendingIdentifyCount = pendingIdentifies.Count;

                // 获取最近的活动
                ViewBag.RecentActivities = activities.OrderByDescending(a => a.addtime).Take(5).ToList();

                // 获取最近的日志
                var logs = _logService.GetRecentLogs(5);
                ViewBag.RecentLogs = logs;

                return View();
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"加载平台管理中心出错: {ex.Message}";
                return View();
            }
        }

        #region 管理员管理

        // GET: SuperAdmin/Admins
        public ActionResult Admins()
        {
            // 检查权限
            if (Session["Username"] == null || (int)Session["UserType"] != 2)
            {
                return RedirectToAction("AccessDenied", "Account");
            }

            try
            {
                var admins = _userService.GetAllAdmins();
                return View(admins);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"获取管理员列表出错: {ex.Message}";
                return View(new adminT[] { });
            }
        }

        // GET: SuperAdmin/AddAdmin
        public ActionResult AddAdmin()
        {
            // 检查权限
            if (Session["Username"] == null || (int)Session["UserType"] != 2)
            {
                return RedirectToAction("AccessDenied", "Account");
            }

            return View();
        }

        // POST: SuperAdmin/AddAdmin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddAdmin(adminT model)
        {
            // 检查权限
            if (Session["Username"] == null || (int)Session["UserType"] != 2)
            {
                return RedirectToAction("AccessDenied", "Account");
            }

            try
            {
                // 将ID设置为0，让Service层自动分配ID
                model.admin_ID = 0;

                bool result = _userService.AddAdmin(model, Session["Username"].ToString());
                if (result)
                {
                    TempData["SuccessMessage"] = "管理员添加成功";
                    return RedirectToAction("Admins");
                }
                else
                {
                    TempData["ErrorMessage"] = "管理员添加失败";
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"添加管理员出错: {ex.Message}";
                return View(model);
            }
        }

        // GET: SuperAdmin/EditAdmin/5
        public ActionResult EditAdmin(int? id)
        {
            // 检查权限
            if (Session["Username"] == null || (int)Session["UserType"] != 2)
            {
                return RedirectToAction("AccessDenied", "Account");
            }

            // 如果没有传入ID，显示管理员列表供选择
            if (id == null)
            {
                try
                {
                    var admins = _userService.GetAllAdmins();
                    return View("SelectAdmin", admins);
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = $"获取管理员列表出错: {ex.Message}";
                    return RedirectToAction("Admins");
                }
            }

            try
            {
                var admin = _userService.GetAdminById(id.Value);
                if (admin == null)
                {
                    TempData["ErrorMessage"] = "找不到指定的管理员";
                    return RedirectToAction("Admins");
                }

                return View(admin);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"获取管理员信息出错: {ex.Message}";
                return RedirectToAction("Admins");
            }
        }

        // POST: SuperAdmin/EditAdmin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditAdmin(adminT model)
        {
            // 检查权限
            if (Session["Username"] == null || (int)Session["UserType"] != 2)
            {
                return RedirectToAction("AccessDenied", "Account");
            }

            try
            {
                bool result = _userService.UpdateAdmin(model);
                if (result)
                {
                    TempData["SuccessMessage"] = "管理员信息更新成功";
                    return RedirectToAction("Admins");
                }
                else
                {
                    TempData["ErrorMessage"] = "管理员信息更新失败";
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"更新管理员信息出错: {ex.Message}";
                return View(model);
            }
        }

        // GET: SuperAdmin/DeleteAdmin
        public ActionResult DeleteAdmin(int? id)
        {
            // 检查权限
            if (Session["Username"] == null || (int)Session["UserType"] != 2)
            {
                return RedirectToAction("AccessDenied", "Account");
            }

            // 如果没有传入ID，显示管理员列表供选择
            if (id == null)
            {
                try
                {
                    var admins = _userService.GetAllAdmins();
                    return View("SelectAdminToDelete", admins);
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = $"获取管理员列表出错: {ex.Message}";
                    return RedirectToAction("Admins");
                }
            }

            try
            {
                var admin = _userService.GetAdminById(id.Value);
                if (admin == null)
                {
                    TempData["ErrorMessage"] = "找不到指定的管理员";
                    return RedirectToAction("Admins");
                }

                return View(admin);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"获取管理员信息出错: {ex.Message}";
                return RedirectToAction("Admins");
            }
        }

        // POST: SuperAdmin/DeleteAdminConfirmed/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteAdminConfirmed(int id)
        {
            // 检查权限
            if (Session["Username"] == null || (int)Session["UserType"] != 2)
            {
                return RedirectToAction("AccessDenied", "Account");
            }

            try
            {
                bool result = _userService.DeleteAdmin(id, Session["Username"].ToString());
                if (result)
                {
                    TempData["SuccessMessage"] = "管理员删除成功";
                }
                else
                {
                    TempData["ErrorMessage"] = "管理员删除失败";
                }
                return RedirectToAction("Admins");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"删除管理员出错: {ex.Message}";
                return RedirectToAction("Admins");
            }
        }

        #endregion

        #region 志愿者管理

        // GET: SuperAdmin/Volunteers
        public ActionResult Volunteers()
        {
            // 检查权限
            if (Session["Username"] == null || (int)Session["UserType"] != 2)
            {
                return RedirectToAction("AccessDenied", "Account");
            }

            try
            {
                var volunteers = _volunteerService.GetAllVolunteers();
                return View(volunteers);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"获取志愿者列表出错: {ex.Message}";
                return View(new volunteerT[] { });
            }
        }

        // GET: SuperAdmin/AddVolunteer
        public ActionResult AddVolunteer()
        {
            // 检查权限
            if (Session["Username"] == null || (int)Session["UserType"] != 2)
            {
                return RedirectToAction("AccessDenied", "Account");
            }

            return View();
        }

        // POST: SuperAdmin/AddVolunteer
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddVolunteer(volunteerT model)
        {
            // 检查权限
            if (Session["Username"] == null || (int)Session["UserType"] != 2)
            {
                return RedirectToAction("AccessDenied", "Account");
            }

            try
            {
                // 将ID设置为0，让Service层自动分配ID
                model.Aid = 0;

                bool result = _volunteerService.AddVolunteer(model, Session["Username"].ToString());
                if (result)
                {
                    TempData["SuccessMessage"] = "志愿者添加成功";
                    return RedirectToAction("Volunteers");
                }
                else
                {
                    TempData["ErrorMessage"] = "志愿者添加失败";
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"添加志愿者出错: {ex.Message}";
                return View(model);
            }
        }

        // GET: SuperAdmin/EditVolunteer/5
        public ActionResult EditVolunteer(int? id)
        {
            // 检查权限
            if (Session["Username"] == null || (int)Session["UserType"] != 2)
            {
                return RedirectToAction("AccessDenied", "Account");
            }

            // 如果没有传入ID，显示志愿者列表供选择
            if (id == null)
            {
                try
                {
                    var volunteers = _volunteerService.GetAllVolunteers();
                    return View("SelectVolunteer", volunteers);
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = $"获取志愿者列表出错: {ex.Message}";
                    return RedirectToAction("Volunteers");
                }
            }

            try
            {
                var volunteer = _volunteerService.GetVolunteerById(id.Value);
                if (volunteer == null)
                {
                    TempData["ErrorMessage"] = "找不到指定的志愿者";
                    return RedirectToAction("Volunteers");
                }

                return View(volunteer);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"获取志愿者信息出错: {ex.Message}";
                return RedirectToAction("Volunteers");
            }
        }

        // POST: SuperAdmin/EditVolunteer
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditVolunteer(volunteerT model)
        {
            // 检查权限
            if (Session["Username"] == null || (int)Session["UserType"] != 2)
            {
                return RedirectToAction("AccessDenied", "Account");
            }

            try
            {
                bool result = _volunteerService.UpdateVolunteer(model, Session["Username"].ToString());
                if (result)
                {
                    TempData["SuccessMessage"] = "志愿者信息更新成功";
                    return RedirectToAction("Volunteers");
                }
                else
                {
                    TempData["ErrorMessage"] = "志愿者信息更新失败";
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"更新志愿者信息出错: {ex.Message}";
                return View(model);
            }
        }

        // GET: SuperAdmin/DeleteVolunteer
        public ActionResult DeleteVolunteer(int? id)
        {
            // 检查权限
            if (Session["Username"] == null || (int)Session["UserType"] != 2)
            {
                return RedirectToAction("AccessDenied", "Account");
            }

            // 如果没有传入ID，显示志愿者列表供选择
            if (id == null)
            {
                try
                {
                    var volunteers = _volunteerService.GetAllVolunteers();
                    return View("SelectVolunteerToDelete", volunteers);
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = $"获取志愿者列表出错: {ex.Message}";
                    return RedirectToAction("Volunteers");
                }
            }

            try
            {
                var volunteer = _volunteerService.GetVolunteerById(id.Value);
                if (volunteer == null)
                {
                    TempData["ErrorMessage"] = "找不到指定的志愿者";
                    return RedirectToAction("Volunteers");
                }

                return View(volunteer);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"获取志愿者信息出错: {ex.Message}";
                return RedirectToAction("Volunteers");
            }
        }

        // POST: SuperAdmin/DeleteVolunteerConfirmed/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteVolunteerConfirmed(int id)
        {
            // 检查权限
            if (Session["Username"] == null || (int)Session["UserType"] != 2)
            {
                return RedirectToAction("AccessDenied", "Account");
            }

            try
            {
                bool result = _volunteerService.DeleteVolunteer(id, Session["Username"].ToString());
                if (result)
                {
                    TempData["SuccessMessage"] = "志愿者删除成功";
                }
                else
                {
                    TempData["ErrorMessage"] = "志愿者删除失败";
                }
                return RedirectToAction("Volunteers");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"删除志愿者出错: {ex.Message}";
                return RedirectToAction("Volunteers");
            }
        }

        #endregion

        #region 志愿者证管理

        // GET: SuperAdmin/ExamineIdentify
        public ActionResult ExamineIdentify()
        {
            // 检查权限
            if (Session["Username"] == null || (int)Session["UserType"] != 2)
            {
                return RedirectToAction("AccessDenied", "Account");
            }

            try
            {
                var identifies = _identifyService.GetPendingIdentifies();
                return View(identifies);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"获取志愿者证申请列表出错: {ex.Message}";
                return View(new VolIdentifyT[] { });
            }
        }

        // POST: SuperAdmin/ApproveIdentify/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ApproveIdentify(int id)
        {
            // 检查权限
            if (Session["Username"] == null || (int)Session["UserType"] != 2)
            {
                return RedirectToAction("AccessDenied", "Account");
            }

            try
            {
                int empId = (int)Session["UserId"];
                bool result = _identifyService.ApproveIdentify(id, empId);
                if (result)
                {
                    TempData["SuccessMessage"] = "志愿者证申请审核通过";
                }
                else
                {
                    TempData["ErrorMessage"] = "志愿者证申请审核失败";
                }
                return RedirectToAction("ExamineIdentify");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"审核志愿者证出错: {ex.Message}";
                return RedirectToAction("ExamineIdentify");
            }
        }

        // POST: SuperAdmin/RejectIdentify/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RejectIdentify(int id)
        {
            // 检查权限
            if (Session["Username"] == null || (int)Session["UserType"] != 2)
            {
                return RedirectToAction("AccessDenied", "Account");
            }

            try
            {
                int empId = (int)Session["UserId"];
                bool result = _identifyService.RejectIdentify(id, empId);
                if (result)
                {
                    TempData["SuccessMessage"] = "志愿者证申请已拒绝";
                }
                else
                {
                    TempData["ErrorMessage"] = "操作失败";
                }
                return RedirectToAction("ExamineIdentify");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"处理志愿者证出错: {ex.Message}";
                return RedirectToAction("ExamineIdentify");
            }
        }

        #endregion

        #region 系统日志管理

        // GET: SuperAdmin/Logs
        public ActionResult Logs(int page = 1)
        {
            // 检查权限
            if (Session["Username"] == null || (int)Session["UserType"] != 2)
            {
                return RedirectToAction("AccessDenied", "Account");
            }

            try
            {
                int pageSize = 20;
                var logs = _logService.GetPaginatedLogs(page, pageSize);
                int totalCount = _logService.GetTotalLogsCount();

                ViewBag.CurrentPage = page;
                ViewBag.TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

                return View(logs);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"获取系统日志出错: {ex.Message}";
                return View(new dalogT[] { });
            }
        }

        #endregion

        #region 个人信息管理

        // GET: SuperAdmin/PersonalInfo
        public ActionResult PersonalInfo()
        {
            // 检查权限
            if (Session["Username"] == null || (int)Session["UserType"] != 2)
            {
                return RedirectToAction("AccessDenied", "Account");
            }

            try
            {
                string username = Session["Username"].ToString();
                var admin = _userService.GetSuperAdminInfo(username);
                if (admin == null)
                {
                    TempData["ErrorMessage"] = "找不到个人信息";
                    return RedirectToAction("Index");
                }

                return View(admin);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"获取个人信息出错: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        // GET: SuperAdmin/ChangePassword
        public ActionResult ChangePassword()
        {
            // 检查权限
            if (Session["Username"] == null || (int)Session["UserType"] != 2)
            {
                return RedirectToAction("AccessDenied", "Account");
            }

            return View();
        }

        // POST: SuperAdmin/ChangePassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(string currentPassword, string newPassword, string confirmPassword)
        {
            // 检查权限
            if (Session["Username"] == null || (int)Session["UserType"] != 2)
            {
                return RedirectToAction("AccessDenied", "Account");
            }

            // 验证新密码
            if (newPassword != confirmPassword)
            {
                ModelState.AddModelError("", "新密码和确认密码不一致");
                return View();
            }

            try
            {
                string username = Session["Username"].ToString();
                bool result = _userService.ChangeSuperAdminPassword(username, currentPassword, newPassword);
                if (result)
                {
                    TempData["SuccessMessage"] = "密码修改成功";
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", "当前密码不正确");
                    return View();
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"修改密码出错: {ex.Message}");
                return View();
            }
        }

        #endregion

        #region 系统统计

        // GET: SuperAdmin/SystemStats
        public ActionResult SystemStats()
        {
            // 检查权限
            if (Session["Username"] == null || (int)Session["UserType"] != 2)
            {
                return RedirectToAction("AccessDenied", "Account");
            }

            try
            {
                // 获取统计数据
                ViewBag.TotalActivities = _activityService.GetAllActivities().Count;
                ViewBag.ActiveActivities = _activityService.GetActiveActivities().Count;
                ViewBag.CompletedActivities = _activityService.GetCompletedActivities().Count;

                ViewBag.TotalVolunteers = _volunteerService.GetAllVolunteers().Count;
                ViewBag.TotalAdmins = _userService.GetAllAdmins().Count;

                ViewBag.TotalHours = _volunteerService.GetTotalVolunteerHours();
                ViewBag.AverageHours = _volunteerService.GetAverageVolunteerHours();

                ViewBag.TotalIdentifies = _identifyService.GetTotalIdentifies();
                ViewBag.ApprovedIdentifies = _identifyService.GetApprovedIdentifies().Count;

                // 按类型统计活动
                var activityTypes = _activityService.GetActivityTypeStats();
                ViewBag.ActivityTypeStats = activityTypes;

                return View();
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"获取统计数据出错: {ex.Message}";
                return View();
            }
        }

        #endregion

       

        // GET: SuperAdmin/ActivityDetails/5
        public ActionResult ActivityDetails(int id)
        {
            // 检查权限
            if (Session["Username"] == null || (int)Session["UserType"] != 2)
            {
                return RedirectToAction("AccessDenied", "Account");
            }

            try
            {
                var activity = _activityService.GetActivityById(id);
                if (activity == null)
                {
                    TempData["ErrorMessage"] = "找不到指定的活动";
                    return RedirectToAction("Activities");
                }

                return View(activity);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"获取活动详情出错: {ex.Message}";
                return RedirectToAction("Activities");
            }
        }
        // GET: SuperAdmin/GetActivityMembers/5
        public ActionResult GetActivityMembers(int id)
        {
            // 检查权限
            if (Session["Username"] == null || (int)Session["UserType"] != 2)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }

            try
            {
                var members = _activityService.GetActivityMembers(id);
                return Json(members, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        // GET: SuperAdmin/Activities
        public ActionResult Activities(string activityType = "", string status = "")
        {
            // 检查权限
            if (Session["Username"] == null || (int)Session["UserType"] != 2)
            {
                return RedirectToAction("AccessDenied", "Account");
            }

            try
            {
                var activities = _activityService.GetAllActivities();

                // 按类型筛选
                if (!string.IsNullOrEmpty(activityType))
                {
                    activities = activities.Where(a => a.activity_type == activityType).ToList();
                    ViewBag.ActivityType = activityType;
                }

                // 按状态筛选
                if (!string.IsNullOrEmpty(status))
                {
                    activities = activities.Where(a => a.status == status).ToList();
                    ViewBag.Status = status;
                }

                return View(activities);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"获取活动列表出错: {ex.Message}";
                return View(new ActivityT[] { });
            }
        }

        // GET: SuperAdmin/SearchActivities
        public ActionResult SearchActivities(string keyword)
        {
            // 检查权限
            if (Session["Username"] == null || (int)Session["UserType"] != 2)
            {
                return RedirectToAction("AccessDenied", "Account");
            }

            try
            {
                if (string.IsNullOrEmpty(keyword))
                {
                    return RedirectToAction("Activities");
                }

                var activities = _activityService.SearchActivitiesByName(keyword);
                ViewBag.Keyword = keyword;
                return View("Activities", activities);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"搜索活动出错: {ex.Message}";
                return RedirectToAction("Activities");
            }
        }
    }
}