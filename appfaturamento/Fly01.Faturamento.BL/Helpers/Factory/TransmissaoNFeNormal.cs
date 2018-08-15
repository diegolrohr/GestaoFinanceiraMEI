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

        public override TransmissaoVM ObterTransmissaoVM()
        {
            var isLocal = AppDefaults.UrlGateway.Contains("fly01local.com.br");

            var versao = EnumHelper.GetValue(typeof(TipoVersaoNFe), ParametrosTributarios.TipoVersaoNFe.ToString());
            var cliente = TransmissaoBLs.TotalTributacaoBL.GetPessoa(NFe.ClienteId);
            var empresa = ApiEmpresaManager.GetEmpresa(TransmissaoBLs.PlataformaUrl);
            var condicaoParcelamento = TransmissaoBLs.CondicaoParcelamentoBL.All.AsNoTracking().Where(x => x.Id == NFe.CondicaoParcelamentoId).FirstOrDefault();
            var formaPagamento = TransmissaoBLs.FormaPagamentoBL.All.AsNoTracking().Where(x => x.Id == NFe.FormaPagamentoId).FirstOrDefault();
            var transportadora = TransmissaoBLs.PessoaBL.AllIncluding(x => x.Estado, x => x.Cidade).Where(x => x.Transportadora && x.Id == NFe.TransportadoraId).AsNoTracking().FirstOrDefault();
            var serieNotaFiscal = TransmissaoBLs.SerieNotaFiscalBL.All.AsNoTracking().Where(x => x.Id == NFe.SerieNotaFiscalId).FirstOrDefault();
            var NFeProdutos = TransmissaoBLs.NFeProdutoBL.AllIncluding(
                x => x.GrupoTributario.Cfop,
                x => x.Produto.Ncm,
                x => x.Produto.Cest,
                x => x.Produto.UnidadeMedida,
                x => x.Produto.EnquadramentoLegalIPI).AsNoTracking().Where(x => x.NotaFiscalId == NFe.Id);
            bool pagaFrete = (
                ((NFe.TipoFrete == TipoFrete.CIF || NFe.TipoFrete == TipoFrete.Remetente) && NFe.TipoVenda == TipoVenda.Normal) ||
                ((NFe.TipoFrete == TipoFrete.FOB || NFe.TipoFrete == TipoFrete.Destinatario) && NFe.TipoVenda == TipoVenda.Devolucao)
            );

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

            //Devolução no faturamento é entrada
            var isSaida = (NFe.TipoVenda == TipoVenda.Normal) || (NFe.TipoVenda == TipoVenda.Complementar && !NFe.NFeRefComplementarIsDevolucao); //??|| NFe.TipoVenda == TipoVenda.Ajuste;
            #region Identificação
            itemTransmissao.Identificador = new Identificador()
            {
                CodigoUF = empresa.Cidade != null ? (empresa.Cidade.Estado != null ? int.Parse(empresa.Cidade.Estado.CodigoIbge) : 0) : 0,
                NaturezaOperacao = NFe.NaturezaOperacao,
                ModeloDocumentoFiscal = 55,
                Serie = int.Parse(serieNotaFiscal.Serie),
                NumeroDocumentoFiscal = NFe.NumNotaFiscal.Value,
                Emissao = TimeZoneHelper.GetDateTimeNow(isLocal),
                EntradaSaida = TimeZoneHelper.GetDateTimeNow(isLocal),
                TipoDocumentoFiscal = isSaida ? TipoNota.Saida : TipoNota.Entrada,
                DestinoOperacao = destinoOperacao,
                CodigoMunicipio = empresa.Cidade != null ? empresa.Cidade.CodigoIbge : null,
                ImpressaoDANFE = TipoImpressaoDanfe.Retrato,
                ChaveAcessoDV = 0,
                CodigoNF = NFe.NumNotaFiscal.Value.ToString(),
                Ambiente = ParametrosTributarios.TipoAmbiente,
                FinalidadeEmissaoNFe = NFe.TipoVenda,
                ConsumidorFinal = cliente.ConsumidorFinal ? 1 : 0,
                PresencaComprador = ParametrosTributarios.TipoPresencaComprador,
                Versao = "2.78",//versao do TSS
                FormaEmissao = ParametrosTributarios.TipoModalidade,
                CodigoProcessoEmissaoNFe = 0
            };
            if (NFe.TipoVenda == TipoVenda.Devolucao || NFe.TipoVenda == TipoVenda.Complementar)
            {
                itemTransmissao.Identificador.NFReferenciada = new NFReferenciada()
                {
                    ChaveNFeReferenciada = NFe.ChaveNFeReferenciada
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
                ModalidadeFrete = (ModalidadeFrete)Enum.Parse(typeof(ModalidadeFrete), NFe.TipoFrete.ToString()),
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

            if (NFe.TipoFrete != TipoFrete.SemFrete)
            {
                itemTransmissao.Transporte.Volume = new Volume()
                {
                    Especie = NFe.TipoEspecie,
                    Quantidade = NFe.QuantidadeVolumes ?? 0,
                    Marca = NFe.Marca,
                    Numeracao = NFe.NumeracaoVolumesTrans,
                    PesoLiquido = NFe.PesoLiquido ?? 0,
                    PesoBruto = NFe.PesoBruto ?? 0,
                };
            }
            #endregion

            #region Detalhes Produtos
            itemTransmissao.Detalhes = new List<Detalhe>();
            var num = 1;
            string UFSiglaEmpresa = (empresa.Cidade != null ? (empresa.Cidade.Estado != null ? empresa.Cidade.Estado.Sigla : "") : "");

            foreach (var item in NFeProdutos)
            {
                var st = TransmissaoBLs.SubstituicaoTributariaBL.AllIncluding(y => y.EstadoOrigem).AsNoTracking().Where(x =>
                    x.NcmId == (item.Produto.NcmId ?? Guid.NewGuid()) &
                    x.CestId == item.Produto.CestId.Value &
                    x.EstadoOrigem.Sigla == UFSiglaEmpresa &
                    x.EstadoDestinoId == cliente.EstadoId &
                    x.TipoSubstituicaoTributaria == (isSaida ? TipoSubstituicaoTributaria.Saida : TipoSubstituicaoTributaria.Entrada)
                    ).FirstOrDefault();
                var CST = item.GrupoTributario.TipoTributacaoPIS.HasValue ? item.GrupoTributario.TipoTributacaoPIS.Value.ToString() : "";
                var itemTributacao = new NotaFiscalItemTributacao();
                itemTributacao = TransmissaoBLs.NotaFiscalItemTributacaoBL.All.Where(x => x.NotaFiscalItemId == item.Id).FirstOrDefault();

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
                    ValorFrete = pagaFrete ? Math.Round(itemTributacao.FreteValorFracionado, 2) : 0
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
                    AliquotaAplicavelCalculoCreditoSN = Math.Round(((item.ValorCreditoICMS / item.Total) * 100), 2),
                    ValorCreditoICMS = Math.Round(item.ValorCreditoICMS, 2),
                };

                if (NFe.TipoVenda == TipoVenda.Complementar && NFe.TipoNfeComplementar == TipoNfeComplementar.ComplPrecoQtd)
                {
                    detalhe.Imposto.ICMS.CodigoSituacaoOperacao = TipoTributacaoICMS.NaoTributadaPeloSN;
                }
                else
                {
                    detalhe.Imposto.ICMS.CodigoSituacaoOperacao = item.GrupoTributario.TipoTributacaoICMS != null ? item.GrupoTributario.TipoTributacaoICMS.Value : TipoTributacaoICMS.TributadaSemPermissaoDeCredito;
                }


                //TODO diego passar o tipo do complemento
                //Mudar esses monstros de ifs, nos demais complementos, por enquanto solucionado o de preço para prod
                if (itemTributacao.CalculaICMS && NFe.TipoVenda != TipoVenda.Complementar)
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
                    if (item.GrupoTributario.TipoTributacaoICMS == TipoTributacaoICMS.TributadaComPermissaoDeCreditoST
                        || item.GrupoTributario.TipoTributacaoICMS == TipoTributacaoICMS.TributadaSemPermissaoDeCreditoST
                        || item.GrupoTributario.TipoTributacaoICMS == TipoTributacaoICMS.IsencaoParaFaixaDeReceitaBrutaST)
                    {
                        detalhe.Imposto.ICMS.ModalidadeBCST = ModalidadeDeterminacaoBCICMSST.MargemValorAgregado;
                        detalhe.Imposto.ICMS.PercentualReducaoBCST = 0;
                    }
                }
                if (itemTributacao.CalculaST && NFe.TipoVenda != TipoVenda.Complementar)
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

                if (itemTributacao.CalculaIPI && NFe.TipoVenda != TipoVenda.Complementar)
                {
                    detalhe.Imposto.IPI = new IPIPai()
                    {
                        CodigoST = item.GrupoTributario.TipoTributacaoIPI.HasValue ?
                            (CSTIPI)System.Enum.Parse(typeof(CSTIPI), item.GrupoTributario.TipoTributacaoIPI.Value.ToString()) :
                            (isSaida ? CSTIPI.OutrasSaidas : CSTIPI.OutrasEntradas),
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
                };
                if (NFe.TipoVenda == TipoVenda.Complementar && NFe.TipoNfeComplementar == TipoNfeComplementar.ComplPrecoQtd)
                {
                    detalhe.Imposto.PIS.CodigoSituacaoTributaria = CSTPISCOFINS.IsentaDaContribuicao;
                }
                else
                {
                    detalhe.Imposto.PIS.CodigoSituacaoTributaria = item.GrupoTributario.TipoTributacaoPIS.HasValue && itemTributacao.CalculaPIS ?
                        (CSTPISCOFINS)((int)item.GrupoTributario.TipoTributacaoPIS) :
                        CSTPISCOFINS.IsentaDaContribuicao;
                }


                if (itemTributacao.CalculaPIS && NFe.TipoVenda != TipoVenda.Complementar)
                {
                    //adValorem =  01|02, AliqEspecifica = 03
                    var tributaveis = "01|02|03";
                    if (tributaveis.Contains(((int)detalhe.Imposto.PIS.CodigoSituacaoTributaria).ToString()))
                    {
                        detalhe.Imposto.PIS.ValorPIS = Math.Round(itemTributacao.PISValor, 2);
                        detalhe.Imposto.PIS.PercentualPIS = ParametrosTributarios.AliquotaPISPASEP;
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
                };
                if (NFe.TipoVenda == TipoVenda.Complementar && NFe.TipoNfeComplementar == TipoNfeComplementar.ComplPrecoQtd)
                {
                    detalhe.Imposto.COFINS.CodigoSituacaoTributaria = CSTPISCOFINS.IsentaDaContribuicao;
                }
                else
                {
                    detalhe.Imposto.COFINS.CodigoSituacaoTributaria = item.GrupoTributario.TipoTributacaoCOFINS != null && itemTributacao.CalculaCOFINS ?
                        ((CSTPISCOFINS)(int)item.GrupoTributario.TipoTributacaoCOFINS.Value) :
                        CSTPISCOFINS.OutrasOperacoes;
                }

                if (itemTributacao.CalculaCOFINS && NFe.TipoVenda != TipoVenda.Complementar)
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
                    ValorFrete = pagaFrete ? itemTransmissao.Detalhes.Sum(x => x.Produto.ValorFrete.Value) : 0,
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
            if (NFe.TipoVenda == TipoVenda.Devolucao)
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
                                ValorPagamento = itemTransmissao.Total.ICMSTotal.ValorTotalNF
                            }
                        }
            };
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
    }
}
