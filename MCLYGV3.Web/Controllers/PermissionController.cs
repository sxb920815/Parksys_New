using MCLYGV3.DB;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;

namespace MCLYGV3.Web.Controllers
{
	public class PermissionController : ApiController
	{

		[HttpGet]
		public HttpResponseMessage BuildPermission()
		{
			AdminMenuOperation.MakePermissionDB();
			HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, "value");
			response.Content = new StringContent("OK", Encoding.UTF8);
			return response;
		}

		[HttpPost]
		public HttpResponseMessage GetModelList()
		{

			string id = HttpContext.Current.Request.Form["id"];
			if (string.IsNullOrEmpty(id))
				id = "0";
			return GetModelList(id);
		}

		[HttpPost]
		public HttpResponseMessage GetModelList(string id)
		{
			List<PermissionRes> list = new List<PermissionRes>();

			var dblist = B_Permission.GetList(t => t.ParentId == id);
			int i = 1;
			foreach (var item in dblist)
			{
				bool HasChild = B_Permission.HasChild(item.ID);
				string Iconic, state;
				if (HasChild)
				{
					Iconic = "fa fa-desktop";
					state = "closed";
				}
				else
				{
					Iconic = "fa  fa-bullhorn";
					state = "open";
				}
				PermissionRes obj = new PermissionRes()
				{
					Id = item.ID,
					Name = item.Name,
					EnglishName = "",
					ParentId = item.ParentId,

					Iconic = Iconic,
					Sort = i,
					IsLast = !B_Permission.HasChild(item.ID),
					Enable = true,
					state = state
				};
				list.Add(obj);
				i++;
			}



			/*
			[{,"IsLast":true,"state":"open","Version":null},{"Id":"20140226141743403806953e2cf8af6","Name":"上传文件例子","EnglishName":"Upload File","ParentId":"201611241127104938174ca14069a09","Url":"SysSampleByUpLoad","Iconic":"fa fa-upload","Sort":0,"Remark":null,"Enable":true,"CreatePerson":"admin","CreateTime":"2014-02-26 14:17:43","IsLast":true,"state":"open","Version":null},{"Id":"2016060200402104175466e25a798cf","Name":"产品类别","EnglishName":"ProductCategory","ParentId":"201611241127104938174ca14069a09","Url":"Spl/ProductCategory","Iconic":"fa  fa-puzzle-piece","Sort":0,"Remark":null,"Enable":true,"CreatePerson":"admin","CreateTime":"2016-06-02 00:40:21","IsLast":true,"state":"open","Version":null}]
			string a = "[,{\,,\,\"IsLast\":false,\"state\":\"closed\",\"Version\":null},{\"Id\":\"20161124112512659817453d009fb84\",\"Name\":\"信息系统\",\"EnglishName\":\"MIS\",\"ParentId\":\"0\",\"Url\":null,\"Iconic\":\"fa fa-puzzle-piece\",\"Sort\":3,\"Remark\":null,\"Enable\":true,\"CreatePerson\":\"admin\",\"CreateTime\":\"2016-11-24 11:25:12\",\"IsLast\":false,\"state\":\"closed\",\"Version\":null},{\"Id\":\"201407241558264790957ebaf9fec63\",\"Name\":\"工作流程\",\"EnglishName\":\"Work Flow\",\"ParentId\":\"0\",\"Url\":\"flow\",\"Iconic\":\"fa fa-sort-amount-asc\",\"Sort\":6,\"Remark\":null,\"Enable\":true,\"CreatePerson\":\"admin\",\"CreateTime\":\"2014-07-24 15:58:26\",\"IsLast\":false,\"state\":\"closed\",\"Version\":null},{\"Id\":\"2016112411022140581745f0f582911\",\"Name\":\"微信系统\",\"EnglishName\":\"WeChat System\",\"ParentId\":\"0\",\"Url\":null,\"Iconic\":\"fa  fa-weixin\",\"Sort\":7,\"Remark\":null,\"Enable\":true,\"CreatePerson\":\"admin\",\"CreateTime\":\"2016-11-24 11:02:21\",\"IsLast\":false,\"state\":\"closed\",\"Version\":null},{\"Id\":\"SystemManage\",\"Name\":\"系统管理\",\"EnglishName\":\"System Management\",\"ParentId\":\"0\",\"Url\":\"sys\",\"Iconic\":\"fa fa-gears\",\"Sort\":99,\"Remark\":null,\"Enable\":true,\"CreatePerson\":\"Administrator\",\"CreateTime\":\"2012-10 - 01 00:00:00\",\"IsLast\":false,\"state\":\"closed\",\"Version\":null},{\"Id\":\"20161124111315488817464f920b54f\",\"Name\":\"权限系统\",\"EnglishName\":\"Authorities\",\"ParentId\":\"0\",\"Url\":null,\"Iconic\":\"fa  fa-shield\",\"Sort\":100,\"Remark\":null,\"Enable\":true,\"CreatePerson\":\"admin\",\"CreateTime\":\"2016-11-24 11:13:15\",\"IsLast\":false,\"state\":\"closed\",\"Version\":null},{\"Id\":\"201611241112087878174673e7fccb2\",\"Name\":\"缺陷系统\",\"EnglishName\":\"DRM System\",\"ParentId\":\"0\",\"Url\":null,\"Iconic\":\"fa  fa - bug\",\"Sort\":101,\"Remark\":\"不再维护\",\"Enable\":true,\"CreatePerson\":\"admin\",\"CreateTime\":\"2016-11-24 11:12:08\",\"IsLast\":false,\"state\":\"closed\",\"Version\":null}]";
			string a = "[,{\"Id\":\"20161124152221600817463b36b059e\",\"Name\":\"控制面板\",\"EnglishName\":\"Control Panel\",\"ParentId\":\"0\",\"Url\":null,\"Iconic\":\"fa  fa-desktop\",\"Sort\":1,\"Remark\":null,\"Enable\":true,\"CreatePerson\":\"admin\",\"CreateTime\":\"2016-11-24 15:22:21\",\"IsLast\":false,\"state\":\"closed\",\"Version\":null},{\"Id\":\"20161124112512659817453d009fb84\",\"Name\":\"信息系统\",\"EnglishName\":\"MIS\",\"ParentId\":\"0\",\"Url\":null,\"Iconic\":\"fa fa-puzzle-piece\",\"Sort\":3,\"Remark\":null,\"Enable\":true,\"CreatePerson\":\"admin\",\"CreateTime\":\"2016-11-24 11:25:12\",\"IsLast\":false,\"state\":\"closed\",\"Version\":null},{\"Id\":\"201407241558264790957ebaf9fec63\",\"Name\":\"工作流程\",\"EnglishName\":\"Work Flow\",\"ParentId\":\"0\",\"Url\":\"flow\",\"Iconic\":\"fa fa-sort-amount-asc\",\"Sort\":6,\"Remark\":null,\"Enable\":true,\"CreatePerson\":\"admin\",\"CreateTime\":\"2014-07-24 15:58:26\",\"IsLast\":false,\"state\":\"closed\",\"Version\":null},{\"Id\":\"2016112411022140581745f0f582911\",\"Name\":\"微信系统\",\"EnglishName\":\"WeChat System\",\"ParentId\":\"0\",\"Url\":null,\"Iconic\":\"fa  fa-weixin\",\"Sort\":7,\"Remark\":null,\"Enable\":true,\"CreatePerson\":\"admin\",\"CreateTime\":\"2016-11-24 11:02:21\",\"IsLast\":false,\"state\":\"closed\",\"Version\":null},{\"Id\":\"SystemManage\",\"Name\":\"系统管理\",\"EnglishName\":\"System Management\",\"ParentId\":\"0\",\"Url\":\"sys\",\"Iconic\":\"fa fa-gears\",\"Sort\":99,\"Remark\":null,\"Enable\":true,\"CreatePerson\":\"Administrator\",\"CreateTime\":\"2012-10 - 01 00:00:00\",\"IsLast\":false,\"state\":\"closed\",\"Version\":null},{\"Id\":\"20161124111315488817464f920b54f\",\"Name\":\"权限系统\",\"EnglishName\":\"Authorities\",\"ParentId\":\"0\",\"Url\":null,\"Iconic\":\"fa  fa-shield\",\"Sort\":100,\"Remark\":null,\"Enable\":true,\"CreatePerson\":\"admin\",\"CreateTime\":\"2016-11-24 11:13:15\",\"IsLast\":false,\"state\":\"closed\",\"Version\":null},{\"Id\":\"201611241112087878174673e7fccb2\",\"Name\":\"缺陷系统\",\"EnglishName\":\"DRM System\",\"ParentId\":\"0\",\"Url\":null,\"Iconic\":\"fa  fa - bug\",\"Sort\":101,\"Remark\":\"不再维护\",\"Enable\":true,\"CreatePerson\":\"admin\",\"CreateTime\":\"2016-11-24 11:12:08\",\"IsLast\":false,\"state\":\"closed\",\"Version\":null}]";
			if (id != "0")
				a = "[{\"Id\":\"201611241127104938174ca14069a09\",\"Name\":\"简单样例\",\"EnglishName\":\"Easy Sample\",\"ParentId\":\"201605312304598866131890ede44b6\",\"Url\":null,\"Iconic\":\"fa  fa-puzzle-piece\",\"Sort\":0,\"Remark\":null,\"Enable\":true,\"CreatePerson\":\"admin\",\"CreateTime\":\"2016-11-24 11:27:10\",\"IsLast\":false,\"state\":\"closed\",\"Version\":null},{\"Id\":\"201611241128515238174870b8252a2\",\"Name\":\"复杂样例\",\"EnglishName\":\"Hard Sample\",\"ParentId\":\"201605312304598866131890ede44b6\",\"Url\":null,\"Iconic\":\"fa  fa-paste\",\"Sort\":0,\"Remark\":null,\"Enable\":true,\"CreatePerson\":\"admin\",\"CreateTime\":\"2016-11-24 11:28:51\",\"IsLast\":false,\"state\":\"closed\",\"Version\":null}]";
				*/

			HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, "value");
			response.Content = new StringContent(JsonConvert.SerializeObject(list), Encoding.UTF8);
			return response;
		}
		[HttpPost]
		public HttpResponseMessage GetRightByRoleAndModule()
		{
			string result = "";
			HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, "value");
			response.Content = new StringContent(result, Encoding.UTF8);
			return response;
		}


		[HttpPost]
		public HttpResponseMessage UpdateRight()
		{
			string Id = HttpContext.Current.Request.Form["Id"];
			int RoleId = int.Parse(HttpContext.Current.Request.Form["RoleId"]);
			string IsValid = HttpContext.Current.Request.Form["IsValid"];
			string result = "OK";
			if (IsValid.ToLower() == "true")
				B_Role.AddPermission(RoleId, Id);
			else
				B_Role.DelPermission(RoleId, Id);

			HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, "value");
			response.Content = new StringContent(result, Encoding.UTF8);
			return response;
		}

		[HttpPost]
		public HttpResponseMessage GetRightByRoleAndModule(int roleId, string moduleId)
		{
			string result = "";

			M_Role role = B_Role.Find(roleId);
			if (role != null)
			{
				var permissionOperationList = B_PermissionOperation.GetList(t => t.RightId == moduleId);
				string permissionName = B_Permission.Find(moduleId).Name;
				List<POperation> plist = new List<POperation>();
				foreach (var item in permissionOperationList)
				{
					POperation po = new POperation();
					po.Ids = item.Ids;
					po.KeyCode = item.KeyCode;
					po.Name = permissionName + "-" + item.Name;
					po.RightId = item.RightId;
					po.IsValid = B_Role.CheckPermission(roleId, item.Ids);
					plist.Add(po);
				}
				GridRows<POperation> grs = new GridRows<POperation>();
				grs.rows = plist;
				grs.total = 0;
				result = JsonConvert.SerializeObject(grs);
			}

			HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, "value");
			response.Content = new StringContent(result, Encoding.UTF8);
			return response;

		}
	}
	public class POperation
	{
		public string Ids { get; set; }
		public string Name { get; set; }
		public string KeyCode { get; set; }
		public bool IsValid { get; set; }
		public string RightId { get; set; }
	}
	public class PermissionRes
	{
		/// <summary>
		/// ID
		/// </summary>
		public string Id { get; set; }
		/// <summary>
		/// 菜单名字
		/// </summary>
		public string Name { get; set; }
		/// <summary>
		/// 英文名字
		/// </summary>
		public string EnglishName { get; set; }
		/// <summary>
		/// 父级ID
		/// </summary>
		public string ParentId { get; set; }
		/// <summary>
		/// URL
		/// </summary>
		public string Url { get; set; }
		/// <summary>
		/// 图标
		/// </summary>
		public string Iconic { get; set; }
		/// <summary>
		/// 排序
		/// </summary>
		public int Sort { get; set; }
		/// <summary>
		/// 标记
		/// </summary>
		public string Remark { get; set; }
		/// <summary>
		/// 可用
		/// </summary>
		public bool Enable { get; set; }
		/// <summary>
		/// 创建人
		/// </summary>
		public string CreatePerson { get; set; }
		/// <summary>
		/// 创建时间
		/// </summary>
		public string CreateTime { get; set; }
		/// <summary>
		/// 是否最后
		/// </summary>
		public bool IsLast { get; set; }
		/// <summary>
		/// 状态
		/// </summary>
		public string state { get; set; }
		/// <summary>
		/// 版本
		/// </summary>
		public string Version { get; set; }


	}

}
