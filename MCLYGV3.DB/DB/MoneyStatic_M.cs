using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace MCLYGV3.DB
{
	/// <summary>
	/// 奖金表
	/// </summary>
	[Table("MoneyStatic")]
	public class M_MoneyStatic
	{
		/// <summary>
		/// ID
		/// </summary>
		[DisplayName("ID")]
		[Key]
		[Column(Order = 1)]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		[Required]
		public int ID  { get; set; }
		/// <summary>
		/// 时间
		/// </summary>
		[DisplayName("时间")]
		[Required]
		public DateTime CreateTime  { get; set; }
		/// <summary>
		/// 保单号
		/// </summary>
		[DisplayName("保单号")]
		[Required]
		public string PolicyNumber  { get; set; }
		/// <summary>
		/// 保险公司
		/// </summary>
		[DisplayName("保险公司")]
		[Required]
		public string InsuranceCompany  { get; set; }
		/// <summary>
		/// 产品
		/// </summary>
		[DisplayName("产品")]
		[Required]
		public string ProductName  { get; set; }
		/// <summary>
		/// 业务员
		/// </summary>
		[DisplayName("业务员")]
		[Required]
		public int UserId  { get; set; }
		/// <summary>
		/// 代理公司
		/// </summary>
		[DisplayName("代理公司")]
		[Required]
		public int CompanyId  { get; set; }
		/// <summary>
		/// 订单金额
		/// </summary>
		[DisplayName("订单金额")]
		[Required]
		public decimal Money  { get; set; }
		/// <summary>
		/// 总提成
		/// </summary>
		[DisplayName("总提成")]
		[Required]
		public decimal CommissionMoney  { get; set; }
		/// <summary>
		/// 代理公司提成
		/// </summary>
		[DisplayName("代理公司提成")]
		[Required]
		public decimal CompanyMoney  { get; set; }
		/// <summary>
		/// 业务员提成
		/// </summary>
		[DisplayName("业务员提成")]
		[Required]
		public decimal UserMoney  { get; set; }
		/// <summary>
		/// 保险公司是否结算
		/// </summary>
		[DisplayName("保险公司是否结算")]
		[Required]
		public bool IsInsuranceCompanyGive  { get; set; }
		/// <summary>
		/// 代理公司是否结算
		/// </summary>
		[DisplayName("代理公司是否结算")]
		[Required]
		public bool IsCompanyGive  { get; set; }



	}
}
