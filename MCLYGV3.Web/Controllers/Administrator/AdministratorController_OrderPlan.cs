using MCLYGV3.DB;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections.Generic;
using System.Text;
using System.Web.Mvc;

namespace MCLYGV3.Web.Controllers
{
	public partial class AdministratorController : AdministratorControll
	{
		
		#region 订单

		public ActionResult OrderPlan_List(string OrderCode)
		{
            ViewBag.OrderCode = OrderCode;
            return View(MyUser);
		}
		public ActionResult OrderPlan_Add()
		{
			return View();
		}
		public ActionResult OrderPlan_Detail(int ID)
		{
			M_OrderPlan obj = B_OrderPlan.Find(ID);
			return View(obj);
		}
		public ActionResult OrderPlan_Edit(int ID)
		{
			M_OrderPlan obj = B_OrderPlan.Find(ID);
			return View(obj);
		}

		[HttpPost]
		public string DelOrderPlan(int ID)
		{
			JsonMessage result;

			bool bol = B_OrderPlan.Del(ID);

			if (bol)
				result = new JsonMessage() { type = 0, message = "成功", value = "" };
			else
				result = new JsonMessage() { type = -1, message = "失败", value = "" };

			Response.ContentType = "application/json";
			Response.Charset = "UTF-8";
			return JsonConvert.SerializeObject(result);
		}

		[HttpPost]
		public string EditOrderPlan()
		{
			JsonMessage result;
			byte[] byts = new byte[Request.InputStream.Length];
			Request.InputStream.Read(byts, 0, byts.Length);
			string req = Encoding.UTF8.GetString(byts);

			M_OrderPlan obj = JsonConvert.DeserializeObject<M_OrderPlan>(req);
			bool bol = B_OrderPlan.Update(obj);
			if (bol)
				result = new JsonMessage() { type = 0, message = "成功", value = req };
			else
				result = new JsonMessage() { type = -1, message = "失败", value = req };

			Response.ContentType = "application/json";
			Response.Charset = "UTF-8";
			return JsonConvert.SerializeObject(result);
		}

		[HttpPost]
		public string AddOrderPlan()
		{
			JsonMessage result;
			byte[] byts = new byte[Request.InputStream.Length];
			Request.InputStream.Read(byts, 0, byts.Length);
			string req = Encoding.UTF8.GetString(byts);

			M_OrderPlan obj = JsonConvert.DeserializeObject<M_OrderPlan>(req);
			obj = B_OrderPlan.Add(obj);
			result = new JsonMessage() { type = 0, message = "成功", value = req };
			Response.ContentType = "application/json";
			Response.Charset = "UTF-8";
			return JsonConvert.SerializeObject(result);
		}

		public string GetOrderPlanList(GridPager pager, string OrderCode)
		{
			List<M_OrderPlan> list=new List<M_OrderPlan>();
			int count;
            //if (string.IsNullOrEmpty(queryStr))
            //{
            //	list = B_OrderPlan.GetListByPage(t => 1 == 1, pager);
            //	count = B_OrderPlan.GetCount(t => 1 == 1);
            //}
            //else
            //{
             foreach( var p in B_Order.Find(OrderCode).PlanList)
            {
                list.Add(new M_OrderPlan() {
                    ID=p.ID,
                   PlanCode= p.PlanCode,
                   DutyAount=p.DutyAount,
                   DutyCode=p.DutyCode,
                   ProfessionCode=p.ProfessionCode,
                   ProfessionName=p.ProfessionName,
                   ModalPremium=p.ModalPremium
                });
                    
            }
            //list = B_OrderPlan.GetListByPage(t => t.OrderID.Contains(queryStr), pager);
            count = B_Order.Find(OrderCode).PlanList.Count;
            //}
            GridRows<M_OrderPlan> grs = new GridRows<M_OrderPlan>();
			grs.rows = list;
			grs.total = count;
			Response.ContentType = "application/json";
			Response.Charset = "UTF-8";
			IsoDateTimeConverter timeFormat = new IsoDateTimeConverter();
			timeFormat.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
			return JsonConvert.SerializeObject(grs, Formatting.Indented, timeFormat);
		}

		#endregion
	
	}
}
