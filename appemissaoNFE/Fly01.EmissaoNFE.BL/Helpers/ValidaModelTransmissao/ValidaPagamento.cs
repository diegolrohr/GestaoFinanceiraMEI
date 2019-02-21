using Fly01.Core.Entities.Domains.Enum;
using Fly01.Core.Notifications;
using Fly01.EmissaoNFE.Domain.ViewModel;
using System.Linq;

namespace Fly01.EmissaoNFE.BL.Helpers.ValidaModelTransmissao
{
    public static class ValidaPagamento
    {
        public static void ExecutarValidaPagamento(ItemTransmissaoVM item, EntitiesBLToValidate entitiesBLToValidate, TransmissaoVM entity, int nItem)
        {
            #region Validação da classe Pagamento

            if (item.Pagamento == null)
                entity.Fail(true, new Error("Os dados de pagamento são obrigatórios.  Item: " + nItem, "Item.Pagamento"));
            else
            {
                entity.Fail(item.Pagamento.ValorTroco.HasValue && item.Pagamento.ValorTroco < 0, new Error("Se informado, o valor do troco não pode ser negativo.", "Item.Pagamento.ValorTroco"));
                ValidarDadosPagamento(item, entity, nItem);
            }

            #endregion Validação da classe Totais
        }

        private static void ValidarDadosPagamento(ItemTransmissaoVM item, TransmissaoVM entity, int nItem)
        {
            if (item.Pagamento.DetalhesPagamentos == null || !item.Pagamento.DetalhesPagamentos.Any())
                entity.Fail(true, new Error("Os dados dos detalhes dos pagamentos são obrigatórios.", "Item.Pagamento.DetalhesPagamentos"));
            else
            {
                ValidarDetalhePagamento(item, entity, nItem);

                var isSemPagamento = item.Identificador.FinalidadeEmissaoNFe == TipoCompraVenda.Ajuste || item.Identificador.FinalidadeEmissaoNFe == TipoCompraVenda.Devolucao || item.Identificador.FinalidadeEmissaoNFe == TipoCompraVenda.Complementar;
                var valorTotalNF = item.Total.ICMSTotal.ValorTotalNF;
                var somaPagamentos = item.Pagamento.DetalhesPagamentos.Where(x => x.TipoFormaPagamento != TipoFormaPagamento.SemPagamento).Sum(x => x.ValorPagamento);
                var troco = item.Pagamento.ValorTroco.HasValue ? item.Pagamento.ValorTroco : 0;

                if (!isSemPagamento)
                {                    
                    entity.Fail((somaPagamentos > valorTotalNF) && ((somaPagamentos - troco) != valorTotalNF), new Error("Valor do troco inválido ou não informado. Troco = (total pagamentos - total nota). Item[" + nItem + "].Pagamento.ValorTroco."));
                }

                if (valorTotalNF.Equals(somaPagamentos))
                {
                    item.Pagamento.ValorTroco = null;
                }
            }
        }

        private static void ValidarDetalhePagamento(ItemTransmissaoVM item, TransmissaoVM entity, int nItem)
        {
            var nItemPagamento = 1;
            foreach (var detalhePagamento in item.Pagamento.DetalhesPagamentos)
            {
                var isSemPagamento = item.Identificador.FinalidadeEmissaoNFe == TipoCompraVenda.Ajuste || item.Identificador.FinalidadeEmissaoNFe == TipoCompraVenda.Devolucao || item.Identificador.FinalidadeEmissaoNFe == TipoCompraVenda.Complementar;
                entity.Fail(detalhePagamento.ValorPagamento <= 0 && (!isSemPagamento && detalhePagamento.TipoFormaPagamento != TipoFormaPagamento.SemPagamento), new Error("O valor do pagamento deve ser maior que zero. Item[" + nItem + "].Pagamento.DetalhesPagamentos[" + (nItemPagamento) + "].ValorPagamento."));
                entity.Fail(isSemPagamento && detalhePagamento.TipoFormaPagamento != TipoFormaPagamento.SemPagamento, new Error("Nota de ajuste ou devolução, somente forma de pagamento do tipo Sem Pagamento. Item[" + nItem + "].Pagamento.DetalhesPagamentos[" + (nItemPagamento) + "].TipoFormaPagamento."));
                entity.Fail(detalhePagamento.TipoFormaPagamento == TipoFormaPagamento.Transferencia, new Error("Forma de pagamento do tipo Transferência inválido, informe o tipo Outros. Item[" + nItem + "].Pagamento.DetalhesPagamentos[" + (nItemPagamento) + "].TipoFormaPagamento."));
                nItemPagamento++;
            }
        }
    }
}
