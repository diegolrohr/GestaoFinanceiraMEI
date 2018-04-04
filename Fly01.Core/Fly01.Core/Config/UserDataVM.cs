using Fly01.Core.VM;
using System;

namespace Fly01.Core.Config
{
    [Serializable]
    public class UserDataVM
    {
        public TokenDataVM TokenData { get; set; }

        public string PlatformUrl { get; set; }

        public string PlatformUser { get; set; }
    }
}