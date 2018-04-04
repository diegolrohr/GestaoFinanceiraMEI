using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Fly01.Financeiro.Domain.Base
{
    public class DomainBase
    {
        [Key]
        public Guid Id { get; set; }

        public DateTime DataInclusao { get; set; }

        public DateTime? DataAlteracao { get; set; }

        public DateTime? DataExclusao { get; set; }

        [Required]
        public string UsuarioInclusao { get; set; }

        public string UsuarioAlteracao { get; set; }

        public string UsuarioExclusao { get; set; }

        public bool Ativo { get; set; }
    }

    public static class DomainToQueryable
    {
        public static IQueryable<TDomainBase> ToQueryable<TDomainBase>(this TDomainBase instance)
        {
            return new[] { instance }.AsQueryable();
        }
    }
}