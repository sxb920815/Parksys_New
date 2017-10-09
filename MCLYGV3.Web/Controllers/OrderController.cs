using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using MCLYGV3.DB;
using MCLYGV3.Web.ClassLib;
using MCLYGV3.Web.Models;
using MCLYGV3.Web.Work;

namespace MCLYGV3.Web.Controllers
{
    [EnableCors("*", "*", "*")]
    public class OrderController : BaseApiController
    {
        [HttpGet]
        public DataJsonResult MyOrder(int pageNo = 1, int limit = 10, int status = 0, string orderCode = "",string policyNo="",string insuranceCompany="",string insuredName="",DateTime? beginTime=null,DateTime? endTime=null)
        {
            var result = new DataJsonResult();

            if (!IsAuth)
                return base.ErrorResult;

            var checkorderCode = string.IsNullOrWhiteSpace(orderCode);
            var checkpolicyNo = string.IsNullOrWhiteSpace(policyNo);
            var checkinsuranceCompany = string.IsNullOrWhiteSpace(insuranceCompany);
            var checkinsuredName = string.IsNullOrWhiteSpace(insuredName);
            var checkBeginTime = beginTime == null;
            var checkEndTime = endTime == null;

            var checkStatus = status > 0;
            Expression<Func<M_Order, bool>> expression =
                x => x.UserId == AuthorizedUser.ID &&
                     ((checkStatus && x.OrderStep == 2) || (!checkStatus && x.OrderStep != 2)) &&
                     (checkorderCode || x.OrderCode == orderCode) &&
                     (checkpolicyNo || x.PolicyNo == policyNo) &&
                     (checkinsuranceCompany || x.InsuranceCompany == insuranceCompany) &&
                     (checkinsuredName || x.InsuredName.Contains(x.InsuredName)) &&
                     (checkBeginTime || x.BuyTime >= beginTime) &&
                     (checkEndTime || x.BuyTime <= endTime);

            var orders = B_Order.GetListByPage(expression,
                new GridPager() { page = pageNo, rows = limit, order = "desc", sort = "BuyTime" })
                .Select(x => new Order_Res(x)).ToList();
            var totalCount = B_Order.GetCount(expression);
            result.Data = new
            {
                OrderList = orders,
                TotalCount = totalCount
            };
            return result;
        }

        [HttpGet]
        public DataJsonResult Detail(string orderCode)
        {
            var result = new DataJsonResult();
            var x = B_Order.Find(orderCode);
            if (x == null)
            {
                result.ReturnCode = "601";
                result.ErrorMessage = "未找到该订单";
                return result;
            }
            result.Data = new Order_Res(x);
            return result;
        }

        [HttpGet]
        public DataJsonResult OrderPerson(string orderCode)
        {
            var result = new DataJsonResult();
            var childPersonList = new List<ChildPerson>();
            foreach (var x in B_OrderPersion.GetList(x => x.OrderCode == orderCode))
            {
                var childperson = B_ChildPersion.GetList(m => m.IdNum == x.IdNum).OrderBy(m=>m.ID).LastOrDefault();
                childPersonList.Add(new ChildPerson()
                {
                    ProfessionCode = x.ProfessionCode,
                    ProfessionName = x.ProfessionName,
                    RealName = x.RealName,
                    IdNum = x.IdNum,
                    AllowancePremium = x.AllowancePremium,
                    AcciDutyAount = x.AcciDutyAount,
                    AcciPremium = x.AcciPremium,
                    MedicalPremium = x.MedicalPremium,
                    MedicalDutyAount = x.MedicalDutyAount,
                    AllowanceDutyAount = x.AllowanceDutyAount,
                    ValidDate = B_OrderChild.Find(childperson?.ChildCode)?.ValidDate??B_Order.Find(orderCode).ChildList.FirstOrDefault()?.ValidDate
                });
            }
            result.Data = childPersonList;
            return result;
        }

        [HttpGet]
        public DataJsonResult HistoryRecord(string orderCode)
        {
            var result = new DataJsonResult();
            var childOrder = B_Order.Single(x => x.OrderCode == orderCode).ChildList.OrderByDescending(x => x.CreateTime)
                .Select(x => new ChildOrderList
                {
                    ChildCode = x.ChildCode,
                    Step = x.Step,
                    Type = x.Type,
                    DutyAount = x.dutyAount,
                    ModalPremium = x.ModalPremium,
                    CreateTime = x.CreateTime.ToString("yyyy-MM-dd HH:mm:ss"),
                    ChildPerson = B_ChildPersion.GetList(o => o.ChildCode == x.ChildCode)
                    .Select(o => new ChildPerson
                    {
                        IdNum = o.IdNum,
                        ProfessionCode = o.ProfessionCode,
                        ProfessionName = o.ProfessionName,
                        RealName = o.RealName,
                        AcciPremium = o.AcciPremium,
                        AcciDutyAount = o.AcciDutyAount,
                        MedicalPremium = o.MedicalPremium,
                        MedicalDutyAount = o.MedicalDutyAount,
                        AllowancePremium = o.AllowancePremium,
                        AllowanceDutyAount = o.AllowanceDutyAount
                    }).ToList(),
                    StartTime = x.ValidDate.ToString("yyyy-MM-dd HH:mm:ss")

                });
            result.Data = childOrder;
            return result;
        }

        [HttpGet]
        public DataJsonResult CancleOrder(string orderCode)
        {
            var result = new DataJsonResult();
            if (!IsAuth)
                return base.ErrorResult;
            var order = B_Order.Single(x => x.OrderCode == orderCode);
            if (order.OrderStep == 1)
            {
                result.ReturnCode = "502";
                result.ErrorMessage = "不能取消已支付的订单";
                return result;
            }
            if (order.UserId != AuthorizedUser.ID)
            {
                result.ReturnCode = "503";
                result.ErrorMessage = "只能取消自己的订单";
                return result;
            }
            order.OrderStep = 2;
            B_Order.Update(order);
            return result;
        }



        [HttpGet]
        public DataJsonResult CancleChildOrder(string childCode)
        {
            var result = new DataJsonResult();
            if (!IsAuth)
                return base.ErrorResult;
            var order = B_OrderChild.Single(x => x.ChildCode == childCode);
            var parentOrder = B_Order.Single(x => x.OrderCode == order.OrderCode);
            if (parentOrder.UserId != AuthorizedUser.ID)
            {
                result.ReturnCode = "502";
                result.ErrorMessage = "只能取消自己的订单";
                return result;
            }
            order.Step = 2;
            if (B_OrderChild.GetCount(x => x.OrderCode == order.OrderCode) == 1)
            {
                parentOrder.OrderStep = 2;
                B_Order.Update(parentOrder);
            }
            B_OrderChild.Update(order);
            return result;
        }


        [HttpGet]
        public DataJsonResult PayInfo(string childCode)
        {
            if (!IsAuth)
                return base.ErrorResult;
            return PayInfoOperation.GetPayInfo(childCode, AuthorizedUser);
        }


    }

    public class ChildOrderList
    {
        public string ChildCode { get; set; }

        public int Step { get; set; }

        public int Type { get; set; }
        public decimal DutyAount { get; set; }
        public decimal ModalPremium { get; set; }

        public string CreateTime { get; set; }
        public string StartTime { get; set; }

        public List<ChildPerson> ChildPerson { get; set; }

    }

    public class ChildPerson
    {
        public string ProfessionCode { get; set; }

        public string ProfessionName { get; set; }
        public string RealName { get; set; }
        public string IdNum { get; set; }
        public decimal AcciPremium { get; set; }
        public decimal AcciDutyAount { get; set; }
        public decimal MedicalPremium { get; set; }
        public decimal AllowancePremium { get; set; }
        public decimal MedicalDutyAount { get; set; }
        public decimal AllowanceDutyAount { get; set; }
        public DateTime? ValidDate { get; set; }

    }

    public class Order_Res
    {
        public Order_Res(M_Order x)
        {
            OrderCode = x.OrderCode;
            ChildList = x.ChildList;
            InsuredName = x.InsuredName;
            encryptString = x.encryptString;
            OrderStep = x.OrderStep;
            PolicyNo = x.PolicyNo;
            Email = x.Email;
            PlanList = x.PlanList;
            applyMonth = x.applyMonth;
            InsuranceCompany = x.InsuranceCompany;
            PayTime = x.PayTime?.ToString("yyyy-MM-dd HH:mm:ss");
            IdentifyNumber = x.IdentifyNumber;
            UserId = x.UserId;
            NowModalPremium = x.NowModalPremium;
            FirstModalPremium = x.FirstModalPremium;
            Surcharge = x.Surcharge;
            EndTime = x.EndTime.ToString("yyyy-MM-dd HH:mm:ss");
            IdentifyPic = x.IdentifyPic;
            StartTime = x.StartTime.ToString("yyyy-MM-dd HH:mm:ss");
            BuyTime = x.BuyTime.ToString("yyyy-MM-dd HH:mm:ss");
            OrderType = x.OrderType;
            CodInd = x.CodInd;
            Tel = x.Tel;
            FirstdutyAount = x.FirstdutyAount;
            NowdutyAount = x.NowdutyAount;
            ChildOrderStep = x.ChildList.Any(m => m.Step == 0) ? 0 : 1;
        }
        /// <summary>
        /// 订单编号
        /// </summary>
        public string OrderCode { get; set; }
        /// <summary>
        /// 用户ID
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// 订单步骤。0未支付1已支付2已过期
        /// </summary>
        public int OrderStep { get; set; }

        public int ChildOrderStep { get; set; }
        /// <summary>
        /// 投保人公司名
        /// </summary>
        public string InsuredName { get; set; }
        /// <summary>
        /// 投保人公司编号
        /// </summary>
        public string IdentifyNumber { get; set; }
        /// <summary>
        /// 证件图片ID
        /// </summary>
        public string IdentifyPic { get; set; }
        /// <summary>
        /// 附加费
        /// </summary>
        public decimal Surcharge { get; set; }
        /// <summary>
        /// 初次总保费
        /// </summary>
        public decimal FirstModalPremium { get; set; }
        /// <summary>
        /// 初次总保额
        /// </summary>
        public decimal FirstdutyAount { get; set; }
        /// <summary>
        /// 当前总保费
        /// </summary>
        public decimal NowdutyAount { get; set; }
        /// <summary>
        /// 当前总保额
        /// </summary>
        public decimal NowModalPremium { get; set; }
        /// <summary>
        /// 保险公司。CT或者PA
        /// </summary>
        public string InsuranceCompany { get; set; }
        /// <summary>
        /// 见费出单。Y或者N
        /// </summary>
        public string CodInd { get; set; }
        /// <summary>
        /// 保单类别。Group
        /// </summary>
        public string OrderType { get; set; }
        /// <summary>
        /// 保险生效时间
        /// </summary>
        public string StartTime { get; set; }
        /// <summary>
        /// 保险失效时间
        /// </summary>
        public string EndTime { get; set; }
        /// <summary>
        /// 保险提交时间
        /// </summary>
        public string BuyTime { get; set; }
        /// <summary>
        /// 付款返回时间
        /// </summary>
        public string PayTime { get; set; }
        /// <summary>
        /// 投保月份
        /// </summary>
        public int applyMonth { get; set; }
        /// <summary>
        /// 保单号
        /// </summary>
        public string PolicyNo { get; set; }
        /// <summary>
        /// 投保计划
        /// </summary>
        public virtual ICollection<M_OrderPlan> PlanList { get; set; }
        /// <summary>
        /// 子订单列表
        /// </summary>
        public virtual ICollection<M_OrderChild> ChildList { get; set; }
        /// <summary>
        /// 电子信箱
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// 电话号码
        /// </summary>
        public string Tel { get; set; }
        /// <summary>
        /// 电子保单密文
        /// </summary>
        public string encryptString { get; set; }
        public Order_Res()
        {
            PlanList = new HashSet<M_OrderPlan>();
            ChildList = new HashSet<M_OrderChild>();
        }
    }

}
