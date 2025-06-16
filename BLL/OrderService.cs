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
        /// <summary>
        /// 添加订单
        /// </summary>
        public bool AddOrder(OrderT order)
        {
            try
            {
                // 检查并处理可能为 null 的字段
                if (order.UserID == null)
                {
                    // 可以设置默认值，或者在这里确保数据库可以接受 null
                    // 例如：order.UserID = 0; // 或其他默认值

                    // 或者，如果该字段不应该为 null，可以返回 false 表示数据不合法
                    return false;
                }

                context.OrderT.Add(order);
                context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 修改订单信息
        /// </summary>
        public bool UpdateOrder(OrderT order)
        {
            try
            {
                // 检查并处理可能为 null 的字段
                if (order.UserID == null)
                {
                    // 可以设置默认值
                    // order.UserID = 0; // 或其他默认值

                    // 或返回 false
                    return false;
                }

                var existingOrder = context.OrderT.Find(order.OrderID);
                if (existingOrder == null)
                    return false;

                // 更新字段
                existingOrder.UserName = order.UserName;
                existingOrder.UserID = order.UserID;
                // 更新其他字段...

                context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}