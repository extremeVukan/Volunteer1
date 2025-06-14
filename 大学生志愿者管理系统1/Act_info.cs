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
    public partial class Act_info : Form
    {
        public Act_info()
        {
            InitializeComponent();
        }

        private void Act_info_Load(object sender, EventArgs e)
        {
            DB.Getcn();
            string str = "select * from activityT where activity_ID=" + Form1.Actid + "";
            DataTable dt = DB.GetDataSet(str);

            lb10ID.Text = Form1.Actid;
            lb11Name.Text = dt.Rows[0][1].ToString();
            lb12Type.Text = dt.Rows[0][2].ToString();
            lb13AddTime.Text = dt.Rows[0][3].ToString();
            lb14StopTime.Text = dt.Rows[0][4].ToString();
            lb15Place.Text = dt.Rows[0][5].ToString();
            lb16Descn.Text = dt.Rows[0][8].ToString();
            lb17Need.Text = dt.Rows[0][6].ToString();
            try
            {
                ptbPicture.Image = Image.FromFile(Application.StartupPath + dt.Rows[0][7].ToString());
                ptbPicture.SizeMode = PictureBoxSizeMode.StretchImage;
            }
            catch
            {
                ptbPicture.Image = Image.FromFile(Application.StartupPath + "\\" + "暂无图片.gif");
                ptbPicture.SizeMode = PictureBoxSizeMode.StretchImage;
            }
            
        }

        private void btncancle_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
