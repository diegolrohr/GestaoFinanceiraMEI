using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace Fly01.Core.Config
{
    [Serializable]
    public class TokenDataVM
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("errorCode")]
        public string ErrorCode { get; set; }

        [JsonProperty("errorMessage")]
        public string ErrorMessage { get; set; }

        [JsonProperty("token_type")]
        public string TokenType { get; set; }

        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }

        [JsonProperty("codigoMaxime")]
        public string CodigoMaxime { get; set; }

        [JsonProperty("userName")]
        public string UserName { get; set; }

        [JsonProperty(".issued")]
        public string IssuedAt { get; set; }

        [JsonProperty(".expires")]
        public string ExpiresAt { get; set; }

        [JsonProperty("trial")]
        public bool Trial { get; set; }

        [JsonIgnore]
        public DateTime? LicenseExpiration { get; set; }

        [JsonProperty("licenseExpiration")]
        public string LicenseExpirationString
        {
            get
            {
                return LicenseExpiration.HasValue ? LicenseExpiration.Value.ToString("yyyy-MM-dd") : null;
            }
            set
            {
                LicenseExpiration = string.IsNullOrWhiteSpace(value) ? (DateTime?)null : DateTime.ParseExact(value, "yyyy-MM-dd", CultureInfo.InvariantCulture);                
            }
        }

        [JsonIgnore]
        public List<NotificationVM> Notifications
        {
            get
            {
                if (string.IsNullOrWhiteSpace(NotificationsMessage))
                {
                    return new List<NotificationVM>();
                }
                return JsonConvert.DeserializeObject<List<NotificationVM>>(NotificationsMessage);
            }
            set
            {
                //
            }
        }

        [JsonProperty("notification")]
        public string NotificationsMessage { get; set; }
    }
}
