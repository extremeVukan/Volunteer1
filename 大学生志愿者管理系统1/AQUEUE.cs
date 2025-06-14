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



namespace 大学生志愿者管理系统1
{
    public partial class AQUEUE : Form
    {
        public AQUEUE()
        {
            InitializeComponent();
        }
        SqlDataAdapter daAqueue, daActivity;
        DataSet ds = new DataSet();
        cartItem cartSrv = new cartItem();
        public static string ActID1 = "";//给后面的表搜索
        
        void init()
        {
            //注意修改,申请队列相关
            DB.Getcn();
            string str = "select Aqueue_ID,Vol_ID,Act_ID,Act_Name,Act_place,Need,Holder from ACTapply_T where Vol_ID=" +  Convert.ToInt32(Login.Vol_ID)+"";
            daAqueue = new SqlDataAdapter(str,DB.cn);
            daAqueue.Fill(ds, "Aqueue_info");
            string sdr = "select * from ActivityT";
            daActivity = new SqlDataAdapter(sdr, DB.cn);
            daActivity.Fill(ds, "Act_info");

        }
        void showXZ()//选择列
        {
            DataGridViewCheckBoxColumn asCode = new DataGridViewCheckBoxColumn();
            asCode.Name = "asCode";
            asCode.HeaderText = "选择";
            dgvAqueue.Columns.Add(asCode);
        }
        void dgvHead()
        {
            dgvAqueue.Columns[0].HeaderText = "编号";
            dgvAqueue.Columns[1].HeaderText = "志愿者编";
            dgvAqueue.Columns[2].HeaderText = "活动编号";
            dgvAqueue.Columns[3].HeaderText = "活动名称";
            dgvAqueue.Columns[4].HeaderText = "活动地点";
            dgvAqueue.Columns[5].HeaderText = "活动人数";
            dgvAqueue.Columns[6].HeaderText = "志愿发起人";
        }

        protected void Bind()
        {
            init();
            DataView dvAqueue = new DataView(ds.Tables["Aqueue_info"]);
            dgvAqueue.DataSource = dvAqueue;
            dgvHead();
            showXZ();
        }

        private void AQUEUE_Load(object sender, EventArgs e)
        {
            DB.Getcn();
            Bind();
            
        }

        private void dgvAqueue_CellContentClick(object sender, DataGridViewCellEventArgs e) //多选转单选
        {
            if (dgvAqueue.Rows.Count > 0)
            {
                for(int i =0; i<dgvAqueue.Rows.Count; i++)
                {
                    DataGridViewCheckBoxCell ck = dgvAqueue.Rows[i].Cells["asCode"] as DataGridViewCheckBoxCell;
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

        private void btnDelete_Click(object sender, EventArgs e)//删除志愿选择
        {
            int s = dgvAqueue.Rows.Count;
            for(int i=0; i<dgvAqueue.Rows.Count; i++)
            {
                if(dgvAqueue.Rows[i].Cells["asCode"].EditedFormattedValue.ToString()=="True")
                {
                    dgvAqueue.Rows.RemoveAt(i);
                    updateDB();//更新数据源
                    
                    

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
        }
        void updateDB()
        {
            try
            {
                SqlCommandBuilder dbAqueue = new SqlCommandBuilder(daAqueue);
                daAqueue.Update(ds, "Aqueue_info");
                SqlCommandBuilder dbAct = new SqlCommandBuilder(daActivity);
                daActivity.Update(ds, "Act_info");          
            }
            catch(SqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        void clean_aqueue()
        {
            List<int> List_index = new List<int>();
            if (dgvAqueue.Rows.Count > 0)
            {
                for(int i = 0; i < dgvAqueue.Rows.Count - 1; i++)
                {
                    List_index.Add(i);
                }
                int z = 0;
                foreach(int n in List_index)//始终删去第0行
                {
                    dgvAqueue.Rows.RemoveAt(n - z);
                    z++;
                }
            }
        }

        private void btnClean_Click(object sender, EventArgs e)
        {
            clean_aqueue();
            updateDB();
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            int s = dgvAqueue.Rows.Count;
            for (int i = 0; i < dgvAqueue.Rows.Count; i++)
            {
                if (dgvAqueue.Rows[i].Cells["asCode"].EditedFormattedValue.ToString() == "True")
                {
                    
                    string sqr = "select * from ACTapply_T";
                    DataTable dt = DB.GetDataSet(sqr);
                    //搜索活动名称,举办人

                    ActID1 = dt.Rows[i][2].ToString();
                    MessageBox.Show("进入申请页面");

                    this.Hide();
                    Apply_info s1 = new Apply_info();
                    s1.ShowDialog();
                    
                    dgvAqueue.Rows.RemoveAt(i);
                    updateDB();//更新数据源
                }
                else
                {
                    s = s - 1;
                }
            }
            if (s == 0)
            {
                MessageBox.Show("选择要申请的志愿");
            }
        }
        private void btnReturn_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
