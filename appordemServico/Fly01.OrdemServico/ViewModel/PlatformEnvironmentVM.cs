using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Fly01.OrdemServico.ViewModel
{
    [Serializable]
    public class PlatformEnvironmentVM
    {
        [JsonProperty("platformUrl")]
        public string PlatformUrl { get; set; }

        [JsonProperty("platformUsers")]
        public List<UserEnvironmentVM> PlatformUsers { get; set; }
    }

    [Serializable]
    public class UserEnvironmentVM
    {
        [JsonProperty("platformUser")]
        public string PlatformUser { get; set; }

        [JsonProperty("platformUserName")]
        public string PlatformUserName { get; set; }
    }
}