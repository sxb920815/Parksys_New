using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace MCLYGV3.DB
{
	/// <summary>
	/// 产品表
	/// </summary>
	[Table("MoneyProduct")]
	public class M_MoneyProduct
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
		/// 产品名称
		/// </summary>
		[DisplayName("产品名称")]
		[Required]
		public string ProductName  { get; set; }
		/// <summary>
		/// 产品中文名称
		/// </summary>
		[DisplayName("产品中文名称")]
		[Required]
		public string ChineseName  { get; set; }
		/// <summary>
		/// 保险公司名称
		/// </summary>
		[DisplayName("保险公司名称")]
		[Required]
		public string CompanyName  { get; set; }
		/// <summary>
		/// 保险公司给我们的费率
		/// </summary>
		[DisplayName("保险公司给我们的费率")]
		[Required]
		public decimal CompanyRate  { get; set; }



	}
}
