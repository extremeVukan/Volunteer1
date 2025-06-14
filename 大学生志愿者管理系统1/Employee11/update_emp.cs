using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace 大学生志愿者管理系统1.Employee11
{
    public partial class update_emp : Form
    {
        public update_emp()
        {
            InitializeComponent();
        }
        SqlDataAdapter daAdmin;
        DataSet ds = new DataSet();

        void init()
        {
            DB.Getcn();
            string str = "select * from adminT";
            daAdmin = new SqlDataAdapter(str, DB.cn);
            daAdmin.Fill(ds, "Admin_info");
            DB.cn.Close();
        }
        void showAll()//DGV载入
        {
            DataView dvAdmin = new DataView(ds.Tables["Admin_info"]);
            dgvEmp.DataSource = dvAdmin;
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
            init();
            showAll();
            dgvHead();
        }

        private void dgvEmp_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            txtid.Text= dgvEmp.CurrentRow.Cells[0].Value.ToString();
            txtname.Text = dgvEmp.CurrentRow.Cells[1].Value.ToString();
            txtsex.Text = dgvEmp.CurrentRow.Cells[2].Value.ToString();
            dtpbirth.Text = dgvEmp.CurrentRow.Cells[3].Value.ToString();
            dtphire.Text = dgvEmp.CurrentRow.Cells[4].Value.ToString();
            txtaddress.Text = dgvEmp.CurrentRow.Cells[5].Value.ToString();
            txtphone.Text = dgvEmp.CurrentRow.Cells[6].Value.ToString();
            txtwages.Text = dgvEmp.CurrentRow.Cells[7].Value.ToString();
            txtresume.Text = dgvEmp.CurrentRow.Cells[8].Value.ToString();
        }

        private void btnYes_Click(object sender, EventArgs e)
        {
            if (txtname.Text == "" || txtphone.Text == "" || txtsex.Text == "")
            {
                MessageBox.Show("姓名或联系方式或性别不能为空");

            }
            else
            {
                
                DialogResult dr = MessageBox.Show("您确定要修改吗？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);//弹窗固定格式
                if (dr == DialogResult.OK)
                {
                    DB.Getcn();
                    int a = dgvEmp.CurrentCell.RowIndex;
                    string str = dgvEmp.Rows[a].Cells["admin_ID"].Value.ToString();
                    DataRow[] ActRows = ds.Tables["Admin_info"].Select("admin_ID='"
                        + str + "'");
                    ActRows[0]["admin_name"] = txtname.Text;
                    ActRows[0]["sex"] = txtsex.Text.Trim();
                    ActRows[0]["birth_date"] = dtpbirth.Value;
                    ActRows[0]["hire_date"] = dtphire.Value;
                    ActRows[0]["address"] = txtaddress.Text;
                    ActRows[0]["wages"] = txtwages.Text;
                    ActRows[0]["resume"] = txtresume.Text;
                }
                SqlCommand cmd = new SqlCommand("add_log", DB.cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("username", SqlDbType.NVarChar));
                cmd.Parameters.Add(new SqlParameter("log_type", SqlDbType.NVarChar));
                cmd.Parameters.Add(new SqlParameter("action_date", SqlDbType.DateTime));
                cmd.Parameters.Add(new SqlParameter("action_table", SqlDbType.NVarChar));
                cmd.Parameters["username"].Value = Login.username;
                cmd.Parameters["log_type"].Value = "修改";
                cmd.Parameters["action_date"].Value = DateTime.Now;
                cmd.Parameters["action_table"].Value = "AdminT表";
                try
                {
                    SqlCommandBuilder dbAdmin = new SqlCommandBuilder(daAdmin);
                    daAdmin.Update(ds, "Admin_info");
                    cmd.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    MessageBox.Show(ex.Message);
                    
                }
                MessageBox.Show("修改成功");
                
            }
            DB.cn.Close();
        }

        private void btncancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
