using System.Web;
using System.Web.Mvc;

namespace Fly01.Core.Presentation.Commons
{
    public static class LayoutHelper
    {
        private static string cdnVersion = "0.1.9";

        private static string serverUrl = $"https://mpn.azureedge.net/lib";

        private static string cssUrl = $"{serverUrl}/{cdnVersion}/mpnui.css";
        private static string jsUrl = $"{serverUrl}/{cdnVersion}/mpnui.js";
        private static string vendorJsUrl = $"{serverUrl}/{cdnVersion}/vendors.mpnui.js";

        public static IHtmlString HeadTemplate(this HtmlHelper<dynamic> html, string appName)
        {
            return new HtmlString(
                $"<title>{appName}</title>{System.Environment.NewLine}" +
                $"<link type=\"text/css\" rel=\"stylesheet\" href=\"Styles/mpnui_0.1.9.css\" />"
            );
        }

        public static IHtmlString ScriptsTemplate(this HtmlHelper html)
        {
            return new HtmlString(
                $"<script src=\"Scripts/mpnui_0.1.9.js\"></script>" +
                $"<script src=\"Scripts/vendors.mpnui_0.1.9.js\"></script>"
            );
        }
    }
}