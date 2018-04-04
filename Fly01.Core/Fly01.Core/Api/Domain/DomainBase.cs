using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Fly01.Core.Notifications;

namespace Fly01.Core.Api.Domain
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

        #region Notification

        [NotMapped]
        public Notification Notification { get; } = new Notification();

        public virtual void Validate() { }

        public void Fail(bool condition, Error error)
        {
            if (condition)
                Notification.Errors.Add(error);
        }

        public bool IsValid()
        {
            return !Notification.HasErrors;
        }

        #endregion

    }

    public static class DomainToQueryable
    {
        public static IQueryable<TDomainBase> ToQueryable<TDomainBase>(this TDomainBase instance)
        {
            return new[] { instance }.AsQueryable();
        }
    }
}