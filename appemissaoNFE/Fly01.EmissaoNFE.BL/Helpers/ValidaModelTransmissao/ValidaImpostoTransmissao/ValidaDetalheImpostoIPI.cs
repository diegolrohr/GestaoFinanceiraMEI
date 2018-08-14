using Fly01.Core.Notifications;
using Fly01.EmissaoNFE.Domain.Entities.NFe;
using Fly01.EmissaoNFE.Domain.ViewModel;

namespace Fly01.EmissaoNFE.BL.Helpers.ValidaModelTransmissao.ValidaImpostoTransmissao
{
    public static class ValidaDetalheImpostoIPI
    {
        public static void ExecutaValidaImpostoIPI(Detalhe detalhe, TransmissaoVM entity, int nItemDetalhe, ItemTransmissaoVM item)
        {
            if (detalhe.Imposto.IPI != null)
            {
                entity.Fail(string.IsNullOrEmpty(detalhe.Imposto.IPI.CodigoEnquadramento),
                    new Error("Código de enquadramento legal do IPI é obrigatório. Item: " + nItemDetalhe, "Item.Detalhes[" + (nItemDetalhe) + "].Imposto.IPI.CodigoEnquadramento"));
                entity.Fail(string.IsNullOrEmpty(detalhe.Imposto.IPI.CodigoST.ToString()),
                    new Error("CST do IPI é obrigatório. Item: " + nItemDetalhe, "Item.Detalhes[" + (nItemDetalhe) + "].Imposto.IPI.CodigoST"));
                entity.Fail(!string.IsNullOrEmpty(detalhe.Imposto.IPI.CodigoST.ToString()) && (int)detalhe.Imposto.IPI.CodigoST < 50 && (int)item.Identificador.TipoDocumentoFiscal == 1,
                    new Error("CST do IPI inválido para uma nota de saída. Item: " + nItemDetalhe, "Item.Detalhes[" + (nItemDetalhe) + "].Imposto.IPI.CodigoST"));
                entity.Fail(!string.IsNullOrEmpty(detalhe.Imposto.IPI.CodigoST.ToString()) && (int)detalhe.Imposto.IPI.CodigoST >= 50 && (int)item.Identificador.TipoDocumentoFiscal == 0,
                    new Error("CST do IPI inválido para uma nota de entrada. Item: " + nItemDetalhe, "Item.Detalhes[" + (nItemDetalhe) + "].Imposto.IPI.CodigoST"));
                entity.Fail(string.IsNullOrEmpty(detalhe.Imposto.IPI.ValorBaseCalculo.ToString()),
                    new Error("Base de cálculo do IPI é obrigatória. Item: " + nItemDetalhe, "Item.Detalhes[" + (nItemDetalhe) + "].Imposto.IPI.ValorBaseCalculo"));
                entity.Fail(string.IsNullOrEmpty(detalhe.Imposto.IPI.PercentualIPI.ToString()),
                    new Error("Alíquota do IPI é obrigatória. Item: " + nItemDetalhe, "Item.Detalhes[" + (nItemDetalhe) + "].Imposto.IPI.PercentualIPI"));
                entity.Fail(string.IsNullOrEmpty(detalhe.Imposto.IPI.ValorIPI.ToString()),
                    new Error("Valor do IPI é obrigatório. Item: " + nItemDetalhe, "Item.Detalhes[" + (nItemDetalhe) + "].Imposto.IPI.ValorIPI"));
                entity.Fail(string.IsNullOrEmpty(detalhe.Imposto.IPI.QtdTotalUnidadeTributavel.ToString()),
                    new Error("Quantidade tributada do IPI é obrigatória. Item: " + nItemDetalhe, "Item.Detalhes[" + (nItemDetalhe) + "].Imposto.IPI.QtdTotalUnidadeTributavel"));
                entity.Fail(string.IsNullOrEmpty(detalhe.Imposto.IPI.ValorUnidadeTributavel.ToString()),
                    new Error("Valor por unidade tributável do IPI é obrigatório. Item: " + nItemDetalhe, "Item.Detalhes[" + (nItemDetalhe) + "].Imposto.IPI.ValorUnidadeTributavel"));
            }
        }
    }
}
