using Fly01.EmissaoNFE.Domain;
using Fly01.EmissaoNFE.Domain.ViewModel;
using Fly01.Faturamento.Domain.Entities;
using Fly01.Faturamento.Domain.Enums;
using Fly01.Core.Domain;
using Fly01.Core.BL;
using Fly01.Core.Notifications;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using EmpresaVM = Fly01.Core.VM.EmpresaVM;
using Fly01.Core.Rest;
using Fly01.Core;

namespace Fly01.Faturamento.BL
{
    public class TotalTributacaoBL : PlataformaBaseBL<TotalTributacao>
    {
        protected PessoaBL PessoaBL { get; set; }
        protected GrupoTributarioBL GrupoTributarioBL { get; set; }
        protected ProdutoBL ProdutoBL { get; set; }
        protected SubstituicaoTributariaBL SubstituicaoTributariaBL { get; set; }
        protected ParametroTributarioBL ParametroTributarioBL { get; set; }
        protected CertificadoDigitalBL CertificadoDigitalBL { get; set; }

        public TotalTributacaoBL(AppDataContextBase context, PessoaBL pessoaBL, GrupoTributarioBL grupoTributarioBL, ProdutoBL produtoBL, SubstituicaoTributariaBL substituicaoTributariaBL, ParametroTributarioBL parametroTributarioBL, CertificadoDigitalBL certificadoDigitalBL) : base(context)
        {
            PessoaBL = pessoaBL;
            GrupoTributarioBL = grupoTributarioBL;
            ProdutoBL = produtoBL;
            SubstituicaoTributariaBL = substituicaoTributariaBL;
            ParametroTributarioBL = parametroTributarioBL;
            CertificadoDigitalBL = certificadoDigitalBL;
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
            return ParametroTributarioBL.All.AsNoTracking().FirstOrDefault();
        }

        public void DadosValidosCalculoTributario(OrdemVenda entity, bool onList = true)
        {
            var pessoa = GetPessoa(entity.ClienteId);
            var clienteUF = pessoa != null ? (pessoa.Estado != null ? pessoa.Estado.Sigla : "") : "";

            var dadosEmpresa = GetDadosEmpresa(PlataformaUrl);
            var empresaUF = dadosEmpresa.Cidade != null ? (dadosEmpresa.Cidade.Estado != null ? dadosEmpresa.Cidade.Estado.Sigla : "") : "";
            var parametros = GetParametrosTributarios();

            if (string.IsNullOrEmpty(empresaUF) || string.IsNullOrEmpty(clienteUF) || parametros == null)
            {
                if (string.IsNullOrEmpty(empresaUF))
                    entity.Notification.Errors.Add(new Error("Informe o estado no cadastro da empresa"));

                if (string.IsNullOrEmpty(clienteUF))
                    entity.Notification.Errors.Add(new Error("Informe o estado no cadastro do cliente"));

                if(parametros == null)
                    entity.Notification.Errors.Add(new Error("Vá em Configurações e defina os seus parâmetros tributários"));

                if (!onList)
                    entity.Notification.Errors.Add(new Error("Salve o orçamento/pedido para finalizá-lo após as alterações"));

                throw new BusinessException(entity.Notification.Get());
            }
        }

        public EmpresaVM GetDadosEmpresa(string plataformaUrl)
        {
            return RestHelper.ExecuteGetRequest<EmpresaVM>($"{AppDefaults.UrlGateway}v2/", $"Empresa/{plataformaUrl}");
        }

        public bool ConfiguracaoTSSOK()
        {
            return VerificaConfiguracaoTSSOK();
        }

        private bool VerificaConfiguracaoTSSOK()
        {
            var ambiente = CertificadoDigitalBL.GetAmbiente();

            var entidade = CertificadoDigitalBL.GetEntidade(ambiente);
            
            try
            {
                if (!string.IsNullOrEmpty(entidade))
                {
                    var header = new Dictionary<string, string>()
                    {
                        { "AppUser", AppUser },
                        { "PlataformaUrl", entidade }
                    };
                    var resourceById = $"configuracaoOK?entidade={entidade}&tipoAmbiente={ambiente}";
                    var response = RestHelper.ExecuteGetRequest<JObject>(AppDefaults.UrlEmissaoNfeApi, resourceById, header, null);
                    return true;
                }
                else
                {
                    throw new BusinessException("Revise as configurações do seu certificado digital(Entidade TSS)");
                }
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        public double TotalSomaTributacaoItem(List<TributacaoItemRetorno> tributacaoItensRetorno)
        {
            var total = 0.0;
            total += tributacaoItensRetorno.Where(x => x.ICMSAgregaTotal).Sum(x => x.ICMSValor);
            total += tributacaoItensRetorno.Sum(x => x.COFINSValor);
            total += tributacaoItensRetorno.Where(x => x.FCPAgregaTotal).Sum(x => x.FCPValor);
            total += tributacaoItensRetorno.Where(x => x.IPIAgregaTotal).Sum(x => x.IPIValor);
            total += tributacaoItensRetorno.Sum(x => x.PISValor);
            total += tributacaoItensRetorno.Where(x => x.STAgregaTotal).Sum(x => x.STValor);
            return total;
        }

        public double TotalSomaTributacaoProduto(List<TributacaoProduto> tributacaoItens, Guid clienteId, TipoFrete tipoFrete, double? valorFrete = 0)
        {
            return TotalSomaTributacaoItem(TotalTributacaoProduto(tributacaoItens, clienteId, tipoFrete, valorFrete).ToList<TributacaoItemRetorno>());
        }

        public double TotalSomaTributacaoServico(List<TributacaoServico> tributacaoItens, Guid clienteId)
        {
            return TotalSomaTributacaoItem(TotalTributacaoServico(tributacaoItens, clienteId).ToList<TributacaoItemRetorno>());
        }

        public List<TributacaoServicoRetorno> TotalTributacaoServico(List<TributacaoServico> tributacaoItens, Guid clienteId)
        {
            var cliente = GetPessoa(clienteId);
            var estadoOrigem = GetDadosEmpresa(PlataformaUrl).Cidade.Estado.Sigla;
            var parametros = GetParametrosTributarios();
            var result = new List<TributacaoServicoRetorno>()
            {
                new TributacaoServicoRetorno()
            };

            //TODO: ver Jamal
            //var parametros = ParametroTributarioBL.All.FirstOrDefault();
            //var somaPercentuais = parametros != null ? ((parametros.AliquotaSimplesNacional + parametros.AliquotaISS + parametros.AliquotaPISPASEP + parametros.AliquotaCOFINS) / 100) : 0;

            //return somaPercentuais > 0 ? (tributacaoServicos.Sum(x => x.Total) * somaPercentuais) : 0;
                        
            return result;
        }

        public List<TributacaoProdutoRetorno> TotalTributacaoProduto(List<TributacaoProduto> tributacaoItens, Guid clienteId, TipoFrete tipoFrete, double? valorFrete = 0)
        {
            var cliente = GetPessoa(clienteId);
            var estadoOrigem = GetDadosEmpresa(PlataformaUrl).Cidade.Estado.Sigla;
            var parametros = GetParametrosTributarios();
            var result = new List<TributacaoProdutoRetorno>();

            var header = new Dictionary<string, string>()
                {
                    { "AppUser", AppUser },
                    { "PlataformaUrl", PlataformaUrl }
                };

            double freteFracionado = tipoFrete == TipoFrete.CIF && valorFrete.HasValue ? valorFrete.Value / tributacaoItens.Sum(x => x.Quantidade) : 0;

            foreach (var itemProduto in tributacaoItens)
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

                var grupoTributario = GetGrupoTributario(itemProduto.GrupoTributarioId);
                var produto = ProdutoBL.All.AsNoTracking().Where(x => x.Id == itemProduto.ProdutoId).FirstOrDefault();
                
                //ICMS
                if (grupoTributario.CalculaIcms)
                {
                    tributacao.Icms = new Icms()
                    {
                        //Aliquota não preencher
                        DespesaNaBase = grupoTributario.AplicaDespesaBaseIcms,
                        Difal = grupoTributario.CalculaIcmsDifal,
                        FreteNaBase = grupoTributario.AplicaFreteBaseIcms,
                        EstadoDestino = cliente.Estado.Sigla,
                        EstadoOrigem = estadoOrigem
                    };
                    if (produto.AliquotaIpi > 0)
                    {
                        tributacao.Icms.IpiNaBase = grupoTributario.AplicaIpiBaseIcms;
                    }
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
                        x.EstadoDestinoId == cliente.EstadoId &
                        x.TipoSubstituicaoTributaria == TipoSubstituicaoTributaria.Saida
                        ).FirstOrDefault();

                    if (st != null)
                    {
                        tributacao.SubstituicaoTributaria = new EmissaoNFE.Domain.SubstituicaoTributaria()
                        {
                            EstadoDestino = cliente.Estado.Sigla,
                            EstadoOrigem = estadoOrigem,
                            FreteNaBase = grupoTributario.AplicaFreteBaseST,
                            DespesaNaBase = grupoTributario.AplicaDespesaBaseST,
                            Mva = st.Mva,
                        };
                        if (produto.AliquotaIpi > 0)
                        {
                            tributacao.SubstituicaoTributaria.IpiNaBase = grupoTributario.AplicaIpiBaseST;
                        }
                    }
                }
                //COFINS
                if(grupoTributario.CalculaCofins)
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
                if(responseTributacao.Fcp != null)
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
                result.Add(itemRetorno);
            }
            return result;
        }

        public List<TributacaoProdutoRetorno> TributacoesOrdemVendaProdutos(List<OrdemVendaProduto> ordemVendaProdutos, Guid clienteId, TipoFrete tipoFrete, double? valorFrete = 0)
        {
            return TotalTributacaoProduto(ordemVendaProdutos.Select(x => new TributacaoProduto()
            {
                Valor = x.Valor,
                Quantidade = x.Quantidade,
                Desconto = x.Desconto,
                Total = x.Total,
                ProdutoId = x.ProdutoId,
                GrupoTributarioId = x.GrupoTributarioId
            }).ToList(), clienteId, tipoFrete, valorFrete);
        }

        public double TotalSomaOrdemVendaProdutos(List<OrdemVendaProduto> ordemVendaProdutos, Guid clienteId, TipoFrete tipoFrete, double? valorFrete = 0)
        {
            //Transforma para a classe auxiliar
            return TotalSomaTributacaoProduto(ordemVendaProdutos.Select(x => new TributacaoProduto()
            {
                Valor = x.Valor,
                Quantidade = x.Quantidade,
                Desconto = x.Desconto,
                Total = x.Total,
                ProdutoId = x.ProdutoId,
                GrupoTributarioId = x.GrupoTributarioId
            }).ToList(), clienteId, tipoFrete, valorFrete);
        }

        public double TotalSomaOrdemVendaServicos(List<OrdemVendaServico> ordemVendaServicos, Guid clienteId)
        {
            //Transforma para a classe auxiliar
            return TotalSomaTributacaoServico(ordemVendaServicos.Select(x => new TributacaoServico()
            {
                Valor = x.Valor,
                Quantidade = x.Quantidade,
                Desconto = x.Desconto,
                Total = x.Total,
                GrupoTributarioId = x.GrupoTributarioId,
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

        public double PISBase { get; set; }

        public double PISAliquota { get; set; }

        public double PISValor { get; set; }        

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