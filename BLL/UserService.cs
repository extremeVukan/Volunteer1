using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace BLL
{
    public class UserService : BaseService
    {
        /// <summary>
        /// 用户登录验证
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        /// <param name="userType">用户类型：0=志愿者，1=管理员，2=平台管理员</param>
        /// <returns>登录结果，用户类型，用户ID，用户名</returns>
        public (bool Success, int UserType, int UserId, string UserName) Login(string username, string password, int userType)
        {
            try
            {
                switch (userType)
                {
                    case 0: // 志愿者
                        var volunteer = context.volunteerT.FirstOrDefault(v => v.AName == username && v.Atelephone == password);
                        if (volunteer != null)
                        {
                            AddLog(username, "登录", "volunteerT");
                            return (true, 0, volunteer.Aid, volunteer.AName);
                        }
                        break;
                    case 1: // 管理员
                        var admin = context.adminT.FirstOrDefault(a => a.admin_Name == username && a.telephone == password);
                        if (admin != null)
                        {
                            AddLog(username, "登录", "adminT");
                            return (true, 1, admin.admin_ID, admin.admin_Name);
                        }
                        break;
                    case 2: // 平台管理员
                        var superAdmin = context.zhuguanT.FirstOrDefault(z => z.Sname == username && z.Semail == password);
                        if (superAdmin != null)
                        {
                            AddLog(username, "登录", "zhuguanT");
                            return (true, 2, superAdmin.S_id, superAdmin.Sname);
                        }
                        break;
                }
                return (false, -1, -1, string.Empty);
            }
            catch (Exception)
            {
                return (false, -1, -1, string.Empty);
            }
        }

        /// <summary>
        /// 注册志愿者账号
        /// </summary>
        public bool RegisterVolunteer(string name, string phone, string email)
        {
            try
            {
                // 检查用户名是否存在
                if (context.volunteerT.Any(v => v.AName == name))
                {
                    return false;
                }

                // 创建新志愿者
                var volunteer = new volunteerT
                {
                    AName = name,
                    Atelephone = phone,
                    email = email,
                    Act_Time = "0"
                };

                context.volunteerT.Add(volunteer);
                context.SaveChanges();
                AddLog("System", "注册志愿者", "volunteerT");
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 修改管理员密码
        /// </summary>
        public bool ChangeAdminPassword(string adminName, string oldPassword, string newPassword)
        {
            try
            {
                var admin = context.adminT.FirstOrDefault(a => a.admin_Name == adminName && a.telephone == oldPassword);
                if (admin == null)
                {
                    return false;
                }

                admin.telephone = newPassword;
                context.SaveChanges();
                AddLog(adminName, "修改密码", "adminT");
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 获取管理员信息
        /// </summary>
        public adminT GetAdminInfo(string adminName)
        {
            return context.adminT.FirstOrDefault(a => a.admin_Name == adminName);
        }

        /// <summary>
        /// 获取超级管理员信息
        /// </summary>
        public zhuguanT GetSuperAdminInfo(string adminName)
        {
            return context.zhuguanT.FirstOrDefault(z => z.Sname == adminName);
        }
        /// <summary>
        /// 获取所有管理员
        /// </summary>
        public List<adminT> GetAllAdmins()
        {
            return context.adminT.ToList();
        }

        /// <summary>
        /// 通过ID删除管理员
        /// </summary>
        public bool DeleteAdmin(int adminId, string currentUsername)
        {
            try
            {
                var admin = context.adminT.Find(adminId);
                if (admin == null)
                {
                    return false;
                }

                context.adminT.Remove(admin);
                context.SaveChanges();

                // 记录日志
                AddLog(currentUsername, "删除", "AdminT表");

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 更新管理员信息
        /// </summary>
        public bool UpdateAdminInfo(adminT admin)
        {
            try
            {
                var existingAdmin = context.adminT.Find(admin.admin_ID);
                if (existingAdmin == null)
                {
                    return false;
                }

                existingAdmin.admin_Name = admin.admin_Name;
                existingAdmin.sex = admin.sex;
                existingAdmin.birth_date = admin.birth_date;
                existingAdmin.hire_date = admin.hire_date;
                existingAdmin.address = admin.address;
                existingAdmin.telephone = admin.telephone;
                existingAdmin.wages = admin.wages;
                existingAdmin.resume = admin.resume;

                context.SaveChanges();
                AddLog(admin.admin_Name, "更新信息", "adminT");
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 获取志愿者信息
        /// </summary>
        public volunteerT GetVolunteerInfo(int volunteerId)
        {
            return context.volunteerT.Find(volunteerId);
        }

        /// <summary>
        /// 获取志愿者信息
        /// </summary>
        public volunteerT GetVolunteerInfoByName(string volunteerName)
        {
            return context.volunteerT.FirstOrDefault(v => v.AName == volunteerName);
        }
    }
}