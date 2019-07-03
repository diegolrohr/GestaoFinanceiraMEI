using Fly01.Core.Entities.Domains.Commons;
using Fly01.EmissaoNFE.Domain.ViewModel;
using Fly01.Faturamento.BL.Helpers.EntitiesBL;
using Fly01.EmissaoNFE.Domain.Enums;
using Fly01.Core.Entities.Domains.Enum;
using System.Collections.Generic;
using Fly01.EmissaoNFE.Domain.Entities.NFe;
using Fly01.EmissaoNFE.Domain.Entities.NFe.ICMS;
using Fly01.EmissaoNFE.Domain.Entities.NFe.PIS;
using Fly01.EmissaoNFE.Domain.Entities.NFe.COFINS;
using Fly01.EmissaoNFE.Domain.Entities.NFe.IPI;
using System.Linq;
using System;
using Fly01.EmissaoNFE.Domain.Entities.NFe.Totais;

namespace Fly01.Faturamento.BL.Helpers.Factory
{
    public class TransmissaoNFeComplementoIPI : TransmissaoNFe
    {
        public TransmissaoNFeComplementoIPI(NFe nfe, TransmissaoBLs transmissaoBLs)
            : base(nfe, transmissaoBLs) { }

        public override TipoNota ObterTipoDocumentoFiscal()
        {
            return NFe.NFeRefComplementarIsDevolucao ? TipoNota.Entrada : TipoNota.Saida;
        }

        public override bool SomaFrete()
        {
            return false;
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

                detalhe.Imposto.ICMS = ObterICMS(item);
                detalhe.Imposto.IPI = ObterIPI(item, itemTributacao);
                detalhe.Imposto.PIS = ObterPIS(item);
                detalhe.Imposto.COFINS = ObterCOFINS(item);

                itemTransmissao.Detalhes.Add(detalhe);
                num++;
            }
        }

        private double CalcularValorTotalNFE(ItemTransmissaoVM itemTransmissao)
        {
            return itemTransmissao.Total.ICMSTotal.SomatorioIPI;
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

        private IPIPai ObterIPI(NFeProduto item, NotaFiscalItemTributacao itemTributacao)
        {
            return new IPIPai()
            {
                //TODO: Revisar CST e valores
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

        private ICMSPai ObterICMS(NFeProduto item)
        {
            return new ICMSPai()
            {
                OrigemMercadoria = item.Produto.OrigemMercadoria,
                CodigoSituacaoOperacao = TipoTributacaoICMS.Imune,//TODO: Ver Wilson TipoTributacaoICMS.NaoTributadaPeloSN
                TipoCRT = ParametrosTributarios.TipoCRT
            };
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
                //TODO: confirmar Wilson
                CodigoSituacaoTributaria = NFe.NFeRefComplementarIsDevolucao ? CSTPISCOFINS.OutrasOperacoesDeEntrada : CSTPISCOFINS.OutrasOperacoesDeSaida
            };
        }
        #endregion
    }
}
