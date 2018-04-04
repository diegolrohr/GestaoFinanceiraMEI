using Fly01.EmissaoNFE.Domain.Entities.NFe;
using Fly01.EmissaoNFE.Domain.Entities.NFe.COFINS;
using Fly01.EmissaoNFE.Domain.Entities.NFe.ICMS;
using Fly01.EmissaoNFE.Domain.Entities.NFe.IPI;
using Fly01.EmissaoNFE.Domain.Entities.NFe.PIS;
using Fly01.EmissaoNFE.Domain.Entities.NFe.Totais;
using Fly01.EmissaoNFE.Domain.Enums;
using Fly01.EmissaoNFE.Domain.ViewModel;
using Fly01.Faturamento.DAL;
using Fly01.Faturamento.Domain.Entities;
using Fly01.Faturamento.Domain.Enums;
using Fly01.Core;
using Fly01.Core.Api.BL;
using Fly01.Core.Helpers;
using Fly01.Core.Notifications;
using Fly01.Core.ValueObjects;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using NFe = Fly01.Faturamento.Domain.Entities.NFe;
using ProdutoNFe = Fly01.EmissaoNFE.Domain.Entities.NFe.Produto;
using StatusNotaFiscal = Fly01.Faturamento.Domain.Enums.StatusNotaFiscal;
using TipoAmbienteNFe = Fly01.EmissaoNFE.Domain.Enums.TipoAmbiente;
using TipoFormaPagamentoNFe = Fly01.EmissaoNFE.Domain.Enums.TipoFormaPagamento;
using TipoModalidadeNFe = Fly01.EmissaoNFE.Domain.Enums.TipoModalidade;

namespace Fly01.Faturamento.BL
{
    public class NFeBL : PlataformaBaseBL<NFe>
    {
        protected SerieNotaFiscalBL SerieNotaFiscalBL { get; set; }
        protected NFeProdutoBL NFeProdutoBL { get; set; }
        protected TotalTributacaoBL TotalTributacaoBL { get; set; }
        protected CertificadoDigitalBL CertificadoDigitalBL { get; set; }
        protected PessoaBL PessoaBL { get; set; }
        protected CondicaoParcelamentoBL CondicaoParcelamentoBL { get; set; }
        protected SubstituicaoTributariaBL SubstituicaoTributariaBL { get; set; }
        protected NotaFiscalItemTributacaoBL NotaFiscalItemTributacaoBL { get; set; }

        public NFeBL(AppDataContext context, SerieNotaFiscalBL serieNotaFiscalBL, NFeProdutoBL nfeProdutoBL, TotalTributacaoBL totalTributacaoBL, CertificadoDigitalBL certificadoDigitalBL, PessoaBL pessoaBL, CondicaoParcelamentoBL condicaoParcelamentoBL, SubstituicaoTributariaBL substituicaoTributariaBL, NotaFiscalItemTributacaoBL notaFiscalItemTributacaoBL) : base(context)
        {
            SerieNotaFiscalBL = serieNotaFiscalBL;
            NFeProdutoBL = nfeProdutoBL;
            TotalTributacaoBL = totalTributacaoBL;
            CertificadoDigitalBL = certificadoDigitalBL;
            PessoaBL = pessoaBL;
            CondicaoParcelamentoBL = condicaoParcelamentoBL;
            SubstituicaoTributariaBL = substituicaoTributariaBL;
            NotaFiscalItemTributacaoBL = notaFiscalItemTributacaoBL;
        }

        public IQueryable<NFe> AllWithoutPlataformaId => repository.All.Where(x => x.Ativo);

        public override void ValidaModel(NFe entity)
        {
            entity.Fail(entity.TipoNotaFiscal != TipoNotaFiscal.NFe, new Error("Permitido somente nota fiscal do tipo NFe"));
            entity.Fail(entity.ValorFrete.HasValue && entity.ValorFrete.Value < 0, new Error("Valor frete não pode ser negativo", "valorFrete"));
            entity.Fail(entity.PesoBruto.HasValue && entity.PesoBruto.Value < 0, new Error("Peso bruto não pode ser negativo", "pesoBruto"));
            entity.Fail(entity.PesoLiquido.HasValue && entity.PesoLiquido.Value < 0, new Error("Peso liquido não pode ser negativo", "pesoLiquido"));
            entity.Fail(entity.QuantidadeVolumes.HasValue && entity.QuantidadeVolumes.Value < 0, new Error("Quantidade de volumes não pode ser negativo", "quantidadeVolumes"));
            entity.Fail((entity.NumNotaFiscal.HasValue || entity.SerieNotaFiscalId.HasValue) && (!entity.NumNotaFiscal.HasValue || !entity.SerieNotaFiscalId.HasValue), new Error("Informe série e número da nota fiscal"));

            var serieNotaFiscal = SerieNotaFiscalBL.All.AsNoTracking().Where(x => x.Id == entity.SerieNotaFiscalId).FirstOrDefault();
            if (entity.SerieNotaFiscalId.HasValue)
            {
                entity.Fail(serieNotaFiscal == null || serieNotaFiscal.StatusSerieNotaFiscal == StatusSerieNotaFiscal.Inutilizada || (serieNotaFiscal.TipoOperacaoSerieNotaFiscal != TipoOperacaoSerieNotaFiscal.NFe && serieNotaFiscal.TipoOperacaoSerieNotaFiscal != TipoOperacaoSerieNotaFiscal.Ambas), new Error("Selecione uma série ativa do tipo NF-e ou tipo ambas"));
            }

            if (entity.Status == StatusNotaFiscal.Transmitida && entity.SerieNotaFiscalId.HasValue && entity.NumNotaFiscal.HasValue)
            {
                var serieENumeroJaUsado = All.AsNoTracking().Any(x => x.Id != entity.Id && (x.SerieNotaFiscalId == entity.SerieNotaFiscalId && x.NumNotaFiscal == entity.NumNotaFiscal));
                //varios numeros de uma mesma serie/tipo inutilizados
                var serieENumeroInutilizado = SerieNotaFiscalBL.All.AsNoTracking().Any(x =>
                    x.StatusSerieNotaFiscal == StatusSerieNotaFiscal.Inutilizada &&
                    x.Serie.ToUpper() == serieNotaFiscal.Serie.ToUpper() &&
                    x.NumNotaFiscal == entity.NumNotaFiscal &&
                    (x.TipoOperacaoSerieNotaFiscal == serieNotaFiscal.TipoOperacaoSerieNotaFiscal || x.TipoOperacaoSerieNotaFiscal == TipoOperacaoSerieNotaFiscal.Ambas));

                if (serieENumeroJaUsado || serieENumeroInutilizado)
                {
                    var sugestaoProximoNumNota = All.Max(x => x.NumNotaFiscal);
                    if (!sugestaoProximoNumNota.HasValue)
                    {
                        sugestaoProximoNumNota = entity.NumNotaFiscal;
                    }

                    do
                    {
                        sugestaoProximoNumNota += 1;
                    }//enquanto sugestão possa estar na lista de inutilizadas
                    while (SerieNotaFiscalBL.All.AsNoTracking().Any(x =>
                        x.StatusSerieNotaFiscal == StatusSerieNotaFiscal.Inutilizada &&
                        x.Serie.ToUpper() == serieNotaFiscal.Serie.ToUpper() &&
                        x.NumNotaFiscal == sugestaoProximoNumNota &&
                        (x.TipoOperacaoSerieNotaFiscal == serieNotaFiscal.TipoOperacaoSerieNotaFiscal || x.TipoOperacaoSerieNotaFiscal == TipoOperacaoSerieNotaFiscal.Ambas)));

                    entity.Fail(true, new Error("Série e número já utilizados ou inutilizados, sugestão de número: " + sugestaoProximoNumNota.ToString(), "numNotaFiscal"));
                }
                else
                {
                    var serie = SerieNotaFiscalBL.All.Where(x => x.Id == entity.SerieNotaFiscalId).FirstOrDefault();
                    serie.NumNotaFiscal = entity.NumNotaFiscal.Value + 1;
                    SerieNotaFiscalBL.Update(serie);
                };
            }

            base.ValidaModel(entity);
        }

        public void TransmitirNFe(NFe entity)
        {
            try
            {
                if (!TotalTributacaoBL.ConfiguracaoTSSOK())
                {
                    throw new BusinessException("Configuração inválida para comunicação com TSS");
                }
                else
                {
                    var parametros = TotalTributacaoBL.GetParametrosTributarios();
                    if (parametros == null)
                    {
                        throw new BusinessException("Acesse o menu Configurações > Parâmetros Tributários e salve as configurações para a transmissão");
                    }

                    var cliente = TotalTributacaoBL.GetPessoa(entity.ClienteId);
                    var empresa = TotalTributacaoBL.GetDadosEmpresa(PlataformaUrl);
                    var condicaoParcelamento = CondicaoParcelamentoBL.All.Where(x => x.Id == entity.CondicaoParcelamentoId).FirstOrDefault();
                    var transportadora = PessoaBL.All.Where(x => x.Transportadora && x.Id == entity.TransportadoraId).FirstOrDefault();
                    var serieNotaFiscal = SerieNotaFiscalBL.All.AsNoTracking().Where(x => x.Id == entity.SerieNotaFiscalId).FirstOrDefault();
                    var NFeProdutos = NFeProdutoBL.AllIncluding(
                        x => x.GrupoTributario.Cfop,
                        x => x.Produto.Ncm,
                        x => x.Produto.Cest,
                        x => x.Produto.UnidadeMedida,
                        x => x.Produto.EnquadramentoLegalIPI).Where(x => x.NotaFiscalId == entity.Id);

                    var totalNFe = CalculaTotalNFe(entity.Id, entity.ValorFrete);

                    var destinoOperacao = TipoDestinoOperacao.Interna;
                    if (cliente.Estado != null && cliente.Estado.Sigla.ToUpper() == "EX" || (empresa.Cidade.Estado != null ? empresa.Cidade.Estado.Sigla.ToUpper() : "") == "EX")
                    {
                        destinoOperacao = TipoDestinoOperacao.Exterior;
                    }
                    else if (cliente.Estado != null && empresa.Cidade.Estado != null && (cliente.Estado.Sigla.ToUpper() != empresa.Cidade.Estado.Sigla.ToUpper()))
                    {
                        destinoOperacao = TipoDestinoOperacao.Interestadual;
                    }
                    var formPag = TipoFormaPagamentoNFe.Outros;
                    if (condicaoParcelamento != null)
                    {
                        formPag = (condicaoParcelamento.QtdParcelas == 1 || condicaoParcelamento.CondicoesParcelamento == "0") ? TipoFormaPagamentoNFe.AVista : TipoFormaPagamentoNFe.APrazo;
                    }

                    var itemTransmissao = new ItemTransmissaoVM();
                    itemTransmissao.NotaId = entity.Numero.ToString();
                    itemTransmissao.Versao = EnumHelper.GetDescription(parametros.TipoVersaoNFe);

                    #region Identificação
                    itemTransmissao.Identificador = new Identificador()
                    {
                        CodigoUF = empresa.Cidade != null ? (empresa.Cidade.Estado != null ? int.Parse(empresa.Cidade.Estado.CodigoIbge) : 0) : 0,
                        NaturezaOperacao = entity.NaturezaOperacao,
                        FormaPagamento = formPag,
                        ModeloDocumentoFiscal = 55,
                        Serie = int.Parse(serieNotaFiscal.Serie),
                        NumeroDocumentoFiscal = entity.NumNotaFiscal.Value,
                        Emissao = DateTime.Now,
                        EntradaSaida = DateTime.Now,
                        TipoDocumentoFiscal = TipoNota.Saida,
                        DestinoOperacao = destinoOperacao,
                        CodigoMunicipio = empresa.Cidade != null ? empresa.Cidade.CodigoIbge : null,
                        ImpressaoDANFE = TipoImpressaoDanfe.Retrato,
                        ChaveAcessoDV = 0,
                        CodigoNF = entity.NumNotaFiscal.Value.ToString(),
                        Ambiente = (TipoAmbienteNFe)Enum.Parse(typeof(TipoAmbienteNFe), parametros.TipoAmbiente.ToString()),
                        FinalidadeEmissaoNFe = TipoFinalidadeEmissaoNFe.Normal,
                        ConsumidorFinal = cliente.ConsumidorFinal ? 1 : 0,
                        PresencaComprador = TipoPresencaComprador.Presencial,
                        Versao = "2.77",
                        FormaEmissao = (TipoModalidadeNFe)Enum.Parse(typeof(TipoModalidadeNFe), parametros.TipoModalidade.ToString()),
                        CodigoProcessoEmissaoNFe = 0
                    };
                    #endregion

                    #region Emitente
                    itemTransmissao.Emitente = new Emitente()
                    {
                        Cnpj = empresa.CNPJ,
                        Nome = empresa.RazaoSocial,
                        NomeFantasia = empresa.NomeFantasia,
                        InscricaoEstadual = empresa.InscricaoEstadual,
                        CRT = CRT.SimplesNacional,
                        Endereco = new Endereco()
                        {
                            Bairro = empresa.Bairro,
                            Cep = empresa.CEP,
                            CodigoMunicipio = empresa.Cidade != null ? empresa.Cidade.CodigoIbge : null,
                            Fone = empresa.Telefone,
                            Logradouro = empresa.Endereco,
                            Municipio = empresa.Cidade != null ? empresa.Cidade.Nome : null,
                            Numero = empresa.Numero,
                            UF = empresa.Cidade != null ? empresa.Cidade.Estado.Sigla : null
                        }
                    };
                    #endregion

                    #region Destinatário
                    itemTransmissao.Destinatario = new Destinatario()
                    {
                        Cnpj = cliente.TipoDocumento == "J" ? cliente.CPFCNPJ : null,
                        Cpf = cliente.TipoDocumento == "F" ? cliente.CPFCNPJ : null,
                        IndInscricaoEstadual = (IndInscricaoEstadual)Enum.Parse(typeof(IndInscricaoEstadual), cliente.TipoIndicacaoInscricaoEstadual.ToString()),
                        InscricaoEstadual = cliente.TipoIndicacaoInscricaoEstadual == TipoIndicacaoInscricaoEstadual.ContribuinteICMS ? cliente.InscricaoEstadual : null,
                        Nome = cliente.NomeComercial,
                        Endereco = new Endereco()
                        {
                            Bairro = cliente.Bairro,
                            Cep = cliente.CEP,
                            CodigoMunicipio = cliente.Cidade != null ? cliente.Cidade.CodigoIbge : null,
                            Fone = cliente.Telefone,
                            Logradouro = cliente.Endereco,
                            Municipio = cliente.Cidade != null ? cliente.Cidade.Nome : null,
                            Numero = cliente.Numero,
                            UF = cliente.Estado != null ? cliente.Estado.Sigla : null
                        }
                    };
                    #endregion

                    #region Transporte
                    itemTransmissao.Transporte = new Transporte()
                    {
                        ModalidadeFrete = (int)entity.TipoFrete
                    };
                    if (transportadora != null) {
                        itemTransmissao.Transporte.Transportadora = new Transportadora()
                        {
                            CNPJ = transportadora != null && transportadora.TipoDocumento == "J" ? transportadora.CPFCNPJ : null,
                            CPF = transportadora != null && transportadora.TipoDocumento == "F" ? transportadora.CPFCNPJ : null,
                            Endereco = transportadora != null ? transportadora.Endereco : null,
                            IE = transportadora != null ? transportadora.InscricaoEstadual : null,
                            Municipio = transportadora != null && transportadora.Cidade != null ? transportadora.Cidade.Nome : null,
                            RazaoSocial = transportadora != null ? transportadora.NomeComercial : null,
                            UF = transportadora != null && transportadora.Estado != null ? transportadora.Estado.Sigla : null
                        };
                    }
                    #endregion

                    #region Detalhes Produtos
                    itemTransmissao.Detalhes = new List<Detalhe>();
                    var num = 1;
                    var UFSiglaEmpresa = (empresa.Cidade != null ? (empresa.Cidade.Estado != null ? empresa.Cidade.Estado.Sigla : "") : "");

                    foreach (var item in NFeProdutos)
                    {
                        var st = SubstituicaoTributariaBL.AllIncluding(y => y.EstadoOrigem).AsNoTracking().Where(x =>
                            x.NcmId == (item.Produto.NcmId.HasValue ? item.Produto.NcmId.Value : Guid.NewGuid()) &
                            x.CestId == item.Produto.CestId.Value &
                            x.EstadoOrigem.Sigla == UFSiglaEmpresa &
                            x.EstadoDestinoId == cliente.EstadoId &
                            x.TipoSubstituicaoTributaria == TipoSubstituicaoTributaria.Saida
                            ).FirstOrDefault();
                        var CST = item.GrupoTributario.TipoTributacaoPIS.HasValue ? item.GrupoTributario.TipoTributacaoPIS.Value.ToString() : "";
                        var itemTributacao = new NotaFiscalItemTributacao();
                        itemTributacao = NotaFiscalItemTributacaoBL.All.Where(x => x.NotaFiscalItemId == item.Id).FirstOrDefault();

                        var produtoNFe = new ProdutoNFe()
                        {
                            CFOP = item.GrupoTributario.Cfop.Codigo,
                            Codigo = string.IsNullOrEmpty(item.Produto.CodigoProduto) ? string.Format("CFOP{0}", item.GrupoTributario.Cfop.Codigo.ToString()) : item.Produto.CodigoProduto,
                            Descricao = item.Produto.Descricao,
                            GTIN = string.IsNullOrEmpty(item.Produto.CodigoBarras) ? null : item.Produto.CodigoBarras,
                            GTIN_UnidadeMedidaTributada = string.IsNullOrEmpty(item.Produto.CodigoBarras) ? null : item.Produto.CodigoBarras,
                            NCM = item.Produto.Ncm != null ? item.Produto.Ncm.Codigo : null,
                            Quantidade = item.Quantidade.ToString(),
                            QuantidadeTributada = item.Quantidade.ToString(),
                            UnidadeMedida = item.Produto.UnidadeMedida != null ? item.Produto.UnidadeMedida.Abreviacao : null,
                            UnidadeMedidaTributada = item.Produto.UnidadeMedida != null ? item.Produto.UnidadeMedida.Abreviacao : null,
                            ValorBruto = (item.Quantidade * item.Valor),
                            ValorUnitario = item.Valor,
                            ValorUnitarioTributado = item.Valor,
                            ValorDesconto = item.Desconto,
                            AgregaTotalNota = CompoemValorTotal.Compoe,
                            CEST = item.Produto.Cest != null ? item.Produto.Cest.Codigo : null,
                            ValorFrete = Math.Round(itemTributacao.FreteValorFracionado, 2)
                        };

                        var detalhe = new Detalhe()
                        {
                            NumeroItem = num,
                            Produto = produtoNFe,
                            Imposto = new Imposto()
                        };

                        detalhe.Imposto.ICMS = new ICMSPai()
                        {
                            OrigemMercadoria = OrigemMercadoria.Nacional,
                            CodigoSituacaoOperacao = item.GrupoTributario.TipoTributacaoICMS != null ? (CSOSN)Enum.Parse(typeof(CSOSN), item.GrupoTributario.TipoTributacaoICMS.ToString()) : CSOSN.Outros,
                            AliquotaAplicavelCalculoCreditoSN = item.ValorCreditoICMS.HasValue ? Math.Round(((item.ValorCreditoICMS.Value / item.Total) * 100), 2) : 0,
                            ValorCreditoICMS = Math.Round(item.ValorCreditoICMS.HasValue ? item.ValorCreditoICMS.Value : 0, 2)
                        };

                        if (itemTributacao.CalculaICMS)
                        {
                            detalhe.Imposto.ICMS.ValorICMSSTRetido = Math.Round(item.ValorICMSSTRetido.HasValue ? item.ValorICMSSTRetido.Value : 0, 2);
                            detalhe.Imposto.ICMS.ValorICMS = Math.Round(itemTributacao.ICMSValor, 2);
                            detalhe.Imposto.ICMS.ValorBC = Math.Round(itemTributacao.ICMSBase, 2);
                        }
                        if (itemTributacao.CalculaST)
                        {
                            detalhe.Imposto.ICMS.UF = cliente.Estado != null ? cliente.Estado.Sigla : null;
                            detalhe.Imposto.ICMS.PercentualMargemValorAdicionadoST = st != null ? st.Mva : 0;
                            detalhe.Imposto.ICMS.ValorBCST = Math.Round(itemTributacao.STBase, 2);
                            detalhe.Imposto.ICMS.AliquotaICMSST = Math.Round(itemTributacao.STAliquota, 2);
                            detalhe.Imposto.ICMS.ValorICMSST = Math.Round(itemTributacao.STValor, 2);
                            detalhe.Imposto.ICMS.ValorBCSTRetido = Math.Round(item.ValorBCSTRetido.HasValue ? item.ValorBCSTRetido.Value : 0, 2);
                        }

                        detalhe.Imposto.COFINS = new COFINSPai()
                        {
                            CodigoSituacaoTributaria = item.GrupoTributario.TipoTributacaoCOFINS != null ? ((CSTPISCOFINS)(int)item.GrupoTributario.TipoTributacaoCOFINS.Value) : CSTPISCOFINS.OutrasOperacoes
                        };

                        if (itemTributacao.CalculaCOFINS)
                        {
                            detalhe.Imposto.COFINS.ValorCOFINS = Math.Round(itemTributacao.COFINSValor,2);
                            detalhe.Imposto.COFINS.ValorBC = Math.Round(itemTributacao.COFINSBase, 2);
                            detalhe.Imposto.COFINS.AliquotaPercentual = Math.Round(itemTributacao.COFINSAliquota, 2);
                        }

                        if (itemTributacao.CalculaIPI)
                        {
                            detalhe.Imposto.IPI = new IPIPai()
                            {
                                CodigoST = item.GrupoTributario.TipoTributacaoIPI.HasValue ? (CSTIPI)Enum.Parse(typeof(CSTIPI), item.GrupoTributario.TipoTributacaoIPI.Value.ToString()) : CSTIPI.OutrasSaidas,
                                ValorIPI = itemTributacao.IPIValor,
                                CodigoEnquadramento = item.Produto.EnquadramentoLegalIPI != null ? item.Produto.EnquadramentoLegalIPI.Codigo : null,
                                ValorBaseCalculo = Math.Round(itemTributacao.IPIBase, 2),
                                PercentualIPI = Math.Round(itemTributacao.IPIAliquota, 2),
                                QtdTotalUnidadeTributavel = item.Quantidade,
                                ValorUnidadeTributavel = Math.Round(item.Valor, 2)
                            };
                        }
                        detalhe.Imposto.PIS = new PISPai()
                        {                            
                            CodigoSituacaoTributaria = item.GrupoTributario.TipoTributacaoPIS.HasValue ? (CSTPISCOFINS)((int)item.GrupoTributario.TipoTributacaoPIS) : CSTPISCOFINS.IsentaDaContribuicao,
                        };

                        if (itemTributacao.CalculaPIS)
                        {
                            detalhe.Imposto.PIS.ValorPIS = Math.Round(itemTributacao.PISValor,2);
                            detalhe.Imposto.PIS.PercentualPIS = parametros.AliquotaPISPASEP;
                            detalhe.Imposto.PIS.ValorBCDoPIS = Math.Round(itemTributacao.PISBase,2);

                            if (CST == "05")
                            {
                                detalhe.Imposto.PISST = new PISST()
                                {
                                    ValorPISST = itemTributacao.PISValor,
                                };
                            }
                        }

                        detalhe.Imposto.TotalAprox = (detalhe.Imposto.COFINS != null ? detalhe.Imposto.COFINS.ValorCOFINS : 0) +
                                         (detalhe.Imposto.ICMS.ValorICMS.HasValue ? detalhe.Imposto.ICMS.ValorICMS.Value : 0) +
                                         (detalhe.Imposto.II != null ? detalhe.Imposto.II.ValorII : 0) +
                                         (detalhe.Imposto.IPI != null ? detalhe.Imposto.IPI.ValorIPI : 0) +
                                         (detalhe.Imposto.PIS != null ? detalhe.Imposto.PIS.ValorPIS : 0) +
                                         (detalhe.Imposto.PISST != null ? detalhe.Imposto.PISST.ValorPISST : 0);

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
                            SomatorioDesconto = NFeProdutos.Sum(x => x.Desconto),
                            SomatorioICMSST = itemTransmissao.Detalhes.Select(x => x.Imposto.ICMS).Any(x => x != null && x.ValorICMSST.HasValue) ? Math.Round(itemTransmissao.Detalhes.Where(x => x.Imposto.ICMS != null && x.Imposto.ICMS.ValorICMSST.HasValue).Sum(x => x.Imposto.ICMS.ValorICMSST.Value), 2) : 0,
                            ValorFrete = entity.TipoFrete == TipoFrete.CIF ? (entity.ValorFrete.HasValue ? entity.ValorFrete.Value : 0) : 0,
                            ValorSeguro = 0,
                            SomatorioIPI = itemTransmissao.Detalhes.Select(x => x.Imposto.IPI).Any(x => x != null) ? Math.Round(itemTransmissao.Detalhes.Where(x => x.Imposto.IPI != null).Sum(x => x.Imposto.IPI.ValorIPI), 2) : 0,
                            SomatorioPis = itemTransmissao.Detalhes.Sum(y => y.Imposto.PIS.ValorPIS),
                            //+(itemTransmissao.Detalhes.Select(x => x.Imposto.PISST).Any(x => x != null) ? itemTransmissao.Detalhes.Where(x => x.Imposto.PISST != null).Sum(y => y.Imposto.PISST.ValorPISST) : 0),
                            SomatorioProdutos = itemTransmissao.Detalhes.Sum(x => x.Produto.ValorBruto),
                            SomatorioOutro = 0
                        }
                    };
                    var icmsTotal = itemTransmissao.Total.ICMSTotal;
                    itemTransmissao.Total.ICMSTotal.TotalTributosAprox =
                        icmsTotal.SomatorioBC + icmsTotal.SomatorioICMS +
                        icmsTotal.SomatorioCofins +
                        icmsTotal.SomatorioICMSST +
                        icmsTotal.SomatorioIPI +
                        icmsTotal.SomatorioPis;

                    itemTransmissao.Total.ICMSTotal.ValorTotalNF =
                        ((itemTransmissao.Total.ICMSTotal.SomatorioProdutos +
                        itemTransmissao.Total.ICMSTotal.SomatorioICMSST +
                        itemTransmissao.Total.ICMSTotal.ValorFrete +
                        itemTransmissao.Total.ICMSTotal.SomatorioIPI) -
                        itemTransmissao.Total.ICMSTotal.SomatorioDesconto);
                    #endregion

                    var notaFiscal = new TransmissaoVM()
                    {
                        Homologacao = CertificadoDigitalBL.All.FirstOrDefault().EntidadeHomologacao,
                        Producao = CertificadoDigitalBL.All.FirstOrDefault().EntidadeProducao,
                        EntidadeAmbiente = (TipoAmbienteNFe)Enum.Parse(typeof(TipoAmbienteNFe), parametros.TipoAmbiente.ToString()),

                        Item = new List<ItemTransmissaoVM>()
                        {
                            itemTransmissao
                        }
                    };

                    var header = new Dictionary<string, string>()
                    {
                        { "AppUser", AppUser },
                        { "PlataformaUrl", PlataformaUrl }
                    };

                    entity.Mensagem = null;

                    var response = RestHelper.ExecutePostRequest<TransmissaoRetornoVM>(AppDefaults.UrlEmissaoNfeApi, "transmissao", JsonConvert.SerializeObject(notaFiscal), null, header);
                    var retorno = response.Notas.FirstOrDefault();
                    if (retorno.Error != null)
                    {
                        entity.Status = StatusNotaFiscal.FalhaTransmissao;
                        var mensagens = "";
                        foreach (var item in retorno.Error)
                        {
                            var schemaMensagens = "";
                            foreach (var schema in item.SchemaMensagem)
                            {
                                schemaMensagens += string.Format("  Erro: {0}\n  Descrição: {1}\n  Campo: {2}\n", schema.Erro, schema.Descricao, schema.Campo);
                            }
                            mensagens += string.Format("Mensagem: {0}\n SchemaXMLMensagens: \n{1} \n\n", item.Mensagem, schemaMensagens);
                        }
                        entity.Mensagem = mensagens;
                    }
                    else
                    {
                        entity.SefazId = retorno.NotaId;
                    }                    
                }
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        public override void Insert(NFe entity)
        {
            entity.Fail(entity.Status != StatusNotaFiscal.NaoTransmitida, new Error("Uma nova NF-e só pode estar com status 'Não Transmitida'", "status"));
            base.Insert(entity);
        }

        public override void Update(NFe entity)
        {
            var previous = All.AsNoTracking().FirstOrDefault(e => e.Id == entity.Id);
            entity.Fail(previous.Status != StatusNotaFiscal.FalhaTransmissao && previous.Status != StatusNotaFiscal.NaoTransmitida & previous.Status != StatusNotaFiscal.NaoAutorizada && entity.Status == StatusNotaFiscal.Transmitida, new Error("Para transmitir, somente notas fiscais com status anterior igual a Não Transmitida ou Não Autorizada", "status"));
            entity.Fail(
                previous.Status != StatusNotaFiscal.NaoTransmitida &&
                entity.Status != StatusNotaFiscal.Transmitida &&
                (entity.SerieNotaFiscalId != previous.SerieNotaFiscalId || entity.NumNotaFiscal != previous.NumNotaFiscal)
                , new Error("Para alterar série e número, somente notas fiscais que ainda não foram transmitidas", "status"));

            ValidaModel(entity);

            if (entity.Status == StatusNotaFiscal.Transmitida && entity.SerieNotaFiscalId.HasValue && entity.NumNotaFiscal.HasValue && entity.IsValid())
            {
                TransmitirNFe(entity);
            }

            base.Update(entity);
        }

        public override void Delete(NFe entityToDelete)
        {
            entityToDelete.Fail(true, new Error("Não é possível deletar"));
        }

        public TotalNotaFiscal CalculaTotalNFe(Guid nfeId, double? valorFreteCIF = 0)
        {
            var nfe = All.Where(x => x.Id == nfeId).FirstOrDefault();

            var produtos = NFeProdutoBL.All.Where(x => x.NotaFiscalId == nfeId).ToList();
            var totalProdutos = produtos != null ? produtos.Sum(x => ((x.Quantidade * x.Valor) - x.Desconto)) : 0.0;
            var totalImpostosProdutos = nfe.TotalImpostosProdutos;

            var result = new TotalNotaFiscal()
            {
                TotalProdutos = Math.Round(totalProdutos, 2, MidpointRounding.AwayFromZero),
                ValorFreteCIF = valorFreteCIF.HasValue ? Math.Round(valorFreteCIF.Value, 2, MidpointRounding.AwayFromZero) : 0,
                TotalImpostosProdutos = Math.Round(totalImpostosProdutos, 2, MidpointRounding.AwayFromZero),
            };

            return result;
        }
    }
}