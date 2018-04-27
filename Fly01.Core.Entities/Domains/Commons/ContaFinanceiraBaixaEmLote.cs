using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fly01.Core.Entities.Domains.Commons
{
    public class ContaFinanceiraBaixaEmLote : PlataformaBase
    {
        [Column(TypeName = "date")]
        public DateTime Data { get; set; }
        
        public virtual List<Guid> ContasFinanceiras { get; set; }
        
        public Guid ContaBancariaId { get; set; }
        
        [DataType(DataType.MultilineText)]
        public string Observacao { get; set; }
        
        public virtual ContaBancaria ContaBancaria { get; set; }
    }
}