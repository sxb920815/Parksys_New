using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Transactions;
using MCLYGV3.Web;
using MCLYGV3.Web.Models;

namespace MCLYGV3.DB
{
	/// <summary>
	/// 管理员数据库操作类
	/// </summary>
	public partial class B_Order
	{

		#region 订单入库


		#region 新增


		/// <summary>
		/// 新增入库
		/// </summary>
		/// <returns></returns>
		public static M_Order AddStorage(string OrderCode, string InsuredName, string IdentifyNumber, string IdentifyPic, decimal Surcharge, string InsuranceCompany, DateTime StartTime, DateTime EndTime, int applyMonth, string Email, string Tel, int userId, List<ClientRequest_RiskRealatePary> RiskRealateParyList, string Ftrno, string BUSINESS_NO, string BK_SERIAL, string applyPolicyNo, DateTime BuyTime, DateTime PayTime, string PolicyNo, string encryptString, int OrderStep = 0, decimal firstAmount = 0)
		{
			var totalModalPremium = 0M;
			var totalDutyAount = 0M;
			var order = new M_Order();
			var orderPlans = new List<M_OrderPlan>();
			foreach (var item in RiskRealateParyList)
			{
				foreach (var x in item.PlanList)
				{
					var orderPlan = new M_OrderPlan()
					{
						ModalPremium = (decimal)x.ModalPremium,
						ProfessionCode = x.ProfessionCode,
						DutyAount = (decimal)x.dutyAount,
						DutyCode = x.dutyCode,
						PlanCode = x.planCode,
						ProfessionName = item.InsurantList.First().ProfessionName
					};

					orderPlans.Add(orderPlan);
				}

			}
			order = new M_Order
			{
				InsuredName = InsuredName,
				BuyTime = BuyTime,
				CodInd = "Y",
				StartTime = StartTime,
				EndTime = EndTime,
				applyMonth = applyMonth,
				IdentifyNumber = IdentifyNumber,
				IdentifyPic = IdentifyPic,
				OrderCode = OrderCode,
				InsuranceCompany = InsuranceCompany,
				UserId = userId,
				OrderType = "Group",
				Email = Email,
				Tel = Tel,
				OrderStep = OrderStep,
				Surcharge = Surcharge,
				PlanList = orderPlans,
				PayTime = PayTime,
				PolicyNo = PolicyNo,
				FirstModalPremium = firstAmount,
				NowModalPremium = firstAmount,
				encryptString = encryptString,
				ChildList = new List<M_OrderChild>()
			};
			var childOrder = new M_OrderChild()
			{
				OrderCode = OrderCode,
				ChildCode = $"{OrderCode}_0",
				Step = OrderStep,
				Type = 0,
				Ftrno = Ftrno,
				ModalPremium = firstAmount,
				BK_SERIAL = BK_SERIAL,
				BUSINESS_NO = BUSINESS_NO,
				applyPolicyNo = applyPolicyNo,
				CreateTime = BuyTime,
				encryptString = encryptString,
				ValidDate = order.StartTime,
                Surcharge = Surcharge
			};
			foreach (var item in RiskRealateParyList)
			{
				var i = 0;
				foreach (var x in item.InsurantList)
				{
					decimal AcciDutyAount = 0, MedicalDutyAount = 0, AllowanceDutyAount = 0, acciPermium = 0, medicalPremium = 0, allowancePremium = 0;
					if (InsuranceCompany == "CT")
					{
						foreach (var v in item.PlanList)
						{
							if (v.ProfessionCode == x.ProfessionCode && v.planCode == CTGroupInsInfo.PlanCodeList[0])
							{
								AcciDutyAount = (decimal)v.dutyAount;
								acciPermium = (decimal)v.ModalPremium;
							}
							if (v.ProfessionCode == x.ProfessionCode && v.planCode == CTGroupInsInfo.PlanCodeList[1])
							{
								MedicalDutyAount = (decimal)v.dutyAount;
								medicalPremium = (decimal)v.ModalPremium;
							}
							if (v.ProfessionCode == x.ProfessionCode && v.planCode == CTGroupInsInfo.PlanCodeList[2])
							{
								AllowanceDutyAount = (decimal)v.dutyAount;
								allowancePremium = (decimal)v.ModalPremium;
							}
						}

					}
					else if (InsuranceCompany == "PA")
					{
						foreach (var v in item.PlanList)
						{
							if (v.ProfessionCode == x.ProfessionCode && v.planCode == PAGroupInsInfo.PlanCodeList[0])
							{
								AcciDutyAount = (decimal)v.dutyAount;
								acciPermium = (decimal)v.ModalPremium;
							}
							if (v.ProfessionCode == x.ProfessionCode && (v.planCode == PAGroupInsInfo.PlanCodeList[1] || v.planCode == PAGroupInsInfo.PlanCodeList[3]))
							{
								MedicalDutyAount = (decimal)v.dutyAount;
								medicalPremium = (decimal)v.ModalPremium;
							}
							if (v.ProfessionCode == x.ProfessionCode && v.planCode == PAGroupInsInfo.PlanCodeList[2])
							{
								AllowanceDutyAount = (decimal)v.dutyAount;
								allowancePremium = (decimal)v.ModalPremium;
							}
						}

					}
					var person = new M_OrderPersion()
					{
						ProfessionName = x.ProfessionName,
						OrderCode = order.OrderCode,
						Type = 0,
						IdNum = x.IdNum,
						ProfessionCode = x.ProfessionCode,
						RealName = x.Name,
						AcciDutyAount = AcciDutyAount,
						AcciPremium = acciPermium,
						MedicalDutyAount = MedicalDutyAount,
						MedicalPremium = medicalPremium,
						AllowanceDutyAount = AllowanceDutyAount,
						AllowancePremium = allowancePremium
					};
					B_OrderPersion.Add(person);
					var childPerson = new M_ChildPersion()
					{
						ProfessionName = x.ProfessionName,
						ChildCode = childOrder.ChildCode,
						IdNum = x.IdNum,
						ProfessionCode = x.ProfessionCode,
						RealName = x.Name,
						AcciDutyAount = AcciDutyAount,
						AcciPremium = acciPermium,
						MedicalDutyAount = MedicalDutyAount,
						MedicalPremium = medicalPremium,
						AllowanceDutyAount = AllowanceDutyAount,
						AllowancePremium = allowancePremium
					};
					B_ChildPersion.Add(childPerson);
				}

				totalModalPremium += (decimal)(item.InsurantList.Count * item.PlanList.Sum(x => x.ModalPremium));
				totalDutyAount += (decimal)(item.InsurantList.Count * item.PlanList.Sum(x => x.dutyAount));
			}
			childOrder.ModalPremium = totalModalPremium;
			childOrder.dutyAount = totalDutyAount;
			order.FirstModalPremium = totalModalPremium;
			order.NowModalPremium = totalModalPremium;
			order.FirstdutyAount = totalDutyAount;
			order.NowdutyAount = totalDutyAount;
			if (firstAmount > 0)
			{
				order.FirstModalPremium = firstAmount;
				order.NowModalPremium = firstAmount;
				childOrder.ModalPremium = firstAmount;
			}
			order.ChildList.Add(childOrder);
			B_Order.Add(order);
			return order;
		}
		#endregion

		public static string CTUpdateOrder(string OrderCode, string modifyFlag, string FtrNo, string RandomNo, decimal sumPremium, List<InsuredUser> InsuredUserList,double rate)
		{
			string ChildCode = "";
			using (DBContext db = new DBContext())
			{
				try
				{
					DateTime ValidDate = DateTime.Parse(DateTime.Now.AddDays(1).ToString("yyyy-MM-dd"));
					M_Order order = db.OrderList.Find(OrderCode);
					ChildCode = $"{OrderCode}_{order.ChildList.Count}";

					#region 添加子订单的
					//===============添加子订单的===============
					M_OrderChild orderChild = new M_OrderChild()
					{
						Ftrno = FtrNo,
						EndorNo = RandomNo,
						CreateTime = DateTime.Now,
						ValidDate = ValidDate,
						OrderCode = OrderCode,
						Type = 1,
						Step = 0,
						ChildCode = ChildCode,
						ModalPremium = 0,
						dutyAount = sumPremium,
                        Surcharge = 0
					};
					//===============添加子订单的===============
					#endregion


					#region 计算某个职业的旧保费，，新保费
					//===============计算某个职业的旧保费，，新保费===============
					Dictionary<string, PlanUpdateConfig> PlanUpdateConfigDic = new Dictionary<string, PlanUpdateConfig>();
					foreach (var plan in order.PlanList)
					{
						if (PlanUpdateConfigDic.ContainsKey(plan.ProfessionCode) == false)
						{
							PlanUpdateConfig tmp = new PlanUpdateConfig() { ProfessionCode = plan.ProfessionCode };
							PlanUpdateConfigDic.Add(plan.ProfessionCode, tmp);
						}

						if (plan.PlanCode == CTGroupInsInfo.PlanCodeList[0])
						{
							PlanUpdateConfigDic[plan.ProfessionCode].OldAcciPremium = plan.ModalPremium;
							PlanUpdateConfigDic[plan.ProfessionCode].AcciDutyAount = plan.DutyAount;
						}
						if (plan.PlanCode == CTGroupInsInfo.PlanCodeList[1])
						{
							PlanUpdateConfigDic[plan.ProfessionCode].OldMedicalPremium = plan.ModalPremium;
							PlanUpdateConfigDic[plan.ProfessionCode].MedicalDutyAount = plan.DutyAount;
						}
						if (plan.PlanCode == CTGroupInsInfo.PlanCodeList[2])
						{
							PlanUpdateConfigDic[plan.ProfessionCode].OldAllowancePremium = plan.ModalPremium;
							PlanUpdateConfigDic[plan.ProfessionCode].AllowanceDutyAount = plan.DutyAount;
						}
					}

					foreach (var item in PlanUpdateConfigDic.Values)
					{
						item.OldTotalPremium = item.OldAcciPremium + item.OldMedicalPremium + item.OldAllowancePremium;
						item.TotleDutyAount = item.AcciDutyAount + item.MedicalDutyAount + item.AllowanceDutyAount;
						//TotleDutyAount

						var o = InsuredUserList.FirstOrDefault(t => t.insuredBusinessSource == item.ProfessionCode);
						decimal NewTotalPremium = 0;
						if (o != null)
							NewTotalPremium = (decimal)o.changeInsured;

						if (NewTotalPremium < 0)
							NewTotalPremium = -NewTotalPremium;

						item.NewTotalPremium = NewTotalPremium;
						item.系数 = Convert.ToDecimal(rate);
						item.NewAcciPremium = Math.Round(item.系数 * item.OldAcciPremium,2);
						item.NewMedicalPremium = Math.Round(item.系数 * item.OldMedicalPremium,2);
						item.NewAllowancePremium = Math.Round(item.系数 * item.OldAllowancePremium,2);

                        var sb=new StringBuilder();
					    sb.Append($"旧总保费：{item.OldTotalPremium}\r\n");
					    sb.Append($"新总保费：{item.NewTotalPremium}\r\n");
					    sb.Append($"系数：{Convert.ToDecimal(rate)}\r\n");

					    sb.Append($"新保费1：{item.NewAcciPremium}\r\n");
					    sb.Append($"新保费2：{item.NewMedicalPremium}\r\n");
					    sb.Append($"新保费3：{item.NewAllowancePremium}\r\n");
                        Log.Write("系数.log", sb.ToString());
					}
					//===============计算某个职业的旧保费，，新保费===============
					#endregion

					#region 批增
					var 保额变化量 = 0M;
					var 保费变化量 = 0M;
					foreach (var x in InsuredUserList)
					{
						PlanUpdateConfig planUpdateConfig = PlanUpdateConfigDic[x.insuredBusinessSource];

						var person = new M_ChildPersion()
						{
							ProfessionName = x.ProfessionName,
							ChildCode = orderChild.ChildCode,
							IdNum = x.identifyNo,
							ProfessionCode = x.insuredBusinessSource,
							RealName = x.clientCName,
							AcciDutyAount = planUpdateConfig.AcciDutyAount,
							AcciPremium = planUpdateConfig.NewAcciPremium,
							MedicalDutyAount = planUpdateConfig.MedicalDutyAount,
							MedicalPremium = planUpdateConfig.NewMedicalPremium,
							AllowanceDutyAount = planUpdateConfig.AllowanceDutyAount,
							AllowancePremium = planUpdateConfig.NewAllowancePremium
						};

						db.ChildPersionList.Add(person);

						if (modifyFlag == "I")
						{

							保额变化量 = 保额变化量 + planUpdateConfig.TotleDutyAount;
							保费变化量 = 保费变化量 + planUpdateConfig.NewTotalPremium;
						}
						else
						{
							orderChild.Step = 1;
							orderChild.Type = 2;
							M_OrderPersion orderPerson = db.OrderPersionList.Single(t => t.OrderCode == OrderCode && t.IdNum == x.identifyNo);
							db.OrderPersionList.Remove(orderPerson);

							保额变化量 = 保额变化量 - planUpdateConfig.TotleDutyAount;
							保费变化量 = 保费变化量 - planUpdateConfig.NewTotalPremium;

						}
					}
					if (modifyFlag == "B")
					{
						order.NowdutyAount = order.NowdutyAount + 保额变化量;
						order.NowModalPremium = order.NowModalPremium + 保费变化量;
					}
					orderChild.ModalPremium = 保费变化量;
					orderChild.dutyAount = 保额变化量;
					order.ChildList.Add(orderChild);
					#endregion
					DbEntityEntry<M_Order> entry = db.Entry(order);
					entry.State = EntityState.Modified;
					db.SaveChanges();
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
				}
			}
			return ChildCode;


		}
		#endregion







		/// <summary>
		/// 修改批增子订单
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		public static string UpdateOrder(CallBackRequest model)
		{
			//修改子订单
			var childOrder = new M_OrderChild();

			if (model.InsuranceCompany == "CT")
			{
				childOrder = B_OrderChild.Single(x => x.Ftrno == model.OrderCode && x.Step == 0);
			}
			if (childOrder != null)
			{
				//==========修改子订单==========
				childOrder.EndorNo = model.EndorNo;
				childOrder.Step = 1;
				childOrder.BUSINESS_NO = model.BusinessNo;
				childOrder.encryptString = model.EncryptString;
				childOrder.applyPolicyNo = model.PolicyNo;
                childOrder.PayTime=DateTime.Now;

                B_OrderChild.Update(childOrder);
				//==========修改子订单==========
				if (childOrder.Type == 1)
				{
					//==========修改主订单==========
					var order = B_Order.Find(childOrder.OrderCode);
					order.NowModalPremium += childOrder.ModalPremium;
					order.NowdutyAount += childOrder.dutyAount;
					B_Order.Update(order);
					//==========修改主订单==========

					//==========添加订单人员==========
					var childPerson = B_ChildPersion.GetList(x => x.ChildCode == childOrder.ChildCode);
					foreach (var x in childPerson)
					{
						var orderPerson = new M_OrderPersion()
						{
							OrderCode = order.OrderCode,
							Type = 0,
							ProfessionCode = x.ProfessionCode,
							ProfessionName = x.ProfessionName,
							AcciDutyAount = x.AcciDutyAount,
							AcciPremium = x.AcciPremium,
							AllowanceDutyAount = x.AllowanceDutyAount,
							AllowancePremium = x.AllowancePremium,
							IdNum = x.IdNum,
							MedicalDutyAount = x.MedicalDutyAount,
							MedicalPremium = x.MedicalPremium,
							RealName = x.RealName
						};
						B_OrderPersion.Add(orderPerson);
					}
					//==========添加订单人员==========
				}

			}
			return "fail";
		}

		/// <summary>
		///修改新增相关订单
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		public static string UpdateAddOrder(CallBackRequest model)
		{
			using (var sorp = new TransactionScope())
			{
				var order = new M_Order();
				if (model.InsuranceCompany == "CT")
				{
					order = B_Order.Single(x => x.ChildList.FirstOrDefault(o => o.Ftrno == model.OrderCode) != null);
				}
				else if (model.InsuranceCompany == "PA")
				{
					order = B_Order.Single(t => t.ChildList.FirstOrDefault(o => o.BUSINESS_NO == model.BusinessNo) != null);
					//order = B_Order.Find(B_OrderChild.Single(t=>t.BUSINESS_NO== model.BusinessNo).OrderCode);
				}
				if (order == null || order.OrderStep != 0)
				{
					return "fail";
				}
				else
				{
					//修改主订单
					order.OrderStep = 1;
					order.PayTime = DateTime.Now;
					order.PolicyNo = model.PolicyNo;
					order.encryptString = model.EncryptString;
					//修改子订单
					var childOrder = order.ChildList.First();
					childOrder.Step = 1;
					childOrder.BUSINESS_NO = model.BusinessNo;
					childOrder.encryptString = model.EncryptString;
				    childOrder.PayTime = DateTime.Now;

                    B_OrderChild.Update(childOrder);
					B_Order.Update(order);
				}
				sorp.Complete();
			}
			return "fail";
		}

	}

	public class CallBackRequest
	{
		/// <summary>
		/// 订单编号
		/// </summary>
		public string OrderCode { get; set; }
		/// <summary>
		/// 保单号
		/// </summary>
		public string PolicyNo { get; set; }
		/// <summary>
		/// 验证码（下载电子保单用）
		/// </summary>
		public string EncryptString { get; set; }
		/// <summary>
		/// 批增号（诚泰）
		/// </summary>
		public string EndorNo { get; set; }
		public string BusinessNo { get; set; }
		public string InsuranceCompany { get; set; }
	}

	public class CallBackRes
	{
		/// <summary>
		/// 
		/// </summary>
		public string paymentDate { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string dataSource { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string bankOrderNo { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string validateCode { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string remark { get; set; }
		/// <summary>
		/// 交易成功
		/// </summary>
		public string errorMsg { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string resultMsg { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string businessNo { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string signMsg { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string resultCode { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string policyNo { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string paymentState { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string paymentSum { get; set; }
	}



	public class PlanUpdateConfig
	{
		public string ProfessionCode { get; set; }

		public decimal AcciDutyAount { get; set; }
		public decimal OldAcciPremium { get; set; }
		public decimal NewAcciPremium { get; set; }

		public decimal MedicalDutyAount { get; set; }
		public decimal OldMedicalPremium { get; set; }
		public decimal NewMedicalPremium { get; set; }

		public decimal AllowanceDutyAount { get; set; }
		public decimal OldAllowancePremium { get; set; }
		public decimal NewAllowancePremium { get; set; }

		public decimal OldTotalPremium { get; set; }
		public decimal NewTotalPremium { get; set; }
		public decimal TotleDutyAount { get; set; }

		public decimal 系数 { get; set; }
	}
}