using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml;
using MCLYGV3.Web.ClassLib;
using MCLYGV3.Web.Models;
using Newtonsoft.Json;

namespace MCLYGV3.Web.Work
{
	public class RequestOperation
	{
		/// <summary>
		/// 诚泰生成保单请求
		/// </summary>
		public static void CTAddRequest(ClientRequest RequestInfo, out string ftrno, out bool success, out string ErrorMessage)
		{
			ErrorMessage = "";
			ftrno = "";
			success = false;
			var result = "";
			var id = RequestInfo.Body.OrderCode;
			#region 请求诚泰保单生成接口
			CTServerRequest CTserverInfo = Transfer.CTConvert(RequestInfo);
			string serverInfo = XmlUtil.Serializer(CTserverInfo.GetType(), CTserverInfo);
			serverInfo = serverInfo.Replace("<?xml version=\"1.0\"?>", "");
			//serverInfo = serverInfo.Replace("xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"", "");
			//serverInfo = serverInfo.Replace("xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"", "");
			//serverInfo = serverInfo.Replace("<CTServerRequest  >", "");
			serverInfo = Regex.Replace(serverInfo, "<CTServerRequest((.|\n)*?)>", "");
			serverInfo = serverInfo.Replace("</CTServerRequest>", "");

			var path = AppDomain.CurrentDomain.BaseDirectory + "PageBrace\\CTModel.xml";
			StreamReader sr = new StreamReader(path);
			var xmlM = sr.ReadToEnd();
			serverInfo = xmlM.Replace("%DATA%", serverInfo);
			string url = ConfigurationManager.AppSettings["CT_BaseUrl"] + "cxf/acciSavePartyService";
			//请求报文写入日志
			Log.Write($"{id}_CT_S_Req.log", url+"\r\n"+serverInfo);
           
			#region 提交的报文
            /*
             * 正确的报文
             * 
<soap:Envelope xmlns:soap="http://schemas.xmlsoap.org/soap/envelope/" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">  
  <soap:Body> 
    <saveParty xmlns="http://saveProposal.accident.provider.app.thirdparty.echannel.ebiz.isoftstone.com/">  
      <saveParty xmlns="">  
        <Head> 
          <partnerCode>MCLY</partnerCode>  
          <passWord>MCLY20170103</passWord>  
          <queryId>89c5b8787e1b44f2a6aa34eb76e8d5f0</queryId>  
          <requestType>00001</requestType>  
          <userName>MCLY</userName> 
        </Head>  
        <Main> 
          <argueSolution>2</argueSolution>  
          <codInd>Y</codInd>  
          <commissionRatio>0.6132</commissionRatio>  
          <currency>CNY</currency>  
          <endDate>2018-09-09 00:00:00</endDate>  
          <operateDate>2017-09-08</operateDate>  
          <planCode>MCLY114401</planCode>  
          <shortRate>1.00</shortRate>  
          <startDate>2017-09-09 00:00:00</startDate>  
          <sumGrossPremium>480</sumGrossPremium>  
          <sumInsured>1200000</sumInsured> 
        </Main>  
        <RelatedParty> 
          <birthDate>0000-00-00</birthDate>  
          <businessSource/>  
          <contactName>济南铭辰利云</contactName>  
          <contactPhone>13888888888</contactPhone>  
          <officePhone>13888888888</officePhone>  
          <educationBackground></educationBackground>  
          <email/>  
          <identifyNumber>6789787989454626456</identifyNumber>  
          <identifyType>1</identifyType>  
          <insuredAddress/>  
          <insuredName>济南铭辰利云</insuredName>  
          <insuredType>2</insuredType>  
          <uWCount>1</uWCount> 
        </RelatedParty>  
        <PlanList> 
          <Plan> 
            <ItemAcci> 
              <insuredBusinessSource>0101001</insuredBusinessSource>  
              <quantity>6</quantity>  
              <rationType>N</rationType> 
            </ItemAcci>  
            <ItemKindList> 
              <ItemKind> 
                <grossPremium>80</grossPremium>  
                <kindCode>1144001</kindCode>  
                <kindInd>1</kindInd>  
                <rate>0.0004</rate>  
                <sumInsured>200000</sumInsured> 
              </ItemKind> 
            </ItemKindList>  
            <riskRealateParyList> 
              <riskRealatePary> 
                <appliPhone>13888888888</appliPhone>  
                <birthday>1996-10-04</birthday>  
                <clientCName>余凯瑞</clientCName>  
                <clientType>1</clientType>  
                <identifyNo>331023199610040310</identifyNo>  
                <identifyType>01</identifyType>  
                <insuredBusinessSource>0101001</insuredBusinessSource>  
                <postCode/>  
                <sex>2</sex>  
                <email/>  
                <homeAddress/>  
                <clientCNameBenefit>法定</clientCNameBenefit> 
              </riskRealatePary>  
              <riskRealatePary> 
                <appliPhone>13888888888</appliPhone>  
                <birthday>1977-03-24</birthday>  
                <clientCName>盛孝斌</clientCName>  
                <clientType>1</clientType>  
                <identifyNo>372421197703241996</identifyNo>  
                <identifyType>01</identifyType>  
                <insuredBusinessSource>0101001</insuredBusinessSource>  
                <postCode/>  
                <sex>2</sex>  
                <email/>  
                <homeAddress/>  
                <clientCNameBenefit>法定</clientCNameBenefit> 
              </riskRealatePary>  
              <riskRealatePary> 
                <appliPhone>13888888888</appliPhone>  
                <birthday>1976-12-29</birthday>  
                <clientCName>沈默</clientCName>  
                <clientType>1</clientType>  
                <identifyNo>510129197612291348</identifyNo>  
                <identifyType>01</identifyType>  
                <insuredBusinessSource>0101001</insuredBusinessSource>  
                <postCode/>  
                <sex>1</sex>  
                <email/>  
                <homeAddress/>  
                <clientCNameBenefit>法定</clientCNameBenefit> 
              </riskRealatePary>  
              <riskRealatePary> 
                <appliPhone>13888888888</appliPhone>  
                <birthday>1962-04-21</birthday>  
                <clientCName>方洁萍</clientCName>  
                <clientType>1</clientType>  
                <identifyNo>512533196204210354</identifyNo>  
                <identifyType>01</identifyType>  
                <insuredBusinessSource>0101001</insuredBusinessSource>  
                <postCode/>  
                <sex>2</sex>  
                <email/>  
                <homeAddress/>  
                <clientCNameBenefit>法定</clientCNameBenefit> 
              </riskRealatePary>  
              <riskRealatePary> 
                <appliPhone>13888888888</appliPhone>  
                <birthday>1961-03-07</birthday>  
                <clientCName>周至强</clientCName>  
                <clientType>1</clientType>  
                <identifyNo>120104196103074359</identifyNo>  
                <identifyType>01</identifyType>  
                <insuredBusinessSource>0101001</insuredBusinessSource>  
                <postCode/>  
                <sex>2</sex>  
                <email/>  
                <homeAddress/>  
                <clientCNameBenefit>法定</clientCNameBenefit> 
              </riskRealatePary>  
              <riskRealatePary> 
                <appliPhone>13888888888</appliPhone>  
                <birthday>1975-02-26</birthday>  
                <clientCName>刘盼盼</clientCName>  
                <clientType>1</clientType>  
                <identifyNo>120103197502264510</identifyNo>  
                <identifyType>01</identifyType>  
                <insuredBusinessSource>0101001</insuredBusinessSource>  
                <postCode/>  
                <sex>2</sex>  
                <email/>  
                <homeAddress/>  
                <clientCNameBenefit>法定</clientCNameBenefit> 
              </riskRealatePary> 
            </riskRealateParyList> 
          </Plan> 
        </PlanList> 
      </saveParty> 
    </saveParty> 
  </soap:Body> 
</soap:Envelope>
             */
            #endregion

            result = HttpOperation.Post(url, serverInfo);
			//响应结果报文写入日志
			Log.Write($"{id}_CT_S_Res.log", result);

			if (!string.IsNullOrWhiteSpace(result))
			{
                #region 返回的报文
                /*
               * 错误的报文
               * <soap:Envelope xmlns:soap="http://schemas.xmlsoap.org/soap/envelope/"><soap:Body><ns2:savePartyResponse xmlns:ns2="http://saveProposal.accident.provider.app.thirdparty.echannel.ebiz.isoftstone.com/"><return><responseBody><responseHead><errorMessage>总保费不等于毛保费之和</errorMessage><requestType>00001</requestType><responseCode>9014</responseCode></responseHead><return/></responseBody></return></ns2:savePartyResponse></soap:Body></soap:Envelope>
               * 
               * 正确的报文
               * <soap:Envelope xmlns:soap="http://schemas.xmlsoap.org/soap/envelope/"><soap:Body><ns2:savePartyResponse xmlns:ns2="http://saveProposal.accident.provider.app.thirdparty.echannel.ebiz.isoftstone.com/"><return><responseBody><responseHead><errorMessage>成功</errorMessage><requestType>00001</requestType><responseCode>0000</responseCode></responseHead><return><ftrno>01000170905018987</ftrno></return></responseBody></return></ns2:savePartyResponse></soap:Body></soap:Envelope>

              */
                #endregion

                string responseCode = Regex.Match(result, "<responseCode>(.+)</responseCode>").Value.Replace("<responseCode>", "").Replace("</responseCode>", "");
				if (responseCode == "0000")
				{
					ftrno = Regex.Match(result, "<ftrno>(.+)</ftrno>").Value.Replace("<ftrno>", "").Replace("</ftrno>", "");
					if (!string.IsNullOrWhiteSpace(ftrno))
						success = true;
					return;
				}
				else
				{
					//<errorMessage>总保费不等于毛保费之和</errorMessage>
					ErrorMessage = Regex.Match(result, "<errorMessage>(.+)</errorMessage>").Value.Replace("<errorMessage>", "").Replace("</errorMessage>", "");
					return;
				}
			}
			else
			{
				ErrorMessage = "保险公司服务器返回失败!";
			}
			return;
			#endregion
		}

		/// <summary>
		/// 平安生成保单请求
		/// </summary>
		public static void PAAddRequest(ClientRequest RequestInfo, out string businessNo, out string applyPolicyNo, out string BK_SERIAL, out bool success, out string ErrorMessage)
		{
			businessNo = "";
			applyPolicyNo = "";
			BK_SERIAL = "";
			success = false;
			ErrorMessage = "";

			var result = "";
			var id = RequestInfo.Body.OrderCode;
			#region 请求平安保单生成接口
			PAServerRequest ServerInfo = Transfer.Convert(RequestInfo);
			string serverInfo = JsonConvert.SerializeObject(ServerInfo);
			//请求报文写入日志
			string url = ConfigurationManager.AppSettings["PA_GroupAddUrl"] + Access_Token.GetAccessToken();
			Log.Write($"{id}_PA_S_Req.log", url + "\r\n\r\n" + serverInfo);

            #region 提交报文
            /* 
            * 正确的提交报文
            * {
   "TRAN_CODE": "000093", 
   "BANK_CODE": "P0010027", 
   "BRNO": "P0010027", 
   "TELLERNO": "P0010027", 
   "BK_ACCT_DATE": "20170908", 
   "BK_ACCT_TIME": "14:37:03", 
   "BK_SERIAL": "S20170908143703", 
   "BK_TRAN_CHNL": "WEB", 
   "REGION_CODE": "000000", 
   "PTP_CODE": "", 
   "ahsPolicy": {
       "policyBaseInfo": {
           "totalModalPremium": "150", 
           "alterableSpecialPromise": "", 
           "insuranceBeginTime": "2017-09-09 00:00:00", 
           "insuranceEndTime": "2018-09-08 23:59:59", 
           "currecyCode": "01", 
           "applyDay": "", 
           "relationshipWithInsured": "H", 
           "businessType": "2", 
           "applyMonth": "12", 
           "applyPersonnelNum": "6"
       }, 
       "policyExtendInfo": {
           "partnerName": "P_MCLYUN", 
           "partnerSystemSeriesNo": "PA201709080236571111", 
           "isSendInvoice": "2", 
           "invokeMobilePhone": "", 
           "invokeEmail": "4405307@qq.com"
       }, 
       "insuranceApplicantInfo": {
           "groupPersonnelInfo": {
               "groupName": "济南铭辰利云", 
               "groupCertificateNo": "6789787989454626456", 
               "groupCertificateType": "04", 
               "industryCode": "", 
               "postCode": "000000", 
               "businessRegionType": "", 
               "companyAttribute": "03", 
               "groupAbbr": "", 
               "businessRegisterId": "6789787989454626456", 
               "address": "", 
               "phoneExchangeArea": "", 
               "phoneExchange": "", 
               "linkManName": "", 
               "linkManSexCode": "", 
               "linkManEmail": "1015061464@qq.com", 
               "linkManMobileTelephone": "", 
               "bankCode": "", 
               "bankAccount": ""
           }
       }, 
       "subjectInfo": [
           {
               "subjectInfo": {
                   "totalModalPremium": "150", 
                   "subjectName": "层级1", 
                   "applyPersonnelNum": "6", 
                   "planInfo": [
                       {
                           "planInfo": {
                               "planCode": "Y502", 
                               "applyNum": "1", 
                               "totalModalPremium": "25", 
                               "applyMonth": "12", 
                               "applyDay": "", 
                               "dutyInfo": [
                                   {
                                       "dutyInfo": {
                                           "dutyCode": "YA01", 
                                           "totalModalPremium": "25", 
                                           "dutyAount": "50000"
                                       }
                                   }
                               ]
                           }
                       }
                   ], 
                   "insurantInfo": [
                       {
                           "insurantInfo": {
                               "personnelAttribute": "100", 
                               "virtualInsuredNum": "", 
                               "personnelName": "余凯瑞", 
                               "personnelAge": "21", 
                               "mobileTelephone": "", 
                               "sexCode": "M", 
                               "birthday": "1996-10-04", 
                               "email": "", 
                               "familyNameSpell": "", 
                               "firstNameSpell": "", 
                               "certificateNo": "331023199610040310", 
                               "certificateType": "01", 
                               "professionGradeCode": "1", 
                               "professionCode": "0101001"
                           }
                       }, 
                       {
                           "insurantInfo": {
                               "personnelAttribute": "100", 
                               "virtualInsuredNum": "", 
                               "personnelName": "盛孝斌", 
                               "personnelAge": "40", 
                               "mobileTelephone": "", 
                               "sexCode": "M", 
                               "birthday": "1977-03-24", 
                               "email": "", 
                               "familyNameSpell": "", 
                               "firstNameSpell": "", 
                               "certificateNo": "372421197703241996", 
                               "certificateType": "01", 
                               "professionGradeCode": "1", 
                               "professionCode": "0101001"
                           }
                       }, 
                       {
                           "insurantInfo": {
                               "personnelAttribute": "100", 
                               "virtualInsuredNum": "", 
                               "personnelName": "沈默", 
                               "personnelAge": "41", 
                               "mobileTelephone": "", 
                               "sexCode": "F", 
                               "birthday": "1976-12-29", 
                               "email": "", 
                               "familyNameSpell": "", 
                               "firstNameSpell": "", 
                               "certificateNo": "510129197612291348", 
                               "certificateType": "01", 
                               "professionGradeCode": "1", 
                               "professionCode": "0101001"
                           }
                       }, 
                       {
                           "insurantInfo": {
                               "personnelAttribute": "100", 
                               "virtualInsuredNum": "", 
                               "personnelName": "方洁萍", 
                               "personnelAge": "55", 
                               "mobileTelephone": "", 
                               "sexCode": "M", 
                               "birthday": "1962-04-21", 
                               "email": "", 
                               "familyNameSpell": "", 
                               "firstNameSpell": "", 
                               "certificateNo": "512533196204210354", 
                               "certificateType": "01", 
                               "professionGradeCode": "1", 
                               "professionCode": "0101001"
                           }
                       }, 
                       {
                           "insurantInfo": {
                               "personnelAttribute": "100", 
                               "virtualInsuredNum": "", 
                               "personnelName": "周至强", 
                               "personnelAge": "56", 
                               "mobileTelephone": "", 
                               "sexCode": "M", 
                               "birthday": "1961-03-07", 
                               "email": "", 
                               "familyNameSpell": "", 
                               "firstNameSpell": "", 
                               "certificateNo": "120104196103074359", 
                               "certificateType": "01", 
                               "professionGradeCode": "1", 
                               "professionCode": "0101001"
                           }
                       }, 
                       {
                           "insurantInfo": {
                               "personnelAttribute": "100", 
                               "virtualInsuredNum": "", 
                               "personnelName": "刘盼盼", 
                               "personnelAge": "42", 
                               "mobileTelephone": "", 
                               "sexCode": "M", 
                               "birthday": "1975-02-26", 
                               "email": "", 
                               "familyNameSpell": "", 
                               "firstNameSpell": "", 
                               "certificateNo": "120103197502264510", 
                               "certificateType": "01", 
                               "professionGradeCode": "1", 
                               "professionCode": "0101001"
                           }
                       }
                   ]
               }
           }
       ]
   }
}
            */
            #endregion

            result = HttpOperation.Post(url, serverInfo);
			//响应结果报文写入日志
			Log.Write($"{id}_PA_S_Res.log", url + "\r\n\r\n" + result);

            #region 返回的报文
            /*
           * 成功的
           * {"ret":"0","msg":"","requestId":"null","data":"{"PA_RSLT_MESG":"生成投保单成功","PA_RSLT_CODE":"999999","BUSINESS_NO":"14010019000143177973","AMOUNT":"623.1","resultMessage":"生成投保单成功","PA_ACCT_TIME":"18:42:46","BK_SERIAL":"S20170905184407","FT_SERIAL":"","resultCode":"999999","applyPolicyInfo":{"offLinePolicyNo":"","noticeNo":"14010019000143177973","coinsDutyProportion":"1","coinsuranceTotalPremium":"0","validateCode":"","isOffLine":"","isFullCoinPremium":"","applyPolicyNo":"51400001900310060489","isGiftInsurance":"","totalPremium":"623.1","invoiceUrl":""},"BANK_CODE":"P0010027","TRAN_CODE":"000093","PTP_CODE":"","REGION_CODE":"000000","PA_ACCT_DATE":"20170905"}"}
           *
           * 失败的 
           * {"ret":"0","msg":"","requestId":"null","data":"{"BK_SERIAL":"S20170905184037","PA_RSLT_MESG":"自然人保单：不能重复投保！;","FT_SERIAL":"","PA_RSLT_CODE":"800001","resultCode":"800001","BANK_CODE":"P0010027","TRAN_CODE":"000093","PTP_CODE":"","REGION_CODE":"000000","resultMessage":"自然人保单：不能重复投保！;","PA_ACCT_TIME":"18:39:14","PA_ACCT_DATE":"20170905"}"}
           * 
          */
            #endregion


            var dataStr = Regex.Match(result, "\"data\":\"(.*)\"").Value;
			if (!string.IsNullOrWhiteSpace(dataStr))
			{
				dataStr = dataStr.Substring(0, dataStr.Length - 1);
				dataStr = dataStr.Substring(8, dataStr.Length - 8);
				try
				{
					PAResponseData data = JsonConvert.DeserializeObject<PAResponseData>(dataStr);
					if (data.resultCode == "999999")
					{
						businessNo = data.BUSINESS_NO;           //订单号
						applyPolicyNo = data.applyPolicyInfo.applyPolicyNo;      //保单号
						BK_SERIAL = data.BK_SERIAL;
						success = true;
						return;
					}
					else
					{
						ErrorMessage = data.PA_RSLT_MESG;
						return;
					}
				}
				catch (Exception)
				{
					ErrorMessage = "保险公司服务器返回报文失败：";
					return;
				}
			}
			else
			{
				ErrorMessage = "保险公司服务器返回失败：";
				return;
			}

			#endregion

		}
	}
}