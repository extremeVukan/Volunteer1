using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using BLL; // 添加BLL引用
using DAL; // 添加DAL引用用于实体类

namespace 大学生志愿者管理系统1.Tactivity
{
    public partial class Insert_activity : Form
    {
        private ActivityService _activityService; // 添加ActivityService引用

        public Insert_activity()
        {
            InitializeComponent();
            _activityService = new ActivityService(); // 初始化ActivityService
        }

        public static string path_source = "";
        private DataTable dtActivities = new DataTable(); // 活动数据表

        void LoadActivities()
        {
            try
            {
                // 使用业务逻辑层获取当前用户的所有活动
                var activities = _activityService.GetActivitiesByHolder(Login.username);

                // 清空现有数据表并创建列
                dtActivities.Clear();
                if (dtActivities.Columns.Count == 0)
                {
                    dtActivities.Columns.Add("activity_ID", typeof(int));
                    dtActivities.Columns.Add("activity_Name", typeof(string));
                    dtActivities.Columns.Add("activity_type", typeof(string));
                    dtActivities.Columns.Add("addtime", typeof(DateTime));
                    dtActivities.Columns.Add("stoptime", typeof(DateTime));
                    dtActivities.Columns.Add("place", typeof(string));
                    dtActivities.Columns.Add("renshu", typeof(int));
                    dtActivities.Columns.Add("descn", typeof(string));
                    dtActivities.Columns.Add("Image", typeof(string));
                    dtActivities.Columns.Add("Holder", typeof(string));
                    dtActivities.Columns.Add("status", typeof(string));
                }

                // 填充数据
                foreach (var activity in activities)
                {
                    DataRow row = dtActivities.NewRow();
                    row["activity_ID"] = activity.activity_ID;
                    row["activity_Name"] = activity.activity_Name ?? "";
                    row["activity_type"] = activity.activity_type ?? "";
                    row["addtime"] = activity.addtime; // 根据签名信息，这是非空DateTime
                    row["stoptime"] = activity.stoptime; // 根据签名信息，这是非空DateTime
                    row["place"] = activity.place ?? "";
                    row["renshu"] = activity.renshu ?? 0;
                    row["descn"] = activity.descn ?? "";
                    row["Image"] = activity.Image ?? "";
                    row["Holder"] = activity.Holder ?? "";
                    row["status"] = activity.status ?? "";
                    dtActivities.Rows.Add(row);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载活动数据出错: {ex.Message}\r\n\r\n堆栈跟踪：{ex.StackTrace}");
            }
        }

        void showAll()
        {
            dgvactivity.DataSource = dtActivities;
        }

        private void Insert_activity_Load(object sender, EventArgs e)
        {
            // 加载活动类型数据
            this.activityTypeTTableAdapter.Fill(this.vOLUNTEERDataSet.activityTypeT);

            // 加载活动数据
            LoadActivities();
            showAll();

            // 获取下一个活动ID
            int nextId = _activityService.GetNextActivityId();
            txtActID.Text = nextId.ToString();
        }

        private void btncancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (txtActID.Text == "" || txtActName.Text == "" || txtActNeed.Text == "" || txtplace.Text == "")
            {
                MessageBox.Show("必选项不能为空");
                return;
            }

            try
            {
                // 解析活动ID和人数
                if (!int.TryParse(txtActID.Text, out int activityId) ||
                    !int.TryParse(txtActNeed.Text, out int need))
                {
                    MessageBox.Show("活动ID和人数必须是整数");
                    return;
                }

                // 创建正确类型(DAL.ActivityT)的新活动对象
                var activity = new DAL.ActivityT  // 明确指定DAL命名空间
                {
                    activity_ID = activityId,
                    activity_Name = txtActName.Text,
                    activity_type = cboCategory.SelectedValue.ToString(),
                    addtime = dtpBegin.Value,
                    stoptime = dtpStop.Value,
                    place = txtplace.Text,
                    renshu = need,
                    descn = txtActResume.Text,
                    Holder = Login.username,
                    status = "进行中"
                };

                // 使用业务逻辑层添加活动
                bool result = _activityService.AddActivity(activity, path_source, Login.username);

                if (result)
                {
                    MessageBox.Show("添加成功");

                    // 重新加载数据
                    LoadActivities();
                    showAll();

                    // 更新下一个活动ID
                    int nextId = _activityService.GetNextActivityId();
                    txtActID.Text = nextId.ToString();

                    // 清空输入框
                    txtActName.Text = "";
                    txtActNeed.Text = "";
                    txtplace.Text = "";
                    txtActResume.Text = "";
                    pictureBox1.Image = null;
                    path_source = "";
                }
                else
                {
                    MessageBox.Show("添加失败，可能是活动ID已存在");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"添加活动时出错: {ex.Message}\r\n\r\n堆栈跟踪：{ex.StackTrace}");
            }
        }
        private void btnpid_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                path_source = ofd.FileName;
                pictureBox1.Image = Image.FromFile(path_source);
                pictureBox1.SizeMode = PictureBoxSizeMode.AutoSize;
            }
        }

        private void label9_Click(object sender, EventArgs e)
        {
            // 空方法，保留原有的事件处理器
        }
    }
}