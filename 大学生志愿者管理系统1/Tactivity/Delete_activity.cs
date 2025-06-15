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

namespace 大学生志愿者管理系统1.Tactivity
{
    public partial class Delete_activity : Form
    {
        private ActivityService _activityService; // 添加ActivityService引用

        public Delete_activity()
        {
            InitializeComponent();
            _activityService = new ActivityService(); // 初始化ActivityService
        }

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
                    row["addtime"] = DateTime.Now;
                    row["stoptime"] =  DateTime.Now;
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
                MessageBox.Show($"加载活动数据出错: {ex.Message}");
            }
        }

        private void Delete_activity_Load(object sender, EventArgs e)
        {
            LoadActivities();
            showAll();
            showXz();
        }

        void showAll()
        {
            dgvactivity.DataSource = dtActivities;
        }

        private void dgvactivity_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvactivity.Columns[e.ColumnIndex].Name == "acCode" && e.RowIndex >= 0)
            {
                if (dgvactivity.Rows.Count > 0)
                {
                    for (int i = 0; i < dgvactivity.Rows.Count; i++)
                    {
                        DataGridViewCheckBoxCell ck = dgvactivity.Rows[i].Cells["acCode"] as DataGridViewCheckBoxCell;
                        if (i != e.RowIndex)
                        {
                            ck.Value = false;
                        }
                        else
                        {
                            ck.Value = true;
                        }
                    }
                }
            }
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            bool selected = false;
            int selectedIndex = -1;
            int activityId = -1;

            // 查找选中的行
            for (int i = 0; i < dgvactivity.Rows.Count; i++)
            {
                if (dgvactivity.Rows[i].Cells["acCode"]?.EditedFormattedValue?.ToString() == "True")
                {
                    selected = true;
                    selectedIndex = i;
                    // 获取活动ID
                    if (dgvactivity.Rows[i].Cells["activity_ID"].Value != null &&
                        int.TryParse(dgvactivity.Rows[i].Cells["activity_ID"].Value.ToString(), out activityId))
                    {
                        break;
                    }
                }
            }

            if (selected && activityId > 0)
            {
                DialogResult dr = MessageBox.Show("是否要删除？", "提示",
                    MessageBoxButtons.OKCancel, MessageBoxIcon.Information);

                if (dr == DialogResult.OK)
                {
                    try
                    {
                        // 使用业务逻辑层删除活动
                        bool result = _activityService.DeleteActivity(activityId, Login.username);

                        if (result)
                        {
                            // 从UI中移除
                            if (selectedIndex >= 0 && selectedIndex < dgvactivity.Rows.Count)
                            {
                                dgvactivity.Rows.RemoveAt(selectedIndex);
                            }
                            MessageBox.Show("删除成功");

                            // 重新加载数据
                            LoadActivities();
                            showAll();
                        }
                        else
                        {
                            MessageBox.Show("删除失败");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"删除过程中出错: {ex.Message}");
                    }
                }
            }
            else
            {
                MessageBox.Show("请选择要删除的对象");
            }
        }

        private void btnCancle_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        void showXz()
        {
            // 检查是否已经添加了选择列
            if (!dgvactivity.Columns.Contains("acCode"))
            {
                DataGridViewCheckBoxColumn acCode = new DataGridViewCheckBoxColumn();
                acCode.Name = "acCode";
                acCode.HeaderText = "选择";
                dgvactivity.Columns.Add(acCode);
            }
        }
    }
}