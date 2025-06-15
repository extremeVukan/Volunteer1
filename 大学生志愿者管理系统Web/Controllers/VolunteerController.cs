using BLL;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace 大学生志愿者管理系统Web.Controllers
{
    public class VolunteerController : Controller
    {
        private ActivityService _activityService;
        private ApplyService _applyService;
        private VolunteerService _volunteerService;
        private IdentifyService _identifyService;

        public VolunteerController()
        {
            _activityService = new ActivityService();
            _applyService = new ApplyService();
            _volunteerService = new VolunteerService();
            _identifyService = new IdentifyService();
        }

        // 辅助方法：处理活动图片路径
        private void ProcessImagePaths(IEnumerable<ActivityT> activities)
        {
            foreach (var activity in activities)
            {
                if (string.IsNullOrEmpty(activity.Image) || activity.Image.Contains("暂无图片"))
                {
                    activity.Image = "Content/Images/暂无图片.gif";
                }
                else
                {
                    // 转换路径格式
                    activity.Image = activity.Image.TrimStart('\\').Replace('\\', '/');
                }
            }
        }

        // 辅助方法：处理单个活动图片路径
        private void ProcessImagePath(ActivityT activity)
        {
            if (activity != null)
            {
                if (string.IsNullOrEmpty(activity.Image) || activity.Image.Contains("暂无图片"))
                {
                    activity.Image = "Content/Images/暂无图片.gif";
                }
                else
                {
                    // 转换路径格式
                    activity.Image = activity.Image.TrimStart('\\').Replace('\\', '/');
                }
            }
        }

        // GET: Volunteer
        public ActionResult Index()
        {
            try
            {
                // 如果已登录，获取志愿时长
                if (Session["UserId"] != null && int.TryParse(Session["UserId"].ToString(), out int volunteerId))
                {
                    int hours = _volunteerService.GetVolunteerHours(volunteerId);
                    ViewBag.VolunteerHours = hours;
                }

                // 获取所有活动
                var activities = _activityService.GetAllActivities();

                // 处理图片路径
                ProcessImagePaths(activities);

                return View(activities);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"加载活动数据出错: {ex.Message}";
                return View(Enumerable.Empty<ActivityT>());
            }
        }

        // GET: Volunteer/Search
        public ActionResult Search(string keyword)
        {
            try
            {
                if (string.IsNullOrEmpty(keyword))
                {
                    return RedirectToAction("Index");
                }

                // 获取志愿时长
                if (Session["UserId"] != null && int.TryParse(Session["UserId"].ToString(), out int volunteerId))
                {
                    int hours = _volunteerService.GetVolunteerHours(volunteerId);
                    ViewBag.VolunteerHours = hours;
                }

                // 搜索活动
                var activities = _activityService.SearchActivitiesByName(keyword);

                // 处理图片路径
                ProcessImagePaths(activities);

                ViewBag.SearchTerm = keyword;
                return View("Index", activities);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"搜索活动出错: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        // GET: Volunteer/Filter
        public ActionResult Filter(string activityType)
        {
            try
            {
                if (string.IsNullOrEmpty(activityType))
                {
                    return RedirectToAction("Index");
                }

                // 获取志愿时长
                if (Session["UserId"] != null && int.TryParse(Session["UserId"].ToString(), out int volunteerId))
                {
                    int hours = _volunteerService.GetVolunteerHours(volunteerId);
                    ViewBag.VolunteerHours = hours;
                }

                // 按类型筛选活动
                var activities = _activityService.GetActivitiesByType(activityType);

                // 处理图片路径
                ProcessImagePaths(activities);

                ViewBag.ActivityType = activityType;
                return View("Index", activities);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"筛选活动出错: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        // GET: Volunteer/ActivityDetails/5
        public ActionResult ActivityDetails(int id)
        {
            try
            {
                // 获取活动详情
                var activity = _activityService.GetActivityById(id);
                if (activity == null)
                {
                    TempData["ErrorMessage"] = "找不到指定的活动";
                    return RedirectToAction("Index");
                }

                // 处理图片路径
                ProcessImagePath(activity);

                return View(activity);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"查看活动详情出错: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        // GET: Volunteer/ApplyActivity/5
        public ActionResult ApplyActivity(int id)
        {
            // 检查是否已登录
            if (Session["Username"] == null)
            {
                TempData["ErrorMessage"] = "请登录后再申请参加活动";
                return RedirectToAction("Login", "Account", new { returnUrl = Url.Action("ApplyActivity", new { id = id }) });
            }

            try
            {
                int volunteerId = (int)Session["UserId"];
                string actId = id.ToString();

                // 获取活动
                var activity = _activityService.GetActivityById(id);
                if (activity == null)
                {
                    TempData["ErrorMessage"] = "找不到指定的活动";
                    return RedirectToAction("Index");
                }

                // 获取活动成员
                var activityMembers = _activityService.GetActivityMembers(id);

                // 检查是否已参加
                bool hasJoined = activityMembers.Any(m => m.Volunteerid == volunteerId);
                if (hasJoined)
                {
                    TempData["ErrorMessage"] = "你已经参加了这个活动";
                    return RedirectToAction("ActivityDetails", new { id = id });
                }

                // 检查活动状态
                if (activity.status == "已结束")
                {
                    TempData["ErrorMessage"] = "该活动已结束";
                    return RedirectToAction("ActivityDetails", new { id = id });
                }

                // 检查人数是否已满
                if (int.TryParse(activity.renshu.ToString(), out int neededPeople) && activityMembers.Count >= neededPeople)
                {
                    TempData["ErrorMessage"] = "人数已满";
                    return RedirectToAction("ActivityDetails", new { id = id });
                }

                // 检查是否已申请
                var applies = _applyService.GetAppliesByVolunteerId(volunteerId);
                bool hasApplied = applies.Any(a => a.Act_ID == id);
                if (hasApplied)
                {
                    TempData["ErrorMessage"] = "已申请";
                    return RedirectToAction("MyApplications");
                }

                // 添加到申请队列
                bool result = _applyService.AddToApplyQueue(volunteerId, id);
                if (result)
                {
                    TempData["SuccessMessage"] = "申请成功";
                    return RedirectToAction("MyApplications");
                }
                else
                {
                    TempData["ErrorMessage"] = "申请失败";
                    return RedirectToAction("ActivityDetails", new { id = id });
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"申请活动出错: {ex.Message}";
                return RedirectToAction("ActivityDetails", new { id = id });
            }
        }

        // GET: Volunteer/MyApplications
        public ActionResult MyApplications()
        {
            // 检查是否已登录
            if (Session["Username"] == null)
            {
                TempData["ErrorMessage"] = "请登录后查看申请";
                return RedirectToAction("Login", "Account");
            }

            try
            {
                int volunteerId = (int)Session["UserId"];
                var applications = _applyService.GetAppliesByVolunteerId(volunteerId);
                return View(applications);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"获取申请列表出错: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        // GET: Volunteer/DeleteApplication/5
        public ActionResult DeleteApplication(int id)
        {
            // 检查是否已登录
            if (Session["Username"] == null)
            {
                TempData["ErrorMessage"] = "请登录后操作";
                return RedirectToAction("Login", "Account");
            }

            try
            {
                bool result = _applyService.DeleteApply(id);
                if (result)
                {
                    TempData["SuccessMessage"] = "申请已删除";
                }
                else
                {
                    TempData["ErrorMessage"] = "删除申请失败";
                }
                return RedirectToAction("MyApplications");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"删除申请出错: {ex.Message}";
                return RedirectToAction("MyApplications");
            }
        }

        // GET: Volunteer/SubmitApplication/5
        public ActionResult SubmitApplication(int id)
        {
            // 检查是否已登录
            if (Session["Username"] == null)
            {
                TempData["ErrorMessage"] = "请登录后操作";
                return RedirectToAction("Login", "Account");
            }

            try
            {
                var apply = _applyService.GetApplyDetails(id);
                if (apply == null)
                {
                    TempData["ErrorMessage"] = "找不到指定的申请";
                    return RedirectToAction("MyApplications");
                }

                // 从Session中获取志愿者信息
                int volunteerId = (int)Session["UserId"];
                string volunteerName = Session["Username"].ToString();

                // 获取志愿者电话 - 从数据库中获取或添加表单
                var volunteer = _volunteerService.GetVolunteerById(volunteerId);
                string phone = volunteer != null ? volunteer.Atelephone : "";

                bool result = _applyService.SubmitApply(
                    volunteerId,
                    volunteerName,
                    phone,
                    Convert.ToInt32( apply.Act_ID),
                    apply.Act_Name,
                    apply.Holder
                );

                if (result)
                {
                    TempData["SuccessMessage"] = "申请已提交，等待审核";
                }
                else
                {
                    TempData["ErrorMessage"] = "提交申请失败";
                }
                return RedirectToAction("MyApplications");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"提交申请出错: {ex.Message}";
                return RedirectToAction("MyApplications");
            }
        }

        // GET: Volunteer/GetVolunteerHours
        public ActionResult GetVolunteerHours()
        {
            // 检查是否已登录
            if (Session["Username"] == null)
            {
                TempData["ErrorMessage"] = "请登录后查看志愿时长";
                return RedirectToAction("Login", "Account");
            }

            try
            {
                int volunteerId = (int)Session["UserId"];
                int hours = _volunteerService.GetVolunteerHours(volunteerId);
                ViewBag.Hours = hours;
                return View();
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"获取志愿时长出错: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        // GET: Volunteer/SelfCheck
        public ActionResult SelfCheck()
        {
            // 检查是否已登录
            if (Session["Username"] == null)
            {
                TempData["ErrorMessage"] = "请登录后查看个人信息";
                return RedirectToAction("Login", "Account");
            }

            try
            {
                int volunteerId = (int)Session["UserId"];
                var volunteer = _volunteerService.GetVolunteerById(volunteerId);
                return View(volunteer);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"获取个人信息出错: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        // GET: Volunteer/EditProfile
        public ActionResult EditProfile()
        {
            // 检查是否已登录
            if (Session["Username"] == null)
            {
                TempData["ErrorMessage"] = "请登录后编辑个人信息";
                return RedirectToAction("Login", "Account");
            }

            try
            {
                int volunteerId = (int)Session["UserId"];
                var volunteer = _volunteerService.GetVolunteerById(volunteerId);
                return View(volunteer);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"获取个人信息出错: {ex.Message}";
                return RedirectToAction("SelfCheck");
            }
        }

        // POST: Volunteer/EditProfile
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditProfile(volunteerT model)
        {
            // 检查是否已登录
            if (Session["Username"] == null)
            {
                TempData["ErrorMessage"] = "请登录后编辑个人信息";
                return RedirectToAction("Login", "Account");
            }

            try
            {
                int volunteerId = (int)Session["UserId"];

                // 确保ID一致
                model.Aid = volunteerId;

                bool result = _volunteerService.UpdateVolunteer(model, Session["Username"].ToString());
                if (result)
                {
                    TempData["SuccessMessage"] = "个人信息已更新";
                    return RedirectToAction("SelfCheck");
                }
                else
                {
                    TempData["ErrorMessage"] = "更新个人信息失败";
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"更新个人信息出错: {ex.Message}";
                return View(model);
            }
        }

        // GET: Volunteer/ApplyIdentify
        public ActionResult ApplyIdentify()
        {
            // 检查是否已登录
            if (Session["Username"] == null)
            {
                TempData["ErrorMessage"] = "请登录后申请志愿者证";
                return RedirectToAction("Login", "Account");
            }

            try
            {
                // 检查志愿者是否已申请证书
                string status = _identifyService.GetVolunteerIdentifyStatus(Session["Username"].ToString());

                if (status != "未拥有")
                {
                    TempData["ErrorMessage"] = "您已申请志愿者证";
                    return RedirectToAction("Index");
                }

                // 检查志愿时长
                int volunteerId = (int)Session["UserId"];
                int hours = _volunteerService.GetVolunteerHours(volunteerId);

                if (hours < 10)
                {
                    TempData["ErrorMessage"] = "很抱歉，您的志愿时长不足10小时";
                    return RedirectToAction("GetVolunteerHours");
                }

                // 获取用户信息
                var volunteer = _volunteerService.GetVolunteerById(volunteerId);
                return View(volunteer);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"申请志愿者证出错: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        // POST: Volunteer/ApplyIdentify
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ApplyIdentify(string phone, string province, string city, string address)
        {
            // 检查是否已登录
            if (Session["Username"] == null)
            {
                TempData["ErrorMessage"] = "请登录后申请志愿者证";
                return RedirectToAction("Login", "Account");
            }

            try
            {
                int volunteerId = (int)Session["UserId"];
                string volunteerName = Session["Username"].ToString();

                // 验证字段
                if (string.IsNullOrEmpty(phone) || string.IsNullOrEmpty(province) ||
                    string.IsNullOrEmpty(city) || string.IsNullOrEmpty(address))
                {
                    TempData["ErrorMessage"] = "请填写所有必填信息";
                    return View();
                }

                bool result = _identifyService.ApplyForIdentify(
                    volunteerId, volunteerName, phone, province, city, address);

                if (result)
                {
                    TempData["SuccessMessage"] = "申请成功，等待审核";
                }
                else
                {
                    TempData["ErrorMessage"] = "申请失败";
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"申请志愿者证出错: {ex.Message}";
                return View();
            }
        }

        // GET: Volunteer/CheckIdentifyStatus
        public ActionResult CheckIdentifyStatus()
        {
            // 检查是否已登录
            if (Session["Username"] == null)
            {
                TempData["ErrorMessage"] = "请登录后查看志愿者证状态";
                return RedirectToAction("Login", "Account");
            }

            try
            {
                string status = _identifyService.GetVolunteerIdentifyStatus(Session["Username"].ToString());
                ViewBag.Status = status;
                return View();
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"查看志愿者证状态出错: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        // GET: Volunteer/MyActivities
        public ActionResult MyActivities()
        {
            // 检查是否已登录
            if (Session["Username"] == null)
            {
                TempData["ErrorMessage"] = "请登录后查看我的活动";
                return RedirectToAction("Login", "Account");
            }

            try
            {
                int volunteerId = (int)Session["UserId"];
                var activities = _activityService.GetActivitiesByVolunteerId(volunteerId);

                // 处理图片路径
                ProcessImagePaths(activities);

                return View(activities);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"获取我的活动出错: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        // 添加一个方法用于辅助
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index");
        }
    }
}