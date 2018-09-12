using System.Web;
using System.Web.Mvc;

namespace Fly01.Core.Presentation.Commons
{
    public static class LayoutHelper
    {
        private static string cdnVersion = "1.1.6";

        //private static string serverUrl = $"https://mpn.azureedge.net/lib/{cdnVersion}"; private static string complJs = ".min"; private static string libName = "mpnui"; /*
        private static string serverUrl = $"https://cdnfly01.azureedge.net/fly/{cdnVersion}"; private static string complJs = ".min"; private static string libName = "fly01ui"; /*
        private static string serverUrl = $"http://localhost:8000/{cdnVersion}"; private static string complJs = ""; /**/

        private static string cssUrl = $"{serverUrl}/{libName}.css";
        private static string jsUrl = $"{serverUrl}/{libName}{complJs}.js";

        public static IHtmlString HeadTemplate(this HtmlHelper<dynamic> html, string appName)
        {
            return new HtmlString(
                $"<title>{appName}</title>{System.Environment.NewLine}" +
                $"<link type=\"text/css\" rel=\"stylesheet\" href=\"{cssUrl}\" />"
            );
        }

        public static IHtmlString ScriptsTemplate(this HtmlHelper html)
        {
            return new HtmlString($"<script src=\"{jsUrl}\"></script>");
        }
    }
}