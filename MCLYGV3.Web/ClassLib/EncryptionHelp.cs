using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MCLYGV3.Web.ClassLib
{
    public class EncryptionHelp
    {
        public static string Hash_SHA_256(string word, bool toUpper = false)
        {
            try
            {
                System.Security.Cryptography.SHA256CryptoServiceProvider SHA256CSP
                    = new System.Security.Cryptography.SHA256CryptoServiceProvider();

                byte[] bytValue = System.Text.Encoding.UTF8.GetBytes(word);
                byte[] bytHash = SHA256CSP.ComputeHash(bytValue);
                SHA256CSP.Clear();

                //根据计算得到的Hash码翻译为SHA-1码
                string sHash = "", sTemp = "";
                for (int counter = 0; counter < bytHash.Count(); counter++)
                {
                    long i = bytHash[counter] / 16;
                    if (i > 9)
                    {
                        sTemp = ((char)(i - 10 + 0x41)).ToString();
                    }
                    else
                    {
                        sTemp = ((char)(i + 0x30)).ToString();
                    }
                    i = bytHash[counter] % 16;
                    if (i > 9)
                    {
                        sTemp += ((char)(i - 10 + 0x41)).ToString();
                    }
                    else
                    {
                        sTemp += ((char)(i + 0x30)).ToString();
                    }
                    sHash += sTemp;
                }

                //根据大小写规则决定返回的字符串
                return toUpper ? sHash : sHash.ToLower();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}