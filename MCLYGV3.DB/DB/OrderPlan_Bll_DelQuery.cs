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
	public partial class B_OrderPlan
	{
		/// <summary>
		/// 返回条数订单
		/// </summary>
		/// <param name="whereLambda">查询条件lambda表达式</param>
		/// <returns></returns>
		public static int GetCount(Expression<Func<M_OrderPlan, bool>> whereLambda)
        	{
            		int result = 0;
            		using (DBContext db = new DBContext())
            		{
                		result = db.OrderPlanList.Count(whereLambda);
           		}
           		return result;
       		}
		
		/// <summary>
		/// 查询订单
		/// </summary>
		/// <param name="whereLambda">查询条件lambda表达式</param>
		/// <returns></returns>
		public static string GetListJson(Expression<Func<M_OrderPlan,bool>> whereLambda)
        {
			string JsonStr="[]";
			using (DBContext db = new DBContext())
			{
				List<M_OrderPlan> list = db.OrderPlanList.Where(whereLambda).ToList();
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
		public static List<M_OrderPlan> GetList(Expression<Func<M_OrderPlan,bool>> whereLambda)
        {
			string JsonStr = GetListJson(whereLambda);
			List<M_OrderPlan> list = JsonConvert.DeserializeObject<List<M_OrderPlan>>(JsonStr);
			return list;
		}
		
		/// <summary>
		/// 分页查询订单
		/// </summary>
		/// <param name="whereLambda">查询条件lambda表达式</param>
		/// <param name="pager">分页条件</param>
		/// <returns></returns>
		public static List<M_OrderPlan> GetListByPage(Expression<Func<M_OrderPlan, bool>> whereLambda, GridPager pager)
		{
			string JsonStr = GetListJsonByPage(whereLambda, pager);
			List<M_OrderPlan> list = JsonConvert.DeserializeObject<List<M_OrderPlan>>(JsonStr);
			return list;
		}



		/// <summary>
		/// 分页查询订单
		/// </summary>
		/// <param name="whereLambda">查询条件lambda表达式</param>
		/// <param name="pager">分页条件</param>
		/// <returns></returns>
		public static string GetListJsonByPage(Expression<Func<M_OrderPlan, bool>> whereLambda, GridPager pager)
		{
			Type type = typeof(M_OrderPlan).GetProperties().FirstOrDefault(t => t.Name == pager.sort).PropertyType;
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

		private static string GetListByPage<T>(Expression<Func<M_OrderPlan, bool>> whereLambda, GridPager pager)
		{
			string JsonStr = "[]";
			using (DBContext db = new DBContext())
			{
				List<M_OrderPlan> list = new List<M_OrderPlan>();
				var OrderByLambda = CreateLambda.GetOrderExpression<M_OrderPlan, T>(pager.sort);
				int skip = pager.rows * (pager.page - 1);
				if (pager.order != "desc")
					list = db.OrderPlanList.Where(whereLambda).OrderBy(OrderByLambda).Skip(skip).Take(pager.rows).ToList();
				else
					list = db.OrderPlanList.Where(whereLambda).OrderByDescending(OrderByLambda).Skip(skip).Take(pager.rows).ToList();
				
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
		public static M_OrderPlan Single(Expression<Func<M_OrderPlan, bool>> whereLambda)
		{
			string JsonStr = SingleJson(whereLambda);
			M_OrderPlan reObj = JsonConvert.DeserializeObject<M_OrderPlan>(JsonStr);
			return reObj; 
		}

		/// <summary>
		/// 查询订单
		/// </summary>
		/// <param name="whereLambda">查询条件lambda表达式</param>
		/// <returns></returns>
		public static string SingleJson(Expression<Func<M_OrderPlan, bool>> whereLambda)
		{
			string JsonStr = "";
			using (DBContext db = new DBContext())
			{
				M_OrderPlan reObj = db.OrderPlanList.Where(whereLambda).FirstOrDefault();
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
		public static M_OrderPlan Find(int ID)
        {
			string JsonStr = FindJson(ID);
			M_OrderPlan reObj = JsonConvert.DeserializeObject<M_OrderPlan>(JsonStr);
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
				M_OrderPlan reObj =  db.OrderPlanList.Find(ID);
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
		public static bool Del(Expression<Func<M_OrderPlan, bool>> whereLambda)
		{
			using (DBContext db = new DBContext())
			{
				try
				{
					db.OrderPlanList.RemoveRange(db.OrderPlanList.Where(whereLambda));
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
					Log.SystemWrite("【OrderPlan】\r\n" + ex.Message + "\r\n\r\n" + sb.ToString());
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
			M_OrderPlan OrderPlanObj = new M_OrderPlan() { ID = ID };
			return Del(OrderPlanObj);
		}


		/// <summary>
		/// 删除订单
		/// </summary>
		/// <param name="OrderPlanObj">订单实体</param>
		/// <returns></returns>
		public static bool Del(M_OrderPlan OrderPlanObj)
		{
			using (DBContext db = new DBContext())
			{
				try
				{
					db.OrderPlanList.Attach(OrderPlanObj);
					db.OrderPlanList.Remove(OrderPlanObj);
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
					Log.SystemWrite("【OrderPlan】\r\n" + ex.Message + "\r\n\r\n" + sb.ToString());
					return false;
				}
			}
		}
	}
}