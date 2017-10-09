using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace MCLYGV3.Web.Models
{
    public class CTServerRequest
    {
        public Head Head { get; set; }
        public Main Main { get; set; }
        public RelatedParty RelatedParty { get; set; }

        public List<Plan> PlanList { get; set; }
    }

    public class Head
    {
        /// <summary>
        /// 合作伙伴编码
        /// </summary>
        public string partnerCode { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string passWord { get; set; }
        /// <summary>
        /// 外网提供的唯一标志
        /// </summary>
        public string queryId { get; set; }
        /// <summary>
        /// 请求类型
        /// </summary>
        public string requestType { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string userName { get; set; }
    }

    public class Main
    {
        /// <summary>
        /// 争议解决方式。1:诉讼 2：仲裁
        /// </summary>
        public string argueSolution { get; set; }
        /// <summary>
        /// 见费出单标志Y:是 N：否
        /// </summary>
        public string codInd { get; set; }
        /// <summary>
        /// 佣金比率
        /// </summary>
        public string commissionRatio { get; set; }
        /// <summary>
        /// 币种
        /// </summary>
        public string currency { get; set; }
        /// <summary>
        /// 截止日期
        /// </summary>
        public string endDate { get; set; }
        /// <summary>
        /// 投保日期
        /// </summary>
        public string operateDate { get; set; }
        /// <summary>
        /// 产品代码（套餐号）MCLY1108A-01
        /// </summary>
        public string planCode { get; set; }
        /// <summary>
        /// 短期费率
        /// </summary>
        public string shortRate { get; set; }
        /// <summary>
        /// 起始日期
        /// </summary>
        public string startDate { get; set; }
        /// <summary>
        /// 总保费
        /// </summary>
        public string sumGrossPremium { get; set; }
        /// <summary>
        /// 总保额
        /// </summary>
        public string sumInsured { get; set; }
    }

    public class RelatedParty
    {
        /// <summary>
        /// 生日
        /// </summary>
        public string birthDate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string businessSource { get; set; }
        /// <summary>
        /// 联系人
        /// </summary>
        public string contactName { get; set; }
        /// <summary>
        /// 联系人电话
        /// </summary>
        public string contactPhone { get; set; }
        /// <summary>
        /// 联系人电话
        /// </summary>
        public string officePhone { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string educationBackground { get; set; }
        /// <summary>
        /// email
        /// </summary>
        public string email { get; set; }
        /// <summary>
        /// 身份证号
        /// </summary>
        public string identifyNumber { get; set; }
        /// <summary>
        /// 证件类别
        /// </summary>
        public string identifyType { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        public string insuredAddress { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string insuredName { get; set; }
        /// <summary>
        /// 投保人类型
        /// </summary>
        public string insuredType { get; set; }
        /// <summary>
        /// 投保份数
        /// </summary>
        public string uWCount { get; set; }
    }

    public class Plan
    {
        public  ItemAcci ItemAcci { get; set; }
        public  List<ItemKind> ItemKindList { get; set; }
        public  List<riskRealatePary> riskRealateParyList { get; set; }
    }

    public class ItemAcci
    {
        /// <summary>
        /// 职业编码
        /// </summary>
        public string insuredBusinessSource { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public string quantity { get; set; }
        /// <summary>
        /// 定额类型N非定额 Y定额
        /// </summary>
        public string rationType { get; set; }
    }
    public class ItemKind
    {
        /// <summary>
        /// 保费
        /// </summary>
        public double grossPremium { get; set; }
        /// <summary>
        /// 险别代码
        /// </summary>
        public string kindCode { get; set; }
        /// <summary>
        /// 主险/附加险标志1	主险2	附加险
        /// </summary>
        public string kindInd { get; set; }
        /// <summary>
        /// 费率
        /// </summary>
        public double rate { get; set; }
        /// <summary>
        /// 保险金额
        /// </summary>
        public double sumInsured { get; set; }
    }

    public class riskRealatePary
    {
        /// <summary>
        /// 电话
        /// </summary>
        public string appliPhone { get; set; }
        /// <summary>
        /// 生日
        /// </summary>
        public string birthday { get; set; }
        /// <summary>
        /// 投保人
        /// </summary>
        public string clientCName { get; set; }
        /// <summary>
        /// 投保类型
        /// </summary>
        public string clientType { get; set; }
        /// <summary>
        /// 证件号码
        /// </summary>
        public string identifyNo { get; set; }
        /// <summary>
        /// 证件类型
        /// </summary>
        public string identifyType { get; set; }
        /// <summary>
        /// 职业编码
        /// </summary>
        public string insuredBusinessSource { get; set; }
        /// <summary>
        /// 邮编
        /// </summary>
        public string postCode { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public string sex { get; set; }
        /// <summary>
        /// 邮箱
        /// </summary>
        public string email { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        public string homeAddress { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string clientCNameBenefit { get; set; }
    }



   

}