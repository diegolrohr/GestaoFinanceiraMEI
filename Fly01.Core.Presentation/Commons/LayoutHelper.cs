using System.Web;
using System.Web.Mvc;

namespace Fly01.Core.Presentation.Commons
{
    public static class LayoutHelper
    {
        private static string localUrl = "http://poad257.poa01.local:8000";
        private static string cdnUrl = "https://cdnfly01.azureedge.net";
        private static string cdnVersion = "1.1.1";

        public static IHtmlString HeadTemplate(this HtmlHelper<dynamic> html, string appName)
        {
            return new HtmlString(
                "<meta charset=\"utf-8\">" +
                "<meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0\">" +
                $"<title>{appName}</title>" +
                "<base href=\"~/\" />" +
                $"<link rel=\"icon\" type=\"image/ico\" href=\"{cdnUrl}/img/fly01icon.ico\" />" +
                $"<link type=\"text/css\" rel=\"stylesheet\" href=\"{cdnUrl}/fly/{cdnVersion}/fly01ui.css\" />"
                //$"<link type=\"text/css\" rel=\"stylesheet\" href=\"{localUrl}/{cdnVersion}/fly01ui.css\" />"
            );
        }

        public static IHtmlString ScriptsTemplate(this HtmlHelper html)
        {
            return new HtmlString(
                $"<script src=\"{cdnUrl}/fly/{cdnVersion}/fly01ui.min.js\"></script>"
                //$"<script src=\"{localUrl}/{cdnVersion}/fly01ui.js\"></script>"
            );
        }
    }
}