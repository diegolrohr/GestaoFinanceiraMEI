using System;
using System.ComponentModel.DataAnnotations;
using Fly01.Core.VM;

namespace Fly01.Compras.Entities.ViewModel
{
    [Serializable]
    public class CidadeVM : DomainBaseVM
    {
        [Required]
        [StringLength(35)]
        public string Nome { get; set; }

        [Required]
        [StringLength(7)]
        public string CodigoIBGE { get; set; }

        [Required]
        public Guid EstadoId { get; set; }

        #region NavigationProperties

        public virtual EstadoVM Estado { get; set; }

        #endregion
    }
}