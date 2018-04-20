using Fly01.Core.Helpers.Attribute;
using Fly01.Core.ViewModels.Presentation.Commons;
using Newtonsoft.Json;
using System;

namespace Fly01.Faturamento.ViewModel
{
    public class ServicoVM : DomainBaseVM
    {
        [JsonProperty("codigoServico")]
        public string CodigoServico { get; set; }

        [JsonProperty("descricao")]
        public string Descricao { get; set; }

        [JsonProperty("nbsId")]
        public Guid? NbsId { get; set; }

        [APIEnum("TipoTributacaoISS")]
        [JsonProperty("tipoTributacaoISS")]
        public string TipoTributacaoISS { get; set; }

        [APIEnum("TipoPagamentoImpostoISS")]
        [JsonProperty("tipoPagamentoImpostoISS")]
        public string TipoPagamentoImpostoISS { get; set; }
        
        [JsonProperty("valorServico")]
        public double ValorServico { get; set; }

        [JsonProperty("observacao")]
        public string Observacao { get; set; }

        [JsonProperty("nbs")]
        public virtual NBSVM Nbs { get; set; }
    }
}
