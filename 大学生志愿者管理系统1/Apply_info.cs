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
    public partial class Apply_info : Form
    {
        public Apply_info()
        {
            InitializeComponent();
        }
        public string Holder = "";
        public string userid = "";
        public static string VV = "";

        private void button1_Click(object sender, EventArgs e)
        {
            if (txtName.Text == "" || txtphone.Text == "")
            {
                MessageBox.Show("请输入信息");
            }
            else
            {
                DB.Getcn();
                string str = "INSERT INTO OrderT (UserID,UserName, Orderdate, phone,ActID, Act_Name,Holder, status, EmpID) " +
                 "VALUES ('"+Convert.ToInt32(Login.Vol_ID)+"','"+ txtName.Text + "', '" + DateTime.Today + "', '" + txtphone.Text + "', '"+AQUEUE.ActID1+"', '" + txtActName.Text + "','" + Holder + "', '未审核', NULL)";

                DB.sqlEx(str);
                MessageBox.Show("申请成功");
                this.Close();
            }
        }

        private void Apply_info_Load(object sender, EventArgs e)
        {
            
            string sdr = "select * from ActivityT where activity_ID='" + AQUEUE.ActID1 + "'";
            DataTable dt = DB.GetDataSet(sdr);
            txtActName.Text = dt.Rows[0][1].ToString();
            Holder = dt.Rows[0][10].ToString();
            txtName.Text = Login.username;
            
        }
    }
}
