using Fly01.Core.Entities.Domains.Enum;
using Fly01.Core.ViewModels.Presentation.Commons;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fly01.OrdemServico.ViewModel
{
    public class OrdemServicoVM : DomainBaseVM
    {
        [Required]
        public StatusOrdemServico Status { get; set; }

        [Required]
        public Guid PessoaId { get; set; }

        [Required]
        public int Numero { get; set; }

        [Required]
        [Column(TypeName = "date")]
        public DateTime DataEmissao { get; set; }

        [Required]
        [Column(TypeName = "date")]
        public DateTime DataEntrega { get; set; }

        public Guid ResponsavelId { get; set; }

        public bool Aprovado { get; set; }

        [DataType(DataType.MultilineText)]
        public string Observacao { get; set; }

        public bool ValorTotal { get; set; }

        #region Navigation
        public virtual PessoaVM Pessoa { get; set; }
        public virtual PessoaVM Responsavel { get; set; }
        #endregion
    }
}
