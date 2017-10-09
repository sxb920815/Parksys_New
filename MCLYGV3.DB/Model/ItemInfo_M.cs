using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace MCLYGV3.DB
{
	/// <summary>
	/// 项目配置
	/// </summary>
	[Table("ItemInfo")]
	public class M_ItemInfo
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
		/// 项目名称
		/// </summary>
		[DisplayName("项目名称")]
		[Required]
		public string Name  { get; set; }
		/// <summary>
		/// 上级ID
		/// </summary>
		[DisplayName("上级ID")]
		[Required]
		public int PID  { get; set; }
		/// <summary>
		/// 类别
		/// </summary>
		[DisplayName("类别")]
		[Required]
		public string type  { get; set; }



	}
}
