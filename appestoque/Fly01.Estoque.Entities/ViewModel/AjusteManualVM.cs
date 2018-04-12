using Fly01.Core.Attribute;
using Fly01.Core.Entities.ViewModels.Commons;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace Fly01.Estoque.Entities.ViewModel
{
    [Serializable]
    public class AjusteManualVM : DomainBaseVM
    {
        [JsonProperty("tipoEntradaSaida")]
        [Display(Name = "Tipo Entrada Saída")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [StringLength(1, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        [APIEnum("TipoEntradaSaida")]
        public string TipoEntradaSaida { get; set; }

        [JsonProperty("tipoMovimentoId")]
        public Guid TipoMovimentoId { get; set; }

        [JsonProperty("produtoId")]
        public Guid ProdutoId { get; set; }

        [JsonProperty("quantidade")]
        public double? Quantidade { get; set; }

        [JsonProperty("observacao")]
        public string Observacao { get; set; }

        #region Navigations Properties

        [JsonProperty("produto")]
        public virtual ProdutoVM Produto { get; set; }

        [JsonProperty("tipoMovimento")]
        public virtual TipoMovimentoVM TipoMovimento { get; set; }

        #endregion

    }
}
