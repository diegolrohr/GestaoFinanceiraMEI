﻿using Fly01.Faturamento.ViewModel;
using Fly01.Core;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Fly01.Core.Presentation;

namespace Fly01.Faturamento.Controllers
{
    public class NFSeServicoController : BaseController<NFSeServicoVM>
    {
        public NFSeServicoController()
        {
            ExpandProperties = "servico($select=descricao),grupoTributario($select=descricao)";
        }

        public override Func<NFSeServicoVM, object> GetDisplayData()
        {
            return x => new
            {
                id = x.Id.ToString(),
                servico_descricao = x.Servico.Descricao,
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

        public JsonResult GetNFSeServicos(string id)
        {
            Dictionary<string, string> filters = new Dictionary<string, string>
            {
                { "notaFiscalId eq", string.IsNullOrEmpty(id) ? new Guid().ToString() : id }
            };
            return GridLoad(filters);
        }
    }
}