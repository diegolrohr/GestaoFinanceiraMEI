namespace Fly01.Core.Entities.Attribute
{
    public class SubtitleAttribute : System.Attribute
    {
        public SubtitleAttribute(string key, string value, string description = "", string cssClass = "")
        {
            Key = key;
            Value = value;
            Description = description;
            CssClass = cssClass;
        }

        public string Key { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }
        public string CssClass { get; set; }
    }
}