using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using 大学生志愿者管理系统1.Log;

namespace 大学生志愿者管理系统1
{
    public partial class Tindex : Form
    {
        public Tindex()
        {
            InitializeComponent();
        }

        private void 修改密码ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            start.Changepwd ch1 = new start.Changepwd();
            ch1.ShowDialog();
        }

        private void 个人信息ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            start.Person p1 = new start.Person();
            p1.ShowDialog();
        }

        private void 添加活动信息ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Tactivity.Insert_activity in1 = new Tactivity.Insert_activity();
            in1.ShowDialog();
        }

        private void 修改活动信息ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Tactivity.Update_activity u2 = new Tactivity.Update_activity();
            u2.ShowDialog();
        }

        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e) 
        {
            Login.username = "";
            this.Hide();
            Login c1 = new Login();
            c1.ShowDialog();
            
        }

        private void 删除活动信息ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Tactivity.Delete_activity d1 = new Tactivity.Delete_activity();
            d1.ShowDialog();
        }

        private void 审核申请ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Order.OrderTables d1 = new Order.OrderTables();
            d1.ShowDialog();
        }

        private void 增加管理员ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(Login.User!=2)
            {
                MessageBox.Show("身份验证失败");
            }
            else
            {
                Employee11.insert_emp s1 = new Employee11.insert_emp();
                s1.ShowDialog();
            }
        }

        private void 添加志愿者ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Login.User != 2)
            {
                MessageBox.Show("身份验证失败");
            }
            else
            {
                Volunteer.InsertVol a1 = new Volunteer.InsertVol();
                a1.ShowDialog();
            }
        }

        private void 结束志愿ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ActStop.ActStop A1 = new ActStop.ActStop();
            A1.ShowDialog();
        }

        private void 修改管理员信息ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Login.User != 2)
            {
                MessageBox.Show("身份验证失败");
            }
            else
            {
                Employee11.update_emp s1 = new Employee11.update_emp();
                s1.ShowDialog();
            }
        }

        private void 删除管理员信息ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Login.User != 2)
            {
                MessageBox.Show("身份验证失败");
            }
            else
            {
                Employee11.delete_emp s1 = new Employee11.delete_emp();
                s1.ShowDialog();
            }
        }

        private void 日志管理ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Login.User != 2)
            {
                MessageBox.Show("身份验证失败");
            }
            else
            {
                Log.Log_Check s1 =new Log_Check();  
                s1.ShowDialog();
            }
        }

        private void 修改志愿者信息ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Login.User != 2)
            {
                MessageBox.Show("身份验证失败");
            }
            else
            {
                Volunteer.UpdateVol s1=new Volunteer.UpdateVol();
                s1.ShowDialog();
            }
        }

        private void 删除志愿者信息ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
                if (Login.User != 2)
                {
                    MessageBox.Show("身份验证失败");
                }
                else
                {
                    Volunteer.deleteVol s1 = new Volunteer.deleteVol();
                    s1.ShowDialog();
                }
            
        }

        private void 志愿者证件申请管理ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Login.User != 2)
            {
                MessageBox.Show("身份验证失败");
            }
            else
            {
                Identify.ExamineIdentify s1 = new Identify.ExamineIdentify(); 
                s1 .ShowDialog();
            }
        }

        private void Tindex_Load(object sender, EventArgs e)
        {
            label1.Text = Login.username;
            string str = "select * from ActivityT where Holder='" + Login.username + "'";
            DataTable dt = DB.GetDataSet(str);
            label2.Text=dt.Rows.Count.ToString()+"次活动";
            if(Login.User == 2)
            {
                label3.Text = "平台管理员";
            }
        }
    }
}
