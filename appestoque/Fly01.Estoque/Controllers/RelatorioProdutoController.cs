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
using System.Linq;
using Fly01.Core.Helpers;

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
            ConfiguracaoPersonalizacaoVM personalizacao = null;
            try
            {
                personalizacao = RestHelper.ExecuteGetRequest<ResultBase<ConfiguracaoPersonalizacaoVM>>("ConfiguracaoPersonalizacao", queryString: null)?.Data?.FirstOrDefault();
            }
            catch (Exception)
            {

            }
            var emiteNotaFiscal = personalizacao != null ? personalizacao.EmiteNotaFiscal : true;

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
                },
                UrlFunctions = Url.Action("Functions") + "?fns=",
                ReadyFn = "fnFormReady"
            };

            config.Elements.Add(new InputTextUI { Id = "descricaoid", Class = "col s12 m4", Label = "Descrição", MaxLength = 200 });
            config.Elements.Add(new InputTextUI { Id = "codigo", Class = "col s12 m2", Label = "Código"});
            config.Elements.Add(new SelectUI
            {
                Id = "tipoProduto",
                Class = "col s12 m3",
                Label = "Tipo",
                Required = true,
                Options = new List<SelectOptionUI>(SystemValueHelper.GetUIElementBase(typeof(TipoProduto)).ToList().FindAll(x => "ProdutoFinal,Insumo,Outros".Contains(x.Value)).OrderByDescending(x => x.Label))                
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
            if (emiteNotaFiscal)
            {
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
            }
            else
            {
                config.Elements.Add(new InputHiddenUI() { Id = "ncmId" });
                config.Elements.Add(new InputHiddenUI() { Id = "enquadramentoLegalIPIId" });
                config.Elements.Add(new InputHiddenUI() { Id = "origemMercadoria" });
            }

            config.Elements.Add(new LabelSetUI() { Id = "labelSetImprimirColunas", Label = "Configurar impressão das colunas"});
            config.Elements.Add(new InputCheckboxUI { Id = "imprimirQuantidade", Class = "col s12 m6 l4", Label = "Imprimir quantidade estoque" });
            config.Elements.Add(new InputCheckboxUI { Id = "imprimirValorCusto", Class = "col s12 m6 l4", Label = "Imprimir valor de custo" });
            config.Elements.Add(new InputCheckboxUI { Id = "imprimirValorVenda", Class = "col s12 m6 l4", Label = "Imprimir valor de venda" });

            if (emiteNotaFiscal)
            {
                config.Elements.Add(new InputCheckboxUI { Id = "imprimirNCM", Class = "col s12 m6 l4", Label = "Imprimir NCM" });
                config.Elements.Add(new InputCheckboxUI { Id = "imprimirEnquadramentoIPI", Class = "col s12 m6 l4", Label = "Imprimir Enquadramento legal IPI" });
                config.Elements.Add(new InputCheckboxUI { Id = "imprimirOrigemMercadoria", Class = "col s12 m6 l4", Label = "Imprimir origem mercadoria" });
            }
            else
            {
                config.Elements.Add(new InputHiddenUI() { Id = "imprimirNCM", Value = "false" });
                config.Elements.Add(new InputHiddenUI() { Id = "imprimirEnquadramentoIPI", Value = "false" });
                config.Elements.Add(new InputHiddenUI() { Id = "imprimirOrigemMercadoria", Value = "false" });
            }

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
                                string origemMercadoria,
                                string imprimirQuantidade,
                                string imprimirValorCusto,
                                string imprimirValorVenda,
                                string imprimirNCM,
                                string imprimirEnquadramentoIPI,
                                string imprimirOrigemMercadoria)
        {
            var queryString = new Dictionary<string, string>
            {
                { "descricao", descricao},
                { "codigo", codigo},
                { "tipoProduto", tipoProduto},
                { "origemMercadoria", origemMercadoria},
                { "imprimirQuantidade", imprimirQuantidade},
                { "imprimirValorCusto", imprimirValorCusto},
                { "imprimirValorVenda", imprimirValorVenda},
                { "imprimirNCM", imprimirNCM},
                { "imprimirEnquadramentoIPI", imprimirEnquadramentoIPI},
                { "imprimirOrigemMercadoria", imprimirOrigemMercadoria},
                { "grupoProdutoId", grupoProdutoId},
                { "unidadeMedidaId", unidadeMedidaId},
                { "ncmId", ncmId},
                { "enquadramentoLegalIPIId", enquadramentoLegalIPIId}
            };

            var response = RestHelper.ExecuteGetRequest<ResultBase<RelatorioProdutoVM>>("relatorioProduto", queryString);

            var reportViewer = new WebReportViewer<RelatorioProdutoVM>(ReportProduto.Instance);
            return File(reportViewer.Print(response.Data, SessionManager.Current.UserData.PlatformUrl), "application/pdf");        
        }
    }
}
