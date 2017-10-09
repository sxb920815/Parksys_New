using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web.Mvc;
using MCLYGV3.DB;
using MCLYGV3.Web.Models;
using Newtonsoft.Json;

namespace MCLYGV3.Web.Controllers
{

    public class LoginController : Controller
    {
        // GET: Login

        public ActionResult AdminLogin()
        {
            return View();
        }



        [HttpPost]
        public ActionResult LoginOnPost()
        {
            //byte[] byts = new byte[Request.InputStream.Length];
            //Request.InputStream.Read(byts, 0, byts.Length);
            //string requestString = Encoding.UTF8.GetString(byts);

            //LoginModel model = JsonConvert.DeserializeObject<LoginModel>(requestString);
            var userName= System.Web.HttpContext.Current.Request.Form["UserName"];
            var password= System.Web.HttpContext.Current.Request.Form["Password"];

            var result = new DataJsonResult();
            var psd = Common.Sha1(password);
            var user = DB.B_AdminUser.Single(x => x.UserName == userName && x.PassWord == psd);
            if (user == null)
            {
                return this.Content(JS.GetReBack("对不起，用户名和密码不正确"));

            }
            else
            {
                Session["AdminUser"] = user.ID;
                return RedirectToAction("Index","Administrator");
            }
        }

		[HttpPost]
        public ActionResult Logout()
        {
            Session.Remove("AdminUser");
            var result = new DataJsonResult();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
    public class JS
    {


        public static string GetReBack(string msg)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<script>");
            sb.Append($"alert('{msg}');");
            sb.Append("window.history.go(-1);");
            sb.Append("</script>");
            return sb.ToString();
        }
        public static string GetReUrl(string msg, string url)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<script>");
            sb.Append($"alert('{msg}');");
            sb.Append($"location.href = '{url}';");
            sb.Append("</script>");
            return sb.ToString();
        }
    }

}