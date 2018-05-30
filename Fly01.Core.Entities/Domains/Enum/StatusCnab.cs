using Fly01.Core.Helpers.Attribute;

namespace Fly01.Core.Entities.Domains.Enum
{
    public enum StatusCnab
    {
        [Subtitle("BoletoGerado", "Boleto Gerado", "GERADO", "blue", "Boleto Gerado, sem arquivo de remessa vinculado")]
        BoletoGerado = 1,

        [Subtitle("AguardandoRetorno", "Aguardando Retorno", "AGUARD", "orange", "O boleto foi remetido para o banco e está aguardando o arquivo de retorno para baixa do título")]
        AguardandoRetorno = 2,

        [Subtitle("Baixado", "Boleto Baixado", "BAIXADO", "green", "O boleto foi remetido para o banco e pago pelo cliente, de acordo com arquivo de retorno recebido")]
        Baixado = 3
    }
}