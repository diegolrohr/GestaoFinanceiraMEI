using Fly01.Core;
using Fly01.Core.BL;
using Fly01.Core.Entities.Domains.Enum;
using Fly01.Core.Notifications;
using Fly01.Core.Rest;
using Fly01.Core.Defaults;
using Fly01.EmissaoNFE.Domain.Entities.NFe;
using Fly01.EmissaoNFE.Domain.Entities.NFe.COFINS;
using Fly01.EmissaoNFE.Domain.Entities.NFe.ICMS;
using Fly01.EmissaoNFE.Domain.Entities.NFe.IPI;
using Fly01.EmissaoNFE.Domain.Entities.NFe.PIS;
using Fly01.EmissaoNFE.Domain.Entities.NFe.Totais;
using Fly01.EmissaoNFE.Domain.ViewModel;
using Fly01.Compras.DAL;
using Fly01.Core.Entities.Domains.Commons;
using Newtonsoft.Json;
using System;
using Fly01.Core.Helpers;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Fly01.EmissaoNFE.Domain.Enums;
using Fly01.Core.ViewModels;
using Fly01.Core.ServiceBus;

namespace Fly01.Compras.BL
{
    public class NFeEntradaBL : PlataformaBaseBL<NFeEntrada>
    {
        const string CSTsICMSST = "201||202||203||900||10||30||70||90";
        protected SerieNotaFiscalBL SerieNotaFiscalBL { get; set; }
        protected NFeProdutoEntradaBL NFeProdutoEntradaBL { get; set; }
        protected TotalTributacaoBL TotalTributacaoBL { get; set; }
        protected PessoaBL PessoaBL { get; set; }
        protected CondicaoParcelamentoBL CondicaoParcelamentoBL { get; set; }
        protected FormaPagamentoBL FormaPagamentoBL { get; set; }
        protected SubstituicaoTributariaBL SubstituicaoTributariaBL { get; set; }
        protected NotaFiscalItemTributacaoEntradaBL NotaFiscalItemTributacaoEntradaBL { get; set; }
        protected CertificadoDigitalBL CertificadoDigitalBL { get; set; }
        protected NotaFiscalInutilizadaBL NotaFiscalInutilizadaBL { get; set; }
        protected EstadoBL EstadoBL { get; set; }

        public NFeEntradaBL(AppDataContext context,
                     SerieNotaFiscalBL serieNotaFiscalBL,
                     NFeProdutoEntradaBL nfeProdutoEntradaBL,
                     TotalTributacaoBL totalTributacaoBL,
                     CertificadoDigitalBL certificadoDigitalBL,
                     PessoaBL pessoaBL,
                     CondicaoParcelamentoBL condicaoParcelamentoBL,
                     SubstituicaoTributariaBL substituicaoTributariaBL,
                     NotaFiscalItemTributacaoEntradaBL notaFiscalItemTributacaoEntradaBL,
                     FormaPagamentoBL formaPagamentoBL,
                     NotaFiscalInutilizadaBL notaFiscalInutilizadaBL,
                     EstadoBL estadoBL)
            : base(context)
        {
            SerieNotaFiscalBL = serieNotaFiscalBL;
            NFeProdutoEntradaBL = nfeProdutoEntradaBL;
            TotalTributacaoBL = totalTributacaoBL;
            CertificadoDigitalBL = certificadoDigitalBL;
            PessoaBL = pessoaBL;
            CondicaoParcelamentoBL = condicaoParcelamentoBL;
            SubstituicaoTributariaBL = substituicaoTributariaBL;
            NotaFiscalItemTributacaoEntradaBL = notaFiscalItemTributacaoEntradaBL;
            FormaPagamentoBL = formaPagamentoBL;
            NotaFiscalInutilizadaBL = notaFiscalInutilizadaBL;
            EstadoBL = estadoBL;
        }

        public IQueryable<NFeEntrada> Everything => repository.All.Where(x => x.Ativo);

        public override void ValidaModel(NFeEntrada entity)
        {
            entity.Fail(entity.TipoNotaFiscal != TipoNotaFiscal.NFe, new Error("Permitido somente nota fiscal do tipo NFe"));
            entity.Fail(entity.ValorFrete.HasValue && entity.ValorFrete.Value < 0, new Error("Valor frete não pode ser negativo", "valorFrete"));
            entity.Fail(entity.PesoBruto.HasValue && entity.PesoBruto.Value < 0, new Error("Peso bruto não pode ser negativo", "pesoBruto"));
            entity.Fail(entity.PesoLiquido.HasValue && entity.PesoLiquido.Value < 0, new Error("Peso liquido não pode ser negativo", "pesoLiquido"));
            entity.Fail(entity.QuantidadeVolumes.HasValue && entity.QuantidadeVolumes.Value < 0, new Error("Quantidade de volumes não pode ser negativo", "quantidadeVolumes"));
            entity.Fail((entity.NumNotaFiscal.HasValue || entity.SerieNotaFiscalId.HasValue) && (!entity.NumNotaFiscal.HasValue || !entity.SerieNotaFiscalId.HasValue), new Error("Informe série e número da nota fiscal"));
            entity.Fail((entity.Status == StatusNotaFiscal.Transmitida && (!entity.SerieNotaFiscalId.HasValue || !entity.NumNotaFiscal.HasValue)), new Error("Para transmitir, informe série e número da nota fiscal"));

            var serieNotaFiscal = SerieNotaFiscalBL.All.AsNoTracking().FirstOrDefault(x => x.Id == entity.SerieNotaFiscalId);
            if (entity.SerieNotaFiscalId.HasValue)
            {
                entity.Fail(serieNotaFiscal == null || (serieNotaFiscal.TipoOperacaoSerieNotaFiscal != TipoOperacaoSerieNotaFiscal.NFe && serieNotaFiscal.TipoOperacaoSerieNotaFiscal != TipoOperacaoSerieNotaFiscal.Ambas), new Error("Selecione uma série ativa do tipo NF-e ou tipo ambas"));
            }


            if (entity.Status == StatusNotaFiscal.Transmitida && entity.SerieNotaFiscalId.HasValue && entity.NumNotaFiscal.HasValue)
            {
                var serieENumeroJaUsado = All.AsNoTracking().Any(x => x.Id != entity.Id && (x.SerieNotaFiscalId == entity.SerieNotaFiscalId && x.NumNotaFiscal == entity.NumNotaFiscal));
                var serieENumeroInutilizado = NotaFiscalInutilizadaBL.All.AsNoTracking().Any(x =>
                    x.Serie.ToUpper() == serieNotaFiscal.Serie.ToUpper() &&
                    x.NumNotaFiscal == entity.NumNotaFiscal);

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
                    while (NotaFiscalInutilizadaBL.All.AsNoTracking().Any(x =>
                        x.Serie.ToUpper() == serieNotaFiscal.Serie.ToUpper() &&
                        x.NumNotaFiscal == sugestaoProximoNumNota));

                    entity.Fail(true, new Error("Série e número já utilizados ou inutilizados, sugestão de número: " + sugestaoProximoNumNota.ToString(), "numNotaFiscal"));
                }
                else
                {
                    var serie = SerieNotaFiscalBL.All.Where(x => x.Id == entity.SerieNotaFiscalId).FirstOrDefault();
                    serie.NumNotaFiscal = entity.NumNotaFiscal.Value + 1;
                    SerieNotaFiscalBL.Update(serie);
                    Producer<SerieNotaFiscal>.Send(serie.GetType().Name, AppUser, PlataformaUrl, serie, RabbitConfig.EnHttpVerb.PUT);
                };
            }

            base.ValidaModel(entity);
        }

        public void TransmitirNFe(NFeEntrada entity)
        {
            try
            {
                if (!TotalTributacaoBL.ConfiguracaoTSSOK())
                {
                    throw new BusinessException("Configuração inválida para comunicação com TSS, verifique os dados da empresa, seu certificado digital e parâmetros tributários");
                }
                else
                {
                    var parametros = TotalTributacaoBL.GetParametrosTributarios();
                    if (parametros == null)
                    {
                        throw new BusinessException("Acesse o menu Configurações > Parâmetros Tributários e salve as configurações para a transmissão");
                    }

                    if (parametros.TipoVersaoNFe != TipoVersaoNFe.v4)
                    {
                        throw new BusinessException("Permitido somente NF-e versão 4.00. Acesse o menu Configurações > Parâmetros Tributários e altere as configurações");
                    }

                    var isLocal = AppDefaults.UrlGateway.Contains("bemacashlocal.com.br");

                    var versao = EnumHelper.GetValue(typeof(TipoVersaoNFe), parametros.TipoVersaoNFe.ToString());
                    var fornecedor = TotalTributacaoBL.GetPessoa(entity.FornecedorId);
                    var empresa = ApiEmpresaManager.GetEmpresa(PlataformaUrl);
                    var utcDefault = "E. South America Standard Time";
                    var utcId = empresa.Cidade != null ? (empresa.Cidade.Estado != null ? empresa.Cidade.Estado.UtcId : utcDefault) : utcDefault;

                    var condicaoParcelamento = CondicaoParcelamentoBL.All.AsNoTracking().Where(x => x.Id == entity.CondicaoParcelamentoId).FirstOrDefault();
                    var formaPagamento = FormaPagamentoBL.All.AsNoTracking().Where(x => x.Id == entity.FormaPagamentoId).FirstOrDefault();
                    var transportadora = PessoaBL.AllIncluding(x => x.Estado, x => x.Cidade).Where(x => x.Transportadora && x.Id == entity.TransportadoraId).AsNoTracking().FirstOrDefault();
                    var serieNotaFiscal = SerieNotaFiscalBL.All.AsNoTracking().Where(x => x.Id == entity.SerieNotaFiscalId).FirstOrDefault();
                    var NFeProdutos = NFeProdutoEntradaBL.AllIncluding(
                        x => x.GrupoTributario.Cfop,
                        x => x.Produto.Ncm,
                        x => x.Produto.Cest,
                        x => x.Produto.UnidadeMedida,
                        x => x.Produto.EnquadramentoLegalIPI).AsNoTracking().Where(x => x.NotaFiscalEntradaId == entity.Id);

                    var destinoOperacao = TipoDestinoOperacao.Interna;
                    if (fornecedor.Estado != null && fornecedor.Estado.Sigla.ToUpper() == "EX" || (empresa.Cidade.Estado != null ? empresa.Cidade.Estado.Sigla.ToUpper() : "") == "EX")
                    {
                        destinoOperacao = TipoDestinoOperacao.Exterior;
                    }
                    else if (fornecedor.Estado != null && empresa.Cidade.Estado != null && (fornecedor.Estado.Sigla.ToUpper() != empresa.Cidade.Estado.Sigla.ToUpper()))
                    {
                        destinoOperacao = TipoDestinoOperacao.Interestadual;
                    }
                    var formPag = FormaPagamentoEmissaoNFE.Outros;
                    if (condicaoParcelamento != null)
                    {
                        formPag = (condicaoParcelamento.QtdParcelas == 1 || condicaoParcelamento.CondicoesParcelamento == "0") ? FormaPagamentoEmissaoNFE.AVista : FormaPagamentoEmissaoNFE.APrazo;
                    }

                    var itemTransmissao = new ItemTransmissaoVM();
                    itemTransmissao.Versao = versao;

                    #region Identificação
                    itemTransmissao.Identificador = new Identificador()
                    {
                        CodigoUF = empresa.Cidade != null ? (empresa.Cidade.Estado != null ? int.Parse(empresa.Cidade.Estado.CodigoIbge) : 0) : 0,
                        NaturezaOperacao = entity.NaturezaOperacao,
                        ModeloDocumentoFiscal = 55,
                        Serie = int.Parse(serieNotaFiscal.Serie),
                        NumeroDocumentoFiscal = entity.NumNotaFiscal.Value,
                        Emissao = TimeZoneHelper.GetDateTimeNow(isLocal, utcId),
                        EntradaSaida = TimeZoneHelper.GetDateTimeNow(isLocal, utcId),
                        TipoDocumentoFiscal = entity.TipoCompra == TipoCompraVenda.Devolucao ? TipoNota.Saida : TipoNota.Entrada,
                        DestinoOperacao = destinoOperacao,
                        CodigoMunicipio = empresa.Cidade != null ? empresa.Cidade.CodigoIbge : null,
                        ImpressaoDANFE = TipoImpressaoDanfe.Retrato,
                        ChaveAcessoDV = 0,
                        CodigoNF = entity.NumNotaFiscal.Value.ToString(),
                        Ambiente = parametros.TipoAmbiente,
                        FinalidadeEmissaoNFe = entity.TipoCompra,
                        ConsumidorFinal = fornecedor.ConsumidorFinal ? 1 : 0,
                        PresencaComprador = parametros.TipoPresencaComprador,
                        Versao = "2.78",//versao do TSS
                        FormaEmissao = parametros.TipoModalidade,
                        CodigoProcessoEmissaoNFe = 0
                    };
                    if (entity.TipoCompra == TipoCompraVenda.Devolucao)
                    {
                        itemTransmissao.Identificador.NFReferenciada = new NFReferenciada()
                        {
                            ChaveNFeReferenciada = entity.ChaveNFeReferenciada
                        };
                    }
                    #endregion

                    #region Emitente
                    itemTransmissao.Emitente = new Emitente()
                    {
                        Cnpj = empresa.CNPJ,
                        Nome = empresa.RazaoSocial,
                        NomeFantasia = empresa.NomeFantasia,
                        InscricaoEstadual = empresa.InscricaoEstadual,
                        CRT = parametros.TipoCRT,
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
                        Cnpj = fornecedor.TipoDocumento == "J" ? fornecedor.CPFCNPJ : null,
                        Cpf = fornecedor.TipoDocumento == "F" ? fornecedor.CPFCNPJ : null,
                        IndInscricaoEstadual = fornecedor.TipoIndicacaoInscricaoEstadual,
                        InscricaoEstadual = fornecedor.TipoIndicacaoInscricaoEstadual == TipoIndicacaoInscricaoEstadual.ContribuinteICMS ? fornecedor.InscricaoEstadual : null,
                        IdentificacaoEstrangeiro = null,
                        Nome = fornecedor.Nome,
                        Endereco = new Endereco()
                        {
                            Bairro = fornecedor.Bairro,
                            Cep = fornecedor.CEP,
                            CodigoMunicipio = fornecedor.Cidade != null ? fornecedor.Cidade.CodigoIbge : null,
                            Fone = fornecedor.Telefone,
                            Logradouro = fornecedor.Endereco,
                            Municipio = fornecedor.Cidade != null ? fornecedor.Cidade.Nome : null,
                            Numero = fornecedor.Numero,
                            UF = fornecedor.Estado != null ? fornecedor.Estado.Sigla : null
                        }
                    };
                    #endregion

                    #region Transporte
                    itemTransmissao.Transporte = new Transporte()
                    {
                        ModalidadeFrete = entity.TipoFrete
                    };
                    if (transportadora != null)
                    {
                        itemTransmissao.Transporte.Transportadora = new Transportadora()
                        {
                            CNPJ = transportadora != null && transportadora?.TipoDocumento == "J" ? transportadora?.CPFCNPJ : null,
                            CPF = transportadora != null && transportadora?.TipoDocumento == "F" ? transportadora?.CPFCNPJ : null,
                            Endereco = transportadora != null ? transportadora?.Endereco : null,
                            IE = transportadora != null ? (transportadora?.TipoIndicacaoInscricaoEstadual == TipoIndicacaoInscricaoEstadual.ContribuinteICMS ? transportadora?.InscricaoEstadual : null) : null,
                            Municipio = transportadora != null && transportadora?.Cidade != null ? transportadora?.Cidade?.Nome : null,
                            RazaoSocial = transportadora != null ? transportadora?.Nome : null,
                            UF = transportadora != null && transportadora?.Estado != null ? transportadora?.Estado?.Sigla : null
                        };
                    }
                    #endregion

                    #region Detalhes Produtos
                    itemTransmissao.Detalhes = new List<Detalhe>();
                    var num = 1;
                    string UFSiglaEmpresa = (empresa.Cidade != null ? (empresa.Cidade.Estado != null ? empresa.Cidade.Estado.Sigla : "") : "");

                    foreach (var item in NFeProdutos)
                    {
                        var st = SubstituicaoTributariaBL.AllIncluding(y => y.EstadoOrigem).AsNoTracking().Where(x =>
                            x.NcmId == (item.Produto.NcmId.HasValue ? item.Produto.NcmId.Value : Guid.NewGuid()) &&
                            ((item.Produto.CestId.HasValue && x.CestId == item.Produto.CestId.Value) || !item.Produto.CestId.HasValue) &&
                            x.EstadoOrigem.Sigla == UFSiglaEmpresa &&
                            x.EstadoDestinoId == fornecedor.EstadoId &&
                            x.TipoSubstituicaoTributaria == (entity.TipoCompra == TipoCompraVenda.Devolucao ? TipoSubstituicaoTributaria.Saida : TipoSubstituicaoTributaria.Entrada)
                            ).FirstOrDefault();
                        var CST = item.GrupoTributario.TipoTributacaoPIS.HasValue ? item.GrupoTributario.TipoTributacaoPIS.Value.ToString() : "";
                        var itemTributacao = new NotaFiscalItemTributacaoEntrada();
                        itemTributacao = NotaFiscalItemTributacaoEntradaBL.All.Where(x => x.NotaFiscalItemEntradaId == item.Id).FirstOrDefault();

                        var produtoNFe = new EmissaoNFE.Domain.Entities.NFe.Produto()
                        {
                            CFOP = item.GrupoTributario.Cfop?.Codigo,
                            Codigo = string.IsNullOrEmpty(item.Produto.CodigoProduto) ? string.Format("CFOP{0}", item.GrupoTributario.Cfop?.Codigo.ToString()) : item.Produto.CodigoProduto,
                            Descricao = item.Produto.Descricao,
                            GTIN = string.IsNullOrEmpty(item.Produto.CodigoBarras) ? "SEM GETIN" : item.Produto.CodigoBarras,
                            GTIN_UnidadeMedidaTributada = string.IsNullOrEmpty(item.Produto.CodigoBarras) ? "SEM GETIN" : item.Produto.CodigoBarras,
                            NCM = item.Produto.Ncm != null ? item.Produto.Ncm.Codigo : null,
                            TipoProduto = item.Produto.TipoProduto,
                            Quantidade = Math.Round(item.Quantidade, 4),
                            QuantidadeTributada = Math.Round(item.Quantidade, 4),
                            UnidadeMedida = item.Produto.UnidadeMedida != null ? item.Produto.UnidadeMedida.Abreviacao : null,
                            UnidadeMedidaTributada = item.Produto.UnidadeMedida != null ? item.Produto.UnidadeMedida.Abreviacao : null,
                            ValorBruto = Math.Round((item.Quantidade > 0 ? (Math.Round(item.Quantidade, 4) * Math.Round(item.Valor, 4)) : Math.Round(item.Valor, 4)), 2),
                            ValorUnitario = Math.Round(item.Valor, 4),
                            ValorUnitarioTributado = Math.Round(item.Valor, 4),
                            ValorDesconto = Math.Round(item.Desconto, 2),
                            AgregaTotalNota = CompoemValorTotal.Compoe,
                            CEST = item.Produto.Cest != null ? item.Produto.Cest.Codigo : null,
                            ValorFrete = entity.TipoFrete == TipoFrete.FOB ? Math.Round(itemTributacao.FreteValorFracionado, 2) : 0
                        };

                        var detalhe = new Detalhe()
                        {
                            NumeroItem = num,
                            Produto = produtoNFe,
                            Imposto = new Imposto()
                        };

                        detalhe.Imposto.ICMS = new ICMSPai()
                        {
                            OrigemMercadoria = item.Produto.OrigemMercadoria,
                            CodigoSituacaoOperacao = item.GrupoTributario.TipoTributacaoICMS != null ? item.GrupoTributario.TipoTributacaoICMS.Value : TipoTributacaoICMS.TributadaSemPermissaoDeCredito,
                            AliquotaAplicavelCalculoCreditoSN = Math.Round(((item.ValorCreditoICMS / (item.Quantidade * item.Valor)) * 100), 2),
                            ValorCreditoICMS = Math.Round(item.ValorCreditoICMS, 2),
                            TipoCRT = parametros.TipoCRT,
                        };

                        if (itemTributacao.CalculaICMS)
                        {
                            detalhe.Imposto.ICMS.ValorICMSSTRetido = Math.Round(item.ValorICMSSTRetido, 2);

                            if (item.GrupoTributario.TipoTributacaoICMS == TipoTributacaoICMS.Outros
                                || item.GrupoTributario.TipoTributacaoICMS == TipoTributacaoICMS.Outros90
                                || item.GrupoTributario.TipoTributacaoICMS == TipoTributacaoICMS.TributadaIntegralmente
                                || item.GrupoTributario.TipoTributacaoICMS == TipoTributacaoICMS.ComReducaoDeBaseDeCalculo
                                || item.GrupoTributario.TipoTributacaoICMS == TipoTributacaoICMS.ICMSCobradoAnteriormentePorST
                                || item.GrupoTributario.TipoTributacaoICMS == TipoTributacaoICMS.ComRedDeBaseDeST
                                || item.GrupoTributario.TipoTributacaoICMS == TipoTributacaoICMS.TributadaComCobrancaDeSubstituicao
                                || item.GrupoTributario.TipoTributacaoICMS == TipoTributacaoICMS.Diferimento
                                )
                            {
                                detalhe.Imposto.ICMS.ModalidadeBC = ModalidadeDeterminacaoBCICMS.ValorDaOperacao;
                                detalhe.Imposto.ICMS.AliquotaICMS = Math.Round(itemTributacao.ICMSAliquota, 2);
                                detalhe.Imposto.ICMS.ModalidadeBCST = ModalidadeDeterminacaoBCICMSST.MargemValorAgregado;
                                detalhe.Imposto.ICMS.ValorICMS = Math.Round(itemTributacao.ICMSValor, 2);
                                detalhe.Imposto.ICMS.ValorBC = Math.Round(itemTributacao.ICMSBase, 2);
                            }
                            if (item.GrupoTributario.TipoTributacaoICMS == TipoTributacaoICMS.TributadaComPermissaoDeCreditoST
                                || item.GrupoTributario.TipoTributacaoICMS == TipoTributacaoICMS.TributadaSemPermissaoDeCreditoST
                                || item.GrupoTributario.TipoTributacaoICMS == TipoTributacaoICMS.IsencaoParaFaixaDeReceitaBrutaST)
                            {
                                detalhe.Imposto.ICMS.ModalidadeBCST = ModalidadeDeterminacaoBCICMSST.MargemValorAgregado;
                                detalhe.Imposto.ICMS.PercentualReducaoBCST = 0;
                            }
                        }
                        if (itemTributacao.CalculaST)
                        {
                            detalhe.Imposto.ICMS.UF = fornecedor.Estado != null ? fornecedor.Estado.Sigla : null;
                            detalhe.Imposto.ICMS.PercentualMargemValorAdicionadoST = st != null ? st.Mva : 0;
                            detalhe.Imposto.ICMS.ValorBCST = Math.Round(itemTributacao.STBase, 2);
                            detalhe.Imposto.ICMS.AliquotaICMSST = Math.Round(itemTributacao.STAliquota, 2);
                            detalhe.Imposto.ICMS.ValorICMSST = Math.Round(itemTributacao.STValor, 2);
                            detalhe.Imposto.ICMS.ValorBCSTRetido = Math.Round(item.ValorBCSTRetido, 2);
                            detalhe.Imposto.ICMS.ValorICMSSTRetido = Math.Round(item.ValorICMSSTRetido, 2);

                            if (versao == "4.00")
                            {
                                // FCP (201, 202, 203 e 900)
                                detalhe.Imposto.ICMS.BaseFCPST = Math.Round(itemTributacao.FCPSTBase, 2);
                                detalhe.Imposto.ICMS.AliquotaFCPST = Math.Round(itemTributacao.FCPSTAliquota, 2);
                                detalhe.Imposto.ICMS.ValorFCPST = Math.Round(itemTributacao.FCPSTValor, 2);
                                // FCP (500)
                                var AliquotaFCPSTRetido = item.ValorBCFCPSTRetidoAnterior > 0 ? Math.Round(((item.ValorFCPSTRetidoAnterior / item.ValorBCFCPSTRetidoAnterior) * 100), 2) : 0;
                                detalhe.Imposto.ICMS.BaseFCPSTRetido = Math.Round(item.ValorBCFCPSTRetidoAnterior, 2);
                                detalhe.Imposto.ICMS.AliquotaFCPSTRetido = AliquotaFCPSTRetido;
                                detalhe.Imposto.ICMS.ValorFCPSTRetido = Math.Round(item.ValorFCPSTRetidoAnterior, 2);
                                detalhe.Imposto.ICMS.AliquotaConsumidorFinal = itemTributacao.STAliquota > 0 ? Math.Round(itemTributacao.STAliquota, 2) + AliquotaFCPSTRetido : 0;
                            }
                        }

                        if (itemTributacao.CalculaIPI)
                        {
                            detalhe.Imposto.IPI = new IPIPai()
                            {
                                CodigoST = item.GrupoTributario.TipoTributacaoIPI.HasValue ?
                                    (CSTIPI)System.Enum.Parse(typeof(CSTIPI), item.GrupoTributario.TipoTributacaoIPI.Value.ToString()) :
                                    CSTIPI.OutrasEntradas,
                                ValorIPI = itemTributacao.IPIValor,
                                CodigoEnquadramento = item.Produto.EnquadramentoLegalIPI != null ?
                                    item.Produto.EnquadramentoLegalIPI.Codigo :
                                    null,
                                ValorBaseCalculo = Math.Round(itemTributacao.IPIBase, 2),
                                PercentualIPI = Math.Round(itemTributacao.IPIAliquota, 2),
                                QtdTotalUnidadeTributavel = item.Quantidade,
                                ValorUnidadeTributavel = Math.Round(item.Valor, 2)
                            };
                        }
                        detalhe.Imposto.PIS = new PISPai()
                        {
                            CodigoSituacaoTributaria = item.GrupoTributario.TipoTributacaoPIS.HasValue && itemTributacao.CalculaPIS ?
                                (CSTPISCOFINS)((int)item.GrupoTributario.TipoTributacaoPIS) :
                                CSTPISCOFINS.IsentaDaContribuicao,
                        };

                        if (itemTributacao.CalculaPIS)
                        {
                            var NaoTributaveis = "04||05||06||07||08||09";
                            if (!NaoTributaveis.Contains(((int)detalhe.Imposto.PIS.CodigoSituacaoTributaria).ToString()))
                            {
                                detalhe.Imposto.PIS.ValorPIS = Math.Round(itemTributacao.PISValor, 2);
                                detalhe.Imposto.PIS.PercentualPIS = parametros.AliquotaPISPASEP;
                                detalhe.Imposto.PIS.ValorBCDoPIS = Math.Round(itemTributacao.PISBase, 2);
                            }

                            if (CST == "05")
                            {
                                detalhe.Imposto.PISST = new PISST()
                                {
                                    ValorPISST = itemTributacao.PISValor,
                                };
                            }
                        }

                        detalhe.Imposto.COFINS = new COFINSPai()
                        {
                            CodigoSituacaoTributaria = item.GrupoTributario.TipoTributacaoCOFINS != null && itemTributacao.CalculaCOFINS ?
                                ((CSTPISCOFINS)(int)item.GrupoTributario.TipoTributacaoCOFINS.Value) :
                                CSTPISCOFINS.OutrasOperacoes
                        };

                        if (itemTributacao.CalculaCOFINS)
                        {
                            var NaoTributaveis = "04||05||06||07||08||09";
                            if (!NaoTributaveis.Contains(((int)detalhe.Imposto.COFINS.CodigoSituacaoTributaria).ToString()))
                            {
                                detalhe.Imposto.COFINS.ValorCOFINS = Math.Round(itemTributacao.COFINSValor, 2);
                                detalhe.Imposto.COFINS.ValorBC = Math.Round(itemTributacao.COFINSBase, 2);
                                detalhe.Imposto.COFINS.AliquotaPercentual = Math.Round(itemTributacao.COFINSAliquota, 2);
                            }
                        }

                        detalhe.Imposto.TotalAprox = (detalhe.Imposto.COFINS != null ? detalhe.Imposto.COFINS.ValorCOFINS : 0) +
                                                    (detalhe.Imposto.ICMS.ValorICMS ?? 0) +
                                                    (detalhe.Imposto.ICMS.ValorFCPST ?? 0) +
                                                    (CSTsICMSST.Contains(((int)detalhe.Imposto.ICMS.CodigoSituacaoOperacao).ToString()) ? (detalhe.Imposto.ICMS.ValorICMSST ?? 0) : 0) +
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
                            SomatorioBCST = itemTransmissao.Detalhes.Select(x => x.Imposto.ICMS).Any(x => x != null && x.ValorBCST.HasValue &&
                            (
                                x.CodigoSituacaoOperacao == TipoTributacaoICMS.TributadaSemPermissaoDeCreditoST ||
                                x.CodigoSituacaoOperacao == TipoTributacaoICMS.Outros ||
                                x.CodigoSituacaoOperacao == TipoTributacaoICMS.TributadaComPermissaoDeCreditoST ||
                                x.CodigoSituacaoOperacao == TipoTributacaoICMS.IsencaoParaFaixaDeReceitaBrutaST
                            )) ?
                            Math.Round(itemTransmissao.Detalhes.Where(x => x.Imposto.ICMS != null && x.Imposto.ICMS.ValorBCST.HasValue &&
                            (
                                x.Imposto.ICMS.CodigoSituacaoOperacao == TipoTributacaoICMS.TributadaSemPermissaoDeCreditoST ||
                                x.Imposto.ICMS.CodigoSituacaoOperacao == TipoTributacaoICMS.Outros ||
                                x.Imposto.ICMS.CodigoSituacaoOperacao == TipoTributacaoICMS.TributadaComPermissaoDeCreditoST ||
                                x.Imposto.ICMS.CodigoSituacaoOperacao == TipoTributacaoICMS.IsencaoParaFaixaDeReceitaBrutaST
                            )).Sum(x => x.Imposto.ICMS.ValorBCST.Value), 2) : 0,
                            SomatorioCofins = itemTransmissao.Detalhes.Select(x => x.Imposto.COFINS).Any(x => x != null) ? Math.Round(itemTransmissao.Detalhes.Sum(x => x.Imposto.COFINS.ValorCOFINS), 2) : 0,
                            SomatorioDesconto = NFeProdutos.Sum(x => x.Desconto),
                            SomatorioICMSST = itemTransmissao.Detalhes.Select(x => x.Imposto.ICMS).Any(x => x != null && x.ValorICMSST.HasValue && CSTsICMSST.Contains(((int)x.CodigoSituacaoOperacao).ToString()))
                                ? Math.Round(itemTransmissao.Detalhes.Where(x => x.Imposto.ICMS != null && x.Imposto.ICMS.ValorICMSST.HasValue && CSTsICMSST.Contains(((int)x.Imposto.ICMS.CodigoSituacaoOperacao).ToString())).Sum(x => x.Imposto.ICMS.ValorICMSST.Value), 2) : 0,
                            ValorFrete = entity.TipoFrete == TipoFrete.FOB ? itemTransmissao.Detalhes.Sum(x => x.Produto.ValorFrete.Value) : 0,
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

                    var tipoFormaPagamento = TipoFormaPagamento.Outros;
                    if (formaPagamento != null)
                    {
                        //Transferência não existe para o SEFAZ
                        tipoFormaPagamento = formaPagamento.TipoFormaPagamento == TipoFormaPagamento.Transferencia ? TipoFormaPagamento.Outros : formaPagamento.TipoFormaPagamento;
                    }
                    if (entity.TipoCompra == TipoCompraVenda.Devolucao)
                    {
                        tipoFormaPagamento = TipoFormaPagamento.SemPagamento;
                    }

                    #region Fatura
                    if (entity.GeraFinanceiro)
                    {
                        itemTransmissao.Cobranca = ObterCobranca(entity.NumNotaFiscal.Value, itemTransmissao.Identificador.TipoDocumentoFiscal, entity.ContaFinanceiraParcelaPaiId);
                    }
                    #endregion

                    #region Pagamento
                    itemTransmissao.Pagamento = new Pagamento()
                    {
                        DetalhesPagamentos = new List<DetalhePagamento>()
                        {
                            new DetalhePagamento()
                            {
                                TipoFormaPagamento = tipoFormaPagamento,
                                ValorPagamento = tipoFormaPagamento == TipoFormaPagamento.SemPagamento ? 0.0 : itemTransmissao.Total.ICMSTotal.ValorTotalNF
                            }
                        }
                    };
                    #endregion

                    if (!string.IsNullOrEmpty(entity.MensagemPadraoNota))
                    {
                        itemTransmissao.InformacoesAdicionais = new InformacoesAdicionais()
                        {
                            InformacoesComplementares = entity.Observacao + " | " + entity.MensagemPadraoNota
                        };
                    }

                    var entidade = CertificadoDigitalBL.GetEntidade();

                    var notaFiscal = new TransmissaoVM()
                    {
                        Homologacao = entidade.Homologacao,
                        Producao = entidade.Producao,
                        EntidadeAmbiente = entidade.EntidadeAmbiente,
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
                    entity.Recomendacao = null;
                    entity.XML = null;
                    entity.PDF = null;
                    entity.CertificadoDigitalId = CertificadoDigitalBL.CertificadoAtualValido()?.Id;
                    entity.TipoAmbiente = entidade.EntidadeAmbiente;

                    var response = RestHelper.ExecutePostRequest<TransmissaoRetornoVM>(AppDefaults.UrlEmissaoNfeApi, "transmissao", JsonConvert.SerializeObject(notaFiscal, JsonSerializerSetting.Edit), null, header);
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
                        entity.XML = retorno.XML;
                        entity.Mensagem = mensagens;
                    }
                    else
                    {
                        entity.SefazId = retorno.NotaId;
                        entity.XML = retorno.XML;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        public Cobranca ObterCobranca(int numNotaFiscal, TipoNota tipoNota, Guid? contaFinanceiraParcelaPaiId)
        {
            var contas = ObterContasFinanceiras(tipoNota, contaFinanceiraParcelaPaiId);
            if (contas != null && contas.Any())
            {
                var cobranca = new Cobranca()
                {
                    Fatura = new Fatura()
                    {
                        NumeroFatura = "NF:" + numNotaFiscal.ToString(),
                        ValorOriginario = contas.Sum(x => x.ValorPrevisto),
                        ValorLiquido = contas.Sum(x => x.ValorPrevisto),
                        ValorDesconto = 0.0
                    }
                };
                var num = 1;
                cobranca.Duplicatas = new List<Duplicata>();
                foreach (var item in contas.OrderBy(x => x.DataVencimento))
                {
                    cobranca.Duplicatas.Add(
                        new Duplicata()
                        {
                            Numero = num.ToString().PadLeft(3, '0'),
                            ValorDuplicata = item.ValorPrevisto,
                            Vencimento = item.DataVencimento
                        });
                    num++;
                }
                return cobranca;
            }
            return null;
        }

        public List<ContaFinanceira> ObterContasFinanceiras(TipoNota tipoNota, Guid? contaFinanceiraParcelaPaiId)
        {
            try
            {
                var header = new Dictionary<string, string>()
                {
                    { "AppUser", AppUser  },
                    { "PlataformaUrl", PlataformaUrl }
                };
                var queryString = new Dictionary<string, string>()
                {
                    {
                        "contaFinanceiraParcelaPaiId",
                            contaFinanceiraParcelaPaiId.HasValue
                            ? contaFinanceiraParcelaPaiId.Value.ToString()
                            : default(Guid).ToString()
                    }
                };

                var contas = new List<ContaFinanceira>();
                if (tipoNota == TipoNota.Saida)
                {
                    var response = RestHelper.ExecuteGetRequest<ResultBase<ContaReceber>>(AppDefaults.UrlFinanceiroApi, "contareceberparcelas", header, queryString);
                    contas.AddRange(response.Data.Cast<ContaFinanceira>().ToList());
                }
                else
                {
                    var response = RestHelper.ExecuteGetRequest<ResultBase<ContaPagar>>(AppDefaults.UrlFinanceiroApi, "contapagarparcelas", header, queryString);
                    contas.AddRange(response.Data.Cast<ContaFinanceira>().ToList());
                }

                return contas;
            }
            catch (Exception ex)
            {
                throw new BusinessException("Erro ao tentar obter as contas financeiras parcelas. " + ex.Message);
            }
        }

        public override void Insert(NFeEntrada entity)
        {
            GetIdPlacaEstado(entity);
            base.Insert(entity);
        }

        public void GetIdPlacaEstado(NFeEntrada entity)
        {
            if (!entity.EstadoPlacaVeiculoId.HasValue && !string.IsNullOrEmpty(entity.EstadoCodigoIbge))
            {
                var dadosEstado = EstadoBL.All.AsNoTracking().FirstOrDefault(x => x.CodigoIbge == entity.EstadoCodigoIbge);
                if (dadosEstado != null)
                {
                    entity.EstadoPlacaVeiculoId = dadosEstado.Id;
                }
            }
        }

        public override void Update(NFeEntrada entity)
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

        public override void Delete(NFeEntrada entityToDelete)
        {
            var status = entityToDelete.Status;
            entityToDelete.Fail(status != StatusNotaFiscal.NaoAutorizada && status != StatusNotaFiscal.NaoTransmitida && status != StatusNotaFiscal.FalhaTransmissao, new Error("Só é possível deletar NF-e com status Não Autorizada, Não Transmitida ou Falha na Transmissão", "status"));
            if (entityToDelete.IsValid())
            {
                base.Delete(entityToDelete);
            }
            else
            {
                throw new BusinessException(entityToDelete.Notification.Get());
            }
        }

        public TotalPedidoNotaFiscal CalculaTotalNFe(Guid nfeId)
        {
            var nfe = All.Where(x => x.Id == nfeId).AsNoTracking().FirstOrDefault();

            var produtos = NFeProdutoEntradaBL.All.AsNoTracking().Where(x => x.NotaFiscalEntradaId == nfeId).ToList();
            var totalProdutos = produtos != null ? produtos.Sum(x => ((x.Quantidade * x.Valor) - x.Desconto)) : 0.0;
            var totalImpostosProdutos = nfe.TotalImpostosProdutos;
            var totalImpostosProdutosNaoAgrega = nfe.TotalImpostosProdutosNaoAgrega;
            bool somaFrete = (
                nfe.TipoFrete == TipoFrete.FOB
            );

            var result = new TotalPedidoNotaFiscal()
            {
                TotalProdutos = Math.Round(totalProdutos, 2, MidpointRounding.AwayFromZero),
                ValorFrete = (somaFrete && nfe.ValorFrete.HasValue) ? Math.Round(nfe.ValorFrete.Value, 2, MidpointRounding.AwayFromZero) : 0,
                TotalImpostosProdutos = Math.Round(totalImpostosProdutos, 2, MidpointRounding.AwayFromZero),
                TotalImpostosProdutosNaoAgrega = Math.Round(totalImpostosProdutosNaoAgrega, 2, MidpointRounding.AwayFromZero),
            };

            return result;
        }
    }
}