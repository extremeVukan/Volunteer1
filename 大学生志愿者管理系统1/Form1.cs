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
    public partial class Form1 : Form
    {
        private ActivityService _activityService; // 添加ActivityService引用
        private ApplyService _applyService; // 添加ApplyService引用
        private VolunteerService _volunteerService; // 添加VolunteerService引用
        private IdentifyService _identifyService; // 添加IdentifyService引用

        public Form1()
        {
            InitializeComponent();
            _activityService = new ActivityService(); // 初始化ActivityService
            _applyService = new ApplyService(); // 初始化ApplyService
            _volunteerService = new VolunteerService(); // 初始化VolunteerService
            _identifyService = new IdentifyService(); // 初始化IdentifyService
        }

        public static int flag = 1; // 1为主窗体，2为管理员窗体
        public static bool Aflag = false; // false为未登录,true已登录
        public static string Actid = "";
        public static int Time = 0;
        cartItem cartSrv = new cartItem();

        void showALLActiveites()
        {
            try
            {
                // 清空现有行
                dgvactivity.Rows.Clear();

                // 使用业务逻辑层获取所有活动
                var activities = _activityService.GetAllActivities();

                foreach (var activity in activities)
                {
                    int index = dgvactivity.Rows.Add();
                    dgvactivity.Rows[index].Cells[0].Value = activity.activity_ID;
                    dgvactivity.Rows[index].Cells[1].Value = activity.activity_Name;
                    dgvactivity.Rows[index].Cells[2].Value = activity.activity_type;
                    dgvactivity.Rows[index].Cells[3].Value = activity.addtime;
                    dgvactivity.Rows[index].Cells[4].Value = activity.stoptime;
                    dgvactivity.Rows[index].Cells[5].Value = activity.place;
                    dgvactivity.Rows[index].Cells[6].Value = activity.renshu;
                    dgvactivity.Rows[index].Cells[7].Value = activity.Holder;
                    dgvactivity.Rows[index].Cells[11].Value = activity.status;

                    try
                    {
                        // 显示活动图片
                        if (!string.IsNullOrEmpty(activity.Image))
                        {
                            Image imageColumn = Image.FromFile(Application.StartupPath + activity.Image);
                            dgvactivity.Rows[index].Cells["Column8"].Value = imageColumn;
                        }
                        else
                        {
                            Image imageColumn = Image.FromFile(Application.StartupPath + "\\" + "暂无图片.gif");
                            dgvactivity.Rows[index].Cells["Column8"].Value = imageColumn;
                        }
                    }
                    catch
                    {
                        Image imageColumn = Image.FromFile(Application.StartupPath + "\\" + "暂无图片.gif");
                        dgvactivity.Rows[index].Cells["Column8"].Value = imageColumn;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载活动数据出错: {ex.Message}");
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {
            // 空方法，保留原有的事件处理器
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            dgvactivity.RowTemplate.Height = 90;
            showALLActiveites();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            dgvactivity.Rows.Clear();
            showALLActiveites();
        }

        private void button1_Click_1(object sender, EventArgs e) // 按活动名称关键字查询
        {
            if (txtPName.Text == "")
            {
                MessageBox.Show("请输入查询关键字");
                return;
            }

            try
            {
                // 清空现有行
                dgvactivity.Rows.Clear();

                // 使用业务逻辑层搜索活动
                var activities = _activityService.SearchActivitiesByName(txtPName.Text);

                foreach (var activity in activities)
                {
                    int index = dgvactivity.Rows.Add();
                    dgvactivity.Rows[index].Cells[0].Value = activity.activity_ID;
                    dgvactivity.Rows[index].Cells[1].Value = activity.activity_Name;
                    dgvactivity.Rows[index].Cells[2].Value = activity.activity_type;
                    dgvactivity.Rows[index].Cells[3].Value = activity.addtime;
                    dgvactivity.Rows[index].Cells[4].Value = activity.stoptime;
                    dgvactivity.Rows[index].Cells[5].Value = activity.place;
                    dgvactivity.Rows[index].Cells[6].Value = activity.renshu;
                    dgvactivity.Rows[index].Cells[7].Value = activity.Holder;
                    dgvactivity.Rows[index].Cells[11].Value = activity.status;

                    try
                    {
                        // 显示活动图片
                        if (!string.IsNullOrEmpty(activity.Image))
                        {
                            Image imageColumn = Image.FromFile(Application.StartupPath + activity.Image);
                            dgvactivity.Rows[index].Cells["Column8"].Value = imageColumn;
                        }
                        else
                        {
                            Image imageColumn = Image.FromFile(Application.StartupPath + "\\" + "暂无图片.gif");
                            dgvactivity.Rows[index].Cells["Column8"].Value = imageColumn;
                        }
                    }
                    catch
                    {
                        Image imageColumn = Image.FromFile(Application.StartupPath + "\\" + "暂无图片.gif");
                        dgvactivity.Rows[index].Cells["Column8"].Value = imageColumn;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"搜索活动出错: {ex.Message}");
            }
        }

        private void btnsearch2_Click(object sender, EventArgs e) // 按照活动类型查询
        {
            try
            {
                // 清空现有行
                dgvactivity.Rows.Clear();

                // 获取选择的活动类型
                string activityType = "";
                int i = comboBox1.SelectedIndex;
                switch (i)
                {
                    case 0:
                        activityType = "教育类";
                        break;
                    case 1:
                        activityType = "社区发展类";
                        break;
                    case 2:
                        activityType = "卫生与医疗类";
                        break;
                    case 3:
                        activityType = "环境保护类";
                        break;
                    case 4:
                        activityType = "动物保护类";
                        break;
                    default:
                        activityType = "紧急救援类";
                        break;
                }

                // 使用业务逻辑层按类型获取活动
                var activities = _activityService.GetActivitiesByType(activityType);

                foreach (var activity in activities)
                {
                    int index = dgvactivity.Rows.Add();
                    dgvactivity.Rows[index].Cells[0].Value = activity.activity_ID;
                    dgvactivity.Rows[index].Cells[1].Value = activity.activity_Name;
                    dgvactivity.Rows[index].Cells[2].Value = activity.activity_type;
                    dgvactivity.Rows[index].Cells[3].Value = activity.addtime;
                    dgvactivity.Rows[index].Cells[4].Value = activity.stoptime;
                    dgvactivity.Rows[index].Cells[5].Value = activity.place;
                    dgvactivity.Rows[index].Cells[6].Value = activity.renshu;
                    dgvactivity.Rows[index].Cells[7].Value = activity.Holder;
                    dgvactivity.Rows[index].Cells[11].Value = activity.status;

                    try
                    {
                        // 显示活动图片
                        if (!string.IsNullOrEmpty(activity.Image))
                        {
                            Image imageColumn = Image.FromFile(Application.StartupPath + activity.Image);
                            dgvactivity.Rows[index].Cells["Column8"].Value = imageColumn;
                        }
                        else
                        {
                            Image imageColumn = Image.FromFile(Application.StartupPath + "\\" + "暂无图片.gif");
                            dgvactivity.Rows[index].Cells["Column8"].Value = imageColumn;
                        }
                    }
                    catch
                    {
                        Image imageColumn = Image.FromFile(Application.StartupPath + "\\" + "暂无图片.gif");
                        dgvactivity.Rows[index].Cells["Column8"].Value = imageColumn;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"按类型搜索活动出错: {ex.Message}");
            }
        }

        private void btnloading_Click(object sender, EventArgs e)
        {
            if (Aflag == true)
            {
                MessageBox.Show("已登录");
            }
            else
            {
                this.Hide();
                Login ll = new Login();
                ll.ShowDialog();
                if (flag == 1)
                {
                    this.Visible = true;
                    label5.Text = Login.username;
                }
            }
        }

        private void btnregister_Click(object sender, EventArgs e)
        {
            Register r1 = new Register();
            r1.ShowDialog();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // 空方法，保留原有的事件处理器
        }

        private void dgvactivity_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int Cindex = e.ColumnIndex;
            int rowIndex = e.RowIndex;

            if (rowIndex < 0 || rowIndex >= dgvactivity.Rows.Count)
                return;

            // 如果是第8列（索引9），查看活动详细信息
            if (Cindex == 9)
            {
                Actid = dgvactivity.Rows[rowIndex].Cells[0].Value.ToString();
                Act_info t1 = new Act_info();
                t1.ShowDialog();
            }

            // 如果是第9列（索引10），查看申请列表
            if (Cindex == 10)
            {
                if (Aflag == false)
                {
                    MessageBox.Show("请登录账号");
                    Login l1 = new Login();
                    l1.ShowDialog();
                    label5.Text = Login.username;
                }
                else
                {
                    try
                    {
                        Actid = dgvactivity.Rows[rowIndex].Cells[0].Value.ToString();
                        string Num = dgvactivity.Rows[rowIndex].Cells[6].Value.ToString();
                        string status = dgvactivity.Rows[rowIndex].Cells[11].Value.ToString();

                        if (!int.TryParse(Actid, out int activityId) ||
                            !int.TryParse(Num, out int neededPeople) ||
                            !int.TryParse(Login.Vol_ID, out int volunteerId))
                        {
                            MessageBox.Show("无效的活动ID或志愿者ID");
                            return;
                        }

                        // 获取活动成员
                        var activityMembers = _activityService.GetActivityMembers(activityId);

                        // 检查志愿者是否已经参加
                        bool hasJoined = activityMembers.Any(m => m.Volunteerid == volunteerId);
                        if (hasJoined)
                        {
                            MessageBox.Show("你已经参加了这个活动");
                            return;
                        }

                        // 检查活动状态
                        if (status == "已结束")
                        {
                            MessageBox.Show("该活动已结束");
                            return;
                        }

                        // 检查人数是否已满
                        if (activityMembers.Count >= neededPeople)
                        {
                            MessageBox.Show("人数已满");
                            return;
                        }

                        // 检查是否已申请
                        var applies = _applyService.GetAppliesByVolunteerId(volunteerId);
                        bool hasApplied = applies.Any(a => a.Act_ID == activityId);
                        if (hasApplied)
                        {
                            MessageBox.Show("已申请");
                            return;
                        }

                        // 添加到申请队列
                        bool result = _applyService.AddToApplyQueue(volunteerId, activityId);
                        if (result)
                        {
                            MessageBox.Show("添加成功");
                        }
                        else
                        {
                            MessageBox.Show("添加失败");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"申请活动出错: {ex.Message}");
                    }
                }
            }
        }

        private void btnQuit_Click(object sender, EventArgs e)
        {
            if (Login.username == "")
            {
                MessageBox.Show("请登录账号");
            }
            else
            {
                MessageBox.Show("已退出账号");

                Login.username = "";
                label5.Text = "";
                Aflag = false;
                label8.Text = "";
            }
        }

        private void btncheck_Click(object sender, EventArgs e)
        {
            AQUEUE s1 = new AQUEUE();
            s1.ShowDialog();
        }

        private void btnShowAqueue_Click(object sender, EventArgs e)
        {
            if (Login.username == "")
            {
                MessageBox.Show("请登录账号");
            }
            else
            {
                AQUEUE s1 = new AQUEUE();
                s1.ShowDialog();
            }
        }

        private void btnVolTime_Click(object sender, EventArgs e)
        {
            if (Login.username == "")
            {
                MessageBox.Show("请登录账号");
                Login s1 = new Login();
                s1.ShowDialog();
            }
            else
            {
                try
                {
                    // 解析志愿者ID
                    if (int.TryParse(Login.Vol_ID, out int volunteerId))
                    {
                        // 获取志愿者时长
                        int hours = _volunteerService.GetVolunteerHours(volunteerId);
                        Time = hours; // 更新全局Time变量
                        label8.Text = hours.ToString() + "小时";
                    }
                    else
                    {
                        MessageBox.Show("无效的志愿者ID");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"获取志愿时长出错: {ex.Message}");
                }
            }
        }

        private void btnidentity_Click(object sender, EventArgs e)
        {
            if (Aflag == false)
            {
                MessageBox.Show("请登录账号");
            }
            else
            {
                SelfCheck.SelfInfoCheck1 s1 = new SelfCheck.SelfInfoCheck1();
                s1.ShowDialog();
            }
        }

        private void btnCard_Click(object sender, EventArgs e)
        {
            if (Aflag == false)
            {
                MessageBox.Show("请登录账号");
                return;
            }

            try
            {
                // 检查志愿者是否已申请证书
                string status = _identifyService.GetVolunteerIdentifyStatus(Login.username);

                if (status != "未拥有")
                {
                    MessageBox.Show("您已申请志愿者证");
                    return;
                }

                // 检查志愿时长
                if (int.TryParse(Login.Vol_ID, out int volunteerId))
                {
                    int hours = _volunteerService.GetVolunteerHours(volunteerId);
                    Time = hours; // 更新全局Time变量

                    if (Time < 10)
                    {
                        MessageBox.Show("很抱歉，您的志愿时长不足10小时");
                        return;
                    }

                    // 打开申请志愿者证页面
                    MessageBox.Show("欢迎申请志愿者证");
                    Identify.VolIdentify s1 = new Identify.VolIdentify();
                    s1.ShowDialog();
                }
                else
                {
                    MessageBox.Show("无效的志愿者ID");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"检查志愿者证状态出错: {ex.Message}");
            }
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            dgvactivity.Rows.Clear();
            dgvactivity.RowTemplate.Height = 90;
            showALLActiveites();
        }
    }
}