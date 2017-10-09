using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Xml;
using System.Xml.Serialization;

namespace MCLYGV3.Web
{
	public class Common
	{
	    private static readonly string key = "";

        /// <summary>
        /// 获得从1970年的时间
        /// </summary>
        /// <returns></returns>
        public static int GetTime()
		{
			DateTime t1 = new DateTime(1970, 1, 1);
			TimeSpan ts = DateTime.Now - t1;
			return (int)ts.TotalSeconds;
		}

		/// <summary>
		/// URL转换
		/// </summary>
		/// <param name="url">url</param>
		/// <returns></returns>
		public static string UrlEncode(string url)
		{
			return HttpUtility.UrlEncode(url);
		}


		/// <summary>
		/// 基于Sha1的自定义加密字符串方法：输入一个字符串，返回一个由40个字符组成的十六进制的哈希散列（字符串）。
		/// </summary>
		/// <param name="str">要加密的字符串</param>
		/// <returns>加密后的十六进制的哈希散列（字符串）</returns>
		public static string Sha1(string str)
		{
			var buffer = Encoding.UTF8.GetBytes(str);
			var data = SHA1.Create().ComputeHash(buffer);
			var sb = new StringBuilder();
			foreach (var t in data)
			{
				sb.Append(t.ToString("X2"));
			}
			return sb.ToString();
		}

        /// <summary>
        /// 密码加密
        /// </summary>
        /// <param name="psd"></param>
        /// <returns></returns>
        public static string Md5Password(string psd)
        {
            var md5 = MD5.Create();
            psd = Sha1(psd);
            psd = Encoding.UTF8.GetString(md5.ComputeHash(Encoding.UTF8.GetBytes(psd + key)));
            psd = Sha1(psd);
            return psd;
        }

    }
}