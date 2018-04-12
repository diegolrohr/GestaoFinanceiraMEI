using Fly01.Core.Entities.Attribute;

namespace Fly01.Financeiro.Domain.Enums
{
    public enum TipoFormaPagamento
    {
        [Subtitle("Dinheiro", "Dinheiro")]
        Dinheiro = 1,
        [Subtitle("Cheque", "Cheque")]
        Cheque = 2,
        [Subtitle("CartaoCredito", "Cartão de Crédito")]
        CartaoCredito = 3,
        [Subtitle("CartaoDebito", "Cartão de Débito")]
        CartaoDebito = 4,
        [Subtitle("Transferencia", "Transferência")]
        Transferencia = 5,
        [Subtitle("Boleto", "Boleto")]
        Boleto = 6
    }
}