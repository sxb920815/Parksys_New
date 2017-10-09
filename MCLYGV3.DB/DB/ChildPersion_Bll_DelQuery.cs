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
	/// 订单数据库操作类
	/// </summary>
	public partial class B_ChildPersion
	{
		/// <summary>
		/// 返回条数订单
		/// </summary>
		/// <param name="whereLambda">查询条件lambda表达式</param>
		/// <returns></returns>
		public static int GetCount(Expression<Func<M_ChildPersion, bool>> whereLambda)
        	{
            		int result = 0;
            		using (DBContext db = new DBContext())
            		{
                		result = db.ChildPersionList.Count(whereLambda);
           		}
           		return result;
       		}
		
		/// <summary>
		/// 查询订单
		/// </summary>
		/// <param name="whereLambda">查询条件lambda表达式</param>
		/// <returns></returns>
		public static string GetListJson(Expression<Func<M_ChildPersion,bool>> whereLambda)
        {
			string JsonStr="[]";
			using (DBContext db = new DBContext())
			{
				List<M_ChildPersion> list = db.ChildPersionList.Where(whereLambda).ToList();
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
		/// 查询订单
		/// </summary>
		/// <param name="whereLambda">查询条件lambda表达式</param>
		/// <returns></returns>
		public static List<M_ChildPersion> GetList(Expression<Func<M_ChildPersion,bool>> whereLambda)
        {
			string JsonStr = GetListJson(whereLambda);
			List<M_ChildPersion> list = JsonConvert.DeserializeObject<List<M_ChildPersion>>(JsonStr);
			return list;
		}
		
		/// <summary>
		/// 分页查询订单
		/// </summary>
		/// <param name="whereLambda">查询条件lambda表达式</param>
		/// <param name="pager">分页条件</param>
		/// <returns></returns>
		public static List<M_ChildPersion> GetListByPage(Expression<Func<M_ChildPersion, bool>> whereLambda, GridPager pager)
		{
			string JsonStr = GetListJsonByPage(whereLambda, pager);
			List<M_ChildPersion> list = JsonConvert.DeserializeObject<List<M_ChildPersion>>(JsonStr);
			return list;
		}



		/// <summary>
		/// 分页查询订单
		/// </summary>
		/// <param name="whereLambda">查询条件lambda表达式</param>
		/// <param name="pager">分页条件</param>
		/// <returns></returns>
		public static string GetListJsonByPage(Expression<Func<M_ChildPersion, bool>> whereLambda, GridPager pager)
		{
			Type type = typeof(M_ChildPersion).GetProperties().FirstOrDefault(t => t.Name == pager.sort).PropertyType;
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

		private static string GetListByPage<T>(Expression<Func<M_ChildPersion, bool>> whereLambda, GridPager pager)
		{
			string JsonStr = "[]";
			using (DBContext db = new DBContext())
			{
				List<M_ChildPersion> list = new List<M_ChildPersion>();
				var OrderByLambda = CreateLambda.GetOrderExpression<M_ChildPersion, T>(pager.sort);
				int skip = pager.rows * (pager.page - 1);
				if (pager.order != "desc")
					list = db.ChildPersionList.Where(whereLambda).OrderBy(OrderByLambda).Skip(skip).Take(pager.rows).ToList();
				else
					list = db.ChildPersionList.Where(whereLambda).OrderByDescending(OrderByLambda).Skip(skip).Take(pager.rows).ToList();
				
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
		/// 查询订单
		/// </summary>
		/// <param name="whereLambda">查询条件lambda表达式</param>
		/// <returns></returns>
		public static M_ChildPersion Single(Expression<Func<M_ChildPersion, bool>> whereLambda)
		{
			string JsonStr = SingleJson(whereLambda);
			M_ChildPersion reObj = JsonConvert.DeserializeObject<M_ChildPersion>(JsonStr);
			return reObj; 
		}

		/// <summary>
		/// 查询订单
		/// </summary>
		/// <param name="whereLambda">查询条件lambda表达式</param>
		/// <returns></returns>
		public static string SingleJson(Expression<Func<M_ChildPersion, bool>> whereLambda)
		{
			string JsonStr = "";
			using (DBContext db = new DBContext())
			{
				M_ChildPersion reObj = db.ChildPersionList.Where(whereLambda).FirstOrDefault();
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
		/// 查询订单
		/// </summary>
		/// <param name="ID">ID</param>

		/// <returns></returns>
		public static M_ChildPersion Find(int ID)
        {
			string JsonStr = FindJson(ID);
			M_ChildPersion reObj = JsonConvert.DeserializeObject<M_ChildPersion>(JsonStr);
			return reObj;
		}

		/// <summary>
		/// 查询订单
		/// </summary>
		/// <param name="ID">ID</param>

		/// <returns></returns>
		public static string FindJson(int ID)
        {
			string JsonStr = "";
			using (DBContext db = new DBContext())
			{
				M_ChildPersion reObj =  db.ChildPersionList.Find(ID);
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
		/// 删除订单
		/// </summary>
		/// <param name="whereLambda">删除条件lambda表达式</param>
		/// <returns></returns>
		public static bool Del(Expression<Func<M_ChildPersion, bool>> whereLambda)
		{
			using (DBContext db = new DBContext())
			{
				try
				{
					db.ChildPersionList.RemoveRange(db.ChildPersionList.Where(whereLambda));
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
					Log.SystemWrite("【ChildPersion】\r\n" + ex.Message + "\r\n\r\n" + sb.ToString());
					return false;
				}
			}
		}


		/// <summary>
		/// 删除订单
		/// </summary>
		/// <param name="ID">ID</param>

		/// <returns></returns>
		public static bool Del(int ID)
		{
			M_ChildPersion ChildPersionObj = new M_ChildPersion() { ID = ID };
			return Del(ChildPersionObj);
		}


		/// <summary>
		/// 删除订单
		/// </summary>
		/// <param name="ChildPersionObj">订单实体</param>
		/// <returns></returns>
		public static bool Del(M_ChildPersion ChildPersionObj)
		{
			using (DBContext db = new DBContext())
			{
				try
				{
					db.ChildPersionList.Attach(ChildPersionObj);
					db.ChildPersionList.Remove(ChildPersionObj);
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
					Log.SystemWrite("【ChildPersion】\r\n" + ex.Message + "\r\n\r\n" + sb.ToString());
					return false;
				}
			}
		}
	}
}