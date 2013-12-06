using System.Web;
using System.Web.Mvc;
using Elmah;

namespace nCommon.Exception
{
    /// <summary>
    /// 应用程序全局异常错误处理类
    /// </summary>
    public class ApplicationErrorHandle
    {
        public static void ProcessError()
        {
            var requestContext = ((MvcHandler)HttpContext.Current.CurrentHandler).RequestContext;
            var routeData = requestContext.RouteData;

            var ex = HttpContext.Current.Server.GetLastError();
            var action = "Index";

            if (HttpContext.Current.Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                action = "Ajax";
            }
            else if (ex is HttpException)
            {
                var httpEx = ex as HttpException;
                switch (httpEx.GetHttpCode())
                {
                    case 404:
                        action = "NotFound";
                        break;
                    default:
                        action = "Index";
                        break;
                }
            }
            routeData.Values["controller"] = "Error";
            routeData.Values["action"] = action;

            IControllerFactory factory = ControllerBuilder.Current.GetControllerFactory();
            IController controller = factory.CreateController(requestContext, "Error");
            ErrorSignal.FromCurrentContext().Raise(ex);
            controller.Execute(requestContext);
            HttpContext.Current.ClearError();
        }
    }
}
