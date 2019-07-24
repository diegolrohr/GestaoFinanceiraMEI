using Fly01.Core.Entities.Domains.Enum;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fly01.Core.Entities.Domains.Commons
{
    public class OrdemServico : PlataformaBase
    {
        [Required]
        public StatusOrdemServico Status { get; set; } = StatusOrdemServico.EmPreenchimento;

        [Required]
        public Guid ClienteId { get; set; }

        public int Numero { get; set; }

        [Required]
        [Column(TypeName = "date")]
        public DateTime DataEmissao { get; set; }

        [Column(TypeName = "date")]
        public DateTime DataEntrega { get; set; }

        public TimeSpan HoraEntrega { get; set; }

        public TimeSpan Duracao { get { return this.Duracao; } set { this.Tempo = value.TotalMinutes; } }

        public double? Tempo { get { return this.Tempo == null ? this.Duracao.TotalMinutes : this.Tempo; } set { this.Tempo = value; } }

        public Guid? ResponsavelId { get; set; }

        public bool Aprovado { get; set; }

        public bool GeraOrdemVenda { get; set; }

        [DataType(DataType.MultilineText)]
        [MaxLength(1000)]
        public string Descricao { get; set; }

        #region Navigation
        public virtual Pessoa Cliente { get; set; }
        public virtual Pessoa Responsavel { get; set; }
        #endregion
    }
}
