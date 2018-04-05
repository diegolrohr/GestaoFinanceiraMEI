using System;

namespace Fly01.Financeiro.Models.ViewModel
{
    public class ImprimirListContasVM
    {
        public Guid Id { get; set; }
        public string Status { get; set; }
        public string Descricao { get; set; }
        public double? Valor { get; set; }
        public string FormaPagamento { get; set; }
        public int? Parcelas { get; set; }
        public string Fornecedor { get; set; }
        public DateTime? Vencimento { get; set; }
    }
}