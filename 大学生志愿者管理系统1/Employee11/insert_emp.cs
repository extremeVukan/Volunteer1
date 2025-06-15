using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace 大学生志愿者管理系统1.Employee11
{
    public partial class insert_emp : Form
    {
        public insert_emp()
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

        private void insert_emp_Load(object sender, EventArgs e)
        {
            init();
            showAll();
            dgvHead();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (txtID.Text == "" || txtName.Text == "")
            {
                MessageBox.Show("编号与姓名不能为空");
            }
            else
            {
                DB.Getcn();
                DialogResult dr = MessageBox.Show("是否要添加？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                if (dr == DialogResult.OK)
                {
                    string Aid = "select * from adminT where admin_ID='" + txtID.Text + "'";
                    DataTable dt1 = DB.GetDataSet(Aid);
                    string Aname = "select * from adminT where admin_Name='" + txtName.Text + "'";
                    DataTable dt2 = DB.GetDataSet(Aname);
                    if (dt1.Rows.Count > 0)
                    {
                        MessageBox.Show("ID已存在");
                    }
                    else
                    {
                        if (dt2.Rows.Count > 0)
                        {
                            MessageBox.Show("名称已存在");
                        }
                        else
                        {
                            DataRow AdmRow = ds.Tables["Admin_info"].NewRow();
                            AdmRow["admin_ID"] =Convert.ToInt32(txtID.Text);
                            AdmRow["admin_Name"] =txtName.Text;
                            AdmRow["sex"] =txtSex.Text;
                            AdmRow["birth_date"] =dtpBirth.Value;
                            AdmRow["hire_date"] =dtpHire.Value;
                            AdmRow["address"] =txtAdress.Text;
                            AdmRow["telephone"] =txtPhone.Text;
                            AdmRow["wages"] =Convert.ToInt32(txtWages.Text);
                            AdmRow["resume"] =txtResume.Text;
                            ds.Tables["Admin_info"].Rows.Add(AdmRow);
                            MessageBox.Show("添加成功");
                        }
                    }
                }
                else
                {
                    return;
                }
                //存储过程添加日志
                SqlCommand cmd = new SqlCommand("add_log", DB.cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("username", SqlDbType.NVarChar));
                cmd.Parameters.Add(new SqlParameter("log_type", SqlDbType.NVarChar));
                cmd.Parameters.Add(new SqlParameter("action_date", SqlDbType.DateTime));
                cmd.Parameters.Add(new SqlParameter("action_table", SqlDbType.NVarChar));
                cmd.Parameters["username"].Value = Login.username;
                cmd.Parameters["log_type"].Value = "添加";
                cmd.Parameters["action_date"].Value = DateTime.Now;
                cmd.Parameters["action_table"].Value = "AdminT表";
                try
                {
                    SqlCommandBuilder dbAdmin = new SqlCommandBuilder(daAdmin);
                    daAdmin.Update(ds,"Admin_info");
                    cmd.ExecuteNonQuery();
                }
                catch(SqlException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void btncancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
