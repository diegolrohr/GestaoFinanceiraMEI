using System;
using Fly01.Core.Entities.Attribute;

namespace Fly01.Financeiro.Entities.ViewModel
{
    [Serializable]
    public enum BorderoStatusVM
    {
        [Subtitle("BorderoNaoGerado", "Não Gerado", cssClass: "orange")]
        BorderoNaoGerado = 1,

        [Subtitle("BorderoGerado", "Gerado", cssClass: "green")]
        BorderoGerado = 2,

        [Subtitle("BorderoEnviadoAoBanco", "Enviado ao Banco", cssClass: "gray")]
        BorderoEnviadoAoBanco = 3,

        [Subtitle("BorderoPendente", "Pendente", cssClass: "red")]
        BorderoPendente = 4,

        [Subtitle("BorderoRejeitado", "Rejeitado", cssClass: "brown")]
        BorderoRejeitado = 5,

        [Subtitle("BorderoEncerrado", "Encerrado", cssClass: "silver")]
        BorderoEncerrado = 6
    }
}