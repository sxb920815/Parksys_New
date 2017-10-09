using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace MCLYGV3.Web.Models
{
	/// <summary>
	/// 服务器端的请求类
	/// </summary>
	/// 
	public class PAServerRequest
	{
		/// <summary>
		/// 交易码: 000093"
		/// </summary>
		public string TRAN_CODE { get; set; }
		/// <summary>
		/// 平安分配，默认为P0010027
		/// </summary>
		public string BANK_CODE { get; set; }
		/// <summary>
		/// （非空）合作伙伴网点号，平安分配，我们用P0010027
		/// </summary>
		public string BRNO { get; set; }
		/// <summary>
		/// （可空）商户号，由合作伙伴分配如果没有参考BRNO。我们用P0010027
		/// </summary>
		public string TELLERNO { get; set; }
		/// <summary>
		/// （非空）交易日期，格式：yyyyMMdd ",
		/// </summary>
		public string BK_ACCT_DATE { get; set; }
		/// <summary>
		/// （非空）交易时间,格式：HH:mm:ss 
		/// </summary>
		public string BK_ACCT_TIME { get; set; }
		/// <summary>
		/// （非空）交易流水号，不可重复,最长20字节"
		/// </summary>
		public string BK_SERIAL { get; set; }
		/// <summary>
		/// 连接方式，默认为WEB
		/// </summary>
		public string BK_TRAN_CHNL { get; set; }
		/// <summary>
		/// （非空）地区码没有可用000000代替。
		/// </summary>
		public string REGION_CODE { get; set; }
		/// <summary>
		/// (可空)区分同个合作伙伴不同的出单账号
		/// </summary>
		public string PTP_CODE { get; set; }
		/// <summary>
		/// 意健险保险信息
		/// </summary>
		public AhsPolicy ahsPolicy { get; set; }
	}
	public class AhsPolicy
	{
		/// <summary>
		/// 报单基本信息
		/// </summary>
		public PolicyBaseInfo policyBaseInfo { get; set; }
		/// <summary>
		/// 报单扩展信息
		/// </summary>
		public PolicyExtendInfo policyExtendInfo { get; set; }
		public InsuranceApplicantInfo insuranceApplicantInfo { get; set; }
		public List<SubjectInfoItem> subjectInfo { get; set; }

	}
	public class SubjectInfoItem
	{
		public SubjectInfo subjectInfo { get; set; }
	}
	/// <summary>
	/// (非空）保单基本信息
	/// </summary>
	public class PolicyBaseInfo
	{
		/// <summary>
		/// （非空）保单实际保费合计"
		/// </summary>
		public string totalModalPremium { get; set; }
		/// <summary>
		/// （可空）可改特别约定(最长1000字节) 
		/// </summary>
		public string alterableSpecialPromise { get; set; }
		/// <summary>
		/// （非空）保险起期，格式yyyy-MM-dd HH:mm:ss",
		/// </summary>
		public string insuranceBeginTime { get; set; }
		/// <summary>
		/// (非空) 保险止期，格式yyyy-MM-dd HH:mm:ss"
		/// </summary>
		public string insuranceEndTime { get; set; }
		/// <summary>
		/// " (非空) 币种。人民币请使用01"
		/// </summary>
		public string currecyCode { get; set; }
		/// <summary>
		/// (月数与天数其中一项非空) 保险期限（日）
		/// </summary>
		public string applyDay { get; set; }
		/// <summary>
		/// （非空）与被保人关系。1:本人 2:配偶 3 :父子 4:父女 5:受益人 6:被保人 7:投保人 A:母子 B:母女 C:兄弟 D:姐弟 G:祖孙 H:雇佣 I:子女 9:其他 8:转换不详
		/// </summary>
		public string relationshipWithInsured { get; set; }
		/// <summary>
		/// 业务类型。当投保人是个人时用1表示，是团体是用2表示
		/// </summary>
		public string businessType { get; set; }
		/// <summary>
		/// (月数与天数其中一项非空) 保险期限（月）
		/// </summary>
		public string applyMonth { get; set; }
		public string applyPersonnelNum { get; set; }
	}
	/// <summary>
	/// 报单扩展信息
	/// </summary>
	public class PolicyExtendInfo
	{
		/// <summary>
		/// （非空）用于保存外部合作伙伴名称,由平安IT分配
		/// </summary>
		public string partnerName { get; set; }
		/// <summary>
		/// （非空）用于保存外部合作伙伴与保单号对应的号码，如合作伙伴订单号、合作伙伴序列号或单证流水号等，不同出单请求请不要重复）
		/// </summary>
		public string partnerSystemSeriesNo { get; set; }
		/// <summary>
		/// （可空）电子发票短信邮件发送配置 0:不发 1:发短信 2:发邮件 3:短信邮件都发，铭辰利云填写2
		/// </summary>
		public string isSendInvoice { get; set; }
		/// <summary>
		/// （可空）电子发票短信发送号码
		/// </summary>
		public string invokeMobilePhone { get; set; }
		/// <summary>
		/// （可空）电子发票邮件发送地址，铭辰利云要填电子发票的接受邮箱
		/// </summary>
		public string invokeEmail { get; set; }

	}
	public class InsuranceApplicantInfo
	{
		/// <summary>
		/// （非空）投保人信息，个人投保信息或者团体投保信息根据需要选择其中一项
		/// </summary>
		public GroupPersonnelInfo groupPersonnelInfo { get; set; }
	}
	/// <summary>
	/// （当投保人为团体时非空）团体投保人信息
	/// </summary>
	public class GroupPersonnelInfo
	{
		/// <summary>
		/// （非空）团体名称 (最长150字节)
		/// </summary>
		public string groupName { get; set; }
		/// <summary>
		/// （非空）团体证件号码 (最长30字节) 
		/// </summary>
		public string groupCertificateNo { get; set; }
		/// <summary>
		/// （可空）团体证件类型 01表示组织机构代码证，02表示税务登记证，03表示异常证件, 04统一社会信用代码
		/// </summary>
		public string groupCertificateType { get; set; }
		/// <summary>
		/// （可空）行业类型 (最长12字节)
		/// </summary>
		public string industryCode { get; set; }
		/// <summary>
		/// （可空）邮政编码 (最长6字节)
		/// </summary>
		public string postCode { get; set; }
		/// <summary>
		/// （可空）经营区域分类 (最长2字节)
		/// </summary>
		public string businessRegionType { get; set; }
		/// <summary>
		/// （可空）单位性质， 07:股份 01:国有 02:集体 33:个体 03:私营 04:中外合资 05:外商独资 08:机关事业 13:社团 39:中外合作 9:其他",
		/// </summary>
		public string companyAttribute { get; set; }
		/// <summary>
		/// （可空）团体简称 (最长20字节)
		/// </summary>
		public string groupAbbr { get; set; }
		/// <summary>
		/// （可空）工商注册号 (最长20字节) 
		/// </summary>
		public string businessRegisterId { get; set; }
		/// <summary>
		/// （可空）联系地址 (最长100字节)
		/// </summary>
		public string address { get; set; }
		/// <summary>
		/// （可空）电话总机区号(最长10字节) 
		/// </summary>
		public string phoneExchangeArea { get; set; }
		/// <summary>
		/// （可空）电话总机(最长30字节)
		/// </summary>
		public string phoneExchange { get; set; }
		/// <summary>
		/// （可空）联系人姓名(最长150字节)
		/// </summary>
		public string linkManName { get; set; }
		/// <summary>
		/// （可空）联系人性别, M表示男，F表示女
		/// </summary>
		public string linkManSexCode { get; set; }
		/// <summary>
		/// （可空）联系人email(最长60字节) ",投保成功后接受消息的邮箱
		/// </summary>
		/// 
		public string linkManEmail { get; set; }
		/// <summary>
		/// （可空）联系人手机号码 (最长20字节)
		/// </summary>
		public string linkManMobileTelephone { get; set; }
		/// <summary>
		/// （可空）开户银行 (最长10字节)
		/// </summary>
		public string bankCode { get; set; }
		/// <summary>
		/// （可空）开户账号 (最长30字节)
		/// </summary>
		public string bankAccount { get; set; }
	}
	public class PlanInfoItem
	{
		public PlanInfo planInfo;
	}
	public class InsurantInfoItem
	{
		public InsurantInfo insurantInfo;
	}

	public class SubjectInfo
	{
		/// <summary>
		/// （非空）层级实际保费合计，请使用阿拉伯数字，例如：5.00, 类型：NUMBER(16,2) ",
		/// </summary>
		public string totalModalPremium { get; set; }
		public string subjectName { get; set; }
		public string applyPersonnelNum { get; set; }

		/// <summary>
		/// （非空）险种列表
		/// </summary>
		public List<PlanInfoItem> planInfo { get; set; }
		/// <summary>
		/// 被保险人列表
		/// </summary>
		public List<InsurantInfoItem> insurantInfo { get; set; }
	}
	public class PlanInfo
	{
		/// <summary>
		/// （非空）险种代码"
		/// </summary>
		public string planCode { get; set; }
		/// <summary>
		/// （非空）投保份数
		/// </summary>
		public string applyNum { get; set; }
		/// <summary>
		/// （非空）实际保费合计
		/// </summary>
		public string totalModalPremium { get; set; }
		/// <summary>
		/// （月数与天数其中一项非空）保险期限（月）
		/// </summary>
		public string applyMonth { get; set; }
		/// <summary>
		/// （月数与天数其中一项非空）保险期限（日）
		/// </summary>
		public string applyDay { get; set; }
		/// <summary>
		/// （非空）责任列表
		/// </summary>
		public List<DutyInfoItem> dutyInfo { get; set; }
	}
	public class DutyInfoItem
	{
		public DutyInfo dutyInfo { get; set; }
	}
	public class DutyInfo
	{
		/// <summary>
		/// （非空）责任编码
		/// </summary>
		public string dutyCode { get; set; }
		/// <summary>
		/// （非空）实际保费合计
		/// </summary>
		public string totalModalPremium { get; set; }
		/// <summary>
		/// （非空）保额
		/// </summary>
		public string dutyAount { get; set; }
	}

	/// <summary>
	/// （非空）被保险人信息
	/// </summary>
	public class InsurantInfo
	{
		/// <summary>
		/// （非空）人员属性 100真实被保人，200表示虚拟被保人，一般情况请填写100",
		/// </summary>
		public string personnelAttribute { get; set; }
		/// <summary>
		/// （如果是虚拟被保人，则非空）虚拟被保险人数，请使用阿拉伯数字，例如：1",
		/// </summary>
		public string virtualInsuredNum { get; set; }
		/// <summary>
		/// （非空）人员名称"
		/// </summary>
		public string personnelName { get; set; }
		/// <summary>
		/// （可空）人员年龄
		/// </summary>
		public string personnelAge { get; set; }
		/// <summary>
		/// "(可空当有需求给被保人发短信时为非空)手机号
		/// </summary>
		public string mobileTelephone { get; set; }
		/// <summary>
		/// （非空）性别，M表示男，F表示女"
		/// </summary>
		public string sexCode { get; set; }
		/// <summary>
		/// （非空）生日，格式yyyy-MM-dd"
		/// </summary>
		/// 
		public string birthday { get; set; }
		/// <summary>
		///  (可空 当有需求给被保人发邮件时为非空)email地址
		/// </summary>
		public string email { get; set; }
		/// <summary>
		/// （可空）姓拼音"
		/// </summary>
		public string familyNameSpell { get; set; }
		/// <summary>
		/// （可空）名拼音
		/// </summary>
		public string firstNameSpell { get; set; }
		/// <summary>
		/// （非空）证件号码
		/// </summary>
		public string certificateNo { get; set; }
		/// <summary>
		/// （非空）证件类型
		/// </summary>
		public string certificateType { get; set; }
		public string professionGradeCode { get; set; }
		public string professionCode { get; set; }
	}




    //返回实例化类
    public class PAResponse
    {
      public string ret { get; set; }
      public string msg { get; set; }
      public string requestId { get; set; }
      public string data { get; set; }
    }
    public class PAResponseData
    {
        public string PA_RSLT_MESG { get; set; }
        public string PA_RSLT_CODE { get; set; }
        public string BUSINESS_NO { get; set; }
        public string AMOUNT { get; set; }
        public string resultMessage { get; set; }
        public string PA_ACCT_TIME { get; set; }
        public string BK_SERIAL { get; set; }
        public string FT_SERIAL { get; set; }
        public string resultCode { get; set; }
        public applyPolicyInfo applyPolicyInfo { get; set; }
        public string BANK_CODE { get; set; }
        public string TRAN_CODE { get; set; }
        public string PTP_CODE { get; set; }
        public string REGION_CODE { get; set; }
        public string PA_ACCT_DATE { get; set; }
    }

    public class applyPolicyInfo
    {
        public string offLinePolicyNo { get; set; }
        public string noticeNo { get; set; }
        public string coinsDutyProportion { get; set; }
        public string coinsuranceTotalPremium { get; set; }
        public string validateCode { get; set; }
        public string isOffLine { get; set; }
        public string isFullCoinPremium { get; set; }
        public string applyPolicyNo { get; set; }
        public string isGiftInsurance { get; set; }
        public string totalPremium { get; set; }
        public string invoiceUrl { get; set; }
    }
}
