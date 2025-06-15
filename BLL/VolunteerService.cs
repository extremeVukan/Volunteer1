using System;
using System.Collections.Generic;
using System.Linq;
using DAL;

namespace BLL
{
    public class VolunteerService : BaseService
    {
        /// <summary>
        /// 获取志愿者累计时长
        /// </summary>
        public int GetVolunteerHours(int volunteerId)
        {
            var volunteer = context.volunteerT.Find(volunteerId);
            if (volunteer != null && !string.IsNullOrEmpty(volunteer.Act_Time))
            {
                if (int.TryParse(volunteer.Act_Time, out int hours))
                {
                    return hours;
                }
            }
            return 0;
        }

        /// <summary>
        /// 更新志愿者时长
        /// </summary>
        public bool UpdateVolunteerHours(int volunteerId, int additionalHours)
        {
            try
            {
                var volunteer = context.volunteerT.Find(volunteerId);
                if (volunteer == null)
                {
                    return false;
                }

                // 当前时长转为int
                int currentHours = 0;
                if (!string.IsNullOrEmpty(volunteer.Act_Time))
                {
                    int.TryParse(volunteer.Act_Time, out currentHours);
                }

                // 更新时长
                volunteer.Act_Time = (currentHours + additionalHours).ToString();
                context.SaveChanges();

                AddLog("System", "更新志愿时长", "volunteerT");
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 获取活动参与的志愿者
        /// </summary>
        public List<ACTMember> GetVolunteersByActivityId(int activityId)
        {
            return context.ACTMember.Where(m => m.ACTID == activityId).ToList();
        }

        /// <summary>
        /// 获取志愿者信息
        /// </summary>
        public volunteerT GetVolunteerById(int volunteerId)
        {
            return context.volunteerT.Find(volunteerId);
        }
    }
}