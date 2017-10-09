using MCLYGV3.Web.ClassLib;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Xml;

// ReSharper disable All

namespace MCLYGV3.Web.Models
{
	/// <summary>
	/// 批改操作类
	/// </summary>
	public static class GroupCorrectOperation
	{
		private static readonly string userName;
		private static readonly string partnerCode;
		private static readonly string passWord;
		private static readonly string Url;
		private static readonly string MappedPath;

		static GroupCorrectOperation()
		{
			userName = "MCLY";
			partnerCode = "MCLY";
			passWord = "MCLY20170103";
			MappedPath = AppDomain.CurrentDomain.BaseDirectory;
			Url = ConfigurationManager.AppSettings["CT_BaseUrl"] + "cxf/groupCorrectSerivce";

		}

		/// <summary>
		/// 想保险公司服务器提交数据，并组织返回结果
		/// </summary>
		/// <param name="id">访问标识</param>
		/// <param name="req">保险公司报文</param>
		/// <returns></returns>
		public static GroupCorrectResponse Post(string id, CtServerGroupCorrectRequest req)
		{
			string str = XmlUtil.Serializer(typeof(CtServerGroupCorrectRequest), req);
			str = str.Replace("<?xml version=\"1.0\"?>", "");

			str = Regex.Replace(str, "<CtServerGroupCorrectRequest((.|\n)*?)>", "");

			//str = str.Replace("<CtServerGroupCorrectRequest xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">", "");
			str = str.Replace("</CtServerGroupCorrectRequest>", "");

			//<CtServerGroupCorrectRequest xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">

			string submitStr;
			try
			{
				submitStr = File.ReadAllText(MappedPath + "\\CTServerGroupCorrectRequestTemplate.xml");
			}
			catch (Exception ex)
			{
				Log.Write("System.Log", id + ",GroupCorrectOperation,Post,读取模版文件出错" + ex.Message);
				GroupCorrectResponse errorResult = new GroupCorrectResponse
				{
					Head = new GroupCorrectResponse_Head
					{
						ResponseMessage = "读取模版文件出错",
						ResponseCode = "-1",
						requestType = "0"
					}
				};
				return errorResult;
			}
			if (submitStr == "" || submitStr.Contains("%DATA%") == false)
			{
				Log.Write("System.Log", id + ",GroupCorrectOperation,Post,模版文件内容出错");
				GroupCorrectResponse errorResult = new GroupCorrectResponse
				{
					Head = new GroupCorrectResponse_Head
					{
						ResponseMessage = "模版文件内容出错",
						ResponseCode = "-1",
						requestType = "0"
					}
				};
				return errorResult;
			}

			submitStr = submitStr.Replace("%DATA%", str);

			/*
             * 正确的提交报文
             * <soapenv:Envelope xmlns:soapenv="http://schemas.xmlsoap.org/soap/envelope/" xmlns:gro="http://groupCorrect.accident.provider.app.thirdparty.echannel.ebiz.isoftstone.com/">
	<soapenv:Header/>
	<soapenv:Body>
		<gro:operateCorrect>
			<!--Optional:-->
			<operateCorrect>

  <Head>
    <partnerCode>MCLY</partnerCode>
    <passWord>MCLY20170103</passWord>
    <queryId>614f9e972d504d1fa91172dde5f1d008</queryId>
    <requestType>00001</requestType>
    <userName>MCLY</userName>
  </Head>
  <Main>
    <policyNo>6000000114420170003331</policyNo>
    <sumPremium>-31.21</sumPremium>
    <validDate>2017-09-09 00:00:00</validDate>
  </Main>
  <CorrPlanList>
    <corrPlan>
      <RiskRealateParyList>
        <RiskRealatePary>
          <appliPhone>13888888888</appliPhone>
          <birthday>1994-06-01</birthday>
          <changeInsured>-31.21</changeInsured>
          <clientCName>s</clientCName>
          <clientType>1</clientType>
          <identifyNo>610632199406015930</identifyNo>
          <identifyType>01</identifyType>
          <insuredBusinessSource>1103001</insuredBusinessSource>
          <modifyFlag>B</modifyFlag>
          <sex>1</sex>
        </RiskRealatePary>
      </RiskRealateParyList>
    </corrPlan>
  </CorrPlanList>

			</operateCorrect>
		</gro:operateCorrect>
	</soapenv:Body>
</soapenv:Envelope>
             * 
             */


			Log.Write($"{id}_S_Req.Log", Url + "\r\n" + submitStr);
			//Log.Write("System.Log", id + ",转发报文到保险公司：" + Url);
			HttpWebRequest httpRequest = (HttpWebRequest)WebRequest.Create(Url);
			httpRequest.CookieContainer = new CookieContainer();
			httpRequest.Method = "POST";//POST 提交
			httpRequest.KeepAlive = true;
			httpRequest.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/43.0.2357.81 Safari/537.36";
			httpRequest.Accept = "application/xml, text/plain, */*";
			httpRequest.ContentType = "application/json;charset=UTF-8";
			byte[] bytes = System.Text.Encoding.UTF8.GetBytes(submitStr);
			httpRequest.ContentLength = bytes.Length;
			try
			{
				Stream stream = httpRequest.GetRequestStream();
				stream.Write(bytes, 0, bytes.Length);
				stream.Close();//以上是POST数据的写入
				HttpWebResponse httpResponse = (HttpWebResponse)httpRequest.GetResponse();
				using (Stream responsestream = httpResponse.GetResponseStream())
				{
					GroupCorrectResponse returnObj;
					if (responsestream != null)
						using (StreamReader sr = new StreamReader(responsestream, System.Text.Encoding.UTF8))
						{
							string content = sr.ReadToEnd();

							//Log.Write("System.Log", id + ",收到保险公司报文");
							/* 
                             * 成功的返回报文
                             * 
                             * <soap:Envelope xmlns:soap="http://schemas.xmlsoap.org/soap/envelope/"><soap:Body><ns2:operateCorrectResponse xmlns:ns2="http://groupCorrect.accident.provider.app.thirdparty.echannel.ebiz.isoftstone.com/"><return><responseBoby><responseHead><errorMessage>成功</errorMessage><requestType>12003</requestType><responseCode>0000</responseCode></responseHead><_return><ftrNo>01000170908019272</ftrNo><payBackFlag>1</payBackFlag><payment>80</payment><randomNo>42942345823970920516396863255240</randomNo></_return></responseBoby></return></ns2:operateCorrectResponse></soap:Body></soap:Envelope>
                             */
							Log.Write(id + "_S_Res.Log", content);
							returnObj = new GroupCorrectResponse();
							returnObj.Head = new GroupCorrectResponse_Head();
							returnObj.Head.ResponseMessage =
								Regex.Match(content, "<errorMessage>(.+)</errorMessage>")
									.Value.Replace("<errorMessage>", "")
									.Replace("</errorMessage>", "");
							returnObj.Head.ResponseCode =
								Regex.Match(content, "<responseCode>(.+)</responseCode>")
									.Value.Replace("<responseCode>", "")
									.Replace("</responseCode>", "");
							returnObj.Head.requestType =
								Regex.Match(content, "<requestType>(.+)</requestType>")
									.Value.Replace("<requestType>", "")
									.Replace("</requestType>", "");

							if (content.Contains("<_return/>")) return returnObj;
							returnObj.Body = new GroupCorrectResponse_Body
							{
								EndorNo = Regex.Match(content, "<endorNo>(.+)</endorNo>")
									.Value.Replace("<endorNo>", "")
									.Replace("</endorNo>", ""),
								FtrNo = Regex.Match(content, "<ftrNo>(.+)</ftrNo>")
									.Value.Replace("<ftrNo>", "")
									.Replace("</ftrNo>", ""),
								policyNo = Regex.Match(content, "<policyNo>(.+)</policyNo>")
									.Value.Replace("<policyNo>", "")
									.Replace("</policyNo>", ""),
								validDate = Regex.Match(content, "<validDate>(.+)</validDate>")
									.Value.Replace("<validDate>", "")
									.Replace("</validDate>", ""),
								PayBackFlag = Regex.Match(content, "<payBackFlag>(.+)</payBackFlag>")
									.Value.Replace("<payBackFlag>", "")
									.Replace("</payBackFlag>", ""),
								PayMent = Regex.Match(content, "<payment>(.+)</payment>")
									.Value.Replace("<payment>", "")
									.Replace("</payment>", ""),
								RandomNo = Regex.Match(content, "<randomNo>(.+)</randomNo>")
									.Value.Replace("<randomNo>", "")
									.Replace("</randomNo>", "")
							};

							return returnObj;
						}
					returnObj = new GroupCorrectResponse();
					returnObj.Head = new GroupCorrectResponse_Head();
					returnObj.Head.ResponseMessage = "StreamReader is null";
					returnObj.Head.ResponseCode = "-1";
					returnObj.Head.requestType = "0";
					return returnObj;
				}
			}
			catch (Exception e)
			{
				Log.Write("System.Log", id + ",发送报文出错：" + e.Message);
				GroupCorrectResponse returnObj = new GroupCorrectResponse();
				returnObj.Head = new GroupCorrectResponse_Head();
				returnObj.Head.ResponseMessage = e.Message;
				returnObj.Head.ResponseCode = "-1";
				returnObj.Head.requestType = "0";

				return returnObj;
			}
		}


		/// <summary>
		/// 客户端报文转服务端报文
		/// </summary>
		/// <param name="obj">客户端报文</param>
		/// <returns></returns>
		public static CtServerGroupCorrectRequest ConvertToCtServerGroupCorrectRequest(GroupCorrectRequest obj)
		{
			CtServerGroupCorrectRequest returnObj = new CtServerGroupCorrectRequest();
			returnObj.Head = new CtServerGroupCorrectRequestHead();
			returnObj.Head.partnerCode = partnerCode;
			returnObj.Head.passWord = passWord;
			returnObj.Head.queryId = Guid.NewGuid().ToString("N");
			returnObj.Head.requestType = "00001";
			returnObj.Head.userName = userName;

			returnObj.Main = new CtServerGroupCorrectRequestMain();
			returnObj.Main.policyNo = obj.policyNo;
			returnObj.Main.sumPremium = obj.sumPremium.ToString(CultureInfo.InvariantCulture);
			returnObj.Main.validDate = Convert.ToDateTime(obj.validDate).ToString("yyyy-MM-dd");

			returnObj.CorrPlanList = new List<corrPlan>();
			var insuredBusinessSourceList = obj.InsuredUserList.Select(s => s.insuredBusinessSource).Distinct();
			foreach (var insuredBusinessSource in insuredBusinessSourceList)
			{
				corrPlan plan = new corrPlan();
				plan.RiskRealateParyList = new List<RiskRealatePary>();
				var persionList = obj.InsuredUserList.Where(s => s.insuredBusinessSource == insuredBusinessSource).ToList();
				foreach (var clientP in persionList)
				{
					Idno idno = new Idno(clientP.identifyNo);
					RiskRealatePary p = new RiskRealatePary
					{
						appliPhone = "13888888888",
						birthday = idno.getBirthday()

					};
					p.changeInsured = clientP.changeInsured.ToString(CultureInfo.InvariantCulture);
					p.clientCName = clientP.clientCName;
					p.clientType = "1";
					p.identifyNo = clientP.identifyNo;
					p.identifyType = "01";
					p.insuredBusinessSource = clientP.insuredBusinessSource;
					p.modifyFlag = obj.modifyFlag;
					p.sex = idno.getSex2();
					plan.RiskRealateParyList.Add(p);
				}
				returnObj.CorrPlanList.Add(plan);
			}

			return returnObj;
		}

	}
}