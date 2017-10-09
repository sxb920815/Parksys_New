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
            return View();
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
}