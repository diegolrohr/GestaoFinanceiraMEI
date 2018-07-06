using Fly01.Estoque.ViewModel;
using Fly01.Core;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Fly01.uiJS.Classes;
using Fly01.uiJS.Defaults;
using Fly01.Core.Rest;
using Fly01.Core.Presentation;
using Fly01.Core.Config;

namespace Fly01.Estoque.Controllers
{
    [OperationRole(ResourceKey = ResoucerHashConst.EstoqueEstoquePosicaoAtual)]
    public class PosicaoAtualController : BaseController<PosicaoAtualVM>
    {
        public override Dictionary<string, string> GetQueryStringDefaultGridLoad()
        {
            Dictionary<string, string> queryStringDefault = AppDefaults.GetQueryStringDefault();

            return queryStringDefault;
        }

        public override List<HtmlUIButton> GetListButtonsOnHeader()
        {
            var target = new List<HtmlUIButton>();

            if (UserCanPerformOperation(ResoucerHashConst.EstoqueEstoqueAjusteManual))
                target.Add(new HtmlUIButton { Id = "alterarEstoque", Label = "Alterar estoque", OnClickFn = "fnAjusteManual" });

            return target;
        }

        public JsonResult Totais()
        {
            var posicaoAtual = RestHelper.ExecuteGetRequest<PosicaoAtualVM>("posicaoatual");
            if (posicaoAtual == null)
                return Json(new { }, JsonRequestBehavior.AllowGet);

            return Json(new
            {
                estoqueTotal = posicaoAtual.EstoqueTotal,
                custoTotal = posicaoAtual.CustoTotal.ToString("C", AppDefaults.CultureInfoDefault),
                vendaTotal = posicaoAtual.VendaTotal.ToString("C", AppDefaults.CultureInfoDefault)
            }, JsonRequestBehavior.AllowGet);
        }

        public override ContentResult List()
        {
            var cfg = new ContentUI
            {
                History = new ContentUIHistory { Default = Url.Action("Index") },
                Header = new HtmlUIHeader
                {
                    Title = "Posição atual",
                    Buttons = new List<HtmlUIButton>(GetListButtonsOnHeader())
                },
                Functions = new List<string> { "fnFormReady", "fnAjusteManual", "fnTotais" },
                UrlFunctions = Url.Action("Functions") + "?fns=",
            };

            cfg.Content.Add(new CardUI
            {
                Class = "col s12 m4",
                Color = "green",
                Id = "fly01cardEstoque",
                Title = "Em Estoque",
                Placeholder = "0,00",
                Action = new LinkUI
                {
                    Label = "Total",
                }
            });

            cfg.Content.Add(new CardUI
            {
                Class = "col s12 m4",
                Color = "blue",
                Id = "fly01cardCustos",
                Title = "Custo",
                Placeholder = "R$ 0,00",
                Action = new LinkUI
                {
                    Label = "Total",
                }
            });

            cfg.Content.Add(new CardUI
            {
                Class = "col s12 m4",
                Color = "totvs-blue",
                Id = "fly01cardVendas",
                Title = "Venda",
                Placeholder = "R$ 0,00",
                Action = new LinkUI
                {
                    Label = "Total",
                }
            });

            var config = new DataTableUI
            {
                Id = "fly01dtprodutos",
                UrlGridLoad = Url.Action("GridLoadPos", "Produto"),
                UrlFunctions = Url.Action("Functions") + "?fns=",
                Options = new DataTableUIConfig { PageLength = 30, WithoutRowMenu = true }
            };

            config.Columns.Add(new DataTableUIColumn { DataField = "codigoProduto", DisplayName = "Código", Priority = 1 });
            config.Columns.Add(new DataTableUIColumn { DataField = "descricao", DisplayName = "Descrição", Priority = 2 });
            config.Columns.Add(new DataTableUIColumn { DataField = "valorCusto", DisplayName = "Valor custo", Priority = 3, Type = "currency" });
            config.Columns.Add(new DataTableUIColumn { DataField = "valorVenda", DisplayName = "Valor venda", Priority = 4, Type = "currency" });
            config.Columns.Add(new DataTableUIColumn { DataField = "saldoProduto", DisplayName = "Qtd. Estoque", Priority = 5 });
            config.Columns.Add(new DataTableUIColumn { DataField = "unidadeMedida_sigla", DisplayName = "Un. Medida", Priority = 6, Searchable = false, Orderable = false });
            config.Columns.Add(new DataTableUIColumn { DataField = "custoTotal", DisplayName = "Custo total", Priority = 7, Type = "currency", Searchable = false, Orderable = false });
            config.Columns.Add(new DataTableUIColumn { DataField = "vendaTotal", DisplayName = "Venda total", Priority = 8, Type = "currency", Searchable = false, Orderable = false });

            cfg.Content.Add(config);

            return Content(JsonConvert.SerializeObject(cfg, JsonSerializerSetting.Default), "application/json");
        }

        public override ContentResult Form() { throw new NotImplementedException(); }

        public override Func<PosicaoAtualVM, object> GetDisplayData()
        {
            throw new NotImplementedException();
        }
    }
}