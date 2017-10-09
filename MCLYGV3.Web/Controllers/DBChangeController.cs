using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MCLYGV3.Web.Controllers
{
    public class DBChangeController : ApiController
    {
        [HttpGet]
        public bool AddUser()
        {
            DBChange.addUser();
            return true;
        }

        [HttpGet]
        public int AddOrder()
        {
            return DBChange.AddOrder();
        }
    }
}
