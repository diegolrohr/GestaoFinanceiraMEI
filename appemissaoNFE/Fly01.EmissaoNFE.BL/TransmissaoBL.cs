using System;
using System.Collections.Generic;
using System.Linq;
using Fly01.Core.BL;
using Fly01.EmissaoNFE.Domain.Entities.NFe;
using Fly01.EmissaoNFE.Domain.Enums;
using Fly01.EmissaoNFE.Domain.ViewModel;
using Fly01.EmissaoNFE.BL.Helpers;
using Fly01.Core.Entities.Domains.Enum;
using Fly01.Core;
using Fly01.Core.Helpers;

namespace Fly01.EmissaoNFE.BL
{
    public class TransmissaoBL : PlataformaBaseBL<TransmissaoVM>
    {
        protected CfopBL CfopBL;
        protected ChaveBL ChaveBL;
        protected CidadeBL CidadeBL;
        protected EmpresaBL EmpresaBL;
        protected EntidadeBL EntidadeBL;
        protected EstadoBL EstadoBL;
        protected NFeBL NFeBL;
        protected HelperValidaModelTransmissao helperValidaModelTransmissao;

        public TransmissaoBL(AppDataContextBase context, CfopBL cfopBL, ChaveBL chaveBL, CidadeBL cidadeBL, EmpresaBL empresaBL, EntidadeBL entidadeBL, EstadoBL estadoBL, NFeBL nfeBL) : base(context)
        {
            CfopBL = cfopBL;
            ChaveBL = chaveBL;
            CidadeBL = cidadeBL;
            EmpresaBL = empresaBL;
            EntidadeBL = entidadeBL;
            EstadoBL = estadoBL;
            NFeBL = nfeBL;
        }

        public string SerializeNota(ItemTransmissaoVM nfe)
        {
            var nota = ConvertToNFe(nfe);
            var xmlString = NFeBL.ConvertToXML(nota, nfe.Emitente.CRT);

            return xmlString;
        }

        public NFeVM ConvertToNFe(ItemTransmissaoVM item)
        {
            var nota = new NFeVM();
            nota.InfoNFe = new InfoNFe();

            nota.InfoNFe.NotaId = item.NotaId;
            nota.InfoNFe.Versao = item.Versao;
            nota.InfoNFe.Identificador = item.Identificador;
            nota.InfoNFe.Identificador.ChaveAcessoDV = Int32.Parse(item.NotaId.Substring(46, 1));
            nota.InfoNFe.Identificador.CodigoNF = item.Identificador.CodigoNF.PadLeft(8, '0');
            nota.InfoNFe.Emitente = item.Emitente;
            nota.InfoNFe.Destinatario = item.Destinatario;

            if ((int)nota.InfoNFe.Identificador.Ambiente == 2)
                nota.InfoNFe.Destinatario.Nome = "NF-E EMITIDA EM AMBIENTE DE HOMOLOGACAO - SEM VALOR FISCAL";

            nota.InfoNFe.Detalhes = item.Detalhes;
            nota.InfoNFe.Total = item.Total;
            nota.InfoNFe.Transporte = item.Transporte;
            nota.InfoNFe.Cobranca = item.Cobranca;
            nota.InfoNFe.Pagamento = item.Pagamento;
            nota.InfoNFe.InformacoesAdicionais = item.InformacoesAdicionais;

            if (item.Emitente.Endereco.UF == "BA" && (nota.InfoNFe.Autorizados == null || !nota.InfoNFe.Autorizados.Any()))
            {
                nota.InfoNFe.Autorizados = new List<Autorizados>();
                nota.InfoNFe.Autorizados.Add(new Autorizados() { CNPJ = "13937073000156" });
            }

            nota.InfoNFe.Exportacao = item.Exportacao;
            nota.InfoNFe.ResponsavelTecnico = item.ResponsavelTecnico;

            return nota;
        }

        public double Arredondar(double? valor, int casas)
        {
            double retorno = valor.HasValue ? valor.Value : 0;

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

        public void MensagemCreditoICMS(TransmissaoVM entity)
        {
            foreach (var nota in entity.Item.Where(x => x.Identificador != null &&
                (
                    (x.Identificador.FinalidadeEmissaoNFe == TipoCompraVenda.Normal || x.Identificador.FinalidadeEmissaoNFe == TipoCompraVenda.Complementar) &&
                    x.Identificador.TipoDocumentoFiscal == TipoNota.Saida)
                ))
            {
                if (nota.Detalhes != null)
                {
                    var creditosDeICMS = nota.Detalhes.Where(x =>
                        (x.Imposto != null) &&
                        (x.Imposto.ICMS != null) &&
                        (x.Imposto.ICMS.CodigoSituacaoOperacao == TipoTributacaoICMS.TributadaComPermissaoDeCredito ||
                        x.Imposto.ICMS.CodigoSituacaoOperacao == TipoTributacaoICMS.TributadaComPermissaoDeCreditoST ||
                        x.Imposto.ICMS.CodigoSituacaoOperacao == TipoTributacaoICMS.Outros ||
                        x.Imposto.ICMS.CodigoSituacaoOperacao == TipoTributacaoICMS.Outros90 ||
                        x.Imposto.ICMS.CodigoSituacaoOperacao == TipoTributacaoICMS.TributadaComCobrancaDeSubstituicao ||
                        x.Imposto.ICMS.CodigoSituacaoOperacao == TipoTributacaoICMS.ComReducaoDeBaseDeCalculo ||
                        x.Imposto.ICMS.CodigoSituacaoOperacao == TipoTributacaoICMS.TributadaComCobrancaDeSubstituicao ||
                        x.Imposto.ICMS.CodigoSituacaoOperacao == TipoTributacaoICMS.ComRedDeBaseDeST ||
                        x.Imposto.ICMS.CodigoSituacaoOperacao == TipoTributacaoICMS.Diferimento ||
                        x.Imposto.ICMS.CodigoSituacaoOperacao == TipoTributacaoICMS.TributadaIntegralmente) &&
                        (x.Imposto.ICMS.ValorCreditoICMS.HasValue));

                    var totalCredito = creditosDeICMS.Sum(x => x.Imposto.ICMS.ValorCreditoICMS.Value);
                    var totalBrutoProduto = creditosDeICMS.Sum(x => x.Produto.ValorBruto);
                    var aliquota = Math.Round(((totalCredito / totalBrutoProduto) * 100), 2);

                    if (totalCredito > 0 && aliquota > 0)
                    {
                        var mensagemAproveitamentoCredito =
                            string.Format("PERMITE O APROVEITAMENTO DO CRÉDITO DE ICMS NO VALOR DE {0}; CORRESPONDENTE À ALÍQUOTA DE {1}%, NOS TERMOS DO ARTIGO 23 DA LC 123.",
                                totalCredito.ToString("C", AppDefaults.CultureInfoDefault),
                                aliquota.ToString("N", AppDefaults.CultureInfoDefault));

                        if (nota.InformacoesAdicionais == null)
                        {
                            nota.InformacoesAdicionais = new InformacoesAdicionais()
                            {
                                InformacoesComplementares = mensagemAproveitamentoCredito
                            };
                        }
                        else
                        {
                            nota.InformacoesAdicionais.InformacoesComplementares =
                                string.IsNullOrEmpty(nota.InformacoesAdicionais.InformacoesComplementares) ?
                                mensagemAproveitamentoCredito :
                                nota.InformacoesAdicionais.InformacoesComplementares + "\n" + mensagemAproveitamentoCredito;
                        }
                    }
                }
            }
        }

        public override void ValidaModel(TransmissaoVM entity)
        {
            EntidadeBL.ValidaModel(entity);

            EntitiesBLToValidate entitiesBL = GetEntitiesBLToValidate();

            helperValidaModelTransmissao = new HelperValidaModelTransmissao(entity, entitiesBL);
            helperValidaModelTransmissao.ExecutarHelperValidaModel();

            base.ValidaModel(entity);
        }

        private EntitiesBLToValidate GetEntitiesBLToValidate()
        {
            return new EntitiesBLToValidate
            {
                _chaveBL = ChaveBL,
                _cidadeBL = CidadeBL,
                _cfopBL = CfopBL,
                _empresaBL = EmpresaBL,
                _entidadeBL = EntidadeBL,
                _estadoBL = EstadoBL,
                _nfeBL = NFeBL
            };
        }
    }
}