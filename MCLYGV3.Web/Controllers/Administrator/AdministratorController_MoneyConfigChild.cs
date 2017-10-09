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
		
		#region 奖金配置表

		public ActionResult MoneyConfigChild_List()
		{
			return View(MyUser);
		}
		public ActionResult MoneyConfigChild_Add()
		{
            List<SelectListItem> InCompanySelect = new List<SelectListItem>();
            List<SelectListItem> UserList = new List<SelectListItem>();
            var CompanyList = B_Company.GetList(t => true);
            foreach (var item in CompanyList)
            {
                SelectListItem li = new SelectListItem() { Text = item.CompanyName, Value = item.ID.ToString() };
                InCompanySelect.Add(li);
            }
            ViewData["Companyid"] = new SelectList(InCompanySelect, "Value", "Text", "0");
            ViewData["UserId"] = new SelectList(UserList);
            return View();
		}
		public ActionResult MoneyConfigChild_Detail(int ID)
		{
			M_MoneyConfigChild obj = B_MoneyConfigChild.Find(ID);
            ViewBag.userName = B_UserInfo.Find(obj.ID).UserName;
			return View(obj);
		}
		public ActionResult MoneyConfigChild_Edit(int ID)
		{
			M_MoneyConfigChild obj = B_MoneyConfigChild.Find(ID);
			return View(obj);
		}

		[HttpPost]
		public string DelMoneyConfigChild(int ID)
		{
			JsonMessage result;

			bool bol = B_MoneyConfigChild.Del(ID);

			if (bol)
				result = new JsonMessage() { type = 0, message = "成功", value = "" };
			else
				result = new JsonMessage() { type = -1, message = "失败", value = "" };

			Response.ContentType = "application/json";
			Response.Charset = "UTF-8";
			return JsonConvert.SerializeObject(result);
		}

		[HttpPost]
		public string EditMoneyConfigChild()
		{
			JsonMessage result;
			byte[] byts = new byte[Request.InputStream.Length];
			Request.InputStream.Read(byts, 0, byts.Length);
			string req = Encoding.UTF8.GetString(byts);

			M_MoneyConfigChild obj = JsonConvert.DeserializeObject<M_MoneyConfigChild>(req);
			bool bol = B_MoneyConfigChild.Update(obj);
			if (bol)
				result = new JsonMessage() { type = 0, message = "成功", value = req };
			else
				result = new JsonMessage() { type = -1, message = "失败", value = req };

			Response.ContentType = "application/json";
			Response.Charset = "UTF-8";
			return JsonConvert.SerializeObject(result);
		}

		[HttpPost]
		public string AddMoneyConfigChild()
		{
			JsonMessage result;
			byte[] byts = new byte[Request.InputStream.Length];
			Request.InputStream.Read(byts, 0, byts.Length);
			string req = Encoding.UTF8.GetString(byts);

			M_MoneyConfigChild obj = JsonConvert.DeserializeObject<M_MoneyConfigChild>(req);
			obj = B_MoneyConfigChild.Add(obj);
			result = new JsonMessage() { type = 0, message = "成功", value = req };
			Response.ContentType = "application/json";
			Response.Charset = "UTF-8";
			return JsonConvert.SerializeObject(result);
		}

		public string GetMoneyConfigChildList(GridPager pager, string queryStr)
		{
			List<M_MoneyConfigChild> list;
			int count;
			if (string.IsNullOrEmpty(queryStr))
			{
				list = B_MoneyConfigChild.GetListByPage(t => 1 == 1, pager);
				count = B_MoneyConfigChild.GetCount(t => 1 == 1);
			}
			else
			{
				list = B_MoneyConfigChild.GetListByPage(t => t.ProductName.Contains(queryStr), pager);
				count = B_MoneyConfigChild.GetCount(t => t.ProductName.Contains(queryStr));
			}
			GridRows<M_MoneyConfigChild> grs = new GridRows<M_MoneyConfigChild>();
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
