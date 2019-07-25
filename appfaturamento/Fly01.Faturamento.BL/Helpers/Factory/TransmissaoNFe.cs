using Fly01.Core;
using Fly01.Core.Entities.Domains.Commons;
using Fly01.Core.Entities.Domains.Enum;
using Fly01.Core.Helpers;
using Fly01.Core.Notifications;
using Fly01.Core.Rest;
using Fly01.EmissaoNFE.Domain.Entities.NFe;
using Fly01.EmissaoNFE.Domain.Entities.NFe.Totais;
using Fly01.EmissaoNFE.Domain.Enums;
using Fly01.EmissaoNFE.Domain.ViewModel;
using Fly01.Faturamento.BL.Helpers.Entities;
using Fly01.Faturamento.BL.Helpers.EntitiesBL;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using EmissaoNFe = Fly01.EmissaoNFE.Domain.Entities.NFe;

namespace Fly01.Faturamento.BL.Helpers.Factory
{
    public abstract class TransmissaoNFe
    {
        const string CSTsICMSST = "201||202||203||900||10||30||51||70||90";
        protected NFe NFe { get; set; }
        protected TransmissaoBLs TransmissaoBLs { get; set; }
        protected ParametroTributario ParametrosTributarios { get; set; }
        public TransmissaoCabecalho Cabecalho { get; set; }

        public TransmissaoNFe(NFe nfe, TransmissaoBLs transmissaoBLs)
        {
            this.NFe = nfe;
            this.TransmissaoBLs = transmissaoBLs;
            ValidaParametrosTributarios();
            this.Cabecalho = ObterTransmissaoCabecalho();
        }

        public abstract TransmissaoVM ObterTransmissaoVM();

        public abstract TipoNota ObterTipoDocumentoFiscal();

        public void ValidaParametrosTributarios()
        {
            this.ParametrosTributarios = TransmissaoBLs.TotalTributacaoBL.GetParametrosTributarios();
            if (this.ParametrosTributarios == null)
            {
                throw new BusinessException("Acesse o menu Configurações > Parâmetros Tributários e salve as configurações para a transmissão");
            }

            if (this.ParametrosTributarios.TipoVersaoNFe != TipoVersaoNFe.v4)
            {
                throw new BusinessException("Permitido somente NF-e versão 4.00. Acesse o menu Configurações > Parâmetros Tributários e altere as configurações");
            }
        }

        public string GetEmpresaUtcId()
        {
            var utcDefault = "E. South America Standard Time";
            return Cabecalho.Empresa.Cidade != null ? (Cabecalho.Empresa.Cidade.Estado != null ? Cabecalho.Empresa.Cidade.Estado.UtcId : utcDefault) : utcDefault;
        }

        public TransmissaoCabecalho ObterTransmissaoCabecalho()
        {
            return new TransmissaoCabecalho()
            {
                IsLocal = AppDefaults.UrlGateway.Contains("bemacashlocal.com.br"),
                Versao = EnumHelper.GetValue(typeof(TipoVersaoNFe), this.ParametrosTributarios.TipoVersaoNFe.ToString()),
                Cliente = TransmissaoBLs.TotalTributacaoBL.GetPessoa(NFe.ClienteId),
                Empresa = ApiEmpresaManager.GetEmpresa(TransmissaoBLs.PlataformaUrl),
                CondicaoParcelamento = TransmissaoBLs.CondicaoParcelamentoBL.All.AsNoTracking().Where(x => x.Id == NFe.CondicaoParcelamentoId).FirstOrDefault(),
                FormaPagamento = TransmissaoBLs.FormaPagamentoBL.All.AsNoTracking().Where(x => x.Id == NFe.FormaPagamentoId).FirstOrDefault(),
                Transportadora = TransmissaoBLs.PessoaBL.AllIncluding(x => x.Estado, x => x.Cidade).Where(x => x.Transportadora && x.Id == NFe.TransportadoraId).AsNoTracking().FirstOrDefault(),
                SerieNotaFiscal = TransmissaoBLs.SerieNotaFiscalBL.All.AsNoTracking().Where(x => x.Id == NFe.SerieNotaFiscalId).FirstOrDefault(),
                UFSaidaPais = TransmissaoBLs.EstadoBL.All.AsNoTracking().Where(x => x.Id == NFe.UFSaidaPaisId).FirstOrDefault(),
                NFeProdutos = ObterNFeProdutos()
            };
        }

        public IQueryable<NFeProduto> ObterNFeProdutos()
        {
            return TransmissaoBLs.NFeProdutoBL.AllIncluding(
                x => x.GrupoTributario.Cfop,
                x => x.Produto.Ncm,
                x => x.Produto.Cest,
                x => x.Produto.UnidadeMedida,
                x => x.Produto.EnquadramentoLegalIPI).AsNoTracking().Where(x => x.NotaFiscalId == NFe.Id);
        }

        public TipoDestinoOperacao DeterminarDestinoOperacao()
        {
            var destinoOperacao = TipoDestinoOperacao.Interna;
            if (Cabecalho.Cliente.Estado != null && Cabecalho.Cliente.Estado.Sigla.ToUpper() == "EX" || (Cabecalho.Empresa.Cidade.Estado != null ? Cabecalho.Empresa.Cidade.Estado.Sigla.ToUpper() : "") == "EX")
            {
                destinoOperacao = TipoDestinoOperacao.Exterior;
            }
            else if (Cabecalho.Cliente.Estado != null && Cabecalho.Empresa.Cidade.Estado != null && (Cabecalho.Cliente.Estado.Sigla.ToUpper() != Cabecalho.Empresa.Cidade.Estado.Sigla.ToUpper()))
            {
                destinoOperacao = TipoDestinoOperacao.Interestadual;
            }
            return destinoOperacao;
        }

        public FormaPagamentoEmissaoNFE ObterFormaPagamento()
        {
            var formaPagamento = FormaPagamentoEmissaoNFE.Outros;
            if (Cabecalho.CondicaoParcelamento != null)
            {
                formaPagamento = (Cabecalho.CondicaoParcelamento.QtdParcelas == 1 || Cabecalho.CondicaoParcelamento.CondicoesParcelamento == "0") ? FormaPagamentoEmissaoNFE.AVista : FormaPagamentoEmissaoNFE.APrazo;
            }
            return formaPagamento;
        }

        public Identificador ObterIdentificador()
        {
            var identificador = new Identificador()
            {
                CodigoUF = Cabecalho.Empresa.Cidade != null ? (Cabecalho.Empresa.Cidade.Estado != null ? int.Parse(Cabecalho.Empresa.Cidade.Estado.CodigoIbge) : 0) : 0,
                NaturezaOperacao = NFe.NaturezaOperacao,
                ModeloDocumentoFiscal = 55,
                Serie = int.Parse(Cabecalho.SerieNotaFiscal.Serie),
                NumeroDocumentoFiscal = NFe.NumNotaFiscal.Value,
                Emissao = TimeZoneHelper.GetDateTimeNow(Cabecalho.IsLocal, GetEmpresaUtcId()),
                EntradaSaida = TimeZoneHelper.GetDateTimeNow(Cabecalho.IsLocal, GetEmpresaUtcId()),
                TipoDocumentoFiscal = ObterTipoDocumentoFiscal(),
                DestinoOperacao = DeterminarDestinoOperacao(),
                CodigoMunicipio = Cabecalho.Empresa.Cidade?.CodigoIbge,
                ImpressaoDANFE = TipoImpressaoDanfe.Retrato,
                ChaveAcessoDV = 0,
                CodigoNF = NFe.NumNotaFiscal.Value.ToString(),
                Ambiente = ParametrosTributarios.TipoAmbiente,
                FinalidadeEmissaoNFe = NFe.TipoVenda,
                ConsumidorFinal = Cabecalho.Cliente.ConsumidorFinal ? 1 : 0,
                PresencaComprador = ParametrosTributarios.TipoPresencaComprador,
                Versao = "2.78",//versao do TSS
                FormaEmissao = ParametrosTributarios.TipoModalidade,
                CodigoProcessoEmissaoNFe = 0
            };
            if (NFe.TipoVenda == TipoCompraVenda.Devolucao || NFe.TipoVenda == TipoCompraVenda.Complementar)
            {
                identificador.NFReferenciada = new NFReferenciada()
                {
                    ChaveNFeReferenciada = NFe.ChaveNFeReferenciada
                };
            }
            return identificador;
        }

        public Emitente ObterEmitente()
        {
            return new Emitente()
            {
                Cnpj = Cabecalho.Empresa.CNPJ.Length == 14 ? Cabecalho.Empresa.CNPJ : null,
                Cpf = Cabecalho.Empresa.CNPJ.Length == 11 ? Cabecalho.Empresa.CNPJ : null,
                Nome = Cabecalho.Empresa.RazaoSocial,
                NomeFantasia = Cabecalho.Empresa.NomeFantasia,
                InscricaoEstadual = Cabecalho.Empresa.InscricaoEstadual,
                CRT = ParametrosTributarios.TipoCRT,
                Endereco = new Endereco()
                {
                    Bairro = Cabecalho.Empresa.Bairro,
                    Cep = Cabecalho.Empresa.CEP,
                    CodigoMunicipio = Cabecalho.Empresa.Cidade?.CodigoIbge,
                    Fone = Cabecalho.Empresa.Telefone,
                    Logradouro = Cabecalho.Empresa.Endereco,
                    Municipio = Cabecalho.Empresa.Cidade?.Nome,
                    Numero = Cabecalho.Empresa.Numero,
                    UF = Cabecalho.Empresa.Cidade?.Estado.Sigla
                }
            };
        }

        public bool EhExportacao()
        {
            return Cabecalho.Cliente?.Estado?.Sigla == "EX";
        }

        public Destinatario ObterDestinatario()
        {
            return new Destinatario()
            {
                Cnpj = (Cabecalho.Cliente.TipoDocumento == "J" && !EhExportacao()) ? Cabecalho.Cliente.CPFCNPJ : null,
                Cpf = (Cabecalho.Cliente.TipoDocumento == "F" && !EhExportacao()) ? Cabecalho.Cliente.CPFCNPJ : null,
                IndInscricaoEstadual = EhExportacao() ? TipoIndicacaoInscricaoEstadual.NaoContribuinte : Cabecalho.Cliente.TipoIndicacaoInscricaoEstadual,
                InscricaoEstadual = (Cabecalho.Cliente.TipoIndicacaoInscricaoEstadual == TipoIndicacaoInscricaoEstadual.ContribuinteICMS && !EhExportacao()) ? Cabecalho.Cliente.InscricaoEstadual : null,
                IdentificacaoEstrangeiro = Cabecalho.Cliente?.IdEstrangeiro,
                Nome = Cabecalho.Cliente.Nome,
                Endereco = new Endereco()
                {
                    Bairro = Cabecalho.Cliente?.Bairro,
                    Cep = Cabecalho.Cliente?.CEP,
                    CodigoMunicipio = Cabecalho.Cliente?.Cidade?.CodigoIbge,
                    Fone = Cabecalho.Cliente?.Telefone,
                    Logradouro = Cabecalho.Cliente?.Endereco,
                    Municipio = Cabecalho.Cliente.Cidade?.Nome,
                    Numero = Cabecalho.Cliente?.Numero,
                    UF = Cabecalho.Cliente?.Estado?.Sigla,
                    PaisCodigoBacen = Cabecalho.Cliente?.Pais?.CodigoBacen,
                    PaisNome = Cabecalho.Cliente?.Pais?.Nome,
                    Complemento = Cabecalho.Cliente?.Complemento
                }
            };
        }

        public Transportadora ObterTransportadora()
        {
            if (Cabecalho.Transportadora != null)
            {
                return new Transportadora()
                {
                    CNPJ = Cabecalho.Transportadora != null && Cabecalho.Transportadora?.TipoDocumento == "J" ? Cabecalho.Transportadora?.CPFCNPJ : null,
                    CPF = Cabecalho.Transportadora != null && Cabecalho.Transportadora?.TipoDocumento == "F" ? Cabecalho.Transportadora?.CPFCNPJ : null,
                    Endereco = Cabecalho.Transportadora?.Endereco,
                    IE = Cabecalho.Transportadora != null ? (Cabecalho.Transportadora?.TipoIndicacaoInscricaoEstadual == TipoIndicacaoInscricaoEstadual.ContribuinteICMS ? Cabecalho.Transportadora?.InscricaoEstadual : null) : null,
                    Municipio = Cabecalho.Transportadora != null && Cabecalho?.Transportadora?.Cidade != null ? Cabecalho.Transportadora?.Cidade?.Nome : null,
                    RazaoSocial = Cabecalho.Transportadora?.Nome,
                    UF = Cabecalho.Transportadora != null && Cabecalho?.Transportadora?.Estado != null ? Cabecalho?.Transportadora?.Estado?.Sigla : null
                };
            }
            else
            {
                return null;
            }
        }

        public Volume ObterVolume()
        {
            if (NFe.TipoFrete != TipoFrete.SemFrete)
            {
                return new Volume()
                {
                    Especie = NFe.TipoEspecie,
                    Quantidade = NFe.QuantidadeVolumes ?? 0,
                    Marca = NFe.Marca,
                    Numeracao = NFe.NumeracaoVolumesTrans,
                    PesoLiquido = NFe.PesoLiquido ?? 0,
                    PesoBruto = NFe.PesoBruto ?? 0,
                };
            }
            else
            {
                return null;
            }
        }

        public EmissaoNFe.Produto ObterProduto(NFeProduto item)
        {
            return new EmissaoNFe.Produto()
            {
                CFOP = item.GrupoTributario.Cfop?.Codigo,
                Codigo = string.IsNullOrEmpty(item.Produto.CodigoProduto) ? string.Format("CFOP{0}", item.GrupoTributario.Cfop?.Codigo.ToString()) : item.Produto.CodigoProduto,
                Descricao = item.Produto.Descricao,
                GTIN = string.IsNullOrEmpty(item.Produto.CodigoBarras) ? "SEM GETIN" : item.Produto.CodigoBarras,
                GTIN_UnidadeMedidaTributada = string.IsNullOrEmpty(item.Produto.CodigoBarras) ? "SEM GETIN" : item.Produto.CodigoBarras,
                NCM = item.Produto.Ncm?.Codigo,
                TipoProduto = item.Produto.TipoProduto,
                Quantidade = Math.Round(item.Quantidade, 4),
                QuantidadeTributada = Math.Round(item.Quantidade, 4),
                UnidadeMedida = item.Produto.UnidadeMedida?.Abreviacao,
                UnidadeMedidaTributada = item.Produto.UnidadeMedida?.Abreviacao,
                ValorBruto = Math.Round((item.Quantidade > 0 ? (Math.Round(item.Quantidade, 4) * Math.Round(item.Valor, 4)) : Math.Round(item.Valor, 4)), 2),
                ValorUnitario = Math.Round(item.Valor, 4),
                ValorUnitarioTributado = Math.Round(item.Valor, 4),
                ValorDesconto = Math.Round(item.Desconto, 2),
                AgregaTotalNota = CompoemValorTotal.Compoe,
                CEST = item.Produto.Cest?.Codigo,
                EXTIPI = item.Produto?.EXTIPI
            };
        }

        public Detalhe ObterDetalhe(NFeProduto nfeProduto, int num)
        {
            return new Detalhe()
            {
                NumeroItem = num,
                Produto = ObterProduto(nfeProduto),
                Imposto = new Imposto()
            };
        }

        public double ObterTotalAproximado(Detalhe detalhe)
        {
            return (detalhe.Imposto.COFINS != null ? detalhe.Imposto.COFINS.ValorCOFINS : 0) +
                   (detalhe.Imposto.ICMS.ValorICMS ?? 0) +
                   (detalhe.Imposto.ICMS.ValorFCPST ?? 0) +
                   (detalhe.Imposto.ICMS.ValorFCP ?? 0) +
                   (CSTsICMSST.Contains(((int)detalhe.Imposto.ICMS.CodigoSituacaoOperacao).ToString()) ? (detalhe.Imposto.ICMS.ValorICMSST ?? 0) : 0) +
                   (detalhe.Imposto.II != null ? detalhe.Imposto.II.ValorII : 0) +
                   (detalhe.Imposto.IPI != null ? detalhe.Imposto.IPI.ValorIPI : 0) +
                   (detalhe.Imposto.PIS != null ? detalhe.Imposto.PIS.ValorPIS : 0) +
                   (detalhe.Imposto.PISST != null ? detalhe.Imposto.PISST.ValorPISST : 0);
        }

        public Pagamento ObterPagamento(double valorPagamento)
        {
            return new Pagamento()
            {
                DetalhesPagamentos = new List<DetalhePagamento>()
                    {
                        new DetalhePagamento()
                        {
                            TipoFormaPagamento = ObterTipoFormaPagamento(),
                            ValorPagamento = ObterTipoFormaPagamento() == TipoFormaPagamento.SemPagamento ? 0.0 : valorPagamento
                        }
                    }
            };
        }

        public Total ObterTotal(List<Detalhe> detalhes)
        {             
            return new Total()
            {
                ICMSTotal = new ICMSTOT()
                {
                    SomatorioBC = detalhes.Select(x => x.Imposto.ICMS).Any(x => x != null && x.ValorBC.HasValue) ? Math.Round(detalhes.Where(x => x.Imposto.ICMS != null && x.Imposto.ICMS.ValorBC.HasValue).Sum(x => x.Imposto.ICMS.ValorBC.Value), 2) : 0,
                    SomatorioICMS = detalhes.Select(x => x.Imposto.ICMS).Any(x => x != null && x.ValorICMS.HasValue) ? Math.Round(detalhes.Where(x => x.Imposto.ICMS != null && x.Imposto.ICMS.ValorICMS.HasValue).Sum(x => x.Imposto.ICMS.ValorICMS.Value), 2) : 0,
                    SomatorioBCST = detalhes.Select(x => x.Imposto.ICMS).Any(x => x != null && x.ValorBCST.HasValue &&
                    (
                        x.CodigoSituacaoOperacao == TipoTributacaoICMS.TributadaSemPermissaoDeCreditoST ||
                        x.CodigoSituacaoOperacao == TipoTributacaoICMS.Outros ||
                        x.CodigoSituacaoOperacao == TipoTributacaoICMS.TributadaComPermissaoDeCreditoST ||
                        x.CodigoSituacaoOperacao == TipoTributacaoICMS.IsencaoParaFaixaDeReceitaBrutaST ||
                        x.CodigoSituacaoOperacao == TipoTributacaoICMS.ComRedDeBaseDeST ||
                        x.CodigoSituacaoOperacao == TipoTributacaoICMS.Outros90 ||
                        x.CodigoSituacaoOperacao == TipoTributacaoICMS.TributadaComCobrancaDeSubstituicao ||
                        x.CodigoSituacaoOperacao == TipoTributacaoICMS.IsentaOuNaoTributadaPorST
                    )) ? 
                    Math.Round(detalhes.Where(x => x.Imposto.ICMS != null && x.Imposto.ICMS.ValorBCST.HasValue && 
                    (
                        x.Imposto.ICMS.CodigoSituacaoOperacao == TipoTributacaoICMS.TributadaSemPermissaoDeCreditoST ||
                        x.Imposto.ICMS.CodigoSituacaoOperacao == TipoTributacaoICMS.Outros ||
                        x.Imposto.ICMS.CodigoSituacaoOperacao == TipoTributacaoICMS.TributadaComPermissaoDeCreditoST ||
                        x.Imposto.ICMS.CodigoSituacaoOperacao == TipoTributacaoICMS.IsencaoParaFaixaDeReceitaBrutaST ||
                        x.Imposto.ICMS.CodigoSituacaoOperacao == TipoTributacaoICMS.ComRedDeBaseDeST ||
                        x.Imposto.ICMS.CodigoSituacaoOperacao == TipoTributacaoICMS.Outros90 ||
                        x.Imposto.ICMS.CodigoSituacaoOperacao == TipoTributacaoICMS.TributadaComCobrancaDeSubstituicao ||
                        x.Imposto.ICMS.CodigoSituacaoOperacao == TipoTributacaoICMS.IsentaOuNaoTributadaPorST
                    )).Sum(x => x.Imposto.ICMS.ValorBCST.Value), 2) : 0,
                    SomatorioCofins = detalhes.Select(x => x.Imposto.COFINS).Any(x => x != null) ? Math.Round(detalhes.Sum(x => x.Imposto.COFINS.ValorCOFINS), 2) : 0,
                    SomatorioDesconto = detalhes.Select(x => x.Produto).Any(x => x != null) ? Math.Round(detalhes.Sum(x => x.Produto.ValorDesconto ?? 0), 2) : 0,
                    SomatorioICMSST = detalhes.Select(x => x.Imposto.ICMS).Any(x => x != null && x.ValorICMSST.HasValue && CSTsICMSST.Contains(((int)x.CodigoSituacaoOperacao).ToString()))
                        ? Math.Round(detalhes.Where(x => x.Imposto.ICMS != null && x.Imposto.ICMS.ValorICMSST.HasValue && CSTsICMSST.Contains(((int)x.Imposto.ICMS.CodigoSituacaoOperacao).ToString())).Sum(x => x.Imposto.ICMS.ValorICMSST.Value), 2) : 0,
                    ValorFrete = SomaFrete() ? detalhes.Sum(x => x.Produto.ValorFrete.Value) : 0,
                    ValorSeguro = 0,
                    SomatorioIPI = detalhes.Select(x => x.Imposto.IPI).Any(x => x != null) ? Math.Round(detalhes.Where(x => x.Imposto.IPI != null).Sum(x => x.Imposto.IPI.ValorIPI), 2) : 0,
                    SomatorioIPIDevolucao = detalhes.Select(x => x.Imposto.IPI).Any(x => x != null) ? Math.Round(detalhes.Where(x => x.Imposto.IPI != null).Sum(x => x.Imposto.IPI.ValorIPIDevolucao), 2) : 0,
                    SomatorioPis = detalhes.Sum(y => y.Imposto.PIS.ValorPIS),
                    SomatorioProdutos = detalhes.Sum(x => x.Produto.ValorBruto),
                    SomatorioOutro = 0,
                    SomatorioFCP = detalhes.Select(x => x.Imposto.ICMS).Any(x => x != null && x.ValorFCP.HasValue &&
                    (
                        x.CodigoSituacaoOperacao == TipoTributacaoICMS.ComReducaoDeBaseDeCalculo ||
                        x.CodigoSituacaoOperacao == TipoTributacaoICMS.Diferimento ||
                        x.CodigoSituacaoOperacao == TipoTributacaoICMS.Outros90 ||
                        x.CodigoSituacaoOperacao == TipoTributacaoICMS.TributadaIntegralmente
                    )) ?
                    Math.Round(detalhes.Where(x => x.Imposto.ICMS != null && x.Imposto.ICMS.ValorFCP.HasValue &&
                    (
                        x.Imposto.ICMS.CodigoSituacaoOperacao == TipoTributacaoICMS.ComReducaoDeBaseDeCalculo ||
                        x.Imposto.ICMS.CodigoSituacaoOperacao == TipoTributacaoICMS.Diferimento ||
                        x.Imposto.ICMS.CodigoSituacaoOperacao == TipoTributacaoICMS.Outros90 ||
                        x.Imposto.ICMS.CodigoSituacaoOperacao == TipoTributacaoICMS.TributadaIntegralmente
                    )).Sum(x => x.Imposto.ICMS.ValorFCP.Value), 2) : 0,
                    SomatorioFCPST = detalhes.Select(x => x.Imposto.ICMS).Any(x => x != null && x.ValorFCPST.HasValue) ? Math.Round(detalhes.Where(x => x.Imposto.ICMS != null && x.Imposto.ICMS.ValorFCPST.HasValue).Sum(x => x.Imposto.ICMS.ValorFCPST.Value), 2) : 0,
                    SomatorioFCPSTRetido = detalhes.Select(x => x.Imposto.ICMS).Any(x => x != null && x.ValorFCPSTRetido.HasValue) ? Math.Round(detalhes.Where(x => x.Imposto.ICMS != null && x.Imposto.ICMS.ValorFCPSTRetido.HasValue).Sum(x => x.Imposto.ICMS.ValorFCPSTRetido.Value), 2) : 0,
                }
            };
        }

        public abstract bool SomaFrete();

        public abstract TipoFormaPagamento ObterTipoFormaPagamento();

        private EntidadeVM ObterEntidade() => TransmissaoBLs.CertificadoDigitalBL.GetEntidade();

        public TransmissaoVM ObterTransmissaoVMApartirDoItem(ItemTransmissaoVM itemTransmissao)
        {
            var entidade = ObterEntidade();
            NFe.CertificadoDigitalId = TransmissaoBLs.CertificadoDigitalBL.CertificadoAtualValido()?.Id;
            NFe.TipoAmbiente = entidade.EntidadeAmbiente;
            return new TransmissaoVM()
            {
                Homologacao = entidade.Homologacao,
                Producao = entidade.Producao,
                EntidadeAmbiente = entidade.EntidadeAmbiente,
                EntidadeAmbienteNFS = entidade.EntidadeAmbienteNFS,
                Item = new List<ItemTransmissaoVM>()
                {
                    itemTransmissao
                }
            };
        }

        public ItemTransmissaoVM ObterCabecalhoItemTransmissao()
        {
            return new ItemTransmissaoVM()
            {
                Versao = Cabecalho.Versao,
                Identificador = ObterIdentificador(),
                Emitente = ObterEmitente(),
                Destinatario = ObterDestinatario(),
                Transporte = new Transporte()
                {
                    ModalidadeFrete = NFe.TipoFrete,
                }
            };
        }

        public NFReferenciada ObterNFReferenciada()
        {
            return new NFReferenciada()
            {
                ChaveNFeReferenciada = NFe.ChaveNFeReferenciada
            };
        }

        public Cobranca ObterCobranca()
        {
            if (NFe.GeraFinanceiro)
            {
                var contas = ObterContasFinanceiras();
                if (contas != null && contas.Any())
                {
                    var cobranca = new Cobranca()
                    {
                        Fatura = new Fatura()
                        {
                            NumeroFatura = "NF:"+NFe.NumNotaFiscal.Value.ToString(),
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
                                Numero = num.ToString().PadLeft(3,'0'),
                                ValorDuplicata = item.ValorPrevisto,
                                Vencimento = item.DataVencimento
                            });
                        num++;
                    }
                    return cobranca;
                }
            }
            return null;
        }

        public List<ContaFinanceira> ObterContasFinanceiras()
        {
            try
            {
                var header = new Dictionary<string, string>()
                {
                    { "AppUser", TransmissaoBLs.AppUser  },
                    { "PlataformaUrl", TransmissaoBLs.PlataformaUrl }
                };
                    var queryString = new Dictionary<string, string>()
                {
                    {
                        "contaFinanceiraParcelaPaiId",
                        NFe.ContaFinanceiraParcelaPaiIdProdutos.HasValue
                            ? NFe.ContaFinanceiraParcelaPaiIdProdutos.Value.ToString()
                            : default(Guid).ToString()
                    }
                };

                var contas = new List<ContaFinanceira>();
                if (ObterTipoDocumentoFiscal() == TipoNota.Saida)
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
                throw new BusinessException("Erro ao tentar obter as contas financeiras parcelas. " + ex.Message );
            }
        }

        public Exportacao ObterExportacao()
        {
            return new Exportacao()
            {
                LocalDespacho = NFe?.LocalDespacho,
                LocalEmbarque = NFe?.LocalEmbarque,
                UFSaidaPais = Cabecalho?.UFSaidaPais?.Sigla
            };
        }
    }
}