using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http;
using MCLYGV3.DB;
using System.Web.Http.Cors;

namespace MCLYGV3.Web.Controllers
{
	[EnableCors("*", "*", "*")]
	public class CallbackController : ApiController
    {
        // GET: Callback
        Random r;

        [HttpPost]
        public string CTPayCallBack()
        {

            string requestString = new StreamReader(HttpContext.Current.Request.InputStream).ReadToEnd();
            r = new Random();
            string logid = DateTime.Now.ToString("yyyyMMddHHmmss") + r.Next(1000, 9999);
            Log.Write(logid + ".CT.PayCallBack.Res.log", requestString);
            //var ctPayRequest = new CTPayCallBackRequest(requestString);
            var ctPayRequest = new CTPayCallBackRequest()
            {
                orderid = HttpContext.Current.Request.Form["orderid"],
                paystatus = HttpContext.Current.Request.Form["paystatus"],
                status = HttpContext.Current.Request.Form["status"],
                policyNo = HttpContext.Current.Request.Form["policyNo"],
                encryptString = HttpContext.Current.Request.Form["encryptString"],
                endorNo = HttpContext.Current.Request.Form["endorNo"],
            };
            if (ctPayRequest.paystatus == "SUCCESS" && ctPayRequest.status == "SUCCESS")
            {
                var req = new CallBackRequest()
                {
                    EncryptString = ctPayRequest.encryptString,
                    OrderCode = ctPayRequest.orderid,
                    EndorNo = ctPayRequest.endorNo,
                    PolicyNo = ctPayRequest.policyNo,
                    InsuranceCompany = "CT"
                };
                if (string.IsNullOrWhiteSpace(req.EndorNo))
                {
                    return B_Order.UpdateAddOrder(req);
                }
                else
                {
                    return B_Order.UpdateOrder(req);
                }
            }
            return "fail";
        }

        [HttpPost]
        public string PAPayCallBack()
        {
            r = new Random();
            string logid = DateTime.Now.ToString("yyyyMMddHHmmss") + r.Next(1000, 9999);
            string requestString = new StreamReader(HttpContext.Current.Request.InputStream).ReadToEnd();
            Log.Write(logid + ".PA.PayCallBack.Res.log", requestString);
            requestString = requestString.Replace("\r\n", "");
            if (requestString.Contains("交易成功"))
            {
                CallBackRes obj = requestString.ToObject<CallBackRes>();
                var req = new CallBackRequest()
                {
                    PolicyNo = obj.policyNo,
                    BusinessNo = obj.businessNo,
                    EncryptString = obj.validateCode,
                    InsuranceCompany = "PA"
                };
                return B_Order.UpdateAddOrder(req);
            }
            return "fail";
        }

        
    }

    public class CTPayCallBackRequest
    {
        public string paystatus { get; set; }
        public string status { get; set; }
        public string policyNo { get; set; }
        public string orderid { get; set; }
        public string encryptString { get; set; }
        public string endorNo { get; set; }

        //public CTPayCallBackRequest(string str)
        //{
        //    var strs = str.Split('&');
        //    foreach (var x in strs)
        //    {
        //        if (x.Split('=')[0]== "paystatus")
        //        {
        //            paystatus = x.Split('=')[1];
        //        }
        //        if (x.Split('=')[0] == "status")
        //        {
        //            status = x.Split('=')[1];
        //        }
        //        if (x.Split('=')[0] == "policyNo")
        //        {
        //            policyNo = x.Split('=')[1];
        //        }
        //        if (x.Split('=')[0] == "orderid")
        //        {
        //            orderid = x.Split('=')[1];
        //        }
        //        if (x.Split('=')[0] == "encryptString")
        //        {
        //            encryptString = x.Split('=')[1];
        //        }
        //        if (x.Split('=')[0] == "endorNo")
        //        {
        //            endorNo = x.Split('=')[1];
        //        }

        //    }
        //}
    }
}