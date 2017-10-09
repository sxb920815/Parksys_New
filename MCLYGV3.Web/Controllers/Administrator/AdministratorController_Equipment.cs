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
        public readonly DB.BaseDataService<M_Equipment> _bs;
        public AdministratorController()
        {
            _bs = new BaseDataService<M_Equipment>();
        }
        #region 设备表

        public ActionResult Equipment_List()
        {
            return View(MyUser);
        }
        public ActionResult Equipment_Add()
        {
            return View();
        }
        public ActionResult Equipment_Detail(int ID)
        {
            var obj = _bs.GetSingleById(ID);
            return View(obj);
        }
        public ActionResult Equipment_Edit(int ID)
        {
            var obj = _bs.GetSingleById(ID);
            return View(obj);
        }

        [HttpPost]
        public string DelEquipment(int ID)
        {
            JsonMessage result;

            bool bol = _bs.DeleteById(ID);

            if (bol)
                result = new JsonMessage() { type = 0, message = "成功", value = "" };
            else
                result = new JsonMessage() { type = -1, message = "失败", value = "" };

            Response.ContentType = "application/json";
            Response.Charset = "UTF-8";
            return JsonConvert.SerializeObject(result);
        }

        [HttpPost]
        public string EditEquipment()
        {
            JsonMessage result;
            byte[] byts = new byte[Request.InputStream.Length];
            Request.InputStream.Read(byts, 0, byts.Length);
            string req = Encoding.UTF8.GetString(byts);

            M_Equipment obj = JsonConvert.DeserializeObject<M_Equipment>(req);
            bool bol = _bs.Update(obj);
            if (bol)
                result = new JsonMessage() { type = 0, message = "成功", value = req };
            else
                result = new JsonMessage() { type = -1, message = "失败", value = req };

            Response.ContentType = "application/json";
            Response.Charset = "UTF-8";
            return JsonConvert.SerializeObject(result);
        }

        [HttpPost]
        public string AddEquipment()
        {
            JsonMessage result;
            byte[] byts = new byte[Request.InputStream.Length];
            Request.InputStream.Read(byts, 0, byts.Length);
            string req = Encoding.UTF8.GetString(byts);

            M_Equipment obj = JsonConvert.DeserializeObject<M_Equipment>(req);
            _bs.Create(obj);
            result = new JsonMessage() { type = 0, message = "成功", value = req };
            Response.ContentType = "application/json";
            Response.Charset = "UTF-8";
            return JsonConvert.SerializeObject(result);
        }

        public string GetEquipmentList(GridPager pager, string queryStr)
        {
            int count = 0;
            var isDesc = pager.order == "desc";
            var checkName = string.IsNullOrWhiteSpace(queryStr);

            var OrderByLambda = CreateLambda.GetOrderExpression<M_Equipment, bool>(pager.sort);
            Expression<Func<M_Equipment, bool>> expression =
                l => (checkName || l.EquipmentName.ToString().Contains(queryStr));

            var list = _bs.GetListByPaged(pager.page, pager.rows, out count, expression, isDesc, OrderByLambda);

            GridRows<M_Equipment> grs = new GridRows<M_Equipment>();
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