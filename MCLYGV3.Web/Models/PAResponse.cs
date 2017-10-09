using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MCLYGV3.Web.Models
{
    public class PAResBase<T>
    {
        /// <summary>
        /// 返回的代码
        /// </summary>
        public string ret { get; set; }

        /// <summary>
        /// 返回的具体对象
        /// </summary>
        public T data { get; set; }

        /// <summary>
        /// 返回的消息
        /// </summary>
        public string msg { get; set; }

    }

    public class PAResAccessToken
    {
        /// <summary>
        /// 返回的失效时间
        /// </summary>
        public string expires_in { get; set; }

        /// <summary>
        /// openid
        /// </summary>
        public string openid { get; set; }

        /// <summary>
        /// access_token
        /// </summary>
        public string access_token { get; set; }

    }
}