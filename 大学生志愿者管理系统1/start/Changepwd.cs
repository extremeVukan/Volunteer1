using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 大学生志愿者管理系统1.start
{
    public partial class Changepwd : Form
    {
        public Changepwd()
        {
            InitializeComponent();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if(txtOldpwd.Text=="")
            {
                MessageBox.Show("请输入原密码");
            }
            else
            {
                DB.Getcn();
                string ser = "select * from adminT where admin_Name='" + txtName.Text + "'and telephone='" + txtOldpwd.Text + "'";
                DataTable dt = DB.GetDataSet(ser);
                if(dt.Rows.Count<1)
                {
                    MessageBox.Show("原密码错误请重新输入");
                }
                else
                {
                    if(txtNewpwd.Text=="" || txtNewpwd2.Text=="")
                    {
                        MessageBox.Show("请输入新密码");
                    }
                    else
                    {
                        if(txtNewpwd.Text!=txtNewpwd2.Text)
                        {
                            MessageBox.Show("两次密码不一致");
                        }
                        else
                        {
                            string sdr = "update adminT set telephone='"
                                + txtNewpwd.Text + "'where admin_Name='" + txtName.Text.Trim() + "'";
                            DB.sqlEx(sdr);
                            MessageBox.Show("修改成功");
                            this.Close();
                        }
                    }
                }
            }
        }

        private void Changepwd_Load(object sender, EventArgs e)
        {
            txtName.Text = Login.username;
        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {

        }

        private void BtnCanser_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
