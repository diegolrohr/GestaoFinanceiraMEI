using System.Web;
using System.Web.Mvc;

namespace Fly01.Core.Presentation.Commons
{
    public static class LayoutHelper
    {
        public static IHtmlString HeadTemplate(this HtmlHelper<dynamic> html, string appName)
        {
            return new HtmlString(
                "<meta charset=\"utf-8\">" +
                "<meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0\">" +
                $"<title>{appName}</title>" +
                "<base href=\"~/\" />" +
                "<link rel=\"icon\" type=\"image/ico\" href=\"https://cdnfly01.azureedge.net/img/fly01icon.ico\" />" +
                "<link type=\"text/css\" rel=\"stylesheet\" href=\"https://cdnfly01.azureedge.net/fly/1.1.1/fly01ui.css\" />"
                //"<link type=\"text/css\" rel=\"stylesheet\" href=\"http://poad257.poa01.local:8000/1.1.0/fly01ui.css\" />"
            );
        }

        public static IHtmlString ScriptsTemplate(this HtmlHelper html)
        {
            return new HtmlString(
                "<script src=\"https://cdnfly01.azureedge.net/fly/1.1.1/fly01ui.min.js\"></script>"
                //"<script src=\"http://poad257.poa01.local:8000/1.1.0/fly01ui.js\"></script>"
            );
        }
    }
}