using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace MCLYGV3.DB
{
	/// <summary>
	/// 订单
	/// </summary>
	[Table("Order")]
	public class M_Order
	{
		/// <summary>
		/// 订单编号
		/// </summary>
		[DisplayName("订单编号")]
		[Key]
		[Column(Order = 1)]
		[Required]
		public string OrderCode  { get; set; }
		/// <summary>
		/// 用户ID
		/// </summary>
		[DisplayName("用户ID")]
		[Required]
		public int UserId  { get; set; }
		/// <summary>
		/// 订单步骤。0未支付1已支付2已过期
		/// </summary>
		[DisplayName("订单步骤。0未支付1已支付2已过期")]
		[Required]
		public int OrderStep  { get; set; }
		/// <summary>
		/// 投保人公司名
		/// </summary>
		[DisplayName("投保人公司名")]
		[Required]
		public string InsuredName  { get; set; }
		/// <summary>
		/// 投保人公司编号
		/// </summary>
		[DisplayName("投保人公司编号")]
		[Required]
		public string IdentifyNumber  { get; set; }
		/// <summary>
		/// 证件图片ID
		/// </summary>
		[DisplayName("证件图片ID")]
		[Required]
		public string IdentifyPic  { get; set; }
		/// <summary>
		/// 附加费
		/// </summary>
		[DisplayName("附加费")]
		[Required]
		public decimal Surcharge  { get; set; }
		/// <summary>
		/// 初次总保费
		/// </summary>
		[DisplayName("初次总保费")]
		[Required]
		public decimal FirstModalPremium  { get; set; }
		/// <summary>
		/// 初次总保额
		/// </summary>
		[DisplayName("初次总保额")]
		[Required]
		public decimal FirstdutyAount  { get; set; }
		/// <summary>
		/// 当前总保费
		/// </summary>
		[DisplayName("当前总保费")]
		[Required]
		public decimal NowdutyAount  { get; set; }
		/// <summary>
		/// 当前总保额
		/// </summary>
		[DisplayName("当前总保额")]
		[Required]
		public decimal NowModalPremium  { get; set; }
		/// <summary>
		/// 保险公司。CT或者PA
		/// </summary>
		[DisplayName("保险公司。CT或者PA")]
		[Required]
		public string InsuranceCompany  { get; set; }
		/// <summary>
		/// 见费出单。Y或者N
		/// </summary>
		[DisplayName("见费出单。Y或者N")]
		[Required]
		public string CodInd  { get; set; }
		/// <summary>
		/// 保单类别。Group
		/// </summary>
		[DisplayName("保单类别。Group")]
		[Required]
		public string OrderType  { get; set; }
		/// <summary>
		/// 保险生效时间
		/// </summary>
		[DisplayName("保险生效时间")]
		[Required]
		public DateTime StartTime  { get; set; }
		/// <summary>
		/// 保险失效时间
		/// </summary>
		[DisplayName("保险失效时间")]
		[Required]
		public DateTime EndTime  { get; set; }
		/// <summary>
		/// 保险提交时间
		/// </summary>
		[DisplayName("保险提交时间")]
		[Required]
		public DateTime BuyTime  { get; set; }
		/// <summary>
		/// 付款返回时间
		/// </summary>
		[DisplayName("付款返回时间")]
		public DateTime? PayTime  { get; set; }
		/// <summary>
		/// 投保月份
		/// </summary>
		[DisplayName("投保月份")]
		[Required]
		public int applyMonth  { get; set; }
		/// <summary>
		/// 保单号
		/// </summary>
		[DisplayName("保单号")]
		public string PolicyNo  { get; set; }
		/// <summary>
		/// 投保计划
		/// </summary>
		[DisplayName("投保计划")]
		public virtual ICollection<M_OrderPlan> PlanList  { get; set; }
		/// <summary>
		/// 子订单列表
		/// </summary>
		[DisplayName("子订单列表")]
		public virtual ICollection<M_OrderChild> ChildList  { get; set; }
		/// <summary>
		/// 电子信箱
		/// </summary>
		[DisplayName("电子信箱")]
		public string Email  { get; set; }
		/// <summary>
		/// 电话号码
		/// </summary>
		[DisplayName("电话号码")]
		public string Tel  { get; set; }
		/// <summary>
		/// 电子保单密文
		/// </summary>
		[DisplayName("电子保单密文")]
		public string encryptString  { get; set; }



		public M_Order()
		{
			PlanList = new HashSet<M_OrderPlan>();
			ChildList = new HashSet<M_OrderChild>();
		}
	}
}
