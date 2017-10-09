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
		/// 添加文件上传
		/// </summary>
		/// <param name="ID">ID</param>
		/// <param name="Name">文件名称</param>
		/// <param name="Type">文件类型</param>
		/// <param name="Url">文件地址</param>
		/// <param name="SmallUrl">缩略图地址</param>
		/// <param name="CreatTime">上传时间</param>
		/// <param name="TableName">关联表名</param>
		/// <param name="TableID">关联表主键</param>

		/// <returns></returns>
		public static M_SysFile Add(string ID,string Name,string Type,string Url,string SmallUrl,DateTime CreatTime,string TableName,string TableID)
		{
			M_SysFile SysFileObj = new M_SysFile();
			SysFileObj.ID = ID;
			SysFileObj.Name = Name;
			SysFileObj.Type = Type;
			SysFileObj.Url = Url;
			SysFileObj.SmallUrl = SmallUrl;
			SysFileObj.CreatTime = CreatTime;
			SysFileObj.TableName = TableName;
			SysFileObj.TableID = TableID;

			return Add(SysFileObj);
		}

		/// <summary>
		/// 添加文件上传
		/// </summary>
		/// <param name="SysFileObj">文件上传实体</param>
		/// <returns></returns>
		public static M_SysFile Add(M_SysFile SysFileObj)
		{
			using (DBContext db = new DBContext())
			{
				try
				{
					
					db.SysFileList.Add(SysFileObj);
					int result = db.SaveChanges();
					return SysFileObj;
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
					return null;
				}
			}
		}
	}
}