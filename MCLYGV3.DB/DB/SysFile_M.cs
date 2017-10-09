using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace MCLYGV3.DB
{
	/// <summary>
	/// 文件上传
	/// </summary>
	[Table("SysFile")]
	public class M_SysFile
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
		/// 文件名称
		/// </summary>
		[DisplayName("文件名称")]
		[Required]
		public string Name  { get; set; }
		/// <summary>
		/// 文件类型
		/// </summary>
		[DisplayName("文件类型")]
		[Required]
		public string Type  { get; set; }
		/// <summary>
		/// 文件地址
		/// </summary>
		[DisplayName("文件地址")]
		[Required]
		public string Url  { get; set; }
		/// <summary>
		/// 缩略图地址
		/// </summary>
		[DisplayName("缩略图地址")]
		public string SmallUrl  { get; set; }
		/// <summary>
		/// 上传时间
		/// </summary>
		[DisplayName("上传时间")]
		[Required]
		public DateTime CreatTime  { get; set; }
		/// <summary>
		/// 关联表名
		/// </summary>
		[DisplayName("关联表名")]
		public string TableName  { get; set; }
		/// <summary>
		/// 关联表主键
		/// </summary>
		[DisplayName("关联表主键")]
		public string TableID  { get; set; }



	}
}
