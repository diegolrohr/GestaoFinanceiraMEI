using System.Web;
using System.Web.Mvc;

namespace Fly01.Core.Presentation.Commons
{
    public static class LayoutHelper
    {
        //private static string cdnVersion = "0.1.6"; /*
        private static string cdnVersion = "stage"; /* */
        private static string libName = "mpnui";

        private static string serverUrl = $"https://mpn.azureedge.net/lib/{cdnVersion}"; /*
        private static string serverUrl = $"http://10.51.5.42:8000/{cdnVersion}"; /* */

        private static string cssUrl = $"{serverUrl}/{libName}.css";
        private static string jsUrl = $"{serverUrl}/{libName}.js";
        private static string vendorJsUrl = $"{serverUrl}/vendors.{libName}.js";

        public static IHtmlString HeadTemplate(this HtmlHelper<dynamic> html, string appName)
        {
            return new HtmlString(
                $"<title>{appName}</title>{System.Environment.NewLine}" +
                $"<link type=\"text/css\" rel=\"stylesheet\" href=\"{cssUrl}\" />"
            );
        }

        public static IHtmlString ScriptsTemplate(this HtmlHelper html)
        {
            return new HtmlString(
                $"<script src=\"{vendorJsUrl}\"></script>" +
                $"<script src=\"{jsUrl}\"></script>"
            );
        }
    }
}