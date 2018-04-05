using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using Fly01.Core.Domain;

namespace Fly01.Financeiro.Domain.Entities
{
    public class ConciliacaoBancaria : PlataformaBase
    {
        [Required]
        public Guid ContaBancariaId { get; set; }      

        //AppDataContext model.builder ignore
        public string Arquivo { get; set; }

        #region Navigations Properties

        public virtual ContaBancaria ContaBancaria { get; set; }
        public virtual List<ConciliacaoBancariaItem> ConciliacaoBancariaItens { get; set; }

        #endregion
    }
}