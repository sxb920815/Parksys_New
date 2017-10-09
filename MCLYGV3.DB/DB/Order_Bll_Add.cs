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
		/// 添加订单
		/// </summary>
		/// <param name="OrderCode">订单编号</param>
		/// <param name="UserId">用户ID</param>
		/// <param name="OrderStep">订单步骤。0未支付1已支付2已过期</param>
		/// <param name="InsuredName">投保人公司名</param>
		/// <param name="IdentifyNumber">投保人公司编号</param>
		/// <param name="IdentifyPic">证件图片ID</param>
		/// <param name="Surcharge">附加费</param>
		/// <param name="FirstModalPremium">初次总保费</param>
		/// <param name="FirstdutyAount">初次总保额</param>
		/// <param name="NowdutyAount">当前总保费</param>
		/// <param name="NowModalPremium">当前总保额</param>
		/// <param name="InsuranceCompany">保险公司。CT或者PA</param>
		/// <param name="CodInd">见费出单。Y或者N</param>
		/// <param name="OrderType">保单类别。Group</param>
		/// <param name="StartTime">保险生效时间</param>
		/// <param name="EndTime">保险失效时间</param>
		/// <param name="BuyTime">保险提交时间</param>
		/// <param name="PayTime">付款返回时间</param>
		/// <param name="applyMonth">投保月份</param>
		/// <param name="PolicyNo">保单号</param>
		/// <param name="Email">电子信箱</param>
		/// <param name="Tel">电话号码</param>
		/// <param name="encryptString">电子保单密文</param>

		/// <returns></returns>
		public static M_Order Add(string OrderCode,int UserId,int OrderStep,string InsuredName,string IdentifyNumber,string IdentifyPic,decimal Surcharge,decimal FirstModalPremium,decimal FirstdutyAount,decimal NowdutyAount,decimal NowModalPremium,string InsuranceCompany,string CodInd,string OrderType,DateTime StartTime,DateTime EndTime,DateTime BuyTime,DateTime PayTime,int applyMonth,string PolicyNo,string Email,string Tel,string encryptString)
		{
			M_Order OrderObj = new M_Order();
			OrderObj.OrderCode = OrderCode;
			OrderObj.UserId = UserId;
			OrderObj.OrderStep = OrderStep;
			OrderObj.InsuredName = InsuredName;
			OrderObj.IdentifyNumber = IdentifyNumber;
			OrderObj.IdentifyPic = IdentifyPic;
			OrderObj.Surcharge = Surcharge;
			OrderObj.FirstModalPremium = FirstModalPremium;
			OrderObj.FirstdutyAount = FirstdutyAount;
			OrderObj.NowdutyAount = NowdutyAount;
			OrderObj.NowModalPremium = NowModalPremium;
			OrderObj.InsuranceCompany = InsuranceCompany;
			OrderObj.CodInd = CodInd;
			OrderObj.OrderType = OrderType;
			OrderObj.StartTime = StartTime;
			OrderObj.EndTime = EndTime;
			OrderObj.BuyTime = BuyTime;
			OrderObj.PayTime = PayTime;
			OrderObj.applyMonth = applyMonth;
			OrderObj.PolicyNo = PolicyNo;
			OrderObj.Email = Email;
			OrderObj.Tel = Tel;
			OrderObj.encryptString = encryptString;

			return Add(OrderObj);
		}

		/// <summary>
		/// 添加订单
		/// </summary>
		/// <param name="OrderObj">订单实体</param>
		/// <returns></returns>
		public static M_Order Add(M_Order OrderObj)
		{
			using (DBContext db = new DBContext())
			{
				try
				{
					
					db.OrderList.Add(OrderObj);
					int result = db.SaveChanges();
					return OrderObj;
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
					return null;
				}
			}
		}
	}
}