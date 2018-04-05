using Fly01.Core.Attribute;

namespace Fly01.Faturamento.Domain.Enums
{
    public enum TipoVenda
    {
        [Subtitle("Normal", "Normal", "Normal", "orange")]
        Normal = 1,
        //[Subtitle("Devolucao", "Devolução", "Devolução", "red")]
        //Devolucao = 2
    }
}
