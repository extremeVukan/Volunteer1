using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BLL; // 引入业务逻辑层命名空间

namespace 大学生志愿者管理系统1
{
    public partial class Act_info : Form
    {
        private ActivityService _activityService; // 声明业务逻辑层服务

        public Act_info()
        {
            InitializeComponent();
            _activityService = new ActivityService(); // 初始化服务
        }

        private void Act_info_Load(object sender, EventArgs e)
        {
            try
            {
                // 使用业务逻辑层获取活动信息，而不是直接执行SQL
                var activity = _activityService.GetActivityById(Convert.ToInt32(Form1.Actid));

                if (activity == null)
                {
                    MessageBox.Show("未找到活动信息", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Close();
                    return;
                }

                // 设置界面控件值
                lb10ID.Text = Form1.Actid;
                lb11Name.Text = activity.activity_Name;
                lb12Type.Text = activity.activity_type;
                lb13AddTime.Text = activity.addtime.ToString();
                lb14StopTime.Text = activity.stoptime.ToString();
                lb15Place.Text = activity.place;
                lb16Descn.Text = activity.descn;
                lb17Need.Text = activity.renshu.ToString();

                try
                {
                    // 处理图片
                    if (!string.IsNullOrEmpty(activity.Image))
                    {
                        ptbPicture.Image = Image.FromFile(Application.StartupPath + activity.Image);
                        ptbPicture.SizeMode = PictureBoxSizeMode.StretchImage;
                    }
                    else
                    {
                        ptbPicture.Image = Image.FromFile(Application.StartupPath + "\\" + "暂无图片.gif");
                        ptbPicture.SizeMode = PictureBoxSizeMode.StretchImage;
                    }
                }
                catch
                {
                    ptbPicture.Image = Image.FromFile(Application.StartupPath + "\\" + "暂无图片.gif");
                    ptbPicture.SizeMode = PictureBoxSizeMode.StretchImage;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载活动信息出错: {ex.Message}", "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
        }

        private void btncancle_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}