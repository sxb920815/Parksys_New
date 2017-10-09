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
		
		#region 代理公司

		public ActionResult Company_List()
		{
			return View(MyUser);
		}
		public ActionResult Company_Add()
		{
			return View();
		}
		public ActionResult Company_Detail(int ID)
		{
			M_Company obj = B_Company.Find(ID);
			return View(obj);
		}
		public ActionResult Company_Edit(int ID)
		{
			M_Company obj = B_Company.Find(ID);
			return View(obj);
		}

		[HttpPost]
		public string DelCompany(int ID)
		{
			JsonMessage result;

			bool bol = B_Company.Del(ID);

			if (bol)
				result = new JsonMessage() { type = 0, message = "成功", value = "" };
			else
				result = new JsonMessage() { type = -1, message = "失败", value = "" };

			Response.ContentType = "application/json";
			Response.Charset = "UTF-8";
			return JsonConvert.SerializeObject(result);
		}

		[HttpPost]
		public string EditCompany()
		{
			JsonMessage result;
			byte[] byts = new byte[Request.InputStream.Length];
			Request.InputStream.Read(byts, 0, byts.Length);
			string req = Encoding.UTF8.GetString(byts);

			M_Company obj = JsonConvert.DeserializeObject<M_Company>(req);
			bool bol = B_Company.Update(obj);
			if (bol)
				result = new JsonMessage() { type = 0, message = "成功", value = req };
			else
				result = new JsonMessage() { type = -1, message = "失败", value = req };

			Response.ContentType = "application/json";
			Response.Charset = "UTF-8";
			return JsonConvert.SerializeObject(result);
		}

		[HttpPost]
		public string AddCompany()
		{
			JsonMessage result;
			byte[] byts = new byte[Request.InputStream.Length];
			Request.InputStream.Read(byts, 0, byts.Length);
			string req = Encoding.UTF8.GetString(byts);

			M_Company obj = JsonConvert.DeserializeObject<M_Company>(req);
			obj = B_Company.Add(obj);
			result = new JsonMessage() { type = 0, message = "成功", value = req };
			Response.ContentType = "application/json";
			Response.Charset = "UTF-8";
			return JsonConvert.SerializeObject(result);
		}

		public string GetCompanyList(GridPager pager, string queryStr)
		{
			List<M_Company> list;
			int count;
			if (string.IsNullOrEmpty(queryStr))
			{
				list = B_Company.GetListByPage(t => 1 == 1, pager);
				count = B_Company.GetCount(t => 1 == 1);
			}
			else
			{
				list = B_Company.GetListByPage(t => t.CompanyName.Contains(queryStr), pager);
				count = B_Company.GetCount(t => t.CompanyName.Contains(queryStr));
			}
			GridRows<M_Company> grs = new GridRows<M_Company>();
			grs.rows = list;
			grs.total = count;
			Response.ContentType = "application/json";
			Response.Charset = "UTF-8";
			IsoDateTimeConverter timeFormat = new IsoDateTimeConverter();
			timeFormat.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
			return JsonConvert.SerializeObject(grs, Formatting.Indented, timeFormat);
		}
        public string GetCompanyNameById(int id)
        {
            M_Company company = B_Company.Find(id);
            if (company != null)
            {
                return company.CompanyName;
            }
            else
            {
                return "";
            }
        }
        public string GetUserList(int company) 
        {
            List<string> UserSelect = new List<string>();
            
            var UserList = B_UserInfo.GetList(t => t.InCompany.ID== company);
            var sb = new StringBuilder();
            foreach (var item in UserList)
            {
                sb.Append($"<option value=\"{item.ID}\">{item.UserName}</option>");
            }
            return sb.ToString();
        }
        #endregion

    }
}
