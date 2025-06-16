using System;
using System.Collections.Generic;
using System.Linq;
using DAL;

namespace BLL
{
    public class OrderService : BaseService
    {
        /// <summary>
        /// 获取特定用户的所有申请
        /// </summary>
        public List<OrderT> GetOrdersByUserName(string userName)
        {
            return context.OrderT.Where(o => o.UserName == userName).ToList();
        }

        /// <summary>
        /// 获取特定管理员的所有申请
        /// </summary>
        public List<OrderT> GetOrdersByHolder(string holderName)
        {
            return context.OrderT.Where(o => o.Holder == holderName).ToList();
        }

        /// <summary>
        /// 审核申请
        /// </summary>
        public bool ApproveOrder(int orderId, int empId)
        {
            try
            {
                var order = context.OrderT.Find(orderId);
                if (order == null)
                {
                    return false;
                }

                order.status = "已审核";
                order.EmpID = empId;

                // 添加到活动成员
                var member = new ACTMember
                {
                    ACTID = order.ActID,
                    ACTNAME = order.Act_Name,
                    Volunteerid = order.UserID,
                    volunteer = order.UserName,
                    PHONE = order.phone,
                    Time = "0" // 注意TIME是字符串类型
                };

                context.ACTMember.Add(member);
                context.SaveChanges();

                AddLog($"管理员ID:{empId}", "审核申请", "OrderT");
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 获取订单详情
        /// </summary>
        public OrderT GetOrderDetails(int orderId)
        {
            return context.OrderT.Find(orderId);
        }
        /// <summary>
        /// 获取特定管理员待审核的申请
        /// </summary>
        public List<OrderT> GetPendingOrdersByHolder(string holderName)
        {
            try
            {
                // 从数据库中查询指定Holder名下且状态为"未审核"的订单
                return context.OrderT
                    .Where(o => o.Holder == holderName && o.status == "未审核")
                    .ToList();
            }
            catch (Exception)
            {
                // 出现异常时返回空列表
                return new List<OrderT>();
            }
        }
    }
}