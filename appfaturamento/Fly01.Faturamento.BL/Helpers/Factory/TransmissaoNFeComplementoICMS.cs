using Fly01.Core.Entities.Domains.Commons;
using Fly01.EmissaoNFE.Domain.ViewModel;
using System;
using Fly01.Faturamento.BL.Helpers.EntitiesBL;
using Fly01.EmissaoNFE.Domain.Enums;
using Fly01.Core.Entities.Domains.Enum;
using Fly01.EmissaoNFE.Domain.Entities.NFe;
using System.Collections.Generic;
using System.Linq;
using Fly01.EmissaoNFE.Domain.Entities.NFe.COFINS;
using Fly01.EmissaoNFE.Domain.Entities.NFe.PIS;
using Fly01.EmissaoNFE.Domain.Entities.NFe.ICMS;
using Fly01.EmissaoNFE.Domain.Entities.NFe.Totais;

namespace Fly01.Faturamento.BL.Helpers.Factory
{
    public class TransmissaoNFeComplementoICMS : TransmissaoNFe
    {
        public TransmissaoNFeComplementoICMS(NFe nfe, TransmissaoBLs transmissaoBLs)
            : base(nfe, transmissaoBLs) { }

        public override TipoNota ObterTipoDocumentoFiscal()
        {
            return NFe.NFeRefComplementarIsDevolucao ? TipoNota.Entrada : TipoNota.Saida;
        }

        public override TipoFormaPagamento ObterTipoFormaPagamento()
        {
            return TipoFormaPagamento.SemPagamento;
        }

        public override TransmissaoVM ObterTransmissaoVM()
        {
            var itemTransmissao = ObterCabecalhoItemTransmissao();
            itemTransmissao.Transporte.ModalidadeFrete = TipoFrete.SemFrete;

            CriarProdutosEImpostosParaDetalhes(itemTransmissao);

            itemTransmissao.Total = ObterTotal(itemTransmissao.Detalhes);
            var icmsTotal = itemTransmissao.Total.ICMSTotal;

            itemTransmissao.Total.ICMSTotal.TotalTributosAprox = CalcularTributosAproximados(icmsTotal);
            itemTransmissao.Total.ICMSTotal.ValorTotalNF = CalcularValorTotalNFE(itemTransmissao);
            itemTransmissao.Pagamento = ObterPagamento(itemTransmissao.Total.ICMSTotal.ValorTotalNF);

            if (!string.IsNullOrEmpty(NFe.MensagemPadraoNota))
            {
                itemTransmissao.InformacoesAdicionais = new InformacoesAdicionais()
                {
                    InformacoesComplementares = NFe.Observacao + " | " + NFe.MensagemPadraoNota
                };
            }

            var transmissao = ObterTransmissaoVMApartirDoItem(itemTransmissao);
            return transmissao;
        }

        public override bool SomaFrete()
        {
            return false;
        }

        private void CriarProdutosEImpostosParaDetalhes(ItemTransmissaoVM itemTransmissao)
        {
            itemTransmissao.Detalhes = new List<Detalhe>();
            var num = 1;

            foreach (var item in ObterNFeProdutos())
            {
                var itemTributacao = new NotaFiscalItemTributacao();
                itemTributacao = TransmissaoBLs.NotaFiscalItemTributacaoBL.All.Where(x => x.NotaFiscalItemId == item.Id).FirstOrDefault();

                var detalhe = ObterDetalhe(item, num);
                detalhe.Produto.Quantidade = 0;
                detalhe.Produto.ValorDesconto = 0;
                detalhe.Produto.ValorOutrasDespesas = 0;
                detalhe.Produto.ValorUnitario = 0;
                detalhe.Produto.ValorBruto = 0;
                detalhe.Produto.QuantidadeTributada = 0;
                detalhe.Produto.ValorUnitarioTributado = 0;

                detalhe.Imposto.ICMS = ObterICMS(item, itemTributacao);
                detalhe.Imposto.PIS = ObterPIS(item);
                if (detalhe.Imposto.PIS.CodigoSituacaoTributaria.ToString() == "05")
                {
                    detalhe.Imposto.PISST = new PISST()
                    {
                        ValorPISST = itemTributacao.PISValor,
                    };
                }
                detalhe.Imposto.COFINS = ObterCOFINS(item);
                detalhe.Imposto.TotalAprox = ObterTotalAproximado(detalhe);

                itemTransmissao.Detalhes.Add(detalhe);
                num++;
            }
        }

        private double CalcularValorTotalNFE(ItemTransmissaoVM itemTransmissao)
        {
            return 0;
        }

        private double CalcularTributosAproximados(ICMSTOT icmsTotal)
        {
            return icmsTotal.SomatorioICMS
                + icmsTotal.SomatorioCofins
                + icmsTotal.SomatorioICMSST
                + icmsTotal.SomatorioIPI
                + icmsTotal.SomatorioPis
                + icmsTotal.SomatorioFCPST
                + icmsTotal.SomatorioFCP;
        }

        #region Impostos
        private void CalculaICMSPai(NFeProduto item, NotaFiscalItemTributacao itemTributacao, ICMSPai ICMS)
        {
            ICMS.ValorICMSSTRetido = Math.Round(item.ValorICMSSTRetido, 2);

            if (item.GrupoTributario.TipoTributacaoICMS == TipoTributacaoICMS.Outros|| item.GrupoTributario.TipoTributacaoICMS == TipoTributacaoICMS.Outros90)
            {
                ICMS.ModalidadeBC = ModalidadeDeterminacaoBCICMS.ValorDaOperacao;
                ICMS.AliquotaICMS = Math.Round(itemTributacao.ICMSAliquota, 2);
                ICMS.ModalidadeBCST = ModalidadeDeterminacaoBCICMSST.MargemValorAgregado;
                ICMS.ValorBC = Math.Round(itemTributacao.ICMSBase, 2);
                ICMS.ValorICMS = Math.Round(itemTributacao.ICMSValor, 2);
            }

            if (item.GrupoTributario.TipoTributacaoICMS == TipoTributacaoICMS.TributadaComPermissaoDeCreditoST
                || item.GrupoTributario.TipoTributacaoICMS == TipoTributacaoICMS.TributadaSemPermissaoDeCreditoST
                || item.GrupoTributario.TipoTributacaoICMS == TipoTributacaoICMS.IsencaoParaFaixaDeReceitaBrutaST
                || item.GrupoTributario.TipoTributacaoICMS == TipoTributacaoICMS.ComRedDeBaseDeST
                || item.GrupoTributario.TipoTributacaoICMS == TipoTributacaoICMS.TributadaComCobrancaDeSubstituicao
                || item.GrupoTributario.TipoTributacaoICMS == TipoTributacaoICMS.TributadaIntegralmente
                || item.GrupoTributario.TipoTributacaoICMS == TipoTributacaoICMS.ComReducaoDeBaseDeCalculo
                )
            {
                ICMS.ModalidadeBCST = ModalidadeDeterminacaoBCICMSST.MargemValorAgregado;
                ICMS.PercentualReducaoBCST = 0;
            }

            if (item.GrupoTributario.TipoTributacaoICMS == TipoTributacaoICMS.ComRedDeBaseDeST
                    || item.GrupoTributario.TipoTributacaoICMS == TipoTributacaoICMS.ComReducaoDeBaseDeCalculo)
            {
                ICMS.PercentualReducaoBC = item.PercentualReducaoBC;
                ICMS.PercentualReducaoBCST = item.PercentualReducaoBCST;
            }

            if (Cabecalho.Versao == "4.00")
            {
                if (
                     item.GrupoTributario.TipoTributacaoICMS == TipoTributacaoICMS.ComReducaoDeBaseDeCalculo
                     || item.GrupoTributario.TipoTributacaoICMS == TipoTributacaoICMS.Diferimento
                     || item.GrupoTributario.TipoTributacaoICMS == TipoTributacaoICMS.Outros90
                     || item.GrupoTributario.TipoTributacaoICMS == TipoTributacaoICMS.TributadaIntegralmente)
                {
                    ICMS.BaseFCP = Math.Round(itemTributacao.FCPBase, 2);
                    ICMS.AliquotaFCP = Math.Round(itemTributacao.FCPAliquota, 2);
                    ICMS.ValorFCP = Math.Round(itemTributacao.FCPValor, 2);
                }
            }
        }

        private ICMSPai ObterICMS(NFeProduto item, NotaFiscalItemTributacao itemTributacao)
        {
            var ICMS = new ICMSPai()
            {
                OrigemMercadoria = item.Produto.OrigemMercadoria,
                AliquotaAplicavelCalculoCreditoSN = Math.Round(((item.ValorCreditoICMS / (item.Quantidade * item.Valor)) * 100), 2),
                ValorCreditoICMS = Math.Round(item.ValorCreditoICMS, 2),
                CodigoSituacaoOperacao = item.GrupoTributario.TipoTributacaoICMS != null ? item.GrupoTributario.TipoTributacaoICMS.Value : TipoTributacaoICMS.TributadaSemPermissaoDeCredito,
                TipoCRT = ParametrosTributarios.TipoCRT,
            };

            CalculaICMSPai(item, itemTributacao, ICMS);

            return ICMS;
        }

        private PISPai ObterPIS(NFeProduto item)
        {
            return new PISPai()
            {
                CodigoSituacaoTributaria = item.GrupoTributario.TipoTributacaoPIS.HasValue
                    ? (CSTPISCOFINS)((int)item.GrupoTributario.TipoTributacaoPIS)
                    : CSTPISCOFINS.IsentaDaContribuicao
            };
        }

        private COFINSPai ObterCOFINS(NFeProduto item)
        {
            return new COFINSPai()
            {
                CodigoSituacaoTributaria = item.GrupoTributario.TipoTributacaoCOFINS != null
                    ? ((CSTPISCOFINS)(int)item.GrupoTributario.TipoTributacaoCOFINS.Value)
                    : CSTPISCOFINS.OutrasOperacoes
            };
        }
        #endregion
    }
}
