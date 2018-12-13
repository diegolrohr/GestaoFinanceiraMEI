using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Fly01.Core.Entities.Domains.Commons
{
    public class StoneTokenBase
    {         
        [JsonIgnore]
        public string Token { get; set; }
    }
}
