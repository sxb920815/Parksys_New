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
	public partial class B_MoneyConfigChild
	{
		/// <summary>
		/// 添加奖金配置表
		/// </summary>
		/// <param name="ProductName">产品名称</param>
		/// <param name="UserId">业务员</param>
		/// <param name="Rate">费率</param>

		/// <returns></returns>
		public static M_MoneyConfigChild Add(string ProductName,int UserId,decimal Rate)
		{
			M_MoneyConfigChild MoneyConfigChildObj = new M_MoneyConfigChild();
			MoneyConfigChildObj.ProductName = ProductName;
			MoneyConfigChildObj.UserId = UserId;
			MoneyConfigChildObj.Rate = Rate;

			return Add(MoneyConfigChildObj);
		}

		/// <summary>
		/// 添加奖金配置表
		/// </summary>
		/// <param name="MoneyConfigChildObj">奖金配置表实体</param>
		/// <returns></returns>
		public static M_MoneyConfigChild Add(M_MoneyConfigChild MoneyConfigChildObj)
		{
			using (DBContext db = new DBContext())
			{
				try
				{
					
					db.MoneyConfigChildList.Add(MoneyConfigChildObj);
					int result = db.SaveChanges();
					return MoneyConfigChildObj;
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
					Log.SystemWrite("【MoneyConfigChild】\r\n" + ex.Message + "\r\n\r\n" + sb.ToString());
					return null;
				}
			}
		}
	}
}