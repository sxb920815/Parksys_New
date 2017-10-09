using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using Aspose.Cells;
using MCLYGV3.DB;
using MCLYGV3.Web.ClassLib;
using MCLYGV3.Web.Models;
using Newtonsoft.Json;

namespace MCLYGV3.Web.Controllers
{
    [EnableCors("*", "*", "*")]
    public class DownLoadController : ApiController
    {
        [HttpPost]
        public DataJsonResult Template()
        {
            var id = DateTime.Now.ToString("yyyyMMddHHmmss");
            var result=new DataJsonResult();
            string req = new StreamReader(System.Web.HttpContext.Current.Request.InputStream).ReadToEnd();
            Log.OtherWrite($"{id}_C_DownLoad_Template_Request.log",req);
            if (string.IsNullOrWhiteSpace(req))
            {
                result.ReturnCode = "502";
                result.ErrorMessage = "请求参数无效";
                return result;
            }
            #region 生成模板
            DownloadInfo obj = JsonConvert.DeserializeObject<DownloadInfo>(req);
            var path = AppDomain.CurrentDomain.BaseDirectory;
            Workbook wb = new Workbook($"{path}/PageBrace/Template.xls");
            if (obj.Type == 1)
            {
                wb = new Workbook($"{path}/PageBrace/TemplateAdd.xls");
            }
            Cells cell = wb.Worksheets[0].Cells;
            int index = 1;
            foreach (DownloadInfo_item c in obj.information)
            {
                for (int i = 0; i < c.numberOfPerson; i++)
                {
                    if (index > 1)
                    {
                        cell.InsertRow(index);
                        cell.CopyRow(cell, index, index);
                    }
                    cell[index, 0].PutValue(index);
                    cell[index, 3].PutValue(c.occupationCode);
                    cell[index, 4].PutValue(c.selhurt);
                    cell[index, 5].PutValue(c.selmedical);
                    cell[index, 6].PutValue(c.selhospital / 180);
                    if (obj.Type == 1)
                    {
                        cell[index, 7].PutValue(c.insurancePeriod);
                    }
                    index++;
                }
            }
            #endregion
            string filename = DateTime.Now.ToString("yyyyMMddHHmmssffff") + ".xls";
            wb.Save($"{path}/TmpDownload/{filename}");
            result.Data=$"/TmpDownload/{filename}";
            Log.OtherWrite($"{id}_C_DownLoad_Template_Response.log", result.ToJson());
            return result;
        }

        [HttpGet]
        public DataJsonResult ElectronicPolicy(string orderCode)
        {
            var result=new DataJsonResult();
            var order = B_Order.Single(x => x.OrderCode == orderCode);
            if (order==null)
            {
                result.ReturnCode = "601";
                result.ErrorMessage = "订单编号错误";
                return result;
            }
            var curTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            var str = $"P0010027{order.PolicyNo}{order.encryptString}group{curTime}";
            var psd = EncryptionHelp.Hash_SHA_256(str);
            var url = "";
            if (order.InsuranceCompany == "PA")
            {
                url = "http://epcis-ptp-dmzstg1.pingan.com.cn:9080/epcis.ptp.partner.getAhsEPolicyPDFNotWithCert.do" +
                      $"?umCode=P0010027&policyNo={order.PolicyNo}&validateCode={order.encryptString}&isSeperated=group&curTime={curTime}&cipherText={psd}";
                //return File(Post(url, ""), "application/pdf", $"{orderCode}.pdf");
            }
            else if (order.InsuranceCompany == "CT")
            {
                //正式地址
                //url = $"http://echac.champion-ic.com/new/policy/download/partnerCode/MCLY/policyNO/"+ order.encryptString;

                //测试地址
                url = ConfigurationManager.AppSettings["CT_BaseUrl"] + "new/policy/download/partnerCode/MCLY/policyNO/" + order.encryptString; 

            }
            result.Data = url;
            return result;
        }

        public static Stream Post(string Url, string data)
        {
            HttpWebRequest httpRequest = (HttpWebRequest)WebRequest.Create(Url);
            httpRequest.CookieContainer = new CookieContainer();
            httpRequest.Method = "POST"; //POST 提交
            httpRequest.KeepAlive = true;
            httpRequest.UserAgent =
                "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/43.0.2357.81 Safari/537.36";
            httpRequest.Accept = "application/xml, text/plain, */*";
            httpRequest.ContentType = "application/json;charset=UTF-8";
            byte[] bytes = Encoding.UTF8.GetBytes(data);
            httpRequest.ContentLength = bytes.Length;
            try
            {
                Stream stream = httpRequest.GetRequestStream();
                stream.Write(bytes, 0, bytes.Length);
                stream.Close(); //以上是POST数据的写入
                HttpWebResponse httpResponse = (HttpWebResponse)httpRequest.GetResponse();
                return httpResponse.GetResponseStream();

            }
            catch (Exception e)
            {
                //Log.Write("System.Log", "出错：" + e.Message);
                return Stream.Null;
            }
        }
    }

    public class DownloadInfo
    {
        public DownloadInfo_item[] information { get; set; }

        public string InsuranceCompany { get; set; }
        /// <summary>
        /// 1:新增 2：批增
        /// </summary>
        public int Type { get; set; }
    }

    public class DownloadInfo_item
    {
        public int numberOfPerson { get; set; }
        public int insurancePeriod { get; set; }
        public string occupationType { get; set; }
        public string occupationCode { get; set; }
        public int selhurt { get; set; }
        public int selmedical { get; set; }
        public int selhospital { get; set; }
        public string personMoney { get; set; }
        public decimal planMoney { get; set; }
    }
}