using System;
using Fly01.Core.Helpers;

namespace Fly01.Financeiro.Entities.ViewModel
{
    [Serializable]
    public enum BankStatementImportStatus
    {
        [Subtitle("Pendente", "Pendente", cssClass: "orange")]
        Pendente = 1,

        [Subtitle("Encerrado", "Encerrado", cssClass: "green")]
        Encerrado = 2
    }
}
