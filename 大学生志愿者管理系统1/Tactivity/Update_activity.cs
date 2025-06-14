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
    public partial class Update_activity : Form
    {
        public Update_activity()
        {
            InitializeComponent();
        }
        SqlDataAdapter daActivity, dalog;
        DataSet ds = new DataSet();
        public static string path_source = ""; //保存图片路径
        void init()
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

        private void btnCancle_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dgvactivity_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            txtId.Text = dgvactivity.CurrentRow.Cells["activity_ID"].Value.ToString();
            cboCategory.Text = dgvactivity.CurrentRow.Cells["activity_type"].Value.ToString();
            txtName.Text = dgvactivity.CurrentRow.Cells["activity_Name"].Value.ToString();
            txtPlace.Text = dgvactivity.CurrentRow.Cells["place"].Value.ToString();
            txtResume.Text = dgvactivity.CurrentRow.Cells["descn"].Value.ToString();
            txtNeed.Text = dgvactivity.CurrentRow.Cells["renshu"].Value.ToString();
            dtpBegin.Text = dgvactivity.CurrentRow.Cells["addtime"].Value.ToString();
            dtpStop.Text = dgvactivity.CurrentRow.Cells["stoptime"].Value.ToString();
            txtHolder.Text = dgvactivity.CurrentRow.Cells["Holder"].Value.ToString();
            try
            {
                pic_act.Image = Image.FromFile(Application.StartupPath
                    + dgvactivity.CurrentRow.Cells["Image"].Value.ToString());
                pic_act.SizeMode = PictureBoxSizeMode.StretchImage;
            }
            catch
            {
                pic_act.Image = Image.FromFile(Application.StartupPath + "\\" + "暂无图片.gif");
            }

        }

        private void btnPic_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if(ofd.ShowDialog()==DialogResult.OK)
            {
                path_source = ofd.FileName;
                pic_act.Image = Image.FromFile(path_source);
                pic_act.SizeMode = PictureBoxSizeMode.StretchImage;

            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if(txtName.Text==""||txtNeed.Text==""||txtPlace.Text=="")
            {
                MessageBox.Show("活动名称，活动人数，活动地点不能为空，请重新输入");
            }
            else
            {
                DialogResult dr = MessageBox.Show("您确定要修改吗？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);//弹窗固定格式
                if (dr == DialogResult.OK)
                {
                    string filename;
                    string fileFolder;
                    string dateTime = "";
                    filename = Path.GetFileName(path_source);
                    dateTime = DateTime.Now.Year.ToString();
                    dateTime = DateTime.Now.Month.ToString();
                    dateTime = DateTime.Now.Day.ToString();
                    dateTime = DateTime.Now.Hour.ToString();
                    dateTime = DateTime.Now.Minute.ToString();
                    dateTime = DateTime.Now.Second.ToString();
                    filename = dateTime + filename;
                    fileFolder = Directory.GetCurrentDirectory() + "\\" + "Act_Images" + "\\"
                       + cboCategory.Text + "\\";
                    fileFolder += filename;


                    int a = dgvactivity.CurrentRow.Index;
                    string str = dgvactivity.Rows[a].Cells["activity_ID"].Value.ToString();
                    DataRow[] ActRows = ds.Tables["activity_info"].Select("activity_ID='"
                        + str + "'");
                    ActRows[0]["activity_type"] = cboCategory.SelectedValue;
                    ActRows[0]["activity_Name"] = txtName.Text.Trim();
                    ActRows[0]["renshu"] = int.Parse(txtNeed.Text.Trim());
                    ActRows[0]["descn"] = txtResume.Text.Trim();
                    ActRows[0]["addtime"] = dtpBegin.Value;
                    ActRows[0]["stoptime"] = dtpStop.Value;
                    ActRows[0]["place"] = txtPlace.Text.Trim();
                    if(path_source!="")
                    {
                        File.Copy(path_source, fileFolder, true);
                        ActRows[0]["Image"] = "\\Act_Images\\" + cboCategory.Text + "\\"
                            + filename;
                    }
                    DataRow drlog = ds.Tables["log_info"].NewRow();
                    drlog["username"] = Login.username;
                    drlog["log_type"] = "修改";
                    drlog["action_date"] = DateTime.Now;
                    drlog["action_table"] = "activity表";
                    ds.Tables["log_info"].Rows.Add(drlog);
                    try
                    {
                        SqlCommandBuilder dbActivity = new SqlCommandBuilder(daActivity);
                        daActivity.Update(ds, "activity_info");
                        SqlCommandBuilder dblog = new SqlCommandBuilder(dalog);
                        dalog.Update(ds, "log_info");
                        MessageBox.Show("修改成功");
                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    DB.cn.Close();

                }
            }
        }

        private void Update_activity_Load(object sender, EventArgs e)
        {
            // TODO: 这行代码将数据加载到表“vOLUNTEERDataSet1.activityTypeT”中。您可以根据需要移动或删除它。
            this.activityTypeTTableAdapter.Fill(this.vOLUNTEERDataSet1.activityTypeT);
            init();
            showAll();
        }
    }
}
