using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DM.WinForm
{
	public class Table
	{
		/// <summary>
		/// 表名
		/// </summary>
		public string TableName { get; set; }
		/// <summary>
		/// 表说明
		/// </summary>
		public string Explain { get; set; }
		/// <summary>
		/// 相对应的菜单ID
		/// </summary>
		public string MenuId { get; set; }

		/// <summary>
		/// 检索字段名
		/// </summary>
		public string QueryFieldName { get; set; }

		/// <summary>
		/// 列的集合
		/// </summary>
		public List<ColumnItem> Columns { get; set; }

	}
	public class ColumnItem
	{
		/// <summary>
		/// 列的名字
		/// </summary>
		public string ColumnName { get; set; }
		/// <summary>
		/// 字段类型
		/// </summary>
		public string ColumnType { get; set; }
		/// <summary>
		/// 是否是集合
		/// </summary>
		public bool IsMultiple { get; set; }
		/// <summary>
		/// 是否是主键
		/// </summary>
		public bool IsPK { get; set; }
		/// <summary>
		/// 是否为空
		/// </summary>
		public bool IsEmpty { get; set; }
		/// <summary>
		/// 描述
		/// </summary>
		public string Explain { get; set; }

	}
}
