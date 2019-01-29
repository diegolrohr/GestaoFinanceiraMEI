using Fly01.Core;
using Fly01.Core.Defaults;
using Fly01.Core.Entities.Domains.Enum;
using Fly01.Core.Helpers;
using Fly01.Core.Presentation;
using Fly01.Core.Presentation.Commons;
using Fly01.Core.Rest;
using Fly01.Core.ViewModels.Presentation.Commons;
using Fly01.uiJS.Classes;
using Fly01.uiJS.Classes.Elements;
using Fly01.uiJS.Classes.Helpers;
using Fly01.uiJS.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Serialization;
using Fly01.EmissaoNFE.Domain.Entities.NFe;
using Fly01.Core.ViewModels;
using System.Dynamic;

namespace Fly01.Compras.Controllers
{
    [OperationRole(ResourceKey = ResourceHashConst.ComprasComprasNFeImportacao)]
    public class NFeImportacaoProdutoController : BaseController<NFeImportacaoProdutoVM>
    {
        public NFeImportacaoProdutoController()
        {
            //TODO: Select properties
            ExpandProperties = "produto,unidadeMedida";
        }

        public override Func<NFeImportacaoProdutoVM, object> GetDisplayData()
        {
            return x => new
            {
                id = x.Id,
                nfeImportacaoId = x.NFeImportacaoId,
                novoProduto = x.NovoProduto,
                produtoId = x.ProdutoId,
                codigo = x.Codigo,
                codigoBarras = x.CodigoBarras,
                descricao = x.Descricao,
                quantidadeString = x.Quantidade.ToString("R", AppDefaults.CultureInfoDefault),
                quantidade = x.Quantidade,
                valorString = x.Valor.ToString("C", AppDefaults.CultureInfoDefault),
                valor = x.Valor,
                unidadeMedida_abreviacao = x.UnidadeMedida.Abreviacao,
                fatorConversaoString = x.Valor.ToString("R", AppDefaults.CultureInfoDefault),
                fatorConversao = x.FatorConversao,
                tipoFatorConversao = x.TipoFatorConversao,
                tipoFatorConversaoDescription = EnumHelper.GetDescription(typeof(TipoFatorConversao), x.TipoFatorConversao),
                tipoFatorConversaoCssClass = EnumHelper.GetCSS(typeof(TipoFatorConversao), x.TipoFatorConversao),
                tipoFatorConversaoValue = EnumHelper.GetValue(typeof(TipoFatorConversao), x.TipoFatorConversao),
                movimentaEstoque = x.MovimentaEstoque,
                atualizaDadosProduto = x.AtualizaDadosProduto,
                atualizaValorCompra = x.AtualizaValorCompra,
                atualizaValorVenda = x.AtualizaValorVenda,
                valorVendaString = x.ValorVenda.ToString("C", AppDefaults.CultureInfoDefault),
                valorVenda = x.ValorVenda,
                tipoValorVenda = x.TipoValorVenda,
                tipoValorVendaDescription = EnumHelper.GetDescription(typeof(TipoAtualizacaoValor), x.TipoValorVenda),
                tipoValorVendaCssClass = EnumHelper.GetCSS(typeof(TipoAtualizacaoValor), x.TipoValorVenda),
                tipoValorVendaValue = EnumHelper.GetValue(typeof(TipoAtualizacaoValor), x.TipoValorVenda)
            };
        }

        public override ContentResult List()
        {
            throw new NotImplementedException();
        }

        protected override ContentUI FormJson()
        {
            throw new NotImplementedException();
        }

        public JsonResult GetNFeImportacaoProdutos(string id)
        {
            Dictionary<string, string> filters = new Dictionary<string, string>
            {
                { "nFeImportacaoId eq", string.IsNullOrEmpty(id) ? new Guid().ToString() : id }
            };
            return GridLoad(filters);
        }

        public JsonResult GetNFeImportacaoProdutosPendencia(string id)
        {
            Dictionary<string, string> filters = new Dictionary<string, string>
            {
                { "nFeImportacaoId eq", string.IsNullOrEmpty(id) ? new Guid().ToString() : id },
                { " and produtoId eq", "null" }
            };
            return GridLoad(filters);
        }
    }
}
