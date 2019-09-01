using System.Linq;
using System.Web.Mvc;
using Fly01.Core.Rest;
using Fly01.Core.Helpers;
using Fly01.Core.ViewModels.Presentation.Commons;
using Fly01.Core.Entities.Domains.Enum;
using System.Collections.Generic;
using Fly01.Core.ViewModels;

namespace Fly01.Core.Presentation.Controllers
{
    [OperationRole(NotApply = true)]
    public class AutoCompleteBaseController : PrimitiveBaseController
    {
        protected JsonResult GetJson(object data) 
            => Json(data, JsonRequestBehavior.AllowGet);

        public JsonResult EstadoSemEX(string term)
        {
            var resourceName = AppDefaults.GetResourceName(typeof(EstadoVM));
            var queryString = AppDefaults.GetQueryStringDefault();

            queryString.AddParam("$filter", $"(contains(nome, '{term}') or contains(sigla, '{term}')) and sigla ne 'EX'");
            queryString.AddParam("$select", "id,nome,sigla,codigoIbge");
            queryString.AddParam("$orderby", "nome");

            var filterObjects = from item in RestHelper.ExecuteGetRequest<ResultBase<EstadoVM>>(resourceName, queryString).Data
                                select new { id = item.Id, label = item.Nome, detail = item.Sigla, estadoCodigoIbge = item.CodigoIbge };

            return GetJson(filterObjects);
        }

        public JsonResult Estado(string term)
        {
            var resourceName = AppDefaults.GetResourceName(typeof(EstadoVM));
            var queryString = AppDefaults.GetQueryStringDefault();

            queryString.AddParam("$filter", $"contains(nome, '{term}') or contains(sigla, '{term}')");
            queryString.AddParam("$select", "id,nome,sigla,codigoIbge");
            queryString.AddParam("$orderby", "nome");

            var filterObjects = from item in RestHelper.ExecuteGetRequest<ResultBase<EstadoVM>>(resourceName, queryString).Data
                                select new { id = item.Id, label = item.Nome, detail = item.Sigla, estadoCodigoIbge = item.CodigoIbge };

            return GetJson(filterObjects);
        }

        public JsonResult Cidade(string term, string prefilter = "")
        {
            if (string.IsNullOrEmpty(prefilter)) return null;

            var resourceName = AppDefaults.GetResourceName(typeof(CidadeVM));
            var queryString = AppDefaults.GetQueryStringDefault();

            queryString.AddParam("$filter", $"contains(nome, '{term}') and estadoId eq {prefilter}");
            queryString.AddParam("$select", "id,nome,estadoId,codigoIbge");
            queryString.AddParam("$orderby", "nome");

            var filterObjects = from item in RestHelper.ExecuteGetRequest<ResultBase<CidadeVM>>(resourceName, queryString).Data
                                select new { id = item.Id, label = item.Nome, estadoId = item.EstadoId, cidadeCodigoIbge = item.CodigoIbge };

            return GetJson(filterObjects);
        }

        public JsonResult EstadoSigla(string term)
        {
            var resourceName = AppDefaults.GetResourceName(typeof(EstadoVM));
            var queryString = AppDefaults.GetQueryStringDefault();

            queryString.AddParam("$filter", $"contains(nome, '{term}')");
            queryString.AddParam("$select", "nome,sigla");
            queryString.AddParam("$orderby", "nome");

            var filterObjects = from item in RestHelper.ExecuteGetRequest<ResultBase<EstadoVM>>(resourceName, queryString).Data
                                select new { id = item.Sigla.ToUpper(), label = item.Nome, detail = item.Sigla };

            return GetJson(filterObjects);
        }

        public JsonResult Ncm(string term)
        {
            var resourceName = AppDefaults.GetResourceName(typeof(NcmVM));
            var queryString = AppDefaults.GetQueryStringDefault();

            queryString.AddParam("$filter", $"contains(descricao, '{term}') or contains(codigo, '{term}')");
            queryString.AddParam("$select", "id,codigo,descricao,aliquotaIPI");
            queryString.AddParam("$orderby", "codigo");

            var filterObjects = from item in RestHelper.ExecuteGetRequest<ResultBase<NcmVM>>(resourceName, queryString).Data
                                select new
                                {
                                    id = item.Id,
                                    label = item.Descricao,
                                    detail = string.Format("Cod: {0} - Alíquota IPI: {1}", item.Codigo, (item.AliquotaIPI / 100).ToString("P", AppDefaults.CultureInfoDefault))
                                };

            return GetJson(filterObjects);
        }

        public JsonResult Cest(string term, string prefilter = "")
        {
            if (string.IsNullOrEmpty(prefilter)) return null;

            var resourceName = AppDefaults.GetResourceName(typeof(CestVM));
            var queryString = AppDefaults.GetQueryStringDefault();

            queryString.AddParam("$filter", $"(contains(descricao, '{term}') or contains(codigo, '{term}')) and ncmId eq {prefilter}");
            queryString.AddParam("$select", "id,codigo,descricao,segmento");
            queryString.AddParam("$orderby", "codigo");

            var filterObjects = from item in RestHelper.ExecuteGetRequest<ResultBase<CestVM>>(resourceName, queryString).Data
                                select new
                                {
                                    id = item.Id,
                                    label = item.Descricao,
                                    detail = $"Código: {item.Codigo} - Segmento: {item.Segmento}"
                                };

            return GetJson(filterObjects);
        }

        public JsonResult Pessoa(string term)
        {
            var resourceName = AppDefaults.GetResourceName(typeof(PessoaVM));
            var queryString = AppDefaults.GetQueryStringDefault();

            queryString.AddParam("$filter", $"contains(nome, '{term}') or contains(cpfcnpj, '{term}')");
            queryString.AddParam("$select", "id,nome,cpfcnpj");
            queryString.AddParam("$orderby", "nome");

            var filterObjects = from item in RestHelper.ExecuteGetRequest<ResultBase<PessoaVM>>(resourceName, queryString).Data
                                select new { id = item.Id, label = item.Nome, detail = item.CPFCNPJ == string.Empty ? "(Sem documento)" : item.CPFCNPJ };

            return GetJson(filterObjects);
        }

        public JsonResult Fornecedor(string term)
        {
            var resourceName = AppDefaults.GetResourceName(typeof(PessoaVM));
            var queryString = AppDefaults.GetQueryStringDefault();

            queryString.AddParam("$filter", $"(contains(nome, '{term}') or contains(cpfcnpj, '{term}')) and fornecedor eq true");
            queryString.AddParam("$select", "id,nome,cpfcnpj");
            queryString.AddParam("$orderby", "nome");

            var filterObjects = from item in RestHelper.ExecuteGetRequest<ResultBase<PessoaVM>>(resourceName, queryString).Data
                                select new { id = item.Id, label = item.Nome, detail = item.CPFCNPJ == string.Empty ? "(Sem documento)" : item.CPFCNPJ };

            return GetJson(filterObjects);
        }

        public JsonResult Cliente(string term)
        {
            var resourceName = AppDefaults.GetResourceName(typeof(PessoaVM));
            var queryString = AppDefaults.GetQueryStringDefault();

            queryString.AddParam("$filter", $"(contains(nome, '{term}') or contains(cpfcnpj, '{term}')) and cliente eq true");
            queryString.AddParam("$select", "id,nome,cpfcnpj");
            queryString.AddParam("$orderby", "nome");

            var filterObjects = from item in RestHelper.ExecuteGetRequest<ResultBase<PessoaVM>>(resourceName, queryString).Data
                                select new { id = item.Id, label = item.Nome, detail = item.CPFCNPJ == string.Empty ? "(Sem documento)" : item.CPFCNPJ };

            return GetJson(filterObjects);
        }

        public JsonResult UnidadeMedida(string term)
        {
            var resourceName = AppDefaults.GetResourceName(typeof(UnidadeMedidaVM));
            var queryString = AppDefaults.GetQueryStringDefault();

            queryString.AddParam("$filter", $"contains(descricao, '{term}') or contains(abreviacao, '{term}')");
            queryString.AddParam("$select", "id,descricao,abreviacao");
            queryString.AddParam("$orderby", "descricao");

            var filterObjects = from item in RestHelper.ExecuteGetRequest<ResultBase<UnidadeMedidaVM>>(resourceName, queryString).Data
                                select new { id = item.Id, label = item.Descricao, detail = item.Abreviacao };

            return GetJson(filterObjects);
        }

        public JsonResult GrupoProduto(string term, string prefilter = "")
        {
            var resourceName = AppDefaults.GetResourceName(typeof(GrupoProdutoVM));
            var queryString = AppDefaults.GetQueryStringDefault();

            queryString.AddParam("$filter", $"contains(descricao, '{term}') and tipoProduto eq {AppDefaults.APIEnumResourceName}TipoProduto'{prefilter}'");
            queryString.AddParam("$select", "id,descricao,aliquotaIpi,ncmId,unidadeMedidaId");
            queryString.AddParam("$orderby", "descricao");

            var filterObjects = from item in RestHelper.ExecuteGetRequest<ResultBase<GrupoProdutoVM>>(resourceName, queryString).Data
                                select new { id = item.Id, label = item.Descricao, detail = "" };

            return GetJson(filterObjects);
        }

        public JsonResult FormaPagamento(string term)
        {
            var resourceName = AppDefaults.GetResourceName(typeof(FormaPagamentoVM));
            var queryString = AppDefaults.GetQueryStringDefault();

            queryString.AddParam("$filter", string.Format("contains(descricao, '{0}')", term));
            queryString.AddParam("$select", "id,descricao,tipoFormaPagamento");
            queryString.AddParam("$orderby", "descricao");

            var filterObjects = from item in RestHelper.ExecuteGetRequest<ResultBase<FormaPagamentoVM>>(resourceName, queryString).Data
                                select new { id = item.Id, label = item.Descricao, detail = EnumHelper.GetValue(typeof(TipoFormaPagamento), item.TipoFormaPagamento) };

            return GetJson(filterObjects);
        }

        public JsonResult Produto(string term)
        {
            var resourceName = AppDefaults.GetResourceName(typeof(ProdutoVM));
            var queryString = AppDefaults.GetQueryStringDefault();

            queryString.AddParam("$expand", "unidadeMedida($select=abreviacao)");
            queryString.AddParam("$filter", $"(contains(descricao, '{term}') or contains(codigoProduto, '{term}') or contains(codigoBarras, '{term}')) and (objetoDeManutencao eq {AppDefaults.APIEnumResourceName}ObjetoDeManutencao'Nao')");
            queryString.AddParam("$select", "id,descricao,codigoProduto,codigoBarras,valorCusto,saldoProduto");
            queryString.AddParam("$orderby", "descricao");

            var filterObjects = from item in RestHelper.ExecuteGetRequest<ResultBase<ProdutoVM>>(resourceName, queryString).Data
                                select new { id = item.Id, label = item.Descricao, detail = $"Código Produto: {item.CodigoProduto} - Código de Barras: {item.CodigoBarras}", valor = item.ValorCusto, unidadeSigla = item.UnidadeMedida?.Abreviacao };

            return GetJson(filterObjects);
        }

        public JsonResult EnquadramentoLegalIPI(string term)
        {
            var resourceName = AppDefaults.GetResourceName(typeof(EnquadramentoLegalIpiVM));
            var queryString = AppDefaults.GetQueryStringDefault();

            queryString.AddParam("$filter", $"contains(descricao, '{term}') or contains(codigo, '{term}')");
            queryString.AddParam("$select", "id,codigo,descricao,grupoCST");
            queryString.AddParam("$orderby", "codigo");

            var filterObjects = from item in RestHelper.ExecuteGetRequest<ResultBase<EnquadramentoLegalIpiVM>>(resourceName, queryString).Data
                                select new
                                {
                                    id = item.Id,
                                    label = item.Descricao,
                                    detail = string.Format("Grupo: {0} - Código: {1}", item.GrupoCST, item.Codigo)
                                };

            return GetJson(filterObjects);
        }

        public JsonResult CondicaoParcelamento(string term)
        {
            var resourceName = AppDefaults.GetResourceName(typeof(CondicaoParcelamentoVM));
            var queryString = AppDefaults.GetQueryStringDefault();

            queryString.AddParam("$filter", $"contains(descricao, '{term}')");
            queryString.AddParam("$select", "id,descricao,condicoesParcelamento,qtdParcelas");
            queryString.AddParam("$orderby", "descricao");

            var filterObjects = from item in RestHelper.ExecuteGetRequest<ResultBase<CondicaoParcelamentoVM>>(resourceName, queryString).Data
                                select new { id = item.Id, label = item.Descricao, condicoesParcelamento = item.CondicoesParcelamento, qtdParcelas = item.QtdParcelas };

            return GetJson(filterObjects);
        }

        public JsonResult Transportadora(string term)
        {
            var resourceName = AppDefaults.GetResourceName(typeof(PessoaVM));
            var queryString = AppDefaults.GetQueryStringDefault();

            queryString.AddParam("$filter", $"(contains(nome, '{term}') or contains(cpfcnpj, '{term}')) and transportadora eq true");
            queryString.AddParam("$select", "id,nome,cpfcnpj");
            queryString.AddParam("$orderby", "nome");

            var filterObjects = from item in RestHelper.ExecuteGetRequest<ResultBase<PessoaVM>>(resourceName, queryString).Data
                                select new { id = item.Id, label = item.Nome, detail = item.CPFCNPJ == string.Empty ? "(Sem documento)" : item.CPFCNPJ };

            return GetJson(filterObjects);
        }

        public JsonResult CategoriaPai(string term, string prefilter)
        {
            var queryString = AppDefaults.GetQueryStringDefault();
            var resourceName = AppDefaults.GetResourceName(typeof(CategoriaVM));

            var filterTipoCarteira = (prefilter != "")
                ? $"tipoCarteira eq {AppDefaults.APIEnumResourceName}TipoCarteira'{prefilter}'"
                : "";

            queryString.AddParam("$filter", $"contains(descricao, '{term}') and {filterTipoCarteira} and categoriaPaiId eq null");
            queryString.AddParam("$select", "id,descricao,categoriaPaiId,tipoCarteira");

            var filterObjects = from item in RestHelper.ExecuteGetRequest<ResultBase<CategoriaVM>>(resourceName, queryString).Data
                                select new { id = item.Id, label = item.Descricao, categoriaPaiId = item.CategoriaPaiId, level = item.Level };

            return GetJson(filterObjects);
        }

        public virtual JsonResult Categoria(string term, string filterTipoCarteira)
        {
            var queryString = AppDefaults.GetQueryStringDefault();
            var resourceName = AppDefaults.GetResourceName(typeof(CategoriaVM));

            queryString.AddParam("$filter", $"contains(descricao, '{term}') {filterTipoCarteira}");
            queryString.AddParam("$select", "id,descricao,categoriaPaiId,tipoCarteira");

            var filterObjects = from item in RestHelper.ExecuteGetRequest<ResultBase<CategoriaVM>>(resourceName, queryString).Data
                                select new { id = item.Id, label = item.Descricao, detail = item.TipoCarteira.ToString(), categoriaPaiId = item.CategoriaPaiId, level = item.Level };

            return GetJson(filterObjects);
        }

        public virtual JsonResult Cfop(string term)
        {
            var resourceName = AppDefaults.GetResourceName(typeof(CfopVM));
            int codigo = 0;
            int.TryParse(term, out codigo);
            var queryString = AppDefaults.GetQueryStringDefault();

            queryString.AddParam("$filter", $"(contains(descricao, '{term}') or codigo eq {codigo})");
            queryString.AddParam("$select", "id,descricao,codigo");
            queryString.AddParam("$orderby", "descricao");

            var filterObjects = from item in RestHelper.ExecuteGetRequest<ResultBase<CfopVM>>(resourceName, queryString).Data
                                select new { id = item.Id, label = item.Descricao, detail = item.Codigo };

            return GetJson(filterObjects);
        }

        public virtual JsonResult GrupoTributario(string term)
        {
            var resourceName = AppDefaults.GetResourceName(typeof(GrupoTributarioVM));
            var queryString = AppDefaults.GetQueryStringDefault();

            queryString.AddParam("$filter", $"contains(descricao, '{term}')");
            queryString.AddParam("$expand", "cfop($select=descricao)");
            queryString.AddParam("$select", "id,descricao,tipoTributacaoICMS");
            queryString.AddParam("$orderby", "descricao");

            var filterObjects = from item in RestHelper.ExecuteGetRequest<ResultBase<GrupoTributarioVM>>(resourceName, queryString).Data
                                select new
                                {
                                    id = item.Id,
                                    label = item.Descricao,
                                    detail = "",
                                    tipoTributacaoICMS = item.TipoTributacaoICMS,
                                    cfopDescricao = item?.Cfop?.Descricao?.Substring(8) ?? ""
                                };

            return GetJson(filterObjects);
        }

        public JsonResult ProdutoServico(string term, string prefilter)
        {
            var resourceName = AppDefaults.GetResourceName(typeof(ProdutoServicoVM));
            var queryString = new Dictionary<string, string> {
                { "kitId", prefilter },
                { "filtro", term }
            };
            var filterObjects = from item in RestHelper.ExecuteGetRequest<ResultBase<ProdutoServicoVM>>(resourceName, queryString).Data
                                select new
                                {
                                    id = item.Id,
                                    label = item.Descricao,
                                    detail = item.TipoItemDescricao,
                                    tipoItem = item.TipoItem,
                                    tipoItemDescricao = item.TipoItemDescricao,
                                    produtoId = (item.TipoItem == TipoItem.Produto ? item.Id.ToString() : null),
                                    servicoId = (item.TipoItem == TipoItem.Servico ? item.Id.ToString() : null)
                                };

            return GetJson(filterObjects);
        }

        public JsonResult Kit(string term)
        {
            var resourceName = AppDefaults.GetResourceName(typeof(KitVM));
            var queryString = AppDefaults.GetQueryStringDefault();

            queryString.AddParam("$filter", $"(contains(descricao,'{term}'))");
            queryString.AddParam("$select", "id,descricao");
            queryString.AddParam("$orderby", "descricao");

            var filterObjects = from item in RestHelper.ExecuteGetRequest<ResultBase<KitVM>>(resourceName, queryString).Data
                                select new { id = item.Id, label = item.Descricao };

            return GetJson(filterObjects);
        }

        public JsonResult AliquotaSimplesNacional(string term, string prefilter)
        {
            var resourceName = AppDefaults.GetResourceName(typeof(AliquotaSimplesNacionalVM));
            var queryString = AppDefaults.GetQueryStringDefault();

            queryString.AddParam("$filter", $"tipoFaixaReceitaBruta eq {AppDefaults.APIEnumResourceName}TipoFaixaReceitaBruta'{prefilter}'");
            queryString.AddParam("$orderby", "tipoEnquadramentoEmpresa");

            var filterObjects = from item in RestHelper.ExecuteGetRequest<ResultBase<AliquotaSimplesNacionalVM>>(resourceName, queryString).Data
                                select new
                                {
                                    id = item.TipoEnquadramentoEmpresa,//utilizado no form
                                    label = EnumHelper.GetValue(typeof(TipoEnquadramentoEmpresa), item.TipoEnquadramentoEmpresa),
                                    simplesNacional = item.SimplesNacional,
                                    impostoRenda = item.ImpostoRenda,
                                    csll = item.Csll,
                                    cofins = item.Cofins,
                                    pisPasep = item.PisPasep,
                                    iss = item.Iss
                                };

            return GetJson(filterObjects);
        }

        public JsonResult EstadoManagerNew(string term)
        {
            var filterObjects = from item in RestHelper.ExecuteGetRequest<ResponseDataNewGatewayVM<ManagerEstadoVM>>($"{AppDefaults.UrlManagerNew}", $"state/{term}")?.Data
                                select new { id = item.Id, label = item.Nome, detail = item.Sigla };

            return GetJson(filterObjects);
        }

        //http://10.51.8.188:3000/api/manager/city?term=porto&stateId=10
        public JsonResult CidadeManagerNew(string term, string prefilter)
        {
            var queryString = new Dictionary<string, string> {
                { "term", term },
                { "stateId", prefilter },
            };

            var filterObjects = from item in RestHelper.ExecuteGetRequest<ResponseDataNewGatewayVM<ManagerCidadeVM>>($"{AppDefaults.UrlManagerNew}", $"city", queryString)?.Data
                                select new { id = item.Id, label = item.Nome, detail = item.CodigoIbge };

            return GetJson(filterObjects);
        }
    }
}