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
using System.IO;


namespace 大学生志愿者管理系统1.Tactivity
{
    public partial class Insert_activity : Form
    {
        public Insert_activity()
        {
            InitializeComponent();
        }
        public static string path_source="";
        SqlDataAdapter daActivity, dalog;
        DataSet ds = new DataSet();
        void init() //初始化
        {
            DB.Getcn();
            string str = "select * from ActivityT where Holder='" + Login.username + "'";
            string sdr = "select * from dalogT";
            daActivity = new SqlDataAdapter(str, DB.cn);
            dalog = new SqlDataAdapter(sdr, DB.cn);
            daActivity.Fill(ds, "activity_info");
            dalog.Fill(ds, "log_info");
        }
        void showAll()
        {
            DataView dvActivity = new DataView(ds.Tables["activity_info"]);
            dgvactivity.DataSource = dvActivity;
        }

        private void Insert_activity_Load(object sender, EventArgs e)
        {
            // TODO: 这行代码将数据加载到表“vOLUNTEERDataSet.activityTypeT”中。您可以根据需要移动或删除它。
            this.activityTypeTTableAdapter.Fill(this.vOLUNTEERDataSet.activityTypeT);
            init();
            showAll();
            string str = "select * from ActivityT";
            DataTable dt = DB.GetDataSet(str);
            int s = dt.Rows.Count-1;
            string NID= dt.Rows[s][0].ToString();
            int A = Convert.ToInt32(NID)+1;
            txtActID.Text = A.ToString();

        }

        private void btncancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (txtActID.Text == "" || txtActName.Text == "" || txtActNeed.Text == ""||txtplace.Text=="")
            {
                MessageBox.Show("必选项不能为空");
            }
            else
            {
                DB.Getcn();
                string str = "select * from ActivityT where activity_ID='" + txtActID.Text + "'";
                DataTable dt = DB.GetDataSet(str);

                if (dt.Rows.Count > 0)
                {
                    MessageBox.Show("编号已存在");
                }
                else
                {
                    string filename;
                    string fileFolder;
                    string dateTime = "";
                    //照片时间标记
                    filename = Path.GetFileName(path_source);
                    dateTime += DateTime.Now.Year.ToString();
                    dateTime += DateTime.Now.Month.ToString();
                    dateTime += DateTime.Now.Day.ToString();
                    dateTime += DateTime.Now.Hour.ToString();
                    dateTime += DateTime.Now.Minute.ToString();
                    dateTime += DateTime.Now.Second.ToString();
                    filename = dateTime + filename;
                    fileFolder = Directory.GetCurrentDirectory() + "\\" + "Act_Images" + "\\"
                        + cboCategory.Text + "\\";
                    fileFolder += filename;

                    DataRow drAct = ds.Tables["activity_info"].NewRow();
                    drAct["activity_ID"] = Convert.ToInt32(txtActID.Text);
                    drAct["activity_Name"] = txtActName.Text;
                    drAct["activity_type"] = cboCategory.SelectedValue;
                    drAct["addtime"] = dtpBegin.Value;
                    drAct["stoptime"] = dtpStop.Value;
                    drAct["place"] = txtplace.Text;
                    drAct["renshu"] = int.Parse(txtActNeed.Text);
                    drAct["descn"] = txtActResume.Text;
                    drAct["Holder"] = Login.username;
                    drAct["status"] = "进行中";
                    if(path_source!="")
                    {
                        File.Copy(path_source, fileFolder, true);
                        drAct["Image"] = "\\Act_Images\\" + cboCategory.Text + "\\" + filename;
                    }
                    else
                    {
                        drAct["Image"] = "暂无图片.gif";
                    }

                    ds.Tables["activity_info"].Rows.Add(drAct);

                    DataRow drlog = ds.Tables["log_info"].NewRow();
                    drlog["username"] = Login.username;
                    drlog["log_type"] = "添加";
                    drlog["action_date"] = DateTime.Now;
                    drlog["action_table"] = "activity表";
                    ds.Tables["log_info"].Rows.Add(drlog);
                   
                    try
                    {
                        SqlCommandBuilder dbActivity = new SqlCommandBuilder(daActivity);
                        daActivity.Update(ds, "activity_info");
                        SqlCommandBuilder dblog = new SqlCommandBuilder(dalog);
                        dalog.Update(ds, "log_info");
                        MessageBox.Show("添加成功");
                    }
                    catch(SqlException ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    DB.cn.Close();
                }
            }
        }

        private void btnpid_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if(ofd.ShowDialog()==DialogResult.OK)
            {
                path_source = ofd.FileName;
                pictureBox1.Image = Image.FromFile(path_source);
                pictureBox1.SizeMode = PictureBoxSizeMode.AutoSize;
            }
        }

        private void label9_Click(object sender, EventArgs e)
        {

        }
    }
}
