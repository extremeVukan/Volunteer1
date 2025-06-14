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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        public static int flag = 1; //1为主窗体，2为管理员窗体
        public static bool Aflag = false;//false为未登录,true已登录
        public static string Actid = "";
        public static int Time = 0;
        cartItem cartSrv = new cartItem();
        void showALLActiveites()
        {
            DB.Getcn();
            string str = "select * from ActivityT";
            SqlCommand cmd = new SqlCommand(str, DB.cn);
            SqlDataReader rdr = cmd.ExecuteReader();
            while(rdr.Read())
            {
                int index = dgvactivity.Rows.Add();
                dgvactivity.Rows[index].Cells[0].Value = rdr[0];
                dgvactivity.Rows[index].Cells[1].Value = rdr[1];
                dgvactivity.Rows[index].Cells[2].Value = rdr[2];
                dgvactivity.Rows[index].Cells[3].Value = rdr[3];
                dgvactivity.Rows[index].Cells[4].Value = rdr[4];
                dgvactivity.Rows[index].Cells[5].Value = rdr[5];
                dgvactivity.Rows[index].Cells[6].Value = rdr[6];
                dgvactivity.Rows[index].Cells[7].Value = rdr[10];
                dgvactivity.Rows[index].Cells[11].Value = rdr[11];
                try
                {
                    Image imageColumn = Image.FromFile(Application.StartupPath + rdr[7]);
                    dgvactivity.Rows[index].Cells["Column8"].Value = imageColumn;
                }
                catch
                {
                    Image imageColumn = Image.FromFile(Application.StartupPath + "\\" + "暂无图片.gif");
                    dgvactivity.Rows[index].Cells["Column8"].Value = imageColumn;
                }
            }
            rdr.Close();
        }




        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            dgvactivity.RowTemplate.Height = 90;
            showALLActiveites();
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            dgvactivity.Rows.Clear();
            showALLActiveites();
        }

        private void button1_Click_1(object sender, EventArgs e)//按商品名称关键字查询
        {
            if(txtPName.Text=="")
            {
                MessageBox.Show("请输入查询关键字");
            }
            else
            {
                dgvactivity.Rows.Clear();
                DB.Getcn();
                string str = "select * from ActivityT where activity_Name like '%" + txtPName.Text + "%'";
                SqlCommand cmd = new SqlCommand(str,DB.cn);
                SqlDataReader rdr = cmd.ExecuteReader();
                while(rdr.Read())
                {
                    int index = dgvactivity.Rows.Add();
                    dgvactivity.Rows[index].Cells[0].Value = rdr[0];
                    dgvactivity.Rows[index].Cells[1].Value = rdr[1];
                    dgvactivity.Rows[index].Cells[2].Value = rdr[2];
                    dgvactivity.Rows[index].Cells[3].Value = rdr[3];
                    dgvactivity.Rows[index].Cells[4].Value = rdr[4];
                    dgvactivity.Rows[index].Cells[5].Value = rdr[5];
                    dgvactivity.Rows[index].Cells[6].Value = rdr[6];
                    dgvactivity.Rows[index].Cells[7].Value = rdr[10];
                    dgvactivity.Rows[index].Cells[11].Value = rdr[11];
                    try
                    {
                        Image imageColumn = Image.FromFile(Application.StartupPath + rdr[7]);
                        dgvactivity.Rows[index].Cells["Column8"].Value = imageColumn;
                    }
                    catch
                    {
                        Image imageColumn = Image.FromFile(Application.StartupPath + "\\" + "暂无图片.gif");
                        dgvactivity.Rows[index].Cells["Column8"].Value = imageColumn;
                    }
                }
                rdr.Close();
            }    
        }

        private void btnsearch2_Click(object sender, EventArgs e) //按照活动类型查询
        {
            dgvactivity.Rows.Clear();
            string str = "";
            DB.Getcn();
            int i = comboBox1.SelectedIndex;
            switch(i)
            {
                case 0:
                    str = "activity_type='教育类'";
                    break;
                case 1:
                    str = "activity_type='社区发展类'";
                    break;
                case 2:
                    str = "activity_type='卫生与医疗类'";
                    break;
                case 3:
                    str = "activity_type='环境保护类'";
                    break;
                case 4:
                    str = "activity_type='动物保护类'";
                    break;
                default:
                    str = "activity_type='紧急救援类'";
                    break;
            }
            string sdr = "select * from ActivityT where "+str+"";
            SqlCommand cmd = new SqlCommand(sdr, DB.cn);
            SqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                int index = dgvactivity.Rows.Add();
                dgvactivity.Rows[index].Cells[0].Value = rdr[0];
                dgvactivity.Rows[index].Cells[1].Value = rdr[1];
                dgvactivity.Rows[index].Cells[2].Value = rdr[2];
                dgvactivity.Rows[index].Cells[3].Value = rdr[3];
                dgvactivity.Rows[index].Cells[4].Value = rdr[4];
                dgvactivity.Rows[index].Cells[5].Value = rdr[5];
                dgvactivity.Rows[index].Cells[6].Value = rdr[6];
                dgvactivity.Rows[index].Cells[7].Value = rdr[10];
                dgvactivity.Rows[index].Cells[11].Value = rdr[11];
                try
                {
                    Image imageColumn = Image.FromFile(Application.StartupPath + rdr[7]);
                    dgvactivity.Rows[index].Cells["Column8"].Value = imageColumn;
                }
                catch
                {
                    Image imageColumn = Image.FromFile(Application.StartupPath + "\\" + "暂无图片.gif");
                    dgvactivity.Rows[index].Cells["Column8"].Value = imageColumn;
                }
            }
            rdr.Close();
        }

        private void btnloading_Click(object sender, EventArgs e)
        {
            if (Aflag == true)
            {
                MessageBox.Show("已登录");
            }
            else
            {
                this.Hide();
                Login ll = new Login();
                ll.ShowDialog();
                if (flag == 1)
                {
                    this.Visible = true;
                    label5.Text = Login.username;
                }
            }
            
        }

        private void btnregister_Click(object sender, EventArgs e)
        {
            Register r1 = new Register();
            r1.ShowDialog();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void dgvactivity_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
            int Cindex = e.ColumnIndex;
            //如果是第八列，查看活动详细信息
            if(Cindex==9)
            {
                //确定行数
                int rowindex = e.RowIndex;
                Actid = dgvactivity.Rows[rowindex].Cells[0].Value.ToString();
                Act_info t1 = new Act_info();
                t1.ShowDialog();
            }
            //如果是第九列，查看申请列表
            if(Cindex==10)
            {
                if(Aflag==false)
                {
                    MessageBox.Show("请登录账号");
                    Login l1 = new Login();
                    l1.ShowDialog();
                    label5.Text = Login.username;
                }
                else
                {
                    //确定是第几行
                    int rowindex2 = e.RowIndex;
                    Actid = dgvactivity.Rows[rowindex2].Cells[0].Value.ToString();
                    string Num = dgvactivity.Rows[rowindex2].Cells[6].Value.ToString();
                    string status = dgvactivity.Rows[rowindex2].Cells[11].Value.ToString();
                    int s1 = Convert.ToInt32(Num);
                    DB.Getcn();

                    string str = "select * from Actapply_T where Act_ID=" + Actid + "";
                    
                    string sqr = "select * from ACTMember where volunteerid=" + Login.Vol_ID + "AND ACTID='"+Actid+"'";
                    string sdr = "select * from ACTMember where ACTID='"+Actid + "'";
                    DataTable dt = DB.GetDataSet(str);
                    DataTable dt1 = DB.GetDataSet(sqr);
                    DataTable dt2 = DB.GetDataSet(sdr);
                    string AttenNum = dt2.Rows.Count.ToString();
                    int s2 = Convert.ToInt32(AttenNum);
                    
                    if (dt1.Rows.Count>0)
                    {
                        MessageBox.Show("你已经参加了这个活动");
                    }
                    else if (status == "已结束")
                    {
                        MessageBox.Show("该活动已结束");
                    }
                    else if (s1 == s2)
                    {
                        MessageBox.Show("人数已满");
                    }
                    else if (dt.Rows.Count > 0)
                    {
                        MessageBox.Show("已申请");
                    }
                    else
                    {
                        cartSrv.Add(Convert.ToInt32(Login.Vol_ID), Convert.ToInt32(Form1.Actid), 1);
                        MessageBox.Show("添加成功");

                    }
                }
            }
        }

        private void btnQuit_Click(object sender, EventArgs e)
        {
            if(Login.username=="")
            {
                MessageBox.Show("请登录账号");
            }
            else
            {
                MessageBox.Show("已退出账号");
                
                Login.username = "";
                label5.Text = "";
                Aflag = false;
                label8.Text = "";
            }
        }

        private void btncheck_Click(object sender, EventArgs e)
        {
            AQUEUE s1 = new AQUEUE();
            s1.ShowDialog();
        }

        private void btnShowAqueue_Click(object sender, EventArgs e)
        {
            if (Login.username=="")
            {
                MessageBox.Show("请登录账号");

            }
            else
            {
                AQUEUE s1 = new AQUEUE();
                s1.ShowDialog();
            }
        }

        private void btnVolTime_Click(object sender, EventArgs e)
        {
            if (Login.username == "")
            {
                MessageBox.Show("请登录账号");
                Login s1 = new Login();
                s1.ShowDialog();
            }
            else {
                string str = "select Act_Time from VolunteerT where Aid='" + Login.Vol_ID + "'";
                DataTable dt = DB.GetDataSet(str);
                Time = Convert.ToInt32(dt.Rows[0][0].ToString());
                label8.Text = dt.Rows[0][0].ToString() + "小时";
            }
        }

        private void btnidentity_Click(object sender, EventArgs e)
        {
            
            if (Aflag == false)
            {
                MessageBox.Show("请登录账号");
                
            }
            else
            {
                SelfCheck.SelfInfoCheck1 s1 =new SelfCheck.SelfInfoCheck1();
                s1.ShowDialog();
            }
        }

        private void btnCard_Click(object sender, EventArgs e)
        {
            string sqr = "select * from VolIdentifyT where VName='" + Login.username + "'";
            DataTable dt = DB.GetDataSet(sqr);
            if (dt.Rows.Count > 0)
            {
                MessageBox.Show("您已申请志愿者证");
            }
            else if (Aflag == false)
            {
                MessageBox.Show("请登录账号");
            }
            else if (Time < 10)
            {
                MessageBox.Show("很抱歉，您的志愿时长不足15小时");
            }
            else
            {
                MessageBox.Show("欢迎申请志愿者证");
                Identify.VolIdentify s1 =new Identify.VolIdentify();
                s1 .ShowDialog();
            }
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            dgvactivity.Rows.Clear();
            dgvactivity.RowTemplate.Height = 90;
            showALLActiveites();
        }
    }
}
