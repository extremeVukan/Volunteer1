using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BLL; // 添加BLL引用
using DAL; // 添加DAL引用用于实体类

namespace 大学生志愿者管理系统1.start
{
    public partial class Person : Form
    {
        private UserService _userService; // 添加UserService引用

        public Person()
        {
            InitializeComponent();
            _userService = new UserService(); // 初始化UserService
        }

        private void Person_Load(object sender, EventArgs e)
        {
            try
            {
                if (Login.User == 2)
                {
                    // 平台管理员
                    var superAdmin = _userService.GetSuperAdminInfo(Login.username);
                    if (superAdmin != null)
                    {
                        txtId.Text = superAdmin.S_id.ToString();
                        txtName.Text = superAdmin.Sname;
                        txtPhone.Text = superAdmin.Semail;
                    }
                }
                else
                {
                    // 普通管理员
                    var admin = _userService.GetAdminInfo(Login.username);
                    if (admin != null)
                    {
                        txtId.Text = admin.admin_ID.ToString();
                        txtName.Text = admin.admin_Name;
                        txtSex.Text = admin.sex;
                        dtpBirth.Value = admin.birth_date ?? DateTime.Now;
                        dtpHire.Value = admin.hire_date ?? DateTime.Now;
                        txtAdress.Text = admin.address;
                        txtPhone.Text = admin.telephone;
                        txtWages.Text = admin.wages?.ToString() ?? "0";
                        txtSume.Text = admin.resume;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载个人信息时出错: {ex.Message}");
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (Login.User != 2) // 只有管理员可以修改信息
            {
                try
                {
                    // 创建admin对象
                    adminT admin = new adminT
                    {
                        admin_ID = int.Parse(txtId.Text),
                        admin_Name = txtName.Text,
                        sex = txtSex.Text,
                        birth_date = dtpBirth.Value,
                        hire_date = dtpHire.Value,
                        address = txtAdress.Text,
                        telephone = txtPhone.Text,
                        wages = int.Parse(txtWages.Text),
                        resume = txtSume.Text
                    };

                    // 调用更新方法
                    bool result = _userService.UpdateAdminInfo(admin);
                    if (result)
                    {
                        MessageBox.Show("修改成功");
                    }
                    else
                    {
                        MessageBox.Show("修改失败");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"更新个人信息时出错: {ex.Message}");
                }
            }
        }

        private void btnCanser_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}