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
using Fly01.Faturamento.DAL;
using Fly01.Core.Entities.Domains.Commons;
using Newtonsoft.Json;
using System;
using Fly01.Core.Helpers;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Fly01.EmissaoNFE.Domain.Enums;

namespace Fly01.Faturamento.BL
{
    public class NFeBL : PlataformaBaseBL<NFe>
    {
        protected SerieNotaFiscalBL SerieNotaFiscalBL { get; set; }
        protected NFeProdutoBL NFeProdutoBL { get; set; }
        protected TotalTributacaoBL TotalTributacaoBL { get; set; }
        protected PessoaBL PessoaBL { get; set; }
        protected CondicaoParcelamentoBL CondicaoParcelamentoBL { get; set; }
        protected FormaPagamentoBL FormaPagamentoBL { get; set; }
        protected SubstituicaoTributariaBL SubstituicaoTributariaBL { get; set; }
        protected NotaFiscalItemTributacaoBL NotaFiscalItemTributacaoBL { get; set; }
        protected CertificadoDigitalBL CertificadoDigitalBL { get; set; }
        protected NotaFiscalInutilizadaBL NotaFiscalInutilizadaBL { get; set; }

        public NFeBL(AppDataContext context,
                     SerieNotaFiscalBL serieNotaFiscalBL,
                     NFeProdutoBL nfeProdutoBL,
                     TotalTributacaoBL totalTributacaoBL,
                     CertificadoDigitalBL certificadoDigitalBL,
                     PessoaBL pessoaBL,
                     CondicaoParcelamentoBL condicaoParcelamentoBL,
                     SubstituicaoTributariaBL substituicaoTributariaBL,
                     NotaFiscalItemTributacaoBL notaFiscalItemTributacaoBL,
                     FormaPagamentoBL formaPagamentoBL,
                     NotaFiscalInutilizadaBL notaFiscalInutilizadaBL)
            : base(context)
        {
            SerieNotaFiscalBL = serieNotaFiscalBL;
            NFeProdutoBL = nfeProdutoBL;
            TotalTributacaoBL = totalTributacaoBL;
            CertificadoDigitalBL = certificadoDigitalBL;
            PessoaBL = pessoaBL;
            CondicaoParcelamentoBL = condicaoParcelamentoBL;
            SubstituicaoTributariaBL = substituicaoTributariaBL;
            NotaFiscalItemTributacaoBL = notaFiscalItemTributacaoBL;
            FormaPagamentoBL = formaPagamentoBL;
            NotaFiscalInutilizadaBL = notaFiscalInutilizadaBL;
        }

        public IQueryable<NFe> Everything => repository.All.Where(x => x.Ativo);

        public override void ValidaModel(NFe entity)
        {
            entity.Fail(entity.TipoNotaFiscal != TipoNotaFiscal.NFe, new Error("Permitido somente nota fiscal do tipo NFe"));

            if (entity.TipoFrete != TipoFrete.SemFrete)
            {
                entity.Fail(entity.ValorFrete.HasValue && entity.ValorFrete.Value < 0, new Error("Valor frete não pode ser negativo", "valorFrete"));
                entity.Fail(entity.PesoBruto.HasValue && entity.PesoBruto.Value < 0, new Error("Peso bruto não pode ser negativo", "pesoBruto"));
                entity.Fail(entity.PesoLiquido.HasValue && entity.PesoLiquido.Value < 0, new Error("Peso liquido não pode ser negativo", "pesoLiquido"));
            }

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

                    if (parametros.TipoVersaoNFe != TipoVersaoNFe.v4)
                    {
                        throw new BusinessException("Permitido somente NF-e versão 4.00. Acesse o menu Configurações > Parâmetros Tributários e altere as configurações");
                    }

                    var isLocal = AppDefaults.UrlGateway.Contains("fly01local.com.br");

                    var versao = EnumHelper.GetValue(typeof(TipoVersaoNFe), parametros.TipoVersaoNFe.ToString());
                    var cliente = TotalTributacaoBL.GetPessoa(entity.ClienteId);
                    var empresa = ApiEmpresaManager.GetEmpresa(PlataformaUrl);
                    var condicaoParcelamento = CondicaoParcelamentoBL.All.AsNoTracking().Where(x => x.Id == entity.CondicaoParcelamentoId).FirstOrDefault();
                    var formaPagamento = FormaPagamentoBL.All.AsNoTracking().Where(x => x.Id == entity.FormaPagamentoId).FirstOrDefault();
                    var transportadora = PessoaBL.AllIncluding(x => x.Estado, x => x.Cidade).Where(x => x.Transportadora && x.Id == entity.TransportadoraId).AsNoTracking().FirstOrDefault();
                    var serieNotaFiscal = SerieNotaFiscalBL.All.AsNoTracking().Where(x => x.Id == entity.SerieNotaFiscalId).FirstOrDefault();
                    var NFeProdutos = NFeProdutoBL.AllIncluding(
                        x => x.GrupoTributario.Cfop,
                        x => x.Produto.Ncm,
                        x => x.Produto.Cest,
                        x => x.Produto.UnidadeMedida,
                        x => x.Produto.EnquadramentoLegalIPI).AsNoTracking().Where(x => x.NotaFiscalId == entity.Id);

                    var destinoOperacao = TipoDestinoOperacao.Interna;
                    if (cliente.Estado != null && cliente.Estado.Sigla.ToUpper() == "EX" || (empresa.Cidade.Estado != null ? empresa.Cidade.Estado.Sigla.ToUpper() : "") == "EX")
                    {
                        destinoOperacao = TipoDestinoOperacao.Exterior;
                    }
                    else if (cliente.Estado != null && empresa.Cidade.Estado != null && (cliente.Estado.Sigla.ToUpper() != empresa.Cidade.Estado.Sigla.ToUpper()))
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

                    var CalendarTimeZoneDefault = "E. South America Standard Time";
                    DateTime now = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Unspecified);

                    TimeZoneInfo clientTimeZone = TimeZoneInfo.FindSystemTimeZoneById(CalendarTimeZoneDefault);
                    var data = TimeZoneInfo.ConvertTimeFromUtc(now, clientTimeZone);

                    #region Identificação
                    itemTransmissao.Identificador = new Identificador()
                    {
                        CodigoUF = empresa.Cidade != null ? (empresa.Cidade.Estado != null ? int.Parse(empresa.Cidade.Estado.CodigoIbge) : 0) : 0,
                        NaturezaOperacao = entity.NaturezaOperacao,
                        ModeloDocumentoFiscal = 55,
                        Serie = int.Parse(serieNotaFiscal.Serie),
                        NumeroDocumentoFiscal = entity.NumNotaFiscal.Value,
                        Emissao = TimeZoneHelper.GetDateTimeNow(isLocal),
                        EntradaSaida = TimeZoneHelper.GetDateTimeNow(isLocal),
                        TipoDocumentoFiscal = entity.TipoVenda == TipoVenda.Devolucao ? TipoNota.Entrada : TipoNota.Saida,
                        DestinoOperacao = destinoOperacao,
                        CodigoMunicipio = empresa.Cidade != null ? empresa.Cidade.CodigoIbge : null,
                        ImpressaoDANFE = TipoImpressaoDanfe.Retrato,
                        ChaveAcessoDV = 0,
                        CodigoNF = entity.NumNotaFiscal.Value.ToString(),
                        Ambiente = parametros.TipoAmbiente,
                        FinalidadeEmissaoNFe = entity.TipoVenda,
                        ConsumidorFinal = cliente.ConsumidorFinal ? 1 : 0,
                        PresencaComprador = parametros.TipoPresencaComprador,
                        Versao = "2.78",//versao do TSS
                        FormaEmissao = parametros.TipoModalidade,
                        CodigoProcessoEmissaoNFe = 0
                    };
                    if (entity.TipoVenda == TipoVenda.Devolucao || entity.TipoVenda == TipoVenda.Complementar)
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
                        CRT = CRT.SimplesNacional,
                        Endereco = new Endereco()
                        {
                            Bairro = empresa.Bairro,
                            Cep = empresa.CEP,
                            CodigoMunicipio = empresa.Cidade?.CodigoIbge,
                            Fone = empresa.Telefone,
                            Logradouro = empresa.Endereco,
                            Municipio = empresa.Cidade?.Nome,
                            Numero = empresa.Numero,
                            UF = empresa.Cidade?.Estado.Sigla
                        }
                    };
                    #endregion

                    #region Destinatário
                    itemTransmissao.Destinatario = new Destinatario()
                    {
                        Cnpj = cliente.TipoDocumento == "J" ? cliente.CPFCNPJ : null,
                        Cpf = cliente.TipoDocumento == "F" ? cliente.CPFCNPJ : null,
                        IndInscricaoEstadual = (IndInscricaoEstadual)System.Enum.Parse(typeof(IndInscricaoEstadual), cliente.TipoIndicacaoInscricaoEstadual.ToString()),
                        InscricaoEstadual = cliente.TipoIndicacaoInscricaoEstadual == TipoIndicacaoInscricaoEstadual.ContribuinteICMS ? cliente.InscricaoEstadual : null,
                        IdentificacaoEstrangeiro = null,
                        Nome = cliente.Nome,
                        Endereco = new Endereco()
                        {
                            Bairro = cliente.Bairro,
                            Cep = cliente.CEP,
                            CodigoMunicipio = cliente.Cidade?.CodigoIbge,
                            Fone = cliente.Telefone,
                            Logradouro = cliente.Endereco,
                            Municipio = cliente.Cidade?.Nome,
                            Numero = cliente.Numero,
                            UF = cliente.Estado?.Sigla
                        }
                    };
                    #endregion

                    #region Transporte
                    itemTransmissao.Transporte = new Transporte()
                    {
                        ModalidadeFrete = (ModalidadeFrete)Enum.Parse(typeof(ModalidadeFrete), entity.TipoFrete.ToString()),
                    };
                    if (transportadora != null)
                    {
                        itemTransmissao.Transporte.Transportadora = new Transportadora()
                        {
                            CNPJ = transportadora != null && transportadora.TipoDocumento == "J" ? transportadora.CPFCNPJ : null,
                            CPF = transportadora != null && transportadora.TipoDocumento == "F" ? transportadora.CPFCNPJ : null,
                            Endereco = transportadora?.Endereco,
                            IE = transportadora != null ? (transportadora.TipoIndicacaoInscricaoEstadual == TipoIndicacaoInscricaoEstadual.ContribuinteICMS ? transportadora.InscricaoEstadual : null) : null,
                            Municipio = transportadora != null && transportadora.Cidade != null ? transportadora.Cidade.Nome : null,
                            RazaoSocial = transportadora?.Nome,
                            UF = transportadora != null && transportadora.Estado != null ? transportadora.Estado.Sigla : null
                        };
                    }

                    if (entity.TipoFrete != TipoFrete.SemFrete)
                    {
                        itemTransmissao.Transporte.Volume = new Volume()
                        {
                            Especie = entity.TipoEspecie,
                            Quantidade = entity.QuantidadeVolumes ?? 0,
                            Marca = entity.Marca,
                            Numeracao = entity.NumeracaoVolumesTrans,
                            PesoLiquido = entity.PesoLiquido ?? 0,
                            PesoBruto = entity.PesoBruto ?? 0,
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
                            x.NcmId == (item.Produto.NcmId ?? Guid.NewGuid()) &
                            x.CestId == item.Produto.CestId.Value &
                            x.EstadoOrigem.Sigla == UFSiglaEmpresa &
                            x.EstadoDestinoId == cliente.EstadoId &
                            x.TipoSubstituicaoTributaria == TipoSubstituicaoTributaria.Saida
                            ).FirstOrDefault();
                        var CST = item.GrupoTributario.TipoTributacaoPIS.HasValue ? item.GrupoTributario.TipoTributacaoPIS.Value.ToString() : "";
                        var itemTributacao = new NotaFiscalItemTributacao();
                        itemTributacao = NotaFiscalItemTributacaoBL.All.Where(x => x.NotaFiscalItemId == item.Id).FirstOrDefault();

                        var produtoNFe = new EmissaoNFE.Domain.Entities.NFe.Produto()
                        {
                            CFOP = item.GrupoTributario.Cfop.Codigo,
                            Codigo = string.IsNullOrEmpty(item.Produto.CodigoProduto) ? string.Format("CFOP{0}", item.GrupoTributario.Cfop.Codigo.ToString()) : item.Produto.CodigoProduto,
                            Descricao = item.Produto.Descricao,
                            GTIN = string.IsNullOrEmpty(item.Produto.CodigoBarras) ? "SEM GETIN" : item.Produto.CodigoBarras,
                            GTIN_UnidadeMedidaTributada = string.IsNullOrEmpty(item.Produto.CodigoBarras) ? "SEM GETIN" : item.Produto.CodigoBarras,
                            NCM = item.Produto.Ncm?.Codigo,
                            TipoProduto = item.Produto.TipoProduto,
                            Quantidade = item.Quantidade,
                            QuantidadeTributada = item.Quantidade,
                            UnidadeMedida = item.Produto.UnidadeMedida?.Abreviacao,
                            UnidadeMedidaTributada = item.Produto.UnidadeMedida?.Abreviacao,
                            ValorBruto = item.Quantidade > 0 ? (item.Quantidade * item.Valor) : item.Valor,
                            ValorUnitario = item.Valor,
                            ValorUnitarioTributado = item.Valor,
                            ValorDesconto = item.Desconto,
                            AgregaTotalNota = CompoemValorTotal.Compoe,
                            CEST = item.Produto.Cest?.Codigo,
                            ValorFrete = (entity.TipoFrete == TipoFrete.CIF || entity.TipoFrete == TipoFrete.Remetente) ? Math.Round(itemTributacao.FreteValorFracionado, 2) : 0
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
                            CodigoSituacaoOperacao = item.GrupoTributario.TipoTributacaoICMS != null ? item.GrupoTributario.TipoTributacaoICMS.Value : TipoTributacaoICMS.TributadaSemPermissaoDeCredito,
                            AliquotaAplicavelCalculoCreditoSN = Math.Round(((item.ValorCreditoICMS / item.Total) * 100), 2),
                            ValorCreditoICMS = Math.Round(item.ValorCreditoICMS, 2),
                        };

                        if (itemTributacao.CalculaICMS)
                        {
                            detalhe.Imposto.ICMS.ValorICMSSTRetido = Math.Round(item.ValorICMSSTRetido, 2);
                            detalhe.Imposto.ICMS.ValorICMS = Math.Round(itemTributacao.ICMSValor, 2);
                            detalhe.Imposto.ICMS.ValorBC = Math.Round(itemTributacao.ICMSBase, 2);

                            if (item.GrupoTributario.TipoTributacaoICMS == TipoTributacaoICMS.Outros)
                            {
                                detalhe.Imposto.ICMS.ModalidadeBC = ModalidadeDeterminacaoBCICMS.ValorDaOperacao;
                                detalhe.Imposto.ICMS.AliquotaICMS = Math.Round(itemTributacao.ICMSAliquota, 2);
                                detalhe.Imposto.ICMS.ModalidadeBCST = ModalidadeDeterminacaoBCICMSST.MargemValorAgregado;
                            }
                        }
                        if (itemTributacao.CalculaST)
                        {
                            detalhe.Imposto.ICMS.UF = cliente.Estado?.Sigla;
                            detalhe.Imposto.ICMS.PercentualMargemValorAdicionadoST = st != null ? st.Mva : 0;
                            detalhe.Imposto.ICMS.ValorBCST = Math.Round(itemTributacao.STBase, 2);
                            detalhe.Imposto.ICMS.AliquotaICMSST = Math.Round(itemTributacao.STAliquota, 2);
                            detalhe.Imposto.ICMS.ValorICMSST = Math.Round(itemTributacao.STValor, 2);
                            detalhe.Imposto.ICMS.ValorBCSTRetido = Math.Round(item.ValorBCSTRetido, 2);

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
                                    CSTIPI.OutrasSaidas,
                                ValorIPI = itemTributacao.IPIValor,
                                CodigoEnquadramento = item.Produto.EnquadramentoLegalIPI?.Codigo,
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
                            //adValorem =  01|02, AliqEspecifica = 03
                            var tributaveis = "01|02|03";
                            if (tributaveis.Contains(((int)detalhe.Imposto.PIS.CodigoSituacaoTributaria).ToString()))
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
                            //adValorem =  01|02, AliqEspecifica = 03
                            var tributaveis = "01|02|03";
                            if (tributaveis.Contains(((int)detalhe.Imposto.COFINS.CodigoSituacaoTributaria).ToString()))
                            {
                                detalhe.Imposto.COFINS.ValorCOFINS = Math.Round(itemTributacao.COFINSValor, 2);
                                detalhe.Imposto.COFINS.ValorBC = Math.Round(itemTributacao.COFINSBase, 2);
                                detalhe.Imposto.COFINS.AliquotaPercentual = Math.Round(itemTributacao.COFINSAliquota, 2);
                            }
                        }

                        detalhe.Imposto.TotalAprox = (detalhe.Imposto.COFINS != null ? detalhe.Imposto.COFINS.ValorCOFINS : 0) +
                                         (detalhe.Imposto.ICMS.ValorICMS ?? 0) +
                                         (detalhe.Imposto.ICMS.ValorFCPST ?? 0) +
                                         (detalhe.Imposto.ICMS.ValorICMSST ?? 0) +
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
                            ValorFrete = (entity.TipoFrete == TipoFrete.CIF || entity.TipoFrete == TipoFrete.Remetente) ? itemTransmissao.Detalhes.Sum(x => x.Produto.ValorFrete.Value) : 0,
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
                    if (entity.TipoVenda == TipoVenda.Devolucao || entity.TipoVenda == TipoVenda.Complementar)
                    {
                        tipoFormaPagamento = TipoFormaPagamento.SemPagamento;
                    }

                    #region Pagamento
                    itemTransmissao.Pagamento = new Pagamento()
                    {
                        DetalhesPagamentos = new List<DetalhePagamento>()
                        {
                            new DetalhePagamento()
                            {
                                TipoFormaPagamento = tipoFormaPagamento,
                                ValorPagamento = tipoFormaPagamento == TipoFormaPagamento.SemPagamento ? 0.00 : itemTransmissao.Total.ICMSTotal.ValorTotalNF
                            }
                        }
                    };
                    #endregion

                    if (!string.IsNullOrEmpty(entity.MensagemPadraoNota))
                    {
                        itemTransmissao.InformacoesAdicionais = new InformacoesAdicionais()
                        {
                            InformacoesComplementares = entity.MensagemPadraoNota
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

        public TotalNotaFiscal CalculaTotalNFe(Guid nfeId)
        {
            var nfe = All.Where(x => x.Id == nfeId).AsNoTracking().FirstOrDefault();

            var produtos = NFeProdutoBL.All.AsNoTracking().Where(x => x.NotaFiscalId == nfeId).ToList();
            var totalProdutos = produtos != null ? produtos.Sum(x => ((x.Quantidade * x.Valor) - x.Desconto)) : 0.0;
            if (nfe.TipoVenda == TipoVenda.Complementar)
            {
                //TODO: Diego
                if (nfe.NaturezaOperacao == "Complemento de Preco")
                {
                    totalProdutos = produtos != null ? produtos.Sum(x => (x.Valor - x.Desconto)) : 0.0;
                }
            }
            var totalImpostosProdutos = nfe.TotalImpostosProdutos;
            var totalImpostosProdutosNaoAgrega = nfe.TotalImpostosProdutosNaoAgrega;
            bool calculaFrete = (
                ((nfe.TipoFrete == TipoFrete.CIF || nfe.TipoFrete == TipoFrete.Remetente) && nfe.TipoVenda == TipoVenda.Normal) ||
                ((nfe.TipoFrete == TipoFrete.FOB || nfe.TipoFrete == TipoFrete.Destinatario) && nfe.TipoVenda == TipoVenda.Devolucao)
            );

            var result = new TotalNotaFiscal()
            {
                TotalProdutos = Math.Round(totalProdutos, 2, MidpointRounding.AwayFromZero),
                ValorFrete = (calculaFrete && nfe.ValorFrete.HasValue) ? Math.Round(nfe.ValorFrete.Value, 2, MidpointRounding.AwayFromZero) : 0,
                TotalImpostosProdutos = Math.Round(totalImpostosProdutos, 2, MidpointRounding.AwayFromZero),
                TotalImpostosProdutosNaoAgrega = Math.Round(totalImpostosProdutosNaoAgrega, 2, MidpointRounding.AwayFromZero),
            };

            return result;
        }
    }
}