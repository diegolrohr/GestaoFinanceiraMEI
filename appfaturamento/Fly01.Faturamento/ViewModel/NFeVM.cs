using Fly01.Core.Entities.Domains.Enum;
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
        public TipoNfeComplementar TipoNfeComplementar { get; set; }
    }
}