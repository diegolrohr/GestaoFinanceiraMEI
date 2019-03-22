using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Fly01.Core;
using Fly01.Core.Config;
using Fly01.Core.Entities.Domains.Enum;
using Fly01.Core.Presentation;
using Fly01.Core.Presentation.Commons;
using Fly01.Core.Rest;
using Fly01.Core.ViewModels.Presentation.Commons;
using Fly01.Estoque.Models.Reports;
using Fly01.Estoque.Models.ViewModel;
using Fly01.uiJS.Classes;
using Fly01.uiJS.Classes.Elements;
using Fly01.uiJS.Classes.Helpers;

namespace Fly01.Estoque.Controllers
{
    [OperationRole(ResourceKey = ResourceHashConst.EstoqueEstoqueRelatorios)]
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

        public override List<HtmlUIButton> GetFormButtonsOnHeader()
        {
            var target = new List<HtmlUIButton>();

            target.Add(new HtmlUIButton { Id = "imprimirRelatorioId", Label = "Imprimir", OnClickFn = "fnImprimirRelatorioProduto" });

            return target;
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
                UrlFunctions = Url.Action("Functions") + "?fns=",
                //Functions = new List<string> {"fnImprimirRelatorioProduto"}
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
            config.Elements.Add(new InputTextUI { Id = "codigo", Class = "col s12 m2", Label = "Código"});
            config.Elements.Add(new SelectUI
            {
                Id = "tipoProduto",
                Class = "col s12 m3",
                Label = "Tipo",
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

            config.Elements.Add(new AutoCompleteUI
            {
                Id = "unidadeMedidaId",
                Class = "col s12 m6",
                Label = "Unidade de Medida",
                DataUrl = @Url.Action("UnidadeMedida", "AutoComplete"),
                LabelId = "unidadeMedidaDescricao"
            });
            config.Elements.Add(new AutoCompleteUI
            {
                Id = "ncmId",
                Class = "col s12 m6",
                Label = "NCM",
                DataUrl = @Url.Action("Ncm", "AutoComplete"),
                LabelId = "ncmDescricao",
            });
            config.Elements.Add(new AutoCompleteUI()
            {
                Id = "enquadramentoLegalIPIId",
                Class = "col s12 m6",
                Label = "Enquadramento Legal do IPI",
                DataUrl = @Url.Action("EnquadramentoLegalIPI", "AutoComplete"),
                LabelId = "enquadramentoLegalIPIDescricao"
            });
            config.Elements.Add(new SelectUI
            {
                Id = "origemMercadoria",
                Class = "col s12 m6",
                Label = "Origem Mercadoria",
                Options = new List<SelectOptionUI>(SystemValueHelper.GetUIElementBase(typeof(OrigemMercadoria)))
            });
            cfg.Content.Add(config);
            return cfg;
        }

        [HttpGet]
        public ActionResult Imprimir(string descricao, 
                                string codigo, 
                                string tipoProduto, 
                                string grupoProdutoId, 
                                string unidadeMedidaId, 
                                string ncmId, 
                                string enquadramentoLegalIPIId, 
                                string origemMercadoria)
        {
            var queryString = new Dictionary<string, string>
            {
                { "descricao", descricao},
                { "codigo", codigo},
                { "tipoProduto", tipoProduto},
                { "grupoProdutoId", grupoProdutoId},
                { "unidadeMedidaId", unidadeMedidaId},
                { "ncmId", ncmId},
                { "enquadramentoLegalIPIId", enquadramentoLegalIPIId},
                { "origemMercadoria", origemMercadoria},
            };

            var response = RestHelper.ExecuteGetRequest<ResultBase<RelatorioProdutoVM>>("relatorioProduto", queryString);

            var reportViewer = new WebReportViewer<RelatorioProdutoVM>(ReportProduto.Instance);
            return File(reportViewer.Print(response.Data, SessionManager.Current.UserData.PlatformUrl), "application/pdf");        
        }
    }
}
