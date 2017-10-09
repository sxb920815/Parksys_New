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
		/// 添加订单
		/// </summary>
		/// <param name="OrderCode">订单编号或子订单编号</param>
		/// <param name="Type">0订单1子订单</param>
		/// <param name="ProfessionCode">职业代码</param>
		/// <param name="ProfessionName">职业名称</param>
		/// <param name="RealName">姓名</param>
		/// <param name="IdNum">身份证号</param>
		/// <param name="AcciPremium">主险意外伤害保费</param>
		/// <param name="AcciDutyAount">主险意外伤害保额</param>
		/// <param name="MedicalPremium">附加医疗保费</param>
		/// <param name="MedicalDutyAount">附加医疗保额</param>
		/// <param name="AllowancePremium">住院津贴保额</param>
		/// <param name="AllowanceDutyAount">住院津贴保费</param>

		/// <returns></returns>
		public static M_OrderPersion Add(string OrderCode,int Type,string ProfessionCode,string ProfessionName,string RealName,string IdNum,decimal AcciPremium,decimal AcciDutyAount,decimal MedicalPremium,decimal MedicalDutyAount,decimal AllowancePremium,decimal AllowanceDutyAount)
		{
			M_OrderPersion OrderPersionObj = new M_OrderPersion();
			OrderPersionObj.OrderCode = OrderCode;
			OrderPersionObj.Type = Type;
			OrderPersionObj.ProfessionCode = ProfessionCode;
			OrderPersionObj.ProfessionName = ProfessionName;
			OrderPersionObj.RealName = RealName;
			OrderPersionObj.IdNum = IdNum;
			OrderPersionObj.AcciPremium = AcciPremium;
			OrderPersionObj.AcciDutyAount = AcciDutyAount;
			OrderPersionObj.MedicalPremium = MedicalPremium;
			OrderPersionObj.MedicalDutyAount = MedicalDutyAount;
			OrderPersionObj.AllowancePremium = AllowancePremium;
			OrderPersionObj.AllowanceDutyAount = AllowanceDutyAount;

			return Add(OrderPersionObj);
		}

		/// <summary>
		/// 添加订单
		/// </summary>
		/// <param name="OrderPersionObj">订单实体</param>
		/// <returns></returns>
		public static M_OrderPersion Add(M_OrderPersion OrderPersionObj)
		{
			using (DBContext db = new DBContext())
			{
				try
				{
					
					db.OrderPersionList.Add(OrderPersionObj);
					int result = db.SaveChanges();
					return OrderPersionObj;
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
					return null;
				}
			}
		}
	}
}