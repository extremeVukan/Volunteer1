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
    public partial class Changepwd : Form
    {
        private UserService _userService; // 添加UserService引用

        public Changepwd()
        {
            InitializeComponent();
            _userService = new UserService(); // 初始化UserService
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (txtOldpwd.Text == "")
            {
                MessageBox.Show("请输入原密码");
                return;
            }

            try
            {
                // 使用业务逻辑层验证原密码并修改密码
                bool result = _userService.ChangeAdminPassword(txtName.Text, txtOldpwd.Text, txtNewpwd.Text);

                if (!result)
                {
                    MessageBox.Show("原密码错误请重新输入");
                    return;
                }

                if (txtNewpwd.Text == "" || txtNewpwd2.Text == "")
                {
                    MessageBox.Show("请输入新密码");
                    return;
                }

                if (txtNewpwd.Text != txtNewpwd2.Text)
                {
                    MessageBox.Show("两次密码不一致");
                    return;
                }

                // 使用业务逻辑层修改密码
                bool changeResult = _userService.ChangeAdminPassword(txtName.Text, txtOldpwd.Text, txtNewpwd.Text);

                if (changeResult)
                {
                    MessageBox.Show("修改成功");
                    this.Close();
                }
                else
                {
                    MessageBox.Show("修改失败，请检查输入信息是否正确");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"修改密码时出错: {ex.Message}");
            }
        }

        private void Changepwd_Load(object sender, EventArgs e)
        {
            txtName.Text = Login.username;
        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {
            // 空方法，保留原有的事件处理器
        }

        private void BtnCanser_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}