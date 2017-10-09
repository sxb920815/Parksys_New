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
	/// 奖金配置表数据库操作类
	/// </summary>
	public partial class B_MoneyConfig
	{
		/// <summary>
		/// 修改奖金配置表
		/// </summary>
		/// <param name="MoneyConfigObj">奖金配置表实体</param>
		/// <returns></returns>
		public static bool UpdateNormal(M_MoneyConfig MoneyConfigObj)
		{
			using (DBContext db = new DBContext())
			{
				try
				{
					db.MoneyConfigList.Attach(MoneyConfigObj);
					DbEntityEntry<M_MoneyConfig> entry = db.Entry(MoneyConfigObj);
					entry.State = EntityState.Modified;
					db.SaveChanges();
					return true;
				}
				catch (Exception ex)
				{
					Log.SystemWrite("【MoneyConfig】\r\n" + ex.Message + "\r\n" + ex.ToString());
					return false;
				}
			}
		}

		/// <summary>
		/// 修改奖金配置表
		/// </summary>
		/// <param name="MoneyConfigObj">奖金配置表实体</param>
		/// <returns></returns>
		public static bool Update(M_MoneyConfig EditMoneyConfigObj)
		{
			using (DBContext db = new DBContext())
			{
				try
				{
					M_MoneyConfig MoneyConfigObj = db.MoneyConfigList.Find(EditMoneyConfigObj.ID);
					MoneyConfigObj.ProductName = EditMoneyConfigObj.ProductName;
					MoneyConfigObj.CompanyId = EditMoneyConfigObj.CompanyId;
					MoneyConfigObj.Rate = EditMoneyConfigObj.Rate;
					MoneyConfigObj.ChildRate = EditMoneyConfigObj.ChildRate;

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
					Log.SystemWrite("【MoneyConfig】\r\n" + ex.Message + "\r\n\r\n" + sb.ToString());
					return false;
				}
			}
		}
	}
}