using Newtonsoft.Json;
using System;

namespace Fly01.Core.ViewModels.Presentation.Commons
{
    public abstract class EmpresaBaseVM
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }
    }
}