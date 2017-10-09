using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Filters;

namespace MCLYGV3.Web.Models
{
    public class ExceptionHandlingAttribute: ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext context)
        {
            var exception = context.Exception;
            if (exception != null)
            {
                Log.Error(exception);
            }
        }
    }
}