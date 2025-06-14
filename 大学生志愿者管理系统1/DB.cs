using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;



namespace 大学生志愿者管理系统1
{
    class DB
    {
        public static SqlConnection cn;
        public static SqlConnection Getcn()
        {
            string mystr = "server=.;database=VOLUNTEER;integrated security=true";
            if(cn==null||cn.State==ConnectionState.Closed)
            {
                cn = new SqlConnection(mystr);
                cn.Open();
            }
            return cn;
        }
        public static DataTable GetDataSet(string sql)
        {
            SqlCommand cmd = new SqlCommand(sql, cn);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            return ds.Tables[0];
        }
        public static Boolean sqlEx(string sql)
        {
            SqlCommand cmd = new SqlCommand(sql, cn);
            try
            {
                cmd.ExecuteNonQuery();
                cn.Close();
            }
            catch(SqlException ex)
            {
                cn.Close();
                MessageBox.Show("执行失败" + ex.Message.ToString());
                return false;
            }
            return true;
        }
    }
}
