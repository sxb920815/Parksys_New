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
	/// 角色权限操作表数据库操作类
	/// </summary>
	public partial class B_PermissionOperation
	{
		/// <summary>
		/// 修改角色权限操作表
		/// </summary>
		/// <param name="PermissionOperationObj">角色权限操作表实体</param>
		/// <returns></returns>
		public static bool UpdateNormal(M_PermissionOperation PermissionOperationObj)
		{
			using (DBContext db = new DBContext())
			{
				try
				{
					db.PermissionOperationList.Attach(PermissionOperationObj);
					DbEntityEntry<M_PermissionOperation> entry = db.Entry(PermissionOperationObj);
					entry.State = EntityState.Modified;
					db.SaveChanges();
					return true;
				}
				catch (Exception ex)
				{
					Log.SystemWrite("【PermissionOperation】\r\n" + ex.Message + "\r\n" + ex.ToString());
					return false;
				}
			}
		}

		/// <summary>
		/// 修改角色权限操作表
		/// </summary>
		/// <param name="PermissionOperationObj">角色权限操作表实体</param>
		/// <returns></returns>
		public static bool Update(M_PermissionOperation EditPermissionOperationObj)
		{
			using (DBContext db = new DBContext())
			{
				try
				{
					M_PermissionOperation PermissionOperationObj = db.PermissionOperationList.Find(EditPermissionOperationObj.Ids);
					PermissionOperationObj.Name = EditPermissionOperationObj.Name;
					PermissionOperationObj.KeyCode = EditPermissionOperationObj.KeyCode;
					PermissionOperationObj.RightId = EditPermissionOperationObj.RightId;

					int count = db.SaveChanges();
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
					Log.SystemWrite("【PermissionOperation】\r\n" + ex.Message + "\r\n\r\n" + sb.ToString());
					return false;
				}
			}
		}
	}
}