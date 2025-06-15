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
using DAL; // 添加DAL引用，用于实体类

namespace 大学生志愿者管理系统1.ActStop
{
    public partial class ActStop : Form
    {
        private ActivityService _activityService; // 活动服务
        private VolunteerService _volunteerService; // 志愿者服务

        public ActStop()
        {
            InitializeComponent();
            _activityService = new ActivityService();
            _volunteerService = new VolunteerService();
        }

        public int lastrowindex;
        DataTable dtVolunteer = new DataTable(); // 志愿者数据表

        void showALLActiveites()
        {
            // 使用活动服务获取当前用户的活动
            var activities = _activityService.GetActivitiesByHolder(Login.username);

            // 清空现有行
            dgvactivity.Rows.Clear();

            // 填充数据
            foreach (var activity in activities)
            {
                int index = dgvactivity.Rows.Add();
                dgvactivity.Rows[index].Cells[0].Value = activity.activity_ID;
                dgvactivity.Rows[index].Cells[1].Value = activity.activity_Name;
                dgvactivity.Rows[index].Cells[2].Value = activity.activity_type;
                dgvactivity.Rows[index].Cells[3].Value = activity.addtime;
                dgvactivity.Rows[index].Cells[4].Value = activity.stoptime;
                dgvactivity.Rows[index].Cells[5].Value = activity.status;
            }
        }

        private void ActStop_Load(object sender, EventArgs e)
        {
            showALLActiveites();
        }

        private void btnAddtime_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvShowVol.Rows.Count == 0)
                {
                    MessageBox.Show("没有志愿者数据");
                    return;
                }

                for (int i = 0; i < dgvShowVol.Rows.Count; i++) // 注意这里的条件修改了
                {
                    // 检查单元格是否存在并且有值
                    if (dgvShowVol.Rows[i].Cells.Count <= 3 ||
                        dgvShowVol.Rows[i].Cells[3].Value == null ||
                        dgvShowVol.Rows[i].Cells.Count <= 6 ||
                        dgvShowVol.Rows[i].Cells[6].Value == null)
                    {
                        continue; // 跳过无效行
                    }

                    string volIdStr = dgvShowVol.Rows[i].Cells[3].Value.ToString();
                    string timeStr = dgvShowVol.Rows[i].Cells[6].Value.ToString();

                    // 解析志愿者ID和时长
                    if (int.TryParse(volIdStr, out int volunteerId) &&
                        int.TryParse(timeStr, out int additionalHours))
                    {
                        // 使用志愿者服务更新时长
                        bool result = _volunteerService.UpdateVolunteerHours(volunteerId, additionalHours);
                        if (result)
                        {
                            MessageBox.Show($"成功为志愿者(ID:{volunteerId})添加{additionalHours}小时");
                        }
                        else
                        {
                            MessageBox.Show($"更新志愿者(ID:{volunteerId})时长失败");
                        }
                    }
                }

                txtVolTime.Text = "";
                dgvShowVol.DataSource = null;
                dgvShowVol.Rows.Clear();

                MessageBox.Show("处理完成");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"发生错误: {ex.Message}");
            }
        }

        private void dgvactivity_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int Cindex = e.ColumnIndex;
            int rowindex = e.RowIndex;

            // 确保行有效
            if (rowindex < 0 || rowindex >= dgvactivity.Rows.Count)
                return;

            string check = dgvactivity.Rows[rowindex].Cells[5].Value?.ToString() ?? "";

            if (Cindex == 6) // 结束活动列
            {
                if (check == "已结束")
                {
                    MessageBox.Show("活动已结束");
                }
                else
                {
                    string actIdStr = dgvactivity.Rows[rowindex].Cells[0].Value?.ToString() ?? "";

                    DialogResult dr = MessageBox.Show("是否结束？", "提示",
                       MessageBoxButtons.OKCancel, MessageBoxIcon.Information);

                    if (dr == DialogResult.OK)
                    {
                        if (int.TryParse(actIdStr, out int activityId))
                        {
                            // 更新UI
                            dgvactivity.Rows[rowindex].Cells[5].Value = "已结束";

                            try
                            {
                                // 使用活动服务结束活动
                                bool result = _activityService.EndActivity(activityId, Login.username);

                                if (result)
                                {
                                    // 获取活动参与者
                                    var volunteers = _volunteerService.GetVolunteersByActivityId(activityId);

                                    if (volunteers == null || volunteers.Count == 0)
                                    {
                                        MessageBox.Show("该活动人数为0");
                                    }
                                    else
                                    {
                                        // 创建数据表显示
                                        dtVolunteer = new DataTable(); // 重新初始化表，避免可能的问题
                                                                       // 添加所需列
                                        dtVolunteer.Columns.Add("ACTID", typeof(int));
                                        dtVolunteer.Columns.Add("ACTNAME", typeof(string));
                                        dtVolunteer.Columns.Add("Volunteerid", typeof(int));
                                        dtVolunteer.Columns.Add("volunteer", typeof(string));
                                        dtVolunteer.Columns.Add("PHONE", typeof(string));
                                        dtVolunteer.Columns.Add("TIME", typeof(string));

                                        // 填充数据
                                        foreach (var vol in volunteers)
                                        {
                                            DataRow row = dtVolunteer.NewRow();
                                            row["ACTID"] = vol.ACTID ?? 0;
                                            row["ACTNAME"] = vol.ACTNAME ?? "";
                                            row["Volunteerid"] = vol.Volunteerid ?? 0;
                                            row["volunteer"] = vol.volunteer ?? "";
                                            row["PHONE"] = vol.PHONE ?? "";
                                            // 使用Time属性，并添加空值检查
                                            row["TIME"] = vol.Time ?? "0";
                                            dtVolunteer.Rows.Add(row);
                                        }

                                        dgvShowVol.DataSource = dtVolunteer;
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("结束活动失败");
                                }
                            }
                            catch (Exception ex)
                            {
                                // 显示详细的错误信息
                                MessageBox.Show($"发生错误: {ex.Message}\r\n\r\n堆栈跟踪: {ex.StackTrace}");
                            }
                        }
                    }
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}