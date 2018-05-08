using Fly01.Core.Helpers.Attribute;

namespace Fly01.Core.Entities.Domains.Enum
{
    public enum StatusBoletoBancaria
    {
        [Subtitle("Aprovado", "Aprovado", "APRO", "green")] 
        Aprovado = 1,

        [Subtitle("AguardandoRertorno", "Aguardando Rertorno", "AGR", "orange")]
        AguardandoRertorno = 2,
    }
}
