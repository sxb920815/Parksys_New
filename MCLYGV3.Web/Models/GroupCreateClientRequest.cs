using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MCLYGV3.Web.Models
{
	public class ClientRequest
	{
		/// <summary>
		/// 头部信息
		/// </summary>
		public ClientRequest_Head Head { get; set; }
		/// <summary>
		/// 保单核心信息
		/// </summary>
		public ClientRequest_Body Body { get; set; }
		/// <summary>
		/// 投保人信息
		/// </summary>
		public ClientRequest_RelatedParty RelatedParty { get; set; }
		/// <summary>
		/// 被保险人详细信息
		/// </summary>
		public List<ClientRequest_RiskRealatePary> RiskRealateParyList { get; set; }
	}
	/// <summary>
	/// 头部信息
	/// </summary>
	public class ClientRequest_Head
	{
		/// <summary>
		/// 访问者
		/// </summary>
		public string AppName { get; set; }
		/// <summary>
		/// 密码
		/// </summary>
		public string Password { get; set; }
	}
	/// <summary>
	/// 保单核心信息
	/// </summary>
	public class ClientRequest_Body
	{
		/// <summary>
		/// 订单号
		/// </summary>
		public string OrderCode { get; set; }
		/// <summary>
		/// 订单类别
		/// </summary>
		public string OrderType { get; set; }
		/// <summary>
		/// 保单生效时间
		/// </summary>
		public DateTime StartDate { get; set; }
		/// <summary>
		/// 保险公司
		/// </summary>
		public string InsuranceCompany { get; set; }
		/// <summary>
		/// 见费出单标记
		/// </summary>
		public string CodInd { get; set; }
		/// <summary>
		/// 投保月份
		/// </summary>
		public int applyMonth { get; set; }

        /// <summary>
        /// 附加费
        /// </summary>
        public decimal Surcharge { get; set; }

    }



	/// <summary>
	/// 投保人信息
	/// </summary>
	public class ClientRequest_RelatedParty
	{
		/// <summary>
		/// 公司名称
		/// </summary>
		public string InsuredName { get; set; }
		/// <summary>
		/// 证件号码
		/// </summary>
		public string IdentifyNumber { get; set; }
		/// <summary>
		/// 证件图片
		/// </summary>
		public string IdentifyPic { get; set; }
		public string Email { get; set; }
		public string Tel { get; set; }
    }

	public class ClientRequest_RiskRealatePary
	{
		/// <summary>
		/// 被保险人列表
		/// </summary>
		public List<ClientRequest_RiskRealatePary_InsurantInfo> InsurantList { get; set; }
		/// <summary>
		/// 具体保险计划列表
		/// </summary>
		public List<ClientRequest_RiskRealatePary_PlanInfo> PlanList { get; set; }
	}

	public class ClientRequest_RiskRealatePary_PlanInfo
	{
		/// <summary>
		/// 险种代码
		/// </summary>
		public string planCode { get; set; }
		/// <summary>
		/// 责任代码
		/// </summary>
		public string dutyCode { get; set; }
		/// <summary>
		/// 职业代码
		/// </summary>
		public string ProfessionCode { get; set; }
		/// <summary>
		/// 保费
		/// </summary>
		public double ModalPremium { get; set; }
		/// <summary>
		/// 保额
		/// </summary>
		public double dutyAount { get; set; }
	}
	public class ClientRequest_RiskRealatePary_InsurantInfo
	{
		public string Name { get; set; }
		public string IdNum { get; set; }
		public string ProfessionCode { get; set; }
		public string ProfessionName { get; set; }
	}

	

}