using System.Web;
using System.Web.Mvc;

namespace Fly01.Core.Presentation.Commons
{
    public static class LayoutHelper
    {
        private static string cdnVersion = "1.1.3";

        private static string localUrl = "http://poad257.poa01.local:8000";
        private static string cdnUrl = "https://cdnfly01.azureedge.net";

        private static string serverUrl = $"{cdnUrl}/fly/{cdnVersion}"; /*
        private static string serverUrl = $"{localUrl}/{cdnVersion}"; /**/

        public static IHtmlString HeadTemplate(this HtmlHelper<dynamic> html, string appName)
        {
            return new HtmlString(
                "<meta charset=\"utf-8\">" +
                "<meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0\">" +
                $"<title>{appName}</title>" +
                "<base href=\"~/\" />" +
                //$"<link rel=\"icon\" type=\"image/ico\" href=\"{cdnUrl}/img/fly01icon.ico\" />" +
                $"<link type=\"text/css\" rel=\"stylesheet\" href=\"{serverUrl}/fly01ui.css\" />" +
                "<link type=\"image/x-icon\" rel=\"icon\" href=\"data:image/jpeg;base64,/9j/4AAQSkZJRgABAQAAAQABAAD/2wCEAAMDAwMDAwQEBAQFBQUFBQcHBgYHBwsICQgJCAsRCwwLCwwLEQ8SDw4PEg8bFRMTFRsfGhkaHyYiIiYwLTA+PlQBAwMDAwMDBAQEBAUFBQUFBwcGBgcHCwgJCAkICxELDAsLDAsRDxIPDg8SDxsVExMVGx8aGRofJiIiJjAtMD4+VP/AABEIABAAEAMBIgACEQEDEQH/xABYAAEBAQAAAAAAAAAAAAAAAAAFBAcQAAEEAwEBAQAAAAAAAAAAAAMBAgQFBgcSABMxAQEAAAAAAAAAAAAAAAAAAAADEQADAAAAAAAAAAAAAAAAAAAAQWH/2gAMAwEAAhEDEQA/ANNyWzJs8j8+z6+mwdbGny62tr64pmGCQRFGGQdjGqjkLwrvP00CNpOmrthYTn8zIcKPZR4ljVyU+jVEcyBeUT054KFV6VOU9Pd2ZdHVdjgOXa+fk2DnsZEuusgr2nyOZTsEVqpy0oVXn9TwGNxJ+2DxsEwbHpNRrJZkWfYSJ4CIcRBm+5whM4ju/qqI1E8boih//9k=\">"
            );
        }

        public static IHtmlString ScriptsTemplate(this HtmlHelper html)
        {
            return new HtmlString($"<script src=\"{serverUrl}/fly01ui.js\"></script>");
        }
    }
}