using System;
using Fly01.Core.Domain;

namespace Fly01.Estoque.Domain.Entities
{
    public class Cidade : DomainBase
    {
        public string NomeCidade { get; set; }
        public string CodigoIbge { get; set; }
        public Guid EstadoId { get; set; }

        #region NavigationProperties

        public virtual Estado Estado { get; set; }

        #endregion
    }
}
