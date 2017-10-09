using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace MCLYGV3.DB
{
	/// <summary>
	/// 角色权限操作表
	/// </summary>
	[Table("PermissionOperation")]
	public class M_PermissionOperation
	{
		/// <summary>
		/// ID
		/// </summary>
		[DisplayName("ID")]
		[Key]
		[Column(Order = 1)]
		[Required]
		public string Ids  { get; set; }
		/// <summary>
		/// 操作名称
		/// </summary>
		[DisplayName("操作名称")]
		[Required]
		public string Name  { get; set; }
		/// <summary>
		/// 操作码
		/// </summary>
		[DisplayName("操作码")]
		[Required]
		public string KeyCode  { get; set; }
		/// <summary>
		/// 功能ID
		/// </summary>
		[DisplayName("功能ID")]
		[Required]
		public string RightId  { get; set; }
		/// <summary>
		/// 对应权限
		/// </summary>
		[DisplayName("对应权限")]
		public virtual ICollection<M_Role> roleList  { get; set; }



		public M_PermissionOperation()
		{
			roleList = new HashSet<M_Role>();
		}
	}
}
