using System;
using System.Collections.Generic;
using System.Linq;
using DAL;

namespace BLL
{
    public class ApplyService : BaseService
    {
        /// <summary>
        /// 获取志愿者的所有申请
        /// </summary>
        public List<ACTapply_T> GetAppliesByVolunteerId(int volunteerId)
        {
            return context.ACTapply_T.Where(a => a.Vol_ID == volunteerId).ToList();
        }

        /// <summary>
        /// 添加活动到申请队列
        /// </summary>
        public bool AddToApplyQueue(int volunteerId, int activityId)
        {
            try
            {
                // 检查是否已经申请过
                var existingApply = context.ACTapply_T.FirstOrDefault(a => a.Vol_ID == volunteerId && a.Act_ID == activityId);
                if (existingApply != null)
                {
                    existingApply.Qty++;
                    context.SaveChanges();
                    return true;
                }

                // 获取活动信息
                var activity = context.ActivityT.Find(activityId);
                if (activity == null)
                {
                    return false;
                }

                // 添加申请
                var apply = new ACTapply_T
                {
                    Vol_ID = volunteerId,
                    Act_ID = activityId,
                    Act_Name = activity.activity_Name,
                    Act_place = activity.place,
                    Need = activity.renshu,
                    Holder = activity.Holder,
                    Qty = 1
                };

                context.ACTapply_T.Add(apply);
                context.SaveChanges();
                AddLog($"志愿者ID:{volunteerId}", "添加申请", "ACTapply_T");
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 删除申请
        /// </summary>
        public bool DeleteApply(int applyId)
        {
            try
            {
                var apply = context.ACTapply_T.Find(applyId);
                if (apply == null)
                {
                    return false;
                }

                context.ACTapply_T.Remove(apply);
                context.SaveChanges();
                AddLog($"志愿者ID:{apply.Vol_ID}", "删除申请", "ACTapply_T");
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 提交正式申请
        /// </summary>
        public bool SubmitApply(int volunteerId, string volunteerName, string phone, int activityId, string activityName, string holder)
        {
            try
            {
                // 创建订单
                var order = new OrderT
                {
                    UserID = volunteerId,
                    UserName = volunteerName,
                    OrderDate = DateTime.Today,
                    phone = phone,
                    ActID = activityId,
                    Act_Name = activityName,
                    Holder = holder,
                    status = "未审核"
                    // EmpID默认为null
                };

                context.OrderT.Add(order);

                // 删除申请队列中的项目
                var apply = context.ACTapply_T.FirstOrDefault(a => a.Vol_ID == volunteerId && a.Act_ID == activityId);
                if (apply != null)
                {
                    context.ACTapply_T.Remove(apply);
                }

                context.SaveChanges();
                AddLog(volunteerName, "提交申请", "OrderT");
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 获取申请详情
        /// </summary>
        public ACTapply_T GetApplyDetails(int applyId)
        {
            return context.ACTapply_T.Find(applyId);
        }
    }
}