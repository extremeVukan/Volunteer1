using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 大学生志愿者管理系统1.Identify
{
    public partial class VolIdentify : Form
    {
        public VolIdentify()
        {
            InitializeComponent();
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            
            if (txtName.Text == "" || txtPhone.Text == ""||txtAddress.Text=="")
            {
                MessageBox.Show("请输入信息");
            }
            else
            {
                DB.Getcn();
                string str = "INSERT INTO VolIdentifyT (VID,VName,Phone, Province, City,Address, Status, EMPID) " +
                 "VALUES ('" + Convert.ToInt32(Login.Vol_ID) + "','" + txtName.Text + "', '" + txtPhone.Text + "', '"  +txtprovince.Text + "','" + txtcity.Text + "','"+txtAddress.Text+"', '未审核', NULL)";

                DB.sqlEx(str);
                MessageBox.Show("申请成功");
                DB.cn.Close();
                this.Close();
                
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
