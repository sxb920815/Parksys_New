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
using MCLYGV3.DB;
using MCLYGV3.Web.Models;
using Newtonsoft.Json;

namespace MCLYGV3.Web.Controllers
{
    [EnableCors("*", "*", "*")]
    public class ApiLoginController : BaseApiController
    {
        [HttpPost]
        public DataJsonResult LoginOnPost()
        {
            var result = new DataJsonResult();
            string requestString = new StreamReader(HttpContext.Current.Request.InputStream).ReadToEnd();
            Log.SystemWriteDebug(requestString);
            var model = requestString.ToObject<LoginModel>();

            var psd = Common.Md5Password(model.Password);
            var user = DB.B_UserInfo.Single(x => x.UserName == model.UserName && x.PassWord == psd);
            if (user == null)
            {
                result.ReturnCode = "601";
                result.ErrorMessage = "用户名或密码错误";
                Log.SystemWriteDebug(result.ToJson());
            }
            else
            {
                Log.SystemWriteDebug($"{user.UserName}登录");
                result.Data = new
                {
                    UserId = EnUserId(user.ID),
                    user.UserName,
                    user.RealName,
                    user.Tel,
                    user.Email,
                    InCompany = user.InCompany?.CompanyName
                };
            }
            return result;
        }
        private static string EnUserId(int id)
        {
            return $"MCLY{id.ToString("00000000")}";
        }

        [HttpPost]
        public DataJsonResult Logout()
        {
            return new DataJsonResult();
        }

        [HttpPost]
        public DataJsonResult ClientPswReset()
        {
            string req = new StreamReader(HttpContext.Current.Request.InputStream).ReadToEnd();
            Log.SystemWriteDebug("修改密码======" + req);
            var ReqObj = JsonConvert.DeserializeObject<ChangePasswordModel>(req);

            var result = new DataJsonResult();
            M_UserInfo User = AuthorizedUser;
            if (User == null)
            {
                result.ReturnCode = "601";
                result.ErrorMessage = "账号不存在";
                return result;
            }
            if (User.PassWord != Common.Md5Password(ReqObj.OldPassword))
            {
                result.ReturnCode = "502";
                result.ErrorMessage = "密码不正确";
                return result;
            }
            else
            {
                if (ReqObj.OldPassword == ReqObj.NewPassword)
                {
                    result.ReturnCode = "503";
                    result.ErrorMessage = "修改密码不允许与原始密码相同";
                    return result;
                }
                else
                {
                    User.PassWord = Common.Md5Password(ReqObj.NewPassword);
                    B_UserInfo.Update(User);
                    result.ErrorMessage = null;
                    result.Data = new
                    {
                        User.ID,
                        User.UserName,
                        User.Tel,
                        User.Email,
                        User.InCompany?.CompanyName
                    };
                    return result;
                }
            }
        }
    }
    #region 接收类
    public class LoginModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }

    #endregion
}
