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
		/// 添加奖金表
		/// </summary>
		/// <param name="CreateTime">时间</param>
		/// <param name="PolicyNumber">保单号</param>
		/// <param name="InsuranceCompany">保险公司</param>
		/// <param name="ProductName">产品</param>
		/// <param name="UserId">业务员</param>
		/// <param name="CompanyId">代理公司</param>
		/// <param name="Money">订单金额</param>
		/// <param name="CommissionMoney">总提成</param>
		/// <param name="CompanyMoney">代理公司提成</param>
		/// <param name="UserMoney">业务员提成</param>
		/// <param name="IsInsuranceCompanyGive">保险公司是否结算</param>
		/// <param name="IsCompanyGive">代理公司是否结算</param>

		/// <returns></returns>
		public static M_MoneyStatic Add(DateTime CreateTime,string PolicyNumber,string InsuranceCompany,string ProductName,int UserId,int CompanyId,decimal Money,decimal CommissionMoney,decimal CompanyMoney,decimal UserMoney,bool IsInsuranceCompanyGive,bool IsCompanyGive)
		{
			M_MoneyStatic MoneyStaticObj = new M_MoneyStatic();
			MoneyStaticObj.CreateTime = CreateTime;
			MoneyStaticObj.PolicyNumber = PolicyNumber;
			MoneyStaticObj.InsuranceCompany = InsuranceCompany;
			MoneyStaticObj.ProductName = ProductName;
			MoneyStaticObj.UserId = UserId;
			MoneyStaticObj.CompanyId = CompanyId;
			MoneyStaticObj.Money = Money;
			MoneyStaticObj.CommissionMoney = CommissionMoney;
			MoneyStaticObj.CompanyMoney = CompanyMoney;
			MoneyStaticObj.UserMoney = UserMoney;
			MoneyStaticObj.IsInsuranceCompanyGive = IsInsuranceCompanyGive;
			MoneyStaticObj.IsCompanyGive = IsCompanyGive;

			return Add(MoneyStaticObj);
		}

		/// <summary>
		/// 添加奖金表
		/// </summary>
		/// <param name="MoneyStaticObj">奖金表实体</param>
		/// <returns></returns>
		public static M_MoneyStatic Add(M_MoneyStatic MoneyStaticObj)
		{
			using (DBContext db = new DBContext())
			{
				try
				{
					
					db.MoneyStaticList.Add(MoneyStaticObj);
					int result = db.SaveChanges();
					return MoneyStaticObj;
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
					return null;
				}
			}
		}
	}
}