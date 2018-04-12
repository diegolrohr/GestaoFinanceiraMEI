using Newtonsoft.Json;
using System;

namespace Fly01.Core.Entities.ViewModels.Commons
{
    public abstract class DomainBaseVM
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }
    }
}