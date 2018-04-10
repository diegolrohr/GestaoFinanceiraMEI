using Newtonsoft.Json;
using System;

namespace Fly01.Faturamento.Entities.ViewModel
{
    [Serializable]
    public class NFeVM : NotaFiscalVM
    {
        [JsonProperty("totalImpostosProdutos")]
        public double TotalImpostosProdutos { get; set; }

        [JsonProperty("totalImpostosProdutosNaoAgrega")]
        public double TotalImpostosProdutosNaoAgrega { get; set; }
    }
}