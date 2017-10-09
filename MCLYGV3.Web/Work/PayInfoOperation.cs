using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using MCLYGV3.DB;
using MCLYGV3.Web.ClassLib;
using MCLYGV3.Web.Controllers;
using MCLYGV3.Web.Models;

namespace MCLYGV3.Web.Work
{
    public class PayInfoOperation
    {
        public static DataJsonResult GetPayInfo(string childCode, M_UserInfo AuthorizedUser)
        {
            var result = new DataJsonResult();
            M_OrderChild order = B_OrderChild.Single(x => x.ChildCode == childCode);
            if (order==null)
            {
                result.ReturnCode = "503";
                result.ErrorMessage = "不存在该子订单";
                return result;
            }
            if (B_Order.Single(x => x.OrderCode == order.OrderCode).UserId != AuthorizedUser.ID)
            {
                result.ErrorMessage = "只能支付自己的订单";
                return result;
            }
            M_Order ParentOrder = B_Order.Find(order.OrderCode);
            if (ParentOrder == null)
            {
                result.ReturnCode = "504";
                result.ErrorMessage = "不存在该订单";
                return result;
            }
            M_ChildPersion childpersion = B_ChildPersion.Single(t => t.ChildCode == childCode);
            if (ParentOrder.InsuranceCompany == "PA")
            {
                PayPAInfo papay = new PayPAInfo
                {
                    customerName = ParentOrder.InsuredName,
                    businessNo = order.BUSINESS_NO,
                    amount = (order.ModalPremium+order.Surcharge).ToString(),
                    documentNo = order.applyPolicyNo,
                    dataSource = "149",
                    currencyNo = "RMB",
                    insuredName = childpersion.RealName,
                    callBackURL = "www.baidu.com",
                    Url =
                        ConfigurationManager.AppSettings["PA_Base_URL"] +
                        "open/appsvr/property/financialPayments?access_token=" + Access_Token.GetAccessToken()
                };


                string signMsg = GetSHA256Psd(papay.customerName, papay.businessNo, papay.amount, papay.documentNo);
                papay.signMsg = signMsg;

                result.Data = papay;
            }
            else if (ParentOrder.InsuranceCompany == "CT")
            {
                var ctPay = new PayCTInfo
                {
                    Url = ConfigurationManager.AppSettings["CT_BaseUrl"] + "chac/YeePay",
                    OrderCoder = order.Ftrno,
                    foreEndURL = "www.baidu.com",
                    payAmt = order.ModalPremium + order.Surcharge,
                    payType = 2,
                    vhlType = 0,
                    randomno = order.EndorNo
                };
                result.Data = ctPay;
            }
            return result;
        }

        private static string GetSHA256Psd(string customerName, string businessNo, string amount, string documentNo, bool toUpper = false)
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
    }
    public class PayPAInfo
    {
        public string Url { get; set; }
        public string dataSource { get; set; }
        public string businessNo { get; set; }
        public string amount { get; set; }
        public string currencyNo { get; set; }
        public string customerName { get; set; }
        public string insuredName { get; set; }
        public string callBackURL { get; set; }
        public string documentNo { get; set; }
        public string signMsg { get; set; }


    }
    public class PayCTInfo
    {
        public string Url { get; set; }
        public string OrderCoder { get; set; }

        public int payType { get; set; } = 2;

        public decimal payAmt { get; set; }

        public int vhlType { get; set; } = 0;

        public string foreEndURL { get; set; }
        public string randomno { get; set; }
    }
}