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
namespace 大学生志愿者管理系统1.Order
{
    public partial class OrderTables : Form
    {
        public OrderTables()
        {
            InitializeComponent();
        }
        SqlDataAdapter daOrder;
        DataSet ds = new DataSet();
        public static string VolID = "";
        public static string VolName = "";
        public static string VolPhone = "";
        public static string ActID = "";
        public static string ActName = "";
        void init()
        {
            DB.Getcn();
            string str = "select * from OrderT where Holder='" +Login.username+"'";
            daOrder = new SqlDataAdapter(str, DB.cn);
            daOrder.Fill(ds, "order_info");
        }
        void showAll()
        {
            init();
            showXZ();
        }
        void showXZ()
        {
            DataGridViewCheckBoxColumn asCode = new DataGridViewCheckBoxColumn();
            asCode.Name = "asCode";
            asCode.HeaderText = "选择";
            dgvOrder.Columns.Add(asCode);
        }

        private void OrderTables_Load(object sender, EventArgs e)
        {
            showAll();
            DataView dvOrder = new DataView(ds.Tables["order_info"]);
            dgvOrder.DataSource = dvOrder;
        }

        private void dgvOrder_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvOrder.Rows.Count > 0)
            {
                for (int i = 0; i < dgvOrder.Rows.Count; i++)
                {
                    DataGridViewCheckBoxCell ck = dgvOrder.Rows[i].Cells["asCode"] as DataGridViewCheckBoxCell;
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
            int s = dgvOrder.Rows.Count;
            for(int i=0;i<dgvOrder.Rows.Count;i++)
            {
                if (dgvOrder.Rows[i].Cells["asCode"].EditedFormattedValue.ToString() == "True")
                {
                    if (dgvOrder.Rows[i].Cells["EmpID"].Value == null)
                    {
                        MessageBox.Show("已审核");
                    }
                    else
                    {
                        dgvOrder.Rows[i].Cells["status"].Value = "已审核";
                        if (string.IsNullOrEmpty(dgvOrder.Rows[i].Cells["EmpID"].Value?.ToString()))
                        {
                            dgvOrder.Rows[i].Cells["EmpID"].Value = Login.Emp_ID;
                            VolID = dgvOrder.Rows[i].Cells["UserID"].Value?.ToString();
                            VolName = dgvOrder.Rows[i].Cells["UserName"].Value?.ToString();
                            VolPhone = dgvOrder.Rows[i].Cells[4].Value?.ToString();
                            ActID = dgvOrder.Rows[i].Cells[5].Value?.ToString();
                            ActName = dgvOrder.Rows[i].Cells[6].Value?.ToString();

                            MessageBox.Show("审核成功");



                        }


                        VolID = dgvOrder.Rows[i].Cells["UserID"].Value?.ToString();
                        VolName = dgvOrder.Rows[i].Cells["UserName"].Value?.ToString();
                        VolPhone = dgvOrder.Rows[i].Cells["phone"].Value?.ToString();
                        ActID = dgvOrder.Rows[i].Cells["ActID"].Value?.ToString();
                        ActName = dgvOrder.Rows[i].Cells[6].Value?.ToString();

                        DB.Getcn();

                        string sqr = "INSERT INTO ACTMember (ACTID,ACTNAME, Volunteerid,volunteer,PHONE,TIME) " +
                        "VALUES ('" + Convert.ToInt32(ActID) + "','" + ActName + "','" + Convert.ToInt32(VolID) + "','" + VolName + "','" + VolPhone + "','" + '0' + "')";
                        DB.sqlEx(sqr);






                        UpdateDB();

                    }
                }
                else
                {
                    s = s - 1;
                }
                if (s == 0)
                {
                    MessageBox.Show("请选择审核项");
                }
            }
            void UpdateDB()
            {
                try
                {
                    SqlCommandBuilder dbOrder = new SqlCommandBuilder(daOrder);
                    daOrder.Update(ds, "order_info");
                }
                catch(SqlException ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
        }

        private void btncancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
        