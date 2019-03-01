﻿using Fly01.uiJS.Classes;
using Fly01.uiJS.Defaults;
using Newtonsoft.Json;
using System;
using System.Web.Mvc;
using Fly01.Core.Presentation;
using Fly01.Core.ViewModels.Presentation.Commons;

namespace Fly01.Estoque.Controllers
{
    [OperationRole(ResourceKey = ResourceHashConst.EstoqueEstoqueRelatorios)]
    public class RelatorioController : BaseController<DomainBaseVM>
    {
        public override Func<DomainBaseVM, object> GetDisplayData()
        {
            throw new NotImplementedException();
        }

        public override ContentResult List()
        {   
            var cfg = new ContentUI
            {
                History = new ContentUIHistory { Default = Url.Action("Index") },
                Header = new HtmlUIHeader
                {
                    Title = "Relatórios",
                },
                UrlFunctions = Url.Action("Functions", "Relatorio", null, Request.Url.Scheme) + "?fns=",
                SidebarUrl = Url.Action("Sidebar", "Home")
            };

           
            cfg.Content.Add(new CardUI
            {
                Class = "col s12 m4",
                Color = "totvs-blue",
                Id = "fly01cardMovimentoEstoque",
                Title = "Movimentação Estoque",
                Placeholder = "",
                Action = new LinkUI
                {
                    Label = "Relatório de Movimentações",
                    OnClick = @Url.Action("List", "MovimentoEstoque")
                }
            });

            return Content(JsonConvert.SerializeObject(cfg, JsonSerializerSetting.Front), "application/json");
        }

        protected override ContentUI FormJson()
        {
            throw new NotImplementedException();
        }
    }
}