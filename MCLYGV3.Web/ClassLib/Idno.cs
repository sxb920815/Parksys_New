using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace MCLYGV3.Web.ClassLib
{
	public class Idno
	{
		private string no;
		public Idno(string no)
		{
			this.no = no;
			if (no.Length != 18)
				this.no = "000000000000000000";
		}
		public int getAge()
		{
			int birthYear, nowYear;
			char[] temp = no.ToCharArray();
			string res = "";
			for (int i = 0; i < 4; i++)
			{
				res += temp[i + 6];
			}
			birthYear = Convert.ToInt32(res);
			nowYear = Convert.ToInt32(DateTime.Now.ToString("yyyy"));
			return nowYear - birthYear;
		}
		/// <summary>
		/// 返回性别，F/M
		/// </summary>
		/// <returns></returns>
		public string getSex()
		{
			char[] temp = no.ToCharArray();
			if (Convert.ToInt32(temp[16]) % 2 == 0)
				return "F";
			else
				return "M";
		}
		/// <summary>
		/// 返回性别，2/1
		/// </summary>
		/// <returns></returns>
		public string getSex2()
		{
			char[] temp = no.ToCharArray();
			if (Convert.ToInt32(temp[16]) % 2 == 0)
				return "2";
			else
				return "1";
		}
		/// <summary>
		/// 返回格式为yyyy-MM-dd
		/// </summary>
		/// <returns></returns>
		public string getBirthday()
		{
			StringBuilder sb = new StringBuilder();
			sb.Append(no.Substring(6, 4));
			sb.Append("-");
			sb.Append(no.Substring(10, 2));
			sb.Append("-");
			sb.Append(no.Substring(12, 2));
			return sb.ToString();
		}
	}
}