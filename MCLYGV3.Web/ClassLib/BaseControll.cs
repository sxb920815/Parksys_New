using MCLYGV3.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MCLYGV3.Web
{
	public class AdministratorControll : Controller
	{
		public M_AdminUser MyUser;
		protected override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			//Session["AdminUser"] = 1;

			if (Session["AdminUser"] == null)
			{
				filterContext.Result = new RedirectResult("/Login/AdminLogin");
			}
			else
			{
				int UserID = (int)Session["AdminUser"];
				MyUser = B_AdminUser.Find(UserID);
			}

			base.OnActionExecuting(filterContext);
		}

	}
}