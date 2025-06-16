using System;
using System.IO;
using System.Web.Mvc;

namespace 大学生志愿者管理系统Web.Controllers
{
    public class ImageController : Controller
    {
        // GET: Image/Activity?path=\Act_Images\教育类\文件名.png
        public ActionResult Activity(string path)
        {
            try
            {
                // 指定WinForm应用的基础路径
                string basePath = @"D:\大学\数据库开发\大学生志愿者管理系统1\大学生志愿者管理系统1\bin\Debug";
                string fullPath = Path.Combine(basePath, path.TrimStart('\\'));

                if (System.IO.File.Exists(fullPath))
                {
                    return File(fullPath, GetContentType(fullPath));
                }

                // 返回默认图片
                return File(Server.MapPath("~/Content/Images/暂无图片.gif"), "image/gif");
            }
            catch
            {
                // 异常时返回默认图片
                return File(Server.MapPath("~/Content/Images/暂无图片.gif"), "image/gif");
            }
        }

        private string GetContentType(string path)
        {
            string ext = Path.GetExtension(path).ToLower();
            switch (ext)
            {
                case ".jpg":
                case ".jpeg":
                    return "image/jpeg";
                case ".png":
                    return "image/png";
                case ".gif":
                    return "image/gif";
                default:
                    return "application/octet-stream";
            }
        }
    }
}