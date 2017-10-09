using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using MCLYGV3.DB.ClassLib;
using Newtonsoft.Json;
using System.Data.Entity.Validation;
using System.IO;
using System.Text;


namespace MCLYGV3.DB
{

	/// <summary>
	/// 管理员数据库操作类
	/// </summary>
	public partial class B_AdminUser
	{

		/// <summary>
		/// 检查权限
		/// </summary>
		/// <param name="AdminUserId">管理员ID</param>
		/// <param name="PermissionOperationId">权限操作码ID</param>
		/// <returns></returns>
		public static bool CheckPermission(int AdminUserId, string PermissionOperationId)
		{
			using (DBContext db = new DBContext())
			{
				var AdminUser = db.AdminUserList.Find(AdminUserId);
				if (AdminUser.IsSupper)
					return true;

				foreach (var item in AdminUser.RoleList)
				{
					if (item.OperationList.Count(t => t.Ids == PermissionOperationId) > 0)
						return true;
				}
				return false;
			}
		}


		/// <summary>
		/// 查询管理员是否属于某个角色
		/// </summary>
		/// <param name="AdminUserId">管理员ID</param>
		/// <param name="RoleId">角色ID</param>
		/// <returns></returns>
		public static bool HasRole(int AdminUserId, int RoleId)
		{
			using (DBContext db = new DBContext())
			{
				var AdminUser = db.AdminUserList.Find(AdminUserId);
				if (AdminUser.RoleList == null)
					return false;
				if (AdminUser.RoleList.Count(t => t.ID == RoleId) > 0)
					return true;

				return false;
			}
		}

		/// <summary>
		/// 重设管理员的角色
		/// </summary>
		/// <param name="AdminUserId">管理员ID</param>
		/// <param name="roleIds">角色ID列表，逗号分隔</param>
		/// <returns></returns>
		public static bool SetRole(int AdminUserId, string roleIds)
		{
			using (DBContext db = new DBContext())
			{
				var AdminUser = db.AdminUserList.Find(AdminUserId);
				if (AdminUser == null)
					return false;

				AdminUser.RoleList.Clear();
				var roleidlist = roleIds.Split(',');
				foreach (var roleid in roleidlist)
				{
					int rid = int.Parse(roleid);
					AdminUser.RoleList.Add(db.RoleList.Find(rid));
				}
				try
				{
					db.SaveChanges();
					return true;

				}
				catch (DbEntityValidationException ex)
				{
					StringBuilder sb = new StringBuilder();
					foreach (var item in ex.EntityValidationErrors)
					{
						foreach (var item2 in item.ValidationErrors)
						{
							sb.Append($"PropertyName:{item2.PropertyName},{item2.ErrorMessage}\r\n\r\n");
						}
					}
					Log.SystemWrite("【AdminUser】\r\n" + ex.Message + "\r\n\r\n" + sb.ToString());
					return false;
				}
				
			}

		}
	}
}