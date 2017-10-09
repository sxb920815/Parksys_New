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
	public partial class B_Order
	{
		/// <summary>
		/// 修改订单
		/// </summary>
		/// <param name="OrderObj">订单实体</param>
		/// <returns></returns>
		public static bool UpdateNormal(M_Order OrderObj)
		{
			using (DBContext db = new DBContext())
			{
				try
				{
					db.OrderList.Attach(OrderObj);
					DbEntityEntry<M_Order> entry = db.Entry(OrderObj);
					entry.State = EntityState.Modified;
					db.SaveChanges();
					return true;
				}
				catch (Exception ex)
				{
					Log.SystemWrite("【Order】\r\n" + ex.Message + "\r\n" + ex.ToString());
					return false;
				}
			}
		}

		/// <summary>
		/// 修改订单
		/// </summary>
		/// <param name="OrderObj">订单实体</param>
		/// <returns></returns>
		public static bool Update(M_Order EditOrderObj)
		{
			using (DBContext db = new DBContext())
			{
				try
				{
					M_Order OrderObj = db.OrderList.Find(EditOrderObj.OrderCode);
					OrderObj.UserId = EditOrderObj.UserId;
					OrderObj.OrderStep = EditOrderObj.OrderStep;
					OrderObj.InsuredName = EditOrderObj.InsuredName;
					OrderObj.IdentifyNumber = EditOrderObj.IdentifyNumber;
					OrderObj.IdentifyPic = EditOrderObj.IdentifyPic;
					OrderObj.Surcharge = EditOrderObj.Surcharge;
					OrderObj.FirstModalPremium = EditOrderObj.FirstModalPremium;
					OrderObj.FirstdutyAount = EditOrderObj.FirstdutyAount;
					OrderObj.NowdutyAount = EditOrderObj.NowdutyAount;
					OrderObj.NowModalPremium = EditOrderObj.NowModalPremium;
					OrderObj.InsuranceCompany = EditOrderObj.InsuranceCompany;
					OrderObj.CodInd = EditOrderObj.CodInd;
					OrderObj.OrderType = EditOrderObj.OrderType;
					OrderObj.StartTime = EditOrderObj.StartTime;
					OrderObj.EndTime = EditOrderObj.EndTime;
					OrderObj.BuyTime = EditOrderObj.BuyTime;
					OrderObj.PayTime = EditOrderObj.PayTime;
					OrderObj.applyMonth = EditOrderObj.applyMonth;
					OrderObj.PolicyNo = EditOrderObj.PolicyNo;
					OrderObj.Email = EditOrderObj.Email;
					OrderObj.Tel = EditOrderObj.Tel;
					OrderObj.encryptString = EditOrderObj.encryptString;

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
					Log.SystemWrite("【Order】\r\n" + ex.Message + "\r\n\r\n" + sb.ToString());
					return false;
				}
			}
		}
	}
}