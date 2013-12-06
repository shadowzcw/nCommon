using System.Web.Mvc;

namespace nCommon.Auth
{
    public class AjaxAuthorizeAttribute : AuthorizeAttribute
    {
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.Request.IsAjaxRequest() )
            {
                filterContext.HttpContext.Response.ContentType = "application/json";
                filterContext.Result = new JsonResult
                {
                    Data = new
                    {
                        result = 0
                    },
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
        }
    }
}