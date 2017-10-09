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
	public partial class B_OrderChild
	{
		/// <summary>
		/// 返回条数订单
		/// </summary>
		/// <param name="whereLambda">查询条件lambda表达式</param>
		/// <returns></returns>
		public static int GetCount(Expression<Func<M_OrderChild, bool>> whereLambda)
        	{
            		int result = 0;
            		using (DBContext db = new DBContext())
            		{
                		result = db.OrderChildList.Count(whereLambda);
           		}
           		return result;
       		}
		
		/// <summary>
		/// 查询订单
		/// </summary>
		/// <param name="whereLambda">查询条件lambda表达式</param>
		/// <returns></returns>
		public static string GetListJson(Expression<Func<M_OrderChild,bool>> whereLambda)
        {
			string JsonStr="[]";
			using (DBContext db = new DBContext())
			{
				List<M_OrderChild> list = db.OrderChildList.Where(whereLambda).ToList();
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
		public static List<M_OrderChild> GetList(Expression<Func<M_OrderChild,bool>> whereLambda)
        {
			string JsonStr = GetListJson(whereLambda);
			List<M_OrderChild> list = JsonConvert.DeserializeObject<List<M_OrderChild>>(JsonStr);
			return list;
		}
		
		/// <summary>
		/// 分页查询订单
		/// </summary>
		/// <param name="whereLambda">查询条件lambda表达式</param>
		/// <param name="pager">分页条件</param>
		/// <returns></returns>
		public static List<M_OrderChild> GetListByPage(Expression<Func<M_OrderChild, bool>> whereLambda, GridPager pager)
		{
			string JsonStr = GetListJsonByPage(whereLambda, pager);
			List<M_OrderChild> list = JsonConvert.DeserializeObject<List<M_OrderChild>>(JsonStr);
			return list;
		}



		/// <summary>
		/// 分页查询订单
		/// </summary>
		/// <param name="whereLambda">查询条件lambda表达式</param>
		/// <param name="pager">分页条件</param>
		/// <returns></returns>
		public static string GetListJsonByPage(Expression<Func<M_OrderChild, bool>> whereLambda, GridPager pager)
		{
			Type type = typeof(M_OrderChild).GetProperties().FirstOrDefault(t => t.Name == pager.sort).PropertyType;
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

		private static string GetListByPage<T>(Expression<Func<M_OrderChild, bool>> whereLambda, GridPager pager)
		{
			string JsonStr = "[]";
			using (DBContext db = new DBContext())
			{
				List<M_OrderChild> list = new List<M_OrderChild>();
				var OrderByLambda = CreateLambda.GetOrderExpression<M_OrderChild, T>(pager.sort);
				int skip = pager.rows * (pager.page - 1);
				if (pager.order != "desc")
					list = db.OrderChildList.Where(whereLambda).OrderBy(OrderByLambda).Skip(skip).Take(pager.rows).ToList();
				else
					list = db.OrderChildList.Where(whereLambda).OrderByDescending(OrderByLambda).Skip(skip).Take(pager.rows).ToList();
				
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
		public static M_OrderChild Single(Expression<Func<M_OrderChild, bool>> whereLambda)
		{
			string JsonStr = SingleJson(whereLambda);
			M_OrderChild reObj = JsonConvert.DeserializeObject<M_OrderChild>(JsonStr);
			return reObj; 
		}

		/// <summary>
		/// 查询订单
		/// </summary>
		/// <param name="whereLambda">查询条件lambda表达式</param>
		/// <returns></returns>
		public static string SingleJson(Expression<Func<M_OrderChild, bool>> whereLambda)
		{
			string JsonStr = "";
			using (DBContext db = new DBContext())
			{
				M_OrderChild reObj = db.OrderChildList.Where(whereLambda).FirstOrDefault();
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
		/// <param name="ChildCode">子订单编号</param>

		/// <returns></returns>
		public static M_OrderChild Find(string ChildCode)
        {
			string JsonStr = FindJson(ChildCode);
			M_OrderChild reObj = JsonConvert.DeserializeObject<M_OrderChild>(JsonStr);
			return reObj;
		}

		/// <summary>
		/// 查询订单
		/// </summary>
		/// <param name="ChildCode">子订单编号</param>

		/// <returns></returns>
		public static string FindJson(string ChildCode)
        {
			string JsonStr = "";
			using (DBContext db = new DBContext())
			{
				M_OrderChild reObj =  db.OrderChildList.Find(ChildCode);
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
		public static bool Del(Expression<Func<M_OrderChild, bool>> whereLambda)
		{
			using (DBContext db = new DBContext())
			{
				try
				{
					db.OrderChildList.RemoveRange(db.OrderChildList.Where(whereLambda));
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
					Log.SystemWrite("【OrderChild】\r\n" + ex.Message + "\r\n\r\n" + sb.ToString());
					return false;
				}
			}
		}


		/// <summary>
		/// 删除订单
		/// </summary>
		/// <param name="ChildCode">子订单编号</param>

		/// <returns></returns>
		public static bool Del(string ChildCode)
		{
			M_OrderChild OrderChildObj = new M_OrderChild() { ChildCode = ChildCode };
			return Del(OrderChildObj);
		}


		/// <summary>
		/// 删除订单
		/// </summary>
		/// <param name="OrderChildObj">订单实体</param>
		/// <returns></returns>
		public static bool Del(M_OrderChild OrderChildObj)
		{
			using (DBContext db = new DBContext())
			{
				try
				{
					db.OrderChildList.Attach(OrderChildObj);
					db.OrderChildList.Remove(OrderChildObj);
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
					Log.SystemWrite("【OrderChild】\r\n" + ex.Message + "\r\n\r\n" + sb.ToString());
					return false;
				}
			}
		}
	}
}