using Fly01.Core.Entities.Attribute;

namespace Fly01.Financeiro.Domain.Enums
{
    public enum TipoPessoa
    {
        [Subtitle("Fisica", "Física")]
        Fisica = 1,
        [Subtitle("Juridica", "Jurídica")]
        Juridica = 2
    }
}