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

		public ActionResult OrderChild_List(string OrderCode)
		{
            ViewBag.OrderCode = OrderCode;
			return View(MyUser);
		}
		public ActionResult OrderChild_Add()
		{
			return View();
		}
		public ActionResult OrderChild_Detail(string ChildCode)
		{
			M_OrderChild obj = B_OrderChild.Find(ChildCode);
			return View(obj);
		}
		public ActionResult OrderChild_Edit(string ChildCode)
		{
			M_OrderChild obj = B_OrderChild.Find(ChildCode);
			return View(obj);
		}

		[HttpPost]
		public string DelOrderChild(string ChildCode)
		{
			JsonMessage result;

			bool bol = B_OrderChild.Del(ChildCode);

			if (bol)
				result = new JsonMessage() { type = 0, message = "成功", value = "" };
			else
				result = new JsonMessage() { type = -1, message = "失败", value = "" };

			Response.ContentType = "application/json";
			Response.Charset = "UTF-8";
			return JsonConvert.SerializeObject(result);
		}

		[HttpPost]
		public string EditOrderChild()
		{
			JsonMessage result;
			byte[] byts = new byte[Request.InputStream.Length];
			Request.InputStream.Read(byts, 0, byts.Length);
			string req = Encoding.UTF8.GetString(byts);

			M_OrderChild obj = JsonConvert.DeserializeObject<M_OrderChild>(req);
			bool bol = B_OrderChild.Update(obj);
			if (bol)
				result = new JsonMessage() { type = 0, message = "成功", value = req };
			else
				result = new JsonMessage() { type = -1, message = "失败", value = req };

			Response.ContentType = "application/json";
			Response.Charset = "UTF-8";
			return JsonConvert.SerializeObject(result);
		}

		[HttpPost]
		public string AddOrderChild()
		{
			JsonMessage result;
			byte[] byts = new byte[Request.InputStream.Length];
			Request.InputStream.Read(byts, 0, byts.Length);
			string req = Encoding.UTF8.GetString(byts);

			M_OrderChild obj = JsonConvert.DeserializeObject<M_OrderChild>(req);
			obj = B_OrderChild.Add(obj);
			result = new JsonMessage() { type = 0, message = "成功", value = req };
			Response.ContentType = "application/json";
			Response.Charset = "UTF-8";
			return JsonConvert.SerializeObject(result);
		}

		public string GetOrderChildListByCode(GridPager pager, string OrderCode)
		{
			List<M_OrderChild> list;
			int count;
			if (string.IsNullOrEmpty(OrderCode))
			{
				list = B_OrderChild.GetListByPage(t => 1 == 1, pager);
				count = B_OrderChild.GetCount(t => 1 == 1);
			}
			else
			{
				list = B_OrderChild.GetListByPage(t => t.OrderCode==OrderCode, pager);
				count = B_OrderChild.GetCount(t => t.OrderCode==OrderCode);
			}
			GridRows<M_OrderChild> grs = new GridRows<M_OrderChild>();
			grs.rows = list;
			grs.total = count;
			Response.ContentType = "application/json";
			Response.Charset = "UTF-8";
			IsoDateTimeConverter timeFormat = new IsoDateTimeConverter();
			timeFormat.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
			return JsonConvert.SerializeObject(grs, Formatting.Indented, timeFormat);
		}

        #endregion
        public string GetOrderChildList(GridPager pager, string queryStr,string OrderCode)
        {
            using (var db = new DBContext())
            {
                List<M_OrderChild> list;
                int count;
                if (string.IsNullOrEmpty(queryStr))
                {
                    list = B_OrderChild.GetListByPage(t => t.OrderCode==OrderCode, pager);
                    count = B_OrderChild.GetCount(t => t.OrderCode == OrderCode);
                }
                else
                {
                    list = B_OrderChild.GetListByPage(t => t.ChildCode.Contains(queryStr)&&t.OrderCode==OrderCode, pager);
                    count = B_OrderChild.GetCount(t => t.OrderCode.Contains(queryStr) && t.OrderCode == OrderCode);
                }
                GridRows<M_OrderChild> grs = new GridRows<M_OrderChild>();
                grs.rows = list;
                grs.total = count;
                Response.ContentType = "application/json";
                Response.Charset = "UTF-8";
                IsoDateTimeConverter timeFormat = new IsoDateTimeConverter();
                timeFormat.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
                return JsonConvert.SerializeObject(grs, Formatting.Indented, timeFormat);
            }

        }
    }
}
