using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using DAL;

namespace BLL
{
    public class BaseService
    {
        protected Model1 context;

        public BaseService()
        {
            context = new Model1();
        }

        /// <summary>
        /// 执行SQL查询并返回DataTable
        /// </summary>
        protected DataTable ExecuteQuery(string sql, params SqlParameter[] parameters)
        {
            DataTable dt = new DataTable();
            using (var connection = new SqlConnection(context.Database.Connection.ConnectionString))
            {
                connection.Open();
                using (var command = new SqlCommand(sql, connection))
                {
                    if (parameters != null)
                    {
                        command.Parameters.AddRange(parameters);
                    }
                    using (var adapter = new SqlDataAdapter(command))
                    {
                        adapter.Fill(dt);
                    }
                }
            }
            return dt;
        }

        /// <summary>
        /// 执行SQL命令并返回受影响的行数
        /// </summary>
        protected int ExecuteNonQuery(string sql, params SqlParameter[] parameters)
        {
            using (var connection = new SqlConnection(context.Database.Connection.ConnectionString))
            {
                connection.Open();
                using (var command = new SqlCommand(sql, connection))
                {
                    if (parameters != null)
                    {
                        command.Parameters.AddRange(parameters);
                    }
                    return command.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// 记录系统日志
        /// </summary>
        protected void AddLog(string username, string logType, string actionTable)
        {
            var log = new dalogT
            {
                username = username,
                log_type = logType,
                action_date = DateTime.Now,
                action_table = actionTable
            };

            context.dalogT.Add(log);
            context.SaveChanges();
        }
    }
}