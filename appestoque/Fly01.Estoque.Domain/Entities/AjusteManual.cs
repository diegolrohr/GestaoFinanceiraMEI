using Fly01.Estoque.Domain.Enums;
using Fly01.Core.Domain;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace Fly01.Estoque.Domain.Entities
{
    public class AjusteManual : PlataformaBase
    {
        public const int ObservacaoMaxLength = 200;

        public TipoEntradaSaida TipoEntradaSaida { get; set; }
        public Guid TipoMovimentoId { get; set; }
        public Guid ProdutoId { get; set; }
        public double? Quantidade { get; set; }
        [StringLength(ObservacaoMaxLength, ErrorMessage = "O campo {0} deve possuir no máximo {1} caracteres.")]
        public string Observacao { get; set; }

        #region Navigations Properties

        [JsonIgnore]
        public virtual Produto Produto { get; set; }

        [JsonIgnore]
        public virtual TipoMovimento TipoMovimento { get; set; }

        #endregion
    }
}