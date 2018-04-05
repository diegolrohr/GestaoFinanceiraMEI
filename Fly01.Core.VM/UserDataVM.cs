using System;

namespace Fly01.Core.VM
{
    [Serializable]
    public class UserDataVM
    {
        public TokenDataVM TokenData { get; set; }

        public string PlatformUrl { get; set; }

        public string PlatformUser { get; set; }
    }
}