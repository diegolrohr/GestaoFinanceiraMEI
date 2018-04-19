using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fly01.Core.Entities.Domains.Commons
{
    [Table("ConfiguracaoNotificacao")]
    public class ConfiguracaoNotificacaoFinanceiro : PlataformaBase
    {
        public bool NotificaViaEmail { get; set; }

        public bool NotificaViaSMS { get; set; }

        public DayOfWeek DiaSemana { get; set; }

        public TimeSpan HoraEnvio { get; set; }

        public bool ContasAPagar { get; set; }

        public bool ContasAReceber { get; set; }

        [StringLength(70, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string EmailDestino { get; set; }

        [StringLength(20, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string ContatoDestino { get; set; }
    }
}