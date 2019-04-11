using Newtonsoft.Json;
using System;

namespace Fly01.Core.ViewModels.Presentation.Commons
{
    public class PaisVM : DomainBaseVM
    {
        [JsonProperty("nome")]
        public string Nome { get; set; }

        [JsonProperty("codigoIbge")]
        public string CodigoIbge { get; set; }

        [JsonProperty("codigoBacen")]
        public string CodigoBacen { get; set; }

        [JsonProperty("codigoSiscomex")]
        public string CodigoSiscomex { get; set; }

        [JsonProperty("sigla")]
        public string Sigla { get; set; }
    }
}