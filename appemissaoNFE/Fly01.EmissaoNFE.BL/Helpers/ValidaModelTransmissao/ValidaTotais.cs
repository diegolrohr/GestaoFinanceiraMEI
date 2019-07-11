using Fly01.Core.Entities.Domains.Enum;
using Fly01.Core.Notifications;
using Fly01.EmissaoNFE.Domain.ViewModel;
using System;
using System.Linq;

namespace Fly01.EmissaoNFE.BL.Helpers.ValidaModelTransmissao
{
    public static class ValidaTotais
    {
        public static void ExecutarValidaTotais(ItemTransmissaoVM item, EntitiesBLToValidate entitiesBLToValidate, TransmissaoVM entity)
        {
            #region Validação da classe Totais

            if (item.Total == null)
                entity.Fail(true, new Error("Os dados de totais são obrigatórios.", "Item.Total"));
            else
            {
                if (item.Total.ICMSTotal == null)
                    entity.Fail(true, new Error("Os dados de ICMSTotal são obrigatórios", "Item.Total.ICMSTotal"));
                else
                {
                    //ValidarSomatorioBaseCalculo(item, entity);
                    //ValidarSomatorioICMS(item, entity);
                    //ValidarSomatorioBaseCalculoST(item, entity);
                    //ValidarSomatorioICMSST(item, entity);
                    //ValidarSomatorioProdutos(item, entity);
                    //ValidarValorFrete(item, entity);
                    //ValidarValorSeguro(item, entity);
                    //ValidarSomatorioDesconto(item, entity);
                    //ValidarSOmatorioImpostoImportacao(item, entity);
                    //ValidarSomatorioIPI(item, entity);
                    //ValidarSomatorioIPIDevolucao(item, entity);
                    //ValidarSomatorioPIS(item, entity);
                    //ValidarSomatorioConfins(item, entity);
                    //ValidarSomatorioOutros(item, entity);
                    //ValidarSomatorioFCPST(item, entity);
                    //ValidarSOmatorioFCPRetido(item, entity);
                    ValidarValorTotalNF(item, entity);
                }
            }

            #endregion Validação da classe Totais
        }

        private static void ValidarValorTotalNF(ItemTransmissaoVM item, TransmissaoVM entity)
        {
            entity.Fail(string.IsNullOrEmpty(item.Total.ICMSTotal.ValorTotalNF.ToString()),
                new Error("Informe o valor total da NFE.", "Item.Total.ICMSTotal.ValorTotalNF"));
        }

        private static void ValidarSOmatorioFCPRetido(ItemTransmissaoVM item, TransmissaoVM entity)
        {
            double somatorioFCPSTRetidoTrue = item.Detalhes.Sum(e => e.Imposto.ICMS.ValorFCPSTRetido ?? 0);
            item.Total.ICMSTotal.SomatorioFCPSTRetido = Math.Round(item.Total.ICMSTotal.SomatorioFCPSTRetido, 2);
            somatorioFCPSTRetidoTrue = Math.Round(somatorioFCPSTRetidoTrue, 2);

            entity.Fail(string.IsNullOrEmpty(item.Total.ICMSTotal.SomatorioFCPSTRetido.ToString()),
                new Error("Informe o somatório do valor de FCP ST Retido.", "Item.Total.ICMSTotal.SomatorioFCPSTRetido"));
            entity.Fail(!somatorioFCPSTRetidoTrue.Equals(item.Total.ICMSTotal.SomatorioFCPSTRetido),
                new Error("O somatório do valor de FCP ST Retido não confere com os valores informados.", "Item.Total.ICMSTotal.SomatorioFCPSTRetido"));
        }

        private static void ValidarSomatorioFCPST(ItemTransmissaoVM item, TransmissaoVM entity)
        {
            double somatorioFCPSTTrue = item.Detalhes.Sum(e => e.Imposto.ICMS.ValorFCPST ?? 0);
            item.Total.ICMSTotal.SomatorioFCPST = Math.Round(item.Total.ICMSTotal.SomatorioFCPST, 2);
            somatorioFCPSTTrue = Math.Round(somatorioFCPSTTrue, 2);

            entity.Fail(string.IsNullOrEmpty(item.Total.ICMSTotal.SomatorioFCPST.ToString()),
                new Error("Informe o somatório do valor de FCP ST.", "Item.Total.ICMSTotal.SomatorioFCPST"));
            entity.Fail(!somatorioFCPSTTrue.Equals(item.Total.ICMSTotal.SomatorioFCPST),
                new Error("O somatório do valor de FCP ST não confere com os valores informados.", "Item.Total.ICMSTotal.SomatorioFCPST"));
        }

        private static void ValidarSomatorioOutros(ItemTransmissaoVM item, TransmissaoVM entity)
        {
            double somatorioOutroTrue = item.Detalhes.Sum(e => e.Produto.ValorOutrasDespesas ?? 0);
            item.Total.ICMSTotal.SomatorioOutro = Arredondar(item.Total.ICMSTotal.SomatorioOutro, 2);
            somatorioOutroTrue = Arredondar(somatorioOutroTrue, 2);

            entity.Fail(string.IsNullOrEmpty(item.Total.ICMSTotal.SomatorioOutro.ToString()),
                new Error("Informe o somatório de valor de Outros.", "Item.Total.ICMSTotal.SomatorioOutro"));
            entity.Fail(!somatorioOutroTrue.Equals(item.Total.ICMSTotal.SomatorioOutro),
                new Error("O somatório do valor de Outros não confere com os valores informados.", "Item.Total.ICMSTotal.SomatorioOutro"));
        }

        private static void ValidarSomatorioConfins(ItemTransmissaoVM item, TransmissaoVM entity)
        {
            double somatorioCofinsTrue = item.Detalhes.Sum(e => e.Imposto.COFINS != null ? e.Imposto.COFINS.ValorCOFINS : 0);
            item.Total.ICMSTotal.SomatorioCofins = Arredondar(item.Total.ICMSTotal.SomatorioCofins, 2);
            somatorioCofinsTrue = Arredondar(somatorioCofinsTrue, 2);

            entity.Fail(string.IsNullOrEmpty(item.Total.ICMSTotal.SomatorioCofins.ToString()),
                new Error("Informe o somatório do valor de COFINS.", "Item.Total.ICMSTotal.SomatorioCofins"));
            entity.Fail(!somatorioCofinsTrue.Equals(item.Total.ICMSTotal.SomatorioCofins),
                new Error("O somatório do valor de COFINS não confere com os valores informados.", "Item.Total.ICMSTotal.SomatorioCofins"));
        }

        private static void ValidarSomatorioPIS(ItemTransmissaoVM item, TransmissaoVM entity)
        {
            double somatorioPISTrue = item.Detalhes.Sum(e => e.Imposto.PIS != null ? e.Imposto.PIS.ValorPIS : 0);
            item.Total.ICMSTotal.SomatorioPis = Arredondar(item.Total.ICMSTotal.SomatorioPis, 2);
            somatorioPISTrue = Arredondar(somatorioPISTrue, 2);

            entity.Fail(string.IsNullOrEmpty(item.Total.ICMSTotal.SomatorioPis.ToString()),
                new Error("Informe o somatório do valor de PIS.", "Item.Total.ICMSTotal.SomatorioPis"));
            entity.Fail(!somatorioPISTrue.Equals(item.Total.ICMSTotal.SomatorioPis),
                new Error("O somatório do valor de PIS não confere com os valores informados.", "Item.Total.ICMSTotal.SomatorioPis"));
        }

        private static void ValidarSomatorioIPIDevolucao(ItemTransmissaoVM item, TransmissaoVM entity)
        {
            double somatorioIPIDevolucaoTrue = item.Detalhes.Sum(e => e.Imposto.IPI != null ? e.Imposto.IPI.ValorIPIDevolucao : 0);
            item.Total.ICMSTotal.SomatorioIPIDevolucao = Arredondar(item.Total.ICMSTotal.SomatorioIPIDevolucao, 2);
            somatorioIPIDevolucaoTrue = Arredondar(somatorioIPIDevolucaoTrue, 2);

            entity.Fail(string.IsNullOrEmpty(item.Total.ICMSTotal.SomatorioIPIDevolucao.ToString()),
                new Error("Informe o somatório do valor de IPI de devolucao.", "Item.Total.ICMSTotal.SomatorioIPIDevolucao"));
            entity.Fail(!somatorioIPIDevolucaoTrue.Equals(item.Total.ICMSTotal.SomatorioIPIDevolucao),
                new Error("O somatório do valor de IPI de devolução não confere com os valores informados.", "Item.Total.ICMSTotal.SomatorioIPIDevolucao"));
        }

        private static void ValidarSomatorioIPI(ItemTransmissaoVM item, TransmissaoVM entity)
        {
            double somatorioIPITrue = item.Detalhes.Sum(e => e.Imposto.IPI != null ? e.Imposto.IPI.ValorIPI : 0);
            item.Total.ICMSTotal.SomatorioIPI = Arredondar(item.Total.ICMSTotal.SomatorioIPI, 2);
            somatorioIPITrue = Arredondar(somatorioIPITrue, 2);

            entity.Fail(string.IsNullOrEmpty(item.Total.ICMSTotal.SomatorioIPI.ToString()),
                new Error("Informe o somatório do valor de IPI.", "Item.Total.ICMSTotal.SomatorioIPI"));
            entity.Fail(!somatorioIPITrue.Equals(item.Total.ICMSTotal.SomatorioIPI),
                new Error("O somatório do valor de IPI não confere com os valores informados.", "Item.Total.ICMSTotal.SomatorioIPI"));
        }

        private static void ValidarSOmatorioImpostoImportacao(ItemTransmissaoVM item, TransmissaoVM entity)
        {
            double? somatorioIITrue = item.Detalhes.Sum(e => e.Imposto.II != null ? e.Imposto.II.ValorII : 0);
            item.Total.ICMSTotal.SomatorioII = Arredondar(item.Total.ICMSTotal.SomatorioII, 2);
            somatorioIITrue = Arredondar(somatorioIITrue, 2);

            entity.Fail(string.IsNullOrEmpty(item.Total.ICMSTotal.SomatorioII.ToString()),
                new Error("Informe o somatório do valor de II.", "Item.Total.ICMSTotal.SomatorioII"));
            entity.Fail(!somatorioIITrue.Equals(item.Total.ICMSTotal.SomatorioII),
                new Error("O somatório do valor de II não confere com os valores informados.", "Item.Total.ICMSTotal.SomatorioII"));
        }

        private static void ValidarSomatorioDesconto(ItemTransmissaoVM item, TransmissaoVM entity)
        {
            double? somatorioDesconto = item.Detalhes.Sum(e => e.Produto.ValorDesconto ?? 0);
            item.Total.ICMSTotal.SomatorioDesconto = Arredondar(item.Total.ICMSTotal.SomatorioDesconto, 2);
            somatorioDesconto = Arredondar(somatorioDesconto, 2);

            entity.Fail(string.IsNullOrEmpty(item.Total.ICMSTotal.SomatorioDesconto.ToString()),
                new Error("Informe o somatório do valor de desconto.", "Item.Total.ICMSTotal.SomatorioDesconto"));
            entity.Fail(!somatorioDesconto.Equals(item.Total.ICMSTotal.SomatorioDesconto),
                new Error("O somatório do valor de desconto não confere com os valores informados.", "Item.Total.ICMSTotal.SomatorioDesconto"));
        }

        private static void ValidarValorSeguro(ItemTransmissaoVM item, TransmissaoVM entity)
        {
            double? valorSeguroTrue = item.Detalhes.Sum(e => e.Produto.ValorSeguro ?? 0);
            item.Total.ICMSTotal.ValorSeguro = Arredondar(item.Total.ICMSTotal.ValorSeguro, 2);
            valorSeguroTrue = Arredondar(valorSeguroTrue, 2);

            entity.Fail(string.IsNullOrEmpty(item.Total.ICMSTotal.ValorSeguro.ToString()),
                new Error("Informe o somatório do valor do seguro.", "Item.Total.ICMSTotal.ValorSeguro"));
            entity.Fail(!valorSeguroTrue.Equals(item.Total.ICMSTotal.ValorSeguro),
                new Error("O somatório do valor do seguro não confere com os valores informados.", "Item.Total.ICMSTotal.ValorSeguro"));
        }

        private static void ValidarValorFrete(ItemTransmissaoVM item, TransmissaoVM entity)
        {
            double? valorFreteTrue = Math.Round(item.Detalhes.Sum(e => e.Produto.ValorFrete ?? 0), 2);
            item.Total.ICMSTotal.ValorFrete = Arredondar(item.Total.ICMSTotal.ValorFrete, 2);
            valorFreteTrue = Arredondar(valorFreteTrue, 2);

            entity.Fail(string.IsNullOrEmpty(item.Total.ICMSTotal.ValorFrete.ToString()),
                new Error("Informe o somatório do valor de frete.", "Item.Total.ICMSTotal.ValorFrete"));
            entity.Fail(!valorFreteTrue.Equals(item.Total.ICMSTotal.ValorFrete),
                new Error("O somatório do valor de frete não confere com os valores informados.", "Item.Total.ICMSTotal.ValorFrete"));
        }

        private static void ValidarSomatorioProdutos(ItemTransmissaoVM item, TransmissaoVM entity)
        {
            double? somatorioProdutosTrue = item.Detalhes.Sum(e => e.Produto.ValorBruto);
            item.Total.ICMSTotal.SomatorioProdutos = Arredondar(item.Total.ICMSTotal.SomatorioProdutos, 2);
            somatorioProdutosTrue = Arredondar(somatorioProdutosTrue, 2);

            entity.Fail(string.IsNullOrEmpty(item.Total.ICMSTotal.SomatorioProdutos.ToString()),
                new Error("Informe o somatório do valor bruto dos produtos.", "Item.Total.ICMSTotal.SomatorioProdutos"));
            entity.Fail(!(somatorioProdutosTrue.Value == item.Total.ICMSTotal.SomatorioProdutos),
                new Error("O somatório do valor bruto dos produtos não confere com os valores informados.", "Item.Total.ICMSTotal.SomatorioProdutos"));
        }

        private static void ValidarSomatorioICMSST(ItemTransmissaoVM item, TransmissaoVM entity)
        {
            var CSTsICMSST = "201||202||203||900||10||30||70||90";
            double? somatorioICMSSTTrue = item.Detalhes.Where(x => CSTsICMSST.Contains(((int)x.Imposto.ICMS.CodigoSituacaoOperacao).ToString())).Sum(e => e.Imposto.ICMS.ValorICMSST ?? 0);
            item.Total.ICMSTotal.SomatorioICMSST = Arredondar(item.Total.ICMSTotal.SomatorioICMSST, 2);
            somatorioICMSSTTrue = Arredondar(somatorioICMSSTTrue, 2);

            entity.Fail(string.IsNullOrEmpty(item.Total.ICMSTotal.SomatorioICMSST.ToString()),
                new Error("Informe o somatório da ICMSST.", "Item.Total.ICMSTotal.SomatorioICMSST"));
            entity.Fail(!somatorioICMSSTTrue.Equals(item.Total.ICMSTotal.SomatorioICMSST),
                new Error("O somatório da ICMSST não confere com os valores informados.", "Item.Total.ICMSTotal.SomatorioICMSST"));
        }

        private static void ValidarSomatorioBaseCalculoST(ItemTransmissaoVM item, TransmissaoVM entity)
        {
            double? somatorioBCSTTrue = item.Detalhes.Where(x => 
                x.Imposto.ICMS.CodigoSituacaoOperacao == TipoTributacaoICMS.TributadaSemPermissaoDeCreditoST ||
                x.Imposto.ICMS.CodigoSituacaoOperacao == TipoTributacaoICMS.Outros ||
                x.Imposto.ICMS.CodigoSituacaoOperacao == TipoTributacaoICMS.TributadaComPermissaoDeCreditoST ||
                x.Imposto.ICMS.CodigoSituacaoOperacao == TipoTributacaoICMS.IsencaoParaFaixaDeReceitaBrutaST ||
                x.Imposto.ICMS.CodigoSituacaoOperacao == TipoTributacaoICMS.ComRedDeBaseDeST ||
                x.Imposto.ICMS.CodigoSituacaoOperacao == TipoTributacaoICMS.TributadaComCobrancaDeSubstituicao ||
                x.Imposto.ICMS.CodigoSituacaoOperacao == TipoTributacaoICMS.Outros90 
            ).Sum(e => e.Imposto.ICMS.ValorBCST ?? 0);
            item.Total.ICMSTotal.SomatorioBCST = Arredondar(item.Total.ICMSTotal.SomatorioBCST, 2);
            somatorioBCSTTrue = Arredondar(somatorioBCSTTrue, 2);

            entity.Fail(string.IsNullOrEmpty(item.Total.ICMSTotal.SomatorioBCST.ToString()),
                new Error("Informe o somatório da BCST.", "Item.Total.ICMSTotal.SomatorioBCST"));
            entity.Fail(!somatorioBCSTTrue.Equals(item.Total.ICMSTotal.SomatorioBCST),
                new Error("O somatório da BCST não confere com os valores informados.", "Item.Total.ICMSTotal.SomatorioBCST"));
        }

        private static void ValidarSomatorioICMS(ItemTransmissaoVM item, TransmissaoVM entity)
        {
            double? somatorioICMSTrue = item.Detalhes.Sum(e => e.Imposto.ICMS.ValorICMS ?? 0);
            item.Total.ICMSTotal.SomatorioICMS = Arredondar(item.Total.ICMSTotal.SomatorioICMS, 2);
            somatorioICMSTrue = Arredondar(somatorioICMSTrue, 2);

            entity.Fail(string.IsNullOrEmpty(item.Total.ICMSTotal.SomatorioICMS.ToString()),
                new Error("Informe o somatório da ICMS.", "Item.Total.ICMSTotal.SomatorioICMS"));
            entity.Fail(!somatorioICMSTrue.Equals(item.Total.ICMSTotal.SomatorioICMS),
                new Error("O somatório da ICMS não confere com os valores informados.", "Item.Total.ICMSTotal.SomatorioICMS"));
        }

        private static void ValidarSomatorioBaseCalculo(ItemTransmissaoVM item, TransmissaoVM entity)
        {
            double? somatorioBCTrue = item.Detalhes.Sum(e => e.Imposto.ICMS.ValorBC ?? 0);
            item.Total.ICMSTotal.SomatorioBC = Arredondar(item.Total.ICMSTotal.SomatorioBC, 2);
            somatorioBCTrue = Arredondar(somatorioBCTrue, 2);

            entity.Fail(string.IsNullOrEmpty(item.Total.ICMSTotal.SomatorioBC.ToString()),
                new Error("Informe o somatório da BC do ICMS.", "Item.Total.ICMSTotal.SomatorioBC"));
            entity.Fail(!somatorioBCTrue.Equals(item.Total.ICMSTotal.SomatorioBC),
                new Error("O somatório da BC do ICMS não confere com os valores informados.", "Item.Total.ICMSTotal.SomatorioBC"));
        }

        private static double Arredondar(double? valor, int casas)
        {
            double retorno = valor ?? 0;
            if (valor.HasValue)
            {
                string[] split = { "." };
                var numero = valor.ToString().Replace(",", ".").Split(split, StringSplitOptions.RemoveEmptyEntries);

                for (int x = 0; x < numero.Length; x++)
                {
                    if (x == 0)
                        continue;
                    else
                        retorno = Math.Round(valor.Value, casas);
                }
            }
            return retorno;
        }
    }
}
