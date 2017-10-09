using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Globalization;
using MCLYGV3.Web.Models;

namespace MCLYGV3.Web.ClassLib
{
    public class Transfer
    {
        public static PAServerRequest Convert(ClientRequest Cinfo)
        {
            PAServerRequest req = new PAServerRequest();
            req.TRAN_CODE = "000093";
            req.BANK_CODE = "P0010027";
            req.BRNO = "P0010027";
            req.TELLERNO = "P0010027";
            req.BK_ACCT_DATE = DateTime.Now.ToString("yyyyMMdd");
            req.BK_ACCT_TIME = DateTime.Now.ToString("HH:mm:ss");
            req.BK_SERIAL = "S" + DateTime.Now.ToString("yyyyMMddHHmmss");
            req.BK_TRAN_CHNL = "WEB";
            req.REGION_CODE = "000000";
            req.PTP_CODE = "";

            req.ahsPolicy = new AhsPolicy();

            #region 基本信息
            PolicyBaseInfo pbase = new PolicyBaseInfo();
            pbase.alterableSpecialPromise = "";
            pbase.insuranceBeginTime = Cinfo.Body.StartDate.ToString("yyyy-MM-dd") + " 00:00:00";
            pbase.insuranceEndTime = Cinfo.Body.StartDate.AddMonths(Cinfo.Body.applyMonth).AddDays(-1).ToString("yyyy-MM-dd") + " 23:59:59";
            pbase.currecyCode = "01";//人民币
            pbase.applyDay = "";
            pbase.applyMonth = Cinfo.Body.applyMonth.ToString();
            pbase.relationshipWithInsured = "H";
            pbase.businessType = "2";
            req.ahsPolicy.policyBaseInfo = pbase;
            #endregion

            #region 扩展信息
            PolicyExtendInfo pExt = new PolicyExtendInfo();
            pExt.partnerName = "P_MCLYUN";
            pExt.partnerSystemSeriesNo = Cinfo.Body.OrderCode;
            pExt.isSendInvoice = "2";
            pExt.invokeMobilePhone = "";
            pExt.invokeEmail = "4405307@qq.com";
            req.ahsPolicy.policyExtendInfo = pExt;
            #endregion

            #region 投保人信息

            InsuranceApplicantInfo iInfo = new InsuranceApplicantInfo();
            iInfo.groupPersonnelInfo = new GroupPersonnelInfo();
            iInfo.groupPersonnelInfo.groupName = Cinfo.RelatedParty.InsuredName;
            iInfo.groupPersonnelInfo.groupCertificateNo = Cinfo.RelatedParty.IdentifyNumber;
            iInfo.groupPersonnelInfo.groupCertificateType = "04";
            iInfo.groupPersonnelInfo.industryCode = "";
            iInfo.groupPersonnelInfo.postCode = "000000";
            iInfo.groupPersonnelInfo.businessRegionType = "";
            iInfo.groupPersonnelInfo.companyAttribute = "03";
            iInfo.groupPersonnelInfo.groupAbbr = "";
            iInfo.groupPersonnelInfo.businessRegisterId = Cinfo.RelatedParty.IdentifyNumber;
            iInfo.groupPersonnelInfo.address = "";
            iInfo.groupPersonnelInfo.phoneExchangeArea = "";
            iInfo.groupPersonnelInfo.phoneExchange = "";
            iInfo.groupPersonnelInfo.linkManName = "";
            iInfo.groupPersonnelInfo.linkManSexCode = "";
            iInfo.groupPersonnelInfo.linkManEmail = "4405307@qq.com";
            if (!string.IsNullOrWhiteSpace(Cinfo.RelatedParty.Email))
            {
                iInfo.groupPersonnelInfo.linkManEmail = Cinfo.RelatedParty.Email;
            }
            iInfo.groupPersonnelInfo.linkManMobileTelephone = "";
            iInfo.groupPersonnelInfo.bankCode = "";
            iInfo.groupPersonnelInfo.bankAccount = "";
            req.ahsPolicy.insuranceApplicantInfo = iInfo;
            #endregion

            #region 投保信息
            req.ahsPolicy.subjectInfo = new List<SubjectInfoItem>();
            int LvNum = 1;
            foreach (var MyRiskRealatePary in Cinfo.RiskRealateParyList)
            {
                //整个投保计划，可以多组

                SubjectInfoItem sInfoItem = new SubjectInfoItem();
                sInfoItem.subjectInfo = new SubjectInfo();


                #region 被保险人信息
                sInfoItem.subjectInfo.insurantInfo = new List<InsurantInfoItem>();
                foreach (var Insurant in MyRiskRealatePary.InsurantList)
                {
                    Idno idno = new Idno(Insurant.IdNum);
                    InsurantInfoItem iInfoItem = new InsurantInfoItem();
                    iInfoItem.insurantInfo = new InsurantInfo();
                    iInfoItem.insurantInfo.personnelAttribute = "100";
                    iInfoItem.insurantInfo.virtualInsuredNum = "";
                    iInfoItem.insurantInfo.personnelName = Insurant.Name;
                    iInfoItem.insurantInfo.mobileTelephone = "";
                    iInfoItem.insurantInfo.personnelAge = idno.getAge().ToString();
                    iInfoItem.insurantInfo.sexCode = idno.getSex();
                    iInfoItem.insurantInfo.birthday = idno.getBirthday();
                    iInfoItem.insurantInfo.email = "";
                    iInfoItem.insurantInfo.familyNameSpell = "";
                    iInfoItem.insurantInfo.firstNameSpell = "";
                    iInfoItem.insurantInfo.certificateNo = Insurant.IdNum;
                    iInfoItem.insurantInfo.certificateType = "01";
                    iInfoItem.insurantInfo.professionGradeCode = "1";
                    iInfoItem.insurantInfo.professionCode = Insurant.ProfessionCode;
                    sInfoItem.subjectInfo.insurantInfo.Add(iInfoItem);
                }
                #endregion

                sInfoItem.subjectInfo.planInfo = new List<PlanInfoItem>();
                PlanInfoItem pInfoItem;
                DutyInfoItem duty;

                #region 投保计划

                foreach (var plan in MyRiskRealatePary.PlanList)
                {
                    pInfoItem = new PlanInfoItem();
                    pInfoItem.planInfo = new PlanInfo();
                    pInfoItem.planInfo.planCode = plan.planCode;
                    pInfoItem.planInfo.applyNum = "1";
                    pInfoItem.planInfo.totalModalPremium = plan.ModalPremium.ToString();/////////////////////////
                    pInfoItem.planInfo.applyMonth = Cinfo.Body.applyMonth.ToString();
                    pInfoItem.planInfo.applyDay = "";

                    pInfoItem.planInfo.dutyInfo = new List<DutyInfoItem>();
                    duty = new DutyInfoItem();
                    duty.dutyInfo = new DutyInfo();
                    duty.dutyInfo.dutyCode = plan.dutyCode;
                    duty.dutyInfo.dutyAount = plan.dutyAount.ToString();
                    duty.dutyInfo.totalModalPremium = plan.ModalPremium.ToString();
                    pInfoItem.planInfo.dutyInfo.Add(duty);
                    sInfoItem.subjectInfo.planInfo.Add(pInfoItem);
                }
                #endregion

                double Premium1 = 0;
                foreach (var item in sInfoItem.subjectInfo.planInfo)
                {
                    Premium1 = Premium1 + double.Parse(item.planInfo.totalModalPremium);
                }
                sInfoItem.subjectInfo.totalModalPremium = (Premium1 * sInfoItem.subjectInfo.insurantInfo.Count).ToString();
                sInfoItem.subjectInfo.subjectName = "层级" + LvNum;
                sInfoItem.subjectInfo.applyPersonnelNum = sInfoItem.subjectInfo.insurantInfo.Count.ToString();
                req.ahsPolicy.subjectInfo.Add(sInfoItem);

                LvNum++;
            }
            #endregion


            double Premium = 0;
            int count = 0;
            Premium = 0;
            count = 0;
            foreach (var item in req.ahsPolicy.subjectInfo)
            {
                Premium = Premium + double.Parse(item.subjectInfo.totalModalPremium);
                count = count + item.subjectInfo.insurantInfo.Count;
            }

            pbase.totalModalPremium = ((decimal)Premium+ Cinfo.Body.Surcharge).ToString();
            pbase.applyPersonnelNum = count.ToString();



            return req;
        }
		 
        public static CTServerRequest CTConvert(ClientRequest Cinfo)
        {
            var totalBF = 0D;
            var totalBE = 0D;
            var req = new CTServerRequest();
            req.Head = new Head();
            req.Head.partnerCode = "MCLY";
            req.Head.passWord = "MCLY20170103";
            req.Head.queryId = Guid.NewGuid().ToString().Replace("-", "");
            req.Head.requestType = "00001";
            req.Head.userName = "MCLY";

            req.Main = new Main();
            req.Main.argueSolution = "2";
            req.Main.codInd = "Y";
            req.Main.commissionRatio = "0.6132";
            req.Main.currency = "CNY";
            req.Main.endDate = Cinfo.Body.StartDate.AddMonths(Cinfo.Body.applyMonth).AddDays(-1).ToString("yyyy-MM-dd 23:59:59");
            req.Main.operateDate = DateTime.Now.ToString("yyyy-MM-dd");
            req.Main.planCode = "MCLY114401";
            req.Main.shortRate = GetShortRate(Cinfo.Body.applyMonth);
            req.Main.startDate = Cinfo.Body.StartDate.ToString("yyyy-MM-dd 00:00:00");


            var idNo = new Idno(Cinfo.RelatedParty.IdentifyNumber);
            req.RelatedParty = new RelatedParty();
            req.RelatedParty.birthDate = idNo.getBirthday();
            req.RelatedParty.businessSource = "";
            req.RelatedParty.contactName = Cinfo.RelatedParty.InsuredName;
            req.RelatedParty.contactPhone = "13888888888";
            req.RelatedParty.officePhone = "13888888888";
            req.RelatedParty.educationBackground = " ";
            req.RelatedParty.email = "";
            req.RelatedParty.identifyNumber = Cinfo.RelatedParty.IdentifyNumber;
            req.RelatedParty.identifyType = "1";
            req.RelatedParty.insuredAddress = "";
            req.RelatedParty.insuredName = Cinfo.RelatedParty.InsuredName;
            req.RelatedParty.insuredType = "2";
            req.RelatedParty.uWCount = "1";

            req.PlanList = new List<Plan>();

		




			foreach (var item in Cinfo.RiskRealateParyList)
            {
                var plan = new Plan();
                plan.ItemKindList = new List<ItemKind>();
                plan.riskRealateParyList = new List<riskRealatePary>();
                foreach (var x in item.InsurantList)
                {
                    var id = new Idno(x.IdNum);
                    plan.riskRealateParyList.Add(new riskRealatePary()
                    {
                        identifyType = "01",
                        email = "",
                        appliPhone = "13888888888",
                        birthday = id.getBirthday(),
                        clientCName = x.Name,
                        clientCNameBenefit = "法定",
                        clientType = "1",
                        homeAddress = "",
                        identifyNo = x.IdNum,
                        insuredBusinessSource = x.ProfessionCode,
                        postCode = "",
                        sex = id.getSex2() 
                    });
                }

				

                foreach (var x in item.PlanList)
                {
					double 全年保费 = x.ModalPremium;
					double 短期费率 = CTGroupInsInfo.bTimeModulusList[Cinfo.Body.applyMonth];
					if (短期费率 != 0)
						全年保费 = 全年保费 / 短期费率;




					plan.ItemKindList.Add(new ItemKind()
                    {
                        sumInsured = x.dutyAount,
                        grossPremium = x.ModalPremium,
                        kindCode = x.planCode,
                        kindInd = x.planCode == "1144001" ? "1" : "2",
                        rate = Math.Round(全年保费 / x.dutyAount, 4)
                    });
                    totalBF += x.ModalPremium * plan.riskRealateParyList.Count;
                    totalBE += x.dutyAount * plan.riskRealateParyList.Count;
                }
                plan.ItemAcci = new ItemAcci()
                {
                    insuredBusinessSource = item.InsurantList.FirstOrDefault()?.ProfessionCode,
                    quantity = plan.riskRealateParyList.Count.ToString(),
                    rationType = "N"
                };

                req.Main.sumGrossPremium = totalBF.ToString();
                req.Main.sumInsured = totalBE.ToString();
                req.PlanList.Add(plan);
            }
            return req;
        }

        /// <summary>
        /// 根据保险有效期得到短期费率
        /// </summary>
        /// <param name="month"></param>
        /// <returns></returns>
	    public static string GetShortRate(int month)
        {
            var shortRote = "";
            switch (month)
            {
                case 1:
                    shortRote = "0.25";
                    break;
                case 2:
                    shortRote = "0.35";
                    break;
                case 3:
                    shortRote = "0.45";
                    break;
                case 4:
                    shortRote = "0.55";
                    break;
                case 5:
                    shortRote = "0.65";
                    break;
                case 6:
                    shortRote = "0.70";
                    break;
                case 7:
                    shortRote = "0.75";
                    break;
                case 8:
                    shortRote = "0.80";
                    break;
                case 9:
                    shortRote = "0.85";
                    break;
                case 10:
                    shortRote = "0.90";
                    break;
                case 11:
                    shortRote = "0.95";
                    break;
                case 12:
                    shortRote = "1.00";
                    break;
            }
            return shortRote;
        }
    }
}