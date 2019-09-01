using Fly01.Core.ViewModels;
using System;
using System.Collections.Generic;

namespace Fly01.Core.Config
{
    [Serializable]
    public class UserDataVM
    {
        public TokenDataVM TokenData { get; set; }
        public string ClientToken { get; set; }        
        public string PlatformUrl { get; set; }
        public string PlatformUser { get; set; }
        public List<PermissionResponseVM> Permissions { get; set; }
    }
}