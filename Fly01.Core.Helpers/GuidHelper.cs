using System;

namespace Fly01.Core.Helpers
{
    public static class GuidHelper
    {
        public static bool IsValidGuid(Guid guid)
        {
            return guid != Guid.Empty;
        }

        public static bool IsValidGuid(string guid)
        {
            try
            {
                return !string.IsNullOrWhiteSpace(guid) && IsValidGuid(Guid.Parse(guid));
            }
            catch
            {
                return false;
            }
        }
    }
}