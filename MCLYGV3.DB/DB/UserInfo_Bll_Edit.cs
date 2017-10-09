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
	/// 用户表数据库操作类
	/// </summary>
	public partial class B_UserInfo
	{
		/// <summary>
		/// 修改用户表
		/// </summary>
		/// <param name="UserInfoObj">用户表实体</param>
		/// <returns></returns>
		public static bool UpdateNormal(M_UserInfo UserInfoObj)
		{
			using (DBContext db = new DBContext())
			{
				try
				{
					db.UserInfoList.Attach(UserInfoObj);
					DbEntityEntry<M_UserInfo> entry = db.Entry(UserInfoObj);
					entry.State = EntityState.Modified;
					db.SaveChanges();
					return true;
				}
				catch (Exception ex)
				{
					Log.SystemWrite("【UserInfo】\r\n" + ex.Message + "\r\n" + ex.ToString());
					return false;
				}
			}
		}

		/// <summary>
		/// 修改用户表
		/// </summary>
		/// <param name="UserInfoObj">用户表实体</param>
		/// <returns></returns>
		public static bool Update(M_UserInfo EditUserInfoObj)
		{
			using (DBContext db = new DBContext())
			{
				try
				{
					M_UserInfo UserInfoObj = db.UserInfoList.Find(EditUserInfoObj.ID);
					UserInfoObj.UserName = EditUserInfoObj.UserName;
					UserInfoObj.RealName = EditUserInfoObj.RealName;
					UserInfoObj.PassWord = EditUserInfoObj.PassWord;
					UserInfoObj.Tel = EditUserInfoObj.Tel;
					UserInfoObj.Email = EditUserInfoObj.Email;
					if (EditUserInfoObj.InCompany != null)
						UserInfoObj.InCompany = db.CompanyList.Find( EditUserInfoObj.InCompany.ID);
					else
						UserInfoObj.InCompany = null;

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
					Log.SystemWrite("【UserInfo】\r\n" + ex.Message + "\r\n\r\n" + sb.ToString());
					return false;
				}
			}
		}
	}
}