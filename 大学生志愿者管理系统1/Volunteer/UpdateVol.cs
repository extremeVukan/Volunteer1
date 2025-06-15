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
    public partial class UpdateVol : Form
    {
        private VolunteerService _volunteerService; // 添加VolunteerService引用

        public UpdateVol()
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

        private void UpdateVol_Load(object sender, EventArgs e)
        {
            LoadVolunteers();
            showAll();
            dgvHead();
        }

        private void dgvvol_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvvol.CurrentRow != null)
            {
                txtID.Text = dgvvol.CurrentRow.Cells[0].Value?.ToString() ?? "";
                txtName.Text = dgvvol.CurrentRow.Cells[1].Value?.ToString() ?? "";
                txtPhone.Text = dgvvol.CurrentRow.Cells[2].Value?.ToString() ?? "";
                txtemail.Text = dgvvol.CurrentRow.Cells[3].Value?.ToString() ?? "";
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (txtName.Text == "" || txtPhone.Text == "" || txtemail.Text == "")
            {
                MessageBox.Show("姓名或联系方式或邮箱不能为空");
                return;
            }

            DialogResult dr = MessageBox.Show("您确定要修改吗？", "提示",
                MessageBoxButtons.OKCancel, MessageBoxIcon.Information);

            if (dr != DialogResult.OK)
            {
                return;
            }

            try
            {
                // 解析志愿者ID
                if (!int.TryParse(txtID.Text, out int volunteerId))
                {
                    MessageBox.Show("志愿者ID必须是整数");
                    return;
                }

                // 创建志愿者对象
                var volunteer = new DAL.volunteerT
                {
                    Aid = volunteerId,
                    AName = txtName.Text.Trim(),
                    Atelephone = txtPhone.Text.Trim(),
                    email = txtemail.Text.Trim()
                };

                // 使用业务逻辑层更新志愿者
                bool result = _volunteerService.UpdateVolunteer(volunteer, Login.username);

                if (result)
                {
                    MessageBox.Show("修改成功");

                    // 重新加载数据
                    LoadVolunteers();
                    showAll();
                }
                else
                {
                    MessageBox.Show("修改失败");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"修改志愿者信息时出错: {ex.Message}");
            }
        }

        private void btncancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}