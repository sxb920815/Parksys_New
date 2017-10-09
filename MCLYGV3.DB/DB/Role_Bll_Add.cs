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
		/// 添加角色表
		/// </summary>
		/// <param name="Name">角色名称</param>
		/// <param name="Description">描述</param>
		/// <param name="CreateTime">创建时间</param>
		/// <param name="CreatePerson">创建用户</param>
		/// <param name="Enabled">是否可用</param>

		/// <returns></returns>
		public static M_Role Add(string Name,string Description,DateTime CreateTime,string CreatePerson,bool Enabled)
		{
			M_Role RoleObj = new M_Role();
			RoleObj.Name = Name;
			RoleObj.Description = Description;
			RoleObj.CreateTime = CreateTime;
			RoleObj.CreatePerson = CreatePerson;
			RoleObj.Enabled = Enabled;

			return Add(RoleObj);
		}

		/// <summary>
		/// 添加角色表
		/// </summary>
		/// <param name="RoleObj">角色表实体</param>
		/// <returns></returns>
		public static M_Role Add(M_Role RoleObj)
		{
			using (DBContext db = new DBContext())
			{
				try
				{
					
					db.RoleList.Add(RoleObj);
					int result = db.SaveChanges();
					return RoleObj;
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
					return null;
				}
			}
		}
	}
}