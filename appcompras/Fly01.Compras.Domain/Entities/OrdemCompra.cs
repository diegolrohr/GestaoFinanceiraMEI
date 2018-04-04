using Fly01.Compras.Domain.Enums;
using Fly01.Core.Api.Domain;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fly01.Compras.Domain.Entities
{
    public class OrdemCompra : PlataformaBase
    {
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Numero { get; set; }

        [Required]
        public StatusOrdemCompra Status { get; set; }

        [Required]
        [Column(TypeName = "date")]
        public DateTime Data{ get; set; }

        public Guid? FormaPagamentoId { get; set; }

        public Guid? CondicaoParcelamentoId { get; set; }

        public Guid? CategoriaId { get; set; }

        [Column(TypeName = "date")]
        public DateTime? DataVencimento { get; set; }

        [Required]
        public TipoOrdemCompra TipoOrdemCompra { get; set; }

        [DataType(DataType.MultilineText)]
        public string Observacao { get; set; }

        #region Navigation Properties

        public virtual CondicaoParcelamento CondicaoParcelamento { get; set; }
        public virtual FormaPagamento FormaPagamento { get; set; }
        public virtual Categoria Categoria { get; set; }

        #endregion

    }
}
