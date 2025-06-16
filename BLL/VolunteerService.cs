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
        /// 获取志愿者总时长
        /// </summary>
        public int GetTotalVolunteerHours()
        {
            try
            {
                int total = 0;
                foreach (var volunteer in context.volunteerT)
                {
                    if (!string.IsNullOrEmpty(volunteer.Act_Time) && int.TryParse(volunteer.Act_Time, out int hours))
                    {
                        total += hours;
                    }
                }
                return total;
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// 获取平均志愿者时长
        /// </summary>
        public double GetAverageVolunteerHours()
        {
            try
            {
                int total = 0;
                int count = 0;

                foreach (var volunteer in context.volunteerT)
                {
                    if (!string.IsNullOrEmpty(volunteer.Act_Time) && int.TryParse(volunteer.Act_Time, out int hours))
                    {
                        total += hours;
                        count++;
                    }
                }

                if (count == 0)
                    return 0;

                return Math.Round((double)total / count, 2);
            }
            catch
            {
                return 0;
            }
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

        /// <summary>
        /// 获取所有志愿者
        /// </summary>
        public List<volunteerT> GetAllVolunteers()
        {
            return context.volunteerT.ToList();
        }

        /// <summary>
        /// 添加志愿者
        /// </summary>
        public bool AddVolunteer(volunteerT volunteer, string currentUsername)
        {
            try
            {
                // 如果ID为0或已存在，则自动分配下一个可用ID
                if (volunteer.Aid == 0 || context.volunteerT.Any(v => v.Aid == volunteer.Aid))
                {
                    volunteer.Aid = GetNextVolunteerId();
                }

                // 检查名称是否已存在
                if (context.volunteerT.Any(v => v.AName == volunteer.AName))
                {
                    return false;
                }

                context.volunteerT.Add(volunteer);
                context.SaveChanges();

                AddLog(currentUsername, "添加", "Volunteer表");
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 更新志愿者信息
        /// </summary>
        public bool UpdateVolunteer(volunteerT volunteer, string currentUsername)
        {
            try
            {
                var existingVolunteer = context.volunteerT.Find(volunteer.Aid);
                if (existingVolunteer == null)
                {
                    return false;
                }

                existingVolunteer.AName = volunteer.AName;
                existingVolunteer.Atelephone = volunteer.Atelephone;
                existingVolunteer.email = volunteer.email;

                context.SaveChanges();

                AddLog(currentUsername, "修改", "Volunteer表");
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 删除志愿者
        /// </summary>
        public bool DeleteVolunteer(int volunteerId, string currentUsername)
        {
            try
            {
                var volunteer = context.volunteerT.Find(volunteerId);
                if (volunteer == null)
                {
                    return false;
                }

                context.volunteerT.Remove(volunteer);
                context.SaveChanges();

                AddLog(currentUsername, "删除", "Volunteer表");
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 获取下一个志愿者ID
        /// </summary>
        public int GetNextVolunteerId()
        {
            if (!context.volunteerT.Any())
            {
                return 1;
            }
            return context.volunteerT.Max(v => v.Aid) + 1;
        }
    }
}