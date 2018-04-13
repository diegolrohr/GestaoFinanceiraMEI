using Fly01.Core.Entities.Attribute;
using Fly01.Core.Entities.ViewModels.Commons;
using Newtonsoft.Json;
using System;

namespace Fly01.Faturamento.Entities.ViewModel
{
    public class ServicoVM : DomainBaseVM
    {
        [JsonProperty("codigoServico")]
        public string CodigoServico { get; set; }

        [JsonProperty("descricao")]
        public string Descricao { get; set; }

        [JsonProperty("nbsId")]
        public Guid? NbsId { get; set; }

        //[APIEnum("TipoServico")]
        //[JsonProperty("tipoServico")]
        //public string TipoServico { get; set; }

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

        #region Navigations Properties
        
        [JsonProperty("nbs")]
        public virtual NBSVM Nbs { get; set; }

        #endregion
    }
}
