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
		
		#region 角色权限操作表

		public ActionResult PermissionOperation_List()
		{
			return View(MyUser);
		}
		public ActionResult PermissionOperation_Add()
		{
			return View();
		}
		public ActionResult PermissionOperation_Detail(string Ids)
		{
			M_PermissionOperation obj = B_PermissionOperation.Find(Ids);
			return View(obj);
		}
		public ActionResult PermissionOperation_Edit(string Ids)
		{
			M_PermissionOperation obj = B_PermissionOperation.Find(Ids);
			return View(obj);
		}

		[HttpPost]
		public string DelPermissionOperation(string Ids)
		{
			JsonMessage result;

			bool bol = B_PermissionOperation.Del(Ids);

			if (bol)
				result = new JsonMessage() { type = 0, message = "成功", value = "" };
			else
				result = new JsonMessage() { type = -1, message = "失败", value = "" };

			Response.ContentType = "application/json";
			Response.Charset = "UTF-8";
			return JsonConvert.SerializeObject(result);
		}

		[HttpPost]
		public string EditPermissionOperation()
		{
			JsonMessage result;
			byte[] byts = new byte[Request.InputStream.Length];
			Request.InputStream.Read(byts, 0, byts.Length);
			string req = Encoding.UTF8.GetString(byts);

			M_PermissionOperation obj = JsonConvert.DeserializeObject<M_PermissionOperation>(req);
			bool bol = B_PermissionOperation.Update(obj);
			if (bol)
				result = new JsonMessage() { type = 0, message = "成功", value = req };
			else
				result = new JsonMessage() { type = -1, message = "失败", value = req };

			Response.ContentType = "application/json";
			Response.Charset = "UTF-8";
			return JsonConvert.SerializeObject(result);
		}

		[HttpPost]
		public string AddPermissionOperation()
		{
			JsonMessage result;
			byte[] byts = new byte[Request.InputStream.Length];
			Request.InputStream.Read(byts, 0, byts.Length);
			string req = Encoding.UTF8.GetString(byts);

			M_PermissionOperation obj = JsonConvert.DeserializeObject<M_PermissionOperation>(req);
			obj = B_PermissionOperation.Add(obj);
			result = new JsonMessage() { type = 0, message = "成功", value = req };
			Response.ContentType = "application/json";
			Response.Charset = "UTF-8";
			return JsonConvert.SerializeObject(result);
		}

		public string GetPermissionOperationList(GridPager pager, string queryStr)
		{
			List<M_PermissionOperation> list;
			int count;
			if (string.IsNullOrEmpty(queryStr))
			{
				list = B_PermissionOperation.GetListByPage(t => 1 == 1, pager);
				count = B_PermissionOperation.GetCount(t => 1 == 1);
			}
			else
			{
				list = B_PermissionOperation.GetListByPage(t => t.Name.Contains(queryStr), pager);
				count = B_PermissionOperation.GetCount(t => t.Name.Contains(queryStr));
			}
			GridRows<M_PermissionOperation> grs = new GridRows<M_PermissionOperation>();
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
