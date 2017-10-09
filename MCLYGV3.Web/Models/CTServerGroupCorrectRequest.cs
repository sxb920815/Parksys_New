using System.Collections.Generic;
// ReSharper disable InconsistentNaming

namespace MCLYGV3.Web.Models
{
    public class CtServerGroupCorrectRequest
    {
        public CtServerGroupCorrectRequestHead Head { get; set; }
        public CtServerGroupCorrectRequestMain Main { get; set; }
        public List<corrPlan> CorrPlanList { get; set; }

    }

    public class CtServerGroupCorrectRequestHead
    {
        public string partnerCode { get; set; }
        public string passWord { get; set; }
        public string queryId { get; set; }
        public string requestType { get; set; }
        public string userName { get; set; }

    }
    public class CtServerGroupCorrectRequestMain
    {
        public string policyNo { get; set; }
        public string sumPremium { get; set; }
        public string validDate { get; set; }

    }

    public class corrPlan
    {
        public List<RiskRealatePary> RiskRealateParyList { get; set; }

    }
    public class RiskRealatePary
    {
        public string appliPhone { get; set; }

        public string birthday { get; set; }
        public string changeInsured { get; set; }
        public string clientCName { get; set; }
        public string clientType { get; set; }
        public string identifyNo { get; set; }
        public string identifyType { get; set; }
        public string insuredBusinessSource { get; set; }
        public string modifyFlag { get; set; }
        public string sex { get; set; }
    }
}
