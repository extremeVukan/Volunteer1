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
                if (volunteer == null || string.IsNullOrEmpty(volunteer.Act_Time))
                {
                    return false;
                }

                if (!int.TryParse(volunteer.Act_Time, out int hours) || hours < 10)
                {
                    return false;
                }

                // 创建申请
                var identify = new VolIdentifyT
                {
                    VID = volunteerId.ToString(),
                    VName = volunteerName,
                    Phone = phone?.Trim(),
                    Province = province,
                    City = city,
                    Address = address,
                    Status = "未审核"
                };

                context.VolIdentifyT.Add(identify);
                int result = context.SaveChanges();

                if (result > 0)
                {
                    AddLog(volunteerName, "申请志愿者证", "VolIdentifyT");
                    return true;
                }
                return false;
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
        /// <summary>
        /// 获取待审核的志愿者证申请
        /// </summary>
        public List<VolIdentifyT> GetPendingIdentifies()
        {
            return context.VolIdentifyT.Where(i => i.Status == "未审核").ToList();
        }

        /// <summary>
        /// 获取已通过的志愿者证
        /// </summary>
        public List<VolIdentifyT> GetApprovedIdentifies()
        {
            return context.VolIdentifyT.Where(i => i.Status == "已通过").ToList();
        }

        /// <summary>
        /// 获取志愿者证总数
        /// </summary>
        public int GetTotalIdentifies()
        {
            return context.VolIdentifyT.Count();
        }

        /// <summary>
        /// 拒绝志愿者证申请
        /// </summary>
        public bool RejectIdentify(int identifyId, int empId)
        {
            try
            {
                var identify = context.VolIdentifyT.Find(identifyId);
                if (identify == null)
                    return false;

                identify.Status = "已拒绝";
                identify.EMPID = empId;

                context.SaveChanges();
                AddLog($"管理员ID:{empId}", "拒绝证件申请", "VolIdentifyT");

                return true;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// 获取志愿者的证件信息
        /// </summary>
        public VolIdentifyT GetVolunteerIdentify(int volunteerId)
        {
            try
            {
                // 修改查询条件：使用VID字段查询，而不是EMPID
                return context.VolIdentifyT.FirstOrDefault(v => v.VID == volunteerId.ToString());
            }
            catch
            {
                return null;
            }
        }

    }
}