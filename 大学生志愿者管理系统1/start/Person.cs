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
    public partial class Person : Form
    {
        public Person()
        {
            InitializeComponent();
        }
        private void Person_Load(object sender, EventArgs e)
        {
            if (Login.User == 2)
            {
                DB.Getcn();
                string str1 = "select * from  zhuguanT where Sname='" + Login.username + "'";
                DataTable dt1 = DB.GetDataSet(str1);
                txtId.Text = dt1.Rows[0][0].ToString();
                txtName.Text = dt1.Rows[0][1].ToString();
                txtPhone.Text = dt1.Rows[0][2].ToString();
            }
            else
            {


                DB.Getcn();
                string str = "select * from  adminT where admin_Name='" + Login.username + "'";
                DataTable dt = DB.GetDataSet(str);
                txtId.Text = dt.Rows[0][0].ToString();
                txtName.Text = dt.Rows[0][1].ToString();
                txtSex.Text = dt.Rows[0][2].ToString();
                dtpBirth.Value = Convert.ToDateTime(dt.Rows[0][3]);
                dtpHire.Value = Convert.ToDateTime(dt.Rows[0][4]);
                txtAdress.Text = dt.Rows[0][5].ToString();
                txtPhone.Text = dt.Rows[0][6].ToString();
                txtWages.Text = dt.Rows[0][7].ToString();
                txtSume.Text = dt.Rows[0][8].ToString();
            }
        }
        

        private void btnOK_Click(object sender, EventArgs e)
        {
            DB.Getcn();
            string str = "update adminT set admin_Name='"
                + txtName.Text + "',sex='"
                + txtSex.Text + "',birth_date='"
                + dtpBirth.Value + "',hire_date='"
                + dtpHire.Value + "',address='"
                + txtAdress.Text + "',telephone='"
                + txtPhone.Text + "',wages='"
                + Convert.ToInt32(txtWages.Text) + "',resume='"
                + txtSume.Text + "'where admin_id=" + Convert.ToInt32(txtId.Text) + "";
            DB.sqlEx(str);
            MessageBox.Show("修改成功");

        }

        private void btnCanser_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
