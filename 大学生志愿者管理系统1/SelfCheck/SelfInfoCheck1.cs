using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BLL; // 添加BLL引用
using DAL; // 添加DAL引用用于实体类

namespace 大学生志愿者管理系统1.SelfCheck
{
    public partial class SelfInfoCheck1 : Form
    {
        private OrderService _orderService; // 添加OrderService引用
        private IdentifyService _identifyService; // 添加IdentifyService引用
        private VolunteerService _volunteerService; // 添加VolunteerService引用

        public SelfInfoCheck1()
        {
            InitializeComponent();
            _orderService = new OrderService(); // 初始化OrderService
            _identifyService = new IdentifyService(); // 初始化IdentifyService
            _volunteerService = new VolunteerService(); // 初始化VolunteerService
        }

        private DataTable dtOrders = new DataTable(); // 订单数据表
        public static string ACTID2 = "";

        void LoadOrders()
        {
            try
            {
                // 使用业务逻辑层获取当前用户的所有订单
                var orders = _orderService.GetOrdersByUserName(Login.username);

                // 清空现有数据表并创建列
                dtOrders.Clear();
                if (dtOrders.Columns.Count == 0)
                {
                    dtOrders.Columns.Add("UserID", typeof(int));
                    dtOrders.Columns.Add("UserName", typeof(string));
                    dtOrders.Columns.Add("Orderdate", typeof(DateTime));
                    dtOrders.Columns.Add("ActID", typeof(int));
                    dtOrders.Columns.Add("Act_Name", typeof(string));
                }

                // 填充数据
                foreach (var order in orders)
                {
                    DataRow row = dtOrders.NewRow();
                    row["UserID"] = order.UserID ?? 0;
                    row["UserName"] = order.UserName ?? "";
                    row["Orderdate"] = order.OrderDate ?? DateTime.Now;
                    row["ActID"] = order.ActID ?? 0;
                    row["Act_Name"] = order.Act_Name ?? "";
                    dtOrders.Rows.Add(row);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载订单数据出错: {ex.Message}");
            }
        }

        void showAll() // DGV载入
        {
            dgvShowAtten.DataSource = dtOrders;
        }

        void dgvHead()
        {
            this.dgvShowAtten.Columns[0].HeaderText = "ID";
            this.dgvShowAtten.Columns[1].HeaderText = "姓名";
            this.dgvShowAtten.Columns[2].HeaderText = "日期";
            this.dgvShowAtten.Columns[3].HeaderText = "活动编号";
            this.dgvShowAtten.Columns[4].HeaderText = "活动";
        }

        void showXZ()
        {
            // 检查是否已经添加了选择列
            if (!dgvShowAtten.Columns.Contains("asCode"))
            {
                DataGridViewCheckBoxColumn asCode = new DataGridViewCheckBoxColumn();
                asCode.Name = "asCode";
                asCode.HeaderText = "选择";
                dgvShowAtten.Columns.Add(asCode);
            }
        }

        private void SelfInfoCheck1_Load(object sender, EventArgs e)
        {
            try
            {
                // 获取证件状态
                string status = _identifyService.GetVolunteerIdentifyStatus(Login.username);
                txtStatus.Text = status;

                // 设置用户名
                txtName.Text = Login.username;

                // 获取志愿时长
                int volunteerId = Convert.ToInt32(Login.Vol_ID);
                int hours = _volunteerService.GetVolunteerHours(volunteerId);
                txtTime.Text = hours.ToString() + "小时";

                // 加载订单数据
                LoadOrders();
                showAll();
                dgvHead();
                showXZ();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载页面数据时出错: {ex.Message}");
            }
        }

        private void dgvShowAtten_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvShowAtten.Columns[e.ColumnIndex].Name == "asCode" && e.RowIndex >= 0)
            {
                if (dgvShowAtten.Rows.Count > 0)
                {
                    for (int i = 0; i < dgvShowAtten.Rows.Count; i++)
                    {
                        DataGridViewCheckBoxCell ck = dgvShowAtten.Rows[i].Cells["asCode"] as DataGridViewCheckBoxCell;
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
        }

        private void btncheck_Click(object sender, EventArgs e)
        {
            bool selected = false;
            int selectedIndex = -1;

            for (int i = 0; i < dgvShowAtten.Rows.Count; i++)
            {
                if (dgvShowAtten.Rows[i].Cells["asCode"]?.EditedFormattedValue?.ToString() == "True")
                {
                    selected = true;
                    selectedIndex = i;
                    break;
                }
            }

            if (selected && selectedIndex >= 0)
            {
                if (dgvShowAtten.Rows[selectedIndex].Cells["ActID"].Value != null)
                {
                    ACTID2 = dgvShowAtten.Rows[selectedIndex].Cells["ActID"].Value.ToString();
                    SelfCheck.Sign signForm = new SelfCheck.Sign();
                    signForm.ShowDialog();
                }
            }
            else
            {
                MessageBox.Show("请选择一个活动");
            }
        }

        private void btnSWEEP_Click(object sender, EventArgs e)
        {
            // 重新加载数据
            LoadOrders();
            showAll();
            dgvHead();
        }
    }
}