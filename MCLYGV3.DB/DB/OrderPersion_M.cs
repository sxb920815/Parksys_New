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
	[Table("OrderPersion")]
	public class M_OrderPersion
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
		/// 订单编号或子订单编号
		/// </summary>
		[DisplayName("订单编号或子订单编号")]
		[Required]
		public string OrderCode  { get; set; }
		/// <summary>
		/// 0订单1子订单
		/// </summary>
		[DisplayName("0订单1子订单")]
		[Required]
		public int Type  { get; set; }
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
		/// 姓名
		/// </summary>
		[DisplayName("姓名")]
		[Required]
		public string RealName  { get; set; }
		/// <summary>
		/// 身份证号
		/// </summary>
		[DisplayName("身份证号")]
		[Required]
		public string IdNum  { get; set; }
		/// <summary>
		/// 主险意外伤害保费
		/// </summary>
		[DisplayName("主险意外伤害保费")]
		[Required]
		public decimal AcciPremium  { get; set; }
		/// <summary>
		/// 主险意外伤害保额
		/// </summary>
		[DisplayName("主险意外伤害保额")]
		[Required]
		public decimal AcciDutyAount  { get; set; }
		/// <summary>
		/// 附加医疗保费
		/// </summary>
		[DisplayName("附加医疗保费")]
		[Required]
		public decimal MedicalPremium  { get; set; }
		/// <summary>
		/// 附加医疗保额
		/// </summary>
		[DisplayName("附加医疗保额")]
		[Required]
		public decimal MedicalDutyAount  { get; set; }
		/// <summary>
		/// 住院津贴保额
		/// </summary>
		[DisplayName("住院津贴保额")]
		[Required]
		public decimal AllowancePremium  { get; set; }
		/// <summary>
		/// 住院津贴保费
		/// </summary>
		[DisplayName("住院津贴保费")]
		[Required]
		public decimal AllowanceDutyAount  { get; set; }



	}
}
