using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 大学生志愿者管理系统1
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }
        public static string username = "";
        public static string Vol_ID ="" ;
        public static string Emp_ID = "";
        public static int User = 0; //身份编码 0为志愿者，1为管理员，2为平台管理员
        private void btnyes_Click(object sender, EventArgs e)
        {
            if(txtName.Text==""|| txtPwd.Text=="")
            {
                MessageBox.Show("请输入用户名或密码");
            }
            else 
            {
                DB.Getcn();
                if(radioButton1.Checked)
                {
                    string spr = "select * from volunteerT where AName='" + txtName.Text + "'and Atelephone='" + txtPwd.Text + "'";
                    DataTable dt = DB.GetDataSet(spr);
                    if(dt.Rows.Count>0)
                    {
                        Vol_ID= dt.Rows[0][0].ToString();
                        MessageBox.Show("登录成功");
                        username = txtName.Text;
                        Form1.Aflag = true;
                        Form1.flag = 1;
                        this.Close();
                        this.DialogResult = DialogResult.OK;
                        
                    }
                    else
                    {
                        MessageBox.Show("用户名或密码错误");
                    }
                }
                if(radioButton2.Checked)
                {
                    string spr = "select * from adminT where admin_Name='" + txtName.Text + "'and telephone='" + txtPwd.Text + "'";
                    DataTable dt = DB.GetDataSet(spr);
                    if(dt.Rows.Count>0)
                    {
                        Emp_ID = dt.Rows[0][0].ToString();
                        MessageBox.Show("登陆成功");
                        username = txtName.Text;
                        Form1.Aflag = true;
                        Form1.flag = 2;
                        User = 1;
                        this.Hide();
                        Tindex t1 = new Tindex();
                        t1.ShowDialog();
                        
                    }
                    else
                    {
                        MessageBox.Show("用户名或密码错误");
                    }
                }
                if (radioButton3.Checked)
                {
                    string spr = "select * from zhuguanT where Sname='" + txtName.Text + "'and Semail='" + txtPwd.Text + "'";
                    DataTable dt = DB.GetDataSet(spr);
                    if (dt.Rows.Count > 0)
                    {
                        Emp_ID = dt.Rows[0][0].ToString();
                        MessageBox.Show("欢迎回来");
                        username = txtName.Text;
                        Form1.flag = 2;
                        User = 2;
                        this.Hide();
                        Tindex t1 = new Tindex();
                        t1.ShowDialog();
                    }
                    else
                    {
                        MessageBox.Show("用户名或密码错误");
                    }
                }
            }
        }

        private void btncancer_Click(object sender, EventArgs e)
        {
            this.Close();
            Form1 m1 = new Form1();
            m1.ShowDialog();
        }

        private void Login_Load(object sender, EventArgs e)
        {

        }
    }
}
