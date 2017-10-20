using MCLYGV3.DB;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text;
using System.Web.Mvc;

namespace MCLYGV3.Web.Controllers
{
    public partial class AdministratorController : AdministratorControll
    {
        // GET: Administrator
        public ActionResult Index()
        {
            return View(MyUser);
        }


        public ActionResult Icon()
        {
            return View();
        }

        public ActionResult Desktop()
        {
            return View(MyUser);
        }

        [HttpPost]
        public JsonResult GetDeaktopData()
        {
            var result = new DesktopViewModel();
            int totalCount = 0;
            var list = _bs_CardAnnal.GetListByPaged(1,8,out totalCount,x=>true,false,new OrderModelField { IsDESC=true,propertyName="CreateTime"});
            result.Data = new {
                TotalCount=totalCount,
                List = list
            };
            return Json(result,JsonRequestBehavior.AllowGet);
        }

        public ActionResult Role_Permission_Edit()
        {
            return View(MyUser);
        }
        public ActionResult Role_List_Ext()
        {
            return View(MyUser);
        }
    }

    public class DesktopViewModel{
        public object Data { get; set; }
    }
}