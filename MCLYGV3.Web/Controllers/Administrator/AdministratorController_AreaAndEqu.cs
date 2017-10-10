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
        
        #region 区域设备关联表

        public ActionResult AreaAndEqu_List()
        {

            return View(MyUser);
        }
        public ActionResult AreaAndEqu_Add()
        {
            List<SelectListItem> AreaSelect = new List<SelectListItem>();
            var AreaList = _bs_Area.GetList(x=>true);
            foreach (var item in AreaList)
            {
                SelectListItem li = new SelectListItem() { Text = item.AreaName, Value = item.AreaId.ToString() };
                AreaSelect.Add(li);
            }
            ViewData["AreaId"] = new SelectList(AreaSelect, "Value", "Text", "0");

            List<SelectListItem> EquipmentSelect = new List<SelectListItem>();
            var EquipmentList = _bs_equ.GetList(x => true);
            foreach (var item in EquipmentList)
            {
                SelectListItem li = new SelectListItem() { Text = item.EquipmentName, Value = item.EquipmentId.ToString() };
                EquipmentSelect.Add(li);
            }
            ViewData["EquipmentId"] = new SelectList(EquipmentSelect, "Value", "Text", "0");

            return View();
        }
        public ActionResult AreaAndEqu_Detail(int ID)
        {
            var obj = _bs_AreaAndEqu.GetSingleById(ID);
            return View(obj);
        }
        public ActionResult AreaAndEqu_Edit(int ID)
        {
            List<SelectListItem> AreaSelect = new List<SelectListItem>();
            var AreaList = _bs_Area.GetList(x => true);
            foreach (var item in AreaList)
            {
                SelectListItem li = new SelectListItem() { Text = item.AreaName, Value = item.AreaId.ToString() };
                AreaSelect.Add(li);
            }
            var obj = _bs_AreaAndEqu.GetSingleById(ID);
            ViewData["AreaId"] = new SelectList(AreaSelect, "Value", "Text",AreaSelect.Find(t => t.Value == obj.AreaId.ToString()).Value);

            List<SelectListItem> EquipmentSelect = new List<SelectListItem>();
            var EquipmentList = _bs_equ.GetList(x => true);
            foreach (var item in EquipmentList)
            {
                SelectListItem li = new SelectListItem() { Text = item.EquipmentName, Value = item.EquipmentId.ToString() };
                EquipmentSelect.Add(li);
            }
            ViewData["EquipmentId"] = new SelectList(EquipmentSelect, "Value", "Text", EquipmentSelect.Find(t => t.Value == obj.EquipmentId.ToString()).Value);

            return View(obj);
        }

        [HttpPost]
        public string DelAreaAndEqu(int ID)
        {
            JsonMessage result;

            bool bol = _bs_AreaAndEqu.DeleteById(ID);

            if (bol)
                result = new JsonMessage() { type = 0, message = "成功", value = "" };
            else
                result = new JsonMessage() { type = -1, message = "失败", value = "" };

            Response.ContentType = "application/json";
            Response.Charset = "UTF-8";
            return JsonConvert.SerializeObject(result);
        }

        [HttpPost]
        public string EditAreaAndEqu()
        {
            JsonMessage result;
            byte[] byts = new byte[Request.InputStream.Length];
            Request.InputStream.Read(byts, 0, byts.Length);
            string req = Encoding.UTF8.GetString(byts);

            DB.M_AreaAndEqu obj = JsonConvert.DeserializeObject<DB.M_AreaAndEqu>(req);
            bool bol = _bs_AreaAndEqu.Update(obj);
            if (bol)
                result = new JsonMessage() { type = 0, message = "成功", value = req };
            else
                result = new JsonMessage() { type = -1, message = "失败", value = req };

            Response.ContentType = "application/json";
            Response.Charset = "UTF-8";
            return JsonConvert.SerializeObject(result);
        }

        [HttpPost]
        public string AddAreaAndEqu()
        {
            JsonMessage result;
            byte[] byts = new byte[Request.InputStream.Length];
            Request.InputStream.Read(byts, 0, byts.Length);
            string req = Encoding.UTF8.GetString(byts);

            DB.M_AreaAndEqu obj = JsonConvert.DeserializeObject<DB.M_AreaAndEqu>(req);
            _bs_AreaAndEqu.Create(obj);
            result = new JsonMessage() { type = 0, message = "成功", value = req };
            Response.ContentType = "application/json";
            Response.Charset = "UTF-8";
            return JsonConvert.SerializeObject(result);
        }

        public string GetAreaAndEquList(GridPager pager, string queryStr)
        {
            int count = 0;
            var isDesc = pager.order == "desc";
            var checkName = string.IsNullOrWhiteSpace(queryStr);

            Expression<Func<DB.M_AreaAndEqu, bool>> expression =
                l =>true;

            var list = _bs_AreaAndEqu.GetListByPaged(pager.page, pager.rows, out count, expression, isDesc, new OrderModelField { IsDESC= isDesc ,propertyName= pager.sort});

            GridRows<DB.M_AreaAndEqu> grs = new GridRows<DB.M_AreaAndEqu>();
            grs.rows = list;
            grs.total = count;
            Response.ContentType = "application/json";
            Response.Charset = "UTF-8";
            IsoDateTimeConverter timeFormat = new IsoDateTimeConverter();
            timeFormat.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
            return JsonConvert.SerializeObject(grs, Formatting.Indented, timeFormat);
        }

        #endregion
    }
}