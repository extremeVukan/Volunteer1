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

namespace 大学生志愿者管理系统1.Volunteer
{
    public partial class UpdateVol : Form
    {
        public UpdateVol()
        {
            InitializeComponent();
        }
        SqlDataAdapter daVol;
        DataSet ds = new DataSet();

        void init()
        {
            DB.Getcn();
            string str = "select Aid,AName,Atelephone,email from volunteerT";
            daVol = new SqlDataAdapter(str, DB.cn);
            daVol.Fill(ds, "Vol_info");
            DB.cn.Close();
        }
        void showAll()//DGV载入
        {
            DataView dvVol = new DataView(ds.Tables["Vol_info"]);
            dgvvol.DataSource = dvVol;
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
            init();
            showAll();
            dgvHead();
            
        }

        private void dgvvol_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            txtID.Text = dgvvol.CurrentRow.Cells[0].Value.ToString();
            txtName.Text = dgvvol.CurrentRow.Cells[1].Value.ToString();
            txtPhone.Text = dgvvol.CurrentRow.Cells[2].Value.ToString();
            txtemail.Text = dgvvol.CurrentRow.Cells[3].Value.ToString();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {

            if (txtName.Text == "" || txtPhone.Text == "" || txtemail.Text == "")
            {
                MessageBox.Show("姓名或联系方式或邮箱不能为空");

            }
            else
            {

                DialogResult dr = MessageBox.Show("您确定要修改吗？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);//弹窗固定格式
                if (dr == DialogResult.OK)
                {
                    DB.Getcn();
                    int a = dgvvol.CurrentCell.RowIndex;
                    string str = dgvvol.Rows[a].Cells["Aid"].Value.ToString();
                    DataRow[] ActRows = ds.Tables["Vol_info"].Select("Aid='"
                        + str + "'");
                    ActRows[0]["AName"] = txtName.Text;
                    ActRows[0]["Atelephone"] = txtPhone.Text.Trim();
                    ActRows[0]["email"] = txtemail.Text;
                    
                    
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
                cmd.Parameters["action_table"].Value = "Volunteer表";
                try
                {
                    SqlCommandBuilder dbAdmin = new SqlCommandBuilder(daVol);
                    daVol.Update(ds, "Vol_info");
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

