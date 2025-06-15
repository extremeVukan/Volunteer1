using System.Web;
using System.Web.Mvc;

namespace 大学生志愿者管理系统Web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
