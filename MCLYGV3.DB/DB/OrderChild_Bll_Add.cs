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
	public partial class B_OrderChild
	{
		/// <summary>
		/// 添加订单
		/// </summary>
		/// <param name="ChildCode">子订单编号</param>
		/// <param name="OrderCode">订单编号</param>
		/// <param name="Step">订单步骤。0未支付1已完成2取消</param>
		/// <param name="Type">类别。0新增1批增2批减</param>
		/// <param name="dutyAount">保额</param>
		/// <param name="ModalPremium">保费</param>
		/// <param name="Surcharge">附加费</param>
		/// <param name="Ftrno">诚泰返回的投保单号</param>
		/// <param name="EndorNo">诚泰返回的批增投保单号</param>
		/// <param name="BUSINESS_NO">平安返回的</param>
		/// <param name="BK_SERIAL	">平安返回的</param>
		/// <param name="applyPolicyNo">平安返回的</param>
		/// <param name="encryptString">电子保单密文</param>
		/// <param name="CreateTime">订单创建时间</param>
		/// <param name="ValidDate">生效时间</param>
		/// <param name="PayTime">支付时间</param>

		/// <returns></returns>
		public static M_OrderChild Add(string ChildCode,string OrderCode,int Step,int Type,decimal dutyAount,decimal ModalPremium,decimal Surcharge,string Ftrno,string EndorNo,string BUSINESS_NO,string BK_SERIAL	,string applyPolicyNo,string encryptString,DateTime CreateTime,DateTime ValidDate,DateTime PayTime)
		{
			M_OrderChild OrderChildObj = new M_OrderChild();
			OrderChildObj.ChildCode = ChildCode;
			OrderChildObj.OrderCode = OrderCode;
			OrderChildObj.Step = Step;
			OrderChildObj.Type = Type;
			OrderChildObj.dutyAount = dutyAount;
			OrderChildObj.ModalPremium = ModalPremium;
			OrderChildObj.Surcharge = Surcharge;
			OrderChildObj.Ftrno = Ftrno;
			OrderChildObj.EndorNo = EndorNo;
			OrderChildObj.BUSINESS_NO = BUSINESS_NO;
			OrderChildObj.BK_SERIAL	 = BK_SERIAL	;
			OrderChildObj.applyPolicyNo = applyPolicyNo;
			OrderChildObj.encryptString = encryptString;
			OrderChildObj.CreateTime = CreateTime;
			OrderChildObj.ValidDate = ValidDate;
			OrderChildObj.PayTime = PayTime;

			return Add(OrderChildObj);
		}

		/// <summary>
		/// 添加订单
		/// </summary>
		/// <param name="OrderChildObj">订单实体</param>
		/// <returns></returns>
		public static M_OrderChild Add(M_OrderChild OrderChildObj)
		{
			using (DBContext db = new DBContext())
			{
				try
				{
					
					db.OrderChildList.Add(OrderChildObj);
					int result = db.SaveChanges();
					return OrderChildObj;
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
					Log.SystemWrite("【OrderChild】\r\n" + ex.Message + "\r\n\r\n" + sb.ToString());
					return null;
				}
			}
		}
	}
}