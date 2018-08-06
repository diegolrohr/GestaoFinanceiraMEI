using Fly01.Core.Entities.Domains.Enum;
using Fly01.Core.Helpers.Attribute;
using Newtonsoft.Json;
using System;

namespace Fly01.Faturamento.ViewModel
{
    [Serializable]
    public class NFeVM : NotaFiscalVM
    {
        [JsonProperty("totalImpostosProdutos")]
        public double TotalImpostosProdutos { get; set; }

        [JsonProperty("totalImpostosProdutosNaoAgrega")]
        public double TotalImpostosProdutosNaoAgrega { get; set; }

        [JsonProperty("tipoNfeComplementar")]
        [APIEnum("TipoNfeComplementar")]
        public string TipoNfeComplementar { get; set; }
    }
}