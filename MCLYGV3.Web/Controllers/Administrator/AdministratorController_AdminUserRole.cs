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

		public ActionResult GetRoleByUser(int userId)
		{
			M_AdminUser User = B_AdminUser.Find(userId);
			return View(User);
		}

		[HttpPost]
		public string GetRoleListByUser(GridPager pager, int userId)
		{
			List<Role> list = new List<Role>();
			int count = B_Role.GetCount(t => 1 == 1);

			var rolelist = B_Role.GetListByPage(t => true, pager);



			string Flag;
			foreach (var item in rolelist)
			{
				if (B_AdminUser.HasRole(userId, item.ID))
					Flag = "1";
				else
					Flag = "0";

				Role role = new Role() { Id = item.ID.ToString(), Name = item.Name, Description = item.Description, Flag = Flag };
				list.Add(role);
			}

			GridRows<Role> grs = new GridRows<Role>();
			grs.rows = list;
			grs.total = count;
			Response.ContentType = "application/json";
			Response.Charset = "UTF-8";
			IsoDateTimeConverter timeFormat = new IsoDateTimeConverter();
			timeFormat.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
			return JsonConvert.SerializeObject(grs, Formatting.Indented, timeFormat);
		}

		[HttpPost]
		public string UpdateUserRoleByUserId(int userId, string roleIds)
		{
			JsonMessage result;
			bool bol = B_AdminUser.SetRole(userId, roleIds);




			if (bol)
				result = new JsonMessage() { type = 0, message = "成功", value = roleIds };
			else
				result = new JsonMessage() { type = -1, message = "失败", value = roleIds };
			Response.ContentType = "application/json";
			Response.Charset = "UTF-8";
			return JsonConvert.SerializeObject(result);
		}




	}
	public class Role
	{
		public string Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public string Flag { get; set; }
	}
}
