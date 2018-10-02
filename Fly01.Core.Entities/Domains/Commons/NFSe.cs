using Fly01.Core.Entities.Domains.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fly01.Core.Entities.Domains.Commons
{
    public class NFSe : NotaFiscal
    {
        public NFSe()
        {
            TipoNotaFiscal = TipoNotaFiscal.NFSe;
        }

        public double TotalImpostosServicos { get; set; }

        [Column(TypeName = "varchar(MAX)")]
        [MaxLength(int.MaxValue)]
        public string XMLUnicoTSS { get; set; }
    }
}