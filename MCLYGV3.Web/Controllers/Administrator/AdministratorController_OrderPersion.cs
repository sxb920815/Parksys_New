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

		public ActionResult OrderPersion_List(string OrderCode)
		{
            ViewBag.OrderCode = OrderCode;
			return View(MyUser);
		}
		public ActionResult OrderPersion_Add()
		{
			return View();
		}
		public ActionResult OrderPersion_Detail(int ID)
		{
			M_OrderPersion obj = B_OrderPersion.Find(ID);
			return View(obj);
		}
		public ActionResult OrderPersion_Edit(int ID)
		{
			M_OrderPersion obj = B_OrderPersion.Find(ID);
			return View(obj);
		}

		[HttpPost]
		public string DelOrderPersion(int ID)
		{
			JsonMessage result;

			bool bol = B_OrderPersion.Del(ID);

			if (bol)
				result = new JsonMessage() { type = 0, message = "成功", value = "" };
			else
				result = new JsonMessage() { type = -1, message = "失败", value = "" };

			Response.ContentType = "application/json";
			Response.Charset = "UTF-8";
			return JsonConvert.SerializeObject(result);
		}

		[HttpPost]
		public string EditOrderPersion()
		{
			JsonMessage result;
			byte[] byts = new byte[Request.InputStream.Length];
			Request.InputStream.Read(byts, 0, byts.Length);
			string req = Encoding.UTF8.GetString(byts);

			M_OrderPersion obj = JsonConvert.DeserializeObject<M_OrderPersion>(req);
			bool bol = B_OrderPersion.Update(obj);
			if (bol)
				result = new JsonMessage() { type = 0, message = "成功", value = req };
			else
				result = new JsonMessage() { type = -1, message = "失败", value = req };

			Response.ContentType = "application/json";
			Response.Charset = "UTF-8";
			return JsonConvert.SerializeObject(result);
		}

		[HttpPost]
		public string AddOrderPersion()
		{
			JsonMessage result;
			byte[] byts = new byte[Request.InputStream.Length];
			Request.InputStream.Read(byts, 0, byts.Length);
			string req = Encoding.UTF8.GetString(byts);

			M_OrderPersion obj = JsonConvert.DeserializeObject<M_OrderPersion>(req);
			obj = B_OrderPersion.Add(obj);
			result = new JsonMessage() { type = 0, message = "成功", value = req };
			Response.ContentType = "application/json";
			Response.Charset = "UTF-8";
			return JsonConvert.SerializeObject(result);
		}

        public string GetOrderPersionListByCode(GridPager pager, string OrderCode)
        {
            List<M_OrderPersion> list;
            int count;
            if (string.IsNullOrEmpty(OrderCode))
            {
                list = B_OrderPersion.GetListByPage(t => 1 == 1, pager);
                count = B_OrderPersion.GetCount(t => 1 == 1);
            }
            else
            {
                list = B_OrderPersion.GetListByPage(t => t.OrderCode== OrderCode, pager);
                count = B_OrderPersion.GetCount(t => t.OrderCode== OrderCode);
            }
            GridRows<M_OrderPersion> grs = new GridRows<M_OrderPersion>();
            grs.rows = list;
            grs.total = count;
            Response.ContentType = "application/json";
            Response.Charset = "UTF-8";
            IsoDateTimeConverter timeFormat = new IsoDateTimeConverter();
            timeFormat.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
            return JsonConvert.SerializeObject(grs, Formatting.Indented, timeFormat);
        }

        public string GetOrderPersionList(GridPager pager, string queryStr,string OrderCode)
		{
			List<M_OrderPersion> list;
			int count;
			if (string.IsNullOrEmpty(queryStr))
			{
				list = B_OrderPersion.GetListByPage(t => t.OrderCode== OrderCode, pager);
				count = B_OrderPersion.GetCount(t => t.OrderCode== OrderCode);
			}
			else
			{
				list = B_OrderPersion.GetListByPage(t => t.OrderCode.Contains(queryStr), pager);
				count = B_OrderPersion.GetCount(t => t.OrderCode.Contains(queryStr));
			}
			GridRows<M_OrderPersion> grs = new GridRows<M_OrderPersion>();
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
