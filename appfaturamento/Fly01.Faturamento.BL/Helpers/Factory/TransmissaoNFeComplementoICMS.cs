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
            : base(nfe, transmissaoBLs) {}

        public override TipoNota ObterTipoDocumentoFiscal()
        {
            return NFe.NFeRefComplementarIsDevolucao ? TipoNota.Entrada : TipoNota.Saida;
        }

        public override TipoFormaPagamento ObterTipoFormaPagamento()
        {
            var tipoFormaPagamento = TipoFormaPagamento.Outros;
            if (Cabecalho.FormaPagamento != null)
            {
                //Transferência não existe para o SEFAZ
                tipoFormaPagamento = Cabecalho.FormaPagamento.TipoFormaPagamento == TipoFormaPagamento.Transferencia ? TipoFormaPagamento.Outros : Cabecalho.FormaPagamento.TipoFormaPagamento;
            }
            return tipoFormaPagamento;
        }

        public override TransmissaoVM ObterTransmissaoVM()
        {
            var itemTransmissao = ObterCabecalhoItemTransmissao();
            itemTransmissao.Transporte.ModalidadeFrete = ModalidadeFrete.SemFrete;

            CriarProdutosEImpostosParaDetalhes(itemTransmissao);

            itemTransmissao.Total = ObterTotal(itemTransmissao.Detalhes);
            itemTransmissao.Total.ICMSTotal.ValorTotalNF = CalcularValorTotalNFE(itemTransmissao);
            itemTransmissao.Pagamento = ObterPagamento(itemTransmissao.Total.ICMSTotal.ValorTotalNF);

            if (!string.IsNullOrEmpty(NFe.MensagemPadraoNota))
            {
                itemTransmissao.InformacoesAdicionais = new InformacoesAdicionais()
                {
                    InformacoesComplementares = NFe.MensagemPadraoNota
                };
            }

            var transmissao = ObterTransmissaoVMApartirDoItem(itemTransmissao);
            return transmissao;
        }

        public override bool PagaFrete()
        {
            return false;
        }

        private void CriarProdutosEImpostosParaDetalhes(ItemTransmissaoVM itemTransmissao)
        {
            itemTransmissao.Detalhes = new List<Detalhe>();
            var num = 1;

            foreach (var item in ObterNFeProdutos())
            {
                var detalhe = ObterDetalhe(item, num);
                detalhe.Produto.Quantidade = 0;
                detalhe.Produto.ValorUnitario = 0;
                detalhe.Produto.ValorBruto = 0;
                detalhe.Produto.QuantidadeTributada = 0;
                detalhe.Produto.ValorUnitarioTributado = 0;

                detalhe.Imposto.ICMS = ObterICMS(item);
                detalhe.Imposto.PIS = ObterPIS(item);
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
                + icmsTotal.SomatorioFCPST;
        }

        #region Impostos
        private void CalculaICMSPai(NFeProduto item, ICMSPai ICMS)
        {
            //Ver tipo do ICMS
            //if (itemTributacao.CalculaICMS)
            //{
            //    ICMS.ValorICMSSTRetido = Math.Round(item.ValorICMSSTRetido, 2);
            //    ICMS.ValorICMS = Math.Round(itemTributacao.ICMSValor, 2);
            //    ICMS.ValorBC = Math.Round(itemTributacao.ICMSBase, 2);

            //    if (item.GrupoTributario.TipoTributacaoICMS == TipoTributacaoICMS.Outros)
            //    {
            //        ICMS.ModalidadeBC = ModalidadeDeterminacaoBCICMS.ValorDaOperacao;
            //        ICMS.AliquotaICMS = Math.Round(itemTributacao.ICMSAliquota, 2);
            //        ICMS.ModalidadeBCST = ModalidadeDeterminacaoBCICMSST.MargemValorAgregado;
            //    }

            //    if (item.GrupoTributario.TipoTributacaoICMS == TipoTributacaoICMS.TributadaComPermissaoDeCreditoST
            //        || item.GrupoTributario.TipoTributacaoICMS == TipoTributacaoICMS.TributadaSemPermissaoDeCreditoST
            //        || item.GrupoTributario.TipoTributacaoICMS == TipoTributacaoICMS.IsencaoParaFaixaDeReceitaBrutaST)
            //    {
            //        ICMS.ModalidadeBCST = ModalidadeDeterminacaoBCICMSST.MargemValorAgregado;
            //        ICMS.PercentualReducaoBCST = 0;
            //    }
            //}
        }

        private ICMSPai ObterICMS(NFeProduto item)
        {
            var ICMS = new ICMSPai()
            {
                OrigemMercadoria = OrigemMercadoria.Nacional,
                //AliquotaAplicavelCalculoCreditoSN = Math.Round(((item.ValorCreditoICMS / item.Total) * 100), 2),
                //ValorCreditoICMS = Math.Round(item.ValorCreditoICMS, 2),
                CodigoSituacaoOperacao = TipoTributacaoICMS.TributadaSemPermissaoDeCredito//TODO: ver Wilson
            };

            CalculaICMSPai(item, ICMS);

            return ICMS;
        }

        private PISPai ObterPIS(NFeProduto item)
        {
            return new PISPai()
            {
                CodigoSituacaoTributaria = CSTPISCOFINS.IsentaDaContribuicao //TODO: Wilson confirmar SemIncidencia
            };
        }

        private COFINSPai ObterCOFINS(NFeProduto item)
        {
            return new COFINSPai()
            {
                //Sem incidencia? TODO: Wilson
                CodigoSituacaoTributaria = NFe.NFeRefComplementarIsDevolucao ? CSTPISCOFINS.OutrasOperacoesDeEntrada : CSTPISCOFINS.OutrasOperacoesDeSaida
            };            
        }
        #endregion
    }
}
