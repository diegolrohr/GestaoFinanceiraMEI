namespace Fly01.Core.Attribute
{
    public class APIEnumAttribute : System.Attribute
    {
        private string EnumContext { get; set; }

        public APIEnumAttribute(string APIPropertyName)
        {
            EnumContext = APIPropertyName;
        }

        public string Get(string appName)
        {
            return string.Concat(appName, EnumContext);
        }
    }
}