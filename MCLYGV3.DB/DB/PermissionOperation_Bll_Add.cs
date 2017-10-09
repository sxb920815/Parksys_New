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
		/// 添加角色权限操作表
		/// </summary>
		/// <param name="Ids">ID</param>
		/// <param name="Name">操作名称</param>
		/// <param name="KeyCode">操作码</param>
		/// <param name="RightId">功能ID</param>

		/// <returns></returns>
		public static M_PermissionOperation Add(string Ids,string Name,string KeyCode,string RightId)
		{
			M_PermissionOperation PermissionOperationObj = new M_PermissionOperation();
			PermissionOperationObj.Ids = Ids;
			PermissionOperationObj.Name = Name;
			PermissionOperationObj.KeyCode = KeyCode;
			PermissionOperationObj.RightId = RightId;

			return Add(PermissionOperationObj);
		}

		/// <summary>
		/// 添加角色权限操作表
		/// </summary>
		/// <param name="PermissionOperationObj">角色权限操作表实体</param>
		/// <returns></returns>
		public static M_PermissionOperation Add(M_PermissionOperation PermissionOperationObj)
		{
			using (DBContext db = new DBContext())
			{
				try
				{
					
					db.PermissionOperationList.Add(PermissionOperationObj);
					int result = db.SaveChanges();
					return PermissionOperationObj;
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
					return null;
				}
			}
		}
	}
}