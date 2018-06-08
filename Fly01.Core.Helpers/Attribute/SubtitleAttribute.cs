namespace Fly01.Core.Helpers.Attribute
{
    public class SubtitleAttribute : System.Attribute
    {
        public SubtitleAttribute(string key, string value, string description = "", string cssClass = "", string tooltipHint = "")
        {
            Key = key;
            Value = value;
            Description = description;
            CssClass = cssClass;
            TooltipHint = tooltipHint;
        }

        public string Key { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }
        public string CssClass { get; set; }
        public string TooltipHint { get; set; }
    }
}