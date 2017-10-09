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
		/// 添加用户表
		/// </summary>
		/// <param name="ID">ID</param>
		/// <param name="UserName">用户名</param>
		/// <param name="RealName">真实姓名</param>
		/// <param name="PassWord">登录密码</param>
		/// <param name="Tel">电话号码</param>
		/// <param name="Email">信箱</param>
		/// <param name="InCompany">所属公司</param>

		/// <returns></returns>
		public static M_UserInfo Add(int ID,string UserName,string RealName,string PassWord,string Tel,string Email,M_Company InCompany)
		{
			M_UserInfo UserInfoObj = new M_UserInfo();
			UserInfoObj.ID = ID;
			UserInfoObj.UserName = UserName;
			UserInfoObj.RealName = RealName;
			UserInfoObj.PassWord = PassWord;
			UserInfoObj.Tel = Tel;
			UserInfoObj.Email = Email;
			UserInfoObj.InCompany = InCompany;

			return Add(UserInfoObj);
		}

		/// <summary>
		/// 添加用户表
		/// </summary>
		/// <param name="UserInfoObj">用户表实体</param>
		/// <returns></returns>
		public static M_UserInfo Add(M_UserInfo UserInfoObj)
		{
			using (DBContext db = new DBContext())
			{
				try
				{
					if (UserInfoObj.InCompany != null)
						db.CompanyList.Attach(UserInfoObj.InCompany);

					db.UserInfoList.Add(UserInfoObj);
					int result = db.SaveChanges();
					return UserInfoObj;
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
					return null;
				}
			}
		}
	}
}