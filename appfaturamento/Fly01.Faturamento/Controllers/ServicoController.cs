using Fly01.Core.Helpers;
using Fly01.uiJS.Classes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Fly01.uiJS.Classes.Elements;
using Fly01.uiJS.Defaults;
using Fly01.Core.Presentation;
using Fly01.uiJS.Enums;
using Fly01.uiJS.Classes.Helpers;
using Fly01.Core.ViewModels.Presentation.Commons;
using Fly01.Core.Presentation.JQueryDataTable;

namespace Fly01.Faturamento.Controllers
{
    [OperationRole(ResourceKey = ResourceHashConst.FaturamentoCadastrosServicos)]
    public class ServicoController : BaseController<ServicoVM>
    {
        protected Func<ServicoVM, object> GetDisplayDataSelect { get; set; }

        public ServicoController()
        {
            ExpandProperties = "nbs($select=id,descricao),iss,unidadeMedida";
            SelectPropertiesList = "id,codigoServico,descricao,registroFixo";

            GetDisplayDataSelect = x => new
            {
                id = x.Id,
                codigoServico = x.CodigoServico,
                descricao = x.Descricao,
                registroFixo = x.RegistroFixo
            };
        }

        public override Func<ServicoVM, object> GetDisplayData()
        {
            return GetDisplayDataSelect;
        }

        public override ContentResult List()
        {
            var cfg = new ContentUIBase(Url.Action("Sidebar", "Home"))
            {
                History = new ContentUIHistory { Default = Url.Action("Index") },
                Header = new HtmlUIHeader
                {
                    Title = "Serviços",
                    Buttons = new List<HtmlUIButton>(GetListButtonsOnHeader())
                },
                UrlFunctions = Url.Action("Functions") + "?fns="
            };
            var config = new DataTableUI { Id = "fly01dt", UrlGridLoad = Url.Action("GridLoad"), UrlFunctions = Url.Action("Functions") + "?fns=" };

            config.Actions.AddRange(GetActionsInGrid(new List<DataTableUIAction>()
            {
                new DataTableUIAction { OnClickFn = "fnEditar", Label = "Editar", ShowIf = "row.registroFixo == 0" },
                new DataTableUIAction { OnClickFn = "fnExcluir", Label = "Excluir", ShowIf = "row.registroFixo == 0" }
            }));

            config.Columns.Add(new DataTableUIColumn { DataField = "codigoServico", DisplayName = "Código", Priority = 1 });
            config.Columns.Add(new DataTableUIColumn { DataField = "descricao", DisplayName = "Descrição", Priority = 2 });

            cfg.Content.Add(config);

            return Content(JsonConvert.SerializeObject(cfg, JsonSerializerSetting.Default), "application/json");
        }

        public override List<HtmlUIButton> GetFormButtonsOnHeader()
        {
            var target = new List<HtmlUIButton>();

            if (UserCanWrite)
            {
                target.Add(new HtmlUIButton { Id = "cancel", Label = "Cancelar", OnClickFn = "fnCancelar", Position = HtmlUIButtonPosition.Out });
                target.Add(new HtmlUIButton { Id = "saveNew", Label = "Salvar e Novo", OnClickFn = "fnSalvar", Type = "submit", Position = HtmlUIButtonPosition.Out });
                target.Add(new HtmlUIButton { Id = "save", Label = "Salvar", OnClickFn = "fnSalvar", Type = "submit", Position = HtmlUIButtonPosition.Main });
            }

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
                    Title = "Dados do Serviço",
                    Buttons = new List<HtmlUIButton>(GetFormButtonsOnHeader())
                },
                UrlFunctions = Url.Action("Functions") + "?fns="
            };

            var config = new FormUI
            {
                Id = "fly01frm",
                Action = new FormUIAction
                {
                    Create = @Url.Action("Create"),
                    Edit = @Url.Action("Edit"),
                    Get = @Url.Action("Json") + "/",
                    List = Url.Action("List"),
                    Form = Url.Action("Form")
                },
                ReadyFn = "fnFormReady",
                UrlFunctions = Url.Action("Functions") + "?fns="
            };

            config.Elements.Add(new InputHiddenUI { Id = "id" });

            config.Elements.Add(new InputTextUI { Id = "codigoServico", Class = "col l3 m3 s12", Label = "Código", Required = true });
            config.Elements.Add(new InputTextUI { Id = "descricao", Class = "col l9 m9 s12", Label = "Descrição", Required = true });

            config.Elements.Add(new InputCurrencyUI { Id = "valorServico", Class = "col l4 m4 s12", Label = "Valor Servico", Required = true });

            config.Elements.Add(new InputTextUI { Id = "codigoTributacaoMunicipal", Class = "col l4 m4 s12", Label = "Código Tributação Municipal", MaxLength = 20 });

            config.Elements.Add(new InputTextUI { Id = "codigoIssEspecifico", Class = "col l4 m4 s12", Label = "Código Iss Específico", MaxLength = 20 });

            config.Elements.Add(new AutoCompleteUI
            {
                Id = "issId",
                Class = "col l12 m12 s12",
                Label = "ISS Tabela Padrão",
                DataUrl = @Url.Action("Iss", "AutoComplete"),
                LabelId = "issDescricao",
                DomEvents = new List<DomEventUI> { new DomEventUI { DomEvent = "autocompleteselect" } }
            });

            config.Elements.Add(new AutoCompleteUI
            {
                Id = "nbsId",
                Class = "col s12",
                Label = "NBS",
                DataUrl = @Url.Action("Nbs", "AutoComplete"),
                LabelId = "nbsDescricao",
                DomEvents = new List<DomEventUI> { new DomEventUI { DomEvent = "autocompleteselect" } }
            });

            config.Elements.Add(new AutoCompleteUI
            {
                Id = "unidadeMedidaId",
                Class = "col s12 m8",
                Label = "Unidade Medida",
                DataUrl = @Url.Action("UnidadeMedida", "AutoComplete"),
                LabelId = "unidadeMedidaDescricao",
            });

            config.Elements.Add(new InputTextUI { Id = "codigoFiscalPrestacao", Class = "col s12 m4", Label = "Código Fiscal de Prestação", MaxLength = 5 });

            config.Elements.Add(new TextAreaUI { Id = "observacao", Class = "col l12 m12 s12", Label = "Observação", MaxLength = 200 });

            #region Helpers
            config.Helpers.Add(new TooltipUI
            {
                Id = "codigoIssEspecifico",
                Tooltip = new HelperUITooltip()
                {
                    Text = "Se informado, este código será prioritário ao Código Iss da Tabela Padrão, irá sair no XML da nota fiscal, exatamente como informado neste campo."
                }
            });
            config.Helpers.Add(new TooltipUI
            {
                Id = "issId",
                Tooltip = new HelperUITooltip()
                {
                    Text = "Se necessário, configure no menu de Parâmetros Tributários, para formatar este Código Iss da tabela padrão, com pontuação ao gerar o XML, depende da configuração esperada pela prefeitura do município. Ex: 104 = 1.04, 2502 = 25.02"
                }
            });
            config.Helpers.Add(new TooltipUI
            {
                Id = "nbsId",
                Tooltip = new HelperUITooltip()
                {
                    Text = "Se informado NBS e o cliente do pedido for consumidor final e faturar este pedido, ao transmitir a nota fiscal, será gerada as informaçoes do IBPT."
                }
            });
            #endregion

            cfg.Content.Add(config);

            return cfg;
        }

        public ContentResult FormModal()
        {
            ModalUIForm config = new ModalUIForm()
            {
                Title = "Adicionar Serviço",
                ConfirmAction = new ModalUIAction() { Label = "Salvar" },
                CancelAction = new ModalUIAction() { Label = "Cancelar" },
                Action = new FormUIAction
                {
                    Create = @Url.Action("Create"),
                    Edit = @Url.Action("Edit"),
                    Get = @Url.Action("Json") + "/",
                },
                Id = "fly01mdlfrmProduto",
                UrlFunctions = Url.Action("Functions") + "?fns=",
            };

            config.Elements.Add(new InputHiddenUI { Id = "id" });

            config.Elements.Add(new InputTextUI { Id = "codigoServico", Class = "col l6 m6 s12", Label = "Código", Required = true });
            config.Elements.Add(new InputCurrencyUI { Id = "valorServico", Class = "col l6 m6 s12", Label = "Valor Servico", Required = true });
            config.Elements.Add(new InputTextUI { Id = "descricao", Class = "col l12 m12 s12", Label = "Descrição", Required = true });

            return Content(JsonConvert.SerializeObject(config, JsonSerializerSetting.Front), "application/json");
        }

        protected override List<JQueryDataTableParamsColumn> GetParamsColumns()
        {
            throw new NotImplementedException();
        }
    }
}