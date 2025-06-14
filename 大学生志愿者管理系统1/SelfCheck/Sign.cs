using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 大学生志愿者管理系统1.SelfCheck
{
    public partial class Sign : Form
    {
        public Sign()
        {
            InitializeComponent();
        }

        private void Sign_Load(object sender, EventArgs e)
        {
            DB.Getcn();
            string str = "select * from activityT where activity_ID='" + SelfInfoCheck1.ACTID2 + "'";
            DataTable dt = DB.GetDataSet(str);

            lb10ID.Text = SelfInfoCheck1.ACTID2;
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

        private void btnSign_Click(object sender, EventArgs e)
        {
            // 检查是否已签到
            string checkQuery = "SELECT SignTime FROM ACTMember WHERE ActID = @ActID AND VolunteerID = @VolunteerID";
            DataTable dt = new DataTable();

            try
            {

                DB.Getcn();
                using (SqlCommand checkCommand = new SqlCommand(checkQuery, DB.cn))
                {
                    checkCommand.Parameters.AddWithValue("@ActID", SelfInfoCheck1.ACTID2);
                    checkCommand.Parameters.AddWithValue("@VolunteerID", Login.Vol_ID);

                    SqlDataAdapter adapter = new SqlDataAdapter(checkCommand);
                    adapter.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("数据库操作失败：" + ex.Message);
            }

            // 检查签到时间是否为空
            if (dt.Rows.Count > 0 && dt.Rows[0]["SignTime"] != DBNull.Value)
            {
                MessageBox.Show("您已签到");
            }
            else
            {
                DateTime signInTime = DateTime.Now;
                dtpsign.Value = signInTime;

                try
                {
                    DB.Getcn();

                    // 更新签到时间
                    string query = "UPDATE ACTMember SET SignTime = @SignTime WHERE ActID = @ActID AND VolunteerID = @VolunteerID";
                    using (SqlCommand command = new SqlCommand(query, DB.cn))
                    {
                        command.Parameters.AddWithValue("@SignTime", signInTime);
                        command.Parameters.AddWithValue("@ActID", SelfInfoCheck1.ACTID2);
                        command.Parameters.AddWithValue("@VolunteerID", Login.Vol_ID);

                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("签到成功！");
                        }
                        else
                        {
                            MessageBox.Show("未找到匹配的记录，更新失败！");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("数据库操作失败：" + ex.Message);
                }
            }


        }




        private void btnturn_Click(object sender, EventArgs e)
        {
            // 检查是否已签到
            string checkQuery = "SELECT SignTime FROM ACTMember WHERE ActID = @ActID AND VolunteerID = @VolunteerID";
            DataTable dt = new DataTable();

            try
            {

                DB.Getcn();
                using (SqlCommand checkCommand = new SqlCommand(checkQuery, DB.cn))
                {
                    checkCommand.Parameters.AddWithValue("@ActID", SelfInfoCheck1.ACTID2);
                    checkCommand.Parameters.AddWithValue("@VolunteerID", Login.Vol_ID);

                    SqlDataAdapter adapter = new SqlDataAdapter(checkCommand);
                    adapter.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("数据库操作失败：" + ex.Message);
            }

            // 检查签到时间是否为空
            if (dt.Rows.Count == 0 && dt.Rows[0]["SignTime"] != DBNull.Value)
            {
                MessageBox.Show("您未签到");
            }
            else
            {
                DateTime signInTime = DateTime.Now;
                dtpturn.Value = signInTime;

                try
                {
                    DB.Getcn();


                    string query = "UPDATE ACTMember SET ReturnTime = @ReturnTime WHERE ActID = @ActID AND VolunteerID = @VolunteerID";
                    using (SqlCommand command = new SqlCommand(query, DB.cn))
                    {
                        command.Parameters.AddWithValue("@ReturnTime", signInTime);
                        command.Parameters.AddWithValue("@ActID", SelfInfoCheck1.ACTID2);
                        command.Parameters.AddWithValue("@VolunteerID", Login.Vol_ID);

                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("签退成功！");
                        }
                        else
                        {
                            MessageBox.Show("未找到匹配的记录，更新失败！");
                        }
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show("数据库操作失败：" + ex.Message);
                }
                TimeSpan timeOfDay1 = dtpsign.Value.TimeOfDay;
                TimeSpan timeOfDay2 = dtpturn.Value.TimeOfDay;

                // 计算时间差
                TimeSpan duration = timeOfDay2 - timeOfDay1;
                int hours = (int)duration.TotalHours;
                int minutes = (int)(duration.TotalMinutes % 60);

                // 格式化为字符串
                string formattedDuration = $"{hours}:{minutes:D2}";

                DB.Getcn();
                using (SqlCommand cmdStatus = new SqlCommand("update ACTMember set Time = @Time where ACTID = @Actid1 AND Volunteerid =@VolID", DB.cn))
                {
                    cmdStatus.Parameters.AddWithValue("@Time", formattedDuration);
                    cmdStatus.Parameters.AddWithValue("@Actid1", SelfInfoCheck1.ACTID2);
                    cmdStatus.Parameters.AddWithValue("@VolID", Login.Vol_ID);
                    cmdStatus.ExecuteNonQuery();
                }

                DB.cn.Close();
            }






        }
    }
}
