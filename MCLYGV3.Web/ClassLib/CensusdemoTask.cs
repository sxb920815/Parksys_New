using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Web;

namespace MCLYGV3.Web.ClassLib
{
    public class CensusdemoTask
    {
        System.Threading.Timer timer;
        string imgaePath_C; 
        string imgaePath_D;

        public CensusdemoTask()
        {
            timer = new System.Threading.Timer(SetCensus, null, 0, 1000 * 10);
            imgaePath_C = ConfigurationManager.AppSettings["DelImagePath_C"];
            imgaePath_D = ConfigurationManager.AppSettings["DelImagePath_D"];
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void SetCensus(object obj)
        {
            //删除C盘照片


            //删除D盘照片




            string txt = string.Format("写入时间:{0}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            Log.SystemWriteDebug($"{DateTime.Now}============{txt}");
            //FileInfo f = new FileInfo("/124.txt");
            //StreamWriter sw = File.Exists("/124.com") ? f.CreateText() : f.AppendText();
            //byte[] txtbytes = Encoding.UTF8.GetBytes(txt);
            //sw.WriteLine(txt);
            //sw.Flush();
            //sw.Close();
            //count++;
        }
    }
}