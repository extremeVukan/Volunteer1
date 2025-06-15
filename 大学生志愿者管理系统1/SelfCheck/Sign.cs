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

namespace 大学生志愿者管理系统1.SelfCheck
{
    public partial class Sign : Form
    {
        private ActivityService _activityService; // 添加ActivityService引用
        private ActivityMemberService _activityMemberService; // 添加ActivityMemberService引用

        public Sign()
        {
            InitializeComponent();
            _activityService = new ActivityService(); // 初始化ActivityService
            _activityMemberService = new ActivityMemberService(); // 初始化ActivityMemberService
        }

        private void Sign_Load(object sender, EventArgs e)
        {
            try
            {
                if (!int.TryParse(SelfInfoCheck1.ACTID2, out int activityId))
                {
                    MessageBox.Show("无效的活动ID");
                    return;
                }

                // 使用业务逻辑层获取活动详情
                var activity = _activityService.GetActivityById(activityId);
                if (activity == null)
                {
                    MessageBox.Show("找不到该活动");
                    return;
                }

                // 显示活动信息
                lb10ID.Text = activity.activity_ID.ToString();
                lb11Name.Text = activity.activity_Name;
                lb12Type.Text = activity.activity_type;
                lb13AddTime.Text = activity.addtime.ToString();
                lb14StopTime.Text = activity.stoptime.ToString();
                lb15Place.Text = activity.place;
                lb16Descn.Text = activity.descn;
                lb17Need.Text = activity.renshu?.ToString() ?? "0";

                // 显示活动图片
                try
                {
                    ptbPicture.Image = Image.FromFile(Application.StartupPath + activity.Image);
                    ptbPicture.SizeMode = PictureBoxSizeMode.StretchImage;
                }
                catch
                {
                    ptbPicture.Image = Image.FromFile(Application.StartupPath + "\\" + "暂无图片.gif");
                    ptbPicture.SizeMode = PictureBoxSizeMode.StretchImage;
                }

                // 获取并显示签到签退时间
                if (int.TryParse(Login.Vol_ID, out int volunteerId))
                {
                    var (signInTime, signOutTime) = _activityMemberService.GetVolunteerSignInfo(activityId, volunteerId);

                    // 如果已签到，显示签到时间
                    if (!string.IsNullOrEmpty(signInTime) && DateTime.TryParse(signInTime, out DateTime signIn))
                    {
                        dtpsign.Value = signIn;
                    }

                    // 如果已签退，显示签退时间
                    if (!string.IsNullOrEmpty(signOutTime) && DateTime.TryParse(signOutTime, out DateTime signOut))
                    {
                        dtpturn.Value = signOut;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载活动数据时出错: {ex.Message}");
            }
        }

        private void btncancle_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSign_Click(object sender, EventArgs e)
        {
            try
            {
                if (!int.TryParse(SelfInfoCheck1.ACTID2, out int activityId) ||
                    !int.TryParse(Login.Vol_ID, out int volunteerId))
                {
                    MessageBox.Show("无效的活动ID或志愿者ID");
                    return;
                }

                // 检查是否已签到
                bool isSignedIn = _activityMemberService.CheckIfSignedIn(activityId, volunteerId);
                if (isSignedIn)
                {
                    MessageBox.Show("您已签到");
                    return;
                }

                // 志愿者签到
                bool result = _activityMemberService.SignIn(activityId, volunteerId);
                if (result)
                {
                    dtpsign.Value = DateTime.Now;
                    MessageBox.Show("签到成功！");
                }
                else
                {
                    MessageBox.Show("签到失败，请确认您已报名该活动并已通过审核");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"签到时出错: {ex.Message}");
            }
        }

        private void btnturn_Click(object sender, EventArgs e)
        {
            try
            {
                if (!int.TryParse(SelfInfoCheck1.ACTID2, out int activityId) ||
                    !int.TryParse(Login.Vol_ID, out int volunteerId))
                {
                    MessageBox.Show("无效的活动ID或志愿者ID");
                    return;
                }

                // 检查是否已签到
                bool isSignedIn = _activityMemberService.CheckIfSignedIn(activityId, volunteerId);
                if (!isSignedIn)
                {
                    MessageBox.Show("您还未签到，不能签退");
                    return;
                }

                // 志愿者签退
                bool result = _activityMemberService.SignOut(activityId, volunteerId);
                if (result)
                {
                    dtpturn.Value = DateTime.Now;
                    MessageBox.Show("签退成功！");

                    // 重新获取签到签退时间
                    var (signInTime, signOutTime) = _activityMemberService.GetVolunteerSignInfo(activityId, volunteerId);

                    // 更新UI
                    if (!string.IsNullOrEmpty(signInTime) && DateTime.TryParse(signInTime, out DateTime signIn) &&
                        !string.IsNullOrEmpty(signOutTime) && DateTime.TryParse(signOutTime, out DateTime signOut))
                    {
                        dtpsign.Value = signIn;
                        dtpturn.Value = signOut;
                    }
                }
                else
                {
                    MessageBox.Show("签退失败！");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"签退时出错: {ex.Message}");
            }
        }
    }
}