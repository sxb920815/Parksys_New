using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace MCLYGV3.DB
{
    public static class Log
    {
        static string MappedPath;
        static Log()
        {
            MappedPath = System.AppDomain.CurrentDomain.BaseDirectory + "\\Log";
            Directory.CreateDirectory(MappedPath);
            MappedPath = MappedPath + "\\";
        }
        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="FileName">文件名</param>
        /// <param name="Content">内容</param>
        public static void Write(string FileName, string Content)
        {
            string TimePath = DateTime.Now.ToString("yyyy-MM") + "\\" + DateTime.Now.ToString("yyyy-MM-dd");
            Directory.CreateDirectory(MappedPath + TimePath);
            File.AppendAllText(MappedPath + TimePath + "\\" + FileName, Content + "\r\n");
        }
        public static void SystemWrite(string msg)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("===========================================================================\r\n");
            sb.Append($"【{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}】\r\n");
            sb.Append(msg + "\r\n");
            File.AppendAllText(MappedPath + "\\system.log", sb.ToString() + "\r\n");
        }
    }
}