using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;


namespace 大学生志愿者管理系统1.ActStop
{
    public partial class ActStop : Form
    {
        public ActStop()
        {
            InitializeComponent();
        }
        public int lastrowindex;
        SqlDataAdapter daActivity, daVolunteer;
        DataSet ds = new DataSet();
        void showALLActiveites()
        {
            DB.Getcn();
            string str = "select activity_id,activity_Name,activity_type,addtime,stoptime,status from ActivityT where Holder='"
                +Login.username+"'";
            daActivity = new SqlDataAdapter(str, DB.cn);
            daActivity.Fill(ds, "Act12_info");


            SqlCommand cmd = new SqlCommand(str, DB.cn);
            SqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                int index = dgvactivity.Rows.Add();
                dgvactivity.Rows[index].Cells[0].Value = rdr[0];
                dgvactivity.Rows[index].Cells[1].Value = rdr[1];
                dgvactivity.Rows[index].Cells[2].Value = rdr[2];
                dgvactivity.Rows[index].Cells[3].Value = rdr[3];
                dgvactivity.Rows[index].Cells[4].Value = rdr[4];
                dgvactivity.Rows[index].Cells[5].Value = rdr[5];

            }
            rdr.Close();
            DB.cn.Close();
        }
        

        private void ActStop_Load(object sender, EventArgs e)
        {
           
            showALLActiveites();
            

        }

        private void btnAddtime_Click(object sender, EventArgs e)
        {
            
            
                int s = dgvShowVol.Rows.Count;
                for (int i = 0; i <= dgvShowVol.Rows.Count; i++)
                {
                    var cell = dgvShowVol.Rows[i].Cells[3];

                    if (cell == null || cell.Value == null)
                    {
                        MessageBox.Show($"添加成功");
                        break;
                    }
                
                string volid = cell.Value.ToString();
                    string TIME = dgvShowVol.Rows[i].Cells[6].Value.ToString();
                    DB.Getcn();
                    string srr = "select Aid,Aname,Act_time from volunteerT where Aid='" + volid + "'";
                    DataTable dt = DB.GetDataSet(srr);
                    string LastVolTime = dt.Rows[0][2].ToString();

                    // 将 LastVolTime 和 txtVolTime.Text 转换为整数
                    int lastVolTimeInt = int.TryParse(LastVolTime, out lastVolTimeInt) ? lastVolTimeInt : 0;
                    int newVolTimeInt = int.TryParse(TIME, out newVolTimeInt) ? newVolTimeInt : 0;

                    // 计算总时长
                    int holeTime = lastVolTimeInt + newVolTimeInt;

                    // 将结果转换为字符串
                    string holeTimeStr = holeTime.ToString();




                    string sdr = "update VolunteerT set Act_Time='"
                                    + holeTimeStr + "' where Aid='" + volid + "'";
                    DB.sqlEx(sdr);
                MessageBox.Show($"添加成功");

            }

                txtVolTime.Text = "";
                DB.cn.Close();
                dgvShowVol.DataSource = null;
                dgvShowVol.Rows.Clear();


            }
        

        private void dgvactivity_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int Cindex = e.ColumnIndex;
            int rowindex = e.RowIndex;
            string check = dgvactivity.Rows[rowindex].Cells[5].Value.ToString();
            if (Cindex == 6)
            {
                if (check == "已结束")
                {
                    MessageBox.Show("活动已结束");
                }
                else
                {
                   
                    string Actid1 = dgvactivity.Rows[rowindex].Cells[0].Value.ToString();

                    DialogResult dr = MessageBox.Show("是否结束？", "提示",
                       MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                    if (dr == DialogResult.OK)
                    {
                        
                       dgvactivity.Rows[rowindex].Cells[5].Value = "已结束";
                        DB.Getcn();
                        using (SqlCommand cmdStatus = new SqlCommand("update ActivityT set status = @Status where activity_id = @Actid", DB.cn))
                        {
                            cmdStatus.Parameters.AddWithValue("@Status", "已结束");
                           cmdStatus.Parameters.AddWithValue("@Actid", Actid1);
                           cmdStatus.ExecuteNonQuery();
                      }

                        DB.cn.Close();



                        DB.Getcn();
                        String sqr = "select * from ACTMember where ACTID ='"+Actid1+"'";
                        DataTable dt = DB.GetDataSet(sqr);
                        if (dt.Rows.Count == 0)
                        {
                            MessageBox.Show("该活动人数为0");
                        }
                        else
                        {
                            daVolunteer = new SqlDataAdapter(sqr, DB.cn);
                            daVolunteer.Fill(ds, "Vol12_info");

                            // 填充新的数据
                            DataView dvVolunteer = new DataView(ds.Tables["Vol12_info"]);
                            dgvShowVol.DataSource = dvVolunteer;






                            // 添加调试输出
                            foreach (DataRow row in ds.Tables["Vol12_info"].Rows)
                            {
                                Console.WriteLine(string.Join(", ", row.ItemArray));
                            }



                            // 添加调试输出
                            for (int i = 0; i < dgvShowVol.Rows.Count; i++)
                            {
                                for (int j = 0; j < dgvShowVol.Columns.Count; j++)
                                {
                                    var cellValue = dgvShowVol.Rows[i].Cells[j].Value;
                                    Console.WriteLine($"Row {i} Cell[{j}] Value: {cellValue}");
                                }
                            }
                        }
                    }
                    DB.cn.Close();
                }
            }
        }
        
    

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
