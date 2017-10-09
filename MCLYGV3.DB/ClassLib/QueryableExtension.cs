using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MCLYGV3.DB
{
	public static class QueryableExtension
	{
		public static IQueryable<T> OrderSort<T>(IQueryable<T> Sour, string SortExpression, string Direction)
		{
			string SortDirection = string.Empty;
			if (Direction == "asc")
				SortDirection = "OrderBy";
			else if (Direction == "desc")
				SortDirection = "OrderByDescending";
			ParameterExpression pe = Expression.Parameter(typeof(T), SortExpression);
			PropertyInfo pi = typeof(T).GetProperty(SortExpression);
			Type[] types = new Type[2];
			types[0] = typeof(T);
			types[1] = pi.PropertyType;
			Expression expr = Expression.Call(typeof(Queryable), SortDirection, types, Sour.Expression, Expression.Lambda(Expression.Property(pe, SortExpression), pe));
			IQueryable<T> query = Sour.Provider.CreateQuery<T>(expr);
			return query;
		}
	}
}
