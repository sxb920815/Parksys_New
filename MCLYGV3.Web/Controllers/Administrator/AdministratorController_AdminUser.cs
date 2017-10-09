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

		#region 管理员

		public ActionResult AdminUser_List()
		{
			return View(MyUser);
		}
		public ActionResult AdminUser_Add()
		{
			return View();
		}
		public ActionResult AdminUser_Detail(int ID)
		{
			M_AdminUser obj = B_AdminUser.Find(ID);
			return View(obj);
		}
		public ActionResult AdminUser_Edit(int ID)
		{
			M_AdminUser obj = B_AdminUser.Find(ID);
			return View(obj);
		}

		[HttpPost]
		public string DelAdminUser(int ID)
		{
			JsonMessage result;

			bool bol = B_AdminUser.Del(ID);

			if (bol)
				result = new JsonMessage() { type = 0, message = "成功", value = "" };
			else
				result = new JsonMessage() { type = -1, message = "失败", value = "" };

			Response.ContentType = "application/json";
			Response.Charset = "UTF-8";
			return JsonConvert.SerializeObject(result);
		}

		[HttpPost]
		public string EditAdminUser()
		{
			JsonMessage result;
			byte[] byts = new byte[Request.InputStream.Length];
			Request.InputStream.Read(byts, 0, byts.Length);
			string req = Encoding.UTF8.GetString(byts);

			M_AdminUser obj = JsonConvert.DeserializeObject<M_AdminUser>(req);
			bool bol = B_AdminUser.Update(obj);
			if (bol)
				result = new JsonMessage() { type = 0, message = "成功", value = req };
			else
				result = new JsonMessage() { type = -1, message = "失败", value = req };

			Response.ContentType = "application/json";
			Response.Charset = "UTF-8";
			return JsonConvert.SerializeObject(result);
		}

		[HttpPost]
		public string AddAdminUser()
		{
			JsonMessage result;
			byte[] byts = new byte[Request.InputStream.Length];
			Request.InputStream.Read(byts, 0, byts.Length);
			string req = Encoding.UTF8.GetString(byts);

			M_AdminUser obj = JsonConvert.DeserializeObject<M_AdminUser>(req);
			obj.RegTime = DateTime.Now;
			obj.NowTime = DateTime.Now;
			obj.LastTime = DateTime.Now;
			obj.PassWord = Common.Sha1(obj.PassWord);
			obj = B_AdminUser.Add(obj);
			result = new JsonMessage() { type = 0, message = "成功", value = req };
			Response.ContentType = "application/json";
			Response.Charset = "UTF-8";
			return JsonConvert.SerializeObject(result);
		}

		public string GetAdminUserList(GridPager pager, string queryStr)
		{
			List<M_AdminUser> list;
			int count;
			if (string.IsNullOrEmpty(queryStr))
			{
				list = B_AdminUser.GetListByPage(t => 1 == 1, pager);
				count = B_AdminUser.GetCount(t => 1 == 1);
			}
			else
			{
				list = B_AdminUser.GetListByPage(t => t.UserName.Contains(queryStr), pager);
				count = B_AdminUser.GetCount(t => t.UserName.Contains(queryStr));
			}
			GridRows<M_AdminUser> grs = new GridRows<M_AdminUser>();
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
