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
    public partial class delete_emp : Form
    {
        private UserService _userService; // 添加UserService引用

        public delete_emp()
        {
            InitializeComponent();
            _userService = new UserService(); // 初始化UserService
        }

        private int selectedAdminId = -1;
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

                // 设置DataGridView数据源
                dgvEmp.DataSource = dtAdmin;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载管理员数据时出错: {ex.Message}");
            }
        }

        void showAll()
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

        void showXz()
        {
            DataGridViewCheckBoxColumn acCode = new DataGridViewCheckBoxColumn();
            acCode.Name = "acCode";
            acCode.HeaderText = "选择";
            dgvEmp.Columns.Add(acCode);
        }

        private void delete_emp_Load(object sender, EventArgs e)
        {
            LoadAdmins();
            dgvHead();
            showXz();
        }

        private void dgvEmp_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvEmp.Rows.Count > 0)
            {
                for (int i = 0; i < dgvEmp.Rows.Count; i++)
                {
                    DataGridViewCheckBoxCell ck = dgvEmp.Rows[i].Cells["acCode"] as DataGridViewCheckBoxCell;
                    if (i != e.RowIndex)
                    {
                        ck.Value = false;
                    }
                    else
                    {
                        ck.Value = true;
                        // 保存选中的管理员ID
                        if (dgvEmp.Rows[i].Cells["admin_ID"].Value != null)
                        {
                            selectedAdminId = Convert.ToInt32(dgvEmp.Rows[i].Cells["admin_ID"].Value);
                        }
                    }
                }
            }
        }

        private void btnok_Click(object sender, EventArgs e)
        {
            bool selected = false;
            int selectedRowIndex = -1;

            // 查找选中的行
            for (int i = 0; i < dgvEmp.Rows.Count; i++)
            {
                if (dgvEmp.Rows[i].Cells["acCode"].EditedFormattedValue.ToString() == "True")
                {
                    selected = true;
                    selectedRowIndex = i;
                    selectedAdminId = Convert.ToInt32(dgvEmp.Rows[i].Cells["admin_ID"].Value);
                    break;
                }
            }

            if (selected)
            {
                DialogResult dr = MessageBox.Show("是否要删除？", "提示",
                    MessageBoxButtons.OKCancel, MessageBoxIcon.Information);

                if (dr == DialogResult.OK)
                {
                    try
                    {
                        // 使用业务逻辑层删除管理员
                        bool result = _userService.DeleteAdmin(selectedAdminId, Login.username);

                        if (result)
                        {
                            // 从UI中移除
                            dgvEmp.Rows.RemoveAt(selectedRowIndex);
                            MessageBox.Show("删除成功");

                            // 重新加载数据
                            LoadAdmins();
                            dgvHead();
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

        private void btnno_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}