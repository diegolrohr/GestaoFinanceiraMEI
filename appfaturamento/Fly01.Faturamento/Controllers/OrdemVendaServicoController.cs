using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Fly01.Faturamento.ViewModel;
using Fly01.Core;
using Fly01.uiJS.Classes;
using Fly01.uiJS.Classes.Elements;
using Newtonsoft.Json;
using Fly01.uiJS.Defaults;
using Fly01.Core.Presentation;
using Fly01.Core.ViewModels;
using Fly01.Core.Presentation.Commons;
using Fly01.uiJS.Classes.Helpers;
using Fly01.Core.Presentation.JQueryDataTable;

namespace Fly01.Faturamento.Controllers
{
    [OperationRole(ResourceKey = ResourceHashConst.FaturamentoFaturamentoVendas)]
    public class OrdemVendaServicoController : BaseController<OrdemVendaServicoVM>
    {
        public OrdemVendaServicoController()
        {
            ExpandProperties = "servico,grupoTributario";
        }

        public override Func<OrdemVendaServicoVM, object> GetDisplayData()
        {
            return x => new
            {
                id = x.Id.ToString(),
                servico_descricao = x.Servico.Descricao,
                grupoTributario_descricao = x.GrupoTributario != null ? x.GrupoTributario.Descricao : "",
                quantidade = x.Quantidade.ToString("N", AppDefaults.CultureInfoDefault),
                valor = x.Valor.ToString("C", AppDefaults.CultureInfoDefault),
                desconto = x.Desconto.ToString("C", AppDefaults.CultureInfoDefault),
                valorOutrasRetencoes = x.ValorOutrasRetencoes.ToString("C", AppDefaults.CultureInfoDefault),
                total = x.Total.ToString("C", AppDefaults.CultureInfoDefault),
            };
        }

        public ContentResult Modal()
        {
            ModalUIForm config = new ModalUIForm()
            {
                Title = "Serviço",
                UrlFunctions = @Url.Action("Functions") + "?fns=",
                ConfirmAction = new ModalUIAction() { Label = "Salvar" },
                CancelAction = new ModalUIAction() { Label = "Cancelar" },
                Action = new FormUIAction
                {
                    Create = @Url.Action("Create"),
                    Edit = @Url.Action("Edit"),
                    Get = @Url.Action("Json") + "/",
                    List = @Url.Action("List")
                },
                Id = "fly01mdlfrmOrdemVendaServico",
                ReadyFn = "fnFormReadyOrdemVendaServico",
                Functions = new List<string>() { "fnChangeTotalServico" }
            };

            config.Elements.Add(new InputHiddenUI { Id = "id" });
            config.Elements.Add(new InputHiddenUI { Id = "ordemVendaId" });

            config.Elements.Add(ElementUIHelper.GetAutoComplete(new AutoCompleteUI
            {
                Id = "servicoId",
                Class = "col s12 m6",
                Label = "Serviço",
                Required = true,
                DataUrl = Url.Action("Servico", "AutoComplete"),
                DataUrlPostModal = Url.Action("FormModal", "Servico"),
                DataPostField = "descricao",
                LabelId = "servicoDescricao",
                DomEvents = new List<DomEventUI> { new DomEventUI { DomEvent = "autocompleteselect", Function = "fnChangeServico" } }
            }, ResourceHashConst.FaturamentoCadastrosServicos));

            config.Elements.Add(ElementUIHelper.GetAutoComplete(new AutoCompleteUI
            {
                Id = "grupoTributarioIdServico",
                Class = "col s12 m6",
                Label = "Grupo Tributário",
                Name = "grupoTributarioId",
                DataUrl = Url.Action("GrupoTributario", "AutoComplete"),
                LabelId = "grupoTributarioDescricaoServico",
                LabelName = "grupoTributarioDescricao",
                DataUrlPostModal = Url.Action("FormModal", "GrupoTributario"),
                DataPostField = "descricao"
            }, ResourceHashConst.FaturamentoCadastrosGrupoTributario));

            config.Elements.Add(new InputFloatUI
            {
                Id = "quantidade",
                Class = "col s12 l6 numeric",
                Label = "Quantidade",
                Value = "1",
                Required = true
            });

            config.Elements.Add(new InputCurrencyUI
            {
                Id = "valor",
                Class = "col s12 l6 numeric",
                Label = "Valor",
                Required = true,
                Value = "0",
            });

            config.Elements.Add(new InputCurrencyUI
            {
                Id = "desconto",
                Class = "col s12 l6",
                Label = "Desconto",
                Value = "0",
            });

            config.Elements.Add(new InputCurrencyUI { Id = "total", Class = "col s12 l6", Label = "Total", Required = true, Disabled = true });

            config.Elements.Add(new InputTextUI
            {
                Id = "descricaoOutrasRetencoes",
                Class = "col s12 l6",
                Label = "Descrição Outras Retenções"
            });

            config.Elements.Add(new InputCurrencyUI
            {
                Id = "valorOutrasRetencoes",
                Class = "col s12 l6 numeric",
                Label = "Outras Retenções"
            });

            #region Helpers
            config.Helpers.Add(new TooltipUI
            {
                Id = "valorOutrasRetencoes",
                Tooltip = new HelperUITooltip()
                {
                    Text = "Se marcar Faturar, será descontado do valor total, junto as demais retenções, de acordo com as configurações do grupo tributário informado."
                }
            });

            config.Elements.Add(new InputCheckboxUI
            {
                Id = "isServicoPrioritario",
                Class = "col s12 m4",
                Label = "Serviço prioritário",
            });
            config.Helpers.Add(new TooltipUI
            {
                Id = "isServicoPrioritario",
                Tooltip = new HelperUITooltip()
                {
                    Text = "Só é permitido um serviço prioritário por pedido. Ao adicionar mais de 1 serviço e se o pedido gerar nota fiscal, os serviços serão agrupados em 1 único serviço para o XML de transmissão. Serão agrupados descrições. Valores e impostos serão somados; porém, código Nbs, código Iss e Código Tributário Municipal, será considerado do serviço marcado como prioritário."
                }
            });
            #endregion

            return Content(JsonConvert.SerializeObject(config, JsonSerializerSetting.Front), "application/json");
        }

        public override ContentResult List() { throw new NotImplementedException(); }

        [OperationRole(PermissionValue = EPermissionValue.Read)]
        public JsonResult GetOrdemVendaServicos(string id)
        {
            Dictionary<string, string> filters = new Dictionary<string, string>
            {
                { "ordemVendaId eq", string.IsNullOrEmpty(id) ? Guid.NewGuid().ToString() : id }
            };
            return GridLoad(filters);
        }

        protected override ContentUI FormJson()
        {
            throw new NotImplementedException();
        }

        protected override List<JQueryDataTableParamsColumn> GetParamsColumns()
        {
            throw new NotImplementedException();
        }
    }
}