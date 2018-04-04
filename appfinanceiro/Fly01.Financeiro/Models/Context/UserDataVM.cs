using Fly01.Utils.VM;
using System;

namespace Fly01.Financeiro.Models.Context
{
    [Serializable]
    public class UserDataVM
    {
        public TokenDataVM TokenData { get; set; }

        public string PlatformUrl { get; set; }

        public string PlatformUser { get; set; }
    }
}