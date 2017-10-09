using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MCLYGV3.Web.Models;

namespace MCLYGV3.DB
{
    public partial class B_Order
    {
        public static string OldUpdateOrder(string PolicyNo, string modifyFlag, string FtrNo, string RandomNo, decimal sumPremium, List<InsuredUser> InsuredUserList,DateTime createTime,string encryptString,DateTime ValidDate)
        {
            string ChildCode = "";
            using (DBContext db = new DBContext())
            {
                try
                {
                    M_Order order = db.OrderList.FirstOrDefault(x=>x.PolicyNo== PolicyNo);
                    if (order==null)
                    {
                        return "";
                    }
                    ChildCode = $"{order.OrderCode}_{order.ChildList.Count}";
                    #region 添加子订单的
                    //===============添加子订单的===============
                    M_OrderChild orderChild = new M_OrderChild()
                    {
                        Ftrno = FtrNo,
                        EndorNo = RandomNo,
                        CreateTime = createTime,
                        ValidDate = ValidDate,
                        OrderCode = order.OrderCode,
                        Type = 1,
                        Step = 1,
                        ChildCode = ChildCode,
                        ModalPremium = 0,
                        dutyAount = sumPremium,
                        encryptString = encryptString,
                    };
                    if (modifyFlag == "B")
                        orderChild.Type = 2;
                    ChildCode = orderChild.ChildCode;
                    //===============添加子订单的===============
                    #endregion

                    #region 批增
                    var 保额变化量 = 0M;
                    var 保费变化量 = 0M;
                    foreach (var x in InsuredUserList)
                    {
                        
                        var orderPerson =
                            db.OrderPersionList.First(m => m.OrderCode == order.OrderCode && m.ProfessionCode == x.insuredBusinessSource);
                        var 系数 = (orderPerson.AllowancePremium + orderPerson.MedicalPremium + orderPerson.AcciPremium)/
                                 (decimal)x.changeInsured;
                        var person = new M_ChildPersion()
                        {
                            ProfessionName = x.ProfessionName,
                            ChildCode = orderChild.ChildCode,
                            IdNum = x.identifyNo,
                            ProfessionCode = x.insuredBusinessSource,
                            RealName = x.clientCName,
                            AcciDutyAount = orderPerson.AcciDutyAount/ 系数,
                            AcciPremium = orderPerson.AcciPremium/ 系数,
                            AllowancePremium = orderPerson.AllowancePremium/ 系数,
                            AllowanceDutyAount = orderPerson.AllowanceDutyAount/ 系数,
                            MedicalDutyAount = orderPerson.MedicalDutyAount/ 系数,
                            MedicalPremium = orderPerson.MedicalPremium/ 系数
                        };
                        db.ChildPersionList.Add(person);
                        保额变化量 = 保额变化量 + (decimal)x.changeInsured;
                        保费变化量 = 保费变化量 + person.AllowancePremium+person.MedicalPremium+person.AcciPremium;
                    }
                    order.NowdutyAount = order.NowdutyAount + 保额变化量;
                    order.NowModalPremium = order.NowModalPremium + 保费变化量;
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


        public static M_Order OldDataAddStorage(string OrderCode, string InsuredName, string IdentifyNumber, string IdentifyPic, decimal Surcharge, string InsuranceCompany, DateTime StartTime, DateTime EndTime, int applyMonth, string Email, string Tel, int userId, List<ClientRequest_RiskRealatePary> RiskRealateParyList, string Ftrno, string BUSINESS_NO, string BK_SERIAL, string applyPolicyNo, DateTime BuyTime, DateTime PayTime, string PolicyNo, string encryptString, int OrderStep = 0, decimal firstAmount = 0)
        {
            if (OrderCode== "CT20170623113108986")
            {
                var aaa = "";
            }
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
                        ModalPremium = Convert.ToDecimal(x.ModalPremium),
                        ProfessionCode = x.ProfessionCode,
                        DutyAount = Convert.ToDecimal(x.dutyAount),
                        DutyCode = x.dutyCode,
                        PlanCode = x.planCode,
                        ProfessionName = item.InsurantList.FirstOrDefault()?.ProfessionName
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
                BK_SERIAL = BK_SERIAL,
                BUSINESS_NO = BUSINESS_NO,
                applyPolicyNo = applyPolicyNo,
                CreateTime = DateTime.Now,
                encryptString = encryptString,
                ValidDate = order.StartTime,
                Surcharge = 0
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
                                AcciDutyAount = Convert.ToDecimal(v.dutyAount);
                                acciPermium = Convert.ToDecimal(v.ModalPremium);
                            }
                            if (v.ProfessionCode == x.ProfessionCode && v.planCode == CTGroupInsInfo.PlanCodeList[1])
                            {
                                MedicalDutyAount = Convert.ToDecimal(v.dutyAount);
                                medicalPremium = Convert.ToDecimal(v.ModalPremium);
                            }
                            if (v.ProfessionCode == x.ProfessionCode && v.planCode == CTGroupInsInfo.PlanCodeList[2])
                            {
                                AllowanceDutyAount = Convert.ToDecimal(v.dutyAount);
                                allowancePremium = Convert.ToDecimal(v.ModalPremium);
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
                            if (v.ProfessionCode == x.ProfessionCode && v.planCode == PAGroupInsInfo.PlanCodeList[1])
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
                        MedicalPremium= medicalPremium,
                        AllowanceDutyAount = AllowanceDutyAount,
                        AllowancePremium=allowancePremium
                    };
                    B_OrderPersion.Add(person);
                }

                totalModalPremium += Convert.ToDecimal((item.InsurantList.Count * item.PlanList.Sum(x => x.ModalPremium)));
                totalDutyAount += Convert.ToDecimal((item.InsurantList.Count * item.PlanList.Sum(x => x.dutyAount)));
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
    }
}
