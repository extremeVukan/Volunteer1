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
                // 检查志愿者表中是否有同名用户
                if (context.volunteerT.Any(v => v.AName == name))
                {
                    return false;
                }

                // 检查管理员表中是否有同名用户
                if (context.adminT.Any(a => a.admin_Name == name))
                {
                    return false;
                }

                // 检查平台管理员表中是否有同名用户
                if (context.zhuguanT.Any(z => z.Sname == name))
                {
                    return false;
                }

                // 创建新志愿者
                var volunteer = new volunteerT
                {
                    Aid = GetNextVolunteerId(), // 分配新的ID
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
            catch (Exception ex)
            {
                // 添加详细的日志记录
                System.Diagnostics.Debug.WriteLine($"RegisterVolunteer失败: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// 获取下一个志愿者ID
        /// </summary>
        private int GetNextVolunteerId()
        {
            if (!context.volunteerT.Any())
            {
                return 1;
            }
            return context.volunteerT.Max(v => v.Aid) + 1;
        }
        /// <summary>
        /// 根据ID获取管理员信息
        /// </summary>
        public adminT GetAdminById(int adminId)
        {
            return context.adminT.Find(adminId);
        }

        /// <summary>
        /// 添加管理员
        /// </summary>
        public bool AddAdmin(adminT admin, string currentUsername)
        {
            try
            {
                // 如果ID为0或已存在，则自动分配下一个可用ID
                if (admin.admin_ID == 0 || context.adminT.Any(a => a.admin_ID == admin.admin_ID))
                {
                    admin.admin_ID = GetNextAdminId();
                }

                context.adminT.Add(admin);
                context.SaveChanges();

                // 记录日志
                AddLog(currentUsername, "添加", "adminT");

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
        public bool UpdateAdmin(adminT admin)
        {
            try
            {
                var existingAdmin = context.adminT.Find(admin.admin_ID);
                if (existingAdmin == null)
                    return false;

                // 更新属性
                existingAdmin.admin_Name = admin.admin_Name;
                existingAdmin.sex = admin.sex;
                existingAdmin.birth_date = admin.birth_date;
                existingAdmin.hire_date = admin.hire_date;
                existingAdmin.address = admin.address;
                existingAdmin.telephone = admin.telephone;
                existingAdmin.wages = admin.wages;
                existingAdmin.resume = admin.resume;

                context.SaveChanges();

                // 记录日志
                AddLog(admin.admin_Name, "更新", "adminT");

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 修改超级管理员密码
        /// </summary>
        public bool ChangeSuperAdminPassword(string username, string currentPassword, string newPassword)
        {
            try
            {
                var admin = context.zhuguanT.FirstOrDefault(a => a.Sname == username && a.Semail == currentPassword);
                if (admin == null)
                    return false;

                admin.Semail = newPassword;
                context.SaveChanges();

                // 记录日志
                AddLog(username, "修改密码", "zhuguanT");

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
        /// <summary>
        /// 获取下一个管理员ID
        /// </summary>
        public int GetNextAdminId()
        {
            if (!context.adminT.Any())
            {
                return 1;
            }
            return context.adminT.Max(a => a.admin_ID) + 1;
        }
        /// <summary>
        /// 检查管理员ID是否存在
        /// </summary>
        public bool IsAdminIdExists(int adminId)
        {
            return context.adminT.Any(a => a.admin_ID == adminId);
        }

        /// <summary>
        /// 检查管理员名称是否存在
        /// </summary>
        public bool IsAdminNameExists(string adminName)
        {
            return context.adminT.Any(a => a.admin_Name == adminName);
        }
    }
}