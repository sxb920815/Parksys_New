using MCLYGV3.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MCLYGV3.Web.App_Start
{
	public class SystemConfig
	{
		public static void Run()
		{
			int count = B_AdminUser.GetCount(t => true);
			if (count == 0)
			{
				M_AdminUser AdminUser = new M_AdminUser();
				AdminUser.UserName = "admin";
				AdminUser.RealName = "管理员";
				AdminUser.PassWord = Common.Sha1("123456");
				AdminUser.IsSupper = true;
				AdminUser.RegTime = DateTime.Now;
				AdminUser.NowTime = DateTime.Now;
				AdminUser.LastTime = DateTime.Now;
				AdminUser.InCompanyId = 0;
				B_AdminUser.Add(AdminUser);
			}

			count = B_Permission.GetCount(t => true);
			if (count == 0)
			{
				AdminMenuOperation.MakePermissionDB();
			}
		}
	}
}