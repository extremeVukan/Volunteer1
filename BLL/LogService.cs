using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using DAL;

namespace BLL
{
    public class LogService : BaseService
    {
        /// <summary>
        /// 获取所有日志
        /// </summary>
        public List<dalogT> GetAllLogs()
        {
            return context.dalogT.ToList();
        }

        /// <summary>
        /// 分页获取日志
        /// </summary>
        public List<dalogT> GetLogsWithPaging(int pageIndex, int pageSize)
        {
            return context.dalogT
                .OrderByDescending(l => l.action_date)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToList();
        }

        /// <summary>
        /// 获取日志总数
        /// </summary>
        public int GetTotalLogCount()
        {
            return context.dalogT.Count();
        }

        /// <summary>
        /// 获取日志总页数
        /// </summary>
        public int GetTotalPageCount(int pageSize)
        {
            int recordCount = GetTotalLogCount();
            int pageCount = recordCount / pageSize;
            if (recordCount % pageSize > 0)
            {
                pageCount++;
            }
            return pageCount;
        }
    }
}