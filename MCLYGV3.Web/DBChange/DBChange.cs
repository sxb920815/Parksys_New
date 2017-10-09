using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using MCLYGV3.DB;
using MCLYGV3.Web.Models;
using Newtonsoft.Json;

namespace MCLYGV3.Web
{
    public class DBChange
    {
        public static void addUser()
        {
            string sqlUser = "select * from dt_users";
            DataTable dtUser = DbHelperSQL.ExecuteDataSet(sqlUser).Tables[0];
            for (int i = 0; i < dtUser.Rows.Count; i++)
            {
                var name = dtUser.Rows[i][2].ToString();
                if (B_UserInfo.GetCount(x => x.UserName == name) == 0)
                {
                    var user = new M_UserInfo()
                    {
                        ID = Convert.ToInt32(dtUser.Rows[i][2]),
                        UserName = name,
                        InCompany = B_Company.Find(1),
                        PassWord = Common.Md5Password("123456"),
                        RealName = name
                    };
                    B_UserInfo.Add(user);
                }
            }
        }

        public static int AddOrder()
        {
            string sql = "SELECT* FROM dt_orders where status=3";
            DataTable dt = DbHelperSQL.ExecuteDataSet(sql).Tables[0];
            var i = 0;
            foreach (DataRow dr in dt.Rows)
            {
                var id = dr["order_no"].ToString();
                if (B_Order.Find(id) != null)
                {
                    continue;
                }
                var userId = Convert.ToInt32(dr["user_id"]);
                var addTime = Convert.ToDateTime(dr["add_time"]);
                var firstAmount = Convert.ToInt32(dr["order_amount"]);
                var payTime = Convert.ToDateTime(dr["complete_time"]);
                var tel = dr["telphone"].ToString();
                var email = dr["email"].ToString();
                var extend_json = dr["extend_json"].ToString();
                extend_json = extend_json.Replace("\"NaN\"", "0");
                var orders = JsonConvert.DeserializeObject<OrderExtendjson>(extend_json);
                var pic = orders.tbrimg;
                if (string.IsNullOrWhiteSpace(pic))
                {
                    pic = "123";
                }
                var startTime = Convert.ToDateTime(orders.startDate);
                var endTime = Convert.ToDateTime(orders.endDate);
                var applyMonth = (endTime.AddSeconds(1).Year - startTime.Year) * 12 + (endTime.AddSeconds(1).Month - startTime.Month);
                var RiskRealateParyList = new List<ClientRequest_RiskRealatePary>();
                var tmpPlanList = orders.listInfoFirst.GroupBy(t => t.zylb);
                string[] planCodeInfo, dutyCodeInfo;
                if (id.Substring(0, 2) == "CT")
                {
                    planCodeInfo = new string[3] { "1144001", "1144002", "1144003" };
                    dutyCodeInfo = new string[3] { "1144001", "1144002", "1144003" };
                }
                else
                {
                    planCodeInfo = new string[3] { "Y502", "J513", "J511" };
                    dutyCodeInfo = new string[3] { "YA01", "JA17", "JD06" };
                }
                foreach (var item in tmpPlanList.Select(x => x.First()))
                {

                    var RiskRealatePary = new ClientRequest_RiskRealatePary
                    {
                        InsurantList = new List<ClientRequest_RiskRealatePary_InsurantInfo>(),
                        PlanList = new List<ClientRequest_RiskRealatePary_PlanInfo>()
                    };
                    ClientRequest_RiskRealatePary_PlanInfo plan;
                    plan = new ClientRequest_RiskRealatePary_PlanInfo()
                    {
                        ProfessionCode = item.zylb,
                        planCode = planCodeInfo[0],
                        dutyCode = dutyCodeInfo[0],
                        dutyAount = (double)item.acciBe,
                        ModalPremium = (double)item.acciBf
                    };
                    RiskRealatePary.PlanList.Add(plan);
                    if (item.medicalBe > 0)
                    {
                        plan = new ClientRequest_RiskRealatePary_PlanInfo()
                        {
                            ProfessionCode = item.zylb,
                            planCode = planCodeInfo[1],
                            dutyCode = dutyCodeInfo[1],
                            dutyAount = (double)item.medicalBe,
                            ModalPremium = (double)item.medicalBf

                        };
                        RiskRealatePary.PlanList.Add(plan);

                    }
                    if (item.allowanceBe > 0)
                    {
                        plan = new ClientRequest_RiskRealatePary_PlanInfo()
                        {
                            ProfessionCode = item.zylb,
                            planCode = planCodeInfo[2],
                            dutyCode = dutyCodeInfo[2],
                            dutyAount = (double)item.allowanceBe,
                            ModalPremium = (double)item.allowanceBf

                        };
                        RiskRealatePary.PlanList.Add(plan);

                    }

                    foreach (var x in orders.listInfo.Where(x => x.zylb == item.zylb))
                    {
                        if (string.IsNullOrWhiteSpace(x.zymc))
                        {
                            x.zymc = "123";
                        }
                        var InsurantInfo = new ClientRequest_RiskRealatePary_InsurantInfo()
                        {
                            ProfessionCode = x.zylb,
                            ProfessionName = x.zymc,
                            IdNum = x.idno,
                            Name = x.name
                        };
                        RiskRealatePary.InsurantList.Add(InsurantInfo);
                    }

                    RiskRealateParyList.Add(RiskRealatePary);
                }

                //foreach (var item in tmpPlanList.Select(x=>x.First()))
                //{

                //}
                B_Order.AddStorage(id, orders.tbr, orders.tbrIdno, pic, 0, id.Substring(0, 2), startTime, endTime, applyMonth, email, tel, userId, RiskRealateParyList, "", "", "", "", addTime, payTime, orders.policyNo, orders.encryptString, 1, firstAmount);
                i++;
            }
            return i;
        }
    }

    public class OrderExtendjson
    {
        public string tbr { get; set; }
        public string tbrIdno { get; set; }
        public string tbrimg { get; set; }
        public string startDate { get; set; }
        public string endDate { get; set; }
        public string tbdh { get; set; }
        public string partnerCod { get; set; }
        public string policyNo { get; set; }
        public string encryptString { get; set; }


        public List<OrderExtendjsonList> listInfo { get; set; }
        public List<OrderExtendjsonList> listInfoFirst { get; set; }


    }
    public class OrderExtendjsonList
    {
        public int id { get; set; }
        public string name { get; set; }
        public string idno { get; set; }
        /// <summary>
        /// 职业类别
        /// </summary>
        public string zylb { get; set; }
        /// <summary>
        /// 职业名称
        /// </summary>
        public string zymc { get; set; }
        /// <summary>
        /// 投保有效时间
        /// </summary>
        public int btime { get; set; }
        /// <summary>
        /// 主线保额
        /// </summary>
        public decimal acciBe { get; set; }
        /// <summary>
        ///费率
        /// </summary>
        public decimal acciRoat { get; set; }
        /// <summary>
        /// 主线保费
        /// </summary>
        public decimal acciBf { get; set; }
        /// <summary>
        /// 附加险保额
        /// </summary>
        public decimal medicalBe { get; set; }
        public decimal medicalRoat { get; set; }
        /// <summary>
        /// 附加险保费
        /// </summary>
        public decimal medicalBf { get; set; }
        /// <summary>
        /// 住院津贴保额
        /// </summary>
        public decimal allowanceBe { get; set; }
        public decimal allowanceRoat { get; set; }
        /// <summary>
        /// 住院津贴保费
        /// </summary>
        public decimal allowanceBf { get; set; }
        /// <summary>
        /// 总保费
        /// </summary>
        public decimal totalBf { get; set; }
        /// <summary>
        /// 总保额
        /// </summary>
        public decimal totalBe { get; set; }

        public decimal totalJf { get; set; }
    }


    public class plan
    {
        public decimal acciBe { get; set; }
        public decimal medicalBe { get; set; }
        public decimal allowanceBe { get; set; }
    }
}