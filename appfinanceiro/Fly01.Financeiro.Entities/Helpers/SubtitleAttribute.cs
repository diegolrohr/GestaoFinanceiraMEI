using System;

namespace Fly01.Financeiro.Entities.Helpers
{
    public class SubtitleAttribute : Attribute
    {
        public SubtitleAttribute(string cssClass, string title, string shortTitle = "")
        {
            CssClass = cssClass;
            Title = title;
            ShortTitle = shortTitle;
        }

        public string CssClass { get; set; }
        public string Title { get; set; }
        public string ShortTitle { get; set; }
    }
}