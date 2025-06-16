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

namespace 大学生志愿者管理系统1
{
    public partial class Apply_info : Form
    {
        private ApplyService _applyService; // 添加ApplyService引用

        public Apply_info()
        {
            InitializeComponent();
            _applyService = new ApplyService(); // 初始化ApplyService
        }

        public string Holder = "";
        public string userid = "";
        public static string VV = "";

        private void button1_Click(object sender, EventArgs e)
        {
            if (txtName.Text == "" || txtphone.Text == "")
            {
                MessageBox.Show("请输入信息");
            }
            else
            {
                try
                {
                    // 解析活动ID和志愿者ID
                    int volunteerId = Convert.ToInt32(Login.Vol_ID);
                    int activityId = Convert.ToInt32(AQUEUE.ActID1);

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
                    MessageBox.Show("申请过程中出错: " + ex.Message);
                }
            }
        }

        private void Apply_info_Load(object sender, EventArgs e)
        {
            string sdr = "select * from ActivityT where activity_ID='" + AQUEUE.ActID1 + "'";
            DataTable dt = DB.GetDataSet(sdr);
            txtActName.Text = dt.Rows[0][1].ToString();
            Holder = dt.Rows[0][10].ToString();
            txtName.Text = Login.username;
        }
    }
}