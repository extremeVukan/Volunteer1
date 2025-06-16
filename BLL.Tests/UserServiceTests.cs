using Microsoft.VisualStudio.TestTools.UnitTesting;
using BLL;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using Moq;
using System.Linq.Expressions;

namespace BLL.Tests
{
    [TestClass]
    public class UserServiceTests
    {
        // 模拟的数据集合
        private readonly List<volunteerT> _volunteers;
        private readonly List<adminT> _admins;
        private readonly List<zhuguanT> _superAdmins;
        private readonly List<dalogT> _logs;

        // 被测试的服务
        private UserService _userService;

        // 模拟的数据库上下文
        private Mock<Model1> _mockContext;

        // 模拟的DbSet
        private Mock<DbSet<volunteerT>> _mockVolunteerSet;
        private Mock<DbSet<adminT>> _mockAdminSet;
        private Mock<DbSet<zhuguanT>> _mockZhuguanSet;
        private Mock<DbSet<dalogT>> _mockLogSet;

        public UserServiceTests()
        {
            // 初始化测试数据
            _volunteers = new List<volunteerT>
            {
                new volunteerT { Aid = 1, AName = "testVolunteer1", Atelephone = "password1", email = "email1@test.com", Act_Time = "10" },
                new volunteerT { Aid = 2, AName = "testVolunteer2", Atelephone = "password2", email = "email2@test.com", Act_Time = "5" }
            };

            _admins = new List<adminT>
            {
                new adminT { admin_ID = 1, admin_Name = "testAdmin1", telephone = "adminpass1", sex = "男", birth_date = DateTime.Now.AddYears(-30), hire_date = DateTime.Now.AddYears(-5), address = "Test Address 1", wages = 5000, resume = "Test Resume 1" },
                new adminT { admin_ID = 2, admin_Name = "testAdmin2", telephone = "adminpass2", sex = "女", birth_date = DateTime.Now.AddYears(-25), hire_date = DateTime.Now.AddYears(-2), address = "Test Address 2", wages = 6000, resume = "Test Resume 2" }
            };

            _superAdmins = new List<zhuguanT>
            {
                new zhuguanT { S_id = 1, Sname = "testSuperAdmin1", Semail = "superpass1" },
                new zhuguanT { S_id = 2, Sname = "testSuperAdmin2", Semail = "superpass2" }
            };

            _logs = new List<dalogT>();

            // 设置所有模拟对象
            SetupMocks();
        }

        [TestInitialize]
        public void Initialize()
        {
            // 在每个测试方法执行前重新设置模拟对象
            SetupMocks();
        }

        private void SetupMocks()
        {
            // 设置模拟的DbSet<volunteerT>
            _mockVolunteerSet = new Mock<DbSet<volunteerT>>();
            SetupDbSet(_mockVolunteerSet, _volunteers);

            // 设置模拟的DbSet<adminT>
            _mockAdminSet = new Mock<DbSet<adminT>>();
            SetupDbSet(_mockAdminSet, _admins);

            // 设置模拟的DbSet<zhuguanT>
            _mockZhuguanSet = new Mock<DbSet<zhuguanT>>();
            SetupDbSet(_mockZhuguanSet, _superAdmins);

            // 设置模拟的DbSet<dalogT>
            _mockLogSet = new Mock<DbSet<dalogT>>();
            SetupDbSet(_mockLogSet, _logs);

            // 设置模拟的Model1上下文
            _mockContext = new Mock<Model1>();
            _mockContext.Setup(c => c.volunteerT).Returns(_mockVolunteerSet.Object);
            _mockContext.Setup(c => c.adminT).Returns(_mockAdminSet.Object);
            _mockContext.Setup(c => c.zhuguanT).Returns(_mockZhuguanSet.Object);
            _mockContext.Setup(c => c.dalogT).Returns(_mockLogSet.Object);

            // 创建UserService的测试实例
            _userService = new UserServiceForTest(_mockContext.Object);
        }

        private void SetupDbSet<T>(Mock<DbSet<T>> mockSet, List<T> data) where T : class
        {
            var queryable = data.AsQueryable();

            mockSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryable.Provider);
            mockSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryable.Expression);
            mockSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            mockSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(() => queryable.GetEnumerator());

            // 设置Find方法
            mockSet.Setup(m => m.Find(It.IsAny<object[]>())).Returns<object[]>(ids =>
            {
                var id = (int)ids[0];
                if (typeof(T) == typeof(volunteerT))
                    return data.FirstOrDefault(x => ((volunteerT)(object)x).Aid == id) as T;
                else if (typeof(T) == typeof(adminT))
                    return data.FirstOrDefault(x => ((adminT)(object)x).admin_ID == id) as T;
                else if (typeof(T) == typeof(zhuguanT))
                    return data.FirstOrDefault(x => ((zhuguanT)(object)x).S_id == id) as T;
                return null;
            });

            // 设置Add方法
            mockSet.Setup(m => m.Add(It.IsAny<T>())).Returns<T>(entity =>
            {
                data.Add(entity);
                return entity;
            });

            // 设置Remove方法
            mockSet.Setup(m => m.Remove(It.IsAny<T>())).Returns<T>(entity =>
            {
                data.Remove(entity);
                return entity;
            });
        }

        [TestMethod]
        public void Login_WithValidVolunteerCredentials_ReturnsSuccess()
        {
            // Arrange
            string username = "testVolunteer1";
            string password = "password1";
            int userType = 0;

            // Act
            var result = _userService.Login(username, password, userType);

            // Assert
            Assert.IsTrue(result.Success);
            Assert.AreEqual(0, result.UserType);
            Assert.AreEqual(1, result.UserId);
            Assert.AreEqual("testVolunteer1", result.UserName);
        }

        [TestMethod]
        public void Login_WithValidAdminCredentials_ReturnsSuccess()
        {
            // Arrange
            string username = "testAdmin1";
            string password = "adminpass1";
            int userType = 1;

            // Act
            var result = _userService.Login(username, password, userType);

            // Assert
            Assert.IsTrue(result.Success);
            Assert.AreEqual(1, result.UserType);
            Assert.AreEqual(1, result.UserId);
            Assert.AreEqual("testAdmin1", result.UserName);
        }

        [TestMethod]
        public void Login_WithValidSuperAdminCredentials_ReturnsSuccess()
        {
            // Arrange
            string username = "testSuperAdmin1";
            string password = "superpass1";
            int userType = 2;

            // Act
            var result = _userService.Login(username, password, userType);

            // Assert
            Assert.IsTrue(result.Success);
            Assert.AreEqual(2, result.UserType);
            Assert.AreEqual(1, result.UserId);
            Assert.AreEqual("testSuperAdmin1", result.UserName);
        }

        [TestMethod]
        public void Login_WithInvalidCredentials_ReturnsFalse()
        {
            // Arrange
            string username = "testVolunteer1";
            string password = "wrongpassword";
            int userType = 0;

            // Act
            var result = _userService.Login(username, password, userType);

            // Assert
            Assert.IsFalse(result.Success);
            Assert.AreEqual(-1, result.UserType);
            Assert.AreEqual(-1, result.UserId);
            Assert.AreEqual(string.Empty, result.UserName);
        }

        [TestMethod]
        public void RegisterVolunteer_WithNewVolunteer_ReturnsTrue()
        {
            // Arrange
            string name = "newVolunteer";
            string phone = "newphone";
            string email = "new@email.com";

            // 设置模拟上下文的SaveChanges方法
            _mockContext.Setup(c => c.SaveChanges()).Returns(1);

            // Act
            bool result = _userService.RegisterVolunteer(name, phone, email);

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(3, _volunteers.Count);
            Assert.AreEqual(name, _volunteers.Last().AName);
            Assert.AreEqual(phone, _volunteers.Last().Atelephone);
            Assert.AreEqual(email, _volunteers.Last().email);
        }

        [TestMethod]
        public void RegisterVolunteer_WithExistingName_ReturnsFalse()
        {
            // Arrange
            string name = "testVolunteer1"; // 已存在的名称
            string phone = "newphone";
            string email = "new@email.com";

            // Act
            bool result = _userService.RegisterVolunteer(name, phone, email);

            // Assert
            Assert.IsFalse(result);
            Assert.AreEqual(2, _volunteers.Count); // 没有添加新志愿者
        }

        [TestMethod]
        public void GetAdminById_WithValidId_ReturnsCorrectAdmin()
        {
            // Arrange
            int adminId = 1;

            // Act
            var result = _userService.GetAdminById(adminId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("testAdmin1", result.admin_Name);
        }

        [TestMethod]
        public void GetAdminById_WithInvalidId_ReturnsNull()
        {
            // Arrange
            int adminId = 999; // 不存在的ID

            // Act
            var result = _userService.GetAdminById(adminId);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void AddAdmin_WithNewAdmin_ReturnsTrue()
        {
            // Arrange
            var newAdmin = new adminT
            {
                admin_ID = 0, // 应该自动分配
                admin_Name = "newAdmin",
                telephone = "newphone",
                sex = "男"
            };
            string currentUsername = "testSuperAdmin1";

            // 设置模拟上下文的SaveChanges方法
            _mockContext.Setup(c => c.SaveChanges()).Returns(1);

            // Act
            bool result = _userService.AddAdmin(newAdmin, currentUsername);

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(3, _admins.Count);
            Assert.AreEqual("newAdmin", _admins.Last().admin_Name);
            Assert.AreEqual(3, _admins.Last().admin_ID); // 应该自动分配ID 3
        }

        [TestMethod]
        public void UpdateAdmin_WithValidAdmin_ReturnsTrue()
        {
            // Arrange
            var updatedAdmin = new adminT
            {
                admin_ID = 1,
                admin_Name = "updatedAdmin",
                telephone = "updatedphone",
                sex = "女",
                address = "Updated Address"
            };

            // 设置模拟上下文的SaveChanges方法
            _mockContext.Setup(c => c.SaveChanges()).Returns(1);

            // Act
            bool result = _userService.UpdateAdmin(updatedAdmin);

            // Assert
            Assert.IsTrue(result);
            var admin = _admins.FirstOrDefault(a => a.admin_ID == 1);
            Assert.IsNotNull(admin);
            Assert.AreEqual("updatedAdmin", admin.admin_Name);
            Assert.AreEqual("updatedphone", admin.telephone);
            Assert.AreEqual("女", admin.sex);
            Assert.AreEqual("Updated Address", admin.address);
        }

        [TestMethod]
        public void UpdateAdmin_WithNonexistentAdmin_ReturnsFalse()
        {
            // Arrange
            var updatedAdmin = new adminT
            {
                admin_ID = 999, // 不存在的ID
                admin_Name = "updatedAdmin",
                telephone = "updatedphone"
            };

            // Act
            bool result = _userService.UpdateAdmin(updatedAdmin);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ChangeSuperAdminPassword_WithValidCredentials_ReturnsTrue()
        {
            // Arrange
            string username = "testSuperAdmin1";
            string currentPassword = "superpass1";
            string newPassword = "newpass1";

            // 设置模拟上下文的SaveChanges方法
            _mockContext.Setup(c => c.SaveChanges()).Returns(1);

            // Act
            bool result = _userService.ChangeSuperAdminPassword(username, currentPassword, newPassword);

            // Assert
            Assert.IsTrue(result);
            var superAdmin = _superAdmins.FirstOrDefault(a => a.Sname == username);
            Assert.IsNotNull(superAdmin);
            Assert.AreEqual(newPassword, superAdmin.Semail);
        }

        [TestMethod]
        public void ChangeSuperAdminPassword_WithInvalidCredentials_ReturnsFalse()
        {
            // Arrange
            string username = "testSuperAdmin1";
            string currentPassword = "wrongpass"; // 错误的密码
            string newPassword = "newpass1";

            // Act
            bool result = _userService.ChangeSuperAdminPassword(username, currentPassword, newPassword);

            // Assert
            Assert.IsFalse(result);
            var superAdmin = _superAdmins.FirstOrDefault(a => a.Sname == username);
            Assert.IsNotNull(superAdmin);
            Assert.AreEqual("superpass1", superAdmin.Semail); // 密码未更改
        }

        [TestMethod]
        public void ChangeAdminPassword_WithValidCredentials_ReturnsTrue()
        {
            // Arrange
            string adminName = "testAdmin1";
            string oldPassword = "adminpass1";
            string newPassword = "newadminpass1";

            // 设置模拟上下文的SaveChanges方法
            _mockContext.Setup(c => c.SaveChanges()).Returns(1);

            // Act
            bool result = _userService.ChangeAdminPassword(adminName, oldPassword, newPassword);

            // Assert
            Assert.IsTrue(result);
            var admin = _admins.FirstOrDefault(a => a.admin_Name == adminName);
            Assert.IsNotNull(admin);
            Assert.AreEqual(newPassword, admin.telephone);
        }

        [TestMethod]
        public void ChangeAdminPassword_WithInvalidCredentials_ReturnsFalse()
        {
            // Arrange
            string adminName = "testAdmin1";
            string oldPassword = "wrongpass"; // 错误的密码
            string newPassword = "newadminpass1";

            // Act
            bool result = _userService.ChangeAdminPassword(adminName, oldPassword, newPassword);

            // Assert
            Assert.IsFalse(result);
            var admin = _admins.FirstOrDefault(a => a.admin_Name == adminName);
            Assert.IsNotNull(admin);
            Assert.AreEqual("adminpass1", admin.telephone); // 密码未更改
        }

        [TestMethod]
        public void GetAdminInfo_WithValidName_ReturnsCorrectAdmin()
        {
            // Arrange
            string adminName = "testAdmin1";

            // Act
            var result = _userService.GetAdminInfo(adminName);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.admin_ID);
            Assert.AreEqual(adminName, result.admin_Name);
        }

        [TestMethod]
        public void GetAdminInfo_WithInvalidName_ReturnsNull()
        {
            // Arrange
            string adminName = "nonexistentAdmin";

            // Act
            var result = _userService.GetAdminInfo(adminName);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void GetSuperAdminInfo_WithValidName_ReturnsCorrectSuperAdmin()
        {
            // Arrange
            string superAdminName = "testSuperAdmin1";

            // Act
            var result = _userService.GetSuperAdminInfo(superAdminName);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.S_id);
            Assert.AreEqual(superAdminName, result.Sname);
        }

        [TestMethod]
        public void GetSuperAdminInfo_WithInvalidName_ReturnsNull()
        {
            // Arrange
            string superAdminName = "nonexistentSuperAdmin";

            // Act
            var result = _userService.GetSuperAdminInfo(superAdminName);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void GetAllAdmins_ReturnsAllAdmins()
        {
            // Act
            var result = _userService.GetAllAdmins();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual("testAdmin1", result[0].admin_Name);
            Assert.AreEqual("testAdmin2", result[1].admin_Name);
        }

        [TestMethod]
        public void DeleteAdmin_WithValidId_ReturnsTrue()
        {
            // Arrange
            int adminId = 1;
            string currentUsername = "testSuperAdmin1";

            // 设置模拟上下文的SaveChanges方法
            _mockContext.Setup(c => c.SaveChanges()).Returns(1);

            // Act
            bool result = _userService.DeleteAdmin(adminId, currentUsername);

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(1, _admins.Count);
            Assert.IsFalse(_admins.Any(a => a.admin_ID == adminId));
        }

        [TestMethod]
        public void DeleteAdmin_WithInvalidId_ReturnsFalse()
        {
            // Arrange
            int adminId = 999; // 不存在的ID
            string currentUsername = "testSuperAdmin1";

            // Act
            bool result = _userService.DeleteAdmin(adminId, currentUsername);

            // Assert
            Assert.IsFalse(result);
            Assert.AreEqual(2, _admins.Count);
        }

        [TestMethod]
        public void GetVolunteerInfo_WithValidId_ReturnsCorrectVolunteer()
        {
            // Arrange
            int volunteerId = 1;

            // Act
            var result = _userService.GetVolunteerInfo(volunteerId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("testVolunteer1", result.AName);
        }

        [TestMethod]
        public void GetVolunteerInfo_WithInvalidId_ReturnsNull()
        {
            // Arrange
            int volunteerId = 999; // 不存在的ID

            // Act
            var result = _userService.GetVolunteerInfo(volunteerId);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void GetVolunteerInfoByName_WithValidName_ReturnsCorrectVolunteer()
        {
            // Arrange
            string volunteerName = "testVolunteer1";

            // Act
            var result = _userService.GetVolunteerInfoByName(volunteerName);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Aid);
        }

        [TestMethod]
        public void GetVolunteerInfoByName_WithInvalidName_ReturnsNull()
        {
            // Arrange
            string volunteerName = "nonexistentVolunteer";

            // Act
            var result = _userService.GetVolunteerInfoByName(volunteerName);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void GetNextAdminId_ReturnsCorrectId()
        {
            // Act
            int result = _userService.GetNextAdminId();

            // Assert
            Assert.AreEqual(3, result); // 应该返回当前最大ID + 1
        }

        [TestMethod]
        public void IsAdminIdExists_WithExistingId_ReturnsTrue()
        {
            // Arrange
            int adminId = 1;

            // Act
            bool result = _userService.IsAdminIdExists(adminId);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsAdminIdExists_WithNonexistentId_ReturnsFalse()
        {
            // Arrange
            int adminId = 999; // 不存在的ID

            // Act
            bool result = _userService.IsAdminIdExists(adminId);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsAdminNameExists_WithExistingName_ReturnsTrue()
        {
            // Arrange
            string adminName = "testAdmin1";

            // Act
            bool result = _userService.IsAdminNameExists(adminName);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsAdminNameExists_WithNonexistentName_ReturnsFalse()
        {
            // Arrange
            string adminName = "nonexistentAdmin";

            // Act
            bool result = _userService.IsAdminNameExists(adminName);

            // Assert
            Assert.IsFalse(result);
        }
    }

    // 自定义UserService类，用于测试
    public class UserServiceForTest : UserService
    {
        public UserServiceForTest(Model1 context)
        {
            // 使用属性注入替换基类中的context
            this.GetType().BaseType.GetField("context", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).SetValue(this, context);
        }
    }
}