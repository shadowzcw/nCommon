using System.Web.Mvc;

namespace nCommon.Auth
{
    public class CrossDomainAccessAttribute : ActionFilterAttribute
    {
        private readonly string _domains;

        public CrossDomainAccessAttribute(string domains)
        {
            _domains = domains;
        }
          
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            filterContext.HttpContext.Response.Headers["Access-Control-Allow-Origin"] = _domains;
            base.OnActionExecuting(filterContext);
        }
    }
}
