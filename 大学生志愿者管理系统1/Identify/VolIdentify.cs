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

namespace 大学生志愿者管理系统1.Identify
{
    public partial class VolIdentify : Form
    {
        private IdentifyService _identifyService; // 添加IdentifyService引用

        public VolIdentify()
        {
            InitializeComponent();
            _identifyService = new IdentifyService(); // 初始化IdentifyService
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            if (txtName.Text == "" || txtPhone.Text == "" || txtAddress.Text == "")
            {
                MessageBox.Show("请输入信息");
                return;
            }

            try
            {
                // 确保志愿者ID有效
                if (string.IsNullOrEmpty(Login.Vol_ID))
                {
                    MessageBox.Show("志愿者ID无效，请重新登录");
                    return;
                }

                if (!int.TryParse(Login.Vol_ID, out int volunteerId))
                {
                    MessageBox.Show("志愿者ID格式错误");
                    return;
                }

                // 使用业务逻辑层申请证件
                bool result = _identifyService.ApplyForIdentify(
                    volunteerId,
                    txtName.Text.Trim(),
                    txtPhone.Text.Trim(),
                    txtprovince.Text.Trim(),
                    txtcity.Text.Trim(),
                    txtAddress.Text.Trim()
                );

                if (result)
                {
                    MessageBox.Show("申请成功");
                    this.Close();
                }
                else
                {
                    MessageBox.Show("申请失败，您可能已经申请过证件或志愿时长不足");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"申请过程中出错: {ex.Message}");
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}