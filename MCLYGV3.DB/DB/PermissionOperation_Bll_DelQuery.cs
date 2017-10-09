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
	/// 角色权限操作表数据库操作类
	/// </summary>
	public partial class B_PermissionOperation
	{
		/// <summary>
		/// 返回条数角色权限操作表
		/// </summary>
		/// <param name="whereLambda">查询条件lambda表达式</param>
		/// <returns></returns>
		public static int GetCount(Expression<Func<M_PermissionOperation, bool>> whereLambda)
        	{
            		int result = 0;
            		using (DBContext db = new DBContext())
            		{
                		result = db.PermissionOperationList.Count(whereLambda);
           		}
           		return result;
       		}
		
		/// <summary>
		/// 查询角色权限操作表
		/// </summary>
		/// <param name="whereLambda">查询条件lambda表达式</param>
		/// <returns></returns>
		public static string GetListJson(Expression<Func<M_PermissionOperation,bool>> whereLambda)
        {
			string JsonStr="[]";
			using (DBContext db = new DBContext())
			{
				List<M_PermissionOperation> list = db.PermissionOperationList.Where(whereLambda).ToList();
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
		/// 查询角色权限操作表
		/// </summary>
		/// <param name="whereLambda">查询条件lambda表达式</param>
		/// <returns></returns>
		public static List<M_PermissionOperation> GetList(Expression<Func<M_PermissionOperation,bool>> whereLambda)
        {
			string JsonStr = GetListJson(whereLambda);
			List<M_PermissionOperation> list = JsonConvert.DeserializeObject<List<M_PermissionOperation>>(JsonStr);
			return list;
		}
		
		/// <summary>
		/// 分页查询角色权限操作表
		/// </summary>
		/// <param name="whereLambda">查询条件lambda表达式</param>
		/// <param name="pager">分页条件</param>
		/// <returns></returns>
		public static List<M_PermissionOperation> GetListByPage(Expression<Func<M_PermissionOperation, bool>> whereLambda, GridPager pager)
		{
			string JsonStr = GetListJsonByPage(whereLambda, pager);
			List<M_PermissionOperation> list = JsonConvert.DeserializeObject<List<M_PermissionOperation>>(JsonStr);
			return list;
		}



		/// <summary>
		/// 分页查询角色权限操作表
		/// </summary>
		/// <param name="whereLambda">查询条件lambda表达式</param>
		/// <param name="pager">分页条件</param>
		/// <returns></returns>
		public static string GetListJsonByPage(Expression<Func<M_PermissionOperation, bool>> whereLambda, GridPager pager)
		{
			Type type = typeof(M_PermissionOperation).GetProperties().FirstOrDefault(t => t.Name == pager.sort).PropertyType;
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

		private static string GetListByPage<T>(Expression<Func<M_PermissionOperation, bool>> whereLambda, GridPager pager)
		{
			string JsonStr = "[]";
			using (DBContext db = new DBContext())
			{
				List<M_PermissionOperation> list = new List<M_PermissionOperation>();
				var OrderByLambda = CreateLambda.GetOrderExpression<M_PermissionOperation, T>(pager.sort);
				int skip = pager.rows * (pager.page - 1);
				if (pager.order != "desc")
					list = db.PermissionOperationList.Where(whereLambda).OrderBy(OrderByLambda).Skip(skip).Take(pager.rows).ToList();
				else
					list = db.PermissionOperationList.Where(whereLambda).OrderByDescending(OrderByLambda).Skip(skip).Take(pager.rows).ToList();
				
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
		/// 查询角色权限操作表
		/// </summary>
		/// <param name="whereLambda">查询条件lambda表达式</param>
		/// <returns></returns>
		public static M_PermissionOperation Single(Expression<Func<M_PermissionOperation, bool>> whereLambda)
		{
			string JsonStr = SingleJson(whereLambda);
			M_PermissionOperation reObj = JsonConvert.DeserializeObject<M_PermissionOperation>(JsonStr);
			return reObj; 
		}

		/// <summary>
		/// 查询角色权限操作表
		/// </summary>
		/// <param name="whereLambda">查询条件lambda表达式</param>
		/// <returns></returns>
		public static string SingleJson(Expression<Func<M_PermissionOperation, bool>> whereLambda)
		{
			string JsonStr = "";
			using (DBContext db = new DBContext())
			{
				M_PermissionOperation reObj = db.PermissionOperationList.Where(whereLambda).FirstOrDefault();
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
		/// 查询角色权限操作表
		/// </summary>
		/// <param name="Ids">ID</param>

		/// <returns></returns>
		public static M_PermissionOperation Find(string Ids)
        {
			string JsonStr = FindJson(Ids);
			M_PermissionOperation reObj = JsonConvert.DeserializeObject<M_PermissionOperation>(JsonStr);
			return reObj;
		}

		/// <summary>
		/// 查询角色权限操作表
		/// </summary>
		/// <param name="Ids">ID</param>

		/// <returns></returns>
		public static string FindJson(string Ids)
        {
			string JsonStr = "";
			using (DBContext db = new DBContext())
			{
				M_PermissionOperation reObj =  db.PermissionOperationList.Find(Ids);
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
		/// 删除角色权限操作表
		/// </summary>
		/// <param name="whereLambda">删除条件lambda表达式</param>
		/// <returns></returns>
		public static bool Del(Expression<Func<M_PermissionOperation, bool>> whereLambda)
		{
			using (DBContext db = new DBContext())
			{
				try
				{
					db.PermissionOperationList.RemoveRange(db.PermissionOperationList.Where(whereLambda));
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
					Log.SystemWrite("【PermissionOperation】\r\n" + ex.Message + "\r\n\r\n" + sb.ToString());
					return false;
				}
			}
		}


		/// <summary>
		/// 删除角色权限操作表
		/// </summary>
		/// <param name="Ids">ID</param>

		/// <returns></returns>
		public static bool Del(string Ids)
		{
			M_PermissionOperation PermissionOperationObj = new M_PermissionOperation() { Ids = Ids };
			return Del(PermissionOperationObj);
		}


		/// <summary>
		/// 删除角色权限操作表
		/// </summary>
		/// <param name="PermissionOperationObj">角色权限操作表实体</param>
		/// <returns></returns>
		public static bool Del(M_PermissionOperation PermissionOperationObj)
		{
			using (DBContext db = new DBContext())
			{
				try
				{
					db.PermissionOperationList.Attach(PermissionOperationObj);
					db.PermissionOperationList.Remove(PermissionOperationObj);
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
					Log.SystemWrite("【PermissionOperation】\r\n" + ex.Message + "\r\n\r\n" + sb.ToString());
					return false;
				}
			}
		}
	}
}