using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace MCLYGV3.Web
{
	public static class HttpOperation
	{
		public static string GetContent(string url, SortedList<string, string> parameter)
		{
			string queryString = "";
			if (parameter.Count != 0)
			{
				queryString = parameter.Keys.Aggregate("?", (current, key) => current + key + "=" + parameter[key] + "&");
				queryString = queryString.TrimEnd('&');
			}

			var httpRequest = (HttpWebRequest)WebRequest.Create(url + queryString);
			httpRequest.Accept = "application/json, text/plain, */*";
			httpRequest.ContentType = "application/json;charset=UTF-8";
			httpRequest.Method = "GET";//POST 提交
			httpRequest.KeepAlive = true;
			httpRequest.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/43.0.2357.81 Safari/537.36";
			try
			{
				var httpResponse = (HttpWebResponse)httpRequest.GetResponse();
				using (var responsestream = httpResponse.GetResponseStream())
				{
					if (responsestream != null)
						using (var sr = new StreamReader(responsestream, Encoding.UTF8))
						{
							var content = sr.ReadToEnd();
							return content;
						}
					else
						return "";

				}
			}
			catch (Exception e)
			{
				return "";
			}
		}
		public static string GetHtml(string url)
		{
			try
			{
				HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
				request.Method = "GET";
				request.ContentType = "application/json;charset=utf-8";
				//Content - Type:application / json; charset = utf - 8
				//       Accept: application / json; charset = utf - 8
				request.Accept = "application/json;charset=utf-8";
				request.AllowAutoRedirect = false;
				request.KeepAlive = true;
				request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/43.0.2357.81 Safari/537.36";
				//接收响应
				HttpWebResponse httpResponse = (HttpWebResponse)request.GetResponse();
				using (Stream responsestream = httpResponse.GetResponseStream())
				{
					using (StreamReader sr = new StreamReader(responsestream, System.Text.Encoding.UTF8))
					{
						string content = sr.ReadToEnd();
						Log.Write($"HttpGet_{DateTime.Now.ToString("HHmmss")}.txt", $"url:{url}\r\n\r\n{content}");
						return content;
					}
				}
			}
			catch (Exception ex)
			{
				Log.Write($"HttpGetError_{DateTime.Now.ToString("HHmmss")}.txt", $"url:{url}\r\n\r\n{ex.Message}\r\n\r\n{ex.ToString()}");
				return "Error:" + ex.Message;
			}
		}

		public static string Post(string Url, string data)
		{
			HttpWebRequest httpRequest = (HttpWebRequest)WebRequest.Create(Url);
			httpRequest.CookieContainer = new CookieContainer();
			httpRequest.Method = "POST";//POST 提交
			httpRequest.KeepAlive = true;
			httpRequest.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/43.0.2357.81 Safari/537.36";
			httpRequest.Accept = "application/xml, text/plain, */*";
			httpRequest.ContentType = "application/json;charset=UTF-8";
			byte[] bytes = Encoding.UTF8.GetBytes(data);
			httpRequest.ContentLength = bytes.Length;
			try
			{
				Stream stream = httpRequest.GetRequestStream();
				stream.Write(bytes, 0, bytes.Length);
				stream.Close();//以上是POST数据的写入
				HttpWebResponse httpResponse = (HttpWebResponse)httpRequest.GetResponse();
				using (Stream responsestream = httpResponse.GetResponseStream())
				{
					if (responsestream != null)
						using (StreamReader sr = new StreamReader(responsestream, Encoding.UTF8))
						{
							string content = sr.ReadToEnd();
							return content;
						}
					else
						return "";
				}
			}
			catch (Exception e)
			{
				Log.Write("System.Log", "出错：" + e.Message);
				return "";
			}

		}
	}
}