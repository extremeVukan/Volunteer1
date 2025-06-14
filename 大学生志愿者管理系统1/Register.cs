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
    public partial class Register : Form
    {
        public Register()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(txtName.Text==""||txtPwd.Text=="")
            {
                MessageBox.Show("用户名或密码不能为空");
            }
            else 
            {
                DB.Getcn();
                string str = "select * from volunteerT where AName='" + txtName.Text + "'";
                DataTable dt = DB.GetDataSet(str);
                if(dt.Rows.Count>0)
                {
                    MessageBox.Show("用户名已存在");
                }
                else
                {
                    string sdr = "insert into volunteerT(AName,Atelephone,email) values('" + txtName.Text + "','" + txtPwd.Text + "','" + txtEmail.Text + "')";
                    DB.sqlEx(sdr);
                    MessageBox.Show("注册成功");
                    this.Close();
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Register_Load(object sender, EventArgs e)
        {

        }
    }
}
