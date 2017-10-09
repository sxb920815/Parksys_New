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
	[Table("MoneyConfigChild")]
	public class M_MoneyConfigChild
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
		/// 业务员
		/// </summary>
		[DisplayName("业务员")]
		[Required]
		public int UserId  { get; set; }
		/// <summary>
		/// 费率
		/// </summary>
		[DisplayName("费率")]
		[Required]
		public decimal Rate  { get; set; }



	}
}
