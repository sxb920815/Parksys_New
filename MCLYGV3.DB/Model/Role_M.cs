using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace MCLYGV3.DB
{
	/// <summary>
	/// 角色表
	/// </summary>
	[Table("Role")]
	public class M_Role
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
		/// 角色名称
		/// </summary>
		[DisplayName("角色名称")]
		[Required]
		public string Name  { get; set; }
		/// <summary>
		/// 描述
		/// </summary>
		[DisplayName("描述")]
		[Required]
		public string Description  { get; set; }
		/// <summary>
		/// 创建时间
		/// </summary>
		[DisplayName("创建时间")]
		[Required]
		public DateTime CreateTime  { get; set; }
		/// <summary>
		/// 创建用户
		/// </summary>
		[DisplayName("创建用户")]
		[Required]
		public string CreatePerson  { get; set; }
		/// <summary>
		/// 是否可用
		/// </summary>
		[DisplayName("是否可用")]
		[Required]
		public bool Enabled  { get; set; }
		/// <summary>
		/// 所属用户
		/// </summary>
		[DisplayName("所属用户")]
		public virtual ICollection<M_AdminUser> AdminUserList  { get; set; }
		/// <summary>
		/// 权限操作列表
		/// </summary>
		[DisplayName("权限操作列表")]
		public virtual ICollection<M_PermissionOperation> OperationList  { get; set; }



		public M_Role()
		{
			AdminUserList = new HashSet<M_AdminUser>();
			OperationList = new HashSet<M_PermissionOperation>();
		}
	}
}
