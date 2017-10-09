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
	/// 产品表数据库操作类
	/// </summary>
	public partial class B_MoneyProduct
	{
		/// <summary>
		/// 修改产品表
		/// </summary>
		/// <param name="MoneyProductObj">产品表实体</param>
		/// <returns></returns>
		public static bool UpdateNormal(M_MoneyProduct MoneyProductObj)
		{
			using (DBContext db = new DBContext())
			{
				try
				{
					db.MoneyProductList.Attach(MoneyProductObj);
					DbEntityEntry<M_MoneyProduct> entry = db.Entry(MoneyProductObj);
					entry.State = EntityState.Modified;
					db.SaveChanges();
					return true;
				}
				catch (Exception ex)
				{
					Log.SystemWrite("【MoneyProduct】\r\n" + ex.Message + "\r\n" + ex.ToString());
					return false;
				}
			}
		}

		/// <summary>
		/// 修改产品表
		/// </summary>
		/// <param name="MoneyProductObj">产品表实体</param>
		/// <returns></returns>
		public static bool Update(M_MoneyProduct EditMoneyProductObj)
		{
			using (DBContext db = new DBContext())
			{
				try
				{
					M_MoneyProduct MoneyProductObj = db.MoneyProductList.Find(EditMoneyProductObj.ID);
					MoneyProductObj.ProductName = EditMoneyProductObj.ProductName;
					MoneyProductObj.ChineseName = EditMoneyProductObj.ChineseName;
					MoneyProductObj.CompanyName = EditMoneyProductObj.CompanyName;
					MoneyProductObj.CompanyRate = EditMoneyProductObj.CompanyRate;

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
					Log.SystemWrite("【MoneyProduct】\r\n" + ex.Message + "\r\n\r\n" + sb.ToString());
					return false;
				}
			}
		}
	}
}