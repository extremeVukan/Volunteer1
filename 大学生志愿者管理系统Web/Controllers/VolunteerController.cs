using System;
using System.Collections.Generic;
using System.Web.Mvc;
using BLL;
using DAL;

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

        // GET: Volunteer
        public ActionResult Index()
        {
            // 检查是否已登录
            if (Session["Username"] == null)
                return RedirectToAction("Login", "Account");

            var activities = _activityService.GetAllActivities();
            return View(activities);
        }

        // GET: Volunteer/ActivityDetails/5
        public ActionResult ActivityDetails(int id)
        {
            var activity = _activityService.GetActivityById(id);
            if (activity == null)
                return HttpNotFound();

            return View(activity);
        }

        // POST: Volunteer/ApplyActivity/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ApplyActivity(int id)
        {
            // 检查是否已登录
            if (Session["Username"] == null)
                return RedirectToAction("Login", "Account");

            try
            {
                int volunteerId = (int)Session["UserId"];

                // 获取活动成员
                var activityMembers = _activityService.GetActivityMembers(id);

                // 检查是否已参加
                bool hasJoined = false;
                foreach (var member in activityMembers)
                {
                    if (member.Volunteerid == volunteerId)
                    {
                        hasJoined = true;
                        break;
                    }
                }

                if (hasJoined)
                {
                    TempData["ErrorMessage"] = "你已经参加了这个活动";
                    return RedirectToAction("ActivityDetails", new { id = id });
                }

                // 获取活动信息
                var activity = _activityService.GetActivityById(id);

                // 检查活动状态
                if (activity.status == "已结束")
                {
                    TempData["ErrorMessage"] = "该活动已结束";
                    return RedirectToAction("ActivityDetails", new { id = id });
                }

                // 检查人数是否已满
                int neededPeople = Convert.ToInt32( activity.renshu);
                if (activityMembers.Count >= neededPeople)
                {
                    TempData["ErrorMessage"] = "人数已满";
                    return RedirectToAction("ActivityDetails", new { id = id });
                }

                // 检查是否已申请
                var applies = _applyService.GetAppliesByVolunteerId(volunteerId);
                bool hasApplied = false;
                foreach (var apply in applies)
                {
                    if (apply.Act_ID == id)
                    {
                        hasApplied = true;
                        break;
                    }
                }

                if (hasApplied)
                {
                    TempData["ErrorMessage"] = "已申请";
                    return RedirectToAction("ActivityDetails", new { id = id });
                }

                // 添加到申请队列
                bool result = _applyService.AddToApplyQueue(volunteerId, id);
                if (result)
                {
                    TempData["SuccessMessage"] = "添加成功";
                }
                else
                {
                    TempData["ErrorMessage"] = "添加失败";
                }

                return RedirectToAction("MyApplications");
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
                return RedirectToAction("Login", "Account");

            int volunteerId = (int)Session["UserId"];
            var applications = _applyService.GetAppliesByVolunteerId(volunteerId);
            return View(applications);
        }

        // GET: Volunteer/MyHours
        public ActionResult MyHours()
        {
            // 检查是否已登录
            if (Session["Username"] == null)
                return RedirectToAction("Login", "Account");

            int volunteerId = (int)Session["UserId"];
            int hours = _volunteerService.GetVolunteerHours(volunteerId);
            ViewBag.Hours = hours;
            return View();
        }

        // GET: Volunteer/ApplyIdentify
        public ActionResult ApplyIdentify()
        {
            // 检查是否已登录
            if (Session["Username"] == null)
                return RedirectToAction("Login", "Account");

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
                return RedirectToAction("MyHours");
            }

            return View();
        }

        // POST: Volunteer/ApplyIdentify
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ApplyIdentify(string phone, string province, string city, string address)
        {
            // 检查是否已登录
            if (Session["Username"] == null)
                return RedirectToAction("Login", "Account");

            try
            {
                int volunteerId = (int)Session["UserId"];
                string volunteerName = Session["Username"].ToString();

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
    }
}