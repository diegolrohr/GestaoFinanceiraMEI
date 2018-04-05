﻿using Fly01.Core.Helpers;
using Fly01.Estoque.Controllers.Base;
using Fly01.Estoque.Entities.ViewModel;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Fly01.Estoque.Controllers
{
    public class ProdutosMenosMovimentadosController : BaseController<ProdutosMenosMovimentadosVM>
    {
        private DateTime _dataInicial, _dataFinal;

        public override Func<ProdutosMenosMovimentadosVM, object> GetDisplayData()
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

        public JsonResult GridLoadProdutosMenosMovimentados(DateTime dataInicial, DateTime dataFinal)
        {
            _dataInicial = dataInicial;
            _dataFinal = dataFinal.AddDays(1).AddMilliseconds(-1);
            return GridLoad();
        }

        public override ContentResult List() { throw new NotImplementedException(); }
        public override ContentResult Form() { throw new NotImplementedException(); }
    }
}