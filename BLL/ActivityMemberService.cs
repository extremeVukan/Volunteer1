using System;
using System.Collections.Generic;
using System.Linq;
using DAL;

namespace BLL
{
    public class ActivityMemberService : BaseService
    {
        /// <summary>
        /// 获取活动详情
        /// </summary>
        public ActivityT GetActivityById(int activityId)
        {
            return context.ActivityT.Find(activityId);
        }

        /// <summary>
        /// 检查志愿者是否已签到
        /// </summary>
        public bool CheckIfSignedIn(int activityId, int volunteerId)
        {
            var member = context.ACTMember.FirstOrDefault(m => m.ACTID == activityId && m.Volunteerid == volunteerId);
            // SignTime是DateTime?类型
            return member != null && member.SignTime.HasValue;
        }

        /// <summary>
        /// 志愿者签到
        /// </summary>
        public bool SignIn(int activityId, int volunteerId)
        {
            try
            {
                var member = context.ACTMember.FirstOrDefault(m => m.ACTID == activityId && m.Volunteerid == volunteerId);
                if (member == null)
                {
                    return false;
                }

                // SignTime是DateTime?类型，直接设置为当前时间
                member.SignTime = DateTime.Now;
                context.SaveChanges();

                AddLog($"志愿者ID:{volunteerId}", "签到", "ACTMember");
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 志愿者签退
        /// </summary>
        public bool SignOut(int activityId, int volunteerId)
        {
            try
            {
                var member = context.ACTMember.FirstOrDefault(m => m.ACTID == activityId && m.Volunteerid == volunteerId);
                // 检查是否已签到
                if (member == null || !member.SignTime.HasValue)
                {
                    return false;
                }

                // 设置签退时间
                DateTime signOutTime = DateTime.Now;
                member.ReturnTime = signOutTime;

                // 计算时长
                DateTime signInTime = member.SignTime.Value;
                TimeSpan duration = signOutTime - signInTime;
                int hours = (int)duration.TotalHours;
                int minutes = (int)(duration.TotalMinutes % 60);
                string formattedDuration = $"{hours}:{minutes:D2}";

                // 设置时间字符串
                member.Time = formattedDuration;

                context.SaveChanges();

                AddLog($"志愿者ID:{volunteerId}", "签退", "ACTMember");
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 获取志愿者签到情况
        /// </summary>
        public (string SignInTime, string SignOutTime) GetVolunteerSignInfo(int activityId, int volunteerId)
        {
            var member = context.ACTMember.FirstOrDefault(m => m.ACTID == activityId && m.Volunteerid == volunteerId);
            if (member == null)
            {
                return (null, null);
            }

            // 将DateTime?转为字符串返回
            return (
                member.SignTime.HasValue ? member.SignTime.Value.ToString("yyyy-MM-dd HH:mm:ss") : null,
                member.ReturnTime.HasValue ? member.ReturnTime.Value.ToString("yyyy-MM-dd HH:mm:ss") : null
            );
        }
    }
}