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

namespace 大学生志愿者管理系统1.Volunteer
{
    public partial class deleteVol : Form
    {
        public deleteVol()
        {
            InitializeComponent();
        }
        private string ShanchuID = "";
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
        void showXz()
        {
            DataGridViewCheckBoxColumn acCode = new DataGridViewCheckBoxColumn();
            acCode.Name = "acCode";
            acCode.HeaderText = "选择";
            dgvvol.Columns.Add(acCode);
        }
        

        private void dgvvol_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
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


        void UpdateDB()
        {
            try
            {
                SqlCommandBuilder dbvolunteer = new SqlCommandBuilder(daVol);
                daVol.Update(ds, "Vol_info");

            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void btndelete_Click(object sender, EventArgs e)
        {
            {
                int s = dgvvol.Rows.Count;
                for (int i = 0; i < dgvvol.Rows.Count; i++)
                {
                    if (dgvvol.Rows[i].Cells["acCode"].EditedFormattedValue.ToString() == "True")
                    {
                        DialogResult dr = MessageBox.Show("是否要删除？", "提示",
                            MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                        if (dr == DialogResult.OK)
                        {
                            DB.Getcn();
                            //寻找目标ID
                            string str = "select * from  volunteerT";
                            DataTable dt = DB.GetDataSet(str);
                            ShanchuID = dt.Rows[i][0].ToString();

                            dgvvol.Rows.RemoveAt(i);
                            //添加日志
                            SqlCommand cmd = new SqlCommand("add_log", DB.cn);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add(new SqlParameter("username", SqlDbType.NVarChar));
                            cmd.Parameters.Add(new SqlParameter("log_type", SqlDbType.NVarChar));
                            cmd.Parameters.Add(new SqlParameter("action_date", SqlDbType.DateTime));
                            cmd.Parameters.Add(new SqlParameter("action_table", SqlDbType.NVarChar));
                            cmd.Parameters["username"].Value = Login.username;
                            cmd.Parameters["log_type"].Value = "删除";
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



                            MessageBox.Show("删除成功");
                            UpdateDB();
                        }
                        else
                        {
                            return;
                        }
                    }
                    else
                    {
                        s = s - 1;
                    }
                }
                if (s == 0)
                {
                    MessageBox.Show("请选择要删除的对象");
                }
                DB.cn.Close();
            }

        }

        private void deleteVol_Load(object sender, EventArgs e)
        {
            init();
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
