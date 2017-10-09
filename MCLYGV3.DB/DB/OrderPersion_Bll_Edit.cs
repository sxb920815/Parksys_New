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
	public partial class B_OrderPersion
	{
		/// <summary>
		/// 修改订单
		/// </summary>
		/// <param name="OrderPersionObj">订单实体</param>
		/// <returns></returns>
		public static bool UpdateNormal(M_OrderPersion OrderPersionObj)
		{
			using (DBContext db = new DBContext())
			{
				try
				{
					db.OrderPersionList.Attach(OrderPersionObj);
					DbEntityEntry<M_OrderPersion> entry = db.Entry(OrderPersionObj);
					entry.State = EntityState.Modified;
					db.SaveChanges();
					return true;
				}
				catch (Exception ex)
				{
					Log.SystemWrite("【OrderPersion】\r\n" + ex.Message + "\r\n" + ex.ToString());
					return false;
				}
			}
		}

		/// <summary>
		/// 修改订单
		/// </summary>
		/// <param name="OrderPersionObj">订单实体</param>
		/// <returns></returns>
		public static bool Update(M_OrderPersion EditOrderPersionObj)
		{
			using (DBContext db = new DBContext())
			{
				try
				{
					M_OrderPersion OrderPersionObj = db.OrderPersionList.Find(EditOrderPersionObj.ID);
					OrderPersionObj.OrderCode = EditOrderPersionObj.OrderCode;
					OrderPersionObj.Type = EditOrderPersionObj.Type;
					OrderPersionObj.ProfessionCode = EditOrderPersionObj.ProfessionCode;
					OrderPersionObj.ProfessionName = EditOrderPersionObj.ProfessionName;
					OrderPersionObj.RealName = EditOrderPersionObj.RealName;
					OrderPersionObj.IdNum = EditOrderPersionObj.IdNum;
					OrderPersionObj.AcciPremium = EditOrderPersionObj.AcciPremium;
					OrderPersionObj.AcciDutyAount = EditOrderPersionObj.AcciDutyAount;
					OrderPersionObj.MedicalPremium = EditOrderPersionObj.MedicalPremium;
					OrderPersionObj.MedicalDutyAount = EditOrderPersionObj.MedicalDutyAount;
					OrderPersionObj.AllowancePremium = EditOrderPersionObj.AllowancePremium;
					OrderPersionObj.AllowanceDutyAount = EditOrderPersionObj.AllowanceDutyAount;

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
					Log.SystemWrite("【OrderPersion】\r\n" + ex.Message + "\r\n\r\n" + sb.ToString());
					return false;
				}
			}
		}
	}
}