using System;

namespace Fly01.Financeiro.Models.ViewModel
{
    public class ImprimirListContasVM
    {
        public Guid Id { get; set; }
        public string Status { get; set; }
        public string Descricao { get; set; }
        public string Valor { get; set; }
        public string FormaPagamento { get; set; }
        public int? Numero { get; set; }
        public string Fornecedor { get; set; }
        public DateTime? Vencimento { get; set; }
        public string Titulo { get; set; }
    }
}