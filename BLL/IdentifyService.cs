using System;
using System.Collections.Generic;
using System.Linq;
using DAL;

namespace BLL
{
    public class IdentifyService : BaseService
    {
        /// <summary>
        /// 申请志愿者证件
        /// </summary>
        public bool ApplyForIdentify(int volunteerId, string volunteerName, string phone, string province, string city, string address)
        {
            try
            {
                // 检查是否已经申请过
                var existingIdentify = context.VolIdentifyT.FirstOrDefault(i => i.VName == volunteerName);
                if (existingIdentify != null)
                {
                    return false;
                }

                // 检查志愿时长
                var volunteer = context.volunteerT.Find(volunteerId);
                if (volunteer == null || Convert.ToInt32( volunteer.Act_Time) < 10)
                {
                    return false;
                }

                // 创建申请
                var identify = new VolIdentifyT
                {
                    VID = volunteerId.ToString(),
                    VName = volunteerName,
                    Phone = phone,
                    Province = province,
                    City = city,
                    Address = address,
                    Status = "未审核"
                };

                context.VolIdentifyT.Add(identify);
                context.SaveChanges();

                AddLog(volunteerName, "申请证件", "VolIdentifyT");
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 获取志愿者证件状态
        /// </summary>
        public string GetVolunteerIdentifyStatus(string volunteerName)
        {
            var identify = context.VolIdentifyT.FirstOrDefault(i => i.VName == volunteerName);
            if (identify == null)
            {
                return "未拥有";
            }
            return identify.Status;
        }

        /// <summary>
        /// 获取所有证件申请
        /// </summary>
        public List<VolIdentifyT> GetAllIdentifies()
        {
            return context.VolIdentifyT.ToList();
        }

        /// <summary>
        /// 审核证件申请
        /// </summary>
        public bool ApproveIdentify(int identifyId, int empId)
        {
            try
            {
                var identify = context.VolIdentifyT.Find(identifyId);
                if (identify == null)
                {
                    return false;
                }

                identify.Status = "已通过";
                identify.EMPID = empId; // 确保EMPID属性名称正确

                context.SaveChanges();
                AddLog($"管理员ID:{empId}", "审核证件", "VolIdentifyT");
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}