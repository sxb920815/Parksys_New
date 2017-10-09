using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using MCLYGV3.DB;
using MCLYGV3.Web.Models;

namespace MCLYGV3.Web.Work
{
    public class StorageOperation
    {
        #region 平安
        /// <summary>
        /// 平安新增入库
        /// </summary>
        /// <returns></returns>
        public static M_Order PAAddStorage(ClientRequest RequestInfo, int userId, string businessNo = "", string applyPolicyNo = "", string BK_SERIAL = "")
        {
            var order = new M_Order();
            using (var socp = new TransactionScope())
            {
                var totalModalPremium = 0M;
                var totalDutyAount = 0M;
                #region 入库
                order = new M_Order
                {
                    InsuredName = RequestInfo.RelatedParty.InsuredName,
                    BuyTime = DateTime.Now,
                    CodInd = RequestInfo.Body.CodInd,
                    StartTime = RequestInfo.Body.StartDate,
                    EndTime = RequestInfo.Body.StartDate.AddMonths(RequestInfo.Body.applyMonth).AddSeconds(-1),
                    applyMonth = RequestInfo.Body.applyMonth,
                    IdentifyNumber = RequestInfo.RelatedParty.IdentifyNumber,
                    IdentifyPic = RequestInfo.RelatedParty.IdentifyPic,
                    OrderCode = RequestInfo.Body.OrderCode,
                    InsuranceCompany = RequestInfo.Body.InsuranceCompany,
                    UserId = userId,
                    OrderType = "Group",
                    Email = RequestInfo.RelatedParty.Email,
                    Tel = RequestInfo.RelatedParty.Tel,
                    OrderStep = 0,
                    PlanList = new List<M_OrderPlan>(),
                    ChildList = new List<M_OrderChild>()
                };
                var childOrder = new M_OrderChild()
                {
                    OrderCode = RequestInfo.Body.OrderCode,
                    ChildCode = $"{RequestInfo.Body.OrderCode}_0",
                    Step = 0,
                    Type = 0,
                    BUSINESS_NO = businessNo,
                    applyPolicyNo = applyPolicyNo,
                    BK_SERIAL = BK_SERIAL,
                };
                foreach (var item in RequestInfo.RiskRealateParyList)
                {
                    var i = 0;
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
                        order.PlanList.Add(orderPlan);
                    }
                    foreach (var x in item.InsurantList)
                    {
                        decimal AcciDutyAount = 0,AcciPremium=0, MedicalDutyAount = 0,MedicalPremium=0, AllowanceDutyAount = 0,AllowancePremium=0;
                        foreach (var v in item.PlanList)
                        {

                            if (v.ProfessionCode == x.ProfessionCode && v.planCode == PAGroupInsInfo.PlanCodeList[0])
                            {
                                AcciDutyAount = (decimal)v.dutyAount;
                                AcciPremium = (decimal)v.ModalPremium;
                            }
                            if (v.ProfessionCode == x.ProfessionCode && (v.planCode == PAGroupInsInfo.PlanCodeList[1] || v.planCode == PAGroupInsInfo.PlanCodeList[3]))
                            {
                                MedicalDutyAount = (decimal)v.dutyAount;
                                MedicalPremium = (decimal)v.ModalPremium;
                            }
                            if (v.ProfessionCode == x.ProfessionCode && v.planCode == PAGroupInsInfo.PlanCodeList[2])
                            {
                                AllowanceDutyAount = (decimal)v.dutyAount;
                                AllowancePremium = (decimal)v.ModalPremium;
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
                            AcciPremium = AcciPremium,
                            MedicalDutyAount = MedicalDutyAount,
                            MedicalPremium = MedicalPremium,
                            AllowanceDutyAount = AllowanceDutyAount,
                            AllowancePremium = AllowancePremium
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
                            AcciPremium = AcciPremium,
                            MedicalDutyAount = MedicalDutyAount,
                            MedicalPremium = MedicalPremium,
                            AllowanceDutyAount = AllowanceDutyAount,
                            AllowancePremium = AllowancePremium
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
                order.ChildList.Add(childOrder);
                B_Order.Add(order);
                #endregion

                socp.Complete();
            }
            return order;
        }

        /// <summary>
        /// 平安批增入库
        /// </summary>
        /// <returns></returns>
        public bool PABatchAddStorage()
        {
            return true;

        }

        /// <summary>
        /// 平安批减入库
        /// </summary>
        /// <returns></returns>
        public bool PABatchReductionStorage()
        {
            return true;

        }
        #endregion
        /// <summary>
        /// 判断是否有未支付的子订单
        /// </summary>
        /// <param name="policyNo"></param>
        /// <returns></returns>
        public static bool HasNoPayChildOrder(string policyNo)
        {
            var order = B_Order.Single(x => x.PolicyNo == policyNo);
            return order.ChildList.Any(x => x.Step == 0);
        }

    }
}