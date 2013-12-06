using System.Web;
using System.Web.SessionState;
using Elmah;

namespace nCommon.Exception
{
    public class CustomErrorPageFactory : ErrorLogPageFactory
    {
        public override System.Web.IHttpHandler GetHandler(System.Web.HttpContext context, string requestType,
                                                           string url, string pathTranslated)
        {
            return base.GetHandler(context, requestType, url, pathTranslated);
        }

        public override void ReleaseHandler(System.Web.IHttpHandler handler)
        {
            base.ReleaseHandler(handler);
        }
    }

    public class ErrorLogHandlerFactory :
    Elmah.ErrorLogPageFactory
    {
        public override IHttpHandler GetHandler(
            HttpContext context,
            string requestType,
            string url, string pathTranslated)
        {
            var handler = base.GetHandler(context, requestType, url, pathTranslated);
            return new SessionRequringHandler(handler);
        }
    }

    public class ElmahAuthorizationHandler :
        HttpModuleBase,
        IRequestAuthorizationHandler
    {
        protected override void OnInit(HttpApplication application)
        {
            application.PreRequestHandlerExecute += delegate
                {
                    var context = application.Context;
                    if (IsFlaggedRequest(context))
                        CompleteAuthorization(context);
                };
            base.OnInit(application);
        }

        protected override bool SupportDiscoverability
        {
            get { return true; }
        }

        public virtual bool Authorize(HttpContext context)
        {
            if (!HttpContext.Current.User.Identity.IsAuthenticated) return false;
            FlagRequest(context);
            return true;
        }

        protected virtual void CompleteAuthorization(HttpContext context)
        {
            // Forbid more than 100 requests within a given session.
            if (context.Session == null)
                throw new System.Exception("Session unavailable.");
            var count = (int)(context.Session["ElmahRequestCount"] ?? 0) + 1;
            if (count > 100)
                throw new HttpException(403, "Unauthorized request.");
            context.Session["ElmahRequestCount"] = count;
        }

        protected virtual void FlagRequest(HttpContext context)
        {
            context.Items[GetType()] = true;
        }

        protected virtual bool IsFlaggedRequest(HttpContext context)
        {
            return (bool)(context.Items[GetType()] ?? false);
        }
    }


    public sealed class SessionRequringHandler :
    IHttpHandler,
    IRequiresSessionState
    {
        public IHttpHandler InnerHandler { get; private set; }
        public SessionRequringHandler(IHttpHandler handler)
        {
            InnerHandler = handler;
        }
        public void ProcessRequest(HttpContext context)
        {
            InnerHandler.ProcessRequest(context);
        }
        public bool IsReusable
        {
            get { return InnerHandler.IsReusable; }
        }
    }
}

