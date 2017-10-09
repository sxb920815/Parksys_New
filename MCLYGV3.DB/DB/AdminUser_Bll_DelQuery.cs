using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using MCLYGV3.DB.ClassLib;
using Newtonsoft.Json;
using System.Data.Entity.Validation;
using System.IO;
using System.Text;


namespace MCLYGV3.DB
{

	/// <summary>
	/// 管理员数据库操作类
	/// </summary>
	public partial class B_AdminUser
	{
		/// <summary>
		/// 返回条数管理员
		/// </summary>
		/// <param name="whereLambda">查询条件lambda表达式</param>
		/// <returns></returns>
		public static int GetCount(Expression<Func<M_AdminUser, bool>> whereLambda)
        	{
            		int result = 0;
            		using (DBContext db = new DBContext())
            		{
                		result = db.AdminUserList.Count(whereLambda);
           		}
           		return result;
       		}
		
		/// <summary>
		/// 查询管理员
		/// </summary>
		/// <param name="whereLambda">查询条件lambda表达式</param>
		/// <returns></returns>
		public static string GetListJson(Expression<Func<M_AdminUser,bool>> whereLambda)
        {
			string JsonStr="[]";
			using (DBContext db = new DBContext())
			{
				List<M_AdminUser> list = db.AdminUserList.Where(whereLambda).ToList();
				JsonSerializerSettings settings = new JsonSerializerSettings();
				settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
				JsonSerializer ser = JsonSerializer.Create(settings);
				using (StringWriter sw = new StringWriter())
				{
					ser.Serialize(sw, list);
					JsonStr = sw.ToString();
				}
			}
			return JsonStr;
		}
		
		/// <summary>
		/// 查询管理员
		/// </summary>
		/// <param name="whereLambda">查询条件lambda表达式</param>
		/// <returns></returns>
		public static List<M_AdminUser> GetList(Expression<Func<M_AdminUser,bool>> whereLambda)
        {
			string JsonStr = GetListJson(whereLambda);
			List<M_AdminUser> list = JsonConvert.DeserializeObject<List<M_AdminUser>>(JsonStr);
			return list;
		}
		
		/// <summary>
		/// 分页查询管理员
		/// </summary>
		/// <param name="whereLambda">查询条件lambda表达式</param>
		/// <param name="pager">分页条件</param>
		/// <returns></returns>
		public static List<M_AdminUser> GetListByPage(Expression<Func<M_AdminUser, bool>> whereLambda, GridPager pager)
		{
			string JsonStr = GetListJsonByPage(whereLambda, pager);
			List<M_AdminUser> list = JsonConvert.DeserializeObject<List<M_AdminUser>>(JsonStr);
			return list;
		}



		/// <summary>
		/// 分页查询管理员
		/// </summary>
		/// <param name="whereLambda">查询条件lambda表达式</param>
		/// <param name="pager">分页条件</param>
		/// <returns></returns>
		public static string GetListJsonByPage(Expression<Func<M_AdminUser, bool>> whereLambda, GridPager pager)
		{
			Type type = typeof(M_AdminUser).GetProperties().FirstOrDefault(t => t.Name == pager.sort).PropertyType;
			if (type == typeof(string))
				return GetListByPage<string>(whereLambda, pager);
			else if (type == typeof(int))
				return GetListByPage<int>(whereLambda, pager);
			else if (type == typeof(double))
				return GetListByPage<double>(whereLambda, pager);
			else if (type == typeof(decimal))
				return GetListByPage<decimal>(whereLambda, pager);
			else if (type == typeof(DateTime))
				return GetListByPage<DateTime>(whereLambda, pager);
			else if (type == typeof(bool))
				return GetListByPage<bool>(whereLambda, pager);
			else
				return "[]";

		}

		private static string GetListByPage<T>(Expression<Func<M_AdminUser, bool>> whereLambda, GridPager pager)
		{
			string JsonStr = "[]";
			using (DBContext db = new DBContext())
			{
				List<M_AdminUser> list = new List<M_AdminUser>();
				var OrderByLambda = CreateLambda.GetOrderExpression<M_AdminUser, T>(pager.sort);
				int skip = pager.rows * (pager.page - 1);
				if (pager.order != "desc")
					list = db.AdminUserList.Where(whereLambda).OrderBy(OrderByLambda).Skip(skip).Take(pager.rows).ToList();
				else
					list = db.AdminUserList.Where(whereLambda).OrderByDescending(OrderByLambda).Skip(skip).Take(pager.rows).ToList();
				
				JsonSerializerSettings settings = new JsonSerializerSettings();
				settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
				JsonSerializer ser = JsonSerializer.Create(settings);
				using (StringWriter sw = new StringWriter())
				{
					ser.Serialize(sw, list);
					JsonStr = sw.ToString();
				}
			}
			return JsonStr;
		}

		
		/// <summary>
		/// 查询管理员
		/// </summary>
		/// <param name="whereLambda">查询条件lambda表达式</param>
		/// <returns></returns>
		public static M_AdminUser Single(Expression<Func<M_AdminUser, bool>> whereLambda)
		{
			string JsonStr = SingleJson(whereLambda);
			M_AdminUser reObj = JsonConvert.DeserializeObject<M_AdminUser>(JsonStr);
			return reObj; 
		}

		/// <summary>
		/// 查询管理员
		/// </summary>
		/// <param name="whereLambda">查询条件lambda表达式</param>
		/// <returns></returns>
		public static string SingleJson(Expression<Func<M_AdminUser, bool>> whereLambda)
		{
			string JsonStr = "";
			using (DBContext db = new DBContext())
			{
				M_AdminUser reObj = db.AdminUserList.Where(whereLambda).FirstOrDefault();
				JsonSerializerSettings settings = new JsonSerializerSettings();
				settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
				JsonSerializer ser = JsonSerializer.Create(settings);
				using (StringWriter sw = new StringWriter())
				{
					ser.Serialize(sw, reObj);
					JsonStr = sw.ToString();
				}
			}
			return JsonStr; 
		}

		/// <summary>
		/// 查询管理员
		/// </summary>
		/// <param name="ID">ID</param>

		/// <returns></returns>
		public static M_AdminUser Find(int ID)
        {
			string JsonStr = FindJson(ID);
			M_AdminUser reObj = JsonConvert.DeserializeObject<M_AdminUser>(JsonStr);
			return reObj;
		}

		/// <summary>
		/// 查询管理员
		/// </summary>
		/// <param name="ID">ID</param>

		/// <returns></returns>
		public static string FindJson(int ID)
        {
			string JsonStr = "";
			using (DBContext db = new DBContext())
			{
				M_AdminUser reObj =  db.AdminUserList.Find(ID);
				JsonSerializerSettings settings = new JsonSerializerSettings();
				settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
				JsonSerializer ser = JsonSerializer.Create(settings);
				using (StringWriter sw = new StringWriter())
				{
					ser.Serialize(sw, reObj);
					JsonStr = sw.ToString();
				}
			}
			return JsonStr; 
		}

		/// <summary>
		/// 删除管理员
		/// </summary>
		/// <param name="whereLambda">删除条件lambda表达式</param>
		/// <returns></returns>
		public static bool Del(Expression<Func<M_AdminUser, bool>> whereLambda)
		{
			using (DBContext db = new DBContext())
			{
				try
				{
					db.AdminUserList.RemoveRange(db.AdminUserList.Where(whereLambda));
					db.SaveChanges();
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
					Log.SystemWrite("【AdminUser】\r\n" + ex.Message + "\r\n\r\n" + sb.ToString());
					return false;
				}
			}
		}


		/// <summary>
		/// 删除管理员
		/// </summary>
		/// <param name="ID">ID</param>

		/// <returns></returns>
		public static bool Del(int ID)
		{
			M_AdminUser AdminUserObj = new M_AdminUser() { ID = ID };
			return Del(AdminUserObj);
		}


		/// <summary>
		/// 删除管理员
		/// </summary>
		/// <param name="AdminUserObj">管理员实体</param>
		/// <returns></returns>
		public static bool Del(M_AdminUser AdminUserObj)
		{
			using (DBContext db = new DBContext())
			{
				try
				{
					db.AdminUserList.Attach(AdminUserObj);
					db.AdminUserList.Remove(AdminUserObj);
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
					Log.SystemWrite("【AdminUser】\r\n" + ex.Message + "\r\n\r\n" + sb.ToString());
					return false;
				}
			}
		}
	}
}