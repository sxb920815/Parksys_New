using MCLYGV3.Web.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace MCLYGV3.Web.ClassLib
{
    public static class Access_Token
    {
        private static DateTime LastDateTime;
        private static double _expires_in;
        private static string _accesstoken;

        static Access_Token()
        {
            _expires_in = 0;
            _accesstoken = GetAccessTokenByServer();
        }

        public static string GetAccessToken()
        {
            TimeSpan ts = DateTime.Now - LastDateTime;
            if (ts.TotalMinutes > _expires_in)
                return GetAccessTokenByServer();
            else
                return _accesstoken;

        }

        private static string GetAccessTokenByServer()
        {
            //https://test-api.pingan.com.cn:20443/oauth/oauth2/access_token?client_id=123&grant_type=123&client_secret=123123
            string baseurl = ConfigurationManager.AppSettings["PA_URL"];//https://test-api.pingan.com.cn:20443/
            string client_id = ConfigurationManager.AppSettings["PA_client_id"];
            string client_secret = ConfigurationManager.AppSettings["PA_client_secret"];

            string url = $"{baseurl}oauth/oauth2/access_token?client_id={client_id}&grant_type=client_credentials&client_secret={client_secret}";
            string res = HttpOperation.GetHtml(url);
            if (res.Contains("Error:") == false)
            {
                if (res.Contains("access_token"))
                {
                    PAResBase<PAResAccessToken> ResObj = JsonConvert.DeserializeObject<PAResBase<PAResAccessToken>>(res);
                    if (ResObj != null)
                    {
                        _expires_in = double.Parse(ResObj.data.expires_in);
                        _accesstoken = ResObj.data.access_token;
                    }
                    else
                    {
                        _expires_in = 0;
                        _accesstoken = "Error";
                    }
                }
                else
                {
                    _expires_in = 0;
                    _accesstoken = "Error";
                }
            }
            else
            {
                _expires_in = 0;
                _accesstoken = "Error";
            }
            LastDateTime = DateTime.Now;
            return _accesstoken;
        }
    }

    
}