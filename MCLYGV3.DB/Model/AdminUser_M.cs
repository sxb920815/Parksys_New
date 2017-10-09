using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace MCLYGV3.DB
{
	/// <summary>
	/// 管理员
	/// </summary>
	[Table("AdminUser")]
	public class M_AdminUser
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
		public string RealName  { get; set; }
		/// <summary>
		/// 登录密码
		/// </summary>
		[DisplayName("登录密码")]
		[Required]
		public string PassWord  { get; set; }
		/// <summary>
		/// 是否超级管理员
		/// </summary>
		[DisplayName("是否超级管理员")]
		[Required]
		public bool IsSupper  { get; set; }
		/// <summary>
		/// 注册时间
		/// </summary>
		[DisplayName("注册时间")]
		[Required]
		public DateTime RegTime  { get; set; }
		/// <summary>
		/// 本次时间
		/// </summary>
		[DisplayName("本次时间")]
		[Required]
		public DateTime NowTime  { get; set; }
		/// <summary>
		/// 上次登录时间
		/// </summary>
		[DisplayName("上次登录时间")]
		[Required]
		public DateTime LastTime  { get; set; }
		/// <summary>
		/// 拥有角色
		/// </summary>
		[DisplayName("拥有角色")]
		public virtual ICollection<M_Role> RoleList  { get; set; }
		/// <summary>
		/// 提成系数1
		/// </summary>
		[DisplayName("提成系数1")]
		public decimal? Rate1  { get; set; }
		/// <summary>
		/// 提成系数2
		/// </summary>
		[DisplayName("提成系数2")]
		public decimal? Rate2  { get; set; }
		/// <summary>
		/// 提成系数3
		/// </summary>
		[DisplayName("提成系数3")]
		public decimal? Rate3  { get; set; }
		/// <summary>
		/// 提成系数4
		/// </summary>
		[DisplayName("提成系数4")]
		public decimal? Rate4  { get; set; }
		/// <summary>
		/// 提成系数5
		/// </summary>
		[DisplayName("提成系数5")]
		public decimal? Rate5  { get; set; }
		/// <summary>
		/// 提成系数6
		/// </summary>
		[DisplayName("提成系数6")]
		public decimal? Rate6  { get; set; }
		/// <summary>
		/// 所属公司ID
		/// </summary>
		[DisplayName("所属公司ID")]
		[Required]
		public int InCompanyId  { get; set; }



		public M_AdminUser()
		{
			RoleList = new HashSet<M_Role>();
		}
	}
}
