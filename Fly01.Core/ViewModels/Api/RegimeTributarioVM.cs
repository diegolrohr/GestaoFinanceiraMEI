using Fly01.EmissaoNFE.Domain.Enums;
using Newtonsoft.Json;
using System;

namespace Fly01.Core.ViewModels.Api

{
    public class RegimeTributarioVM
    {   
        public bool IncentivadorCultural { get; set; }

        public bool SimplesNacional { get; set; }
        
        [JsonIgnore]
        public TipoRegimeTributario RegimeTributario { get; set; }
        
        [JsonProperty("RegimeTributario")]
        public string TipoRegimeTributario
        {
            get { return ((int)RegimeTributario).ToString(); }
            set { RegimeTributario = (TipoRegimeTributario)Enum.Parse(typeof(TipoRegimeTributario), value); }
        }
    }
}
