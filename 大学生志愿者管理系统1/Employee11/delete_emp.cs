using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace 大学生志愿者管理系统1.Employee11
{
    public partial class delete_emp : Form
    {
        public delete_emp()
        {
            InitializeComponent();
        }
        private string ShanchuID = "";
        SqlDataAdapter daAdmin;
        DataSet ds = new DataSet();

        void init()
        {
            DB.Getcn();
            string str = "select * from adminT";
            daAdmin = new SqlDataAdapter(str, DB.cn);
            daAdmin.Fill(ds, "Admin_info");
            DB.cn.Close();
        }
        void showAll()//DGV载入
        {
            DataView dvAdmin = new DataView(ds.Tables["Admin_info"]);
            dgvEmp.DataSource = dvAdmin;
        }
        void dgvHead()
        {
            this.dgvEmp.Columns[0].HeaderText = "ID";
            this.dgvEmp.Columns[1].HeaderText = "姓名";
            this.dgvEmp.Columns[2].HeaderText = "性别";
            this.dgvEmp.Columns[3].HeaderText = "生日";
            this.dgvEmp.Columns[4].HeaderText = "入职日期";
            this.dgvEmp.Columns[5].HeaderText = "住址";
            this.dgvEmp.Columns[6].HeaderText = "联系方式";
            this.dgvEmp.Columns[7].HeaderText = "薪资";
            this.dgvEmp.Columns[8].HeaderText = "备注";




        }
        void showXz()
        {
            DataGridViewCheckBoxColumn acCode = new DataGridViewCheckBoxColumn();
            acCode.Name = "acCode";
            acCode.HeaderText = "选择";
            dgvEmp.Columns.Add(acCode);
        }

        private void delete_emp_Load(object sender, EventArgs e)
        {
            init();
            showAll();
            dgvHead();
            showXz();   
        }

        private void dgvEmp_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvEmp.Rows.Count > 0)
            {
                for (int i = 0; i < dgvEmp.Rows.Count; i++)
                {
                    DataGridViewCheckBoxCell ck = dgvEmp.Rows[i].Cells["acCode"] as DataGridViewCheckBoxCell;
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
        void UpdateDB()
        {
            try
            {
                SqlCommandBuilder dbvolunteer = new SqlCommandBuilder(daAdmin);
                daAdmin.Update(ds, "Admin_info");
                
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void btnok_Click(object sender, EventArgs e)
        {
            int s = dgvEmp.Rows.Count;
            for (int i = 0; i < dgvEmp.Rows.Count; i++)
            {
                if (dgvEmp.Rows[i].Cells["acCode"].EditedFormattedValue.ToString() == "True")
                {
                    DialogResult dr = MessageBox.Show("是否要删除？", "提示",
                        MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                    if (dr == DialogResult.OK)
                    {
                        DB.Getcn();
                        //寻找目标ID
                        string str = "select * from  adminT";
                        DataTable dt = DB.GetDataSet(str);
                        ShanchuID = dt.Rows[i][0].ToString();

                        dgvEmp.Rows.RemoveAt(i);
                        //添加日志
                        SqlCommand cmd = new SqlCommand("add_log", DB.cn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("username", SqlDbType.NVarChar));
                        cmd.Parameters.Add(new SqlParameter("log_type", SqlDbType.NVarChar));
                        cmd.Parameters.Add(new SqlParameter("action_date", SqlDbType.DateTime));
                        cmd.Parameters.Add(new SqlParameter("action_table", SqlDbType.NVarChar));
                        cmd.Parameters["username"].Value = Login.username;
                        cmd.Parameters["log_type"].Value = "删除";
                        cmd.Parameters["action_date"].Value = DateTime.Now;
                        cmd.Parameters["action_table"].Value = "AdminT表";
                        try
                        {
                            SqlCommandBuilder dbAdmin = new SqlCommandBuilder(daAdmin);
                            daAdmin.Update(ds, "Admin_info");
                            cmd.ExecuteNonQuery();
                        }
                        catch (SqlException ex)
                        {
                            MessageBox.Show(ex.Message);
                        }



                        MessageBox.Show("删除成功");
                        UpdateDB();
                    }
                    else
                    {
                        return;
                    }
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
            DB.cn.Close();
        }

        private void btnno_Click(object sender, EventArgs e)
        {
            this.Close();   
        }
    }
}
