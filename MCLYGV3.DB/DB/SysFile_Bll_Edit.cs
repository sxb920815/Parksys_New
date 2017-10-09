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
	/// 文件上传数据库操作类
	/// </summary>
	public partial class B_SysFile
	{
		/// <summary>
		/// 修改文件上传
		/// </summary>
		/// <param name="SysFileObj">文件上传实体</param>
		/// <returns></returns>
		public static bool UpdateNormal(M_SysFile SysFileObj)
		{
			using (DBContext db = new DBContext())
			{
				try
				{
					db.SysFileList.Attach(SysFileObj);
					DbEntityEntry<M_SysFile> entry = db.Entry(SysFileObj);
					entry.State = EntityState.Modified;
					db.SaveChanges();
					return true;
				}
				catch (Exception ex)
				{
					Log.SystemWrite("【SysFile】\r\n" + ex.Message + "\r\n" + ex.ToString());
					return false;
				}
			}
		}

		/// <summary>
		/// 修改文件上传
		/// </summary>
		/// <param name="SysFileObj">文件上传实体</param>
		/// <returns></returns>
		public static bool Update(M_SysFile EditSysFileObj)
		{
			using (DBContext db = new DBContext())
			{
				try
				{
					M_SysFile SysFileObj = db.SysFileList.Find(EditSysFileObj.ID);
					SysFileObj.Name = EditSysFileObj.Name;
					SysFileObj.Type = EditSysFileObj.Type;
					SysFileObj.Url = EditSysFileObj.Url;
					SysFileObj.SmallUrl = EditSysFileObj.SmallUrl;
					SysFileObj.CreatTime = EditSysFileObj.CreatTime;
					SysFileObj.TableName = EditSysFileObj.TableName;
					SysFileObj.TableID = EditSysFileObj.TableID;

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
					Log.SystemWrite("【SysFile】\r\n" + ex.Message + "\r\n\r\n" + sb.ToString());
					return false;
				}
			}
		}
	}
}