using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web;
using System.Web.Http.Filters;

namespace nCommon.OAuth
{
    public class TokenFilterAttribute : AuthorizationFilterAttribute
    {
        public override void OnAuthorization(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            var unauthorized = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
            if (actionContext.Request.Headers.All(x => x.Key != "Authorization"))
            {
                actionContext.Response = unauthorized;
                return;
            }
            string authHeader = actionContext.Request.Headers.GetValues("Authorization").FirstOrDefault();
            if (authHeader == null)
            {
                actionContext.Response = unauthorized;
                return;
            }
            var principal = WebAPIAuthorizationManager.VerifyOAuth2(actionContext.Request, actionContext.Request.RequestUri.AbsoluteUri);
            if (principal != null)
            {
                Thread.CurrentPrincipal = principal;
                if (HttpContext.Current != null)
                {
                    HttpContext.Current.User = principal;
                }
                return;
            }
            actionContext.Response = unauthorized;
            base.OnAuthorization(actionContext);
        }
    }
}