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

        public TransmissaoCabecalho ObterTransmissaoCabecalho()
        {
            return new TransmissaoCabecalho()
            {
                IsLocal = AppDefaults.UrlGateway.Contains("fly01local.com.br"),
                Versao = EnumHelper.GetValue(typeof(TipoVersaoNFe), this.ParametrosTributarios.TipoVersaoNFe.ToString()),
                Cliente = TransmissaoBLs.TotalTributacaoBL.GetPessoa(NFe.ClienteId),
                Empresa = ApiEmpresaManager.GetEmpresa(TransmissaoBLs.PlataformaUrl),
                CondicaoParcelamento = TransmissaoBLs.CondicaoParcelamentoBL.All.AsNoTracking().Where(x => x.Id == NFe.CondicaoParcelamentoId).FirstOrDefault(),
                FormaPagamento = TransmissaoBLs.FormaPagamentoBL.All.AsNoTracking().Where(x => x.Id == NFe.FormaPagamentoId).FirstOrDefault(),
                Transportadora = TransmissaoBLs.PessoaBL.AllIncluding(x => x.Estado, x => x.Cidade).Where(x => x.Transportadora && x.Id == NFe.TransportadoraId).AsNoTracking().FirstOrDefault(),
                SerieNotaFiscal = TransmissaoBLs.SerieNotaFiscalBL.All.AsNoTracking().Where(x => x.Id == NFe.SerieNotaFiscalId).FirstOrDefault(),
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
                Emissao = TimeZoneHelper.GetDateTimeNow(Cabecalho.IsLocal),
                EntradaSaida = TimeZoneHelper.GetDateTimeNow(Cabecalho.IsLocal),
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
            if (NFe.TipoVenda == TipoVenda.Devolucao || NFe.TipoVenda == TipoVenda.Complementar)
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
                Cnpj = Cabecalho.Empresa.CNPJ,
                Nome = Cabecalho.Empresa.RazaoSocial,
                NomeFantasia = Cabecalho.Empresa.NomeFantasia,
                InscricaoEstadual = Cabecalho.Empresa.InscricaoEstadual,
                CRT = CRT.SimplesNacional,
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

        public Destinatario ObterDestinatario()
        {
            return new Destinatario()
            {
                Cnpj = Cabecalho.Cliente.TipoDocumento == "J" ? Cabecalho.Cliente.CPFCNPJ : null,
                Cpf = Cabecalho.Cliente.TipoDocumento == "F" ? Cabecalho.Cliente.CPFCNPJ : null,
                IndInscricaoEstadual = (IndInscricaoEstadual)System.Enum.Parse(typeof(IndInscricaoEstadual), Cabecalho.Cliente.TipoIndicacaoInscricaoEstadual.ToString()),
                InscricaoEstadual = Cabecalho.Cliente.TipoIndicacaoInscricaoEstadual == TipoIndicacaoInscricaoEstadual.ContribuinteICMS ? Cabecalho.Cliente.InscricaoEstadual : null,
                IdentificacaoEstrangeiro = null,
                Nome = Cabecalho.Cliente.Nome,
                Endereco = new Endereco()
                {
                    Bairro = Cabecalho.Cliente.Bairro,
                    Cep = Cabecalho.Cliente.CEP,
                    CodigoMunicipio = Cabecalho.Cliente.Cidade?.CodigoIbge,
                    Fone = Cabecalho.Cliente.Telefone,
                    Logradouro = Cabecalho.Cliente.Endereco,
                    Municipio = Cabecalho.Cliente.Cidade?.Nome,
                    Numero = Cabecalho.Cliente.Numero,
                    UF = Cabecalho.Cliente.Estado?.Sigla
                }
            };
        }

        public Transportadora ObterTransportadora()
        {
            if (Cabecalho.Transportadora != null)
            {
                return new Transportadora()
                {
                    CNPJ = Cabecalho.Transportadora != null && Cabecalho.Transportadora.TipoDocumento == "J" ? Cabecalho.Transportadora.CPFCNPJ : null,
                    CPF = Cabecalho.Transportadora != null && Cabecalho.Transportadora.TipoDocumento == "F" ? Cabecalho.Transportadora.CPFCNPJ : null,
                    Endereco = Cabecalho.Transportadora?.Endereco,
                    IE = Cabecalho.Transportadora != null ? (Cabecalho.Transportadora.TipoIndicacaoInscricaoEstadual == TipoIndicacaoInscricaoEstadual.ContribuinteICMS ? Cabecalho.Transportadora.InscricaoEstadual : null) : null,
                    Municipio = Cabecalho.Transportadora != null && Cabecalho.Transportadora.Cidade != null ? Cabecalho.Transportadora.Cidade.Nome : null,
                    RazaoSocial = Cabecalho.Transportadora?.Nome,
                    UF = Cabecalho.Transportadora != null && Cabecalho.Transportadora.Estado != null ? Cabecalho.Transportadora.Estado.Sigla : null
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
                CEST = item.Produto.Cest?.Codigo
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
                   (detalhe.Imposto.ICMS.ValorICMSST ?? 0) +
                   (detalhe.Imposto.II != null ? detalhe.Imposto.II.ValorII : 0) +
                   (detalhe.Imposto.IPI != null ? detalhe.Imposto.IPI.ValorIPI : 0) +
                   (detalhe.Imposto.PIS != null ? detalhe.Imposto.PIS.ValorPIS : 0) +
                   (detalhe.Imposto.PISST != null ? detalhe.Imposto.PISST.ValorPISST : 0);
        }

        public Pagamento ObterPagamento(double valorPagamento)
        {
            var tipoFormaPagamento = TipoFormaPagamento.Outros;
            if (Cabecalho.FormaPagamento != null)
            {
                //Transferência não existe para o SEFAZ
                tipoFormaPagamento = Cabecalho.FormaPagamento.TipoFormaPagamento == TipoFormaPagamento.Transferencia ? TipoFormaPagamento.Outros : Cabecalho.FormaPagamento.TipoFormaPagamento;
            }
            return new Pagamento()
            {
                DetalhesPagamentos = new List<DetalhePagamento>()
                    {
                        new DetalhePagamento()
                        {
                            TipoFormaPagamento = tipoFormaPagamento,
                            ValorPagamento = valorPagamento
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
                    SomatorioBCST = detalhes.Select(x => x.Imposto.ICMS).Any(x => x != null && x.ValorBCST.HasValue) ? Math.Round(detalhes.Where(x => x.Imposto.ICMS != null && x.Imposto.ICMS.ValorBCST.HasValue).Sum(x => x.Imposto.ICMS.ValorBCST.Value), 2) : 0,
                    SomatorioCofins = detalhes.Select(x => x.Imposto.COFINS).Any(x => x != null) ? Math.Round(detalhes.Sum(x => x.Imposto.COFINS.ValorCOFINS), 2) : 0,
                    SomatorioDesconto = ObterNFeProdutos().Sum(x => x.Desconto),
                    SomatorioICMSST = detalhes.Select(x => x.Imposto.ICMS).Any(x => x != null && x.ValorICMSST.HasValue) ? Math.Round(detalhes.Where(x => x.Imposto.ICMS != null && x.Imposto.ICMS.ValorICMSST.HasValue).Sum(x => x.Imposto.ICMS.ValorICMSST.Value), 2) : 0,
                    ValorFrete = PagaFrete() ? detalhes.Sum(x => x.Produto.ValorFrete.Value) : 0,
                    ValorSeguro = 0,
                    SomatorioIPI = detalhes.Select(x => x.Imposto.IPI).Any(x => x != null) ? Math.Round(detalhes.Where(x => x.Imposto.IPI != null).Sum(x => x.Imposto.IPI.ValorIPI), 2) : 0,
                    SomatorioIPIDevolucao = detalhes.Select(x => x.Imposto.IPI).Any(x => x != null) ? Math.Round(detalhes.Where(x => x.Imposto.IPI != null).Sum(x => x.Imposto.IPI.ValorIPIDevolucao), 2) : 0,
                    SomatorioPis = detalhes.Sum(y => y.Imposto.PIS.ValorPIS),
                    SomatorioProdutos = detalhes.Sum(x => x.Produto.ValorBruto),
                    SomatorioOutro = 0,
                    SomatorioFCP = 0,
                    SomatorioFCPST = detalhes.Select(x => x.Imposto.ICMS).Any(x => x != null && x.ValorFCPST.HasValue) ? Math.Round(detalhes.Where(x => x.Imposto.ICMS != null && x.Imposto.ICMS.ValorFCPST.HasValue).Sum(x => x.Imposto.ICMS.ValorFCPST.Value), 2) : 0,
                    SomatorioFCPSTRetido = detalhes.Select(x => x.Imposto.ICMS).Any(x => x != null && x.ValorFCPSTRetido.HasValue) ? Math.Round(detalhes.Where(x => x.Imposto.ICMS != null && x.Imposto.ICMS.ValorFCPSTRetido.HasValue).Sum(x => x.Imposto.ICMS.ValorFCPSTRetido.Value), 2) : 0,
                }
            };
        }

        public abstract bool PagaFrete();

        private EntidadeVM ObterEntidade() => TransmissaoBLs.CertificadoDigitalBL.GetEntidade();

        public TransmissaoVM ObterTransmissaoVMApartirDoItem(ItemTransmissaoVM itemTransmissao)
        {
            var entidade = ObterEntidade();
            return new TransmissaoVM()
            {
                Homologacao = entidade.Homologacao,
                Producao = entidade.Producao,
                EntidadeAmbiente = entidade.EntidadeAmbiente,
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
                    ModalidadeFrete = (ModalidadeFrete)Enum.Parse(typeof(ModalidadeFrete), NFe.TipoFrete.ToString()),
                }
            };
        }
    }
}