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

        [JsonProperty("issId")]
        public Guid? IssId { get; set; }

        [JsonProperty("codigoTributacaoMunicipal")]
        public string CodigoTributacaoMunicipal { get; set; }
        
        [JsonProperty("valorServico")]
        public double ValorServico { get; set; }

        [JsonProperty("observacao")]
        public string Observacao { get; set; }

        [JsonProperty("codigoIssEspecifico")]
        public string CodigoIssEspecifico { get; set; }

        [JsonProperty("codigoFiscalPrestacao")]
        public string CodigoFiscalPrestacao { get; set; }

        [JsonProperty("unidadeMedidaId")]
        public Guid? UnidadeMedidaId { get; set; }

        [JsonProperty("codigoIss")]
        public string CodigoIss { get; set; }

        [JsonProperty("codigoNbs")]
        public string CodigoNbs { get; set; }

        [JsonProperty("abreviacaoUnidadeMedida")]
        public string AbreviacaoUnidadeMedida { get; set; }

        [JsonProperty("iss")]
        public virtual ISSVM Iss { get; set; }

        [JsonProperty("nbs")]
        public virtual NBSVM Nbs { get; set; }

        [JsonProperty("unidadeMedida")]
        public virtual UnidadeMedidaVM UnidadeMedida { get; set; }

        [JsonProperty("registroFixo")]
        public bool RegistroFixo { get; set; }
    }
}
