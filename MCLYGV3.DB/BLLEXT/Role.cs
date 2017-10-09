using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;

namespace MCLYGV3.DB
{

	/// <summary>
	/// 角色表数据库操作类
	/// </summary>
	public partial class B_Role
	{
		/// <summary>
		/// 查询管理员
		/// </summary>
		/// <param name="whereLambda">查询条件lambda表达式</param>
		/// <returns></returns>
		public static bool CheckPermission(int roleid, string PermissionOperationId)
		{
			using (DBContext db = new DBContext())
			{
				var role = db.RoleList.Find(roleid);
				if (role.OperationList.Count(t => t.Ids == PermissionOperationId) > 0)
					return true;

				return false;


			}
		}
		public static void DelPermission(int roleid, string PermissionOperationId)
		{
			using (DBContext db = new DBContext())
			{
				var role = db.RoleList.Find(roleid);
				if (role.OperationList.Count(t => t.Ids == PermissionOperationId) == 0)
					return;

				role.OperationList.Remove(role.OperationList.First(t => t.Ids == PermissionOperationId));
				
				try
				{
					db.SaveChanges();
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
					return;
				}
			}



		}
		public static void AddPermission(int roleid, string PermissionOperationId)
		{
			using (DBContext db = new DBContext())
			{
				var role = db.RoleList.Find(roleid);
				if (role.OperationList.Count(t => t.Ids == PermissionOperationId) > 0)
					return;

				M_PermissionOperation obj = db.PermissionOperationList.Find(PermissionOperationId);
				role.OperationList.Add(obj);
				try
				{
					db.SaveChanges();
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
					return ;
				}
			}
		}
	}
}