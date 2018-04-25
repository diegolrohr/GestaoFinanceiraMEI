using Fly01.Faturamento.Controllers.Base;
using Fly01.Faturamento.ViewModel;
using Fly01.Core;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Fly01.Faturamento.Controllers
{
    public class NFeProdutoController : BaseController<NFeProdutoVM>
    {
        public NFeProdutoController()
        {
            ExpandProperties = "produto($select=descricao),grupoTributario($select=descricao)";
        }

        public override Func<NFeProdutoVM, object> GetDisplayData()
        {
            return x => new
            {
                id = x.Id.ToString(),
                produto_descricao = x.Produto.Descricao,
                grupoTributario_descricao = x.GrupoTributario.Descricao,
                quantidade = x.Quantidade.ToString("N", AppDefaults.CultureInfoDefault),
                valor = x.Valor.ToString("C", AppDefaults.CultureInfoDefault),
                desconto = x.Desconto.ToString("C", AppDefaults.CultureInfoDefault),
                total = x.Total.ToString("C", AppDefaults.CultureInfoDefault),
            };
        }

        public override ContentResult Form()
        {
            throw new NotImplementedException();
        }

        public override ContentResult List()
        {
            throw new NotImplementedException();
        }

        public JsonResult GetNFeProdutos(string id)
        {
            Dictionary<string, string> filters = new Dictionary<string, string>
            {
                { "notaFiscalId eq", string.IsNullOrEmpty(id) ? new Guid().ToString() : id }
            };
            return GridLoad(filters);
        }
    }
}