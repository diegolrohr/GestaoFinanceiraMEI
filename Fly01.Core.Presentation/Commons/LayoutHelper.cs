using System.Web;
using System.Web.Mvc;

namespace Fly01.Core.Presentation.Commons
{
    public static class LayoutHelper
    {
        private static string cdnVersion = "1.1.4";

        private static string serverUrl = $"https://cdnfly01.azureedge.net/fly/{cdnVersion}"; private static string complJs = ".min"; /*
        private static string serverUrl = $"http://localhost:8000/{cdnVersion}";
        private static string complJs = ""; /**/

        private static string cssUrl = $"{serverUrl}/fly01ui.css";
        private static string jsUrl = $"{serverUrl}/fly01ui{complJs}.js";


        public static IHtmlString HeadTemplate(this HtmlHelper<dynamic> html, string appName)
        {
            return new HtmlString(
                "<meta charset=\"utf-8\">" +
                "<meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0\">" +
                $"<title>{appName}</title>" +
                "<base href=\"~/\" />" +
                $"<link type=\"text/css\" rel=\"stylesheet\" href=\"{cssUrl}\" />"
            );
        }

        public static IHtmlString ScriptsTemplate(this HtmlHelper html)
        {
            return new HtmlString($"<script src=\"{jsUrl}\"></script>");
        }
    }
}