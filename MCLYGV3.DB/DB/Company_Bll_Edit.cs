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
		/// 修改代理公司
		/// </summary>
		/// <param name="CompanyObj">代理公司实体</param>
		/// <returns></returns>
		public static bool UpdateNormal(M_Company CompanyObj)
		{
			using (DBContext db = new DBContext())
			{
				try
				{
					db.CompanyList.Attach(CompanyObj);
					DbEntityEntry<M_Company> entry = db.Entry(CompanyObj);
					entry.State = EntityState.Modified;
					db.SaveChanges();
					return true;
				}
				catch (Exception ex)
				{
					Log.SystemWrite("【Company】\r\n" + ex.Message + "\r\n" + ex.ToString());
					return false;
				}
			}
		}

		/// <summary>
		/// 修改代理公司
		/// </summary>
		/// <param name="CompanyObj">代理公司实体</param>
		/// <returns></returns>
		public static bool Update(M_Company EditCompanyObj)
		{
			using (DBContext db = new DBContext())
			{
				try
				{
					M_Company CompanyObj = db.CompanyList.Find(EditCompanyObj.ID);
					CompanyObj.CompanyName = EditCompanyObj.CompanyName;
					CompanyObj.Tel = EditCompanyObj.Tel;
					CompanyObj.Email = EditCompanyObj.Email;
					CompanyObj.Address = EditCompanyObj.Address;
					CompanyObj.Logo = EditCompanyObj.Logo;

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
					Log.SystemWrite("【Company】\r\n" + ex.Message + "\r\n\r\n" + sb.ToString());
					return false;
				}
			}
		}
	}
}