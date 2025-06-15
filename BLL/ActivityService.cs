using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using DAL;

namespace BLL
{
    public class ActivityService : BaseService
    {
        /// <summary>
        /// 获取所有活动
        /// </summary>
        public List<ActivityT> GetAllActivities()
        {
            return context.ActivityT.ToList();
        }

        /// <summary>
        /// 根据活动名称关键词搜索活动
        /// </summary>
        public List<ActivityT> SearchActivitiesByName(string nameKeyword)
        {
            return context.ActivityT.Where(a => a.activity_Name.Contains(nameKeyword)).ToList();
        }

        /// <summary>
        /// 根据活动类型获取活动
        /// </summary>
        public List<ActivityT> GetActivitiesByType(string activityType)
        {
            return context.ActivityT.Where(a => a.activity_type == activityType).ToList();
        }

        /// <summary>
        /// 根据活动发起人获取活动
        /// </summary>
        public List<ActivityT> GetActivitiesByHolder(string holderName)
        {
            return context.ActivityT.Where(a => a.Holder == holderName).ToList();
        }

        /// <summary>
        /// 根据ID获取活动
        /// </summary>
        public ActivityT GetActivityById(int activityId)
        {
            return context.ActivityT.Find(activityId);
        }

        /// <summary>
        /// 获取活动详细信息（包括图片路径）
        /// </summary>
        public DataTable GetActivityDetails(int activityId)
        {
            string sql = "SELECT * FROM ActivityT WHERE activity_ID = @activityId";
            return ExecuteQuery(sql, new SqlParameter("@activityId", activityId));
        }

        /// <summary>
        /// 获取下一个活动ID
        /// </summary>
        public int GetNextActivityId()
        {
            if (!context.ActivityT.Any())
            {
                return 1;
            }
            return context.ActivityT.Max(a => a.activity_ID) + 1;
        }

        /// <summary>
        /// 添加新活动
        /// </summary>
        public bool AddActivity(ActivityT activity, string imagePath, string username)
        {
            try
            {
                // 处理图片
                if (!string.IsNullOrEmpty(imagePath))
                {
                    string dateTime = DateTime.Now.ToString("yyyyMMddHHmmss");
                    string fileName = dateTime + Path.GetFileName(imagePath);
                    string targetFolder = Path.Combine(
                        AppDomain.CurrentDomain.BaseDirectory,
                        "Act_Images",
                        activity.activity_type);

                    // 确保目录存在
                    if (!Directory.Exists(targetFolder))
                    {
                        Directory.CreateDirectory(targetFolder);
                    }

                    string targetPath = Path.Combine(targetFolder, fileName);
                    File.Copy(imagePath, targetPath, true);

                    // 保存相对路径
                    activity.Image = $"\\Act_Images\\{activity.activity_type}\\{fileName}";
                }
                else
                {
                    activity.Image = "暂无图片.gif";
                }

                context.ActivityT.Add(activity);
                context.SaveChanges();

                AddLog(username, "添加", "ActivityT");
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 更新活动信息
        /// </summary>
        public bool UpdateActivity(ActivityT activity, string imagePath, string username)
        {
            try
            {
                var existingActivity = context.ActivityT.Find(activity.activity_ID);
                if (existingActivity == null)
                {
                    return false;
                }

                // 处理图片
                if (!string.IsNullOrEmpty(imagePath))
                {
                    string dateTime = DateTime.Now.ToString("yyyyMMddHHmmss");
                    string fileName = dateTime + Path.GetFileName(imagePath);
                    string targetFolder = Path.Combine(
                        AppDomain.CurrentDomain.BaseDirectory,
                        "Act_Images",
                        activity.activity_type);

                    // 确保目录存在
                    if (!Directory.Exists(targetFolder))
                    {
                        Directory.CreateDirectory(targetFolder);
                    }

                    string targetPath = Path.Combine(targetFolder, fileName);
                    File.Copy(imagePath, targetPath, true);

                    // 保存相对路径
                    activity.Image = $"\\Act_Images\\{activity.activity_type}\\{fileName}";
                }

                existingActivity.activity_Name = activity.activity_Name;
                existingActivity.activity_type = activity.activity_type;
                existingActivity.addtime = activity.addtime;
                existingActivity.stoptime = activity.stoptime;
                existingActivity.place = activity.place;
                existingActivity.renshu = activity.renshu;
                existingActivity.descn = activity.descn;

                if (activity.Image != null)
                {
                    existingActivity.Image = activity.Image;
                }

                context.SaveChanges();
                AddLog(username, "修改", "ActivityT");
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 删除活动
        /// </summary>
        public bool DeleteActivity(int activityId, string username)
        {
            try
            {
                var activity = context.ActivityT.Find(activityId);
                if (activity == null)
                {
                    return false;
                }

                context.ActivityT.Remove(activity);
                context.SaveChanges();
                AddLog(username, "删除", "ActivityT");
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 结束活动
        /// </summary>
        public bool EndActivity(int activityId, string username)
        {
            try
            {
                var activity = context.ActivityT.Find(activityId);
                if (activity == null)
                {
                    return false;
                }

                activity.status = "已结束";
                context.SaveChanges();
                AddLog(username, "结束活动", "ActivityT");
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 获取活动参与者
        /// </summary>
        public List<ACTMember> GetActivityMembers(int activityId)
        {
            return context.ACTMember.Where(m => m.ACTID == activityId).ToList();
        }
    }
}
