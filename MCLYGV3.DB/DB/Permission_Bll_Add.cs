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
		/// 添加权限表
		/// </summary>
		/// <param name="ID">ID</param>
		/// <param name="Name">权限名称</param>
		/// <param name="ParentId">上级ID</param>
		/// <param name="Description">描述</param>
		/// <param name="Url">网址</param>
		/// <param name="CreatePerson">创建人</param>
		/// <param name="CreateTime">创建时间</param>

		/// <returns></returns>
		public static M_Permission Add(string ID,string Name,string ParentId,string Description,string Url,string CreatePerson,string CreateTime)
		{
			M_Permission PermissionObj = new M_Permission();
			PermissionObj.ID = ID;
			PermissionObj.Name = Name;
			PermissionObj.ParentId = ParentId;
			PermissionObj.Description = Description;
			PermissionObj.Url = Url;
			PermissionObj.CreatePerson = CreatePerson;
			PermissionObj.CreateTime = CreateTime;

			return Add(PermissionObj);
		}

		/// <summary>
		/// 添加权限表
		/// </summary>
		/// <param name="PermissionObj">权限表实体</param>
		/// <returns></returns>
		public static M_Permission Add(M_Permission PermissionObj)
		{
			using (DBContext db = new DBContext())
			{
				try
				{
					
					db.PermissionList.Add(PermissionObj);
					int result = db.SaveChanges();
					return PermissionObj;
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
					return null;
				}
			}
		}
	}
}