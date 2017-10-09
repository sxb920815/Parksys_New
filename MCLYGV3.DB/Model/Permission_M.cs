using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace MCLYGV3.DB
{
	/// <summary>
	/// 权限表
	/// </summary>
	[Table("Permission")]
	public class M_Permission
	{
		/// <summary>
		/// ID
		/// </summary>
		[DisplayName("ID")]
		[Key]
		[Column(Order = 1)]
		[Required]
		public string ID  { get; set; }
		/// <summary>
		/// 权限名称
		/// </summary>
		[DisplayName("权限名称")]
		[Required]
		public string Name  { get; set; }
		/// <summary>
		/// 上级ID
		/// </summary>
		[DisplayName("上级ID")]
		[Required]
		public string ParentId  { get; set; }
		/// <summary>
		/// 描述
		/// </summary>
		[DisplayName("描述")]
		public string Description  { get; set; }
		/// <summary>
		/// 网址
		/// </summary>
		[DisplayName("网址")]
		public string Url  { get; set; }
		/// <summary>
		/// 创建人
		/// </summary>
		[DisplayName("创建人")]
		public string CreatePerson  { get; set; }
		/// <summary>
		/// 创建时间
		/// </summary>
		[DisplayName("创建时间")]
		public string CreateTime  { get; set; }



	}
}
