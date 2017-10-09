using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Filters;
using MCLYGV3.DB;

namespace MCLYGV3.Web.Models
{
    [EnableCors("*", "*", "*")]
    public class BaseApiController : ApiController
    {

        public DataJsonResult ErrorResult;
        public bool IsAuth;

        public BaseApiController()
        {
            ErrorResult = new DataJsonResult();
            var key = "MCLY20170901MCLYMCLY20170901MCLY";
            var iv = "MCLY20170901MCLY";
            string text = HttpContext.Current.Request.Headers["Authorization"];
            if (!string.IsNullOrWhiteSpace(text))
            {
                try
                {
                    var orgStr = DecryptByAES(text, key, iv);
                    if (orgStr.Contains("+"))
                    {
                        var psd = orgStr.Split('+');
                        if (DateTime.Now.ToString("yyyy-MM-dd") == psd[1])
                        {
                            IsAuth = true;
                            AuthorizedUser = B_UserInfo.Find(UnUserId(psd[0]));
                            if (AuthorizedUser == null)
                            {
                                IsAuth = false;
                                ErrorResult.ReturnCode = "5010";
                                ErrorResult.ErrorMessage = "身份验证失败";
                            }
                        }
                        else
                        {
                            IsAuth = false;
                            ErrorResult.ReturnCode = "5011";
                            ErrorResult.ErrorMessage = "时间错误";
                        }
                    }
                }
                catch (Exception)
                {
                    IsAuth = false;
                    ErrorResult.ReturnCode = "5022";
                    ErrorResult.ErrorMessage = "Token格式错误";
                }
                
               
            }
           
        }
        public string Options()
        {
            return null; // HTTP 200 response with empty body
        }
        public M_UserInfo AuthorizedUser { get; set; }


        public static int UnUserId(string userId)
        {
            var id =Convert.ToInt32(userId.Substring(4, userId.Length - 4));
            return id;
        }

        /// <summary>  
        /// AES解密  
        /// </summary>  
        /// <param name="input">密文字节数组</param>  
        /// <param name="key">密钥（32位）</param>  
        /// <returns>返回解密后的字符串</returns>  
        public static string DecryptByAES(string input, string key,string AES_IV)
        {
            byte[] inputBytes = HexStringToByteArray(input);
            byte[] keyBytes = Encoding.UTF8.GetBytes(key.Substring(0, 32));
            using (AesCryptoServiceProvider aesAlg = new AesCryptoServiceProvider())
            {
                aesAlg.Key = keyBytes;
                aesAlg.IV = Encoding.UTF8.GetBytes(AES_IV.Substring(0, 16));

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                using (MemoryStream msEncrypt = new MemoryStream(inputBytes))
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srEncrypt = new StreamReader(csEncrypt))
                        {
                            return srEncrypt.ReadToEnd();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 将指定的16进制字符串转换为byte数组
        /// </summary>
        /// <param name="s">16进制字符串(如：“7F 2C 4A”或“7F2C4A”都可以)</param>
        /// <returns>16进制字符串对应的byte数组</returns>
        public static byte[] HexStringToByteArray(string s)
        {
            s = s.Replace(" ", "");
            byte[] buffer = new byte[s.Length / 2];
            for (int i = 0; i < s.Length; i += 2)
                buffer[i / 2] = (byte)Convert.ToByte(s.Substring(i, 2), 16);
            return buffer;
        }

    }



}
