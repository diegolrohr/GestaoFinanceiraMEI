using System;
using Fly01.Core.Notifications;
using Newtonsoft.Json;
using Fly01.Core.Rest;
using System.Collections.Generic;

namespace Fly01.Core.API
{
    public class SocketIOHelper
    {
        public static void NewMessage(SocketMessageVM message)
        {
            Emit("newMessage", message);
        }

        public static void AnotherEventExample(SocketUserMessageVM message)
        {
            Emit("userEvent", message);
        }

        private static void Emit(string emitEvent, SocketMessageBaseVM message)
        {
            try
            {
                RestHelper.ExecutePostRequest(AppDefaults.UrlGatewayNew, $"notifications/{emitEvent}", JsonConvert.SerializeObject(message), null, null, 300);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex?.Message);
            }
        }
    }
    public class SocketMessageBaseVM
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("notificationDate")]
        public DateTime NotificationDate { get; set; }

        [JsonProperty("readDate")]
        public DateTime? ReadDate { get; set; }
    }
    public class SocketMessageVM : SocketMessageBaseVM
    {
        [JsonProperty("platformId")]
        public string PlatformId { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("messageType")]
        public string MessageType { get; set; }

        [JsonProperty("platformApps")]
        public List<SocketPlatformAppVM> PlatformApps { get; set; }
    }
    public class SocketPlatformAppVM
    {
        [JsonProperty("clientId")]
        public string ClientId { get; set; }

        [JsonProperty("actionUrl")]
        public string ActionUrl { get; set; }
    }
    public class SocketPlatformVM
    {
        [JsonProperty("Id")]
        public string Id { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
    public class SocketUserMessageVM : SocketMessageBaseVM
    {

        [JsonProperty("platform")]
        public SocketPlatformVM Platform { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }
    }
}