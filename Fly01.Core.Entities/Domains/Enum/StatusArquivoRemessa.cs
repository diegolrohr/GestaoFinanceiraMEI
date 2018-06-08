using Fly01.Core.Helpers.Attribute;

namespace Fly01.Core.Entities.Domains.Enum
{
    public enum StatusArquivoRemessa
    {
        [Subtitle("AguardandoRetorno", "Aguardando Retorno", "AGUARD", "orange", "Aguardando arquivo de retorno")]
        AguardandoRetorno = 1, 

        [Subtitle("RetornoParcial", "Retorno Parcial", "PARCIAL", "black", "Não foram pagos todos os boletos referente a este arquivo")]
        RetornoParcial = 2,

        [Subtitle("Finalizado", "Finalizado", "FINALI", "green", "Todos os boletos deste arquivo foram pagos com sucesso")]
        Finalizado = 3
    }
}