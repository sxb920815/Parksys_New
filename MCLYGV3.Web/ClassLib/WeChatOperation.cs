using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace MCLYGV3.Web
{


	public static class WeChatOperation
	{
		private static DateTime AccessTokenLastTime, JsTicketLastTime;
		private static string AccessToken, JsTicket;
		static WeChatOperation()
		{
			AccessTokenLastTime = DateTime.Now;
			JsTicketLastTime = DateTime.Now;

			AccessToken = GetServerAccessToken();
			JsTicket = GetServerJsTicket();
		}
		public static string GetAccessToken()
		{
			TimeSpan tsSpan = DateTime.Now - AccessTokenLastTime;
			if (tsSpan.TotalSeconds > 6000)
				return GetServerAccessToken();
			else
				return AccessToken;
		}
		public static string GetJsTicket()
		{
			TimeSpan tsSpan = DateTime.Now - JsTicketLastTime;
			if (tsSpan.TotalSeconds > 6000)
				return GetServerJsTicket();
			else
				return JsTicket;
		}
		private static string GetServerJsTicket()
		{
			//https://api.weixin.qq.com/cgi-bin/ticket/getticket?access_token=BqSKLEmETi81jRv2krBFaNjJuWnSoMSbBJzmVJckz50TexHZ0abc5iUTPJ1g0Sni6y9rb4VFUxwXQR2bPKUNcGVgGemtBJy68txyVCqAiXK8bFhUD0bjExP5VBXtj53sDQRcAHALYZ&type=jsapi
			//string appsecret = "739c3261102665b2744f2aa7df348310";
			string appID = ConfigurationManager.AppSettings["appID"];
			string appsecret = ConfigurationManager.AppSettings["appsecret"];

			SortedList<string, string> pSortedList = new SortedList<string, string>();
			pSortedList.Add("access_token", GetAccessToken());
			pSortedList.Add("type", "jsapi");
			string Result = HttpOperation.GetContent("https://api.weixin.qq.com/cgi-bin/ticket/getticket", pSortedList);

			if (Result.Contains("ticket"))
			{
				JsTicketRes obj = JsonConvert.DeserializeObject<JsTicketRes>(Result);
				return obj.ticket;
			}
			else
			{
				return "";
			}
		}

		public static string GetMenu()
		{
			//https://api.weixin.qq.com/cgi-bin/menu/get?access_token=ACCESS_TOKEN

			SortedList<string, string> pSortedList = new SortedList<string, string>();
			pSortedList.Add("access_token", GetAccessToken());
			string Result = HttpOperation.GetContent("https://api.weixin.qq.com/cgi-bin/menu/get", pSortedList);
			return Result;
		}


		public static string GetJsSignature(long timestamp, string url)
		{
			string jsapi_ticket = GetJsTicket();
			string noncestr = ConfigurationManager.AppSettings["nonceStr"];
			string string1 = $"jsapi_ticket={jsapi_ticket}&noncestr={noncestr}&timestamp={timestamp}&url={url}";
			return Common.Sha1(string1);
		}


		private static string GetServerAccessToken()
		{
			//string appID = "wx8ea2f6e11da52228";
			//string appsecret = "739c3261102665b2744f2aa7df348310";
			string appID = ConfigurationManager.AppSettings["appID"];
			string appsecret = ConfigurationManager.AppSettings["appsecret"];

			SortedList<string, string> pSortedList = new SortedList<string, string>();
			pSortedList.Add("grant_type", "client_credential");
			pSortedList.Add("appid", appID);
			pSortedList.Add("secret", appsecret);

			string Result = HttpOperation.GetContent("https://api.weixin.qq.com/cgi-bin/token", pSortedList);

			if (Result.Contains("access_token"))
			{

				AccessTokenRes obj = JsonConvert.DeserializeObject<AccessTokenRes>(Result);
				return obj.access_token;
			}
			else
			{
				return "";
			}
		}

		public static JsAccessTokenRes GetJsAccessToken(string code)
		{
			////https://api.weixin.qq.com/sns/oauth2/access_token?appid=APPID&secret=SECRET&code=CODE&grant_type=authorization_code
			string appID = ConfigurationManager.AppSettings["appID"];
			string appsecret = ConfigurationManager.AppSettings["appsecret"];

			SortedList<string, string> pSortedList = new SortedList<string, string>();
			
			pSortedList.Add("appid", appID);
			pSortedList.Add("secret", appsecret);
			pSortedList.Add("code", code);
			pSortedList.Add("grant_type", "authorization_code");

			string Result = HttpOperation.GetContent("https://api.weixin.qq.com/sns/oauth2/access_token", pSortedList);
			if (Result.Contains("access_token"))
			{
				JsAccessTokenRes obj = JsonConvert.DeserializeObject<JsAccessTokenRes>(Result);
				return obj;
			}
			else
			{
				return null;
			}
		}

		public static string GetUserInfo(string access_token,string openid)
		{
			//https://api.weixin.qq.com/sns/userinfo?access_token=ACCESS_TOKEN&openid=OPENID&lang=zh_CN

			SortedList<string, string> pSortedList = new SortedList<string, string>();

			pSortedList.Add("access_token", access_token);
			pSortedList.Add("openid", openid);

			string Result = HttpOperation.GetContent("https://api.weixin.qq.com/sns/userinfo", pSortedList);
			return Result;

		}

	}

	public class AccessTokenRes
	{
		/// <summary>
		/// AccessToken
		/// </summary>
		public string access_token { get; set; }

		/// <summary>
		/// 剩余秒数
		/// </summary>
		public int expires_in { get; set; }

	}
	public class JsAccessTokenRes: AccessTokenRes
	{
		public string refresh_token { get; set; }
		public string openid { get; set; }
		public string scope { get; set; }
	}
	public class JsTicketRes
	{
		public int errcode { get; set; }
		public string errmsg { get; set; }
		public string ticket { get; set; }
		public int expires_in { get; set; }
	}

	//{"errcode":0,"errmsg":"ok","ticket":"sM4AOVdWfPE4DxkXGEs8VDM76hKuU4EDdFuhZRE3PHsNFydnivDvPxrAMj4reDxATB6uu7PG27vCsIytmLHMhA","expires_in":7200}


}