using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Fly01.Core.Entities.Domains.Commons
{
    public class ConciliacaoBancaria : EmpresaBase
    {
        [Required]
        public Guid ContaBancariaId { get; set; }

        public string Arquivo { get; set; }

        public virtual ContaBancaria ContaBancaria { get; set; }
        public virtual List<ConciliacaoBancariaItem> ConciliacaoBancariaItens { get; set; }
    }
}