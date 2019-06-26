using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Filters;
//using System.Web.Mvc;

namespace ToolsWebClient.Filters
{
    /// <summary>
    /// 跨域
    /// </summary>
    public class Cores : ActionFilterAttribute
    {
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            base.OnActionExecuted(actionExecutedContext);
            actionExecutedContext.Response.Headers.Add("Access-Control-Allow-Origin", "*");
        }
    }
}