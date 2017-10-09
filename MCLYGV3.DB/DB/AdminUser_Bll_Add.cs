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
		/// 添加管理员
		/// </summary>
		/// <param name="ID">ID</param>
		/// <param name="UserName">用户名</param>
		/// <param name="RealName">真实姓名</param>
		/// <param name="PassWord">登录密码</param>
		/// <param name="IsSupper">是否超级管理员</param>
		/// <param name="RegTime">注册时间</param>
		/// <param name="NowTime">本次时间</param>
		/// <param name="LastTime">上次登录时间</param>
		/// <param name="Rate1">提成系数1</param>
		/// <param name="Rate2">提成系数2</param>
		/// <param name="Rate3">提成系数3</param>
		/// <param name="Rate4">提成系数4</param>
		/// <param name="Rate5">提成系数5</param>
		/// <param name="Rate6">提成系数6</param>
		/// <param name="InCompanyId">所属公司ID</param>

		/// <returns></returns>
		public static M_AdminUser Add(int ID,string UserName,string RealName,string PassWord,bool IsSupper,DateTime RegTime,DateTime NowTime,DateTime LastTime,decimal Rate1,decimal Rate2,decimal Rate3,decimal Rate4,decimal Rate5,decimal Rate6,int InCompanyId)
		{
			M_AdminUser AdminUserObj = new M_AdminUser();
			AdminUserObj.ID = ID;
			AdminUserObj.UserName = UserName;
			AdminUserObj.RealName = RealName;
			AdminUserObj.PassWord = PassWord;
			AdminUserObj.IsSupper = IsSupper;
			AdminUserObj.RegTime = RegTime;
			AdminUserObj.NowTime = NowTime;
			AdminUserObj.LastTime = LastTime;
			AdminUserObj.Rate1 = Rate1;
			AdminUserObj.Rate2 = Rate2;
			AdminUserObj.Rate3 = Rate3;
			AdminUserObj.Rate4 = Rate4;
			AdminUserObj.Rate5 = Rate5;
			AdminUserObj.Rate6 = Rate6;
			AdminUserObj.InCompanyId = InCompanyId;

			return Add(AdminUserObj);
		}

		/// <summary>
		/// 添加管理员
		/// </summary>
		/// <param name="AdminUserObj">管理员实体</param>
		/// <returns></returns>
		public static M_AdminUser Add(M_AdminUser AdminUserObj)
		{
			using (DBContext db = new DBContext())
			{
				try
				{
					
					db.AdminUserList.Add(AdminUserObj);
					int result = db.SaveChanges();
					return AdminUserObj;
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
					return null;
				}
			}
		}
	}
}