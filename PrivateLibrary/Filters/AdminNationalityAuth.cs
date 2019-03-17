using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PrivateLibrary.Filters
{
    public class AdminNationalityAuth : System.Web.Mvc.ActionFilterAttribute, System.Web.Mvc.IActionFilter
    {

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (!HttpContext.Current.Session["Admin"].ToString().Equals("1"))
            {
                filterContext.Result = new RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary(
                    new { Controller = "BookCorrects", Action = "Index" }
                ));
            }
            base.OnActionExecuted(filterContext);
        }

    }
}