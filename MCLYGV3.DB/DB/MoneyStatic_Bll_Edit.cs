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
	/// 奖金表数据库操作类
	/// </summary>
	public partial class B_MoneyStatic
	{
		/// <summary>
		/// 修改奖金表
		/// </summary>
		/// <param name="MoneyStaticObj">奖金表实体</param>
		/// <returns></returns>
		public static bool UpdateNormal(M_MoneyStatic MoneyStaticObj)
		{
			using (DBContext db = new DBContext())
			{
				try
				{
					db.MoneyStaticList.Attach(MoneyStaticObj);
					DbEntityEntry<M_MoneyStatic> entry = db.Entry(MoneyStaticObj);
					entry.State = EntityState.Modified;
					db.SaveChanges();
					return true;
				}
				catch (Exception ex)
				{
					Log.SystemWrite("【MoneyStatic】\r\n" + ex.Message + "\r\n" + ex.ToString());
					return false;
				}
			}
		}

		/// <summary>
		/// 修改奖金表
		/// </summary>
		/// <param name="MoneyStaticObj">奖金表实体</param>
		/// <returns></returns>
		public static bool Update(M_MoneyStatic EditMoneyStaticObj)
		{
			using (DBContext db = new DBContext())
			{
				try
				{
					M_MoneyStatic MoneyStaticObj = db.MoneyStaticList.Find(EditMoneyStaticObj.ID);
					MoneyStaticObj.CreateTime = EditMoneyStaticObj.CreateTime;
					MoneyStaticObj.PolicyNumber = EditMoneyStaticObj.PolicyNumber;
					MoneyStaticObj.InsuranceCompany = EditMoneyStaticObj.InsuranceCompany;
					MoneyStaticObj.ProductName = EditMoneyStaticObj.ProductName;
					MoneyStaticObj.UserId = EditMoneyStaticObj.UserId;
					MoneyStaticObj.CompanyId = EditMoneyStaticObj.CompanyId;
					MoneyStaticObj.Money = EditMoneyStaticObj.Money;
					MoneyStaticObj.CommissionMoney = EditMoneyStaticObj.CommissionMoney;
					MoneyStaticObj.CompanyMoney = EditMoneyStaticObj.CompanyMoney;
					MoneyStaticObj.UserMoney = EditMoneyStaticObj.UserMoney;
					MoneyStaticObj.IsInsuranceCompanyGive = EditMoneyStaticObj.IsInsuranceCompanyGive;
					MoneyStaticObj.IsCompanyGive = EditMoneyStaticObj.IsCompanyGive;

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
					Log.SystemWrite("【MoneyStatic】\r\n" + ex.Message + "\r\n\r\n" + sb.ToString());
					return false;
				}
			}
		}
	}
}