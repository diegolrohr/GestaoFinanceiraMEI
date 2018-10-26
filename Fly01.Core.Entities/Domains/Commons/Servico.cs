using System;
using System.ComponentModel.DataAnnotations;

namespace Fly01.Core.Entities.Domains.Commons
{
    [Serializable]
    public class Servico : PlataformaBase
    {
        public string CodigoServico { get; set; }

        public string Descricao { get; set; }

        public Guid? NbsId { get; set; }

        public Guid? IssId { get; set; }

        [StringLength(20)]
        public string CodigoTributacaoMunicipal { get; set; }

        public double ValorServico { get; set; }

        [StringLength(200, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string Observacao { get; set; }

        [StringLength(20)]
        public string CodigoIssEspecifico { get; set; }

        [StringLength(5)]
        public string CodigoFiscalPrestacao { get; set; }

        public Guid? UnidadeMedidaId { get; set; }

        public virtual Nbs Nbs { get; set; }

        public virtual Iss Iss { get; set; }

        public virtual UnidadeMedida UnidadeMedida { get; set; }
    }
}
