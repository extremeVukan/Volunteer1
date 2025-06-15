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
    public partial class Login : Form
    {
        private UserService _userService; // 添加UserService引用

        public Login()
        {
            InitializeComponent();
            _userService = new UserService(); // 初始化UserService
        }

        public static string username = "";
        public static string Vol_ID = "";
        public static string Emp_ID = "";
        public static int User = 0; // 身份编码 0为志愿者，1为管理员，2为平台管理员

        private void btnyes_Click(object sender, EventArgs e)
        {
            if (txtName.Text == "" || txtPwd.Text == "")
            {
                MessageBox.Show("请输入用户名或密码");
                return;
            }

            try
            {
                // 确定用户类型
                int userType = 0;
                if (radioButton1.Checked)
                {
                    userType = 0; // 志愿者
                }
                else if (radioButton2.Checked)
                {
                    userType = 1; // 管理员
                }
                else if (radioButton3.Checked)
                {
                    userType = 2; // 平台管理员
                }

                // 使用业务逻辑层验证登录
                var (success, type, userId, userName) = _userService.Login(txtName.Text, txtPwd.Text, userType);

                if (success)
                {
                    username = userName;
                    User = type;
                    Form1.Aflag = true;

                    switch (type)
                    {
                        case 0: // 志愿者
                            Vol_ID = userId.ToString();
                            Form1.flag = 1;
                            MessageBox.Show("登录成功");
                            this.DialogResult = DialogResult.OK;
                            this.Close();
                            break;

                        case 1: // 管理员
                            Emp_ID = userId.ToString();
                            Form1.flag = 2;
                            MessageBox.Show("登录成功");
                            this.Hide();
                            Tindex t1 = new Tindex();
                            t1.ShowDialog();
                            break;

                        case 2: // 平台管理员
                            Emp_ID = userId.ToString();
                            Form1.flag = 2;
                            MessageBox.Show("欢迎回来");
                            this.Hide();
                            Tindex t2 = new Tindex();
                            t2.ShowDialog();
                            break;
                    }
                }
                else
                {
                    MessageBox.Show("用户名或密码错误");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"登录过程中出错: {ex.Message}");
            }
        }

        private void btncancer_Click(object sender, EventArgs e)
        {
            this.Close();
            Form1 m1 = new Form1();
            m1.ShowDialog();
        }

        private void Login_Load(object sender, EventArgs e)
        {
            // 空方法，保留原有的事件处理器
        }
    }
}