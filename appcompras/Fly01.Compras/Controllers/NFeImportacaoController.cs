using Fly01.Core.Defaults;
using Fly01.Core.Entities.Domains.Enum;
using Fly01.Core.Helpers;
using Fly01.Core.Presentation;
using Fly01.Core.Presentation.Commons;
using Fly01.Core.ViewModels.Presentation.Commons;
using Fly01.uiJS.Classes;
using Fly01.uiJS.Classes.Elements;
using Fly01.uiJS.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Fly01.Compras.Controllers
{
    //  [OperationRole(ResourceKey = ResourceHashConst.ComprasComprasNFeImportada)]
    public class NFeImportacaoController : BaseController<NFeImportacaoVM>
    {
        public override Func<NFeImportacaoVM, object> GetDisplayData()
        {
            return x => new
            {
                id = x.Id,
                fornecedor_nome = x.Fornecedor.Nome,
                data = x.DataVencimento.ToString("dd/MM/yyyy"),
                status = x.Status,
                statusDescription = EnumHelper.GetDescription(typeof(StatusNotaFiscal), x.Status),
                statusCssClass = EnumHelper.GetCSS(typeof(StatusNotaFiscal), x.Status),
                statusValue = EnumHelper.GetValue(typeof(StatusNotaFiscal), x.Status),
                valorTotal = x.ValorTota
            };
        }

        public override List<HtmlUIButton> GetListButtonsOnHeader()
        {
            var target = new List<HtmlUIButton>();

            //if (UserCanWrite)
            //{
            target.Add(new HtmlUIButton { Id = "new", Label = "Novo", OnClickFn = "fnNovo", Position = HtmlUIButtonPosition.Main });
            target.Add(new HtmlUIButton { Id = "import", Label = $"Importar XMl", OnClickFn = "fnImportarXML", Position = HtmlUIButtonPosition.Out });
            //}

            return target;
        }

        public override ContentResult List()
        {
            var cfg = new ContentUIBase(Url.Action("Sidebar", "Home"))
            {
                History = new ContentUIHistory { Default = Url.Action("Index") },
                Header = new HtmlUIHeader
                {
                    Title = "Importação de notas fiscais de entrada",
                    Buttons = new List<HtmlUIButton>(GetListButtonsOnHeader())
                },
                UrlFunctions = Url.Action("Functions") + "?fns="
            };

            var config = new DataTableUI
            {
                Id = "fly01dt",
                UrlGridLoad = Url.Action("GridLoad"),
                UrlFunctions = Url.Action("Functions") + "?fns=",
                Options = new DataTableUIConfig
                {
                    OrderColumn = 0,
                    OrderDir = "asc"
                }
            };

            config.Actions.AddRange(GetActionsInGrid(new List<DataTableUIAction>()
            {
                new DataTableUIAction { OnClickFn = "fnEditar", Label = "Editar", ShowIf = "row.registroFixo == 0" },
                new DataTableUIAction { OnClickFn = "fnEditar", Label = "Visualizar", ShowIf = "row.registroFixo == 0" },
                new DataTableUIAction { OnClickFn = "fnExcluir", Label = "Excluir", ShowIf = "row.registroFixo == 0" },
                new DataTableUIAction { OnClickFn = "fnEditar", Label = "Baixar XML", ShowIf = "row.registroFixo == 0" }
            }));

            config.Columns.Add(new DataTableUIColumn { DataField = "fornecedor_nome", DisplayName = "Fornecedor", Priority = 1 });
            config.Columns.Add(new DataTableUIColumn { DataField = "data", DisplayName = "Data", Priority = 2 });
            config.Columns.Add(new DataTableUIColumn { DataField = "status", DisplayName = "Status", Priority = 3, Type = "tel" });
            config.Columns.Add(new DataTableUIColumn { DataField = "valorTotal", DisplayName = "Valor Total", Priority = 3, Type = "tel" });

            cfg.Content.Add(config);

            return Content(JsonConvert.SerializeObject(cfg, JsonSerializerSetting.Front), "application/json");
        }

        protected override ContentUI FormJson()
        {
            throw new NotImplementedException();
        }

        public ContentResult FormImportacaoXML()
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
                    Title = "Importação XML",
                    Buttons = new List<HtmlUIButton>(GetFormButtonsOnHeader())
                },
                UrlFunctions = Url.Action("Functions") + "?fns="
            };

            var config = new FormWizardUI
            {
                Id = "fly01frm",
                Action = new FormUIAction
                {
                    Create = @Url.Action("Create"),
                    Edit = @Url.Action("Edit"),
                    Get = @Url.Action("Json") + "/",
                    List = @Url.Action("List")
                },
                UrlFunctions = Url.Action("Functions") + "?fns=",
                //ReadyFn = "fnFormReadyPedido",
                //Functions = new List<string> { "fnChangeEstado" },
                Steps = new List<FormWizardUIStep>()
                {
                    new FormWizardUIStep()
                    {
                        Title = "Importar arquivo",
                        Id = "stepImportarArquivo",
                        Quantity = 1,
                    },
                    new FormWizardUIStep()
                    {
                        Title = "Fornecedor",
                        Id = "stepFornecedor",
                        Quantity = 4,
                    },
                    new FormWizardUIStep()
                    {
                        Title = "Transportadora",
                        Id = "steptransportadora",
                        Quantity = 3,
                    },
                    new FormWizardUIStep()
                    {
                        Title = "Produtos Pendentes",
                        Id = "stepProdutosPendentes",
                        Quantity = 5,
                    },
                    new FormWizardUIStep()
                    {
                        Title = "Produtos",
                        Id = "stepProdutos",
                        Quantity = 11,
                    },
                    new FormWizardUIStep()
                    {
                        Title = "Resulmo/Finalizar",
                        Id = "stepResulmoFinalizar",
                        Quantity = 15,
                    }
                },
                ShowStepNumbers = true
            };

            #region stepImportação
            config.Elements.Add(new InputFileUI { Id = "arquivoXML", Class = "col s12", Label = "Arquivo de importação (.xml)", Required = false, Accept = ".xml" });
            #endregion

            #region step Fornecedor
            config.Elements.Add(new InputTextUI
            {
                Id = "fornecedor-match",
                Class = "col s12 m7",
                Label = "Fornecedor Importado",
                DomEvents = new List<DomEventUI>
                {
                    new DomEventUI { DomEvent = "change", Function = "" }
                }
            });

            config.Elements.Add(new InputCheckboxUI
            {
                Id = "atualizarFornecedor",
                Class = "col s12 m5",
                Label = "Atualizar dados do fornecedor encontrado",
                DomEvents = new List<DomEventUI>
                {
                    new DomEventUI { DomEvent = "change", Function = "" }
                }
            });

            config.Elements.Add(new InputCheckboxUI
            {
                Id = "vincularFornecedor",
                Class = "col s12 m5",
                Label = "Vincular com um existente",
                DomEvents = new List<DomEventUI>
                {
                    new DomEventUI { DomEvent = "change", Function = "" }
                }
            });

            config.Elements.Add(ElementUIHelper.GetAutoComplete(new AutoCompleteUI
            {
                Id = "fornecedorId",
                Class = "col s12 m8",
                Label = "Fornecedor",
                DataUrl = Url.Action("Fornecedor", "AutoComplete"),
                LabelId = "fornecedorNome",
                DataUrlPost = Url.Action("PostFornecedor")
            }, null ));
            #endregion

            cfg.Content.Add(config);
            return Content(JsonConvert.SerializeObject(cfg, JsonSerializerSetting.Front), "application/json");
        }
    }
}
