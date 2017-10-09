using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace MCLYGV3.DB
{
	/// <summary>
	/// 奖金配置表
	/// </summary>
	[Table("MoneyConfig")]
	public class M_MoneyConfig
	{
		/// <summary>
		/// 
		/// </summary>
		[DisplayName("")]
		[Key]
		[Column(Order = 1)]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		[Required]
		public int ID  { get; set; }
		/// <summary>
		/// 产品名称
		/// </summary>
		[DisplayName("产品名称")]
		[Required]
		public string ProductName  { get; set; }
		/// <summary>
		/// 代理公司
		/// </summary>
		[DisplayName("代理公司")]
		[Required]
		public int CompanyId  { get; set; }
		/// <summary>
		/// 费率
		/// </summary>
		[DisplayName("费率")]
		[Required]
		public decimal Rate  { get; set; }
		/// <summary>
		/// 默认业务员费率
		/// </summary>
		[DisplayName("默认业务员费率")]
		[Required]
		public decimal ChildRate  { get; set; }



	}
}
