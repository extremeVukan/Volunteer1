using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;

namespace 大学生志愿者管理系统1.SelfCheck
{
    public partial class SelfInfoCheck1 : Form
    {
        public SelfInfoCheck1()
        {
            InitializeComponent();
        }
        SqlDataAdapter daAct;
        DataSet ds = new DataSet();
        public static string ACTID2 = "";

        void init()
        {
            DB.Getcn();
            string str = "select UserID,UserName,orderdate,ActID,Act_Name from OrderT where UserName='" + Login.username + "'";
            daAct = new SqlDataAdapter(str, DB.cn);
            daAct.Fill(ds, "Act_info");
            DB.cn.Close();
        }
        void showAll()//DGV载入
        {
            DataView dvAct = new DataView(ds.Tables["Act_info"]);
            dgvShowAtten.DataSource = dvAct;
        }
        void dgvHead()
        {
            this.dgvShowAtten.Columns[0].HeaderText = "ID";
            this.dgvShowAtten.Columns[1].HeaderText = "姓名";
            this.dgvShowAtten.Columns[2].HeaderText = "日期";
            this.dgvShowAtten.Columns[3].HeaderText = "活动编号";
            this.dgvShowAtten.Columns[4].HeaderText = "活动";


        }
        void showXZ()
        {
            DataGridViewCheckBoxColumn asCode = new DataGridViewCheckBoxColumn();
            asCode.Name = "asCode";
            asCode.HeaderText = "选择";
            dgvShowAtten.Columns.Add(asCode);
        }

        private void SelfInfoCheck1_Load(object sender, EventArgs e)
        {
            string sqr = "select * from VolIdentifyT where VName='" + Login.username + "'";
            DataTable dt1 = DB.GetDataSet(sqr);

            if (dt1.Rows.Count == 0)
            {

                txtStatus.Text = "未拥有";
            }
            else
            {
                txtStatus.Text = dt1.Rows[0][7].ToString();
            }
            txtName.Text = Login.username;
            string str = "select Act_Time from VolunteerT where Aid='" + Login.Vol_ID + "'";
            DataTable dt = DB.GetDataSet(str);
            int Time = Convert.ToInt32(dt.Rows[0][0].ToString());
            txtTime.Text = dt.Rows[0][0].ToString() + "小时";
            init();
            showAll();
            dgvHead();
            showXZ();

        }

        private void dgvShowAtten_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvShowAtten.Rows.Count > 0)
            {
                for (int i = 0; i < dgvShowAtten.Rows.Count; i++)
                {
                    DataGridViewCheckBoxCell ck = dgvShowAtten.Rows[i].Cells["asCode"] as DataGridViewCheckBoxCell;
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

        private void btncheck_Click(object sender, EventArgs e)
        {
            int s = dgvShowAtten.Rows.Count;
            for (int i = 0; i < dgvShowAtten.Rows.Count; i++)
            {
                if (dgvShowAtten.Rows[i].Cells["asCode"].EditedFormattedValue.ToString() == "True")
                {
                    ACTID2 = dgvShowAtten.Rows[i].Cells[3].Value.ToString();
                    SelfCheck.Sign s1 =new SelfCheck.Sign();
                    s1.ShowDialog();
                }
            }
        }

        private void btnSWEEP_Click(object sender, EventArgs e)
        {
            dgvShowAtten.Rows.Clear();
            init();
            showAll();
            dgvHead();
            
        }
    }
}
