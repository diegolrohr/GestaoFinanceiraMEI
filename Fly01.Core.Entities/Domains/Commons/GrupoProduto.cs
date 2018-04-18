using System;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using Fly01.Core.Entities.Domains.Enum;

namespace Fly01.Core.Entities.Domains.Commons
{
    public class GrupoProduto : PlataformaBase
    {
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [StringLength(40, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string Descricao { get; set; }

        public double AliquotaIpi { get; set; }
        
        public TipoProduto TipoProduto { get; set; }

        public Guid? UnidadeMedidaId { get; set; }

        public Guid? NcmId { get; set; }

        public virtual Ncm Ncm { get; set; }

        public virtual UnidadeMedida UnidadeMedida { get; set; }
    }
}