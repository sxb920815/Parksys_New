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
		
		#region 权限表

		public ActionResult Permission_List()
		{
			return View(MyUser);
		}
		public ActionResult Permission_Add()
		{
			return View();
		}
		public ActionResult Permission_Detail(string ID)
		{
			M_Permission obj = B_Permission.Find(ID);
			return View(obj);
		}
		public ActionResult Permission_Edit(string ID)
		{
			M_Permission obj = B_Permission.Find(ID);
			return View(obj);
		}

		[HttpPost]
		public string DelPermission(string ID)
		{
			JsonMessage result;

			bool bol = B_Permission.Del(ID);

			if (bol)
				result = new JsonMessage() { type = 0, message = "成功", value = "" };
			else
				result = new JsonMessage() { type = -1, message = "失败", value = "" };

			Response.ContentType = "application/json";
			Response.Charset = "UTF-8";
			return JsonConvert.SerializeObject(result);
		}

		[HttpPost]
		public string EditPermission()
		{
			JsonMessage result;
			byte[] byts = new byte[Request.InputStream.Length];
			Request.InputStream.Read(byts, 0, byts.Length);
			string req = Encoding.UTF8.GetString(byts);

			M_Permission obj = JsonConvert.DeserializeObject<M_Permission>(req);
			bool bol = B_Permission.Update(obj);
			if (bol)
				result = new JsonMessage() { type = 0, message = "成功", value = req };
			else
				result = new JsonMessage() { type = -1, message = "失败", value = req };

			Response.ContentType = "application/json";
			Response.Charset = "UTF-8";
			return JsonConvert.SerializeObject(result);
		}

		[HttpPost]
		public string AddPermission()
		{
			JsonMessage result;
			byte[] byts = new byte[Request.InputStream.Length];
			Request.InputStream.Read(byts, 0, byts.Length);
			string req = Encoding.UTF8.GetString(byts);

			M_Permission obj = JsonConvert.DeserializeObject<M_Permission>(req);
			obj = B_Permission.Add(obj);
			result = new JsonMessage() { type = 0, message = "成功", value = req };
			Response.ContentType = "application/json";
			Response.Charset = "UTF-8";
			return JsonConvert.SerializeObject(result);
		}

		public string GetPermissionList(GridPager pager, string queryStr)
		{
			List<M_Permission> list;
			int count;
			if (string.IsNullOrEmpty(queryStr))
			{
				list = B_Permission.GetListByPage(t => 1 == 1, pager);
				count = B_Permission.GetCount(t => 1 == 1);
			}
			else
			{
				list = B_Permission.GetListByPage(t => t.Name.Contains(queryStr), pager);
				count = B_Permission.GetCount(t => t.Name.Contains(queryStr));
			}
			GridRows<M_Permission> grs = new GridRows<M_Permission>();
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
