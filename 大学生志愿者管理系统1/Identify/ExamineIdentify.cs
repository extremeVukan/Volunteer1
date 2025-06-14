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

namespace 大学生志愿者管理系统1.Identify
{
    public partial class ExamineIdentify : Form
    {
        public ExamineIdentify()
        {
            InitializeComponent();
        }
        SqlDataAdapter daIden;
        DataSet ds = new DataSet();

        void init()
        {
            DB.Getcn();
            string str = "select * from VolIdentifyT ";
            daIden = new SqlDataAdapter(str, DB.cn);
            daIden.Fill(ds, "Iden_info");
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
            dgvIdentify.Columns.Add(asCode);
        }

        private void ExamineIdentify_Load(object sender, EventArgs e)
        {
            showAll();
            DataView dvIden = new DataView(ds.Tables["Iden_info"]);
            dgvIdentify.DataSource = dvIden;
        }

        private void dgvIdentify_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvIdentify.Rows.Count > 0)
            {
                for (int i = 0; i < dgvIdentify.Rows.Count; i++)
                {
                    DataGridViewCheckBoxCell ck = dgvIdentify.Rows[i].Cells["asCode"] as DataGridViewCheckBoxCell;
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

        private void btnOK_Click(object sender, EventArgs e)
        {
            int s = dgvIdentify.Rows.Count;
            for (int i = 0; i < dgvIdentify.Rows.Count; i++)
            {
                if (dgvIdentify.Rows[i].Cells["asCode"].EditedFormattedValue.ToString() == "True")
                {
                    dgvIdentify.Rows[i].Cells["Status"].Value = "已通过";
                    if (string.IsNullOrEmpty(dgvIdentify.Rows[i].Cells["EmpID"].Value?.ToString()))
                    {
                        dgvIdentify.Rows[i].Cells["EMPID"].Value = Login.Emp_ID;
                        MessageBox.Show("审核成功");





                    }
                    UpdateDB();

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
        }
          
        void UpdateDB()
        {
            try
            {
                SqlCommandBuilder dbIden = new SqlCommandBuilder(daIden);
                daIden.Update(ds, "Iden_info");
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void btncancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
