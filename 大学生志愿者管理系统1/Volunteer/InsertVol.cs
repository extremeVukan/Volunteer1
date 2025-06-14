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
namespace 大学生志愿者管理系统1.Volunteer
{
    public partial class InsertVol : Form
    {
        public InsertVol()
        {
            InitializeComponent();
        }
        SqlDataAdapter daVol, daLog;
        DataSet ds = new DataSet();

        void init() //初始化
        {
            DB.Getcn();
            string str = "select * from VolunteerT";
            string sdr = "select * from dalogT";
            daVol = new SqlDataAdapter(str, DB.cn);
            daLog = new SqlDataAdapter(sdr, DB.cn);
            daVol.Fill(ds, "Vol_info");
            daLog.Fill(ds, "log_info");
            DB.cn.Close();
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
            init();
            showAll();
            dgvHead();
            txtTime.Text = "0";
            int S = dgvvol.Rows.Count + 1;
            txtID.Text=S.ToString();
        }

        void showAll()
        {
            DataView dvvol = new DataView(ds.Tables["Vol_info"]);
            dgvvol.DataSource = dvvol;
        }
        private SqlTransaction tansactionInit()
        {
            DB.Getcn();
            SqlCommandBuilder dbVolunteer = new SqlCommandBuilder(daVol);
            SqlCommandBuilder dblog = new SqlCommandBuilder(daLog);

            daVol.InsertCommand = dbVolunteer.GetDeleteCommand();
            daLog.InsertCommand = dblog.GetDeleteCommand();

            SqlTransaction st = DB.cn.BeginTransaction();
            daVol.InsertCommand.Transaction = st;
            daLog.InsertCommand.Transaction = st;

            daVol.InsertCommand.Connection = DB.cn;
            daLog.InsertCommand.Connection = DB.cn;
            return st;
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
            }
            else
            {
                DB.Getcn();
                DialogResult dr = MessageBox.Show("是否要添加？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                if (dr == DialogResult.OK)
                {
                    string Aid = "select * from VolunteerT where Aid='" + txtID.Text + "'";
                    DataTable dt1 = DB.GetDataSet(Aid);
                    string Aname = "select * from VolunteerT where Aname='" + txtName.Text + "'";
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
                            DataRow volRow = ds.Tables["Vol_info"].NewRow();
                            volRow["Aid"] = Convert.ToInt32(txtID.Text);
                            volRow["Aname"] = txtName.Text;
                            volRow["email"] = txtemail.Text;
                            volRow["Atelephone"] = txtPhone.Text;
                            volRow["Act_Time"] = txtTime.Text;
                            ds.Tables["Vol_info"].Rows.Add(volRow);
                            MessageBox.Show("添加成功");

                            DataRow drlog = ds.Tables["log_info"].NewRow();
                            drlog["username"] = Login.username;
                            drlog["log_type"] = "添加";
                            drlog["action_date"] = DateTime.Now;
                            drlog["action_table"] = "Volunteer表";
                            ds.Tables["log_info"].Rows.Add(drlog);

                            UpdateDB();
                            
                        }

                    }

                }
                else
                {
                    return;
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
                //void updateDB()
                //{
                //SqlTransaction st = tansactionInit();
                //try
                //{
                //daVol.Update(ds, "Vol_info");
                //daLog.Update(ds, "log_info");
                //st.Commit();
                //DB.cn.Close();
                //}
                //catch (SqlException ex)
                //{
                //st.Rollback();
                //DB.cn.Close();
                //MessageBox.Show(ex.Message.ToString());
                //}
                //}
            }
        }
    }
}
