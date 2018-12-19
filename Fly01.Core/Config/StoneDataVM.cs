using Fly01.Core.ViewModels.Presentation.Commons;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Fly01.Core.Config
{
    [Serializable]
    public class StoneDataVM
    {
        public StoneDataVM()
        {
            Simulacoes = new List<StoneAntecipacaoVM>();
        }

        [JsonProperty("token")]
        public string Token { get; set; }

        [JsonProperty("simulacoes")]
        public List<StoneAntecipacaoVM> Simulacoes { get; set; }

        [JsonProperty("dadosBancarios")]
        public StoneDadosBancariosVM DadosBancarios { get; set; }

        [JsonProperty("antecipacaoConfiguracao")]
        public StoneConfiguracaoVM AntecipacaoConfiguracao { get; set; }

        [JsonProperty("antecipacaoTotais")]
        public StoneTotaisVM AntecipacaoTotais { get; set; }
        
    }
}
