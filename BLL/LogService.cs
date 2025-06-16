using System;
using System.Collections.Generic;
using System.Linq;
using DAL;

namespace BLL
{
    public class LogService : BaseService
    {
        /// <summary>
        /// 获取最近的日志记录
        /// </summary>
        public List<dalogT> GetRecentLogs(int count)
        {
            try
            {
                return context.dalogT
                    .OrderByDescending(l => l.action_date)
                    .Take(count)
                    .ToList();
            }
            catch
            {
                return new List<dalogT>();
            }
        }

        /// <summary>
        /// 获取分页的日志记录
        /// </summary>
        public List<dalogT> GetPaginatedLogs(int page, int pageSize)
        {
            try
            {
                return context.dalogT
                    .OrderByDescending(l => l.action_date)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();
            }
            catch
            {
                return new List<dalogT>();
            }
        }

        /// <summary>
        /// 获取日志总数
        /// </summary>
        public int GetTotalLogsCount()
        {
            try
            {
                return context.dalogT.Count();
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// 按用户名筛选日志
        /// </summary>
        public List<dalogT> GetLogsByUsername(string username, int page = 1, int pageSize = 20)
        {
            try
            {
                return context.dalogT
                    .Where(l => l.username == username)
                    .OrderByDescending(l => l.action_date)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();
            }
            catch
            {
                return new List<dalogT>();
            }
        }

        /// <summary>
        /// 按操作类型筛选日志
        /// </summary>
        public List<dalogT> GetLogsByType(string logType, int page = 1, int pageSize = 20)
        {
            try
            {
                return context.dalogT
                    .Where(l => l.log_type == logType)
                    .OrderByDescending(l => l.action_date)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();
            }
            catch
            {
                return new List<dalogT>();
            }
        }
        /// <summary>
        /// 获取所有日志记录
        /// </summary>
        public List<dalogT> GetAllLogs()
        {
            try
            {
                return context.dalogT
                    .OrderByDescending(l => l.action_date)
                    .ToList();
            }
            catch
            {
                return new List<dalogT>();
            }
        }
        /// <summary>
        /// 按日期范围筛选日志
        /// </summary>
        public List<dalogT> GetLogsByDateRange(DateTime startDate, DateTime endDate, int page = 1, int pageSize = 20)
        {
            try
            {
                return context.dalogT
                    .Where(l => l.action_date >= startDate && l.action_date <= endDate)
                    .OrderByDescending(l => l.action_date)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();
            }
            catch
            {
                return new List<dalogT>();
            }
        }
    }
}