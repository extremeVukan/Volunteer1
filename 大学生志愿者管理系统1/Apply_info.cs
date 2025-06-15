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
    public partial class Apply_info : Form
    {
        private ApplyService _applyService; // 添加ApplyService引用
        private ActivityService _activityService; // 添加ActivityService引用

        public Apply_info()
        {
            InitializeComponent();
            _applyService = new ApplyService(); // 初始化ApplyService
            _activityService = new ActivityService(); // 初始化ActivityService
        }

        public string Holder = "";
        public string userid = "";
        public static string VV = "";

        private void button1_Click(object sender, EventArgs e)
        {
            if (txtName.Text == "" || txtphone.Text == "")
            {
                MessageBox.Show("请输入信息");
                return;
            }

            try
            {
                // 解析活动ID和志愿者ID
                if (!int.TryParse(AQUEUE.ActID1, out int activityId) ||
                    !int.TryParse(Login.Vol_ID, out int volunteerId))
                {
                    MessageBox.Show("无效的活动ID或志愿者ID");
                    return;
                }

                // 使用业务逻辑层提交申请
                bool result = _applyService.SubmitApply(
                    volunteerId,
                    txtName.Text,
                    txtphone.Text,
                    activityId,
                    txtActName.Text,
                    Holder
                );

                if (result)
                {
                    MessageBox.Show("申请成功");
                    this.Close();
                }
                else
                {
                    MessageBox.Show("申请失败，请稍后重试");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"提交申请过程中出错: {ex.Message}");
            }
        }

        private void Apply_info_Load(object sender, EventArgs e)
        {
            try
            {
                // 解析活动ID
                if (int.TryParse(AQUEUE.ActID1, out int activityId))
                {
                    // 使用业务逻辑层获取活动详情
                    var activity = _activityService.GetActivityById(activityId);

                    if (activity != null)
                    {
                        txtActName.Text = activity.activity_Name;
                        Holder = activity.Holder;
                    }
                    else
                    {
                        MessageBox.Show("无法获取活动信息");
                        this.Close();
                    }
                }
                else
                {
                    MessageBox.Show("无效的活动ID");
                    this.Close();
                }

                // 设置用户名
                txtName.Text = Login.username;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载申请信息时出错: {ex.Message}");
                this.Close();
            }
        }
    }
}