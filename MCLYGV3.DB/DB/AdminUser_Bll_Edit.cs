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
	/// 管理员数据库操作类
	/// </summary>
	public partial class B_AdminUser
	{
		/// <summary>
		/// 修改管理员
		/// </summary>
		/// <param name="AdminUserObj">管理员实体</param>
		/// <returns></returns>
		public static bool UpdateNormal(M_AdminUser AdminUserObj)
		{
			using (DBContext db = new DBContext())
			{
				try
				{
					db.AdminUserList.Attach(AdminUserObj);
					DbEntityEntry<M_AdminUser> entry = db.Entry(AdminUserObj);
					entry.State = EntityState.Modified;
					db.SaveChanges();
					return true;
				}
				catch (Exception ex)
				{
					Log.SystemWrite("【AdminUser】\r\n" + ex.Message + "\r\n" + ex.ToString());
					return false;
				}
			}
		}

		/// <summary>
		/// 修改管理员
		/// </summary>
		/// <param name="AdminUserObj">管理员实体</param>
		/// <returns></returns>
		public static bool Update(M_AdminUser EditAdminUserObj)
		{
			using (DBContext db = new DBContext())
			{
				try
				{
					M_AdminUser AdminUserObj = db.AdminUserList.Find(EditAdminUserObj.ID);
					AdminUserObj.UserName = EditAdminUserObj.UserName;
					AdminUserObj.RealName = EditAdminUserObj.RealName;
					AdminUserObj.PassWord = EditAdminUserObj.PassWord;
					AdminUserObj.IsSupper = EditAdminUserObj.IsSupper;
					AdminUserObj.RegTime = EditAdminUserObj.RegTime;
					AdminUserObj.NowTime = EditAdminUserObj.NowTime;
					AdminUserObj.LastTime = EditAdminUserObj.LastTime;
					AdminUserObj.Rate1 = EditAdminUserObj.Rate1;
					AdminUserObj.Rate2 = EditAdminUserObj.Rate2;
					AdminUserObj.Rate3 = EditAdminUserObj.Rate3;
					AdminUserObj.Rate4 = EditAdminUserObj.Rate4;
					AdminUserObj.Rate5 = EditAdminUserObj.Rate5;
					AdminUserObj.Rate6 = EditAdminUserObj.Rate6;
					AdminUserObj.InCompanyId = EditAdminUserObj.InCompanyId;

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
					Log.SystemWrite("【AdminUser】\r\n" + ex.Message + "\r\n\r\n" + sb.ToString());
					return false;
				}
			}
		}
	}
}