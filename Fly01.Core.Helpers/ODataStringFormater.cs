namespace Fly01.Core.Helpers
{
    public static class ODataStringFormater
    {
        public static string GetString(string value)
        {
            if (value.Length > 0 && value.Contains("'"))
            {
                value = value.Replace("'", "");
            }

            return value;
        }
    }
}