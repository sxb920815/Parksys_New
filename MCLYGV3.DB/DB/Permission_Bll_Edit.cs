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
	/// 权限表数据库操作类
	/// </summary>
	public partial class B_Permission
	{
		/// <summary>
		/// 修改权限表
		/// </summary>
		/// <param name="PermissionObj">权限表实体</param>
		/// <returns></returns>
		public static bool UpdateNormal(M_Permission PermissionObj)
		{
			using (DBContext db = new DBContext())
			{
				try
				{
					db.PermissionList.Attach(PermissionObj);
					DbEntityEntry<M_Permission> entry = db.Entry(PermissionObj);
					entry.State = EntityState.Modified;
					db.SaveChanges();
					return true;
				}
				catch (Exception ex)
				{
					Log.SystemWrite("【Permission】\r\n" + ex.Message + "\r\n" + ex.ToString());
					return false;
				}
			}
		}

		/// <summary>
		/// 修改权限表
		/// </summary>
		/// <param name="PermissionObj">权限表实体</param>
		/// <returns></returns>
		public static bool Update(M_Permission EditPermissionObj)
		{
			using (DBContext db = new DBContext())
			{
				try
				{
					M_Permission PermissionObj = db.PermissionList.Find(EditPermissionObj.ID);
					PermissionObj.Name = EditPermissionObj.Name;
					PermissionObj.ParentId = EditPermissionObj.ParentId;
					PermissionObj.Description = EditPermissionObj.Description;
					PermissionObj.Url = EditPermissionObj.Url;
					PermissionObj.CreatePerson = EditPermissionObj.CreatePerson;
					PermissionObj.CreateTime = EditPermissionObj.CreateTime;

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
					Log.SystemWrite("【Permission】\r\n" + ex.Message + "\r\n\r\n" + sb.ToString());
					return false;
				}
			}
		}
	}
}