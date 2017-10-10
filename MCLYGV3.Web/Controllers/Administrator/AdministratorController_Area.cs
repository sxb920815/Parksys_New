using MCLYGV3.DB;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace MCLYGV3.Web.Controllers
{
    public partial class AdministratorController : AdministratorControll
    {
        
        #region 设备表

        public ActionResult Area_List()
        {
            return View(MyUser);
        }
        public ActionResult Area_Add()
        {
            return View();
        }
        public ActionResult Area_Detail(int ID)
        {
            var obj = _bs_Area.GetSingleById(ID);
            return View(obj);
        }
        public ActionResult Area_Edit(int ID)
        {
            var obj = _bs_Area.GetSingleById(ID);
            return View(obj);
        }

        [HttpPost]
        public string DelArea(int ID)
        {
            JsonMessage result;

            bool bol = _bs_Area.DeleteById(ID);

            if (bol)
                result = new JsonMessage() { type = 0, message = "成功", value = "" };
            else
                result = new JsonMessage() { type = -1, message = "失败", value = "" };

            Response.ContentType = "application/json";
            Response.Charset = "UTF-8";
            return JsonConvert.SerializeObject(result);
        }

        [HttpPost]
        public string EditArea()
        {
            JsonMessage result;
            byte[] byts = new byte[Request.InputStream.Length];
            Request.InputStream.Read(byts, 0, byts.Length);
            string req = Encoding.UTF8.GetString(byts);

            DB.M_Area obj = JsonConvert.DeserializeObject<DB.M_Area>(req);
            bool bol = _bs_Area.Update(obj);
            if (bol)
                result = new JsonMessage() { type = 0, message = "成功", value = req };
            else
                result = new JsonMessage() { type = -1, message = "失败", value = req };

            Response.ContentType = "application/json";
            Response.Charset = "UTF-8";
            return JsonConvert.SerializeObject(result);
        }

        [HttpPost]
        public string AddArea()
        {
            JsonMessage result;
            byte[] byts = new byte[Request.InputStream.Length];
            Request.InputStream.Read(byts, 0, byts.Length);
            string req = Encoding.UTF8.GetString(byts);

            DB.M_Area obj = JsonConvert.DeserializeObject<DB.M_Area>(req);
            obj.CreateTime = DateTime.Now;
            _bs_Area.Create(obj);
            result = new JsonMessage() { type = 0, message = "成功", value = req };
            Response.ContentType = "application/json";
            Response.Charset = "UTF-8";
            return JsonConvert.SerializeObject(result);
        }

        public string GetAreaList(GridPager pager, string queryStr)
        {
            int count = 0;
            var isDesc = pager.order == "desc";
            var checkName = string.IsNullOrWhiteSpace(queryStr);

            Expression<Func<DB.M_Area, bool>> expression =
                l => (checkName || l.AreaName.ToString().Contains(queryStr));

            var list = _bs_Area.GetListByPaged(pager.page, pager.rows, out count, expression, isDesc, new OrderModelField { IsDESC= isDesc ,propertyName= pager.sort});

            GridRows<DB.M_Area> grs = new GridRows<DB.M_Area>();
            grs.rows = list;
            grs.total = count;
            Response.ContentType = "application/json";
            Response.Charset = "UTF-8";
            IsoDateTimeConverter timeFormat = new IsoDateTimeConverter();
            timeFormat.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
            return JsonConvert.SerializeObject(grs, Formatting.Indented, timeFormat);
        }

        [HttpGet]
        public string GetAreaName(int areaId)
        {
            var area = _bs_Area.GetSingleById(areaId);
            return area?.AreaName;
        }
        #endregion
    }
}