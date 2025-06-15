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

namespace 大学生志愿者管理系统1.Volunteer
{
    public partial class deleteVol : Form
    {
        private VolunteerService _volunteerService; // 添加VolunteerService引用

        public deleteVol()
        {
            InitializeComponent();
            _volunteerService = new VolunteerService(); // 初始化VolunteerService
        }

        private DataTable dtVolunteers = new DataTable(); // 志愿者数据表

        void LoadVolunteers()
        {
            try
            {
                // 使用业务逻辑层获取所有志愿者
                var volunteers = _volunteerService.GetAllVolunteers();

                // 清空现有数据表并创建列
                dtVolunteers.Clear();
                if (dtVolunteers.Columns.Count == 0)
                {
                    dtVolunteers.Columns.Add("Aid", typeof(int));
                    dtVolunteers.Columns.Add("AName", typeof(string));
                    dtVolunteers.Columns.Add("Atelephone", typeof(string));
                    dtVolunteers.Columns.Add("email", typeof(string));
                }

                // 填充数据
                foreach (var volunteer in volunteers)
                {
                    DataRow row = dtVolunteers.NewRow();
                    row["Aid"] = volunteer.Aid;
                    row["AName"] = volunteer.AName ?? "";
                    row["Atelephone"] = volunteer.Atelephone ?? "";
                    row["email"] = volunteer.email ?? "";
                    dtVolunteers.Rows.Add(row);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载志愿者数据出错: {ex.Message}");
            }
        }

        void showAll() // DGV载入
        {
            dgvvol.DataSource = dtVolunteers;
        }

        void dgvHead()
        {
            this.dgvvol.Columns[0].HeaderText = "ID";
            this.dgvvol.Columns[1].HeaderText = "姓名";
            this.dgvvol.Columns[2].HeaderText = "联系方式";
            this.dgvvol.Columns[3].HeaderText = "邮箱";
        }

        void showXz()
        {
            // 检查是否已经添加了选择列
            if (!dgvvol.Columns.Contains("acCode"))
            {
                DataGridViewCheckBoxColumn acCode = new DataGridViewCheckBoxColumn();
                acCode.Name = "acCode";
                acCode.HeaderText = "选择";
                dgvvol.Columns.Add(acCode);
            }
        }

        private void dgvvol_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvvol.Columns[e.ColumnIndex].Name == "acCode" && e.RowIndex >= 0)
            {
                if (dgvvol.Rows.Count > 0)
                {
                    for (int i = 0; i < dgvvol.Rows.Count; i++)
                    {
                        DataGridViewCheckBoxCell ck = dgvvol.Rows[i].Cells["acCode"] as DataGridViewCheckBoxCell;
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

        private void btndelete_Click(object sender, EventArgs e)
        {
            bool selected = false;
            int selectedIndex = -1;

            for (int i = 0; i < dgvvol.Rows.Count; i++)
            {
                if (dgvvol.Rows[i].Cells["acCode"]?.EditedFormattedValue?.ToString() == "True")
                {
                    selected = true;
                    selectedIndex = i;
                    break;
                }
            }

            if (selected && selectedIndex >= 0)
            {
                DialogResult dr = MessageBox.Show("是否要删除？", "提示",
                    MessageBoxButtons.OKCancel, MessageBoxIcon.Information);

                if (dr == DialogResult.OK)
                {
                    try
                    {
                        if (dgvvol.Rows[selectedIndex].Cells["Aid"].Value != null &&
                            int.TryParse(dgvvol.Rows[selectedIndex].Cells["Aid"].Value.ToString(), out int volunteerId))
                        {
                            // 使用业务逻辑层删除志愿者
                            bool result = _volunteerService.DeleteVolunteer(volunteerId, Login.username);

                            if (result)
                            {
                                dgvvol.Rows.RemoveAt(selectedIndex);
                                MessageBox.Show("删除成功");

                                // 重新加载数据
                                LoadVolunteers();
                                showAll();
                            }
                            else
                            {
                                MessageBox.Show("删除失败");
                            }
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

        private void deleteVol_Load(object sender, EventArgs e)
        {
            LoadVolunteers();
            showAll();
            dgvHead();
            showXz();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}