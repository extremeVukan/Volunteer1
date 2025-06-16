using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BLL;
using DAL;

namespace 大学生志愿者管理系统Web.Controllers
{
    public class AdminController : Controller
    {
        private ActivityService _activityService;
        private UserService _userService;
        private OrderService _orderService;
        private VolunteerService _volunteerService;

        public AdminController()
        {
            _activityService = new ActivityService();
            _userService = new UserService();
            _orderService = new OrderService();
            _volunteerService = new VolunteerService();
        }

        // GET: Admin
        public ActionResult Index()
        {
            // 检查是否已登录
            if (Session["Username"] == null)
            {
                TempData["ErrorMessage"] = "请登录后访问";
                return RedirectToAction("Login", "Account");
            }

            // 检查是否是管理员
            if ((int)Session["UserType"] != 1 && (int)Session["UserType"] != 2)
            {
                return RedirectToAction("AccessDenied", "Account");
            }

            try
            {
                string username = Session["Username"].ToString();
                ViewBag.Username = username;

                // 获取当前管理员创建的活动
                var activities = _activityService.GetActivitiesByHolder(username);
                ViewBag.ActivityCount = activities.Count;

                // 获取当前管理员待审核的申请数量
                var pendingOrders = _orderService.GetPendingOrdersByHolder(username);
                ViewBag.PendingOrderCount = pendingOrders.Count;

                return View(activities);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"加载管理面板出错: {ex.Message}";
                return View(new List<ActivityT>());
            }
        }

        #region 个人信息管理

        // GET: Admin/PersonalInfo
        public ActionResult PersonalInfo()
        {
            // 检查权限
            if (Session["Username"] == null || ((int)Session["UserType"] != 1 && (int)Session["UserType"] != 2))
            {
                return RedirectToAction("AccessDenied", "Account");
            }

            try
            {
                string username = Session["Username"].ToString();
                var admin = _userService.GetAdminInfo(username);
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

        // POST: Admin/UpdatePersonalInfo
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdatePersonalInfo(adminT model)
        {
            // 检查权限
            if (Session["Username"] == null || ((int)Session["UserType"] != 1 && (int)Session["UserType"] != 2))
            {
                return RedirectToAction("AccessDenied", "Account");
            }

            try
            {
                bool result = _userService.UpdateAdminInfo(model);
                if (result)
                {
                    TempData["SuccessMessage"] = "个人信息更新成功";
                }
                else
                {
                    TempData["ErrorMessage"] = "个人信息更新失败";
                }
                return RedirectToAction("PersonalInfo");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"更新个人信息出错: {ex.Message}";
                return View("PersonalInfo", model);
            }
        }

        // GET: Admin/ChangePassword
        public ActionResult ChangePassword()
        {
            // 检查权限
            if (Session["Username"] == null || ((int)Session["UserType"] != 1 && (int)Session["UserType"] != 2))
            {
                return RedirectToAction("AccessDenied", "Account");
            }

            return View();
        }

        // POST: Admin/ChangePassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(string currentPassword, string newPassword, string confirmPassword)
        {
            // 检查权限
            if (Session["Username"] == null || ((int)Session["UserType"] != 1 && (int)Session["UserType"] != 2))
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
                bool result = _userService.ChangeAdminPassword(username, currentPassword, newPassword);
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

        #region 活动管理

        // GET: Admin/Activities
        public ActionResult Activities(string activityType = "", string status = "", int page = 1, int pageSize = 8)
        {
            // 检查权限
            if (Session["Username"] == null || ((int)Session["UserType"] != 1 && (int)Session["UserType"] != 2))
            {
                return RedirectToAction("AccessDenied", "Account");
            }

            try
            {
                string username = Session["Username"].ToString();
                var allActivities = _activityService.GetActivitiesByHolder(username);

                // 按类型筛选
                if (!string.IsNullOrEmpty(activityType))
                {
                    allActivities = allActivities.Where(a => a.activity_type == activityType).ToList();
                    ViewBag.ActivityType = activityType;
                }

                // 按状态筛选
                if (!string.IsNullOrEmpty(status))
                {
                    allActivities = allActivities.Where(a => (a.status ?? "") == status).ToList();
                    ViewBag.Status = status;
                }

                // 分页逻辑
                int totalCount = allActivities.Count();
                int totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

                // 确保页码有效
                if (page < 1) page = 1;
                if (page > totalPages && totalPages > 0) page = totalPages;

                // 获取当前页的数据
                var activities = allActivities
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                // 传递分页信息到视图
                ViewBag.CurrentPage = page;
                ViewBag.PageSize = pageSize;
                ViewBag.TotalPages = totalPages;
                ViewBag.TotalCount = totalCount;

                return View(activities);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"获取活动列表出错: {ex.Message}";
                return View(new List<ActivityT>());
            }
        }

        // GET: Admin/ActivityDetails/5
        public ActionResult ActivityDetails(int id)
        {
            // 检查权限
            if (Session["Username"] == null || ((int)Session["UserType"] != 1 && (int)Session["UserType"] != 2))
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

        // GET: Admin/AddActivity
        public ActionResult AddActivity()
        {
            // 检查权限
            if (Session["Username"] == null || ((int)Session["UserType"] != 1 && (int)Session["UserType"] != 2))
            {
                return RedirectToAction("AccessDenied", "Account");
            }

            try
            {
                // 获取下一个活动ID
                int nextId = _activityService.GetNextActivityId();
                ViewBag.NextActivityId = nextId;

                // 获取活动类型列表
                ViewBag.ActivityTypes = new List<string>
                {
                    "教育类", "社区发展类", "卫生与医疗类",
                    "环境保护类", "动物保护类", "紧急救援类"
                };

                return View();
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"准备添加活动页面时出错: {ex.Message}";
                return RedirectToAction("Activities");
            }
        }

        // POST: Admin/AddActivity
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddActivity(ActivityT model, HttpPostedFileBase activityImage)
        {
            // 检查权限
            if (Session["Username"] == null || ((int)Session["UserType"] != 1 && (int)Session["UserType"] != 2))
            {
                return RedirectToAction("AccessDenied", "Account");
            }

            try
            {
                // 设置活动创建者
                model.Holder = Session["Username"].ToString();
                model.status = "进行中";

                // 处理图片上传
                if (activityImage != null && activityImage.ContentLength > 0)
                {
                    // 生成文件名和路径
                    string dateTime = DateTime.Now.ToString("yyyyMMddHHmmss");
                    string fileName = dateTime + Path.GetFileName(activityImage.FileName);

                    // 1. 保存到Web应用程序目录
                    string webAppPath = Server.MapPath("~/Act_Images");
                    if (!Directory.Exists(webAppPath))
                    {
                        Directory.CreateDirectory(webAppPath);
                    }

                    string webTypePath = Path.Combine(webAppPath, model.activity_type);
                    if (!Directory.Exists(webTypePath))
                    {
                        Directory.CreateDirectory(webTypePath);
                    }

                    string webFilePath = Path.Combine(webTypePath, fileName);
                    activityImage.SaveAs(webFilePath);

                    // 2. 同时保存到WinForm应用程序目录
                    string winformPath = @"D:\大学\数据库开发\大学生志愿者管理系统1\大学生志愿者管理系统1\bin\Debug\Act_Images";
                    if (!Directory.Exists(winformPath))
                    {
                        Directory.CreateDirectory(winformPath);
                    }

                    string winformTypePath = Path.Combine(winformPath, model.activity_type);
                    if (!Directory.Exists(winformTypePath))
                    {
                        Directory.CreateDirectory(winformTypePath);
                    }

                    string winformFilePath = Path.Combine(winformTypePath, fileName);
                    // 复制到WinForm目录
                    System.IO.File.Copy(webFilePath, winformFilePath, true);

                    // 设置相对路径 - 这里使用相对于WinForm的路径
                    model.Image = $"Act_Images\\{model.activity_type}\\{fileName}";
                }
                else
                {
                    // 没有图片，使用默认图片
                    model.Image = "暂无图片.gif";
                }

                // 添加活动
                bool result = _activityService.AddActivity(model, null, model.Holder);
                if (result)
                {
                    TempData["SuccessMessage"] = "活动添加成功";
                    return RedirectToAction("Activities");
                }
                else
                {
                    TempData["ErrorMessage"] = "活动添加失败";

                    // 重新获取活动类型列表
                    ViewBag.ActivityTypes = new List<string>
            {
                "教育类", "社区发展类", "卫生与医疗类",
                "环境保护类", "动物保护类", "紧急救援类"
            };

                    return View(model);
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"添加活动出错: {ex.Message}";

                // 重新获取活动类型列表
                ViewBag.ActivityTypes = new List<string>
        {
            "教育类", "社区发展类", "卫生与医疗类",
            "环境保护类", "动物保护类", "紧急救援类"
        };

                return View(model);
            }
        }

        // GET: Admin/EditActivity/5
        public ActionResult EditActivity(int? id)
        {
            // 检查权限
            if (Session["Username"] == null || ((int)Session["UserType"] != 1 && (int)Session["UserType"] != 2))
            {
                return RedirectToAction("AccessDenied", "Account");
            }

            // 如果没有传入ID，显示活动列表供选择
            if (id == null)
            {
                try
                {
                    string username = Session["Username"].ToString();
                    var activities = _activityService.GetActivitiesByHolder(username);
                    return View("SelectActivity", activities);
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = $"获取活动列表出错: {ex.Message}";
                    return RedirectToAction("Activities");
                }
            }

            try
            {
                var activity = _activityService.GetActivityById(id.Value);
                if (activity == null)
                {
                    TempData["ErrorMessage"] = "找不到指定的活动";
                    return RedirectToAction("Activities");
                }

                // 获取活动类型列表
                ViewBag.ActivityTypes = new List<string>
                {
                    "教育类", "社区发展类", "卫生与医疗类",
                    "环境保护类", "动物保护类", "紧急救援类"
                };

                return View(activity);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"获取活动信息出错: {ex.Message}";
                return RedirectToAction("Activities");
            }
        }

        // POST: Admin/EditActivity
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditActivity(ActivityT model, HttpPostedFileBase activityImage)
        {
            // 检查权限
            if (Session["Username"] == null || ((int)Session["UserType"] != 1 && (int)Session["UserType"] != 2))
            {
                return RedirectToAction("AccessDenied", "Account");
            }

            try
            {
                // 处理图片上传
                if (activityImage != null && activityImage.ContentLength > 0)
                {
                    // 生成文件名和路径
                    string dateTime = DateTime.Now.ToString("yyyyMMddHHmmss");
                    string fileName = dateTime + Path.GetFileName(activityImage.FileName);

                    // 1. 保存到Web应用程序目录
                    string webAppPath = Server.MapPath("~/Act_Images");
                    if (!Directory.Exists(webAppPath))
                    {
                        Directory.CreateDirectory(webAppPath);
                    }

                    string webTypePath = Path.Combine(webAppPath, model.activity_type);
                    if (!Directory.Exists(webTypePath))
                    {
                        Directory.CreateDirectory(webTypePath);
                    }

                    string webFilePath = Path.Combine(webTypePath, fileName);
                    activityImage.SaveAs(webFilePath);

                    // 2. 同时保存到WinForm应用程序目录
                    string winformPath = @"D:\大学\数据库开发\大学生志愿者管理系统1\大学生志愿者管理系统1\bin\Debug\Act_Images";
                    if (!Directory.Exists(winformPath))
                    {
                        Directory.CreateDirectory(winformPath);
                    }

                    string winformTypePath = Path.Combine(winformPath, model.activity_type);
                    if (!Directory.Exists(winformTypePath))
                    {
                        Directory.CreateDirectory(winformTypePath);
                    }

                    string winformFilePath = Path.Combine(winformTypePath, fileName);
                    // 复制到WinForm目录
                    System.IO.File.Copy(webFilePath, winformFilePath, true);

                    // 设置相对路径 - 确保使用正确的格式，不要以"~"开头
                    model.Image = $"Act_Images\\{model.activity_type}\\{fileName}"; // 使用反斜杠格式
                }
                // 找到原活动记录，保留原图片路径（如果未上传新图片）
                else
                {
                    var existingActivity = _activityService.GetActivityById(model.activity_ID);
                    if (existingActivity != null && !string.IsNullOrEmpty(existingActivity.Image))
                    {
                        // 保留原路径，确保它不以"~"开头
                        model.Image = existingActivity.Image.StartsWith("~")
                            ? existingActivity.Image.Substring(1)
                            : existingActivity.Image;
                    }
                }

                bool result = _activityService.UpdateActivity(model, null, Session["Username"].ToString());
                if (result)
                {
                    TempData["SuccessMessage"] = "活动信息更新成功";
                    return RedirectToAction("Activities");
                }
                else
                {
                    TempData["ErrorMessage"] = "活动信息更新失败";

                    // 重新获取活动类型列表
                    ViewBag.ActivityTypes = new List<string>
            {
                "教育类", "社区发展类", "卫生与医疗类",
                "环境保护类", "动物保护类", "紧急救援类"
            };

                    return View(model);
                }
            }
            catch (Exception ex)
            {
                // 添加更详细的错误信息用于调试
                TempData["ErrorMessage"] = $"更新活动信息出错: {ex.Message}，堆栈跟踪：{ex.StackTrace}";

                // 重新获取活动类型列表
                ViewBag.ActivityTypes = new List<string>
        {
            "教育类", "社区发展类", "卫生与医疗类",
            "环境保护类", "动物保护类", "紧急救援类"
        };

                return View(model);
            }
        }
        // GET: Admin/DeleteActivity
        public ActionResult DeleteActivity(int? id)
        {
            // 检查权限
            if (Session["Username"] == null || ((int)Session["UserType"] != 1 && (int)Session["UserType"] != 2))
            {
                return RedirectToAction("AccessDenied", "Account");
            }

            // 如果没有传入ID，显示活动列表供选择
            if (id == null)
            {
                try
                {
                    string username = Session["Username"].ToString();
                    var activities = _activityService.GetActivitiesByHolder(username);
                    return View("SelectActivityToDelete", activities);
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = $"获取活动列表出错: {ex.Message}";
                    return RedirectToAction("Activities");
                }
            }

            try
            {
                var activity = _activityService.GetActivityById(id.Value);
                if (activity == null)
                {
                    TempData["ErrorMessage"] = "找不到指定的活动";
                    return RedirectToAction("Activities");
                }

                return View(activity);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"获取活动信息出错: {ex.Message}";
                return RedirectToAction("Activities");
            }
        }

        // POST: Admin/DeleteActivityConfirmed
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteActivityConfirmed(int id)
        {
            // 检查权限
            if (Session["Username"] == null || ((int)Session["UserType"] != 1 && (int)Session["UserType"] != 2))
            {
                return RedirectToAction("AccessDenied", "Account");
            }

            try
            {
                bool result = _activityService.DeleteActivity(id, Session["Username"].ToString());
                if (result)
                {
                    TempData["SuccessMessage"] = "活动删除成功";
                }
                else
                {
                    TempData["ErrorMessage"] = "活动删除失败";
                }
                return RedirectToAction("Activities");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"删除活动出错: {ex.Message}";
                return RedirectToAction("Activities");
            }
        }

        // GET: Admin/EndActivity
        public ActionResult EndActivity()
        {
            // 检查权限
            if (Session["Username"] == null || ((int)Session["UserType"] != 1 && (int)Session["UserType"] != 2))
            {
                return RedirectToAction("AccessDenied", "Account");
            }

            try
            {
                string username = Session["Username"].ToString();
                var activities = _activityService.GetActivitiesByHolder(username)
                    .Where(a => a.status != "已结束")
                    .ToList();

                return View(activities);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"获取活动列表出错: {ex.Message}";
                return View(new List<ActivityT>());
            }
        }

        // POST: Admin/EndActivityConfirmed
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EndActivityConfirmed(int id)
        {
            // 检查权限
            if (Session["Username"] == null || ((int)Session["UserType"] != 1 && (int)Session["UserType"] != 2))
            {
                return RedirectToAction("AccessDenied", "Account");
            }

            try
            {
                bool result = _activityService.EndActivity(id, Session["Username"].ToString());
                if (result)
                {
                    // 获取活动参与者
                    var members = _activityService.GetActivityMembers(id);

                    // 保存ActivityId供后续操作使用
                    TempData["ActivityId"] = id;
                    TempData["SuccessMessage"] = "活动已成功结束";

                    // 使用新创建的视图，确保名称正确
                    return View("ActivityMembersList", members);
                }
                else
                {
                    TempData["ErrorMessage"] = "结束活动失败";
                    return RedirectToAction("EndActivity");
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"结束活动出错: {ex.Message}";
                return RedirectToAction("EndActivity");
            }
        }

        // POST: Admin/UpdateVolunteerHours
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateVolunteerHours(int volunteerId, int additionalHours)
        {
            // 检查权限
            if (Session["Username"] == null || ((int)Session["UserType"] != 1 && (int)Session["UserType"] != 2))
            {
                return RedirectToAction("AccessDenied", "Account");
            }

            try
            {
                bool result = _volunteerService.UpdateVolunteerHours(volunteerId, additionalHours);
                if (result)
                {
                    TempData["SuccessMessage"] = $"成功为志愿者(ID:{volunteerId})添加{additionalHours}小时";
                }
                else
                {
                    TempData["ErrorMessage"] = $"更新志愿者(ID:{volunteerId})时长失败";
                }

                // 返回上一个活动成员页面
                if (TempData["ActivityId"] != null)
                {
                    int activityId = (int)TempData["ActivityId"];
                    // 保留ActivityId供下次请求使用
                    TempData.Keep("ActivityId");

                    var members = _activityService.GetActivityMembers(activityId);
                    ViewBag.ActivityId = activityId;
                    // 使用新的视图名称
                    return View("ActivityMembersList", members);
                }
                else
                {
                    return RedirectToAction("EndActivity");
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"更新志愿时长出错: {ex.Message}";
                return RedirectToAction("EndActivity");
            }
        }

        #endregion

        #region 审核申请

        // GET: Admin/ApplicationReview
        public ActionResult ApplicationReview()
        {
            // 检查权限
            if (Session["Username"] == null || ((int)Session["UserType"] != 1 && (int)Session["UserType"] != 2))
            {
                return RedirectToAction("AccessDenied", "Account");
            }

            try
            {
                string username = Session["Username"].ToString();
                var orders = _orderService.GetOrdersByHolder(username);
                return View(orders);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"获取申请列表出错: {ex.Message}";
                return View(new List<OrderT>());
            }
        }

        // POST: Admin/ApproveApplication
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ApproveApplication(int id)
        {
            // 检查权限
            if (Session["Username"] == null || ((int)Session["UserType"] != 1 && (int)Session["UserType"] != 2))
            {
                return RedirectToAction("AccessDenied", "Account");
            }

            try
            {
                int empId = (int)Session["UserId"];
                bool result = _orderService.ApproveOrder(id, empId);
                if (result)
                {
                    TempData["SuccessMessage"] = "申请已审核通过";
                }
                else
                {
                    TempData["ErrorMessage"] = "审核申请失败";
                }
                return RedirectToAction("ApplicationReview");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"审核申请出错: {ex.Message}";
                return RedirectToAction("ApplicationReview");
            }
        }
        // GET: Admin/ActivityParticipants/5
        public ActionResult ActivityParticipants(int? id)
        {
            // 检查权限
            if (Session["Username"] == null || ((int)Session["UserType"] != 1 && (int)Session["UserType"] != 2))
            {
                return RedirectToAction("AccessDenied", "Account");
            }

            // 如果ID为空，重定向到活动列表
            if (id == null)
            {
                TempData["ErrorMessage"] = "未指定活动ID，请从活动列表中选择一个活动";
                return RedirectToAction("Activities");
            }

            try
            {
                // 获取活动信息
                var activity = _activityService.GetActivityById(id.Value);
                if (activity == null)
                {
                    TempData["ErrorMessage"] = "找不到指定的活动";
                    return RedirectToAction("Activities");
                }

                // 获取活动参与者
                var members = _activityService.GetActivityMembers(id.Value);

                // 传递活动基本信息到视图
                ViewBag.ActivityId = id.Value;
                ViewBag.ActivityName = activity.activity_Name;

                return View(members);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"获取活动参与者出错: {ex.Message}";
                return RedirectToAction("ActivityDetails", new { id = id.Value });
            }
        }
        // POST: Admin/ConfirmSignIn
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ConfirmSignIn(int volunteerId, int activityId)
        {
            // 检查权限
            if (Session["Username"] == null || ((int)Session["UserType"] != 1 && (int)Session["UserType"] != 2))
            {
                return RedirectToAction("AccessDenied", "Account");
            }

            try
            {
                // 查找活动成员
                var member = _activityService.GetActivityMembers(activityId)
                    .FirstOrDefault(m => m.Volunteerid == volunteerId);

                if (member == null)
                {
                    TempData["ErrorMessage"] = "找不到指定的志愿者信息";
                    return RedirectToAction("ActivityParticipants", new { id = activityId });
                }

                // 更新签到时间
                bool result = _activityService.UpdateMemberSignTime(activityId, volunteerId, DateTime.Now);
                if (result)
                {
                    TempData["SuccessMessage"] = $"志愿者 {member.volunteer}(ID:{volunteerId}) 签到成功";
                }
                else
                {
                    TempData["ErrorMessage"] = "签到操作失败";
                }

                return RedirectToAction("ActivityParticipants", new { id = activityId });
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"签到出错: {ex.Message}";
                return RedirectToAction("ActivityParticipants", new { id = activityId });
            }
        }
        // GET: Admin/GetActivitiesJson
        public ActionResult GetActivitiesJson(string activityType = "", string status = "")
        {
            // 检查权限
            if (Session["Username"] == null || ((int)Session["UserType"] != 1 && (int)Session["UserType"] != 2))
            {
                return Json(new { success = false, message = "没有权限执行此操作" }, JsonRequestBehavior.AllowGet);
            }

            try
            {
                string username = Session["Username"].ToString();
                var allActivities = _activityService.GetActivitiesByHolder(username);

                // 按类型筛选
                if (!string.IsNullOrEmpty(activityType))
                {
                    allActivities = allActivities.Where(a => a.activity_type == activityType).ToList();
                }

                // 按状态筛选
                if (!string.IsNullOrEmpty(status))
                {
                    allActivities = allActivities.Where(a => (a.status ?? "") == status).ToList();
                }

                // 分离进行中和已结束的活动
                var ongoingActivities = allActivities.Where(a => a.status != "已结束").ToList();
                var endedActivities = allActivities.Where(a => a.status == "已结束").ToList();

                return Json(new
                {
                    success = true,
                    ongoingCount = ongoingActivities.Count,
                    endedCount = endedActivities.Count
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        // POST: Admin/EndActivityTask
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EndActivityTask(int activityId)
        {
            // 检查权限
            if (Session["Username"] == null || ((int)Session["UserType"] != 1 && (int)Session["UserType"] != 2))
            {
                return RedirectToAction("AccessDenied", "Account");
            }

            try
            {
                // 获取活动信息
                var activity = _activityService.GetActivityById(activityId);
                if (activity == null)
                {
                    TempData["ErrorMessage"] = "找不到指定的活动";
                    return RedirectToAction("Activities");
                }

                // 获取活动参与者
                var members = _activityService.GetActivityMembers(activityId);

                // 统计处理的数据
                int autoSignOutCount = 0;   // 自动签退人数
                int hoursAddedCount = 0;    // 更新时长人数
                int totalHoursAdded = 0;    // 总计增加时长

                DateTime now = DateTime.Now;

                foreach (var member in members)
                {
                    // 只处理已签到的志愿者
                    if (member.SignTime.HasValue)
                    {
                        DateTime returnTime;
                        bool needUpdateHours = false;

                        // 情况1：已签到但未签退的志愿者，自动签退
                        if (!member.ReturnTime.HasValue)
                        {
                            bool signOutResult = _activityService.UpdateMemberReturnTime(activityId, member.Volunteerid.Value, now);
                            if (signOutResult)
                            {
                                autoSignOutCount++;
                                returnTime = now;
                                needUpdateHours = true;
                            }
                            else
                            {
                                continue; // 签退失败，跳过此志愿者
                            }
                        }
                        // 情况2：已签到且已签退的志愿者，计算时长
                        else
                        {
                            returnTime = member.ReturnTime.Value;
                            needUpdateHours = true;
                        }

                        // 更新志愿者时长
                        if (needUpdateHours && member.Volunteerid.HasValue)
                        {
                            // 计算时长
                            int hours = _activityService.CalculateVolunteerHours(member.SignTime.Value, returnTime);

                            // 更新志愿者的累计时长
                            bool updateHoursResult = _volunteerService.UpdateVolunteerHours(member.Volunteerid.Value, hours);

                            if (updateHoursResult)
                            {
                                hoursAddedCount++;
                                totalHoursAdded += hours;
                            }
                        }
                    }
                }

                // 结束活动
                bool result = _activityService.EndActivity(activityId, Session["Username"].ToString());
                if (result)
                {
                    string successMessage = $"活动 \"{activity.activity_Name}\" 已成功结束";

                    // 添加自动签退信息
                    if (autoSignOutCount > 0)
                    {
                        successMessage += $"，已为{autoSignOutCount}名未签退志愿者自动处理";
                    }

                    // 添加时长更新信息
                    if (hoursAddedCount > 0)
                    {
                        successMessage += $"，共为{hoursAddedCount}名志愿者增加了{totalHoursAdded}小时志愿时长";
                    }

                    TempData["SuccessMessage"] = successMessage;
                }
                else
                {
                    TempData["ErrorMessage"] = "结束活动失败";
                }

                return RedirectToAction("Activities");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"结束活动出错: {ex.Message}";
                return RedirectToAction("Activities");
            }
        }
        // POST: Admin/ConfirmSignOut
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ConfirmSignOut(int volunteerId, int activityId)
        {
            // 检查权限
            if (Session["Username"] == null || ((int)Session["UserType"] != 1 && (int)Session["UserType"] != 2))
            {
                return RedirectToAction("AccessDenied", "Account");
            }

            try
            {
                // 查找活动成员
                var member = _activityService.GetActivityMembers(activityId)
                    .FirstOrDefault(m => m.Volunteerid == volunteerId);

                if (member == null)
                {
                    TempData["ErrorMessage"] = "找不到指定的志愿者信息";
                    return RedirectToAction("ActivityParticipants", new { id = activityId });
                }

                // 检查是否已签到
                if (!member.SignTime.HasValue)
                {
                    TempData["ErrorMessage"] = "该志愿者尚未签到，无法签退";
                    return RedirectToAction("ActivityParticipants", new { id = activityId });
                }

                // 检查是否已签退
                if (member.ReturnTime.HasValue)
                {
                    TempData["ErrorMessage"] = "该志愿者已经签退";
                    return RedirectToAction("ActivityParticipants", new { id = activityId });
                }

                // 更新签退时间
                DateTime now = DateTime.Now;
                bool result = _activityService.UpdateMemberReturnTime(activityId, volunteerId, now);

                if (result)
                {
                    // 计算志愿时长
                    int hoursSpent = _activityService.CalculateVolunteerHours(member.SignTime.Value, now);

                    TempData["SuccessMessage"] = $"志愿者 {member.volunteer}(ID:{volunteerId}) 签退成功，志愿时长 {hoursSpent} 小时";
                }
                else
                {
                    TempData["ErrorMessage"] = "签退操作失败";
                }

                return RedirectToAction("ActivityParticipants", new { id = activityId });
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"签退出错: {ex.Message}";
                return RedirectToAction("ActivityParticipants", new { id = activityId });
            }
        }
        #endregion
    }
}