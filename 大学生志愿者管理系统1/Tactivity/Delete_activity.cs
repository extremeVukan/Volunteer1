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



namespace 大学生志愿者管理系统1.Tactivity
{
    public partial class Delete_activity : Form
    {
        public Delete_activity()
        {
            InitializeComponent();
        }
        SqlDataAdapter daActivity, dalog;
        DataSet ds = new DataSet();
        private string ShanchuID = "";
        void init()
        {
            DB.Getcn();
            string str = "select * from ActivityT where Holder='"+Login.username+"'";
            string sdr = "select * from dalogT";
            daActivity = new SqlDataAdapter(str, DB.cn);
            dalog = new SqlDataAdapter(sdr, DB.cn);
            daActivity.Fill(ds, "activity_info");
            dalog.Fill(ds, "log_info");
        }

        private void Delete_activity_Load(object sender, EventArgs e)
        {
            showXz();
            init();
            showAll();
        }
        void showAll()
        {
            DataView dvActivity = new DataView(ds.Tables["activity_info"]);
            dgvactivity.DataSource = dvActivity;
        }

        private void dgvactivity_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if(dgvactivity.Rows.Count>0)
            {
                for(int i=0;i<dgvactivity.Rows.Count; i++)
                {
                    DataGridViewCheckBoxCell ck = dgvactivity.Rows[i].Cells["acCode"] as DataGridViewCheckBoxCell;
                    if(i!=e.RowIndex)
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

        private void btnDel_Click(object sender, EventArgs e)
        {
            int s = dgvactivity.Rows.Count;
            for(int i=0;i<dgvactivity.Rows.Count;i++)
            {
                if(dgvactivity.Rows[i].Cells["acCode"].EditedFormattedValue.ToString()=="True")
                {
                    DialogResult dr = MessageBox.Show("是否要删除？", "提示",
                        MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                    if(dr==DialogResult.OK)
                    {
                        //寻找目标ID
                        string str = "select * from  activityT";
                        DataTable dt = DB.GetDataSet(str);
                        ShanchuID = dt.Rows[i][0].ToString();

                        dgvactivity.Rows.RemoveAt(i);
                        //添加日志
                        DataRow drlog = ds.Tables["log_info"].NewRow();
                        drlog["username"] = Login.username;
                        drlog["log_type"] = "删除";
                        drlog["action_date"] = DateTime.Now;
                        drlog["action_table"] = "activity表";
                        ds.Tables["log_info"].Rows.Add(drlog);
                        




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
            if(s==0)
            {
                MessageBox.Show("请选择要删除的对象");
            }
        }
        void UpdateDB()
        {
            try
            {
                SqlCommandBuilder dbActivity = new SqlCommandBuilder(daActivity);
                daActivity.Update(ds, "activity_info");
                SqlCommandBuilder dblog = new SqlCommandBuilder(dalog);
                dalog.Update(ds, "log_info");
            }
            catch(SqlException ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void btnCancle_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        void showXz()
        {
            DataGridViewCheckBoxColumn acCode = new DataGridViewCheckBoxColumn();
            acCode.Name = "acCode";
            acCode.HeaderText = "选择";
            dgvactivity.Columns.Add(acCode);
        }
    }
}
