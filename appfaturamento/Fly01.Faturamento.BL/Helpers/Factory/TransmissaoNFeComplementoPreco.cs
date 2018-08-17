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

namespace Fly01.Faturamento.BL.Helpers.Factory
{
    public class TransmissaoNFeComplementoPreco: TransmissaoNFe
    {
        public TransmissaoNFeComplementoPreco(NFe nfe, TransmissaoBLs transmissaoBLs) 
            : base(nfe, transmissaoBLs) {}

        public override TipoNota ObterTipoDocumentoFiscal()
        {
            return NFe.NFeRefComplementarIsDevolucao ? TipoNota.Entrada : TipoNota.Saida;
        }

        public override bool PagaFrete()
        {
            return false;
        }

        public override TransmissaoVM ObterTransmissaoVM()
        {
            var itemTransmissao = ObterCabecalhoItemTransmissao();

            //Complementar não pode sai a tag de Transportadora no XML
            //itemTransmissao.Transporte.Transportadora = null;
            itemTransmissao.Transporte.ModalidadeFrete = ModalidadeFrete.SemFrete;
            //itemTransmissao.Transporte.Volume = null;

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

        private void CriarProdutosEImpostosParaDetalhes(ItemTransmissaoVM itemTransmissao)
        {
            itemTransmissao.Detalhes = new List<Detalhe>();
            var num = 1;

            foreach (var item in ObterNFeProdutos())
            {               
                var detalhe = ObterDetalhe(item, num);

                detalhe.Imposto.ICMS = ObterICMS(item);
                detalhe.Imposto.PIS = ObterPIS(item);
                detalhe.Imposto.COFINS = ObterCOFINS(item);

                itemTransmissao.Detalhes.Add(detalhe);
                num++;
            }
        }

        private static double CalcularValorTotalNFE(ItemTransmissaoVM itemTransmissao)
        {
            return (itemTransmissao.Total.ICMSTotal.SomatorioProdutos - itemTransmissao.Total.ICMSTotal.SomatorioDesconto);
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

        private ICMSPai ObterICMS(NFeProduto item)
        {
            var ICMS = new ICMSPai()
            {
                OrigemMercadoria = OrigemMercadoria.Nacional,
                CodigoSituacaoOperacao = TipoTributacaoICMS.Imune//TODO: Ver Wilson TipoTributacaoICMS.NaoTributadaPeloSN
            };

            return ICMS;
        }

        private PISPai ObterPIS(NFeProduto item)
        {
            var PIS = new PISPai()
            {
                CodigoSituacaoTributaria = CSTPISCOFINS.IsentaDaContribuicao //TODO Wilson confirmar SemIncidencia
            };

            return PIS;
        }

        private COFINSPai ObterCOFINS(NFeProduto item)
        {
            var COFINS = new COFINSPai()
            {
                //TODO: confirmar Wilson
                CodigoSituacaoTributaria = NFe.NFeRefComplementarIsDevolucao ? CSTPISCOFINS.OutrasOperacoesDeEntrada : CSTPISCOFINS.OutrasOperacoesDeSaida
            };

            return COFINS;
        }
        #endregion
    }
}
