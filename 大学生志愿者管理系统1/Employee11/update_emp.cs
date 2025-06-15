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

namespace 大学生志愿者管理系统1.Employee11
{
    public partial class update_emp : Form
    {
        private UserService _userService; // 添加UserService引用

        public update_emp()
        {
            InitializeComponent();
            _userService = new UserService(); // 初始化UserService
        }

        private DataTable dtAdmin = new DataTable(); // 管理员数据表

        void LoadAdmins()
        {
            try
            {
                // 使用业务逻辑层获取所有管理员
                var admins = _userService.GetAllAdmins();

                // 清空现有数据表并创建列
                dtAdmin.Clear();
                if (dtAdmin.Columns.Count == 0)
                {
                    dtAdmin.Columns.Add("admin_ID", typeof(int));
                    dtAdmin.Columns.Add("admin_Name", typeof(string));
                    dtAdmin.Columns.Add("sex", typeof(string));
                    dtAdmin.Columns.Add("birth_date", typeof(DateTime));
                    dtAdmin.Columns.Add("hire_date", typeof(DateTime));
                    dtAdmin.Columns.Add("address", typeof(string));
                    dtAdmin.Columns.Add("telephone", typeof(string));
                    dtAdmin.Columns.Add("wages", typeof(int));
                    dtAdmin.Columns.Add("resume", typeof(string));
                }

                // 填充数据
                foreach (var admin in admins)
                {
                    DataRow row = dtAdmin.NewRow();
                    row["admin_ID"] = admin.admin_ID;
                    row["admin_Name"] = admin.admin_Name ?? "";
                    row["sex"] = admin.sex ?? "";
                    row["birth_date"] = admin.birth_date ?? DateTime.Now;
                    row["hire_date"] = admin.hire_date ?? DateTime.Now;
                    row["address"] = admin.address ?? "";
                    row["telephone"] = admin.telephone ?? "";
                    row["wages"] = admin.wages ?? 0;
                    row["resume"] = admin.resume ?? "";
                    dtAdmin.Rows.Add(row);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载管理员数据出错: {ex.Message}");
            }
        }

        void showAll() // DGV载入
        {
            dgvEmp.DataSource = dtAdmin;
        }

        void dgvHead()
        {
            this.dgvEmp.Columns[0].HeaderText = "ID";
            this.dgvEmp.Columns[1].HeaderText = "姓名";
            this.dgvEmp.Columns[2].HeaderText = "性别";
            this.dgvEmp.Columns[3].HeaderText = "生日";
            this.dgvEmp.Columns[4].HeaderText = "入职日期";
            this.dgvEmp.Columns[5].HeaderText = "住址";
            this.dgvEmp.Columns[6].HeaderText = "联系方式";
            this.dgvEmp.Columns[7].HeaderText = "薪资";
            this.dgvEmp.Columns[8].HeaderText = "备注";
        }

        private void update_emp_Load(object sender, EventArgs e)
        {
            LoadAdmins();
            showAll();
            dgvHead();
        }

        private void dgvEmp_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvEmp.CurrentRow != null)
            {
                txtid.Text = dgvEmp.CurrentRow.Cells[0].Value?.ToString() ?? "";
                txtname.Text = dgvEmp.CurrentRow.Cells[1].Value?.ToString() ?? "";
                txtsex.Text = dgvEmp.CurrentRow.Cells[2].Value?.ToString() ?? "";

                if (dgvEmp.CurrentRow.Cells[3].Value is DateTime birthDate)
                {
                    dtpbirth.Value = birthDate;
                }

                if (dgvEmp.CurrentRow.Cells[4].Value is DateTime hireDate)
                {
                    dtphire.Value = hireDate;
                }

                txtaddress.Text = dgvEmp.CurrentRow.Cells[5].Value?.ToString() ?? "";
                txtphone.Text = dgvEmp.CurrentRow.Cells[6].Value?.ToString() ?? "";
                txtwages.Text = dgvEmp.CurrentRow.Cells[7].Value?.ToString() ?? "";
                txtresume.Text = dgvEmp.CurrentRow.Cells[8].Value?.ToString() ?? "";
            }
        }

        private void btnYes_Click(object sender, EventArgs e)
        {
            if (txtname.Text == "" || txtphone.Text == "" || txtsex.Text == "")
            {
                MessageBox.Show("姓名或联系方式或性别不能为空");
                return;
            }

            // 确保选择了行
            if (dgvEmp.CurrentCell == null || dgvEmp.CurrentCell.RowIndex < 0)
            {
                MessageBox.Show("请先选择要修改的管理员");
                return;
            }

            // 解析管理员ID
            if (!int.TryParse(txtid.Text, out int adminId))
            {
                MessageBox.Show("管理员ID必须是整数");
                return;
            }

            // 解析薪资
            if (!int.TryParse(txtwages.Text, out int wages))
            {
                MessageBox.Show("薪资必须是整数");
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
                // 创建管理员对象
                var admin = new adminT
                {
                    admin_ID = adminId,
                    admin_Name = txtname.Text,
                    sex = txtsex.Text.Trim(),
                    birth_date = dtpbirth.Value,
                    hire_date = dtphire.Value,
                    address = txtaddress.Text,
                    telephone = txtphone.Text,
                    wages = wages,
                    resume = txtresume.Text
                };

                // 使用业务逻辑层更新管理员
                bool result = _userService.UpdateAdminInfo(admin);

                if (result)
                {
                    MessageBox.Show("修改成功");

                    // 重新加载数据
                    LoadAdmins();
                    showAll();
                }
                else
                {
                    MessageBox.Show("修改失败");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"修改管理员信息时出错: {ex.Message}");
            }
        }

        private void btncancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}