using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Fly01.Core.Entities.Domains.Commons
{
    public class StoneTokenBase
    {
        [Required]
        [JsonProperty("token")]
        public string Token { get; set; }
    }
}
