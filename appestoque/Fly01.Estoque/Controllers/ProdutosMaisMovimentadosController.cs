﻿using Fly01.Core.Helpers;
using Fly01.Core.Presentation;
using Fly01.Estoque.ViewModel;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Fly01.Estoque.Controllers
{
    public class ProdutosMaisMovimentadosController : BaseController<ProdutosMaisMovimentadosVM>
    {
        private DateTime _dataInicial, _dataFinal;

        public override Func<ProdutosMaisMovimentadosVM, object> GetDisplayData()
        {
            return x => new { id = x.Id.ToString(),
                              descricao = x.Descricao,
                              saldoProduto = x.SaldoProduto,
                              totalMovimentos = x.TotalMovimentos};
        }

        public override Dictionary<string, string> GetQueryStringDefaultGridLoad()
        {
            var customFilters = base.GetQueryStringDefaultGridLoad();
            customFilters.AddParam("dataInicial", _dataInicial.ToString("yyyy-MM-dd 00:00:00.000"));
            customFilters.AddParam("dataFinal", _dataFinal.ToString("yyyy-MM-dd HH:mm:ss.fff"));
            return customFilters;
        }

        [OperationRole(NotApply = true)]
        public JsonResult GridLoadProdutosMaisMovimentados(DateTime dataInicial, DateTime dataFinal)
        {
            _dataInicial = dataInicial;
            _dataFinal = dataFinal.AddDays(1).AddMilliseconds(-1);
            return GridLoad();
        }

        public override ContentResult List() { throw new NotImplementedException(); }
        public override ContentResult Form() { throw new NotImplementedException(); }
    }
}