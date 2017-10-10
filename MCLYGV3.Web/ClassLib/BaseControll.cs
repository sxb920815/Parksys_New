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
        public readonly DB.BaseDataService<M_Equipment> _bs_equ;
        public readonly DB.BaseDataService<DB.M_Area> _bs_Area;
        public readonly DB.BaseDataService<M_AreaAndEqu> _bs_AreaAndEqu;
        public readonly DB.BaseDataService<M_Car> _bs_Car;
        public AdministratorControll()
        {
            _bs_equ = new BaseDataService<M_Equipment>();
            _bs_Area = new BaseDataService<DB.M_Area>();
            _bs_AreaAndEqu = new BaseDataService<M_AreaAndEqu>();
            _bs_Car = new BaseDataService<M_Car>();
        }
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