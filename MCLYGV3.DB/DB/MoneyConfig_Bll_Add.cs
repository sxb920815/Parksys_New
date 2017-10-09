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
		/// 添加奖金配置表
		/// </summary>
		/// <param name="ProductName">产品名称</param>
		/// <param name="CompanyId">代理公司</param>
		/// <param name="Rate">费率</param>
		/// <param name="ChildRate">默认业务员费率</param>

		/// <returns></returns>
		public static M_MoneyConfig Add(string ProductName,int CompanyId,decimal Rate,decimal ChildRate)
		{
			M_MoneyConfig MoneyConfigObj = new M_MoneyConfig();
			MoneyConfigObj.ProductName = ProductName;
			MoneyConfigObj.CompanyId = CompanyId;
			MoneyConfigObj.Rate = Rate;
			MoneyConfigObj.ChildRate = ChildRate;

			return Add(MoneyConfigObj);
		}

		/// <summary>
		/// 添加奖金配置表
		/// </summary>
		/// <param name="MoneyConfigObj">奖金配置表实体</param>
		/// <returns></returns>
		public static M_MoneyConfig Add(M_MoneyConfig MoneyConfigObj)
		{
			using (DBContext db = new DBContext())
			{
				try
				{
					
					db.MoneyConfigList.Add(MoneyConfigObj);
					int result = db.SaveChanges();
					return MoneyConfigObj;
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
					return null;
				}
			}
		}
	}
}