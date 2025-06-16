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

namespace 大学生志愿者管理系统1.Order
{
    public partial class OrderTables : Form
    {
        private OrderService _orderService; // 添加OrderService引用

        public OrderTables()
        {
            InitializeComponent();
            _orderService = new OrderService(); // 初始化OrderService

            // 检查登录状态
            if (string.IsNullOrEmpty(Login.username) || string.IsNullOrEmpty(Login.Emp_ID))
            {
                MessageBox.Show("请先登录系统");
                this.Close();
            }
        }

        private DataTable dtOrders = new DataTable(); // 订单数据表
        private List<OrderT> orderList; // 保存订单列表

        public static string VolID = "";
        public static string VolName = "";
        public static string VolPhone = "";
        public static string ActID = "";
        public static string ActName = "";

        void LoadOrders()
        {
            try
            {
                // 使用业务逻辑层获取当前用户的所有订单
                orderList = _orderService.GetOrdersByHolder(Login.username);

                // 清空现有数据表并创建列
                dtOrders.Clear();
                if (dtOrders.Columns.Count == 0)
                {
                    dtOrders.Columns.Add("OrderID", typeof(int));
                    dtOrders.Columns.Add("UserID", typeof(int));
                    dtOrders.Columns.Add("UserName", typeof(string));
                    dtOrders.Columns.Add("Orderdate", typeof(DateTime));
                    dtOrders.Columns.Add("phone", typeof(string));
                    dtOrders.Columns.Add("ActID", typeof(int));
                    dtOrders.Columns.Add("Act_Name", typeof(string));
                    dtOrders.Columns.Add("Holder", typeof(string));
                    dtOrders.Columns.Add("status", typeof(string));
                    dtOrders.Columns.Add("EmpID", typeof(int));
                }

                // 填充数据
                foreach (var order in orderList)
                {
                    DataRow row = dtOrders.NewRow();
                    row["OrderID"] = order.OrderID;

                    // 确保UserID不为null - 主要修复点
                    if (order.UserID.HasValue)
                    {
                        row["UserID"] = order.UserID.Value;
                    }
                    else
                    {
                        // 设置为默认值而不是null
                        row["UserID"] = -1; // 或任何合适的默认值
                    }

                    row["UserName"] = order.UserName ?? "";
                    row["Orderdate"] = order.OrderDate ?? DateTime.Now;
                    row["phone"] = order.phone ?? "";

                    // 确保ActID不为null
                    if (order.ActID.HasValue)
                    {
                        row["ActID"] = order.ActID.Value;
                    }
                    else
                    {
                        row["ActID"] = -1; // 或任何合适的默认值
                    }

                    row["Act_Name"] = order.Act_Name ?? "";
                    row["Holder"] = order.Holder ?? "";
                    row["status"] = order.status ?? "";

                    // 正确处理EmpID
                    if (order.EmpID.HasValue)
                    {
                        row["EmpID"] = order.EmpID.Value;
                    }
                    else
                    {
                        row["EmpID"] = DBNull.Value; // 使用DBNull.Value而不是null
                    }

                    dtOrders.Rows.Add(row);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载订单数据出错: {ex.Message}\n\n{ex.StackTrace}"); // 添加更详细的错误信息
            }
        }

        void showAll()
        {
            try
            {
                LoadOrders();
                dgvOrder.DataSource = dtOrders;
                showXZ();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"显示数据时出错: {ex.Message}");
            }
        }

        void showXZ()
        {
            try
            {
                // 检查是否已经添加了选择列
                if (!dgvOrder.Columns.Contains("asCode"))
                {
                    DataGridViewCheckBoxColumn asCode = new DataGridViewCheckBoxColumn();
                    asCode.Name = "asCode";
                    asCode.HeaderText = "选择";
                    dgvOrder.Columns.Add(asCode);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"添加选择列时出错: {ex.Message}");
            }
        }

        private void OrderTables_Load(object sender, EventArgs e)
        {
            try
            {
                showAll();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"窗体加载时出错: {ex.Message}");
            }
        }

        private void dgvOrder_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (dgvOrder.Columns[e.ColumnIndex].Name == "asCode" && e.RowIndex >= 0)
                {
                    if (dgvOrder.Rows.Count > 0)
                    {
                        for (int i = 0; i < dgvOrder.Rows.Count; i++)
                        {
                            DataGridViewCheckBoxCell ck = dgvOrder.Rows[i].Cells["asCode"] as DataGridViewCheckBoxCell;
                            if (ck != null)
                            {
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
            }
            catch (Exception ex)
            {
                MessageBox.Show($"选择行时出错: {ex.Message}");
            }
        }

        private void btncheck_Click(object sender, EventArgs e)
        {
            bool selected = false;
            int selectedIndex = -1;

            try
            {
                for (int i = 0; i < dgvOrder.Rows.Count; i++)
                {
                    if (dgvOrder.Rows[i].Cells["asCode"]?.EditedFormattedValue?.ToString() == "True")
                    {
                        selected = true;
                        selectedIndex = i;
                        break;
                    }
                }

                if (selected && selectedIndex >= 0)
                {
                    // 检查是否已审核
                    string status = dgvOrder.Rows[selectedIndex].Cells["status"].Value?.ToString() ?? "";
                    if (status == "已审核")
                    {
                        MessageBox.Show("该订单已审核");
                        return;
                    }

                    // 获取订单ID
                    if (dgvOrder.Rows[selectedIndex].Cells["OrderID"].Value == null ||
                        !int.TryParse(dgvOrder.Rows[selectedIndex].Cells["OrderID"].Value.ToString(), out int orderId))
                    {
                        MessageBox.Show("无法获取订单ID");
                        return;
                    }

                    // 确保管理员ID有效
                    if (string.IsNullOrEmpty(Login.Emp_ID))
                    {
                        MessageBox.Show("管理员ID无效，请重新登录");
                        return;
                    }

                    if (!int.TryParse(Login.Emp_ID, out int empId))
                    {
                        MessageBox.Show("管理员ID格式错误");
                        return;
                    }

                    // 保存订单信息用于显示
                    VolID = dgvOrder.Rows[selectedIndex].Cells["UserID"].Value?.ToString() ?? "-1";
                    VolName = dgvOrder.Rows[selectedIndex].Cells["UserName"].Value?.ToString() ?? "";
                    VolPhone = dgvOrder.Rows[selectedIndex].Cells["phone"].Value?.ToString() ?? "";
                    ActID = dgvOrder.Rows[selectedIndex].Cells["ActID"].Value?.ToString() ?? "-1";
                    ActName = dgvOrder.Rows[selectedIndex].Cells["Act_Name"].Value?.ToString() ?? "";

                    // 使用业务逻辑层审核订单
                    bool result = _orderService.ApproveOrder(orderId, empId);

                    if (result)
                    {
                        // 更新UI
                        dgvOrder.Rows[selectedIndex].Cells["status"].Value = "已审核";
                        dgvOrder.Rows[selectedIndex].Cells["EmpID"].Value = empId;
                        MessageBox.Show("审核成功");

                        // 重新加载数据
                        showAll();
                    }
                    else
                    {
                        MessageBox.Show("审核失败");
                    }
                }
                else
                {
                    MessageBox.Show("请选择审核项");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"审核过程中出错: {ex.Message}\n\n{ex.StackTrace}");
            }
        }

        private void btncancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}