using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;

namespace Fly01.Core.Helpers
{
    public static class HintHelper
    {
        public static MvcHtmlString Hint(this HtmlHelper html, string hint)
        {
            var span = new TagBuilder("span");
            span.InnerHtml = "<i class=\"fa fa-question-circle\" aria-hidden=\"true\"></i>";

            var htmlAttributes = new Dictionary<string, object> { { "class", "hint" }, { "data-toggle", "tooltip-hint" }, { "data-placement", "top" }, { "title", hint } };
            span.MergeAttributes(new RouteValueDictionary(htmlAttributes));

            return MvcHtmlString.Create(span.ToString());
        }
    }
}