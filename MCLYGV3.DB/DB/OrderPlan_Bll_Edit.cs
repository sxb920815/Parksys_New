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
	public partial class B_OrderPlan
	{
		/// <summary>
		/// 修改订单
		/// </summary>
		/// <param name="OrderPlanObj">订单实体</param>
		/// <returns></returns>
		public static bool UpdateNormal(M_OrderPlan OrderPlanObj)
		{
			using (DBContext db = new DBContext())
			{
				try
				{
					db.OrderPlanList.Attach(OrderPlanObj);
					DbEntityEntry<M_OrderPlan> entry = db.Entry(OrderPlanObj);
					entry.State = EntityState.Modified;
					db.SaveChanges();
					return true;
				}
				catch (Exception ex)
				{
					Log.SystemWrite("【OrderPlan】\r\n" + ex.Message + "\r\n" + ex.ToString());
					return false;
				}
			}
		}

		/// <summary>
		/// 修改订单
		/// </summary>
		/// <param name="OrderPlanObj">订单实体</param>
		/// <returns></returns>
		public static bool Update(M_OrderPlan EditOrderPlanObj)
		{
			using (DBContext db = new DBContext())
			{
				try
				{
					M_OrderPlan OrderPlanObj = db.OrderPlanList.Find(EditOrderPlanObj.ID);
					OrderPlanObj.ProfessionCode = EditOrderPlanObj.ProfessionCode;
					OrderPlanObj.ProfessionName = EditOrderPlanObj.ProfessionName;
					OrderPlanObj.PlanCode = EditOrderPlanObj.PlanCode;
					OrderPlanObj.DutyCode = EditOrderPlanObj.DutyCode;
					OrderPlanObj.ModalPremium = EditOrderPlanObj.ModalPremium;
					OrderPlanObj.DutyAount = EditOrderPlanObj.DutyAount;

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
					Log.SystemWrite("【OrderPlan】\r\n" + ex.Message + "\r\n\r\n" + sb.ToString());
					return false;
				}
			}
		}
	}
}