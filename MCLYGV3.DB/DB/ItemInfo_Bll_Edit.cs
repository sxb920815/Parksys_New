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
		/// 修改项目配置
		/// </summary>
		/// <param name="ItemInfoObj">项目配置实体</param>
		/// <returns></returns>
		public static bool UpdateNormal(M_ItemInfo ItemInfoObj)
		{
			using (DBContext db = new DBContext())
			{
				try
				{
					db.ItemInfoList.Attach(ItemInfoObj);
					DbEntityEntry<M_ItemInfo> entry = db.Entry(ItemInfoObj);
					entry.State = EntityState.Modified;
					db.SaveChanges();
					return true;
				}
				catch (Exception ex)
				{
					Log.SystemWrite("【ItemInfo】\r\n" + ex.Message + "\r\n" + ex.ToString());
					return false;
				}
			}
		}

		/// <summary>
		/// 修改项目配置
		/// </summary>
		/// <param name="ItemInfoObj">项目配置实体</param>
		/// <returns></returns>
		public static bool Update(M_ItemInfo EditItemInfoObj)
		{
			using (DBContext db = new DBContext())
			{
				try
				{
					M_ItemInfo ItemInfoObj = db.ItemInfoList.Find(EditItemInfoObj.ID);
					ItemInfoObj.Name = EditItemInfoObj.Name;
					ItemInfoObj.PID = EditItemInfoObj.PID;
					ItemInfoObj.type = EditItemInfoObj.type;

					int count = db.SaveChanges();
					return true;
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
					return false;
				}
			}
		}
	}
}