using Fly01.Core.Helpers.Attribute;
using Newtonsoft.Json;
using System;

namespace Fly01.Core.ViewModels.Presentation.Commons
{
    public class ProdutoVM : DomainBaseVM
    {
        [JsonProperty("imageProduto")]
        public string ImageProduto { get; set; }

        [JsonProperty("descricao")]
        public string Descricao { get; set; }

        [JsonProperty("grupoProdutoId")]
        public Guid? GrupoProdutoId { get; set; }

        [JsonProperty("unidadeMedidaId")]
        public Guid? UnidadeMedidaId { get; set; }

        [JsonProperty("ncmId")]
        public Guid? NcmId { get; set; }

        [APIEnum("TipoProduto")]
        [JsonProperty("tipoProduto")]
        public string TipoProduto { get; set; }

        [JsonProperty("saldoProduto")]
        public double? SaldoProduto { get; set; }

        [JsonProperty("codigoProduto")]
        public string CodigoProduto { get; set; }

        [JsonProperty("codigoBarras")]
        public string CodigoBarras { get; set; }

        [JsonProperty("valorVenda")]
        public double ValorVenda { get; set; }

        [JsonProperty("valorCusto")]
        public double ValorCusto { get; set; }

        [JsonProperty("saldoMinimo")]
        public double SaldoMinimo { get; set; }

        [JsonProperty("observacao")]
        public string Observacao { get; set; }

        [JsonProperty("aliquotaIpi")]
        public double AliquotaIpi { get; set; }

        [JsonProperty("cestId")]
        public Guid? CestId { get; set; }

        [JsonProperty("codigoNcm")]
        public string CodigoNcm { get; set; }

        [JsonProperty("codigoCest")]
        public string CodigoCest { get; set; }

        [JsonProperty("abreviacaoUnidadeMedida")]
        public string AbreviacaoUnidadeMedida { get; set; }

        [JsonProperty("codigoEnquadramentoLegalIPI")]
        public string CodigoEnquadramentoLegalIPI { get; set; }

        [JsonProperty("enquadramentoLegalIPIId")]
        public Guid? EnquadramentoLegalIPIId { get; set; }

        [APIEnum("OrigemMercadoria")]
        [JsonProperty("origemMercadoria")]
        public string OrigemMercadoria { get; set; }

        [JsonProperty("extipi")]
        public string EXTIPI { get; set; }

        [JsonProperty("grupoProduto")]
        public virtual GrupoProdutoVM GrupoProduto { get; set; }

        [JsonProperty("unidadeMedida")]
        public virtual UnidadeMedidaVM UnidadeMedida { get; set; }

        [JsonProperty("ncm")]
        public virtual NcmVM Ncm { get; set; }

        [JsonProperty("cest")]
        public virtual CestVM Cest { get; set; }

        [JsonProperty("enquadramentoLegalIPI")]
        public virtual EnquadramentoLegalIpiVM EnquadramentoLegalIPI { get; set; }

        [JsonProperty("registroFixo")]
        public bool RegistroFixo { get; set; }
    }
}