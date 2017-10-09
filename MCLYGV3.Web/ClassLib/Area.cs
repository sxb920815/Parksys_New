using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;

namespace MCLYGV3.Web
{

	public static class Area
	{
		private static Dictionary<string, M_Area> AreaDic;
		static Area()
		{
			AreaDic = new Dictionary<string, M_Area>();
			string FileName = HostingEnvironment.MapPath("\\") + "\\Config\\Area.txt";
			StreamReader sr = new StreamReader(FileName);
			string line = string.Empty;
			while ((line = sr.ReadLine()) != null)
			{
				string[] arr = line.Split(',');
				M_Area obj = new M_Area() { ID = arr[0], Name = arr[1] };
				AreaDic.Add(arr[0], obj);
				if (AreaDic.ContainsKey(arr[2]))
					AreaDic[arr[2]].ChildsList.Add(obj);

			}
		}
		public static string GetAreaNameByID(string id)
		{
			if (AreaDic.ContainsKey(id))
			{
				M_Area obj = AreaDic[id];
				return obj.Name;
			}
			return "";

		}

		public static List<M_Area> GetProvince()
		{
			return GetAreaByParent("100000");
		}

		public static List<M_Area> GetAreaByParent(string pid)
		{
			List<M_Area> list = null;
			M_Area pobj = AreaDic[pid];
			if (pobj != null)
				list = pobj.ChildsList.ToList();
			return list;
		}

	}

	public class M_Area
	{
		public string ID { get; set; }
		public string Name { get; set; }
		public virtual ICollection<M_Area> ChildsList { get; set; }
		public M_Area()
		{
			ChildsList = new HashSet<M_Area>();
		}
	}
}