﻿using Newtonsoft.Json;

namespace Fly01.Core.ViewModels.Presentation
{
    public class AppsInstaladosVM
    {
        [JsonProperty("appName")]
        public string AppName { get; set; }

        [JsonProperty("appId")]
        public string AppId { get; set; }
    }
}