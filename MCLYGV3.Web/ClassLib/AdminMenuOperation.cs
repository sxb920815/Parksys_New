using MCLYGV3.DB;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace MCLYGV3.Web
{
	public static class AdminMenuOperation
	{
		private static MenuConfig _MenuConfigObj;
		private static Dictionary<string, MenuItem> _MenuConfigDic;
		private static Dictionary<string, string> _MenuPidList;
		static AdminMenuOperation()
		{
			string MappedPath = AppDomain.CurrentDomain.BaseDirectory;
			string configxml = File.ReadAllText(MappedPath + "\\PageBrace\\BackFrame\\Menu.xml");
			_MenuConfigObj = (MenuConfig)XmlUtil.Deserialize(typeof(MenuConfig), configxml);

			_MenuConfigDic = new Dictionary<string, MenuItem>();
			_MenuPidList = new Dictionary<string, string>();
			foreach (MenuItem item in _MenuConfigObj.ItemList)
			{
				AddObjToMenuConfigDic(item, "0");
			}

		}
		public static MenuItem GetItemById(string id)
		{
			if (_MenuConfigDic.ContainsKey(id))
				return _MenuConfigDic[id];
			else
				return null;
		}
		public static void MakePermissionDB()
		{
			B_Permission.Del(t => true);
			B_PermissionOperation.Del(t => true);
			

			foreach (var item in _MenuConfigDic.Values)
			{
				string pid = _MenuPidList[item.id];
				B_Permission.Add(item.id, item.text, pid, item.text, "#", "system", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
				
				
			}

			var list = B_Permission.GetList(t => true);
			foreach (var item in list)
			{
				if (B_Permission.HasChild(item.ID))
				{
					B_PermissionOperation.Add(item.ID + "_Show", "显示", "Show", item.ID);
				}
				else
				{
					B_PermissionOperation.Add(item.ID + "_Add", "添加", "Add", item.ID);
					B_PermissionOperation.Add(item.ID + "_Details", "明细", "Details", item.ID);
					B_PermissionOperation.Add(item.ID + "_Edit", "编辑", "Edit", item.ID);
					B_PermissionOperation.Add(item.ID + "_Del", "删除", "Del", item.ID);
				}
			}




		}
		public static List<MenuItem> GetChildsById(string id)
		{
			MenuItem parent = GetItemById(id);
			if (parent == null)
				return null;

			return parent.ItemList;
		}
		public static List<MenuItem> GetFirstLvElement()
		{
			return _MenuConfigObj.ItemList;
		}
		private static void AddObjToMenuConfigDic(MenuItem item, string pid)
		{
			if (item == null)
				return;
			if (_MenuConfigDic.ContainsKey(item.id) == false)
			{
				_MenuConfigDic.Add(item.id, item);
				_MenuPidList.Add(item.id, pid);
			}


			if (item.ItemList == null || item.ItemList.Count == 0)
				return;

			foreach (var itemsub in item.ItemList)
			{
				AddObjToMenuConfigDic(itemsub, item.id);
			}
		}

	}
	public class MenuConfig
	{
		public List<MenuItem> ItemList { get; set; }
	}
	public class MenuItem : CMenuItem
	{
		/// <summary>
		/// 字菜单列表
		/// </summary>
		public List<MenuItem> ItemList { get; set; }
	}
	public class CMenuItem
	{
		/// <summary>
		/// 标识
		/// </summary>
		public string id { get; set; }
		/// <summary>
		/// 菜单名称
		/// </summary>
		public string text { get; set; }

		/// <summary>
		/// 图标
		/// </summary>
		public string iconCls { get; set; }

		/// <summary>
		/// 连接地址
		/// </summary>
		public string attributes { get; set; }
		/// <summary>
		/// 状态
		/// </summary>
		public string state { get; set; }

	}

}