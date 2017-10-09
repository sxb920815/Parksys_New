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
	[Table("OrderPlan")]
	public class M_OrderPlan
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
		/// 职业代码
		/// </summary>
		[DisplayName("职业代码")]
		[Required]
		public string ProfessionCode  { get; set; }
		/// <summary>
		/// 职业名称
		/// </summary>
		[DisplayName("职业名称")]
		[Required]
		public string ProfessionName  { get; set; }
		/// <summary>
		/// 险种代码
		/// </summary>
		[DisplayName("险种代码")]
		[Required]
		public string PlanCode  { get; set; }
		/// <summary>
		/// 责任代码
		/// </summary>
		[DisplayName("责任代码")]
		[Required]
		public string DutyCode  { get; set; }
		/// <summary>
		/// 保费
		/// </summary>
		[DisplayName("保费")]
		[Required]
		public decimal ModalPremium  { get; set; }
		/// <summary>
		/// 保额
		/// </summary>
		[DisplayName("保额")]
		[Required]
		public decimal DutyAount  { get; set; }



	}
}
