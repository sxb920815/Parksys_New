using MCLYGV3.DB;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using MCLYGV3.Web.Models;

namespace MCLYGV3.Web.Controllers
{
    [EnableCors("*", "*", "*")]
    public class SystemController : ApiController
    {

        [HttpPost]
        public HttpResponseMessage GetTopMenu()
        {
            int userid = int.Parse(HttpContext.Current.Request.Form["userid"]);
            string result = "[]";
            M_AdminUser AdminUser = B_AdminUser.Find(userid);
            if (AdminUser != null)
            {
                List<MenuItem> FirstLvList = AdminMenuOperation.GetFirstLvElement();
                List<CMenuItem> ReturnList = FirstLvList.Select(t => new CMenuItem { id = t.id, text = t.text, attributes = t.attributes, iconCls = t.iconCls, state = t.state }).ToList();
                result = JsonConvert.SerializeObject(ReturnList);
            }
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, "value");
            response.Content = new StringContent(result, Encoding.UTF8);
            return response;
        }

        [HttpPost]
        public HttpResponseMessage GetTreeByEasyui(string id)
        {
            string result = "[]";

            List<MenuItem> Childs = AdminMenuOperation.GetChildsById(id);
            if (Childs != null)
            {
                List<CMenuItem> ReturnList = Childs.Select(t => new CMenuItem { id = t.id, text = t.text, attributes = t.attributes, iconCls = t.iconCls, state = t.state }).ToList();
                result = JsonConvert.SerializeObject(ReturnList);
            }
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, "value");
            response.Content = new StringContent(result, Encoding.UTF8);
            return response;
        }


        [HttpGet]
        public DataJsonResult GetDateTimeNow()
        {
            var result=new DataJsonResult();
            result.Data = DateTime.Now;
            return result;
        }


        [HttpPost]
        public HttpResponseMessage GetAreaByParent(string id)
        {
            string result = "";
            List<M_Area> list = Area.GetAreaByParent(id);
            if (list != null && list.Count > 0)
            {
                StringBuilder sb = new StringBuilder();
                foreach (var item in list)
                {
                    sb.Append($"<option value='{item.ID}'>{item.Name}</option>");
                }
                result = sb.ToString();
            }
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, "value");
            response.Content = new StringContent(result, Encoding.UTF8);
            return response;
        }

        [HttpPost]
        public HttpResponseMessage GetAreaNameByID(string id)
        {
            string result = Area.GetAreaNameByID(id);
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, "value");
            response.Content = new StringContent(result, Encoding.UTF8);
            return response;
        }

        [HttpPost]
        public JsonMessage CheckUserPermissions(int AdminUserId, string PermissionOperationId)
        {
            bool bol = B_AdminUser.CheckPermission(AdminUserId, PermissionOperationId);
            JsonMessage result;
            if (bol)
                result = new JsonMessage() { type = 0, message = "鉴权成功", value = "Success" };
            else
                result = new JsonMessage() { type = -1, message = "对不起，您没有权限", value = "Faild" };

            return result;
        }


       
    }

   
}
