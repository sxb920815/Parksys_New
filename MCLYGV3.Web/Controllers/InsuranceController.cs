using MCLYGV3.Web.ClassLib;
using MCLYGV3.Web.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Xml;
using MCLYGV3.DB;
using MCLYGV3.Web.Work;

namespace MCLYGV3.Web.Controllers
{
    [EnableCors("*", "*", "*")]
    public class InsuranceController : BaseApiController
	{

		[HttpPost]
		//接受并处理用户请求,再向服务器发出post请求
		public DataJsonResult Create()
		{
			var req = new DataJsonResult();
            if (!IsAuth)
                return base.ErrorResult;
            string requestString = new StreamReader(HttpContext.Current.Request.InputStream).ReadToEnd();
			ClientRequest RequestInfo;
			try
			{
				RequestInfo = JsonConvert.DeserializeObject<ClientRequest>(requestString);
			}
			catch (Exception e)
			{
				Log.Write($"Error_C_Req.log", $"[UserID:{AuthorizedUser.ID}]\r\n\r\n{requestString}");
                req.ReturnCode = "502";
				req.ErrorMessage = "提交报文转换出错" + e.Message;
				return req;
			}
			if (B_Order.GetCount(t => t.OrderCode == RequestInfo.Body.OrderCode) > 0)
			{
				Log.Write($"Error_C_Req.log", $"[UserID:{AuthorizedUser.ID}]\r\n\r\n{requestString}");
                req.ReturnCode = "503";
				req.ErrorMessage = "订单编号重复";
				return req;

			}
            var id = RequestInfo.Body.OrderCode;
            //请求日志
            Log.Write($"{id}_{RequestInfo.Body.InsuranceCompany}_C_Req.log", $"[UserID:{AuthorizedUser.ID}]\r\n\r\n{requestString}");

            var success = false;
			var businessNo = "";
			var applyPolicyNo = "";
			var ftrno = "";
			var BK_SERIAL = "";
			string ErrorMessage = "";
			if (RequestInfo.Body.InsuranceCompany == "PA")
			{
				RequestOperation.PAAddRequest(RequestInfo, out businessNo, out applyPolicyNo, out BK_SERIAL, out success, out ErrorMessage);
			}
			if (RequestInfo.Body.InsuranceCompany == "CT")
			{
			    //success = true;
                RequestOperation.CTAddRequest(RequestInfo, out ftrno, out success, out ErrorMessage);
			}

			var order = new M_Order();
			if (success)
			{
				// 入库
				if (RequestInfo.Body.InsuranceCompany == "CT")
				{
					//添加订单
					order = B_Order.AddStorage(RequestInfo.Body.OrderCode, RequestInfo.RelatedParty.InsuredName, RequestInfo.RelatedParty.IdentifyNumber, RequestInfo.RelatedParty.IdentifyPic, RequestInfo.Body.Surcharge, RequestInfo.Body.InsuranceCompany, RequestInfo.Body.StartDate, RequestInfo.Body.StartDate.AddMonths(RequestInfo.Body.applyMonth).AddSeconds(-1), RequestInfo.Body.applyMonth, RequestInfo.RelatedParty.Email, RequestInfo.RelatedParty.Tel, AuthorizedUser.ID, RequestInfo.RiskRealateParyList, ftrno, "", "", "", DateTime.Now, DateTime.Now, "", "");
				}
				else if (RequestInfo.Body.InsuranceCompany == "PA")
				{
					//添加订单
					order = B_Order.AddStorage(RequestInfo.Body.OrderCode, RequestInfo.RelatedParty.InsuredName, RequestInfo.RelatedParty.IdentifyNumber, RequestInfo.RelatedParty.IdentifyPic, RequestInfo.Body.Surcharge, RequestInfo.Body.InsuranceCompany, RequestInfo.Body.StartDate, RequestInfo.Body.StartDate.AddMonths(RequestInfo.Body.applyMonth).AddSeconds(-1), RequestInfo.Body.applyMonth, RequestInfo.RelatedParty.Email, RequestInfo.RelatedParty.Tel, AuthorizedUser.ID, RequestInfo.RiskRealateParyList, "", businessNo, BK_SERIAL, applyPolicyNo, DateTime.Now, DateTime.Now, "", "");
				}

                //获取支付信息
                if (order != null)
                    req = PayInfoOperation.GetPayInfo($"{order.OrderCode}_0", AuthorizedUser);
                else
                {
                    req.ErrorMessage = "订单保存失败";
                    req.ReturnCode = "504";
                }
				Log.Write($"{id}_{RequestInfo.Body.InsuranceCompany}_C_Res.log", req.ToJson());
				return req;
			}
			else
			{
                req.ReturnCode = "505";
				req.ErrorMessage = ErrorMessage;
				Log.Write($"{id}_{RequestInfo.Body.InsuranceCompany}_C_Res.log", req.ToJson());
				return req;
			}
		}

		/// <summary>
		/// 诚泰批增批减
		/// </summary>
		/// <param name="req"></param>
		/// <returns></returns>
		[HttpPost]
		public DataJsonResult Update()
		{
			var req = new DataJsonResult();
            if (!IsAuth)
                return base.ErrorResult;
            GroupCorrectRequest RequestInfo;
			string requestString = new StreamReader(HttpContext.Current.Request.InputStream).ReadToEnd();
			try
			{
				RequestInfo = requestString.ToObject<GroupCorrectRequest>();
			}
			catch (Exception e)
			{
				Log.Write($"Error_CT_PZ_C_Req.log", $"[UserID:{AuthorizedUser.ID}]\r\n\r\n{requestString}");
				req.ReturnCode = "502";
				req.ErrorMessage = "提交报文转换出错" + e.Message;
				return req;
			}
           
			M_Order order = B_Order.Single(t => t.PolicyNo == RequestInfo.policyNo);
			if (order == null)
			{
				Log.Write($"Error_CT_PZ_C_Req.log", $"[UserID:{AuthorizedUser.ID}]\r\n\r\n{requestString}");
				req.ReturnCode = "503";
				req.ErrorMessage = "提交保单号出错：保单号不存在";
				return req;
			}
            //检查身份证号是否有重复
            string idNum = "";
		    var orderPersons = B_OrderPersion.GetList(x => x.OrderCode == order.OrderCode);
            foreach (var x in RequestInfo.InsuredUserList)
            {
                if (orderPersons.Select(m=>m.IdNum).ToList().Contains(x.identifyNo) || idNum == x.identifyNo)
                {
                    req.ReturnCode = "602";
                    req.ErrorMessage = $"身份证号：{x.identifyNo}有重复";
                    return req;
                }
                idNum = x.identifyNo;
            }
			string OrderCode = order.OrderCode;
		    var time = DateTime.Now.ToString("HHmmss");
		    var logName = $"{OrderCode}_{time}_{RequestInfo.InsuranceCompany}_PZ";
			Log.Write($"{logName}_C_Req.log", $"[UserID:{AuthorizedUser.ID}]\r\n\r\n{requestString}");

			if (StorageOperation.HasNoPayChildOrder(RequestInfo.policyNo))
			{
				req.ReturnCode = "504";
				req.ErrorMessage = "您有未支付的订单，请先支付或取消订单";
				Log.Write($"{logName}_C_Res.log", req.ToJson());
				return req;
			}
			if (RequestInfo.InsuranceCompany == "CT")
			{
				var serverReq = GroupCorrectOperation.ConvertToCtServerGroupCorrectRequest(RequestInfo);
				var result = GroupCorrectOperation.Post(logName, serverReq);
				if (result.Head.ResponseCode != "0000")
				{
					req.ReturnCode = "505";
					req.ErrorMessage = result.Head.ResponseMessage;
					Log.Write($"{logName}_C_Res.log", req.ToJson());
					return req;
				}

				string ChildCode = B_Order.CTUpdateOrder(OrderCode, RequestInfo.modifyFlag, result.Body.FtrNo, result.Body.RandomNo, (decimal)RequestInfo.sumPremium, RequestInfo.InsuredUserList, RequestInfo.rate);
				if (RequestInfo.modifyFlag == "I")
				{
					req = PayInfoOperation.GetPayInfo(ChildCode, AuthorizedUser);
				}
				Log.Write($"{logName}_C_Res.log", req.ToJson());
				return req;
			}
			req.ReturnCode = "506";
			req.ErrorMessage = "目前暂不支持诚泰以外的批增批减";
			Log.Write($"{logName}_C_Res.log", req.ToJson());
			return req;
		}

		[HttpGet]
		public string GetSHA256Psd(string customerName, string businessNo, string amount, string documentNo, bool toUpper = false)
		{
			try
			{
				var word = $"149{customerName}{businessNo}RMB{amount}{documentNo}P_MCLYUN";
				System.Security.Cryptography.SHA256CryptoServiceProvider SHA256CSP
					= new System.Security.Cryptography.SHA256CryptoServiceProvider();
				byte[] bytValue = System.Text.Encoding.UTF8.GetBytes(word);
				byte[] bytHash = SHA256CSP.ComputeHash(bytValue);
				SHA256CSP.Clear();
				//根据计算得到的Hash码翻译为SHA-1码
				string sHash = "", sTemp = "";
				for (int counter = 0; counter < bytHash.Count(); counter++)
				{
					long i = bytHash[counter] / 16;
					if (i > 9)
					{
						sTemp = ((char)(i - 10 + 0x41)).ToString();
					}
					else
					{
						sTemp = ((char)(i + 0x30)).ToString();
					}
					i = bytHash[counter] % 16;
					if (i > 9)
					{
						sTemp += ((char)(i - 10 + 0x41)).ToString();
					}
					else
					{
						sTemp += ((char)(i + 0x30)).ToString();
					}
					sHash += sTemp;
				}

				//根据大小写规则决定返回的字符串
				return toUpper ? sHash : sHash.ToLower();

			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

		[HttpGet]
		public string GetPayData(string childCode)
		{
			return "";
		}
	}
}
