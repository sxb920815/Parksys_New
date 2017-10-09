using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace MCLYGV3.Web
{
    public static class Log
    {
        static string MappedPath, MappedPathOther;
        static Log()
        {
            MappedPath = System.AppDomain.CurrentDomain.BaseDirectory + "\\Log";
			MappedPathOther = System.AppDomain.CurrentDomain.BaseDirectory + "\\LogOther";
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

		/// <summary>
		/// 写日志
		/// </summary>
		/// <param name="FileName">文件名</param>
		/// <param name="Content">内容</param>
		public static void OtherWrite(string FileName, string Content)
		{
			string TimePath = DateTime.Now.ToString("yyyy-MM") + "\\" + DateTime.Now.ToString("yyyy-MM-dd");
			Directory.CreateDirectory(MappedPathOther + TimePath);
			File.AppendAllText(MappedPathOther + TimePath + "\\" + FileName, Content + "\r\n");
		}

		public static void SystemWriteDebug(string msg)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("===========================================================================\r\n");
            sb.Append($"【{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}】\r\n");
            sb.Append(msg + "\r\n");
            File.AppendAllText(MappedPath + "\\system.Debug.log", sb.ToString() + "\r\n");
        }
		public static void SystemWriteError(string msg)
		{
			StringBuilder sb = new StringBuilder();
			sb.Append("===========================================================================\r\n");
			sb.Append($"【{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}】\r\n");
			sb.Append(msg + "\r\n");
			File.AppendAllText(MappedPath + "\\system.Error.log", sb.ToString() + "\r\n");
		}

        public static void Error(Exception ex)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("===========================================================================\r\n");
            sb.Append($"【{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}】\r\n");
            sb.Append("\t错误信息：" + ex.Message);
            sb.Append("\r\n\t错误源：" + ex.Source);
            sb.Append("\r\n\t异常方法：" + ex.TargetSite);
            sb.Append("\r\n\t堆栈信息：" + ex.StackTrace);
            sb.Append("===========================================================================\r\n");
            File.AppendAllText(MappedPath + "\\system.Error.log", sb.ToString() + "\r\n");
        }
    }
}