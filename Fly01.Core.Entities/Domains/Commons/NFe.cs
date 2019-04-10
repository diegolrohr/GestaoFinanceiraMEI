using Fly01.Core.Entities.Domains.Enum;
using System;
using System.ComponentModel.DataAnnotations;

namespace Fly01.Core.Entities.Domains.Commons
{
    public class NFe : NotaFiscal
    {
        public NFe()
        {
            TipoNotaFiscal = TipoNotaFiscal.NFe;
        }
        
        public double TotalImpostosProdutos { get; set; }

        public double TotalImpostosProdutosNaoAgrega { get; set; }

        public TipoNfeComplementar TipoNfeComplementar { get; set; }

        public Guid? UFSaidaPaisId { get; set; }

        [StringLength(60)]
        public string LocalEmbarque { get; set; }

        [StringLength(60)]
        public string LocalDespacho { get; set; }

        public virtual Estado UFSaidaPais { get; set; }
    }
}