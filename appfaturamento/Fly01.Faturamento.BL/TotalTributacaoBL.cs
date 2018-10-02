using Fly01.Core;
using Fly01.Core.BL;
using Fly01.Core.Entities.Domains.Enum;
using Fly01.Core.Notifications;
using Fly01.Core.Rest;
using Fly01.EmissaoNFE.Domain;
using Fly01.EmissaoNFE.Domain.ViewModel;
using Fly01.Core.Entities.Domains.Commons;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Fly01.Core.ViewModels;

namespace Fly01.Faturamento.BL
{
    public class TotalTributacaoBL : PlataformaBaseBL<TotalTributacao>
    {
        public ManagerEmpresaVM empresa;
        public string empresaUF;
        protected PessoaBL PessoaBL { get; set; }
        protected GrupoTributarioBL GrupoTributarioBL { get; set; }
        protected ProdutoBL ProdutoBL { get; set; }
        protected ServicoBL ServicoBL { get; set; }
        protected SubstituicaoTributariaBL SubstituicaoTributariaBL { get; set; }
        protected ParametroTributarioBL ParametroTributarioBL { get; set; }
        protected CertificadoDigitalBL CertificadoDigitalBL { get; set; }
        protected OrdemVendaProdutoBL OrdemVendaProdutoBL { get; set; }
        protected OrdemVendaServicoBL OrdemVendaServicoBL { get; set; }

        public TotalTributacaoBL(AppDataContextBase context, PessoaBL pessoaBL, GrupoTributarioBL grupoTributarioBL, ProdutoBL produtoBL, ServicoBL servicoBL, SubstituicaoTributariaBL substituicaoTributariaBL, ParametroTributarioBL parametroTributarioBL, CertificadoDigitalBL certificadoDigitalBL, OrdemVendaProdutoBL ordemVendaProdutoBL, OrdemVendaServicoBL ordemVendaServicoBL) : base(context)
        {
            PessoaBL = pessoaBL;
            GrupoTributarioBL = grupoTributarioBL;
            ProdutoBL = produtoBL;
            ServicoBL = servicoBL;
            SubstituicaoTributariaBL = substituicaoTributariaBL;
            ParametroTributarioBL = parametroTributarioBL;
            CertificadoDigitalBL = certificadoDigitalBL;
            empresa = RestHelper.ExecuteGetRequest<ManagerEmpresaVM>($"{AppDefaults.UrlGateway}v2/", $"Empresa/{PlataformaUrl}");
            empresaUF = empresa.Cidade != null ? (empresa.Cidade.Estado != null ? empresa.Cidade.Estado.Sigla : string.Empty) : string.Empty;
            OrdemVendaProdutoBL = ordemVendaProdutoBL;
            OrdemVendaServicoBL = ordemVendaServicoBL;
        }

        public GrupoTributario GetGrupoTributario(Guid grupoTributarioId)
        {
            return GrupoTributarioBL.All.Where(x => x.Id == grupoTributarioId).AsNoTracking().FirstOrDefault();
        }

        public Pessoa GetPessoa(Guid pessoaId)
        {
            return PessoaBL.AllIncluding(y => y.Estado, y => y.Cidade).Where(x => x.Id == pessoaId).AsNoTracking().FirstOrDefault();
        }

        public Produto GetProduto(Guid produtoId)
        {
            return ProdutoBL.All.Where(x => x.Id == produtoId).AsNoTracking().FirstOrDefault();
        }

        public Servico GetServico(Guid servicoId)
        {
            return ServicoBL.All.Where(x => x.Id == servicoId).AsNoTracking().FirstOrDefault();
        }

        public ParametroTributario GetParametrosTributarios()
        {
            return ParametroTributarioBL.All.Where(x => x.Cnpj == empresa.CNPJ && x.InscricaoEstadual == empresa.InscricaoEstadual && x.UF == empresaUF).FirstOrDefault();
        }

        public List<OrdemVendaProduto> GetOrdemVendaProdutos(Guid ordemVendaId)
        {
            return OrdemVendaProdutoBL.All.Where(x => x.OrdemVendaId == ordemVendaId).ToList();
        }

        public List<OrdemVendaServico> GetOrdemVendaServicos(Guid ordemVendaId)
        {
            return OrdemVendaServicoBL.All.Where(x => x.OrdemVendaId == ordemVendaId).ToList();
        }

        public void DadosValidosCalculoTributario(OrdemVenda entity, Guid clienteId, bool onList = true)
        {
            var pessoa = GetPessoa(clienteId);
            var clienteUF = pessoa != null ? (pessoa.Estado != null ? pessoa.Estado.Sigla : "") : "";
            var ordemVendaProdutos = GetOrdemVendaProdutos(entity.Id);
            var ordemVendaServicos = GetOrdemVendaServicos(entity.Id);
            int num = 1;
            foreach (var item in ordemVendaProdutos)
            {
                if (GetProduto(item.ProdutoId) == null)
                {
                    throw new BusinessException(string.Format("Produto informado no item {0}, inexistente ou excluído.", num));
                }
                if (GetGrupoTributario(item.GrupoTributarioId ?? default(Guid)) == null)
                {
                    throw new BusinessException(string.Format("Informe um Grupo Tributário válido no produto {0}.", num));
                }
                num++;
            }
            num = 1;
            foreach (var item in ordemVendaServicos)
            {
                if (GetServico(item.ServicoId) == null)
                {
                    throw new BusinessException(string.Format("Serviço informado no item {0}, inexistente ou excluído.", num));
                }
                if (GetGrupoTributario(item.GrupoTributarioId ?? default(Guid)) == null)
                {
                    throw new BusinessException(string.Format("Informe um Grupo Tributário válido no serviço {0}.", num));
                }
                num++;
            }

            var dadosEmpresa = ApiEmpresaManager.GetEmpresa(PlataformaUrl);
            var empresaUF = dadosEmpresa.Cidade != null ? (dadosEmpresa.Cidade.Estado != null ? dadosEmpresa.Cidade.Estado.Sigla : "") : "";
            var parametros = GetParametrosTributarios();

            if (string.IsNullOrEmpty(empresaUF) || string.IsNullOrEmpty(clienteUF) || parametros == null)
            {
                if (string.IsNullOrEmpty(empresaUF))
                    entity.Notification.Errors.Add(new Error("Informe o estado no cadastro da empresa"));

                if (string.IsNullOrEmpty(clienteUF))
                    entity.Notification.Errors.Add(new Error("Informe o estado no cadastro do cliente"));

                if (parametros == null)
                    entity.Notification.Errors.Add(new Error("Vá em Configurações e defina os seus parâmetros tributários"));

                if (!onList)
                    entity.Notification.Errors.Add(new Error("Salve o orçamento/pedido para finalizá-lo após as alterações"));

                throw new BusinessException(entity.Notification.Get());
            }
        }

        public bool ConfiguracaoTSSOK(string plataformaId = null)
        {
            try
            {
                var retorno = CertificadoDigitalBL.GetEntidade(plataformaId) ?? CertificadoDigitalBL.GetEntidade();

                if (retorno != null)
                {
                    string entidade = retorno.EntidadeAmbiente == TipoAmbiente.Producao ? retorno.Producao : retorno.Homologacao;

                    var header = new Dictionary<string, string>()
                    {
                        { "AppUser", AppUser },
                        { "PlataformaUrl", entidade }
                    };
                    var resourceById = $"configuracaoOK?entidade={entidade}&tipoAmbiente={retorno.EntidadeAmbiente}";

                    var response = RestHelper.ExecuteGetRequest<JObject>(AppDefaults.UrlEmissaoNfeApi, resourceById, header, null);
                    return true;
                }
                else
                {
                    throw new BusinessException("Para transmitir, cadastre o seu Certificado Digital em Configurações");
                }
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        public bool ConfiguracaoTSSOKNFS(string plataformaId = null)
        {
            try
            {
                var empresa = ApiEmpresaManager.GetEmpresa(plataformaId);

                var retorno = CertificadoDigitalBL.GetEntidade(plataformaId) ?? CertificadoDigitalBL.GetEntidade();

                if (retorno != null)
                {
                    string entidade = retorno.EntidadeAmbiente == TipoAmbiente.Producao ? retorno.Producao : retorno.Homologacao;

                    var header = new Dictionary<string, string>()
                    {
                        { "AppUser", AppUser },
                        { "PlataformaUrl", entidade }
                    };
                    var resourceById = $"configuracaoOKNFS?entidade={entidade}&tipoAmbiente={retorno.EntidadeAmbiente}&codigoIBGEMunicipio={empresa.Cidade?.CodigoIbge ?? ""}";

                    var response = RestHelper.ExecuteGetRequest<JObject>(AppDefaults.UrlEmissaoNfeApi, resourceById, header, null);
                    return true;
                }
                else
                {
                    throw new BusinessException("Para transmitir, cadastre o seu Certificado Digital em Configurações");
                }
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        public double TributacaoItemAgregaNota(List<TributacaoItemRetorno> tributacaoItensRetorno)
        {
            var total = 0.0;
            total += tributacaoItensRetorno.Sum(x => x.IPIValor);
            total += tributacaoItensRetorno.Sum(x => x.STValor);
            total += tributacaoItensRetorno.Sum(x => x.FCPSTValor);
            return total;
        }

        public double TributacaoItemNaoAgregaNota(List<TributacaoItemRetorno> tributacaoItensRetorno)
        {
            var total = 0.0;
            total += tributacaoItensRetorno.Sum(x => x.ICMSValor);
            total += tributacaoItensRetorno.Sum(x => x.COFINSValor);
            total += tributacaoItensRetorno.Sum(x => x.FCPValor);
            total += tributacaoItensRetorno.Sum(x => x.PISValor);
            return total;
        }

        public double TributacaoServicoNaoAgregaNota(List<TributacaoServicoRetorno> tributacaoServicosRetorno)
        {
            var total = 0.0;
            total += tributacaoServicosRetorno.Sum(x => x.ISSValor);
            total += tributacaoServicosRetorno.Sum(x => x.PISValor);
            total += tributacaoServicosRetorno.Sum(x => x.COFINSValor);
            total += tributacaoServicosRetorno.Sum(x => x.CSLLValor);
            total += tributacaoServicosRetorno.Sum(x => x.INSSValor);
            total += tributacaoServicosRetorno.Sum(x => x.ImpostoRendaValor);

            return total;
        }

        public double TributacaoServicoRetencao(List<TributacaoServicoRetorno> tributacaoServicosRetorno)
        {
            var total = 0.0;
            total += tributacaoServicosRetorno.Sum(x => x.ISSValorRetencao);
            total += tributacaoServicosRetorno.Sum(x => x.PISValorRetencao);
            total += tributacaoServicosRetorno.Sum(x => x.COFINSValorRetencao);
            total += tributacaoServicosRetorno.Sum(x => x.CSLLValorRetencao);
            total += tributacaoServicosRetorno.Sum(x => x.INSSValorRetencao);
            total += tributacaoServicosRetorno.Sum(x => x.ImpostoRendaValorRetencao);

            return total;
        }

        public double TotalSomaTributacaoProduto(List<TributacaoProduto> tributacaoItens, Guid clienteId, TipoVenda tipoVenda, TipoNfeComplementar tipoNfeComplementar, TipoFrete tipoFrete, bool nFeRefIsDevolucao, double? valorFrete = 0)
        {
            return TributacaoItemAgregaNota(TotalTributacaoProduto(tributacaoItens, clienteId, tipoVenda, tipoNfeComplementar, tipoFrete, nFeRefIsDevolucao, valorFrete).ToList<TributacaoItemRetorno>());
        }

        public double TotalSomaTributacaoProdutoNaoAgrega(List<TributacaoProduto> tributacaoItens, Guid clienteId, TipoVenda tipoVenda, TipoNfeComplementar tipoNfeComplementar, TipoFrete tipoFrete, bool nFeRefIsDevolucao, double? valorFrete = 0)
        {
            return TributacaoItemNaoAgregaNota(TotalTributacaoProduto(tributacaoItens, clienteId, tipoVenda, tipoNfeComplementar, tipoFrete, nFeRefIsDevolucao, valorFrete).ToList<TributacaoItemRetorno>());
        }

        public double TotalSomaTributacaoServicoNaoAgrega(List<TributacaoServico> tributacaoItens, Guid clienteId)
        {
            return TributacaoServicoNaoAgregaNota(TotalTributacaoServico(tributacaoItens, clienteId).ToList<TributacaoServicoRetorno>());
        }

        public double TotalSomaRetencaoServico(List<TributacaoServico> tributacaoItens, Guid clienteId)
        {
            return TributacaoServicoRetencao(TotalTributacaoServico(tributacaoItens, clienteId).ToList<TributacaoServicoRetorno>());
        }

        public List<TributacaoServicoRetorno> TotalTributacaoServico(List<TributacaoServico> tributacaoItens, Guid clienteId)
        {
            var cliente = GetPessoa(clienteId);
            var empresa = ApiEmpresaManager.GetEmpresa(PlataformaUrl);
            var estadoOrigem = empresa.Cidade.Estado.Sigla;
            var parametros = GetParametrosTributarios();
            var result = new List<TributacaoServicoRetorno>();

            var header = new Dictionary<string, string>()
                {
                    { "AppUser", AppUser },
                    { "PlataformaUrl", PlataformaUrl }
                };


            var num = 1;
            foreach (var itemServico in tributacaoItens)
            {
                if (itemServico.GrupoTributarioId != default(Guid))
                {
                    var grupoTributario = GetGrupoTributario(itemServico.GrupoTributarioId);
                    if (grupoTributario == null)
                    {
                        throw new BusinessException(string.Format("Informe um Grupo Tributário válido no serviço {0}.", num));
                    }
                    var servico = GetServico(itemServico.ServicoId);
                    if (servico == null)
                    {
                        throw new BusinessException(string.Format("Serviço informado no item {0}, inexistente ou excluído.", num));
                    }

                    num++;

                    var itemRetorno = new TributacaoServicoRetorno()
                    {
                        ServicoId = itemServico.ServicoId,
                        GrupoTributarioId = itemServico.GrupoTributarioId

                    };
                    var tributacao = new Tributacao();
                    tributacao.ValorBase = itemServico.Total;
                    tributacao.SimplesNacional = true;
                    
                    //ISS
                    if (grupoTributario.CalculaIss || grupoTributario.RetemISS)
                    {
                        tributacao.Iss = new EmissaoNFE.Domain.Iss()
                        {
                            Aliquota = parametros != null ? parametros.AliquotaISS : 0,
                            CalculaIss = grupoTributario.CalculaIss,
                            RetemIss = grupoTributario.RetemISS
                        };
                    }
                    //COFINS
                    if (grupoTributario.CalculaCofins || grupoTributario.RetemCofins)
                    {
                        tributacao.Cofins = new Cofins()
                        {
                            Aliquota = parametros != null ? parametros.AliquotaCOFINS : 0,
                            CalculaCofins = grupoTributario.CalculaCofins,
                            RetemCofins = grupoTributario.RetemCofins
                        };
                    }
                    //PIS
                    if (grupoTributario.CalculaPis || grupoTributario.RetemPis)
                    {
                        tributacao.Pis = new Pis()
                        {
                            Aliquota = parametros != null ? parametros.AliquotaPISPASEP : 0,
                            CalculaPis = grupoTributario.CalculaPis,
                            RetemPis = grupoTributario.RetemPis
                        };
                    }
                    //CSLL
                    if (grupoTributario.CalculaCSLL || grupoTributario.RetemCSLL)
                    {
                        tributacao.Csll = new Csll()
                        {
                            //TODO: add as aliquotas
                            //Aliquota = parametros != null ? parametros.AliquotaCSLL : 0,
                            CalculaCsll = grupoTributario.CalculaCSLL,
                            RetemCsll = grupoTributario.RetemCSLL
                        };
                    }
                    //INSS
                    if (grupoTributario.CalculaINSS || grupoTributario.RetemINSS)
                    {
                        tributacao.Inss = new Inss()
                        {
                            //TODO: Aliquota = parametros != null ? parametros.AliquotaINSS : 0,
                            CalculaInss = grupoTributario.CalculaINSS,
                            RetemInss = grupoTributario.RetemINSS
                        };
                    }
                    //ImpostoRenda
                    if (grupoTributario.CalculaImpostoRenda || grupoTributario.RetemImpostoRenda)
                    {
                        tributacao.ImpostoRenda = new ImpostoRenda()
                        {
                            //TODO: Aliquota = parametros != null ? parametros.AliquotaImpostoRenda : 0,
                            CalculaImpostoRenda = grupoTributario.CalculaImpostoRenda,
                            RetemImpostoRenda = grupoTributario.RetemImpostoRenda
                        };
                    }

                    var json = JsonConvert.SerializeObject(tributacao);
                    var responseTributacao = RestHelper.ExecutePostRequest<TributacaoRetornoVM>(AppDefaults.UrlEmissaoNfeApi, "tributacao", json, null, header);

                    if (responseTributacao.Iss != null)
                    {
                        itemRetorno.ISSBase = responseTributacao.Iss.Base;
                        itemRetorno.ISSAliquota = responseTributacao.Iss.Aliquota;
                        itemRetorno.ISSValor = responseTributacao.Iss.Valor;
                        itemRetorno.ISSValorRetencao = responseTributacao.Iss.ValorRetencao;
                    }
                    if (responseTributacao.Pis != null)
                    {
                        itemRetorno.PISBase = responseTributacao.Pis.Base;
                        itemRetorno.PISAliquota = responseTributacao.Pis.Aliquota;
                        itemRetorno.PISValor = responseTributacao.Pis.Valor;
                        itemRetorno.PISValorRetencao = responseTributacao.Pis.ValorRetencao;
                    }
                    if (responseTributacao.Cofins != null)
                    {
                        itemRetorno.COFINSBase = responseTributacao.Cofins.Base;
                        itemRetorno.COFINSAliquota = responseTributacao.Cofins.Aliquota;
                        itemRetorno.COFINSValor = responseTributacao.Cofins.Valor;
                        itemRetorno.COFINSValorRetencao = responseTributacao.Cofins.ValorRetencao;
                    }
                    if (responseTributacao.Csll != null)
                    {
                        itemRetorno.CSLLBase = responseTributacao.Csll.Base;
                        itemRetorno.CSLLAliquota = responseTributacao.Csll.Aliquota;
                        itemRetorno.CSLLValor = responseTributacao.Csll.Valor;
                        itemRetorno.CSLLValorRetencao = responseTributacao.Csll.ValorRetencao;
                    }
                    if (responseTributacao.Inss != null)
                    {
                        itemRetorno.INSSBase = responseTributacao.Inss.Base;
                        itemRetorno.INSSAliquota = responseTributacao.Inss.Aliquota;
                        itemRetorno.INSSValor = responseTributacao.Inss.Valor;
                        itemRetorno.INSSValorRetencao = responseTributacao.Inss.ValorRetencao;
                    }
                    if (responseTributacao.ImpostoRenda != null)
                    {
                        itemRetorno.ImpostoRendaBase = responseTributacao.Inss.Base;
                        itemRetorno.ImpostoRendaAliquota = responseTributacao.Inss.Aliquota;
                        itemRetorno.ImpostoRendaValor = responseTributacao.Inss.Valor;
                        itemRetorno.ImpostoRendaValorRetencao = responseTributacao.Inss.ValorRetencao;
                    }

                    result.Add(itemRetorno);
                }
                else
                {
                    throw new BusinessException("Para calcular tributações de um pedido que gera nota fiscal, informe o grupo tributário nos serviços.");
                }
            }

            return result;
        }

        public List<TributacaoProdutoRetorno> TotalTributacaoProduto(List<TributacaoProduto> tributacaoItens, Guid clienteId, TipoVenda tipoVenda, TipoNfeComplementar tipoNfeComplementar, TipoFrete tipoFrete, bool nFeRefIsDevolucao, double? valorFrete = 0)
        {
            var result = new List<TributacaoProdutoRetorno>();
            if (tipoNfeComplementar == TipoNfeComplementar.ComplPrecoQtd)
            {
                return result;
            }

            var cliente = GetPessoa(clienteId);
            var empresa = ApiEmpresaManager.GetEmpresa(PlataformaUrl);
            string estadoOrigem = empresa.Cidade.Estado.Sigla;
            var parametros = GetParametrosTributarios();

            var header = new Dictionary<string, string>()
                {
                    { "AppUser", AppUser },
                    { "PlataformaUrl", PlataformaUrl }
                };

            bool calculaFrete = (
                ((tipoFrete == TipoFrete.CIF || tipoFrete == TipoFrete.Remetente) && tipoVenda == TipoVenda.Normal) ||
                ((tipoFrete == TipoFrete.FOB || tipoFrete == TipoFrete.Destinatario) && tipoVenda == TipoVenda.Devolucao)
            );

            double freteFracionado = calculaFrete && valorFrete.HasValue ? valorFrete.Value / tributacaoItens.Sum(x => x.Quantidade) : 0;

            var num = 1;
            foreach (var itemProduto in tributacaoItens)
            {
                var grupoTributario = GetGrupoTributario(itemProduto.GrupoTributarioId);
                if (grupoTributario == null)
                {
                    throw new BusinessException(string.Format("Informe um Grupo Tributário válido no produto {0}.", num));
                }
                var produto = ProdutoBL.All.AsNoTracking().Where(x => x.Id == itemProduto.ProdutoId).FirstOrDefault();
                if (produto == null)
                {
                    throw new BusinessException(string.Format("Produto informado no item {0}, inexistente ou excluído.", num));
                }

                num++;

                var itemRetorno = new TributacaoProdutoRetorno()
                {
                    FreteValorFracionado = (freteFracionado * itemProduto.Quantidade),
                    ProdutoId = itemProduto.ProdutoId,
                    GrupoTributarioId = itemProduto.GrupoTributarioId

                };
                var tributacao = new Tributacao();
                tributacao.ValorBase = itemProduto.Total;
                tributacao.ValorFrete = itemRetorno.FreteValorFracionado;
                tributacao.SimplesNacional = true;

                //ICMS
                //refatorar para cada caso
                if ((grupoTributario.CalculaIcms && tipoVenda != TipoVenda.Complementar)
                    || (tipoVenda == TipoVenda.Complementar && tipoNfeComplementar == TipoNfeComplementar.ComplIcms))
                {
                    tributacao.Icms = new Icms()
                    {
                        Aliquota = parametros.AliquotaSimplesNacional,
                        DespesaNaBase = grupoTributario.AplicaDespesaBaseIcms,
                        Difal = grupoTributario.CalculaIcmsDifal,
                        FreteNaBase = grupoTributario.AplicaFreteBaseIcms,
                        EstadoOrigem = (tipoVenda != TipoVenda.Devolucao ? estadoOrigem : cliente.Estado.Sigla),//inverte na devolução
                        EstadoDestino = (tipoVenda != TipoVenda.Devolucao ? cliente.Estado.Sigla : estadoOrigem),
                        CSOSN = grupoTributario.TipoTributacaoICMS != null ? grupoTributario.TipoTributacaoICMS.Value : TipoTributacaoICMS.Outros,
                    };
                    if (produto.AliquotaIpi > 0)
                    {
                        tributacao.Icms.IpiNaBase = grupoTributario.AplicaIpiBaseIcms;
                    }

                    tributacao.Fcp = new Fcp()
                    {
                        Aliquota = parametros.AliquotaFCP
                    };
                }
                //IPI
                if ((grupoTributario.CalculaIpi && produto.AliquotaIpi > 0 && tipoVenda != TipoVenda.Complementar)
                    || (tipoVenda == TipoVenda.Complementar && tipoNfeComplementar == TipoNfeComplementar.ComplIpi))
                {
                    tributacao.Ipi = new Ipi()
                    {
                        Aliquota = produto.AliquotaIpi,
                        DespesaNaBase = grupoTributario.AplicaDespesaBaseIpi,
                        FreteNaBase = grupoTributario.AplicaFreteBaseIpi
                    };
                }
                //ST
                if ((grupoTributario.CalculaSubstituicaoTributaria && tipoVenda != TipoVenda.Complementar)
                    || (tipoVenda == TipoVenda.Complementar && tipoNfeComplementar == TipoNfeComplementar.ComplIcmsST))
                {
                    //no Faturamento devolução é entrada ou se o complemento for de uma devolução
                    var isSaida = (tipoVenda == TipoVenda.Normal)
                        || (tipoVenda == TipoVenda.Complementar && !nFeRefIsDevolucao);

                    var st = SubstituicaoTributariaBL.AllIncluding(y => y.EstadoOrigem, y => y.EstadoDestino).AsNoTracking().Where(x =>
                        x.NcmId == (produto.NcmId.HasValue ? produto.NcmId.Value : Guid.NewGuid()) &
                        x.CestId == produto.CestId.Value &
                        x.EstadoOrigem.Sigla == (tipoVenda != TipoVenda.Devolucao ? estadoOrigem : cliente.Estado.Sigla) & //inverte na devolução
                        x.EstadoDestino.Sigla == (tipoVenda != TipoVenda.Devolucao ? cliente.Estado.Sigla : estadoOrigem) &
                        x.TipoSubstituicaoTributaria == (isSaida ? TipoSubstituicaoTributaria.Saida : TipoSubstituicaoTributaria.Entrada)
                        ).FirstOrDefault();

                    if (st != null)
                    {
                        tributacao.SubstituicaoTributaria = new EmissaoNFE.Domain.SubstituicaoTributaria()
                        {
                            EstadoDestino = (tipoVenda != TipoVenda.Devolucao ? cliente.Estado.Sigla : estadoOrigem),
                            EstadoOrigem = (tipoVenda != TipoVenda.Devolucao ? estadoOrigem : cliente.Estado.Sigla),
                            FreteNaBase = grupoTributario.AplicaFreteBaseST,
                            DespesaNaBase = grupoTributario.AplicaDespesaBaseST,
                            Mva = st.Mva,
                            AliquotaIntraEstadual = st.AliquotaIntraEstadual,
                            AliquotaInterEstadual = st.AliquotaInterEstadual,
                        };
                        if (produto.AliquotaIpi > 0)
                        {
                            tributacao.SubstituicaoTributaria.IpiNaBase = grupoTributario.AplicaIpiBaseST;
                        }

                        // FCP ST
                        tributacao.FcpSt = new FcpSt()
                        {
                            Aliquota = st.Fcp
                        };
                    }
                }
                //COFINS
                if ((grupoTributario.CalculaCofins || grupoTributario.RetemCofins) && tipoVenda != TipoVenda.Complementar)
                {
                    tributacao.Cofins = new Cofins()
                    {
                        Aliquota = parametros != null ? parametros.AliquotaCOFINS : 0,
                        FreteNaBase = grupoTributario.AplicaFreteBaseCofins,
                        CalculaCofins = grupoTributario.CalculaCofins,
                        RetemCofins = grupoTributario.RetemCofins
                    };
                }
                //PIS
                if ((grupoTributario.CalculaPis || grupoTributario.RetemPis) && tipoVenda != TipoVenda.Complementar)
                {
                    tributacao.Pis = new Pis()
                    {
                        Aliquota = parametros != null ? parametros.AliquotaPISPASEP : 0,
                        FreteNaBase = grupoTributario.AplicaFreteBasePis,
                        CalculaPis = grupoTributario.CalculaPis,
                        RetemPis = grupoTributario.RetemPis
                    };
                }

                var json = JsonConvert.SerializeObject(tributacao);
                var responseTributacao = RestHelper.ExecutePostRequest<TributacaoRetornoVM>(AppDefaults.UrlEmissaoNfeApi, "tributacao", json, null, header);
                if (responseTributacao.Fcp != null)
                {
                    itemRetorno.FCPBase = responseTributacao.Fcp.Base;
                    itemRetorno.FCPAliquota = responseTributacao.Fcp.Aliquota;
                    itemRetorno.FCPValor = responseTributacao.Fcp.Valor;
                    itemRetorno.FCPAgregaTotal = responseTributacao.Fcp.AgregaTotalNota;
                }
                if (responseTributacao.Icms != null)
                {
                    itemRetorno.ICMSBase = responseTributacao.Icms.Base;
                    itemRetorno.ICMSAliquota = responseTributacao.Icms.Aliquota;
                    itemRetorno.ICMSValor = responseTributacao.Icms.Valor;
                    itemRetorno.ICMSAgregaTotal = responseTributacao.Icms.AgregaTotalNota;
                }
                if (responseTributacao.Ipi != null)
                {
                    itemRetorno.IPIBase = responseTributacao.Ipi.Base;
                    itemRetorno.IPIAliquota = responseTributacao.Ipi.Aliquota;
                    itemRetorno.IPIValor = responseTributacao.Ipi.Valor;
                    itemRetorno.IPIAgregaTotal = responseTributacao.Ipi.AgregaTotalNota;
                }
                if (responseTributacao.SubstituicaoTributaria != null)
                {
                    itemRetorno.STBase = responseTributacao.SubstituicaoTributaria.Base;
                    itemRetorno.STAliquota = responseTributacao.SubstituicaoTributaria.Aliquota;
                    itemRetorno.STValor = responseTributacao.SubstituicaoTributaria.Valor;
                    itemRetorno.STAgregaTotal = responseTributacao.SubstituicaoTributaria.AgregaTotalNota;
                }
                if (responseTributacao.FcpSt != null)
                {
                    itemRetorno.FCPSTBase = responseTributacao.FcpSt.Base;
                    itemRetorno.FCPSTAliquota = responseTributacao.FcpSt.Aliquota;
                    itemRetorno.FCPSTValor = responseTributacao.FcpSt.Valor;
                    itemRetorno.FCPSTAgregaTotal = responseTributacao.FcpSt.AgregaTotalNota;
                }
                if (responseTributacao.Pis != null)
                {
                    itemRetorno.PISBase = responseTributacao.Pis.Base;
                    itemRetorno.PISAliquota = responseTributacao.Pis.Aliquota;
                    itemRetorno.PISValor = responseTributacao.Pis.Valor;
                    itemRetorno.PISValorRetencao = responseTributacao.Pis.ValorRetencao;
                }
                if (responseTributacao.Cofins != null)
                {
                    itemRetorno.COFINSAliquota = responseTributacao.Cofins.Base;
                    itemRetorno.COFINSAliquota = responseTributacao.Cofins.Aliquota;
                    itemRetorno.COFINSValor = responseTributacao.Cofins.Valor;
                    itemRetorno.COFINSValorRetencao = responseTributacao.Cofins.ValorRetencao;
                }

                result.Add(itemRetorno);
            }
            return result;
        }

        public List<TributacaoProdutoRetorno> TributacoesOrdemVendaProdutos(List<OrdemVendaProduto> ordemVendaProdutos, Guid clienteId, TipoVenda tipoVenda, TipoNfeComplementar tipoNfeComplementar, TipoFrete tipoFrete, bool nFeRefIsDevolucao, double? valorFrete = 0)
        {
            return TotalTributacaoProduto(ordemVendaProdutos.Select(x => new TributacaoProduto()
            {
                Valor = x.Valor,
                Quantidade = x.Quantidade,
                Desconto = x.Desconto,
                Total = x.Total,
                ProdutoId = x.ProdutoId,
                GrupoTributarioId = x.GrupoTributarioId.Value
            }).ToList(), clienteId, tipoVenda, tipoNfeComplementar, tipoFrete, nFeRefIsDevolucao, valorFrete);
        }

        public List<TributacaoServicoRetorno> TributacoesOrdemVendaServicos(List<OrdemVendaServico> ordemVendaServicos, Guid clienteId)
        {
            return TotalTributacaoServico(ordemVendaServicos.Select(x => new TributacaoServico()
            {
                Valor = x.Valor,
                Quantidade = x.Quantidade,
                Desconto = x.Desconto,
                Total = x.Total,
                ServicoId = x.ServicoId,
                GrupoTributarioId = x.GrupoTributarioId.Value
            }).ToList(), clienteId);
        }

        public double TotalSomaOrdemVendaProdutos(List<OrdemVendaProduto> ordemVendaProdutos, Guid clienteId, TipoVenda tipoVenda, TipoNfeComplementar tipoNfeComplementar, TipoFrete tipoFrete, bool nFeRefIsDevolucao, double? valorFrete = 0)
        {
            //Transforma para a classe auxiliar
            return TotalSomaTributacaoProduto(ordemVendaProdutos.Select(x => new TributacaoProduto()
            {
                Valor = x.Valor,
                Quantidade = x.Quantidade,
                Desconto = x.Desconto,
                Total = x.Total,
                ProdutoId = x.ProdutoId,
                GrupoTributarioId = x.GrupoTributarioId.Value
            }).ToList(), clienteId, tipoVenda, tipoNfeComplementar, tipoFrete, nFeRefIsDevolucao, valorFrete);
        }

        public double TotalSomaOrdemVendaProdutosNaoAgrega(List<OrdemVendaProduto> ordemVendaProdutos, Guid clienteId, TipoVenda tipoVenda, TipoNfeComplementar tipoNfeComplementar, TipoFrete tipoFrete, bool nFeRefIsDevolucao, double? valorFrete = 0)
        {
            //Transforma para a classe auxiliar
            return TotalSomaTributacaoProdutoNaoAgrega(ordemVendaProdutos.Select(x => new TributacaoProduto()
            {
                Valor = x.Valor,
                Quantidade = x.Quantidade,
                Desconto = x.Desconto,
                Total = x.Total,
                ProdutoId = x.ProdutoId,
                GrupoTributarioId = x.GrupoTributarioId.Value
            }).ToList(), clienteId, tipoVenda, tipoNfeComplementar, tipoFrete, nFeRefIsDevolucao, valorFrete);
        }

        public double TotalSomaOrdemVendaServicosNaoAgrega(List<OrdemVendaServico> ordemVendaServicos, Guid clienteId)
        {
            //Transforma para a classe auxiliar
            return TotalSomaTributacaoServicoNaoAgrega(ordemVendaServicos.Select(x => new TributacaoServico()
            {
                Valor = x.Valor,
                Quantidade = x.Quantidade,
                Desconto = x.Desconto,
                Total = x.Total,
                ServicoId = x.ServicoId,
                GrupoTributarioId = x.GrupoTributarioId.Value
            }).ToList(), clienteId);
        }

        public double TotalSomaRetencaoOrdemVendaServicos(List<OrdemVendaServico> ordemVendaServicos, Guid clienteId)
        {
            //Transforma para a classe auxiliar
            return TotalSomaRetencaoServico(ordemVendaServicos.Select(x => new TributacaoServico()
            {
                Valor = x.Valor,
                Quantidade = x.Quantidade,
                Desconto = x.Desconto,
                Total = x.Total,
                GrupoTributarioId = x.GrupoTributarioId.Value,
                GrupoTributario = x.GrupoTributario
            }).ToList(), clienteId);
        }
    }

    #region Classes auxiliares, calculo pode ser para ordemVenda ou notaFiscal
    public class TributacaoItem
    {
        public double Quantidade { get; set; }

        public double Valor { get; set; }

        public double Desconto { get; set; }

        public Guid GrupoTributarioId { get; set; }

        public double Total { get; set; }

        public virtual GrupoTributario GrupoTributario { get; set; }
    }

    public class TributacaoItemRetorno
    {
        public double FreteValorFracionado { get; set; }

        public double ICMSBase { get; set; }

        public double ICMSAliquota { get; set; }

        public double ICMSValor { get; set; }

        public bool ICMSAgregaTotal { get; set; }

        public double IPIBase { get; set; }

        public double IPIAliquota { get; set; }

        public double IPIValor { get; set; }

        public bool IPIAgregaTotal { get; set; }

        public double STBase { get; set; }

        public double STAliquota { get; set; }

        public double STValor { get; set; }

        public bool STAgregaTotal { get; set; }

        public double FCPBase { get; set; }

        public double FCPAliquota { get; set; }

        public double FCPValor { get; set; }

        public bool FCPAgregaTotal { get; set; }

        public double COFINSBase { get; set; }

        public double COFINSAliquota { get; set; }

        public double COFINSValor { get; set; }

        public double COFINSValorRetencao { get; set; }

        public double PISBase { get; set; }

        public double PISAliquota { get; set; }

        public double PISValor { get; set; }

        public double PISValorRetencao { get; set; }

        public double FCPSTBase { get; set; }

        public double FCPSTAliquota { get; set; }

        public double FCPSTValor { get; set; }

        public bool FCPSTAgregaTotal { get; set; }

        public Guid GrupoTributarioId { get; set; }
    }

    public class TributacaoProduto : TributacaoItem
    {
        public Guid ProdutoId { get; set; }
    }

    public class TributacaoServico : TributacaoItem
    {
        public Guid ServicoId { get; set; }
    }

    public class TributacaoProdutoRetorno : TributacaoItemRetorno
    {
        public Guid ProdutoId { get; set; }
    }

    public class TributacaoServicoRetorno : TributacaoItemRetorno
    {
        public Guid ServicoId { get; set; }

        public double ISSBase { get; set; }

        public double ISSAliquota { get; set; }

        public double ISSValor { get; set; }

        public double ISSValorRetencao { get; set; }

        public double CSLLBase { get; set; }

        public double CSLLAliquota { get; set; }

        public double CSLLValor { get; set; }

        public double CSLLValorRetencao { get; set; }

        public double INSSBase { get; set; }

        public double INSSAliquota { get; set; }

        public double INSSValor { get; set; }

        public double INSSValorRetencao { get; set; }

        public double ImpostoRendaBase { get; set; }

        public double ImpostoRendaAliquota { get; set; }

        public double ImpostoRendaValor { get; set; }

        public double ImpostoRendaValorRetencao { get; set; }
    }
    #endregion


}