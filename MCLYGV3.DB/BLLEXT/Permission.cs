using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;

namespace MCLYGV3.DB
{

	/// <summary>
	/// 权限表数据库操作类
	/// </summary>
	public partial class B_Permission
	{
		

		/// <summary>
		/// 添加权限表
		/// </summary>
		/// <param name="PermissionObj">权限表实体</param>
		/// <returns></returns>
		public static bool HasChild(string id)
		{
			using (DBContext db = new DBContext())
			{
				int count = db.PermissionList.Count(t => t.ParentId == id);
				if (count > 0)
					return true;
				else
					return false;
			}
		}

		


	}
}