using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;

namespace MCLYGV3.DB
{

	/// <summary>
	/// 项目配置数据库操作类
	/// </summary>
	public partial class B_ItemInfo
	{
		/// <summary>
		/// 添加项目配置
		/// </summary>
		/// <param name="Name">项目名称</param>
		/// <param name="PID">上级ID</param>
		/// <param name="type">类别</param>

		/// <returns></returns>
		public static M_ItemInfo Add(string Name,int PID,string type)
		{
			M_ItemInfo ItemInfoObj = new M_ItemInfo();
			ItemInfoObj.Name = Name;
			ItemInfoObj.PID = PID;
			ItemInfoObj.type = type;

			return Add(ItemInfoObj);
		}

		/// <summary>
		/// 添加项目配置
		/// </summary>
		/// <param name="ItemInfoObj">项目配置实体</param>
		/// <returns></returns>
		public static M_ItemInfo Add(M_ItemInfo ItemInfoObj)
		{
			using (DBContext db = new DBContext())
			{
				try
				{
					
					db.ItemInfoList.Add(ItemInfoObj);
					int result = db.SaveChanges();
					return ItemInfoObj;
				}
				catch (DbEntityValidationException ex)
				{
					StringBuilder sb = new StringBuilder();
					foreach (var item in ex.EntityValidationErrors)
					{
						foreach (var item2 in item.ValidationErrors)
						{
							sb.Append($"PropertyName:{item2.PropertyName},{item2.ErrorMessage}\r\n\r\n");
						}
					}
					Log.SystemWrite("【ItemInfo】\r\n" + ex.Message + "\r\n\r\n" + sb.ToString());
					return null;
				}
			}
		}
	}
}