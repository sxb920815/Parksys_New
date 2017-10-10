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
        
        #region 内部车辆表

        public ActionResult Car_List()
        {

            return View(MyUser);
        }
        public ActionResult Car_Add()
        {
            List<SelectListItem> AreaSelect = new List<SelectListItem>();
            var AreaList = _bs_Area.GetList(x=>true);
            foreach (var item in AreaList)
            {
                SelectListItem li = new SelectListItem() { Text = item.AreaName, Value = item.AreaId.ToString() };
                AreaSelect.Add(li);
            }
            ViewData["AreaId"] = new SelectList(AreaSelect, "Value", "Text", "0");

            return View();
        }
        public ActionResult Car_Detail(int ID)
        {
            var obj = _bs_Car.GetSingleById(ID);
            return View(obj);
        }
        public ActionResult Car_Edit(int ID)
        {
            List<SelectListItem> AreaSelect = new List<SelectListItem>();
            var AreaList = _bs_Area.GetList(x => true);
            foreach (var item in AreaList)
            {
                SelectListItem li = new SelectListItem() { Text = item.AreaName, Value = item.AreaId.ToString() };
                AreaSelect.Add(li);
            }
            var obj = _bs_Car.GetSingleById(ID);

            ViewData["AreaId"] = new SelectList(AreaSelect, "Value", "Text",AreaSelect.Find(t => t.Value == obj.AreaId.ToString()).Value);

            return View(obj);
        }

        [HttpPost]
        public string DelCar(int ID)
        {
            JsonMessage result;

            bool bol = _bs_Car.DeleteById(ID);

            if (bol)
                result = new JsonMessage() { type = 0, message = "成功", value = "" };
            else
                result = new JsonMessage() { type = -1, message = "失败", value = "" };

            Response.ContentType = "application/json";
            Response.Charset = "UTF-8";
            return JsonConvert.SerializeObject(result);
        }

        [HttpPost]
        public string EditCar()
        {
            JsonMessage result;
            byte[] byts = new byte[Request.InputStream.Length];
            Request.InputStream.Read(byts, 0, byts.Length);
            string req = Encoding.UTF8.GetString(byts);

            DB.M_Car obj = JsonConvert.DeserializeObject<DB.M_Car>(req);
            bool bol = _bs_Car.Update(obj);
            if (bol)
                result = new JsonMessage() { type = 0, message = "成功", value = req };
            else
                result = new JsonMessage() { type = -1, message = "失败", value = req };

            Response.ContentType = "application/json";
            Response.Charset = "UTF-8";
            return JsonConvert.SerializeObject(result);
        }

        [HttpPost]
        public string AddCar()
        {
            JsonMessage result;
            byte[] byts = new byte[Request.InputStream.Length];
            Request.InputStream.Read(byts, 0, byts.Length);
            string req = Encoding.UTF8.GetString(byts);

            DB.M_Car obj = JsonConvert.DeserializeObject<DB.M_Car>(req);
            obj.CreateTime = DateTime.Now;
            _bs_Car.Create(obj);
            result = new JsonMessage() { type = 0, message = "成功", value = req };
            Response.ContentType = "application/json";
            Response.Charset = "UTF-8";
            return JsonConvert.SerializeObject(result);
        }

        public string GetCarList(GridPager pager, string queryStr)
        {
            int count = 0;
            var isDesc = pager.order == "desc";
            var checkName = string.IsNullOrWhiteSpace(queryStr);

            Expression<Func<DB.M_Car, bool>> expression =
                l => (checkName || (l.License.ToString().Contains(queryStr) ||
                l.OwnerName.ToString().Contains(queryStr) ||
                l.OwnerPhone.ToString().Contains(queryStr)));

            var list = _bs_Car.GetListByPaged(pager.page, pager.rows, out count, expression, isDesc, new OrderModelField { IsDESC= isDesc ,propertyName= pager.sort});

            GridRows<DB.M_Car> grs = new GridRows<DB.M_Car>();
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