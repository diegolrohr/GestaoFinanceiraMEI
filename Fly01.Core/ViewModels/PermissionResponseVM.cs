using Newtonsoft.Json;
using System;

namespace Fly01.Core.ViewModels
{
    [Serializable]
    public class PermissionResponseVM
    {
        [JsonProperty("resourceHash")]
        public string ResourceHash { get; set; }

        [JsonProperty("permissionValue")]
        public EPermissionValue PermissionValue { get; set; }
    }
}