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
        DateTime dt;

        public CensusdemoTask()
        {
            timer = new System.Threading.Timer(SetCensus, null, 0, 1000 * 10);
            imgaePath_C = ConfigurationManager.AppSettings["DelImagePath_C"];
            imgaePath_D = ConfigurationManager.AppSettings["DelImagePath_D"];
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void SetCensus(object obj)
        {
            dt = DateTime.Now.AddMonths(-1);
            //todo 删除C盘照片
            Director(imgaePath_C);

            dt = DateTime.Now.AddMonths(-12);
            //todo 删除D盘照片
            Director(imgaePath_D);
        }

        public void Director(string path)
        {
            DirectoryInfo d = new DirectoryInfo(path);
            FileSystemInfo[] fsinfos = d.GetFileSystemInfos();
            foreach (FileSystemInfo fsinfo in fsinfos)
            {
                if (fsinfo is DirectoryInfo)     //判断是否为文件夹  
                {
                    Director(fsinfo.FullName);//递归调用  
                }
                else
                {
                    if (fsinfo.CreationTime<dt)
                    {
                        File.Delete(fsinfo.FullName);
                        Log.SystemWriteDebug($"删除文件:{fsinfo.FullName}");
                    }
                }
            }
            if (fsinfos.Count()==0)
            {
                try
                {
                    File.Delete(path);
                }
                catch (Exception e)
                {
                    Log.SystemWriteError($"{e.Message}");
                }
            }
        }

    }
}