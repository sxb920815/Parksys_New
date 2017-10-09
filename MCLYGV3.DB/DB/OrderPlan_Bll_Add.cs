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
		/// 添加订单
		/// </summary>
		/// <param name="ProfessionCode">职业代码</param>
		/// <param name="ProfessionName">职业名称</param>
		/// <param name="PlanCode">险种代码</param>
		/// <param name="DutyCode">责任代码</param>
		/// <param name="ModalPremium">保费</param>
		/// <param name="DutyAount">保额</param>

		/// <returns></returns>
		public static M_OrderPlan Add(string ProfessionCode,string ProfessionName,string PlanCode,string DutyCode,decimal ModalPremium,decimal DutyAount)
		{
			M_OrderPlan OrderPlanObj = new M_OrderPlan();
			OrderPlanObj.ProfessionCode = ProfessionCode;
			OrderPlanObj.ProfessionName = ProfessionName;
			OrderPlanObj.PlanCode = PlanCode;
			OrderPlanObj.DutyCode = DutyCode;
			OrderPlanObj.ModalPremium = ModalPremium;
			OrderPlanObj.DutyAount = DutyAount;

			return Add(OrderPlanObj);
		}

		/// <summary>
		/// 添加订单
		/// </summary>
		/// <param name="OrderPlanObj">订单实体</param>
		/// <returns></returns>
		public static M_OrderPlan Add(M_OrderPlan OrderPlanObj)
		{
			using (DBContext db = new DBContext())
			{
				try
				{
					
					db.OrderPlanList.Add(OrderPlanObj);
					int result = db.SaveChanges();
					return OrderPlanObj;
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
					return null;
				}
			}
		}
	}
}