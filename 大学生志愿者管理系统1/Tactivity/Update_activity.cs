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
    public partial class Update_activity : Form
    {
        private ActivityService _activityService; // 添加ActivityService引用

        public Update_activity()
        {
            InitializeComponent();
            _activityService = new ActivityService(); // 初始化ActivityService
        }

        public static string path_source = ""; // 保存图片路径
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

        private void btnCancle_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dgvactivity_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvactivity.CurrentRow != null)
            {
                txtId.Text = dgvactivity.CurrentRow.Cells["activity_ID"].Value?.ToString() ?? "";
                cboCategory.Text = dgvactivity.CurrentRow.Cells["activity_type"].Value?.ToString() ?? "";
                txtName.Text = dgvactivity.CurrentRow.Cells["activity_Name"].Value?.ToString() ?? "";
                txtPlace.Text = dgvactivity.CurrentRow.Cells["place"].Value?.ToString() ?? "";
                txtResume.Text = dgvactivity.CurrentRow.Cells["descn"].Value?.ToString() ?? "";
                txtNeed.Text = dgvactivity.CurrentRow.Cells["renshu"].Value?.ToString() ?? "";

                if (dgvactivity.CurrentRow.Cells["addtime"].Value is DateTime addTime)
                {
                    dtpBegin.Value = addTime;
                }

                if (dgvactivity.CurrentRow.Cells["stoptime"].Value is DateTime stopTime)
                {
                    dtpStop.Value = stopTime;
                }

                txtHolder.Text = dgvactivity.CurrentRow.Cells["Holder"].Value?.ToString() ?? "";

                try
                {
                    string imagePath = dgvactivity.CurrentRow.Cells["Image"].Value?.ToString() ?? "";
                    if (!string.IsNullOrEmpty(imagePath))
                    {
                        pic_act.Image = Image.FromFile(Application.StartupPath + imagePath);
                        pic_act.SizeMode = PictureBoxSizeMode.StretchImage;
                    }
                    else
                    {
                        pic_act.Image = Image.FromFile(Application.StartupPath + "\\" + "暂无图片.gif");
                    }
                }
                catch
                {
                    pic_act.Image = Image.FromFile(Application.StartupPath + "\\" + "暂无图片.gif");
                }
            }
        }

        private void btnPic_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                path_source = ofd.FileName;
                pic_act.Image = Image.FromFile(path_source);
                pic_act.SizeMode = PictureBoxSizeMode.StretchImage;
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (txtName.Text == "" || txtNeed.Text == "" || txtPlace.Text == "")
            {
                MessageBox.Show("活动名称，活动人数，活动地点不能为空，请重新输入");
                return;
            }

            DialogResult dr = MessageBox.Show("您确定要修改吗？", "提示",
                MessageBoxButtons.OKCancel, MessageBoxIcon.Information);

            if (dr == DialogResult.OK)
            {
                try
                {
                    // 解析活动ID和人数
                    if (!int.TryParse(txtId.Text, out int activityId) ||
                        !int.TryParse(txtNeed.Text, out int need))
                    {
                        MessageBox.Show("活动ID和人数必须是整数");
                        return;
                    }

                    // 创建活动对象
                    var activity = new DAL.ActivityT  // 明确指定DAL命名空间
                    {
                        activity_ID = activityId,
                        activity_Name = txtName.Text.Trim(),
                        activity_type = cboCategory.SelectedValue.ToString(),
                        addtime = dtpBegin.Value,
                        stoptime = dtpStop.Value,
                        place = txtPlace.Text.Trim(),
                        renshu = need,
                        descn = txtResume.Text.Trim(),
                        Holder = txtHolder.Text,
                        status = "进行中"
                    };

                    // 使用业务逻辑层更新活动
                    bool result = _activityService.UpdateActivity(activity, path_source, Login.username);

                    if (result)
                    {
                        MessageBox.Show("修改成功");

                        // 重新加载数据
                        LoadActivities();
                        showAll();

                        // 清空图片路径
                        path_source = "";
                    }
                    else
                    {
                        MessageBox.Show("修改失败");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"修改活动时出错: {ex.Message}");
                }
            }
        }

        private void Update_activity_Load(object sender, EventArgs e)
        {
            // 加载活动类型数据
            this.activityTypeTTableAdapter.Fill(this.vOLUNTEERDataSet1.activityTypeT);

            // 加载活动数据
            LoadActivities();
            showAll();
        }
    }
}