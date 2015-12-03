using System.IO;

namespace Niqiu.Core.Helpers
{
   public class FileHelper
    {
       public static string GetContentType(string extensionName)
       {
           extensionName = extensionName.Replace(".", "");
           var str = "text/html";
            switch (extensionName.ToLower())
            {
                //图片
                case "jpeg":
                case "jpg":
                case "jpe":
                    str = "image/jpeg";
                    break;
                case "gif":
                    str = "image/gif";
                    break;
                case "bmp":
                    str = "image/bmp";
                    break;
                case "png":
                    str = "image/png";
                    break;
                // 文档
                case "rtf":
                    str = "text/rtf";
                    break;
                case "txt":
                    str = "text/plain";
                    break;//
                //excel
                case "xql":
                case "xsd":
                case "xslt":
                    str = "text/xml";
                    break;
                //xml
                case "xls":
                    str = "application/vnd.ms-excel";
                    break;
                case "doc":
                case "docx":
                    str = "application/msword";
                    break;
                case "rtx":
                    str = "text/richtext";
                    break;
                //压缩文件
                case "zip":
                    str = "application/zip";
                    break;
                //可执行文件
                case "exe":
                    str = "application/octet-stream";
                    break;
                case "html":
                case "htx":
                case "htm":
                    str = "text/html";
                    break;
            }

            return str;
        }

       /// <summary>
       /// 换算文件大小
       /// </summary>
       /// <param name="bb"></param>
       /// <returns></returns>
       public static string ShortLength(long bb)
       {
           var k = bb / 1024;
           if (k >= 1)
           {
               var m = k / 1024;
               if (m >= 1)
               {
                   return m + "M";
               }
               return k + "k";
           }
           return bb + "byte";
       }

    }
}
