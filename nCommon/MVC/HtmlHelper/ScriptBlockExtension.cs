using System;
using System.Text;
using System.Web.Mvc;
using System.Web.WebPages;

namespace nCommon.MVC.HtmlHelper
{
    public static class ScriptBlockExtension
    {
        private const string Scriptblock = "ScriptBlock";

        public static MvcHtmlString ScriptBlock(this System.Web.Mvc.HtmlHelper htmlHelper,
          Func<dynamic, HelperResult> template)
        {
            var context = htmlHelper.ViewContext.HttpContext;
            if (!context.Request.IsAjaxRequest())
            {
                var scriptBuilder = context.Items[Scriptblock]
                     as StringBuilder ?? new StringBuilder();

                scriptBuilder.Append(template(null).ToHtmlString());

                context.Items[Scriptblock] = scriptBuilder;

                return new MvcHtmlString(string.Empty);
            }
            return new MvcHtmlString(template(null).ToHtmlString());
        }

        public static MvcHtmlString WriteScriptBlocks(this System.Web.Mvc.HtmlHelper htmlHelper)
        {
            var scriptBuilder = htmlHelper.ViewContext.HttpContext.Items[Scriptblock]
                   as StringBuilder ?? new StringBuilder();

            return new MvcHtmlString(scriptBuilder.ToString());
        }
    }
}
