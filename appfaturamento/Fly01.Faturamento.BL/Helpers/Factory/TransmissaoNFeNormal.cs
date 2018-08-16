using Fly01.Core;
using Fly01.Core.Entities.Domains.Commons;
using Fly01.Core.Entities.Domains.Enum;
using Fly01.Core.Helpers;
using Fly01.Core.Rest;
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

        public TipoNota TipoDocumentoFiscal()
        {
            return TipoNota.Saida;
        }

        public bool PagaFrete()
        {
            return (NFe.TipoFrete == TipoFrete.CIF || NFe.TipoFrete == TipoFrete.Remetente);
        }

        public override TransmissaoVM ObterTransmissaoVM()
        {
            var itemTransmissao = new ItemTransmissaoVM();
            itemTransmissao.Versao = Cabecalho.Versao;
            itemTransmissao.Identificador = ObterIdentificador();
            itemTransmissao.Emitente = ObterEmitente();
            itemTransmissao.Destinatario = ObterDestinatario();

            #region Transporte
            itemTransmissao.Transporte = new Transporte()
            {
                ModalidadeFrete = (ModalidadeFrete)Enum.Parse(typeof(ModalidadeFrete), NFe.TipoFrete.ToString()),
            };
            itemTransmissao.Transporte.Transportadora = ObterTransportadora();
            itemTransmissao.Transporte.Volume = ObterVolume();
            #endregion

            #region Detalhes Produtos
            itemTransmissao.Detalhes = new List<Detalhe>();
            var num = 1;

            foreach (var item in ObterNFeProdutos())
            {
                var itemTributacao = new NotaFiscalItemTributacao();
                itemTributacao = TransmissaoBLs.NotaFiscalItemTributacaoBL.All.Where(x => x.NotaFiscalItemId == item.Id).FirstOrDefault();

                var detalhe = ObterDetalhe(item, num);
                detalhe.Produto.ValorFrete = PagaFrete() ? Math.Round(itemTributacao.FreteValorFracionado, 2) : 0;

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
            #endregion

            #region Total
            itemTransmissao.Total = new Total()
            {
                ICMSTotal = new ICMSTOT()
                {
                    SomatorioBC = itemTransmissao.Detalhes.Select(x => x.Imposto.ICMS).Any(x => x != null && x.ValorBC.HasValue) ? Math.Round(itemTransmissao.Detalhes.Where(x => x.Imposto.ICMS != null && x.Imposto.ICMS.ValorBC.HasValue).Sum(x => x.Imposto.ICMS.ValorBC.Value), 2) : 0,
                    SomatorioICMS = itemTransmissao.Detalhes.Select(x => x.Imposto.ICMS).Any(x => x != null && x.ValorICMS.HasValue) ? Math.Round(itemTransmissao.Detalhes.Where(x => x.Imposto.ICMS != null && x.Imposto.ICMS.ValorICMS.HasValue).Sum(x => x.Imposto.ICMS.ValorICMS.Value), 2) : 0,
                    SomatorioBCST = itemTransmissao.Detalhes.Select(x => x.Imposto.ICMS).Any(x => x != null && x.ValorBCST.HasValue) ? Math.Round(itemTransmissao.Detalhes.Where(x => x.Imposto.ICMS != null && x.Imposto.ICMS.ValorBCST.HasValue).Sum(x => x.Imposto.ICMS.ValorBCST.Value), 2) : 0,
                    SomatorioCofins = itemTransmissao.Detalhes.Select(x => x.Imposto.COFINS).Any(x => x != null) ? Math.Round(itemTransmissao.Detalhes.Sum(x => x.Imposto.COFINS.ValorCOFINS), 2) : 0,
                    SomatorioDesconto = ObterNFeProdutos().Sum(x => x.Desconto),
                    SomatorioICMSST = itemTransmissao.Detalhes.Select(x => x.Imposto.ICMS).Any(x => x != null && x.ValorICMSST.HasValue) ? Math.Round(itemTransmissao.Detalhes.Where(x => x.Imposto.ICMS != null && x.Imposto.ICMS.ValorICMSST.HasValue).Sum(x => x.Imposto.ICMS.ValorICMSST.Value), 2) : 0,
                    ValorFrete = PagaFrete() ? itemTransmissao.Detalhes.Sum(x => x.Produto.ValorFrete.Value) : 0,
                    ValorSeguro = 0,
                    SomatorioIPI = itemTransmissao.Detalhes.Select(x => x.Imposto.IPI).Any(x => x != null) ? Math.Round(itemTransmissao.Detalhes.Where(x => x.Imposto.IPI != null).Sum(x => x.Imposto.IPI.ValorIPI), 2) : 0,
                    SomatorioIPIDevolucao = itemTransmissao.Detalhes.Select(x => x.Imposto.IPI).Any(x => x != null) ? Math.Round(itemTransmissao.Detalhes.Where(x => x.Imposto.IPI != null).Sum(x => x.Imposto.IPI.ValorIPIDevolucao), 2) : 0,
                    SomatorioPis = itemTransmissao.Detalhes.Sum(y => y.Imposto.PIS.ValorPIS),
                    //+(itemTransmissao.Detalhes.Select(x => x.Imposto.PISST).Any(x => x != null) ? itemTransmissao.Detalhes.Where(x => x.Imposto.PISST != null).Sum(y => y.Imposto.PISST.ValorPISST) : 0),
                    SomatorioProdutos = itemTransmissao.Detalhes.Sum(x => x.Produto.ValorBruto),
                    SomatorioOutro = 0,
                    SomatorioFCP = 0,
                    SomatorioFCPST = itemTransmissao.Detalhes.Select(x => x.Imposto.ICMS).Any(x => x != null && x.ValorFCPST.HasValue) ? Math.Round(itemTransmissao.Detalhes.Where(x => x.Imposto.ICMS != null && x.Imposto.ICMS.ValorFCPST.HasValue).Sum(x => x.Imposto.ICMS.ValorFCPST.Value), 2) : 0,
                    SomatorioFCPSTRetido = itemTransmissao.Detalhes.Select(x => x.Imposto.ICMS).Any(x => x != null && x.ValorFCPSTRetido.HasValue) ? Math.Round(itemTransmissao.Detalhes.Where(x => x.Imposto.ICMS != null && x.Imposto.ICMS.ValorFCPSTRetido.HasValue).Sum(x => x.Imposto.ICMS.ValorFCPSTRetido.Value), 2) : 0,
                }
            };
            var icmsTotal = itemTransmissao.Total.ICMSTotal;
            itemTransmissao.Total.ICMSTotal.TotalTributosAprox =
                icmsTotal.SomatorioICMS +
                icmsTotal.SomatorioCofins +
                icmsTotal.SomatorioICMSST +
                icmsTotal.SomatorioIPI +
                icmsTotal.SomatorioPis +
                icmsTotal.SomatorioFCPST;

            itemTransmissao.Total.ICMSTotal.ValorTotalNF =
                ((itemTransmissao.Total.ICMSTotal.SomatorioProdutos +
                itemTransmissao.Total.ICMSTotal.SomatorioICMSST +
                itemTransmissao.Total.ICMSTotal.ValorFrete +
                itemTransmissao.Total.ICMSTotal.SomatorioIPI +
                itemTransmissao.Total.ICMSTotal.SomatorioFCPST) -
                itemTransmissao.Total.ICMSTotal.SomatorioDesconto);
            #endregion

            #region Pagamento
            itemTransmissao.Pagamento = ObterPagamento(itemTransmissao.Total.ICMSTotal.ValorTotalNF);
            
            #endregion

            if (!string.IsNullOrEmpty(NFe.MensagemPadraoNota))
            {
                itemTransmissao.InformacoesAdicionais = new InformacoesAdicionais()
                {
                    InformacoesComplementares = NFe.MensagemPadraoNota
                };
            }

            var entidade = TransmissaoBLs.CertificadoDigitalBL.GetEntidade();

            var transmissao = new TransmissaoVM()
            {
                Homologacao = entidade.Homologacao,
                Producao = entidade.Producao,
                EntidadeAmbiente = entidade.EntidadeAmbiente,
                Item = new List<ItemTransmissaoVM>()
                        {
                            itemTransmissao
                        }
            };
            return transmissao;
        }

        #region Impostos
        private ICMSPai ObterICMS(NFeProduto item, NotaFiscalItemTributacao itemTributacao)
        {
            var ICMS = new ICMSPai()
            {
                OrigemMercadoria = OrigemMercadoria.Nacional,
                AliquotaAplicavelCalculoCreditoSN = Math.Round(((item.ValorCreditoICMS / item.Total) * 100), 2),
                ValorCreditoICMS = Math.Round(item.ValorCreditoICMS, 2),
                CodigoSituacaoOperacao = item.GrupoTributario.TipoTributacaoICMS != null ? item.GrupoTributario.TipoTributacaoICMS.Value : TipoTributacaoICMS.TributadaSemPermissaoDeCredito
            };

            if (itemTributacao.CalculaICMS)
            {
                ICMS.ValorICMSSTRetido = Math.Round(item.ValorICMSSTRetido, 2);
                ICMS.ValorICMS = Math.Round(itemTributacao.ICMSValor, 2);
                ICMS.ValorBC = Math.Round(itemTributacao.ICMSBase, 2);

                if (item.GrupoTributario.TipoTributacaoICMS == TipoTributacaoICMS.Outros)
                {
                    ICMS.ModalidadeBC = ModalidadeDeterminacaoBCICMS.ValorDaOperacao;
                    ICMS.AliquotaICMS = Math.Round(itemTributacao.ICMSAliquota, 2);
                    ICMS.ModalidadeBCST = ModalidadeDeterminacaoBCICMSST.MargemValorAgregado;
                }
                if (item.GrupoTributario.TipoTributacaoICMS == TipoTributacaoICMS.TributadaComPermissaoDeCreditoST
                    || item.GrupoTributario.TipoTributacaoICMS == TipoTributacaoICMS.TributadaSemPermissaoDeCreditoST
                    || item.GrupoTributario.TipoTributacaoICMS == TipoTributacaoICMS.IsencaoParaFaixaDeReceitaBrutaST)
                {
                    ICMS.ModalidadeBCST = ModalidadeDeterminacaoBCICMSST.MargemValorAgregado;
                    ICMS.PercentualReducaoBCST = 0;
                }
            }
            if (itemTributacao.CalculaST)
            {
                string UFSiglaEmpresa = (Cabecalho.Empresa.Cidade != null ? (Cabecalho.Empresa.Cidade.Estado != null ? Cabecalho.Empresa.Cidade.Estado.Sigla : "") : "");
                var st = TransmissaoBLs.SubstituicaoTributariaBL.AllIncluding(y => y.EstadoOrigem).AsNoTracking().Where(x =>
                    x.NcmId == (item.Produto.NcmId ?? Guid.NewGuid()) &
                    x.CestId == item.Produto.CestId.Value &
                    x.EstadoOrigem.Sigla == UFSiglaEmpresa &
                    x.EstadoDestinoId == Cabecalho.Cliente.EstadoId &
                    x.TipoSubstituicaoTributaria == TipoSubstituicaoTributaria.Saida
                    ).FirstOrDefault();
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
            else
            {
                return null;
            }
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
                //adValorem =  01|02, AliqEspecifica = 03
                var tributaveis = "01|02|03";
                if (tributaveis.Contains(((int)PIS.CodigoSituacaoTributaria).ToString()))
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
                //adValorem =  01|02, AliqEspecifica = 03
                var tributaveis = "01|02|03";
                if (tributaveis.Contains(((int)COFINS.CodigoSituacaoTributaria).ToString()))
                {
                    COFINS.ValorCOFINS = Math.Round(itemTributacao.COFINSValor, 2);
                    COFINS.ValorBC = Math.Round(itemTributacao.COFINSBase, 2);
                    COFINS.AliquotaPercentual = Math.Round(itemTributacao.COFINSAliquota, 2);
                }
            }
            return COFINS;
        }
    }
}