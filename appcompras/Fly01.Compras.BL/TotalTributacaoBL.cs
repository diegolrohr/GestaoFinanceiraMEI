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

namespace Fly01.Compras.BL
{
    public class TotalTributacaoBL : PlataformaBaseBL<TotalTributacao>
    {
        protected ManagerEmpresaVM empresa;
        protected string empresaUF;
        protected PessoaBL PessoaBL { get; set; }
        protected GrupoTributarioBL GrupoTributarioBL { get; set; }
        protected ProdutoBL ProdutoBL { get; set; }
        protected SubstituicaoTributariaBL SubstituicaoTributariaBL { get; set; }
        protected ParametroTributarioBL ParametroTributarioBL { get; set; }
        protected CertificadoDigitalBL CertificadoDigitalBL { get; set; }
        protected PedidoItemBL PedidoItemBL { get; set; }

        protected void GetOrUpdateEmpresa()
        {
            if (empresa == null || (empresa != null && empresa?.PlatformUrl?.Fly01Url != PlataformaUrl))
            {
                empresa = ApiEmpresaManager.GetEmpresa(PlataformaUrl);
                empresaUF = empresa.Cidade != null ? (empresa.Cidade.Estado != null ? empresa.Cidade.Estado.Sigla : string.Empty) : string.Empty;
            }
        }

        public ManagerEmpresaVM GetEmpresa()
        {
            GetOrUpdateEmpresa();
            return empresa;
        }

        public TotalTributacaoBL(AppDataContextBase context, PessoaBL pessoaBL, GrupoTributarioBL grupoTributarioBL, ProdutoBL produtoBL, SubstituicaoTributariaBL substituicaoTributariaBL, ParametroTributarioBL parametroTributarioBL, CertificadoDigitalBL certificadoDigitalBL, PedidoItemBL pedidoItemBL) : base(context)
        {
            PessoaBL = pessoaBL;
            GrupoTributarioBL = grupoTributarioBL;
            ProdutoBL = produtoBL;
            SubstituicaoTributariaBL = substituicaoTributariaBL;
            ParametroTributarioBL = parametroTributarioBL;
            CertificadoDigitalBL = certificadoDigitalBL;
            PedidoItemBL = pedidoItemBL;
        }

        public GrupoTributario GetGrupoTributario(Guid grupoTributarioId)
        {
            return GrupoTributarioBL.All.Where(x => x.Id == grupoTributarioId).AsNoTracking().FirstOrDefault();
        }

        public Pessoa GetPessoa(Guid pessoaId)
        {
            return PessoaBL.AllIncluding(y => y.Estado, y => y.Cidade).Where(x => x.Id == pessoaId).AsNoTracking().FirstOrDefault();
        }

        public ParametroTributario GetParametrosTributarios()
        {
            return ParametroTributarioBL.ParametroAtualValido();
        }

        public Produto GetProduto(Guid produtoId)
        {
            return ProdutoBL.All.Where(x => x.Id == produtoId).AsNoTracking().FirstOrDefault();
        }

        public List<PedidoItem> GetPedidoItens(Guid pedidoId)
        {
            return PedidoItemBL.AllIncluding(x => x.GrupoTributario).Where(x => x.PedidoId == pedidoId).ToList();
        }

        public void DadosValidosCalculoTributario(Pedido entity, Guid fornecedorId, bool onList = true)
        {
            GetOrUpdateEmpresa();
            var pessoa = GetPessoa(fornecedorId);
            var fornecedorUF = pessoa != null ? (pessoa.Estado != null ? pessoa.Estado.Sigla : "") : "";
            var pedidoItens = GetPedidoItens(entity.Id);
            var parametros = GetParametrosTributarios();

            int num = 1;
            foreach (var item in pedidoItens)
            {
                if (GetProduto(item.ProdutoId) == null)
                {
                    throw new BusinessException(string.Format("Produto informado no item {0}, inexistente ou excluído.", num));
                }
                if (GetGrupoTributario(item.GrupoTributarioId ?? default(Guid)) == null)
                {
                    throw new BusinessException(string.Format("Informe um Grupo Tributário válido no produto {0}.", num));
                }
                if (parametros.TipoCRT != TipoCRT.RegimeNormal && entity.TipoCompra != TipoCompraVenda.Devolucao && ((int)item?.GrupoTributario.TipoTributacaoICMS >= 0 && (int)item?.GrupoTributario.TipoTributacaoICMS <= 90))
                {
                    throw new BusinessException(string.Format("Seu regime é Simples Nacional e no grupo tributário do produto {0}, foi configurado CST, altere para CSOSN.", num));
                }
                if (parametros.TipoCRT == TipoCRT.RegimeNormal && entity.TipoCompra != TipoCompraVenda.Devolucao && ((int)item?.GrupoTributario.TipoTributacaoICMS >= 101 && (int)item?.GrupoTributario.TipoTributacaoICMS <= 900))
                {
                    throw new BusinessException(string.Format("Seu regime é Normal e no grupo tributário do produto {0}, foi configurado CSOSN, altere para CST.", num));
                }
                num++;
            }

            if (string.IsNullOrEmpty(empresaUF) || string.IsNullOrEmpty(fornecedorUF) || parametros == null)
            {
                if (string.IsNullOrEmpty(empresaUF))
                    entity.Notification.Errors.Add(new Error("Informe o estado no cadastro da empresa."));

                if (string.IsNullOrEmpty(fornecedorUF))
                    entity.Notification.Errors.Add(new Error("Informe o estado no cadastro do fornecedor."));

                if (parametros == null)
                    entity.Notification.Errors.Add(new Error("Vá em Configurações e defina os seus parâmetros tributários."));

                if (!onList)
                    entity.Notification.Errors.Add(new Error("Salve o orçamento/pedido para finalizá-lo após as alterações."));

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

        public double TotalSomaTributacaoProduto(List<TributacaoProduto> tributacaoItens, Guid fornecedorId, TipoCompraVenda tipoCompra, TipoFrete tipoFrete, double? valorFrete = 0)
        {
            return TributacaoItemAgregaNota(TotalTributacaoProduto(tributacaoItens, fornecedorId, tipoCompra, tipoFrete, valorFrete).ToList<TributacaoItemRetorno>());
        }

        public double TotalSomaTributacaoProdutoNaoAgrega(List<TributacaoProduto> tributacaoItens, Guid fornecedorId, TipoCompraVenda tipoCompra, TipoFrete tipoFrete, double? valorFrete = 0)
        {
            return TributacaoItemNaoAgregaNota(TotalTributacaoProduto(tributacaoItens, fornecedorId, tipoCompra, tipoFrete, valorFrete).ToList<TributacaoItemRetorno>());
        }

        public List<TributacaoProdutoRetorno> TotalTributacaoProduto(List<TributacaoProduto> tributacaoItens, Guid fornecedorId, TipoCompraVenda tipoCompra, TipoFrete tipoFrete, double? valorFrete = 0)
        {
            var fornecedor = GetPessoa(fornecedorId);
            GetOrUpdateEmpresa();
            var parametros = GetParametrosTributarios();
            var result = new List<TributacaoProdutoRetorno>();

            var header = new Dictionary<string, string>()
                {
                    { "AppUser", AppUser },
                    { "PlataformaUrl", PlataformaUrl }
                };

            double freteFracionado = valorFrete.HasValue ? valorFrete.Value / tributacaoItens.Sum(x => x.Quantidade) : 0;

            var num = 1;
            foreach (var itemProduto in tributacaoItens)
            {
                if (itemProduto.GrupoTributarioId != default(Guid))
                {
                    var grupoTributario = GetGrupoTributario(itemProduto.GrupoTributarioId);
                    if (grupoTributario == null)
                    {
                        throw new BusinessException(string.Format("Informe um Grupo Tributário válido no produto {0}.", num));
                    }
                    var produto = GetProduto(itemProduto.ProdutoId);
                    if (produto == null)
                    {
                        throw new BusinessException(string.Format("Produto informado no item {0}, inexistente ou excluído.", num));
                    }

                    num++;

                    var itemRetorno = new TributacaoProdutoRetorno()
                    {
                        FreteValorFracionado = (freteFracionado * itemProduto.Quantidade),
                        ProdutoId = itemProduto.ProdutoId,
                        GrupoTributarioId = itemProduto.GrupoTributarioId,
                        PedidoItemId = itemProduto.PedidoItemId
                    };
                    var tributacao = new Tributacao();
                    tributacao.ValorBase = itemProduto.Total;
                    tributacao.ValorFrete = itemRetorno.FreteValorFracionado;

                    tributacao.SimplesNacional = parametros.TipoCRT != TipoCRT.RegimeNormal;

                    //ICMS
                    if (grupoTributario.CalculaIcms)
                    {
                        tributacao.Icms = new Icms()
                        {
                            Aliquota = parametros.AliquotaSimplesNacional,
                            DespesaNaBase = grupoTributario.AplicaDespesaBaseIcms,
                            Difal = grupoTributario.CalculaIcmsDifal,
                            FreteNaBase = grupoTributario.AplicaFreteBaseIcms,
                            EstadoOrigem = (tipoCompra != TipoCompraVenda.Devolucao ? fornecedor.Estado.Sigla : empresaUF),//na devolução inverte
                            EstadoDestino = (tipoCompra != TipoCompraVenda.Devolucao ? empresaUF : fornecedor.Estado.Sigla),
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
                    if (grupoTributario.CalculaIpi && produto.AliquotaIpi > 0)
                    {
                        tributacao.Ipi = new Ipi()
                        {
                            Aliquota = produto.AliquotaIpi,
                            DespesaNaBase = grupoTributario.AplicaDespesaBaseIpi,
                            FreteNaBase = grupoTributario.AplicaFreteBaseIpi
                        };
                    }
                    //ST
                    if (grupoTributario.CalculaSubstituicaoTributaria)
                    {
                        var isEntrada = (tipoCompra == TipoCompraVenda.Normal)
                            || (tipoCompra == TipoCompraVenda.Complementar);

                        var st = SubstituicaoTributariaBL.AllIncluding(y => y.EstadoOrigem, y => y.EstadoDestino).AsNoTracking().Where(x =>
                            x.NcmId == (produto.NcmId.HasValue ? produto.NcmId.Value : Guid.NewGuid()) &&
                            ((produto.CestId.HasValue && x.CestId == produto.CestId.Value) || !produto.CestId.HasValue) &&
                            x.EstadoOrigem.Sigla == (tipoCompra != TipoCompraVenda.Devolucao ? fornecedor.Estado.Sigla : empresaUF) &&
                            x.EstadoDestino.Sigla == (tipoCompra != TipoCompraVenda.Devolucao ? empresaUF : fornecedor.Estado.Sigla) &&
                            x.TipoSubstituicaoTributaria == (isEntrada ? TipoSubstituicaoTributaria.Entrada : TipoSubstituicaoTributaria.Saida)
                            ).FirstOrDefault();

                        if (st != null)
                        {
                            tributacao.SubstituicaoTributaria = new EmissaoNFE.Domain.SubstituicaoTributaria()
                            {
                                EstadoOrigem = fornecedor.Estado.Sigla,
                                EstadoDestino = empresaUF,
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
                    if (grupoTributario.CalculaCofins)
                    {
                        tributacao.Cofins = new Cofins()
                        {
                            Aliquota = parametros != null ? parametros.AliquotaCOFINS : 0,
                            FreteNaBase = grupoTributario.AplicaFreteBaseCofins
                        };
                    }
                    //PIS
                    if (grupoTributario.CalculaPis)
                    {
                        tributacao.Pis = new Pis()
                        {
                            Aliquota = parametros != null ? parametros.AliquotaPISPASEP : 0,
                            FreteNaBase = grupoTributario.AplicaFreteBasePis
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
                    }
                    if (responseTributacao.Cofins != null)
                    {
                        itemRetorno.COFINSAliquota = responseTributacao.Cofins.Base;
                        itemRetorno.COFINSAliquota = responseTributacao.Cofins.Aliquota;
                        itemRetorno.COFINSValor = responseTributacao.Cofins.Valor;
                    }

                    result.Add(itemRetorno);
                }
                else
                {
                    throw new BusinessException("Para calcular tributações de um pedido que gera nota fiscal, informe o grupo tributário nos produtos.");
                }
            }
            return result;
        }

        public List<TributacaoProdutoRetorno> TributacoesOrdemCompraItem(List<PedidoItem> ordemVendaProdutos, Guid fornecedorId, TipoCompraVenda tipoCompra, TipoFrete tipoFrete, double? valorFrete = 0)
        {
            return TotalTributacaoProduto(ordemVendaProdutos.Select(x => new TributacaoProduto()
            {
                Valor = x.Valor,
                Quantidade = x.Quantidade,
                Desconto = x.Desconto,
                Total = x.Total,
                ProdutoId = x.ProdutoId,
                GrupoTributarioId = x.GrupoTributarioId.Value,
                PedidoItemId = x.Id

            }).ToList(), fornecedorId, tipoCompra, tipoFrete, valorFrete);
        }

        public double TotalSomaOrdemCompraProdutos(List<PedidoItem> ordemVendaProdutos, Guid fornecedorId, TipoCompraVenda tipoCompra, TipoFrete tipoFrete, double? valorFrete = 0)
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
            }).ToList(), fornecedorId, tipoCompra, tipoFrete, valorFrete);
        }

        public double TotalSomaOrdemCompraProdutosNaoAgrega(List<PedidoItem> ordemVendaProdutos, Guid fornecedorId, TipoCompraVenda tipoCompra, TipoFrete tipoFrete, double? valorFrete = 0)
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
            }).ToList(), fornecedorId, tipoCompra, tipoFrete, valorFrete);
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

        public double Fcp { get; set; }

        public double Icms { get; set; }

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

        public double PISBase { get; set; }

        public double PISAliquota { get; set; }

        public double PISValor { get; set; }

        public double FCPSTBase { get; set; }

        public double FCPSTAliquota { get; set; }

        public double FCPSTValor { get; set; }

        public bool FCPSTAgregaTotal { get; set; }

        public Guid GrupoTributarioId { get; set; }
    }

    public class TributacaoProduto : TributacaoItem
    {
        public Guid ProdutoId { get; set; }
        public Guid PedidoItemId { get; set; }
    }
    //se necessário mudar
    public class TributacaoServico : TributacaoItem
    {

    }

    public class TributacaoProdutoRetorno : TributacaoItemRetorno
    {
        public Guid ProdutoId { get; set; }
        public Guid PedidoItemId { get; set; }
    }
    //se necessário mudar
    public class TributacaoServicoRetorno : TributacaoItemRetorno
    {

    }
    #endregion


}