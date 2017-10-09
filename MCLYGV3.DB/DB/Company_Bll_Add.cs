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
	/// 代理公司数据库操作类
	/// </summary>
	public partial class B_Company
	{
		/// <summary>
		/// 添加代理公司
		/// </summary>
		/// <param name="CompanyName">公司名</param>
		/// <param name="Tel">电话</param>
		/// <param name="Email">信箱</param>
		/// <param name="Address">地址</param>
		/// <param name="Logo">Logo</param>

		/// <returns></returns>
		public static M_Company Add(string CompanyName,string Tel,string Email,string Address,string Logo)
		{
			M_Company CompanyObj = new M_Company();
			CompanyObj.CompanyName = CompanyName;
			CompanyObj.Tel = Tel;
			CompanyObj.Email = Email;
			CompanyObj.Address = Address;
			CompanyObj.Logo = Logo;

			return Add(CompanyObj);
		}

		/// <summary>
		/// 添加代理公司
		/// </summary>
		/// <param name="CompanyObj">代理公司实体</param>
		/// <returns></returns>
		public static M_Company Add(M_Company CompanyObj)
		{
			using (DBContext db = new DBContext())
			{
				try
				{
					
					db.CompanyList.Add(CompanyObj);
					int result = db.SaveChanges();
					return CompanyObj;
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
					Log.SystemWrite("【Company】\r\n" + ex.Message + "\r\n\r\n" + sb.ToString());
					return null;
				}
			}
		}
	}
}