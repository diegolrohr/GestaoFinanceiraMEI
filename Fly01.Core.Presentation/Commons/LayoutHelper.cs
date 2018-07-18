using System.Web;
using System.Web.Mvc;

namespace Fly01.Core.Presentation.Commons
{
    public static class LayoutHelper
    {
        private static string cdnVersion = "1.1.4";

        private static string serverUrl = $"https://cdnfly01.azureedge.net/fly/{cdnVersion}"; /*
        private static string serverUrl = $"http://poad257.poa01.local:8000/{cdnVersion}"; /**/

        public static IHtmlString HeadTemplate(this HtmlHelper<dynamic> html, string appName)
        {
            return new HtmlString(
                "<meta charset=\"utf-8\">" +
                "<meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0\">" +
                $"<title>{appName}</title>" +
                "<base href=\"~/\" />" +
                $"<link type=\"text/css\" rel=\"stylesheet\" href=\"{serverUrl}/fly01ui.css\" />"
            );
        }

        public static IHtmlString ScriptsTemplate(this HtmlHelper html)
        {
            return new HtmlString($"<script src=\"{serverUrl}/fly01ui.js\"></script>");
        }
    }
}