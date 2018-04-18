using System;
using System.ComponentModel.DataAnnotations;
using Fly01.Core.Entities.Domains;
using Fly01.Core.Entities.Domains.Commons;
using Fly01.Core.Entities.Domains.Enum;

namespace Fly01.Compras.Domain.Entities
{
    public class GrupoProduto : PlataformaBase
    {
        [StringLength(40, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public string Descricao { get; set; }

        public double AliquotaIpi { get; set; }

        public TipoProduto TipoProduto { get; set; }

        public Guid? UnidadeMedidaId { get; set; }

        public Guid? NcmId { get; set; }

        #region Navigations Properties

        public virtual Ncm Ncm { get; set; }

        public virtual UnidadeMedida UnidadeMedida { get; set; }

        #endregion
    }
}