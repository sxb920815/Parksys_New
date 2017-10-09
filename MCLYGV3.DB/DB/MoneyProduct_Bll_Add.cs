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
		/// 添加产品表
		/// </summary>
		/// <param name="ProductName">产品名称</param>
		/// <param name="ChineseName">产品中文名称</param>
		/// <param name="CompanyName">保险公司名称</param>
		/// <param name="CompanyRate">保险公司给我们的费率</param>

		/// <returns></returns>
		public static M_MoneyProduct Add(string ProductName,string ChineseName,string CompanyName,decimal CompanyRate)
		{
			M_MoneyProduct MoneyProductObj = new M_MoneyProduct();
			MoneyProductObj.ProductName = ProductName;
			MoneyProductObj.ChineseName = ChineseName;
			MoneyProductObj.CompanyName = CompanyName;
			MoneyProductObj.CompanyRate = CompanyRate;

			return Add(MoneyProductObj);
		}

		/// <summary>
		/// 添加产品表
		/// </summary>
		/// <param name="MoneyProductObj">产品表实体</param>
		/// <returns></returns>
		public static M_MoneyProduct Add(M_MoneyProduct MoneyProductObj)
		{
			using (DBContext db = new DBContext())
			{
				try
				{
					
					db.MoneyProductList.Add(MoneyProductObj);
					int result = db.SaveChanges();
					return MoneyProductObj;
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
					return null;
				}
			}
		}
	}
}