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

		public ActionResult MoneyConfig_List()
		{
			return View(MyUser);
		}
		public ActionResult MoneyConfig_Add()
		{
            List<SelectListItem> InCompanySelect = new List<SelectListItem>();

            var CompanyList = B_Company.GetList(t => true);
            foreach (var item in CompanyList)
            {
                SelectListItem li = new SelectListItem() { Text = item.CompanyName, Value = item.ID.ToString() };
                InCompanySelect.Add(li);
            }
            ViewData["CompanyId"] = new SelectList(InCompanySelect, "Value", "Text", "0");
            return View();
		}
		public ActionResult MoneyConfig_Detail(int ID)
		{
			M_MoneyConfig obj = B_MoneyConfig.Find(ID);
            ViewBag.companyName = B_Company.Find(obj.CompanyId).CompanyName;
			return View(obj);
		}
		public ActionResult MoneyConfig_Edit(int ID)
		{
			M_MoneyConfig obj = B_MoneyConfig.Find(ID);
            List<SelectListItem> InCompanySelect = new List<SelectListItem>();

            var CompanyList = B_Company.GetList(t => true);
            foreach (var item in CompanyList)
            {
                SelectListItem li = new SelectListItem() { Text = item.CompanyName, Value = item.ID.ToString() };
                InCompanySelect.Add(li);
            }
            ViewData["InCompanySelect"] = new SelectList(InCompanySelect, "Value", "Text", "0");
            return View(obj);
		}

		[HttpPost]
		public string DelMoneyConfig(int ID)
		{
			JsonMessage result;

			bool bol = B_MoneyConfig.Del(ID);

			if (bol)
				result = new JsonMessage() { type = 0, message = "成功", value = "" };
			else
				result = new JsonMessage() { type = -1, message = "失败", value = "" };

			Response.ContentType = "application/json";
			Response.Charset = "UTF-8";
			return JsonConvert.SerializeObject(result);
		}

		[HttpPost]
		public string EditMoneyConfig()
		{
			JsonMessage result;
			byte[] byts = new byte[Request.InputStream.Length];
			Request.InputStream.Read(byts, 0, byts.Length);
			string req = Encoding.UTF8.GetString(byts);

			M_MoneyConfig obj = JsonConvert.DeserializeObject<M_MoneyConfig>(req);
			bool bol = B_MoneyConfig.Update(obj);
			if (bol)
				result = new JsonMessage() { type = 0, message = "成功", value = req };
			else
				result = new JsonMessage() { type = -1, message = "失败", value = req };

			Response.ContentType = "application/json";
			Response.Charset = "UTF-8";
			return JsonConvert.SerializeObject(result);
		}

		[HttpPost]
		public string AddMoneyConfig()
		{
			JsonMessage result;
			byte[] byts = new byte[Request.InputStream.Length];
			Request.InputStream.Read(byts, 0, byts.Length);
			string req = Encoding.UTF8.GetString(byts);

			M_MoneyConfig obj = JsonConvert.DeserializeObject<M_MoneyConfig>(req);
            if (B_MoneyConfig.GetCount(t => t.ProductName == obj.ProductName && t.CompanyId == obj.CompanyId) != 0)
            {
                result = new JsonMessage() { type = 1, message = "添加失败!不能添加产品名称,公司名称一样的记录", value = "" };
            }
            else if (obj.Rate < obj.ChildRate)
            {
                result = new JsonMessage() { type = 1, message = "公司费率不能低于业务员默认费率", value = "" };
            }
            else if (obj.Rate>1||obj.ChildRate>1)
            {
                result = new JsonMessage() { type = 1, message = "费率不能高于1", value = "" };
            }
            else { 

                obj = B_MoneyConfig.Add(obj);
                result = new JsonMessage() { type = 0, message = "成功", value = req };
            }
			Response.ContentType = "application/json";
			Response.Charset = "UTF-8";
			return JsonConvert.SerializeObject(result);
		}

		public string GetMoneyConfigList(GridPager pager, string queryStr)
		{
			List<M_MoneyConfig> list;
			int count;
			if (string.IsNullOrEmpty(queryStr))
			{
				list = B_MoneyConfig.GetListByPage(t => 1 == 1, pager);
				count = B_MoneyConfig.GetCount(t => 1 == 1);
			}
			else
			{
				list = B_MoneyConfig.GetListByPage(t => t.ProductName.Contains(queryStr), pager);
				count = B_MoneyConfig.GetCount(t => t.ProductName.Contains(queryStr));
			}
			GridRows<M_MoneyConfig> grs = new GridRows<M_MoneyConfig>();
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
