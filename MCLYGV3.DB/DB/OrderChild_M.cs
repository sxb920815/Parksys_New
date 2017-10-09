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
	[Table("OrderChild")]
	public class M_OrderChild
	{
		/// <summary>
		/// 子订单编号
		/// </summary>
		[DisplayName("子订单编号")]
		[Key]
		[Column(Order = 1)]
		[Required]
		public string ChildCode  { get; set; }
		/// <summary>
		/// 订单编号
		/// </summary>
		[DisplayName("订单编号")]
		[Required]
		public string OrderCode  { get; set; }
		/// <summary>
		/// 订单步骤。0未支付1已完成2取消
		/// </summary>
		[DisplayName("订单步骤。0未支付1已完成2取消")]
		[Required]
		public int Step  { get; set; }
		/// <summary>
		/// 类别。0新增1批增2批减
		/// </summary>
		[DisplayName("类别。0新增1批增2批减")]
		[Required]
		public int Type  { get; set; }
		/// <summary>
		/// 保额
		/// </summary>
		[DisplayName("保额")]
		[Required]
		public decimal dutyAount  { get; set; }
		/// <summary>
		/// 保费
		/// </summary>
		[DisplayName("保费")]
		[Required]
		public decimal ModalPremium  { get; set; }
		/// <summary>
		/// 附加费
		/// </summary>
		[DisplayName("附加费")]
		[Required]
		public decimal Surcharge  { get; set; }
		/// <summary>
		/// 诚泰返回的投保单号
		/// </summary>
		[DisplayName("诚泰返回的投保单号")]
		public string Ftrno  { get; set; }
		/// <summary>
		/// 诚泰返回的批增投保单号
		/// </summary>
		[DisplayName("诚泰返回的批增投保单号")]
		public string EndorNo  { get; set; }
		/// <summary>
		/// 平安返回的
		/// </summary>
		[DisplayName("平安返回的")]
		public string BUSINESS_NO  { get; set; }
		/// <summary>
		/// 平安返回的
		/// </summary>
		[DisplayName("平安返回的")]
		public string BK_SERIAL	  { get; set; }
		/// <summary>
		/// 平安返回的
		/// </summary>
		[DisplayName("平安返回的")]
		public string applyPolicyNo  { get; set; }
		/// <summary>
		/// 电子保单密文
		/// </summary>
		[DisplayName("电子保单密文")]
		public string encryptString  { get; set; }
		/// <summary>
		/// 订单创建时间
		/// </summary>
		[DisplayName("订单创建时间")]
		[Required]
		public DateTime CreateTime  { get; set; }
		/// <summary>
		/// 生效时间
		/// </summary>
		[DisplayName("生效时间")]
		[Required]
		public DateTime ValidDate  { get; set; }
		/// <summary>
		/// 支付时间
		/// </summary>
		[DisplayName("支付时间")]
		public DateTime? PayTime  { get; set; }



	}
}
