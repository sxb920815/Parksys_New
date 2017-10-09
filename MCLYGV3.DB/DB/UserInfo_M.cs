using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace MCLYGV3.DB
{
	/// <summary>
	/// 用户表
	/// </summary>
	[Table("UserInfo")]
	public class M_UserInfo
	{
		/// <summary>
		/// ID
		/// </summary>
		[DisplayName("ID")]
		[Key]
		[Column(Order = 1)]
		[DatabaseGenerated(DatabaseGeneratedOption.None)]
		[Required]
		public int ID  { get; set; }
		/// <summary>
		/// 用户名
		/// </summary>
		[DisplayName("用户名")]
		[Required]
		public string UserName  { get; set; }
		/// <summary>
		/// 真实姓名
		/// </summary>
		[DisplayName("真实姓名")]
		[Required]
		public string RealName  { get; set; }
		/// <summary>
		/// 登录密码
		/// </summary>
		[DisplayName("登录密码")]
		[Required]
		public string PassWord  { get; set; }
		/// <summary>
		/// 电话号码
		/// </summary>
		[DisplayName("电话号码")]
		public string Tel  { get; set; }
		/// <summary>
		/// 信箱
		/// </summary>
		[DisplayName("信箱")]
		public string Email  { get; set; }
		/// <summary>
		/// 所属公司
		/// </summary>
		[DisplayName("所属公司")]
		[Required]
		public virtual M_Company InCompany  { get; set; }



	}
}
