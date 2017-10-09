using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;

namespace MCLYGV3.Web.Models
{
    public class DataJsonResult
    {
        public string ErrorMessage { get; set; }

        public string ReturnCode { get; set; } = "200";

        public bool Success
        {
            get { return string.IsNullOrWhiteSpace(ErrorMessage); }
            set
            {
                Success = string.IsNullOrWhiteSpace(ErrorMessage);
            }
        }

        public Object Data { get; set; }
    }
}