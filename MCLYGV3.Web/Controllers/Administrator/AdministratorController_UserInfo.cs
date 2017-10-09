using MCLYGV3.DB;
using MCLYGV3.Web.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace MCLYGV3.Web.Controllers
{
    public partial class AdministratorController : AdministratorControll
    {

        #region 用户表

        public ActionResult UserInfo_List()
        {
            return View(MyUser);
        }
        public ActionResult UserInfo_Add()
        {

            List<SelectListItem> InCompanySelect = new List<SelectListItem>();

            var CompanyList = B_Company.GetList(t => true);
            foreach (var item in CompanyList)
            {
                SelectListItem li = new SelectListItem() { Text = item.CompanyName, Value = item.ID.ToString() };
                InCompanySelect.Add(li);
            }
            ViewData["InCompanySelect"] = new SelectList(InCompanySelect, "Value", "Text", "0");
            return View();
        }
        public ActionResult UserInfo_Detail(int ID)
        {
            M_UserInfo obj = B_UserInfo.Find(ID);
            return View(obj);
        }
        public ActionResult UserInfo_Edit(int ID)
        {
            List<SelectListItem> InCompanySelect = new List<SelectListItem>();

            var CompanyList = B_Company.GetList(t => true);
            foreach (var item in CompanyList)
            {
                SelectListItem li = new SelectListItem() { Text = item.CompanyName, Value = item.ID.ToString() };
                InCompanySelect.Add(li);
            }

            M_UserInfo obj = B_UserInfo.Find(ID);
            ViewData["InCompanySelect"] = new SelectList(InCompanySelect, "Value", "Text", InCompanySelect.Find(t => t.Value == obj.InCompany.ID.ToString()).Value);
            return View(obj);
        }

        [HttpPost]
        public string DelUserInfo(int ID)
        {
            JsonMessage result;

            bool bol = B_UserInfo.Del(ID);

            if (bol)
                result = new JsonMessage() { type = 0, message = "成功", value = "" };
            else
                result = new JsonMessage() { type = -1, message = "失败", value = "" };

            Response.ContentType = "application/json";
            Response.Charset = "UTF-8";
            return JsonConvert.SerializeObject(result);
        }

        [HttpPost]
        public string EditUserInfo()
        {
            JsonMessage result;
            byte[] byts = new byte[Request.InputStream.Length];
            Request.InputStream.Read(byts, 0, byts.Length);
            string req = Encoding.UTF8.GetString(byts);
            ReqUserInfo ReqObj = JsonConvert.DeserializeObject<ReqUserInfo>(req);
            var oldObj = B_UserInfo.Find(ReqObj.ID);
            if (B_UserInfo.GetCount(t => t.UserName == ReqObj.UserName) > 0 && ReqObj.UserName != oldObj.UserName)
            {
                result = new JsonMessage() { type = 2, message = "用户名重复", value = req };
                Response.ContentType = "application/json";
                Response.Charset = "UTF-8";
                return JsonConvert.SerializeObject(result);
            }
            // oldObj.UserName = ReqObj.UserName;
            oldObj.Tel = ReqObj.Tel;
            oldObj.RealName = ReqObj.RealName;
            oldObj.Email = ReqObj.Email;
            M_Company Company = B_Company.Find(int.Parse(ReqObj.InCompanySelect));
            if (Company == null)
            {
                result = new JsonMessage() { type = 1, message = "公司不存在", value = req };
                Response.ContentType = "application/json";
                Response.Charset = "UTF-8";
                return JsonConvert.SerializeObject(result);
            }
            oldObj.InCompany = Company;
            bool bol = B_UserInfo.Update(oldObj);
            if (bol)
                result = new JsonMessage() { type = 0, message = "成功", value = req };
            else
                result = new JsonMessage() { type = -1, message = "失败", value = req };

            Response.ContentType = "application/json";
            Response.Charset = "UTF-8";
            return JsonConvert.SerializeObject(result);
        }

        [HttpPost]
        public string AddUserInfo()
        {
            JsonMessage result;
            byte[] byts = new byte[Request.InputStream.Length];
            Request.InputStream.Read(byts, 0, byts.Length);
            string req = Encoding.UTF8.GetString(byts);

            ReqUserInfo ReqObj = JsonConvert.DeserializeObject<ReqUserInfo>(req);


            if (B_UserInfo.GetCount(t => t.UserName == ReqObj.UserName) > 0)
            {
                result = new JsonMessage() { type = 2, message = "用户名重复", value = req };
                Response.ContentType = "application/json";
                Response.Charset = "UTF-8";
                return JsonConvert.SerializeObject(result);
            }

            M_Company Company = B_Company.Find(int.Parse(ReqObj.InCompanySelect));
            if (Company == null)
            {
                result = new JsonMessage() { type = 1, message = "公司不存在", value = req };
                Response.ContentType = "application/json";
                Response.Charset = "UTF-8";
                return JsonConvert.SerializeObject(result);
            }
            var maxId = B_UserInfo.GetMaxId();
            M_UserInfo obj = new M_UserInfo()
            {
                UserName = ReqObj.UserName,
                RealName = ReqObj.RealName,
                Email = ReqObj.Email,
                Tel = ReqObj.Tel,
                PassWord = Common.Md5Password(ReqObj.PassWord),
                InCompany = Company
            };
            if (maxId < 100000)
            {
                obj.ID = 100000;
            }
            else
            {
                obj.ID = maxId + 1;
            }
            obj = B_UserInfo.Add(obj);
            result = new JsonMessage() { type = 0, message = "成功", value = req };

            Response.ContentType = "application/json";
            Response.Charset = "UTF-8";
            return JsonConvert.SerializeObject(result);
        }

        public string GetUserInfoList(GridPager pager, string queryStr)
        {
            List<M_UserInfo> list;
            int count;
            if (string.IsNullOrEmpty(queryStr))
            {
                list = B_UserInfo.GetListByPage(t => 1 == 1, pager);
                count = B_UserInfo.GetCount(t => 1 == 1);
            }
            else
            {
                list = B_UserInfo.GetListByPage(t => t.UserName.Contains(queryStr), pager);
                count = B_UserInfo.GetCount(t => t.UserName.Contains(queryStr));
            }
            GridRows<M_UserInfo> grs = new GridRows<M_UserInfo>();
            grs.rows = list;
            grs.total = count;
            Response.ContentType = "application/json";
            Response.Charset = "UTF-8";
            IsoDateTimeConverter timeFormat = new IsoDateTimeConverter();
            timeFormat.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
            return JsonConvert.SerializeObject(grs, Formatting.Indented, timeFormat);
        }

        #endregion
        public string PswReset(int ID)
        {
            JsonMessage result;
            M_UserInfo User = B_UserInfo.Find(ID);
            User.PassWord = Common.Md5Password("123456");
            if (B_UserInfo.Update(User))
                result = new JsonMessage() { type = 0, message = "成功", value = "" };
            else
                result = new JsonMessage() { type = -1, message = "失败", value = "" };
            Response.ContentType = "application/json";
            Response.Charset = "UTF-8";
            return JsonConvert.SerializeObject(result);
        }

        //    [HttpPost]
        //    public string ClientPswReset()
        //    {
        //        byte[] byts = new byte[Request.InputStream.Length];
        //        Request.InputStream.Read(byts, 0, byts.Length);

        ////string requestString = new StreamReader(HttpContext.Current.Request.InputStream).ReadToEnd();

        //        string req = Encoding.UTF8.GetString(byts);
        //        Log.SystemWriteDebug("修改密码======"+ req);
        //        var ReqObj = JsonConvert.DeserializeObject<ChangePasswordModel>(req);

        //        var result=new DataJsonResult();
        //        M_UserInfo User = B_UserInfo.Find(ReqObj.Id);
        //        if (User == null)
        //        {
        //            result.ReturnCode = "501";
        //            result.ErrorMessage = "账号不存在";
        //            return JsonConvert.SerializeObject(result);
        //        }
        //        Response.ContentType = "application/json";
        //        Response.Charset = "UTF-8";
        //        if (User.PassWord != Common.Md5Password(ReqObj.OldPassword))
        //        {
        //            result.ReturnCode = "502";
        //            result.ErrorMessage = "密码不正确";
        //            return JsonConvert.SerializeObject(result);
        //        }
        //        else
        //        {
        //            if (ReqObj.OldPassword == ReqObj.NewPassword)
        //            {
        //                result.ReturnCode = "503";
        //                result.ErrorMessage = "修改密码不允许与原始密码相同";
        //                return JsonConvert.SerializeObject(result);
        //            }
        //            else
        //            {
        //                User.PassWord = Common.Md5Password(ReqObj.NewPassword);
        //                B_UserInfo.Update(User);
        //                result.ErrorMessage = null;
        //                result.Data = new
        //                {
        //                    User.ID,
        //                    User.UserName,
        //                    User.Tel,
        //                    User.Email,
        //                    User.InCompany?.CompanyName
        //                };
        //                return JsonConvert.SerializeObject(result);
        //            }
        //        }
        //    }
    }

    public class ReqUserInfo : M_UserInfo
    {
        public string InCompanySelect { get; set; }

    }

    public class ChangePasswordModel
    {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }


}
