using Fly01.Core.Entities.Domains.Enum;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Fly01.Core.ViewModels.Presentation.Commons
{
    public class SocketMessageVM
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("platformId")]
        public string PlatformId { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("messageType")]
        public string MessageType { get; set; }

        [JsonProperty("platformApps")]
        public List<SocketPlatformAppVM> PlatformApps { get; set; }

        [JsonProperty("notificationDate")]
        public DateTime NotificationDate { get; set; }

        [JsonProperty("readDate")]
        public DateTime? ReadDate { get; set; }
    }
}