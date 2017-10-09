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
		/// 修改订单
		/// </summary>
		/// <param name="OrderChildObj">订单实体</param>
		/// <returns></returns>
		public static bool UpdateNormal(M_OrderChild OrderChildObj)
		{
			using (DBContext db = new DBContext())
			{
				try
				{
					db.OrderChildList.Attach(OrderChildObj);
					DbEntityEntry<M_OrderChild> entry = db.Entry(OrderChildObj);
					entry.State = EntityState.Modified;
					db.SaveChanges();
					return true;
				}
				catch (Exception ex)
				{
					Log.SystemWrite("【OrderChild】\r\n" + ex.Message + "\r\n" + ex.ToString());
					return false;
				}
			}
		}

		/// <summary>
		/// 修改订单
		/// </summary>
		/// <param name="OrderChildObj">订单实体</param>
		/// <returns></returns>
		public static bool Update(M_OrderChild EditOrderChildObj)
		{
			using (DBContext db = new DBContext())
			{
				try
				{
					M_OrderChild OrderChildObj = db.OrderChildList.Find(EditOrderChildObj.ChildCode);
					OrderChildObj.OrderCode = EditOrderChildObj.OrderCode;
					OrderChildObj.Step = EditOrderChildObj.Step;
					OrderChildObj.Type = EditOrderChildObj.Type;
					OrderChildObj.dutyAount = EditOrderChildObj.dutyAount;
					OrderChildObj.ModalPremium = EditOrderChildObj.ModalPremium;
					OrderChildObj.Surcharge = EditOrderChildObj.Surcharge;
					OrderChildObj.Ftrno = EditOrderChildObj.Ftrno;
					OrderChildObj.EndorNo = EditOrderChildObj.EndorNo;
					OrderChildObj.BUSINESS_NO = EditOrderChildObj.BUSINESS_NO;
					OrderChildObj.BK_SERIAL	 = EditOrderChildObj.BK_SERIAL	;
					OrderChildObj.applyPolicyNo = EditOrderChildObj.applyPolicyNo;
					OrderChildObj.encryptString = EditOrderChildObj.encryptString;
					OrderChildObj.CreateTime = EditOrderChildObj.CreateTime;
					OrderChildObj.ValidDate = EditOrderChildObj.ValidDate;
					OrderChildObj.PayTime = EditOrderChildObj.PayTime;

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
					Log.SystemWrite("【OrderChild】\r\n" + ex.Message + "\r\n\r\n" + sb.ToString());
					return false;
				}
			}
		}
	}
}