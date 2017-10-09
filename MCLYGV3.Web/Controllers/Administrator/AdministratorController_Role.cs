using MCLYGV3.DB;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Mvc;

namespace MCLYGV3.Web.Controllers
{
	public partial class AdministratorController : AdministratorControll
	{
		
		#region 角色表

		public ActionResult Role_List()
		{
			return View(MyUser);
		}
		public ActionResult Role_Add()
		{
			return View();
		}
		public ActionResult Role_Detail(int ID)
		{
			M_Role obj = B_Role.Find(ID);
			return View(obj);
		}
		public ActionResult Role_Edit(int ID)
		{
			M_Role obj = B_Role.Find(ID);
			return View(obj);
		}

		[HttpPost]
		public string DelRole(int ID)
		{
			JsonMessage result;

			bool bol = B_Role.Del(ID);

			if (bol)
				result = new JsonMessage() { type = 0, message = "成功", value = "" };
			else
				result = new JsonMessage() { type = -1, message = "失败", value = "" };

			Response.ContentType = "application/json";
			Response.Charset = "UTF-8";
			return JsonConvert.SerializeObject(result);
		}

		[HttpPost]
		public string EditRole()
		{
			JsonMessage result;
			byte[] byts = new byte[Request.InputStream.Length];
			Request.InputStream.Read(byts, 0, byts.Length);
			string req = Encoding.UTF8.GetString(byts);

			M_Role obj = JsonConvert.DeserializeObject<M_Role>(req);
			bool bol = B_Role.Update(obj);
			if (bol)
				result = new JsonMessage() { type = 0, message = "成功", value = req };
			else
				result = new JsonMessage() { type = -1, message = "失败", value = req };

			Response.ContentType = "application/json";
			Response.Charset = "UTF-8";
			return JsonConvert.SerializeObject(result);
		}

		[HttpPost]
		public string AddRole()
		{
			JsonMessage result;
			byte[] byts = new byte[Request.InputStream.Length];
			Request.InputStream.Read(byts, 0, byts.Length);
			string req = Encoding.UTF8.GetString(byts);

			M_Role obj = JsonConvert.DeserializeObject<M_Role>(req);
			obj.CreateTime = DateTime.Now;
			obj.CreatePerson = MyUser.UserName;
			obj = B_Role.Add(obj);
			result = new JsonMessage() { type = 0, message = "成功", value = req };
			Response.ContentType = "application/json";
			Response.Charset = "UTF-8";
			return JsonConvert.SerializeObject(result);
		}

		public string GetRoleList(GridPager pager, string queryStr)
		{
			List<M_Role> list;
			int count;
			if (string.IsNullOrEmpty(queryStr))
			{
				list = B_Role.GetListByPage(t => 1 == 1, pager);
				count = B_Role.GetCount(t => 1 == 1);
			}
			else
			{
				list = B_Role.GetListByPage(t => t.Name.Contains(queryStr), pager);
				count = B_Role.GetCount(t => t.Name.Contains(queryStr));
			}
			GridRows<M_Role> grs = new GridRows<M_Role>();
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
