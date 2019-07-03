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
using System.Data.Entity;

namespace Fly01.Faturamento.BL.Helpers.Factory
{
    public class TransmissaoNFeComplementoICMSST : TransmissaoNFe
    {
        public TransmissaoNFeComplementoICMSST(NFe nfe, TransmissaoBLs transmissaoBLs)
            : base(nfe, transmissaoBLs) { }

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
                detalhe.Imposto.COFINS = ObterCOFINS(item);
                detalhe.Imposto.TotalAprox = ObterTotalAproximado(detalhe);

                itemTransmissao.Detalhes.Add(detalhe);
                num++;
            }
        }

        private double CalcularValorTotalNFE(ItemTransmissaoVM itemTransmissao)
        {
            return (itemTransmissao.Total.ICMSTotal.SomatorioICMSST + itemTransmissao.Total.ICMSTotal.SomatorioFCPST);
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
        private void CalculaSTICMSPai(NFeProduto item, NotaFiscalItemTributacao itemTributacao, ICMSPai ICMS)
        {
            if (itemTributacao.CalculaST)
            {
                string UFSiglaEmpresa = (Cabecalho.Empresa.Cidade != null ? (Cabecalho.Empresa.Cidade.Estado != null ? Cabecalho.Empresa.Cidade.Estado.Sigla : "") : "");
                var st = TransmissaoBLs.SubstituicaoTributariaBL.AllIncluding(y => y.EstadoOrigem).AsNoTracking().Where(x =>
                    x.NcmId == (item.Produto.NcmId ?? Guid.NewGuid()) &&
                    ((item.Produto.CestId.HasValue && x.CestId == item.Produto.CestId.Value) || !item.Produto.CestId.HasValue) &&
                    x.EstadoOrigem.Sigla == UFSiglaEmpresa &&
                    x.EstadoDestinoId == Cabecalho.Cliente.EstadoId &&
                    x.TipoSubstituicaoTributaria == TipoSubstituicaoTributaria.Saida
                    ).FirstOrDefault();
                //TODO: colocar cst fixo? ver Wilson
                var CST = item.GrupoTributario.TipoTributacaoPIS.HasValue ? item.GrupoTributario.TipoTributacaoPIS.Value.ToString() : "";

                ICMS.UF = Cabecalho.Cliente.Estado?.Sigla;
                ICMS.PercentualMargemValorAdicionadoST = st != null ? st.Mva : 0;
                ICMS.ValorBCST = Math.Round(itemTributacao.STBase, 2);
                ICMS.AliquotaICMSST = Math.Round(itemTributacao.STAliquota, 2);
                ICMS.ValorICMSST = Math.Round(itemTributacao.STValor, 2);
                ICMS.ValorBCSTRetido = Math.Round(item.ValorBCSTRetido, 2);

                if (Cabecalho.Versao == "4.00")
                {
                    // FCP (201, 202, 203 e 900)
                    ICMS.BaseFCPST = Math.Round(itemTributacao.FCPSTBase, 2);
                    ICMS.AliquotaFCPST = Math.Round(itemTributacao.FCPSTAliquota, 2);
                    ICMS.ValorFCPST = Math.Round(itemTributacao.FCPSTValor, 2);
                    // FCP (500)
                    var AliquotaFCPSTRetido = item.ValorBCFCPSTRetidoAnterior > 0 ? Math.Round(((item.ValorFCPSTRetidoAnterior / item.ValorBCFCPSTRetidoAnterior) * 100), 2) : 0;
                    ICMS.BaseFCPSTRetido = Math.Round(item.ValorBCFCPSTRetidoAnterior, 2);
                    ICMS.AliquotaFCPSTRetido = AliquotaFCPSTRetido;
                    ICMS.ValorFCPSTRetido = Math.Round(item.ValorFCPSTRetidoAnterior, 2);
                    ICMS.AliquotaConsumidorFinal = itemTributacao.STAliquota > 0 ? Math.Round(itemTributacao.STAliquota, 2) + AliquotaFCPSTRetido : 0;
                }
            }
        }

        private void CalculaICMSPai(NFeProduto item, NotaFiscalItemTributacao itemTributacao, ICMSPai ICMS)
        {
            if (itemTributacao.CalculaICMS)
            {
                ICMS.ValorICMSSTRetido = Math.Round(item.ValorICMSSTRetido, 2);

                if (item.GrupoTributario.TipoTributacaoICMS == TipoTributacaoICMS.Outros)
                {
                    ICMS.ModalidadeBC = ModalidadeDeterminacaoBCICMS.ValorDaOperacao;
                    ICMS.AliquotaICMS = Math.Round(itemTributacao.ICMSAliquota, 2);
                    ICMS.ModalidadeBCST = ModalidadeDeterminacaoBCICMSST.MargemValorAgregado;
                    ICMS.ValorICMS = Math.Round(itemTributacao.ICMSValor, 2);
                    ICMS.ValorBC = Math.Round(itemTributacao.ICMSBase, 2);
                }

                if (item.GrupoTributario.TipoTributacaoICMS == TipoTributacaoICMS.TributadaComPermissaoDeCreditoST
                    || item.GrupoTributario.TipoTributacaoICMS == TipoTributacaoICMS.TributadaSemPermissaoDeCreditoST
                    || item.GrupoTributario.TipoTributacaoICMS == TipoTributacaoICMS.IsencaoParaFaixaDeReceitaBrutaST)
                {
                    ICMS.ModalidadeBCST = ModalidadeDeterminacaoBCICMSST.MargemValorAgregado;
                    ICMS.PercentualReducaoBCST = 0;
                }
            }
        }

        private ICMSPai ObterICMS(NFeProduto item, NotaFiscalItemTributacao itemTributacao)
        {
            var ICMS = new ICMSPai()
            {
                OrigemMercadoria = item.Produto.OrigemMercadoria,
                //AliquotaAplicavelCalculoCreditoSN = Math.Round(((item.ValorCreditoICMS / (item.Quantidade * item.Valor)) * 100), 2),
                //ValorCreditoICMS = Math.Round(item.ValorCreditoICMS, 2),
                CodigoSituacaoOperacao = TipoTributacaoICMS.TributadaSemPermissaoDeCredito, //TODO: ver Wilson
                TipoCRT = ParametrosTributarios.TipoCRT,

            };
            CalculaICMSPai(item, itemTributacao, ICMS);

            CalculaSTICMSPai(item, itemTributacao, ICMS);

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
