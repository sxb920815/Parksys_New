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
        public readonly DB.BaseDataService<M_BreakRuleAnnal> _bs_BreakRuleAnnal;
        public readonly DB.BaseDataService<DB.M_WaveAnnal> _bs_WaveAnnal;
        public readonly DB.BaseDataService<M_CardAnnal> _bs_CardAnnal;
        public AdministratorController()
        {
            _bs_BreakRuleAnnal = new BaseDataService<M_BreakRuleAnnal>();
            _bs_WaveAnnal = new BaseDataService<DB.M_WaveAnnal>();
            _bs_CardAnnal = new BaseDataService<M_CardAnnal>();
        }

        #region 违章记录
        /// <summary>
        /// 违章记录列表
        /// </summary>
        /// <returns></returns>
        public ActionResult BreakRuleAnnal_List()
        {
            return View(MyUser);
        }
        public string GetBreakRuleAnnalList(GridPager pager, string queryStr = "", DateTime? startTime = null, DateTime? endTime = null)
        {
            int count = 0;
            var isDesc = pager.order == "desc";
            var checkName = string.IsNullOrWhiteSpace(queryStr);
            var checkStartTime = startTime == null;
            var checkEndTime = endTime == null;

            Expression<Func<DB.M_CardAnnal, bool>> expression =
                 l => (checkName || l.License.Contains(queryStr)) &&
                (checkStartTime || l.CreateTime >= startTime) &&
                (checkEndTime || l.CreateTime <= endTime) &&
                l.IsBreak==1;

            var list = _bs_CardAnnal.GetListByPaged(pager.page, pager.rows, out count, expression, isDesc, new OrderModelField { IsDESC = isDesc, propertyName = pager.sort });

            GridRows<DB.M_CardAnnal> grs = new GridRows<DB.M_CardAnnal>();
            grs.rows = list;
            grs.total = count;
            Response.ContentType = "application/json";
            Response.Charset = "UTF-8";
            IsoDateTimeConverter timeFormat = new IsoDateTimeConverter();
            timeFormat.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
            return JsonConvert.SerializeObject(grs, Formatting.Indented, timeFormat);
        }

        [HttpPost]
        public string DelBreakRuleAnnal(int ID)
        {
            JsonMessage result;

            bool bol = _bs_BreakRuleAnnal.DeleteById(ID);

            if (bol)
                result = new JsonMessage() { type = 0, message = "成功", value = "" };
            else
                result = new JsonMessage() { type = -1, message = "失败", value = "" };

            Response.ContentType = "application/json";
            Response.Charset = "UTF-8";
            return JsonConvert.SerializeObject(result);
        }
        #endregion



        #region 出入记录
        /// <summary>
        /// 出入记录列表
        /// </summary>
        /// <returns></returns>
        public ActionResult CardAnnal_List()
        {
            return View(MyUser);
        }
        public string GetCardAnnalList(GridPager pager, string queryStr = "", DateTime? startTime = null, DateTime? endTime = null)
        {
            int count = 0;
            var isDesc = pager.order == "desc";
            var checkName = string.IsNullOrWhiteSpace(queryStr);
            var checkStartTime = startTime == null;
            var checkEndTime = endTime == null;

            Expression<Func<DB.M_CardAnnal, bool>> expression =
                 l => (checkName || l.License.Contains(queryStr)) &&
                (checkStartTime || l.CreateTime >= startTime) &&
                (checkEndTime || l.CreateTime <= endTime);

            var list = _bs_CardAnnal.GetListByPaged(pager.page, pager.rows, out count, expression, isDesc, new OrderModelField { IsDESC = isDesc, propertyName = pager.sort });

            GridRows<DB.M_CardAnnal> grs = new GridRows<DB.M_CardAnnal>();
            grs.rows = list;
            grs.total = count;
            Response.ContentType = "application/json";
            Response.Charset = "UTF-8";
            IsoDateTimeConverter timeFormat = new IsoDateTimeConverter();
            timeFormat.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
            return JsonConvert.SerializeObject(grs, Formatting.Indented, timeFormat);
        }
        [HttpPost]
        public string DelCardAnnal(int ID)
        {
            JsonMessage result;

            bool bol = _bs_CardAnnal.DeleteById(ID);

            if (bol)
                result = new JsonMessage() { type = 0, message = "成功", value = "" };
            else
                result = new JsonMessage() { type = -1, message = "失败", value = "" };

            Response.ContentType = "application/json";
            Response.Charset = "UTF-8";
            return JsonConvert.SerializeObject(result);
        }
        #endregion



        #region 微波记录
        /// <summary>
        /// 微波记录列表
        /// </summary>
        /// <returns></returns>
        public ActionResult WaveAnnal_List()
        {
            return View(MyUser);
        }
        public string GetWaveAnnalList(GridPager pager, string queryStr = "", DateTime? startTime = null, DateTime? endTime = null)
        {
            int count = 0;
            var isDesc = pager.order == "desc";
            var checkName = string.IsNullOrWhiteSpace(queryStr);
            var checkStartTime = startTime == null;
            var checkEndTime = endTime == null;

            Expression<Func<DB.M_WaveAnnal, bool>> expression =
                 l => (checkName || l.WaveCardId.Contains(queryStr)) &&
                (checkStartTime || l.CreateTime >= startTime) &&
                (checkEndTime || l.CreateTime <= endTime);

            var list = _bs_WaveAnnal.GetListByPaged(pager.page, pager.rows, out count, expression, isDesc, new OrderModelField { IsDESC = isDesc, propertyName = pager.sort });

            GridRows<DB.M_WaveAnnal> grs = new GridRows<DB.M_WaveAnnal>();
            grs.rows = list;
            grs.total = count;
            Response.ContentType = "application/json";
            Response.Charset = "UTF-8";
            IsoDateTimeConverter timeFormat = new IsoDateTimeConverter();
            timeFormat.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
            return JsonConvert.SerializeObject(grs, Formatting.Indented, timeFormat);
        }
        [HttpPost]
        public string DelWaveAnnal(int ID)
        {
            JsonMessage result;

            bool bol = _bs_WaveAnnal.DeleteById(ID);

            if (bol)
                result = new JsonMessage() { type = 0, message = "成功", value = "" };
            else
                result = new JsonMessage() { type = -1, message = "失败", value = "" };

            Response.ContentType = "application/json";
            Response.Charset = "UTF-8";
            return JsonConvert.SerializeObject(result);
        }
        #endregion
        public ActionResult ShowImage(int ID)
        {
            var obj = _bs_CardAnnal.GetSingleById(ID);
            return View(obj);
        }




        public ActionResult WaveCard_List(int waveAnnalId = 0)
        {
            ViewBag.WaveAnnalId = waveAnnalId;
            return View(MyUser);
        }
        public string GetWaveCard(GridPager pager, int waveAnnalId = 0)
        {
            int count = 0;
            var isDesc = pager.order == "desc";

            var waveAnnal = _bs_WaveAnnal.GetSingleById(waveAnnalId);
            var beginTime = waveAnnal.CreateTime.AddSeconds(-10);
            var endTime = waveAnnal.CreateTime.AddSeconds(10);

            Expression<Func<DB.M_CardAnnal, bool>> expression =
                 l => l.CreateTime>=beginTime && l.CreateTime<=endTime;

            var list = _bs_CardAnnal.GetListByPaged(pager.page, pager.rows, out count, expression, isDesc, new OrderModelField { IsDESC = isDesc, propertyName = pager.sort });

            GridRows<DB.M_CardAnnal> grs = new GridRows<DB.M_CardAnnal>();
            grs.rows = list;
            grs.total = count;
            Response.ContentType = "application/json";
            Response.Charset = "UTF-8";
            IsoDateTimeConverter timeFormat = new IsoDateTimeConverter();
            timeFormat.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
            return JsonConvert.SerializeObject(grs, Formatting.Indented, timeFormat);
        }

    }
}