using System;
using System.Text.RegularExpressions;
using System.Web;

namespace nCommon.Auth
{
    /// <summary>
    /// 跨域form认证跳转returnurl重写
    /// </summary>
    public class CrossDomainRedirectModule : IHttpModule
    {
        /// <summary>
        /// 配置的统一登录域名
        /// </summary>
        private static readonly string AuthDomain = System.Configuration.ConfigurationManager.AppSettings["n.AuthDomain"];

        public void Init(HttpApplication context)
        {
            context.EndRequest += OnEndRequest;
        }

        private void OnEndRequest(object sender, EventArgs e)
        {
            var context = sender as HttpApplication;
            string redirectUrl = context.Response.RedirectLocation;

            if (string.IsNullOrEmpty(redirectUrl) || (!string.IsNullOrEmpty(AuthDomain) && !redirectUrl.StartsWith(AuthDomain)) || string.IsNullOrEmpty(AuthDomain)) return;
            if (!string.IsNullOrEmpty(redirectUrl))
            {
                context.Response.RedirectLocation = Regex.Replace(redirectUrl, "ReturnUrl=(?<url>.*)", m =>
                {
                    //这里!string.IsNullOrEmpty(AuthDomain) && redirectUrl.StartsWith(AuthDomain)判断是为了解决form跨域认证请求url带参数时Response.RedirectLocation的returnUrl参数重复的问题
                    var returnUrl = !string.IsNullOrEmpty(AuthDomain) && redirectUrl.StartsWith(AuthDomain) ? context.Request.Url.AbsoluteUri : m.Groups["url"].Value;
                    string url = HttpUtility.UrlDecode(returnUrl);
                    var u = new Uri(context.Request.Url, url);
                    return string.Format("ReturnUrl={0}", HttpUtility.UrlEncode(u.ToString()));
                }, RegexOptions.Singleline | RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture);
            }
        }

        public void Dispose()
        {

        }
    }
}
