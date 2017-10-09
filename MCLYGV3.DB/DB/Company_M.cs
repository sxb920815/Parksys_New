using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace MCLYGV3.DB
{
	/// <summary>
	/// 代理公司
	/// </summary>
	[Table("Company")]
	public class M_Company
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
		/// 公司名
		/// </summary>
		[DisplayName("公司名")]
		[Required]
		public string CompanyName  { get; set; }
		/// <summary>
		/// 电话
		/// </summary>
		[DisplayName("电话")]
		public string Tel  { get; set; }
		/// <summary>
		/// 信箱
		/// </summary>
		[DisplayName("信箱")]
		public string Email  { get; set; }
		/// <summary>
		/// 地址
		/// </summary>
		[DisplayName("地址")]
		public string Address  { get; set; }
		/// <summary>
		/// 业务员列表
		/// </summary>
		[DisplayName("业务员列表")]
		public virtual ICollection<M_UserInfo> UserList  { get; set; }
		/// <summary>
		/// Logo
		/// </summary>
		[DisplayName("Logo")]
		public string Logo  { get; set; }



		public M_Company()
		{
			UserList = new HashSet<M_UserInfo>();
		}
	}
}
