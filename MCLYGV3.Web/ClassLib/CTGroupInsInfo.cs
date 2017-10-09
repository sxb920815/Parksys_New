using MCLYGV3.DB;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace MCLYGV3.Web
{
    public static class CTGroupInsInfo
    {
        private static Dictionary<string, ZYInfo> ZYInfoList = new Dictionary<string, ZYInfo>();
        public static List<double> bTimeModulusList = new List<double>() { 0, 0.25, 0.35, 0.45, 0.55, 0.65, 0.7, 0.75, 0.8, 0.85, 0.9, 0.95, 1 };
        private static Dictionary<string, Rate> selhurt = new Dictionary<string, Rate>();
        private static Dictionary<string, Rate> selmedical = new Dictionary<string, Rate>();
        private static Dictionary<string, Rate> selhospital = new Dictionary<string, Rate>();
        private static double roatePoint = 0.65;
        public static string[] PlanCodeList = { "1144001", "1144002", "1144003" };
        static CTGroupInsInfo()
        {
            selhurt.Add("1", new Rate() { min = 0.000140, max = 0.0170838720 });
            selhurt.Add("2", new Rate() { min = 0.000140, max = 0.0170838720 });
            selhurt.Add("3", new Rate() { min = 0.000140, max = 0.0170838720 });
            selhurt.Add("4", new Rate() { min = 0.000315, max = 0.0170838720 });
            selhurt.Add("5", new Rate() { min = 0.000525, max = 0.0294222240 });
            selmedical.Add("1", new Rate() { min = 0.00045, max = 0.026325 });
            selmedical.Add("2", new Rate() { min = 0.00045, max = 0.026325 });
            selmedical.Add("3", new Rate() { min = 0.00045, max = 0.026325 });
            selmedical.Add("4", new Rate() { min = 0.00180, max = 0.030000 });
            selmedical.Add("5", new Rate() { min = 0.00320, max = 0.040000 });
            selhospital.Add("1", new Rate() { min = 0.0557685, max = 0.34425 });
            selhospital.Add("2", new Rate() { min = 0.0557685, max = 0.34425 });
            selhospital.Add("3", new Rate() { min = 0.0557685, max = 0.34425 });
            selhospital.Add("4", new Rate() { min = 0.1487160, max = 0.98789 });
            selhospital.Add("5", new Rate() { min = 0.1858950, max = 1.46770 });

            FileStream fs = new FileStream(AppDomain.CurrentDomain.BaseDirectory + "ClassLib\\ZYlevel_CT.txt", FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            StreamReader read = new StreamReader(fs, Encoding.UTF8);
            int i = 0;
            string line;
            while ((line = read.ReadLine()) != null)
            {
                i++;
                string[] tmp = line.Split(',');
                ZYInfo info = new ZYInfo() { ProfessionCode = tmp[0], ProfessionName = tmp[1], ProfessionLv = int.Parse(tmp[2]) };
                ZYInfoList.Add(tmp[0], info);
            }
        }
        public static int GetProfessionLv(string code)
        {
            if (ZYInfoList.ContainsKey(code))
            {
                return ZYInfoList[code].ProfessionLv;
            }
            else
            {
                return -1;
            }
        }
        public static string GetProfessionName(string code)
        {
            if (ZYInfoList.ContainsKey(code))
            {
                return ZYInfoList[code].ProfessionName;
            }
            else
            {
                return "";
            }
        }
        public static decimal getPrice(string plancode, int ProfessionLv, double dutyAmout, int btime)
        {
            double result = 0;
            if (plancode == PlanCodeList[0])
            {
                if (selhurt.ContainsKey(ProfessionLv.ToString()))
                {
                    if (selhurt[ProfessionLv.ToString()].min / (1 - roatePoint) < selhurt[ProfessionLv.ToString()].max && selhurt[ProfessionLv.ToString()].min < selhurt[ProfessionLv.ToString()].min / (1 - roatePoint))
                        result = dutyAmout * (selhurt[ProfessionLv.ToString()].min / (1 - roatePoint)) * bTimeModulusList[btime];

                }
            }
            if (plancode == PlanCodeList[1])
            {
                if (selhurt.ContainsKey(ProfessionLv.ToString()))
                {
                    if (selmedical[ProfessionLv.ToString()].min / (1 - roatePoint) < selmedical[ProfessionLv.ToString()].max && selmedical[ProfessionLv.ToString()].min < selmedical[ProfessionLv.ToString()].min / (1 - roatePoint))
                        result = dutyAmout * (selmedical[ProfessionLv.ToString()].min / (1 - roatePoint)) * bTimeModulusList[btime];
                }
            }
            if (plancode == PlanCodeList[2])
            {
                if (selhurt.ContainsKey(ProfessionLv.ToString()))
                {
                    if (selhospital[ProfessionLv.ToString()].min / (1 - roatePoint) < selhospital[ProfessionLv.ToString()].max && selhospital[ProfessionLv.ToString()].min < selhospital[ProfessionLv.ToString()].min / (1 - roatePoint))
                        result = dutyAmout * (selhospital[ProfessionLv.ToString()].min / (1 - roatePoint)) * bTimeModulusList[btime];
                }
            }
            return decimal.Parse(result.ToString("0.00"));
        }
    }

    public class Rate
    {
        public double min;
        public double max;
    }

}