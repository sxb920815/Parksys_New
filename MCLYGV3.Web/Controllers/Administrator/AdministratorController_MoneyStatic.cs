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
		
		#region 奖金表

		public ActionResult MoneyStatic_List()
		{
			return View(MyUser);
		}
		public ActionResult MoneyStatic_Add()
		{
            List<SelectListItem> InCompanySelect = new List<SelectListItem>();

            var CompanyList = B_Company.GetList(t => true);
            foreach (var item in CompanyList)
            {
                SelectListItem li = new SelectListItem() { Text = item.CompanyName, Value = item.ID.ToString() };
                InCompanySelect.Add(li);
            }
            ViewData["InCompanySelect"] = new SelectList(InCompanySelect, "Value", "Text", "0");
            return View();
		}
		public ActionResult MoneyStatic_Detail(int ID)
		{
			M_MoneyStatic obj = B_MoneyStatic.Find(ID);
            ViewBag.companyName = B_Company.Find(obj.CompanyId).CompanyName;
            ViewBag.saler = B_UserInfo.Find(obj.UserId).UserName;
			return View(obj);
		}
		public ActionResult MoneyStatic_Edit(int ID)
		{
			M_MoneyStatic obj = B_MoneyStatic.Find(ID);
			return View(obj);
		}

		[HttpPost]
		public string DelMoneyStatic(int ID)
		{
			JsonMessage result;

			bool bol = B_MoneyStatic.Del(ID);

			if (bol)
				result = new JsonMessage() { type = 0, message = "成功", value = "" };
			else
				result = new JsonMessage() { type = -1, message = "失败", value = "" };

			Response.ContentType = "application/json";
			Response.Charset = "UTF-8";
			return JsonConvert.SerializeObject(result);
		}

		[HttpPost]
		public string EditMoneyStatic()
		{
			JsonMessage result;
			byte[] byts = new byte[Request.InputStream.Length];
			Request.InputStream.Read(byts, 0, byts.Length);
			string req = Encoding.UTF8.GetString(byts);

			M_MoneyStatic obj = JsonConvert.DeserializeObject<M_MoneyStatic>(req);
			bool bol = B_MoneyStatic.Update(obj);
			if (bol)
				result = new JsonMessage() { type = 0, message = "成功", value = req };
			else
				result = new JsonMessage() { type = -1, message = "失败", value = req };

			Response.ContentType = "application/json";
			Response.Charset = "UTF-8";
			return JsonConvert.SerializeObject(result);
		}

		[HttpPost]
		public string AddMoneyStatic()
		{
			JsonMessage result;
			byte[] byts = new byte[Request.InputStream.Length];
			Request.InputStream.Read(byts, 0, byts.Length);
			string req = Encoding.UTF8.GetString(byts);

			M_MoneyStatic obj = JsonConvert.DeserializeObject<M_MoneyStatic>(req);
			obj = B_MoneyStatic.Add(obj);
			result = new JsonMessage() { type = 0, message = "成功", value = req };
			Response.ContentType = "application/json";
			Response.Charset = "UTF-8";
			return JsonConvert.SerializeObject(result);
		}

		public string GetMoneyStaticList(GridPager pager, string queryStr)
		{
			List<M_MoneyStatic> list;
			int count;
			if (string.IsNullOrEmpty(queryStr))
			{
				list = B_MoneyStatic.GetListByPage(t => 1 == 1, pager);
				count = B_MoneyStatic.GetCount(t => 1 == 1);
			}
			else
			{
				list = B_MoneyStatic.GetListByPage(t => t.ProductName.Contains(queryStr), pager);
				count = B_MoneyStatic.GetCount(t => t.ProductName.Contains(queryStr));
			}
			GridRows<M_MoneyStatic> grs = new GridRows<M_MoneyStatic>();
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
