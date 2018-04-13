using Fly01.Core.Entities.Attribute;
using Newtonsoft.Json;
using System;

namespace Fly01.Core.Entities.ViewModels.Commons
{
    public abstract class ProdutoBaseVM : DomainBaseVM
    {
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

        [JsonProperty("enquadramentoLegalIPIId")]
        public Guid? EnquadramentoLegalIPIId { get; set; }

        #region Navigations Properties

        [JsonProperty("grupoProduto")]
        public virtual GrupoProdutoBaseVM GrupoProduto { get; set; }

        [JsonProperty("unidadeMedida")]
        public virtual UnidadeMedidaBaseVM UnidadeMedida { get; set; }

        [JsonProperty("ncm")]
        public virtual NcmBaseVM Ncm { get; set; }

        [JsonProperty("cest")]
        public virtual CestBaseVM Cest { get; set; }

        [JsonProperty("enquadramentoLegalIPI")]
        public virtual EnquadramentoLegalIpiBaseVM EnquadramentoLegalIPI { get; set; }

        #endregion

    }
}
