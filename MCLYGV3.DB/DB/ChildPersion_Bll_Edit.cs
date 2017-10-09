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
	/// 订单数据库操作类
	/// </summary>
	public partial class B_ChildPersion
	{
		/// <summary>
		/// 修改订单
		/// </summary>
		/// <param name="ChildPersionObj">订单实体</param>
		/// <returns></returns>
		public static bool UpdateNormal(M_ChildPersion ChildPersionObj)
		{
			using (DBContext db = new DBContext())
			{
				try
				{
					db.ChildPersionList.Attach(ChildPersionObj);
					DbEntityEntry<M_ChildPersion> entry = db.Entry(ChildPersionObj);
					entry.State = EntityState.Modified;
					db.SaveChanges();
					return true;
				}
				catch (Exception ex)
				{
					Log.SystemWrite("【ChildPersion】\r\n" + ex.Message + "\r\n" + ex.ToString());
					return false;
				}
			}
		}

		/// <summary>
		/// 修改订单
		/// </summary>
		/// <param name="ChildPersionObj">订单实体</param>
		/// <returns></returns>
		public static bool Update(M_ChildPersion EditChildPersionObj)
		{
			using (DBContext db = new DBContext())
			{
				try
				{
					M_ChildPersion ChildPersionObj = db.ChildPersionList.Find(EditChildPersionObj.ID);
					ChildPersionObj.ChildCode = EditChildPersionObj.ChildCode;
					ChildPersionObj.ProfessionCode = EditChildPersionObj.ProfessionCode;
					ChildPersionObj.ProfessionName = EditChildPersionObj.ProfessionName;
					ChildPersionObj.RealName = EditChildPersionObj.RealName;
					ChildPersionObj.IdNum = EditChildPersionObj.IdNum;
					ChildPersionObj.AcciPremium = EditChildPersionObj.AcciPremium;
					ChildPersionObj.AcciDutyAount = EditChildPersionObj.AcciDutyAount;
					ChildPersionObj.MedicalPremium = EditChildPersionObj.MedicalPremium;
					ChildPersionObj.MedicalDutyAount = EditChildPersionObj.MedicalDutyAount;
					ChildPersionObj.AllowancePremium = EditChildPersionObj.AllowancePremium;
					ChildPersionObj.AllowanceDutyAount = EditChildPersionObj.AllowanceDutyAount;

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
					Log.SystemWrite("【ChildPersion】\r\n" + ex.Message + "\r\n\r\n" + sb.ToString());
					return false;
				}
			}
		}
	}
}