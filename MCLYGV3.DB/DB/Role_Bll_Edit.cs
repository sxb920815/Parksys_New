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
		/// 修改角色表
		/// </summary>
		/// <param name="RoleObj">角色表实体</param>
		/// <returns></returns>
		public static bool UpdateNormal(M_Role RoleObj)
		{
			using (DBContext db = new DBContext())
			{
				try
				{
					db.RoleList.Attach(RoleObj);
					DbEntityEntry<M_Role> entry = db.Entry(RoleObj);
					entry.State = EntityState.Modified;
					db.SaveChanges();
					return true;
				}
				catch (Exception ex)
				{
					Log.SystemWrite("【Role】\r\n" + ex.Message + "\r\n" + ex.ToString());
					return false;
				}
			}
		}

		/// <summary>
		/// 修改角色表
		/// </summary>
		/// <param name="RoleObj">角色表实体</param>
		/// <returns></returns>
		public static bool Update(M_Role EditRoleObj)
		{
			using (DBContext db = new DBContext())
			{
				try
				{
					M_Role RoleObj = db.RoleList.Find(EditRoleObj.ID);
					RoleObj.Name = EditRoleObj.Name;
					RoleObj.Description = EditRoleObj.Description;
					RoleObj.CreateTime = EditRoleObj.CreateTime;
					RoleObj.CreatePerson = EditRoleObj.CreatePerson;
					RoleObj.Enabled = EditRoleObj.Enabled;

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
					Log.SystemWrite("【Role】\r\n" + ex.Message + "\r\n\r\n" + sb.ToString());
					return false;
				}
			}
		}
	}
}