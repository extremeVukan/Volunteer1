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
    public partial class InsertVol : Form
    {
        private VolunteerService _volunteerService; // 添加VolunteerService引用

        public InsertVol()
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
                    dtVolunteers.Columns.Add("Act_Time", typeof(string));
                }

                // 填充数据
                foreach (var volunteer in volunteers)
                {
                    DataRow row = dtVolunteers.NewRow();
                    row["Aid"] = volunteer.Aid;
                    row["AName"] = volunteer.AName ?? "";
                    row["Atelephone"] = volunteer.Atelephone ?? "";
                    row["email"] = volunteer.email ?? "";
                    row["Act_Time"] = volunteer.Act_Time ?? "0";
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
            this.dgvvol.Columns[4].HeaderText = "志愿时长";
        }

        private void InsertVol_Load(object sender, EventArgs e)
        {
            LoadVolunteers();
            showAll();
            dgvHead();
            txtTime.Text = "0";

            // 获取下一个志愿者ID
            int nextId = _volunteerService.GetNextVolunteerId();
            txtID.Text = nextId.ToString();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (txtID.Text == "" || txtName.Text == "")
            {
                MessageBox.Show("ID和姓名不能为空");
                return;
            }

            DialogResult dr = MessageBox.Show("是否要添加？", "提示",
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

                // 创建新志愿者对象
                var volunteer = new DAL.volunteerT
                {
                    Aid = volunteerId,
                    AName = txtName.Text,
                    Atelephone = txtPhone.Text,
                    email = txtemail.Text,
                    Act_Time = txtTime.Text
                };

                // 使用业务逻辑层添加志愿者
                bool result = _volunteerService.AddVolunteer(volunteer, Login.username);

                if (result)
                {
                    MessageBox.Show("添加成功");

                    // 重新加载数据
                    LoadVolunteers();
                    showAll();

                    // 更新下一个志愿者ID
                    int nextId = _volunteerService.GetNextVolunteerId();
                    txtID.Text = nextId.ToString();

                    // 清空输入框
                    txtName.Text = "";
                    txtPhone.Text = "";
                    txtemail.Text = "";
                    txtTime.Text = "0";
                }
                else
                {
                    MessageBox.Show("添加失败，ID或姓名已存在");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"添加志愿者时出错: {ex.Message}");
            }
        }
    }
}