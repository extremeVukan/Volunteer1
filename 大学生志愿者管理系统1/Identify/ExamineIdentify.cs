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

namespace 大学生志愿者管理系统1.Identify
{
    public partial class ExamineIdentify : Form
    {
        private IdentifyService _identifyService; // 添加IdentifyService引用

        public ExamineIdentify()
        {
            InitializeComponent();
            _identifyService = new IdentifyService(); // 初始化IdentifyService
        }

        private DataTable dtIdentify = new DataTable(); // 证件申请数据表
        private List<VolIdentifyT> identifyList; // 保存证件申请列表

        void LoadIdentifies()
        {
            try
            {
                // 使用业务逻辑层获取所有证件申请
                identifyList = _identifyService.GetAllIdentifies();

                // 清空现有数据表并创建列
                dtIdentify.Clear();
                if (dtIdentify.Columns.Count == 0)
                {
                    dtIdentify.Columns.Add("Num", typeof(int));
                    dtIdentify.Columns.Add("VID", typeof(string));
                    dtIdentify.Columns.Add("VName", typeof(string));
                    dtIdentify.Columns.Add("Phone", typeof(string));
                    dtIdentify.Columns.Add("Province", typeof(string));
                    dtIdentify.Columns.Add("City", typeof(string));
                    dtIdentify.Columns.Add("Address", typeof(string));
                    dtIdentify.Columns.Add("Status", typeof(string));
                    dtIdentify.Columns.Add("EMPID", typeof(int));
                }

                // 填充数据
                foreach (var identify in identifyList)
                {
                    DataRow row = dtIdentify.NewRow();
                    row["Num"] = identify.Num;
                    row["VID"] = identify.VID ?? "";
                    row["VName"] = identify.VName ?? "";
                    row["Phone"] = identify.Phone ?? "";
                    row["Province"] = identify.Province ?? "";
                    row["City"] = identify.City ?? "";
                    row["Address"] = identify.Address ?? "";
                    row["Status"] = identify.Status ?? "";
                    row["EMPID"] = identify.EMPID ?? (object)DBNull.Value;
                    dtIdentify.Rows.Add(row);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载证件申请数据出错: {ex.Message}");
            }
        }

        void showAll()
        {
            LoadIdentifies();
            dgvIdentify.DataSource = dtIdentify;
            showXZ();
        }

        void showXZ()
        {
            // 检查是否已经添加了选择列
            if (!dgvIdentify.Columns.Contains("asCode"))
            {
                DataGridViewCheckBoxColumn asCode = new DataGridViewCheckBoxColumn();
                asCode.Name = "asCode";
                asCode.HeaderText = "选择";
                dgvIdentify.Columns.Add(asCode);
            }
        }

        private void ExamineIdentify_Load(object sender, EventArgs e)
        {
            showAll();
        }

        private void dgvIdentify_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvIdentify.Columns[e.ColumnIndex].Name == "asCode" && e.RowIndex >= 0)
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
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            bool selected = false;
            int selectedIndex = -1;

            for (int i = 0; i < dgvIdentify.Rows.Count; i++)
            {
                if (dgvIdentify.Rows[i].Cells["asCode"]?.EditedFormattedValue?.ToString() == "True")
                {
                    selected = true;
                    selectedIndex = i;
                    break;
                }
            }

            if (selected && selectedIndex >= 0)
            {
                // 获取选中的证件申请ID
                if (dgvIdentify.Rows[selectedIndex].Cells["Num"].Value != null &&
                    int.TryParse(dgvIdentify.Rows[selectedIndex].Cells["Num"].Value.ToString(), out int identifyId))
                {
                    // 确保已登录管理员ID有效
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

                    try
                    {
                        // 使用业务逻辑层审核证件申请
                        bool result = _identifyService.ApproveIdentify(identifyId, empId);

                        if (result)
                        {
                            // 更新UI
                            dgvIdentify.Rows[selectedIndex].Cells["Status"].Value = "已通过";
                            dgvIdentify.Rows[selectedIndex].Cells["EMPID"].Value = empId;
                            MessageBox.Show("审核成功");

                            // 重新加载数据
                            showAll();
                        }
                        else
                        {
                            MessageBox.Show("审核失败");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"审核过程中出错: {ex.Message}");
                    }
                }
                else
                {
                    MessageBox.Show("无法获取证件申请ID");
                }
            }
            else
            {
                MessageBox.Show("请选择审核项");
            }
        }

        private void btncancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}