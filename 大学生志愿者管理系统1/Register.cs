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

namespace 大学生志愿者管理系统1
{
    public partial class Register : Form
    {
        private UserService _userService; // 添加UserService引用

        public Register()
        {
            InitializeComponent();
            _userService = new UserService(); // 初始化UserService
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (txtName.Text == "" || txtPwd.Text == "")
            {
                MessageBox.Show("用户名或密码不能为空");
                return;
            }

            try
            {
                // 使用业务逻辑层注册志愿者账号
                bool result = _userService.RegisterVolunteer(txtName.Text, txtPwd.Text, txtEmail.Text);

                if (result)
                {
                    MessageBox.Show("注册成功");
                    this.Close();
                }
                else
                {
                    MessageBox.Show("用户名已存在");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"注册过程中出错: {ex.Message}");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Register_Load(object sender, EventArgs e)
        {
            // 空方法，保留原有的事件处理器
        }
    }
}