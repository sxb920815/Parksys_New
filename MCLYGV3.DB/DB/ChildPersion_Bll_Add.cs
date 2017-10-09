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
		/// 添加订单
		/// </summary>
		/// <param name="ChildCode">订单编号或子订单编号</param>
		/// <param name="ProfessionCode">职业代码</param>
		/// <param name="ProfessionName">职业名称</param>
		/// <param name="RealName">姓名</param>
		/// <param name="IdNum">身份证号</param>
		/// <param name="AcciPremium">主险意外伤害保费</param>
		/// <param name="AcciDutyAount">主险意外伤害保额</param>
		/// <param name="MedicalPremium">附加医疗保费</param>
		/// <param name="MedicalDutyAount">附加医疗保额</param>
		/// <param name="AllowancePremium">住院津贴保费</param>
		/// <param name="AllowanceDutyAount">住院津贴保额</param>

		/// <returns></returns>
		public static M_ChildPersion Add(string ChildCode,string ProfessionCode,string ProfessionName,string RealName,string IdNum,decimal AcciPremium,decimal AcciDutyAount,decimal MedicalPremium,decimal MedicalDutyAount,decimal AllowancePremium,decimal AllowanceDutyAount)
		{
			M_ChildPersion ChildPersionObj = new M_ChildPersion();
			ChildPersionObj.ChildCode = ChildCode;
			ChildPersionObj.ProfessionCode = ProfessionCode;
			ChildPersionObj.ProfessionName = ProfessionName;
			ChildPersionObj.RealName = RealName;
			ChildPersionObj.IdNum = IdNum;
			ChildPersionObj.AcciPremium = AcciPremium;
			ChildPersionObj.AcciDutyAount = AcciDutyAount;
			ChildPersionObj.MedicalPremium = MedicalPremium;
			ChildPersionObj.MedicalDutyAount = MedicalDutyAount;
			ChildPersionObj.AllowancePremium = AllowancePremium;
			ChildPersionObj.AllowanceDutyAount = AllowanceDutyAount;

			return Add(ChildPersionObj);
		}

		/// <summary>
		/// 添加订单
		/// </summary>
		/// <param name="ChildPersionObj">订单实体</param>
		/// <returns></returns>
		public static M_ChildPersion Add(M_ChildPersion ChildPersionObj)
		{
			using (DBContext db = new DBContext())
			{
				try
				{
					
					db.ChildPersionList.Add(ChildPersionObj);
					int result = db.SaveChanges();
					return ChildPersionObj;
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
					return null;
				}
			}
		}
	}
}