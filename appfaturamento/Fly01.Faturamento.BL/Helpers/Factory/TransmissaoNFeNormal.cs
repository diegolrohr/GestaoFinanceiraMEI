using Fly01.Core.Entities.Domains.Commons;
using Fly01.Core.Entities.Domains.Enum;
using Fly01.EmissaoNFE.Domain.Entities.NFe;
using Fly01.EmissaoNFE.Domain.Entities.NFe.COFINS;
using Fly01.EmissaoNFE.Domain.Entities.NFe.ICMS;
using Fly01.EmissaoNFE.Domain.Entities.NFe.IPI;
using Fly01.EmissaoNFE.Domain.Entities.NFe.PIS;
using Fly01.EmissaoNFE.Domain.Entities.NFe.Totais;
using Fly01.EmissaoNFE.Domain.Enums;
using Fly01.EmissaoNFE.Domain.ViewModel;
using Fly01.Faturamento.BL.Helpers.EntitiesBL;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Fly01.Faturamento.BL.Helpers.Factory
{
    public class TransmissaoNFeNormal : TransmissaoNFe
    {
        public TransmissaoNFeNormal(NFe nfe, TransmissaoBLs transmissaoBLs)
            : base(nfe, transmissaoBLs) { }

        public override TipoNota ObterTipoDocumentoFiscal()
        {
            return TipoNota.Saida;
        }

        public override bool SomaFrete()
        {
            return NFe.TipoFrete == TipoFrete.FOB;
        }

        public override TransmissaoVM ObterTransmissaoVM()
        {
            var itemTransmissao = ObterCabecalhoItemTransmissao();

            itemTransmissao.Transporte.Transportadora = ObterTransportadora();
            itemTransmissao.Transporte.Volume = ObterVolume();

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
            if (NFe.GeraFinanceiro)
            {
                itemTransmissao.Cobranca = ObterCobranca();
            }

            if (EhExportacao())
            {
                itemTransmissao.Exportacao = ObterExportacao();
            }

            var transmissao = ObterTransmissaoVMApartirDoItem(itemTransmissao);
            return transmissao;
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
                detalhe.Produto.ValorFrete = SomaFrete() ? Math.Round(itemTributacao.FreteValorFracionado, 2) : 0;

                detalhe.Imposto.ICMS = ObterICMS(item, itemTributacao);
                detalhe.Imposto.IPI = ObterIPI(item, itemTributacao);
                detalhe.Imposto.PIS = ObterPIS(item, itemTributacao);
                if (detalhe.Imposto.PIS.CodigoSituacaoTributaria.ToString() == "05")
                {
                    detalhe.Imposto.PISST = new PISST()
                    {
                        ValorPISST = itemTributacao.PISValor,
                    };
                }
                detalhe.Imposto.COFINS = ObterCOFINS(item, itemTributacao);
                detalhe.Imposto.TotalAprox = ObterTotalAproximado(detalhe);

                itemTransmissao.Detalhes.Add(detalhe);
                num++;
            }
        }

        private double CalcularValorTotalNFE(ItemTransmissaoVM itemTransmissao)
        {
            var total = (
                (itemTransmissao.Total.ICMSTotal.SomatorioProdutos +
                itemTransmissao.Total.ICMSTotal.SomatorioICMSST +
                itemTransmissao.Total.ICMSTotal.SomatorioIPI +
                itemTransmissao.Total.ICMSTotal.SomatorioFCPST) -
                itemTransmissao.Total.ICMSTotal.SomatorioDesconto);

            if (SomaFrete())
            {
                total += itemTransmissao.Total.ICMSTotal.ValorFrete;
            }
            return total;
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

                ICMS.UF = Cabecalho.Cliente.Estado?.Sigla;
                ICMS.PercentualMargemValorAdicionadoST = st != null ? st.Mva : 0;
                ICMS.ValorBCST = Math.Round(itemTributacao.STBase, 2);
                ICMS.AliquotaICMSST = Math.Round(itemTributacao.STAliquota, 2);
                ICMS.ValorICMSST = Math.Round(itemTributacao.STValor, 2);
                ICMS.ValorBCSTRetido = Math.Round(item.ValorBCSTRetido, 2);
                ICMS.ValorICMSSTRetido = Math.Round(item.ValorICMSSTRetido, 2);

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

                if (item.GrupoTributario.TipoTributacaoICMS == TipoTributacaoICMS.Outros 
                    || item.GrupoTributario.TipoTributacaoICMS == TipoTributacaoICMS.ComRedDeBaseDeST
                    || item.GrupoTributario.TipoTributacaoICMS == TipoTributacaoICMS.ComReducaoDeBaseDeCalculo
                    || item.GrupoTributario.TipoTributacaoICMS == TipoTributacaoICMS.Diferimento
                    || item.GrupoTributario.TipoTributacaoICMS == TipoTributacaoICMS.TributadaComCobrancaDeSubstituicao
                    || item.GrupoTributario.TipoTributacaoICMS == TipoTributacaoICMS.Outros90
                    || item.GrupoTributario.TipoTributacaoICMS == TipoTributacaoICMS.TributadaIntegralmente                  
                    )
                {
                    ICMS.ModalidadeBC = ModalidadeDeterminacaoBCICMS.ValorDaOperacao;
                    ICMS.AliquotaICMS = Math.Round(itemTributacao.ICMSAliquota, 2);
                    ICMS.ModalidadeBCST = ModalidadeDeterminacaoBCICMSST.MargemValorAgregado;
                    ICMS.ValorBC = Math.Round(itemTributacao.ICMSBase, 2);
                    ICMS.ValorICMS = Math.Round(itemTributacao.ICMSValor, 2);
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
                AliquotaAplicavelCalculoCreditoSN = Math.Round(((item.ValorCreditoICMS / (item.Quantidade * item.Valor)) * 100), 2),
                ValorCreditoICMS = Math.Round(item.ValorCreditoICMS, 2),
                CodigoSituacaoOperacao = item.GrupoTributario.TipoTributacaoICMS != null ? item.GrupoTributario.TipoTributacaoICMS.Value : TipoTributacaoICMS.TributadaSemPermissaoDeCredito,
                TipoCRT = ParametrosTributarios.TipoCRT
            };

            CalculaICMSPai(item, itemTributacao, ICMS);

            CalculaSTICMSPai(item, itemTributacao, ICMS);

            return ICMS;
        }

        private IPIPai ObterIPI(NFeProduto item, NotaFiscalItemTributacao itemTributacao)
        {
            if (itemTributacao.CalculaIPI)
            {
                return new IPIPai()
                {
                    CodigoST = item.GrupoTributario.TipoTributacaoIPI.HasValue ?
                        (CSTIPI)Enum.Parse(typeof(CSTIPI), item.GrupoTributario.TipoTributacaoIPI.Value.ToString()) : CSTIPI.OutrasSaidas,
                    ValorIPI = itemTributacao.IPIValor,
                    CodigoEnquadramento = item.Produto.EnquadramentoLegalIPI?.Codigo,
                    ValorBaseCalculo = Math.Round(itemTributacao.IPIBase, 2),
                    PercentualIPI = Math.Round(itemTributacao.IPIAliquota, 2),
                    QtdTotalUnidadeTributavel = item.Quantidade,
                    ValorUnidadeTributavel = Math.Round(item.Valor, 2)
                };
            }

            return null;
        }

        private PISPai ObterPIS(NFeProduto item, NotaFiscalItemTributacao itemTributacao)
        {
            var PIS = new PISPai()
            {
                CodigoSituacaoTributaria = item.GrupoTributario.TipoTributacaoPIS.HasValue && itemTributacao.CalculaPIS ?
                (CSTPISCOFINS)((int)item.GrupoTributario.TipoTributacaoPIS) :
                CSTPISCOFINS.IsentaDaContribuicao
            };

            if (itemTributacao.CalculaPIS)
            {
                var NaoTributaveis = "04||05||06||07||08||09";
                if (!NaoTributaveis.Contains(((int)PIS.CodigoSituacaoTributaria).ToString()))
                {
                    PIS.ValorPIS = Math.Round(itemTributacao.PISValor, 2);
                    PIS.PercentualPIS = ParametrosTributarios.AliquotaPISPASEP;
                    PIS.ValorBCDoPIS = Math.Round(itemTributacao.PISBase, 2);
                }
            }
            return PIS;
        }

        private COFINSPai ObterCOFINS(NFeProduto item, NotaFiscalItemTributacao itemTributacao)
        {
            var COFINS = new COFINSPai()
            {
                CodigoSituacaoTributaria = item.GrupoTributario.TipoTributacaoCOFINS != null && itemTributacao.CalculaCOFINS ?
                ((CSTPISCOFINS)(int)item.GrupoTributario.TipoTributacaoCOFINS.Value) :
                CSTPISCOFINS.OutrasOperacoes
            };

            if (itemTributacao.CalculaCOFINS)
            {
                var NaoTributaveis = "04||05||06||07||08||09";
                if (!NaoTributaveis.Contains(((int)COFINS.CodigoSituacaoTributaria).ToString()))
                {
                    COFINS.ValorCOFINS = Math.Round(itemTributacao.COFINSValor, 2);
                    COFINS.ValorBC = Math.Round(itemTributacao.COFINSBase, 2);
                    COFINS.AliquotaPercentual = Math.Round(itemTributacao.COFINSAliquota, 2);
                }
            }
            return COFINS;
        }
        #endregion
    }
}