using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Fly01.Core.Entities.Domains.Enum;
using Fly01.Core.Presentation;
using Fly01.Core.Presentation.Commons;
using Fly01.Core.ViewModels.Presentation.Commons;
using Fly01.uiJS.Classes;
using Fly01.uiJS.Classes.Elements;
using Fly01.uiJS.Classes.Helpers;

namespace Fly01.Estoque.Controllers
{
    public class RelatorioProdutoController : BaseController<ProdutoVM>
    {

        private string GrupoProdutoResourceHash { get; set; }
        public override Func<ProdutoVM, object> GetDisplayData()
        {
            throw new NotImplementedException();
        }

        public override ContentResult List()
        {
            throw new NotImplementedException();
        }

        protected override ContentUI FormJson()
        {
            var cfg = new ContentUIBase(Url.Action("Sidebar", "Home"))
            {
                History = new ContentUIHistory
                {
                    Default = Url.Action("Create"),
                    WithParams = Url.Action("Edit")
                },
                Header = new HtmlUIHeader
                {
                    Title = "Relatório Produtos",
                    Buttons = new List<HtmlUIButton>(GetFormButtonsOnHeader())
                },
                UrlFunctions = Url.Action("Functions") + "?fns="
            };

            var config = new FormUI
            {
                Id = "fly01frmRelatorioProduto",
                Action = new FormUIAction
                {
                    //Create = @Url.Action("Create"),
                    //List = Url.Action("Form")
                },
                UrlFunctions = Url.Action("Functions") + "?fns="
            };

            config.Elements.Add(new InputTextUI { Id = "descricaoid", Class = "col s12 m4", Label = "Descrição", MaxLength = 200 });
            config.Elements.Add(new InputTextUI { Id = "codigoId", Class = "col s12 m2", Label = "Código"});
            config.Elements.Add(new SelectUI
            {
                Id = "tipoProduto",
                Class = "col s12 m3",
                Label = "Tipo",
                Required = true,
                Options = new List<SelectOptionUI>(SystemValueHelper.GetUIElementBase(typeof(TipoProduto))),
                DomEvents = new List<DomEventUI>() { new DomEventUI() { DomEvent = "change", Function = "fnChangeTipoProduto" } }
            });
            config.Elements.Add(ElementUIHelper.GetAutoComplete(new AutoCompleteUI
            {
                Id = "grupoProdutoId",
                Class = "col s12 m3",
                Label = "Grupo",
                DataUrl = @Url.Action("GrupoProduto", "AutoComplete"),
                LabelId = "grupoProdutoDescricao",
                PreFilter = "tipoProduto",
            }, GrupoProdutoResourceHash));

            cfg.Content.Add(config);
            return cfg;
        }
    }


}
