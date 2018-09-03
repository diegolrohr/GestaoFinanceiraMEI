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
        public ManagerEmpresaVM empresa;
        public string empresaUF;
        protected PessoaBL PessoaBL { get; set; }
        protected GrupoTributarioBL GrupoTributarioBL { get; set; }
        protected ProdutoBL ProdutoBL { get; set; }
        protected SubstituicaoTributariaBL SubstituicaoTributariaBL { get; set; }
        protected ParametroTributarioBL ParametroTributarioBL { get; set; }
        protected CertificadoDigitalBL CertificadoDigitalBL { get; set; }
        protected PedidoItemBL PedidoItemBL { get; set; }

        public TotalTributacaoBL(AppDataContextBase context, PessoaBL pessoaBL, GrupoTributarioBL grupoTributarioBL, ProdutoBL produtoBL, SubstituicaoTributariaBL substituicaoTributariaBL, ParametroTributarioBL parametroTributarioBL, CertificadoDigitalBL certificadoDigitalBL, PedidoItemBL pedidoItemBL) : base(context)
        {
            PessoaBL = pessoaBL;
            GrupoTributarioBL = grupoTributarioBL;
            ProdutoBL = produtoBL;
            SubstituicaoTributariaBL = substituicaoTributariaBL;
            ParametroTributarioBL = parametroTributarioBL;
            CertificadoDigitalBL = certificadoDigitalBL;
            PedidoItemBL = pedidoItemBL;
            empresa = RestHelper.ExecuteGetRequest<ManagerEmpresaVM>($"{AppDefaults.UrlGateway}v2/", $"Empresa/{PlataformaUrl}");
            empresaUF = empresa.Cidade != null ? (empresa.Cidade.Estado != null ? empresa.Cidade.Estado.Sigla : string.Empty) : string.Empty;
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
            return ParametroTributarioBL.All.Where(x => x.Cnpj == empresa.CNPJ && x.InscricaoEstadual == empresa.InscricaoEstadual && x.UF == empresaUF).FirstOrDefault();
        }

        public Produto GetProduto(Guid produtoId)
        {
            return ProdutoBL.All.Where(x => x.Id == produtoId).AsNoTracking().FirstOrDefault();
        }

        public List<PedidoItem> GetPedidoItens(Guid pedidoId)
        {
            return PedidoItemBL.All.Where(x => x.PedidoId == pedidoId).ToList();
        }

        public void DadosValidosCalculoTributario(OrdemCompra entity, Guid fornecedorId, bool onList = true)
        {
            var pessoa = GetPessoa(fornecedorId);
            var fornecedorUF = pessoa != null ? (pessoa.Estado != null ? pessoa.Estado.Sigla : "") : "";
            var pedidoItens = GetPedidoItens(entity.Id);
            int num = 1;
            foreach (var item in pedidoItens)
            {
                if (GetProduto(item.ProdutoId) == null)
                {
                    throw new BusinessException("Produto informado no item, inexistente ou excluído.");
                }
                if (GetGrupoTributario(item.GrupoTributarioId ?? default(Guid)) == null)
                {
                    throw new BusinessException("Grupo Tributário informado no item, inexistente ou excluído.");
                }
                num++;
            }

            var dadosEmpresa = ApiEmpresaManager.GetEmpresa(PlataformaUrl);
            var empresaUF = dadosEmpresa.Cidade != null ? (dadosEmpresa.Cidade.Estado != null ? dadosEmpresa.Cidade.Estado.Sigla : "") : "";
            var parametros = GetParametrosTributarios();

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
                    string entidade = (int)retorno.EntidadeAmbiente == 1 ? retorno.Producao : retorno.Homologacao;

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

        public double TotalSomaTributacaoProduto(List<TributacaoProduto> tributacaoItens, Guid fornecedorId, TipoVenda tipoCompra, TipoFrete tipoFrete, double? valorFrete = 0)
        {
            return TributacaoItemAgregaNota(TotalTributacaoProduto(tributacaoItens, fornecedorId, tipoCompra, tipoFrete, valorFrete).ToList<TributacaoItemRetorno>());
        }

        public double TotalSomaTributacaoProdutoNaoAgrega(List<TributacaoProduto> tributacaoItens, Guid fornecedorId, TipoVenda tipoCompra, TipoFrete tipoFrete, double? valorFrete = 0)
        {
            return TributacaoItemNaoAgregaNota(TotalTributacaoProduto(tributacaoItens, fornecedorId, tipoCompra, tipoFrete, valorFrete).ToList<TributacaoItemRetorno>());
        }

        public List<TributacaoProdutoRetorno> TotalTributacaoProduto(List<TributacaoProduto> tributacaoItens, Guid fornecedorId, TipoVenda tipoCompra, TipoFrete tipoFrete, double? valorFrete = 0)
        {
            var fornecedor = GetPessoa(fornecedorId);
            var empresa = ApiEmpresaManager.GetEmpresa(PlataformaUrl);
            string estadoOrigem = empresa.Cidade.Estado.Sigla;
            var parametros = GetParametrosTributarios();
            var result = new List<TributacaoProdutoRetorno>();

            var header = new Dictionary<string, string>()
                {
                    { "AppUser", AppUser },
                    { "PlataformaUrl", PlataformaUrl }
                };

            bool calculaFrete = (
                ((tipoFrete == TipoFrete.CIF || tipoFrete == TipoFrete.Remetente) && tipoCompra == TipoVenda.Devolucao) ||
                ((tipoFrete == TipoFrete.FOB || tipoFrete == TipoFrete.Destinatario) && tipoCompra == TipoVenda.Normal)
            );

            double freteFracionado = calculaFrete && valorFrete.HasValue ? valorFrete.Value / tributacaoItens.Sum(x => x.Quantidade) : 0;

            foreach (var itemProduto in tributacaoItens)
            {
                if (itemProduto.GrupoTributarioId != default(Guid))
                {

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

                    var grupoTributario = GetGrupoTributario(itemProduto.GrupoTributarioId);
                    var produto = ProdutoBL.All.AsNoTracking().Where(x => x.Id == itemProduto.ProdutoId).FirstOrDefault();

                    //ICMS
                    if (grupoTributario.CalculaIcms)
                    {
                        tributacao.Icms = new Icms()
                        {
                            Aliquota = parametros.AliquotaSimplesNacional,
                            DespesaNaBase = grupoTributario.AplicaDespesaBaseIcms,
                            Difal = grupoTributario.CalculaIcmsDifal,
                            FreteNaBase = grupoTributario.AplicaFreteBaseIcms,
                            EstadoDestino = fornecedor.Estado.Sigla,
                            EstadoOrigem = estadoOrigem,
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
                        var st = SubstituicaoTributariaBL.AllIncluding(y => y.EstadoOrigem).AsNoTracking().Where(x =>
                            x.NcmId == (produto.NcmId.HasValue ? produto.NcmId.Value : Guid.NewGuid()) &
                            x.CestId == produto.CestId.Value &
                            x.EstadoOrigem.Sigla == estadoOrigem &
                            x.EstadoDestinoId == fornecedor.EstadoId &
                            x.TipoSubstituicaoTributaria == TipoSubstituicaoTributaria.Saida
                            ).FirstOrDefault();

                        if (st != null)
                        {
                            tributacao.SubstituicaoTributaria = new EmissaoNFE.Domain.SubstituicaoTributaria()
                            {
                                EstadoDestino = fornecedor.Estado.Sigla,
                                EstadoOrigem = estadoOrigem,
                                FreteNaBase = grupoTributario.AplicaFreteBaseST,
                                DespesaNaBase = grupoTributario.AplicaDespesaBaseST,
                                Mva = st.Mva,
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
                        itemRetorno.COFINSBase = itemProduto.Total + (grupoTributario.AplicaFreteBaseCofins ? itemRetorno.FreteValorFracionado : 0);
                        itemRetorno.COFINSAliquota = parametros != null ? parametros.AliquotaCOFINS : 0;
                        itemRetorno.COFINSValor = Math.Round(itemRetorno.COFINSBase / 100 * itemRetorno.COFINSAliquota, 2);
                    }
                    //PIS
                    if (grupoTributario.CalculaPis)
                    {
                        itemRetorno.PISBase = itemProduto.Total + (grupoTributario.AplicaFreteBasePis ? itemRetorno.FreteValorFracionado : 0);
                        itemRetorno.PISAliquota = parametros != null ? parametros.AliquotaPISPASEP : 0;
                        itemRetorno.PISValor = Math.Round(itemRetorno.PISBase / 100 * itemRetorno.PISAliquota, 2);
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
                    result.Add(itemRetorno);
                }
                else
                {
                    throw new BusinessException("Para calcular tributações de um pedido que gera nota fiscal, informe o grupo tributário nos produtos.");
                }
            }
            return result;
        }

        public List<TributacaoProdutoRetorno> TributacoesOrdemCompraItem(List<PedidoItem> ordemVendaProdutos, Guid fornecedorId, TipoVenda tipoCompra, TipoFrete tipoFrete, double? valorFrete = 0)
        {
            return TotalTributacaoProduto(ordemVendaProdutos.Select(x => new TributacaoProduto()
            {
                Valor = x.Valor,
                Quantidade = x.Quantidade,
                Desconto = x.Desconto,
                Total = x.Total,
                ProdutoId = x.ProdutoId,
                GrupoTributarioId = x.GrupoTributarioId.Value
            }).ToList(), fornecedorId, tipoCompra, tipoFrete, valorFrete);
        }

        public double TotalSomaOrdemCompraProdutos(List<PedidoItem> ordemVendaProdutos, Guid fornecedorId, TipoVenda tipoCompra, TipoFrete tipoFrete, double? valorFrete = 0)
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

        public double TotalSomaOrdemCompraProdutosNaoAgrega(List<PedidoItem> ordemVendaProdutos, Guid fornecedorId, TipoVenda tipoCompra, TipoFrete tipoFrete, double? valorFrete = 0)
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
        public Guid? ProdutoId { get; set; }
    }
    //se necessário mudar
    public class TributacaoServico : TributacaoItem
    {

    }

    public class TributacaoProdutoRetorno : TributacaoItemRetorno
    {
        public Guid? ProdutoId { get; set; }
    }
    //se necessário mudar
    public class TributacaoServicoRetorno : TributacaoItemRetorno
    {

    }
    #endregion


}