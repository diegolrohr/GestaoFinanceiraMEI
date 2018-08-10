using Fly01.Core.Notifications;
using Fly01.EmissaoNFE.Domain.Entities.NFe;
using Fly01.EmissaoNFE.Domain.ViewModel;
using System;
using System.Linq;

namespace Fly01.EmissaoNFE.BL.Helpers.ValidaModelTransmissao
{
    public static class ValidaDetalheProduto
    {
        public static void ExecutarValidaDetalheProduto(Detalhe detalhe, EntitiesBLToValidate entitiesBLToValidate, TransmissaoVM entity, int nItemDetalhe)
        {
            #region Validações da classe Detalhe.Produto

            if (detalhe.Produto == null)
                entity.Fail(true, new Error("Os dados de produto são obrigatórios. Item: " + nItemDetalhe, "Item.Detalhes[" + (nItemDetalhe) + "].Produto"));
            else
            {
                detalhe.NumeroItem = nItemDetalhe;

                ValidarCodigoProduto(detalhe, entity, nItemDetalhe);
                ValidarCodigoDeBarras(detalhe, entity, nItemDetalhe);
                ValidarDescricaoProduto(detalhe, entity, nItemDetalhe);
                ValidarNCMProduto(detalhe, entity, nItemDetalhe);
                ValidarCFOP(detalhe, entity, nItemDetalhe);
                ValidarUnidadeMedida(detalhe, entity, nItemDetalhe);
                ValidarQuantidadeProduto(detalhe, entity, nItemDetalhe);
                ValidarValorUnitarioProduto(detalhe, entity, nItemDetalhe);
                ValidarValorBrutoProduto(detalhe, entity, nItemDetalhe);
                ValidarCodigoBarrasTributacao(detalhe, entity, nItemDetalhe);
                ValidarUnidadeDeTributacao(detalhe, entity, nItemDetalhe);
                ValidarQuantidadeDeTributacao(detalhe, entity, nItemDetalhe);
                ValidarValorUnitarioDeTributacao(detalhe, entity, nItemDetalhe);
                ValidarAgregaTotalNota(detalhe, entity, nItemDetalhe);
                ValidaCodigoProdutoInvalido(detalhe, entity, nItemDetalhe);
                ValidaCodigoBarrasProduto(detalhe, entity, nItemDetalhe);
                ValidarCFOPProduto(detalhe, entitiesBLToValidate, entity, nItemDetalhe);
                ValidarUnidadeMedidaProduto(detalhe, entity, nItemDetalhe);
                ValidarCodigoDeBarrasUnidadeTributacao(detalhe, entity, nItemDetalhe);
                ValidarUnidadeTributacao(detalhe, entity, nItemDetalhe);

                ValidarLengthDescricaoProduto(detalhe, entity, nItemDetalhe);
                ValidarLenghtNCMProduto(detalhe, entity, nItemDetalhe);
                ValidarLengthQuantidadeProduto(detalhe, entity, nItemDetalhe);
                ValidarLengthValorUnitarioProduto(detalhe, entity, nItemDetalhe);
                ValidarLengthValorBrutoProduto(detalhe, entity, nItemDetalhe);
                ValidarLengthQuantidadeTributada(detalhe, entity, nItemDetalhe);
                ValidarLengthValorUnitario(detalhe, entity, nItemDetalhe);
                ValidarLengthValorFrete(detalhe, entity, nItemDetalhe);
                ValidarLengthValorSeguro(detalhe, entity, nItemDetalhe);
                ValidarLengthValorDesconto(detalhe, entity, nItemDetalhe);
                ValidarLengthValorDespesa(detalhe, entity, nItemDetalhe);
            }
            #endregion
        }

        #region Validações
        private static void ValidarLengthValorDespesa(Detalhe detalhe, TransmissaoVM entity, int nItemDetalhe)
        {
            if (detalhe.Produto.ValorOutrasDespesas != null)
            {
                string[] split = { "." };
                var numero = detalhe.Produto.ValorOutrasDespesas.ToString().Split(split, StringSplitOptions.RemoveEmptyEntries);

                for (int x = 0; x < numero.Length; x++)
                {
                    if (x == 0)
                        entity.Fail(numero[x].Length < 1 || numero[x].Length > 15, new Error("Valor de outras despesas inválido. (Tam. 15.2) Item: " + nItemDetalhe, "Item.Detalhes[" + (nItemDetalhe) + "].Produto.ValorOutrasDespesas"));
                    else
                        entity.Fail(numero[x] != null && numero[x].Length > 2, new Error("Quantidade de casas decimais inválida. (Tam. 15.2) Item: " + nItemDetalhe, "Item.Detalhes[" + (nItemDetalhe) + "].Produto.ValorOutrasDespesas"));
                }
            }
        }

        private static void ValidarLengthValorDesconto(Detalhe detalhe, TransmissaoVM entity, int nItemDetalhe)
        {
            if (detalhe.Produto.ValorDesconto != null)
            {
                string[] split = { "." };
                var numero = detalhe.Produto.ValorDesconto.ToString().Split(split, StringSplitOptions.RemoveEmptyEntries);

                for (int x = 0; x < numero.Length; x++)
                {
                    if (x == 0)
                        entity.Fail(numero[x].Length < 1 || numero[x].Length > 15, new Error("Valor de desconto inválido. (Tam. 15.2) Item: " + nItemDetalhe, "Item.Detalhes[" + (nItemDetalhe) + "].Produto.ValorDesconto"));
                    else
                        entity.Fail(numero[x] != null && numero[x].Length > 2, new Error("Quantidade de casas decimais inválida. (Tam. 15.2) Item: " + nItemDetalhe, "Item.Detalhes[" + (nItemDetalhe) + "].Produto.ValorDesconto"));
                }
            }
        }

        private static void ValidarLengthValorSeguro(Detalhe detalhe, TransmissaoVM entity, int nItemDetalhe)
        {
            if (detalhe.Produto.ValorSeguro != null)
            {
                string[] split = { "." };
                var numero = detalhe.Produto.ValorSeguro.ToString().Split(split, StringSplitOptions.RemoveEmptyEntries);

                for (int x = 0; x < numero.Length; x++)
                {
                    if (x == 0)
                        entity.Fail(numero[x].Length < 1 || numero[x].Length > 15, new Error("Valor de seguro inválido. (Tam. 15.2) Item: " + nItemDetalhe, "Item.Detalhes[" + (nItemDetalhe) + "].Produto.ValorSeguro"));
                    else
                        entity.Fail(numero[x] != null && numero[x].Length > 2, new Error("Quantidade de casas decimais inválida. (Tam. 15.2) Item: " + nItemDetalhe, "Item.Detalhes[" + (nItemDetalhe) + "].Produto.ValorSeguro"));
                }
            }
        }

        private static void ValidarLengthValorFrete(Detalhe detalhe, TransmissaoVM entity, int nItemDetalhe)
        {
            if (detalhe.Produto.ValorFrete != null)
            {
                string[] split = { "." };
                var numero = detalhe.Produto.ValorFrete.ToString().Split(split, StringSplitOptions.RemoveEmptyEntries);

                for (int x = 0; x < numero.Length; x++)
                {
                    if (x == 0)
                        entity.Fail(numero[x].Length < 1 || numero[x].Length > 15, new Error("Valor de frete inválido. (Tam. 15.2) Item: " + nItemDetalhe, "Item.Detalhes[" + (nItemDetalhe) + "].Produto.ValorFrete"));
                    else
                        entity.Fail(numero[x] != null && numero[x].Length > 2, new Error("Quantidade de casas decimais inválida. (Tam. 15.2) Item: " + nItemDetalhe, "Item.Detalhes[" + (nItemDetalhe) + "].Produto.ValorFrete"));
                }
            }
        }

        private static void ValidarLengthValorUnitario(Detalhe detalhe, TransmissaoVM entity, int nItemDetalhe)
        {
            if (!string.IsNullOrEmpty(detalhe.Produto.ValorUnitarioTributado.ToString()))
            {
                string[] split = { "." };
                var numero = detalhe.Produto.ValorUnitarioTributado.ToString().Split(split, StringSplitOptions.RemoveEmptyEntries);

                for (int x = 0; x < numero.Length; x++)
                {
                    if (x == 0)
                        entity.Fail(numero[x].Length < 1 || numero[x].Length > 21, new Error("Valor unitário tributado inválido. (Tam. 21.10) Item: " + nItemDetalhe, "Item.Detalhes[" + (nItemDetalhe) + "].Produto.ValorUnitarioTributado"));
                    else
                        entity.Fail(numero[x] != null && numero[x].Length > 10, new Error("Quantidade de casas decimais inválida. (Tam. 21.10) Item: " + nItemDetalhe, "Item.Detalhes[" + (nItemDetalhe) + "].Produto.ValorUnitarioTributado"));
                }
            }
        }

        private static void ValidarLengthQuantidadeTributada(Detalhe detalhe, TransmissaoVM entity, int nItemDetalhe)
        {
            if (!string.IsNullOrEmpty(detalhe.Produto.QuantidadeTributada.ToString()))
            {
                string[] split = { "." };
                var numero = detalhe.Produto.QuantidadeTributada.ToString().Split(split, StringSplitOptions.RemoveEmptyEntries);

                for (int x = 0; x < numero.Length; x++)
                {
                    if (x == 0)
                        entity.Fail(numero[x].Length < 1 || numero[x].Length > 15, new Error("Quantidade tributada do produto inválida. (Tam. 15.4) Item: " + nItemDetalhe, "Item.Detalhes[" + (nItemDetalhe) + "].Produto.QuantidadeTributada"));
                    else
                        entity.Fail(numero[x] != null && numero[x].Length > 4, new Error("Quantidade de casas decimais inválida. (Tam. 15.4) Item: " + nItemDetalhe, "Item.Detalhes[" + (nItemDetalhe) + "].Produto.QuantidadeTributada"));
                }
            }
        }

        private static void ValidarCodigoDeBarrasUnidadeTributacao(Detalhe detalhe, TransmissaoVM entity, int nItemDetalhe)
        {
            if (detalhe.Produto.GTIN_UnidadeMedidaTributada != null && detalhe.Produto.GTIN_UnidadeMedidaTributada.ToUpper() != "SEM GTIN")
            {
                entity.Fail(IsCodigoBarrasTributacaoInvalida(detalhe)
                , new Error("Codigo de barras (GTIN/EAN) da unidade de tributação do produto inválido. (Tam. 0/8/12/13/14) Item: " + nItemDetalhe, "Item.Detalhes[" + (nItemDetalhe) + "].Produto.GTIN_UnidadeMedidaTributada"));
            }
        }

        private static bool IsCodigoBarrasTributacaoInvalida(Detalhe detalhe)
        {
            
            return !(detalhe.Produto.GTIN_UnidadeMedidaTributada.Length == 0 ||
                   detalhe.Produto.GTIN_UnidadeMedidaTributada.Length == 8 ||
                   detalhe.Produto.GTIN_UnidadeMedidaTributada.Length == 12 ||
                   detalhe.Produto.GTIN_UnidadeMedidaTributada.Length == 13 ||
                   detalhe.Produto.GTIN_UnidadeMedidaTributada.Length == 14);
        }

        private static void ValidarLengthValorBrutoProduto(Detalhe detalhe, TransmissaoVM entity, int nItemDetalhe)
        {
            if (!string.IsNullOrEmpty(detalhe.Produto.ValorBruto.ToString()))
            {
                string[] split = { "." };
                var numero = detalhe.Produto.ValorBruto.ToString().Split(split, StringSplitOptions.RemoveEmptyEntries);

                for (int x = 0; x < numero.Length; x++)
                {
                    if (x == 0)
                        entity.Fail(numero[x].Length < 1 || numero[x].Length > 15, new Error("Valor bruto do produto inválido. (Tam. 15.2) Item: " + nItemDetalhe, "Item.Detalhes[" + (nItemDetalhe) + "].Produto.ValorBruto"));
                    else
                        entity.Fail(numero[x] != null && numero[x].Length > 2, new Error("Quantidade de casas decimais inválida. (Tam. 15.2) Item: " + nItemDetalhe, "Item.Detalhes[" + (nItemDetalhe) + "].Produto.ValorBruto"));
                }
            }
        }

        private static void ValidarLengthValorUnitarioProduto(Detalhe detalhe, TransmissaoVM entity, int nItemDetalhe)
        {
            if (!string.IsNullOrEmpty(detalhe.Produto.ValorUnitario.ToString()))
            {
                string[] split = { "." };
                var numero = detalhe.Produto.ValorUnitario.ToString().Split(split, StringSplitOptions.RemoveEmptyEntries);

                for (int x = 0; x < numero.Length; x++)
                {
                    if (x == 0)
                        entity.Fail(numero[x].Length < 1 || numero[x].Length > 21, new Error("Valor unitário do produto inválido. (Tam. 21.10) Item: " + nItemDetalhe, "Item.Detalhes[" + (nItemDetalhe) + "].Produto.ValorUnitario"));
                    else
                        entity.Fail(numero[x] != null && numero[x].Length > 10, new Error("Quantidade de casas decimais inválida. (Tam. 21.10) Item: " + nItemDetalhe, "Item.Detalhes[" + (nItemDetalhe) + "].Produto.ValorUnitario"));
                }
            }
        }

        private static void ValidarLengthQuantidadeProduto(Detalhe detalhe, TransmissaoVM entity, int nItemDetalhe)
        {
            if (!string.IsNullOrEmpty(detalhe.Produto.Quantidade.ToString()))
            {
                string[] split = { "." };
                var numero = detalhe.Produto.Quantidade.ToString().Split(split, StringSplitOptions.RemoveEmptyEntries);

                for (int x = 0; x < numero.Length; x++)
                {
                    if (x == 0)
                        entity.Fail(numero[x].Length < 1 || numero[x].Length > 15, new Error("Quantidade do produto inválida. (Tam. 15.4) Item: " + nItemDetalhe, "Item.Detalhes[" + (nItemDetalhe) + "].Produto.Quantidade"));
                    else
                        entity.Fail(numero[x] != null && numero[x].Length > 4, new Error("Quantidade de casas decimais inválida. (Tam. 15.4) Item: " + nItemDetalhe, "Item.Detalhes[" + (nItemDetalhe) + "].Produto.Quantidade"));
                }
            }
        }

        private static void ValidarUnidadeTributacao(Detalhe detalhe, TransmissaoVM entity, int nItemDetalhe)
        {
            entity.Fail(detalhe.Produto.UnidadeMedidaTributada != null && (detalhe.Produto.UnidadeMedidaTributada.Length < 1 || detalhe.Produto.UnidadeMedidaTributada.Length > 6),
                new Error("Unidade de tributação inválida. Item: " + nItemDetalhe, "Item.Detalhes[" + (nItemDetalhe) + "].Produto.UnidadeMedidaTributada"));
        }

        private static void ValidarUnidadeMedidaProduto(Detalhe detalhe, TransmissaoVM entity, int nItemDetalhe)
        {
            entity.Fail(detalhe.Produto.UnidadeMedida != null && (detalhe.Produto.UnidadeMedida.Length < 1 || detalhe.Produto.UnidadeMedida.Length > 6),
                                new Error("Unidade de medida do produto inválida. Item: " + nItemDetalhe, "Item.Detalhes[" + (nItemDetalhe) + "].Produto.UnidadeMedida"));
        }

        private static void ValidarCFOPProduto(Detalhe detalhe, EntitiesBLToValidate entitiesBLToValidate, TransmissaoVM entity, int nItemDetalhe)
        {
            entity.Fail(detalhe.Produto.CFOP.ToString().Length != 4 || string.IsNullOrEmpty(entitiesBLToValidate._cfopBL.All.Where(e => e.Codigo == detalhe.Produto.CFOP).FirstOrDefault().Codigo.ToString()),
                                new Error("CFOP do produto inválido. Item: " + nItemDetalhe, "Item.Detalhes[" + (nItemDetalhe) + "].Produto.CFOP"));
        }

        private static void ValidarLenghtNCMProduto(Detalhe detalhe, TransmissaoVM entity, int nItemDetalhe)
        {
            entity.Fail(detalhe.Produto.NCM != null && (detalhe.Produto.NCM.Length < 2 || detalhe.Produto.NCM.Length > 8),
                                new Error("NCM do produto inválido. (Tam. 2-8) Item: " + nItemDetalhe, "Item.Detalhes[" + (nItemDetalhe) + "].Produto.NCM"));
        }

        private static void ValidarLengthDescricaoProduto(Detalhe detalhe, TransmissaoVM entity, int nItemDetalhe)
        {
            entity.Fail(detalhe.Produto.Descricao != null && (detalhe.Produto.Descricao.Length < 1 || detalhe.Produto.Descricao.Length > 120),
                                new Error("Descrição do produto inválida. (Tam. 1-120) Item: " + nItemDetalhe, "Item.Detalhes[" + (nItemDetalhe) + "].Produto.Descricao"));
        }

        private static void ValidaCodigoBarrasProduto(Detalhe detalhe, TransmissaoVM entity, int nItemDetalhe)
        {
            entity.Fail(IsCodigoBarrasInvalido(detalhe)
            , new Error("Codigo de barras (GTIN/EAN) do produto inválido. (Tam. 0/8/12/13/14) Item: " + nItemDetalhe, "Item.Detalhes[" + (nItemDetalhe) + "].Produto.GTIN"));
        }

        private static bool IsCodigoBarrasInvalido(Detalhe detalhe)
        {
            return (detalhe.Produto.GTIN != null && detalhe.Produto.GTIN.ToUpper() != "SEM GTIN") &&
                !(detalhe.Produto.GTIN.Length == 0 ||
                detalhe.Produto.GTIN.Length == 8 ||
                detalhe.Produto.GTIN.Length == 12 ||
                detalhe.Produto.GTIN.Length == 13 ||
                detalhe.Produto.GTIN.Length == 14);
        }

        private static void ValidaCodigoProdutoInvalido(Detalhe detalhe, TransmissaoVM entity, int nItemDetalhe)
        {
            entity.Fail(detalhe.Produto.Codigo != null && (detalhe.Produto.Codigo.Length < 1 || detalhe.Produto.Codigo.Length > 60),
                                new Error("Código inválido do produto. (Tam. 1-60) Item: " + nItemDetalhe, "Item.Detalhes[" + (nItemDetalhe) + "].Produto.Codigo"));
        }

        private static void ValidarAgregaTotalNota(Detalhe detalhe, TransmissaoVM entity, int nItemDetalhe)
        {
            entity.Fail(string.IsNullOrEmpty(detalhe.Produto.AgregaTotalNota.ToString()),
                                new Error("Valor unitário de tributação do produto é um dado obrigatório. Item: " + nItemDetalhe, "Item.Detalhes[" + (nItemDetalhe) + "].Produto.AgregaTotalNota"));
        }

        private static void ValidarValorUnitarioDeTributacao(Detalhe detalhe, TransmissaoVM entity, int nItemDetalhe)
        {
            entity.Fail(string.IsNullOrEmpty(detalhe.Produto.ValorUnitarioTributado.ToString()),
                                new Error("Valor unitário de tributação do produto é um dado obrigatório. Item: " + nItemDetalhe, "Item.Detalhes[" + (nItemDetalhe) + "].Produto.ValorUnitarioTributado"));
        }

        private static void ValidarQuantidadeDeTributacao(Detalhe detalhe, TransmissaoVM entity, int nItemDetalhe)
        {
            entity.Fail(string.IsNullOrEmpty(detalhe.Produto.QuantidadeTributada.ToString()),
                                new Error("Quantidade de tributação do produto é um dado obrigatório. Item: " + nItemDetalhe, "Item.Detalhes[" + (nItemDetalhe) + "].Produto.QuantidadeTributada"));
        }

        private static void ValidarUnidadeDeTributacao(Detalhe detalhe, TransmissaoVM entity, int nItemDetalhe)
        {
            entity.Fail(string.IsNullOrEmpty(detalhe.Produto.UnidadeMedidaTributada),
                                new Error("Unidade de tributação do produto é um dado obrigatório. Item: " + nItemDetalhe, "Item.Detalhes[" + (nItemDetalhe) + "].Produto.UnidadeMedidaTributada"));
        }

        private static void ValidarCodigoBarrasTributacao(Detalhe detalhe, TransmissaoVM entity, int nItemDetalhe)
        {
            entity.Fail(string.IsNullOrEmpty(detalhe.Produto.GTIN_UnidadeMedidaTributada),
                                new Error("Codigo de barras (GTIN/EAN) da unidade de tributação é um dado obrigatório. Se não tiver informe SEM GTIN. Item: " + nItemDetalhe, "Item.Detalhes[" + (nItemDetalhe) + "].Produto.GTIN_UnidadeMedidaTributada"));
        }

        private static void ValidarValorBrutoProduto(Detalhe detalhe, TransmissaoVM entity, int nItemDetalhe)
        {
            entity.Fail(string.IsNullOrEmpty(detalhe.Produto.ValorBruto.ToString()),
                                new Error("Valor bruto do produto é um dado obrigatório. Item: " + nItemDetalhe, "Item.Detalhes[" + (nItemDetalhe) + "].Produto.ValorUnitario"));
        }

        private static void ValidarValorUnitarioProduto(Detalhe detalhe, TransmissaoVM entity, int nItemDetalhe)
        {
            entity.Fail(string.IsNullOrEmpty(detalhe.Produto.ValorUnitario.ToString()),
                                new Error("Valor unitário do produto é um dado obrigatório. Item: " + nItemDetalhe, "Item.Detalhes[" + (nItemDetalhe) + "].Produto.ValorUnitario"));
        }

        private static void ValidarQuantidadeProduto(Detalhe detalhe, TransmissaoVM entity, int nItemDetalhe)
        {
            entity.Fail(string.IsNullOrEmpty(detalhe.Produto.Quantidade.ToString()),
                                new Error("Quantidade do produto é um dado obrigatório. Item: " + nItemDetalhe, "Item.Detalhes[" + (nItemDetalhe) + "].Produto.Quantidade"));
        }

        private static void ValidarUnidadeMedida(Detalhe detalhe, TransmissaoVM entity, int nItemDetalhe)
        {
            entity.Fail(string.IsNullOrEmpty(detalhe.Produto.UnidadeMedida),
                                new Error("Unidade de medida do produto é um dado obrigatório. Item: " + nItemDetalhe, "Item.Detalhes[" + (nItemDetalhe) + "].Produto.UnidadeMedida"));
        }

        private static void ValidarCFOP(Detalhe detalhe, TransmissaoVM entity, int nItemDetalhe)
        {
            entity.Fail(string.IsNullOrEmpty(detalhe.Produto.CFOP.ToString()),
                                new Error("CFOP do produto é um dado obrigatório. Item: " + nItemDetalhe, "Item.Detalhes[" + (nItemDetalhe) + "].Produto.CFOP"));
        }

        private static void ValidarNCMProduto(Detalhe detalhe, TransmissaoVM entity, int nItemDetalhe)
        {
            entity.Fail(string.IsNullOrEmpty(detalhe.Produto.NCM),
                                new Error("NCM do produto é um dado obrigatório. Item: " + nItemDetalhe, "Item.Detalhes[" + (nItemDetalhe) + "].Produto.NCM"));
        }

        private static void ValidarDescricaoProduto(Detalhe detalhe, TransmissaoVM entity, int nItemDetalhe)
        {
            entity.Fail(string.IsNullOrEmpty(detalhe.Produto.Descricao),
                                new Error("Descrição do produto é um dado obrigatório. Item: " + nItemDetalhe, "Item.Detalhes[" + (nItemDetalhe) + "].Produto.Descricao"));
        }

        private static void ValidarCodigoDeBarras(Detalhe detalhe, TransmissaoVM entity, int nItemDetalhe)
        {
            entity.Fail(string.IsNullOrEmpty(detalhe.Produto.GTIN),
                                new Error("Codigo de barras (GTIN/EAN) do produto é um dado obrigatório. Se não tiver informe SEM GTIN. Item: " + nItemDetalhe, "Item.Detalhes[" + (nItemDetalhe) + "].Produto.GTIN"));
        }

        private static void ValidarCodigoProduto(Detalhe detalhe, TransmissaoVM entity, int nItemDetalhe)
        {
            entity.Fail(string.IsNullOrEmpty(detalhe.Produto.Codigo),
                                new Error("Código do produto é um dado obrigatório. Item: " + nItemDetalhe, "Item.Detalhes[" + (nItemDetalhe) + "].Produto.Codigo"));
        }
        #endregion

    }
}
