using Fly01.Core.Entities.Domains;
using System;
using System.ComponentModel.DataAnnotations;

namespace Fly01.Compras.Domain.Entities
{
    public class Cest : DomainBase
    {
        [Required]
        public string Codigo { get; set; }
        
        [Required]
        [MaxLength(650)]
        public string Descricao { get; set; }

        public string Segmento { get; set; }

        public string Item { get; set; }

        public string Anexo { get; set; }

        public Guid? NcmId { get; set; }

        #region NavigationProperties

        public virtual NCM Ncm { get; set; }

        #endregion
    }
}