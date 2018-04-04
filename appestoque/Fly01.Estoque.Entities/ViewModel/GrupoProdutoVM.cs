using Fly01.Core.Api;
using Fly01.Core.VM;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace Fly01.Estoque.Entities.ViewModel
{
    [Serializable]
    public class GrupoProdutoVM : DomainBaseVM
    {
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [JsonProperty("descricao")]
        public string Descricao { get; set; }

        [JsonProperty("aliquotaIpi")]
        public double AliquotaIpi { get; set; }

        [JsonProperty("tipoProduto")]
        [APIEnum("TipoProduto")]
        public string TipoProduto { get; set; }

        [JsonProperty("unidadeMedidaId")]
        public Guid? UnidadeMedidaId { get; set; }

        [JsonProperty("ncmId")]
        public Guid? NcmId { get; set; }

        #region Navigations Properties

        [JsonProperty("ncm")]
        public virtual NCMVM Ncm { get; set; }
        [JsonProperty("unidadeMedida")]
        public virtual UnidadeMedidaVM UnidadeMedida { get; set; }

        #endregion
    }
}
