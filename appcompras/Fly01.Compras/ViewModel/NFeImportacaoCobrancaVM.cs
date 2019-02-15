using Fly01.Core.Entities.Domains.Commons;
using Fly01.Core.ViewModels.Presentation.Commons;
using Newtonsoft.Json;
using System;

namespace Fly01.Compras.ViewModel
{
    public class NFeImportacaoCobrancaVM : DomainBaseVM
    {
        [JsonProperty("nfeImportacaoId")]
        public Guid NFeImportacaoId { get; set; }

        [JsonProperty("numero")]
        public string Numero { get; set; }

        [JsonProperty("valor")]
        public double Valor { get; set; }

        [JsonProperty("dataVencimento")]
        public DateTime DataVencimento { get; set; }

        [JsonProperty("contaFinanceiraId")]
        public Guid? ContaFinanceiraId { get; set; }

        [JsonProperty("nfeImportacao")]
        public virtual NFeImportacao NFeImportacao { get; set; }
    }
}