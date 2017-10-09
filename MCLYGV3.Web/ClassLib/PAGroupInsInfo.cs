using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace MCLYGV3.Web
{
	public static class PAGroupInsInfo
	{
		private static Dictionary<string, ZYInfo> ZYInfoList;
		private static List<ZYPrice> ZYPriceList;
		public static string[] PlanCodeList = new string[] { "Y502", "J513", "J511", "J506" };
		public static string[] dutyCodeList = new string[] { "YA01", "JA17", "JD06", "JA17" };
		static PAGroupInsInfo()
		{
			ZYInfoList = new Dictionary<string, ZYInfo>();

			string filepath = AppDomain.CurrentDomain.BaseDirectory + "ClassLib\\ZYlevel_pingan.txt";
			FileStream fs = new FileStream(filepath, FileMode.Open, FileAccess.Read,FileShare.ReadWrite);
			StreamReader read = new StreamReader(fs, Encoding.UTF8);
			string strReadline;
			while ((strReadline = read.ReadLine()) != null)
			{
				string[] tmp = strReadline.Split(',');
				ZYInfo info = new ZYInfo() { ProfessionCode = tmp[0], ProfessionName = tmp[1], ProfessionLv = int.Parse(tmp[2]) };
				ZYInfoList.Add(tmp[0], info);
			}

			ZYPriceList = new List<ZYPrice>();

			filepath = AppDomain.CurrentDomain.BaseDirectory + "ClassLib\\ZYPrice_pingan.txt";
			fs = new FileStream(filepath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
			read = new StreamReader(fs, Encoding.UTF8);
			while ((strReadline = read.ReadLine()) != null)
			{
				string[] tmp = strReadline.Split(',');
				ZYPrice info = new ZYPrice() { planCode = tmp[0], ProfessionLv = int.Parse(tmp[1]), dutyAount = int.Parse(tmp[2]), ModalPremium = decimal.Parse(tmp[3]) };
				ZYPriceList.Add(info);
			}
			fs.Close();
			read.Close();

		}
		public static string GetProfessionNameByCode(string code)
		{
			if (ZYInfoList.ContainsKey(code))
				return ZYInfoList[code].ProfessionName;
			else
				return "";
		}
		public static int GetProfessionLvByCode(string code)
		{
			if (ZYInfoList.ContainsKey(code))
				return ZYInfoList[code].ProfessionLv;
			else
				return -1;
		}

		public static decimal GetPrice(string planCode, int ProfessionLv, int dutyAount)
		{
			if (ProfessionLv == 2)
				ProfessionLv = 1;
			if (ProfessionLv == 3)
				ProfessionLv = 1;

			var obj = ZYPriceList.FirstOrDefault(t => t.planCode == planCode && t.ProfessionLv == ProfessionLv && t.dutyAount == dutyAount);
			if (obj == null)
				return 0;
			else
				return obj.ModalPremium;
		}





		public static void Test()
		{


		}
	}
	public class ZYInfo
	{
		/// <summary>
		/// 职业代码
		/// </summary>
		public string ProfessionCode { get; set; }
		/// <summary>
		/// 职业名称
		/// </summary>
		public string ProfessionName { get; set; }
		/// <summary>
		/// 职业等级
		/// </summary>
		public int ProfessionLv { get; set; }
	}


	public class ZYPrice
	{
		/// <summary>
		/// 险种代码
		/// </summary>
		public string planCode { get; set; }
		/// <summary>
		/// 职业级别
		/// </summary>
		public int ProfessionLv { get; set; }
		/// <summary>
		/// 保额
		/// </summary>
		public int dutyAount { get; set; }
		/// <summary>
		/// 保费
		/// </summary>
		public decimal ModalPremium { get; set; }
	}
}