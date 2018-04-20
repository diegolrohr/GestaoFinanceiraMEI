﻿using Fly01.Financeiro.Controllers.Base;
using Fly01.Financeiro.ViewModel;
using Fly01.Core;
using Fly01.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Fly01.Core.Rest;

namespace Fly01.Financeiro.Controllers
{
    public class DespesaPorCategoriaController : BaseController<DespesaPorCategoriaVM>
    {
        private DateTime _dataInicial, _dataFinal;
        private bool _somaRealizados, _somaPrevistos;

        public override Func<DespesaPorCategoriaVM, object> GetDisplayData()
        {
            return x => new
            {
                id = x.Id.ToString(),
                categoria = x.Categoria,
                categoriaPaiId = x.CategoriaPaiId,
                tipoCarteira = "(-) DESPESAS",
                soma = x.Soma.ToString("C", AppDefaults.CultureInfoDefault)
            };
        }

        public override Dictionary<string, string> GetQueryStringDefaultGridLoad()
        {
            var customFilters = base.GetQueryStringDefaultGridLoad();
            customFilters.AddParam("dataInicial", _dataInicial.ToString("yyyy-MM-dd 00:00:00.000"));
            customFilters.AddParam("dataFinal", _dataFinal.ToString("yyyy-MM-dd HH:mm:ss.fff"));
            customFilters.AddParam("somaRealizados", _somaRealizados.ToString());
            customFilters.AddParam("somaPrevistos", _somaPrevistos.ToString());
            return customFilters;
        }

        public JsonResult LoadDespesasPorCategoria(DateTime dataInicial, DateTime dataFinal)
        {
            _dataInicial = dataInicial;
            _dataFinal = dataFinal.AddDays(1).AddMilliseconds(-1);
            _somaRealizados = true;
            _somaPrevistos = false;
            return GridLoad();
        }

        public JsonResult Total(DateTime dataInicial, DateTime dataFinal)
        {
            _dataInicial = dataInicial;
            _dataFinal = dataFinal.AddDays(1).AddMilliseconds(-1);
            _somaRealizados = true;
            _somaPrevistos = false;

            var total = RestHelper.ExecuteGetRequest<ResultBase<DespesaPorCategoriaVM>>("DespesaPorCategoria", GetQueryStringDefaultGridLoad())
                                    .Data
                                    .Where(x => x.CategoriaPaiId == null)
                                    .Sum(x => x.Soma)
                                    .ToString("C", AppDefaults.CultureInfoDefault);

            return Json(new { total }, JsonRequestBehavior.AllowGet);
        }

        public override ContentResult List()
        {
            throw new NotImplementedException();
        }

        public override ContentResult Form()
        {
            throw new NotImplementedException();
        }
    }
}