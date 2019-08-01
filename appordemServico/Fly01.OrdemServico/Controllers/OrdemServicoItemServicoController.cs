using Fly01.Core;
using Fly01.Core.Presentation;
using Fly01.Core.Presentation.Commons;
using Fly01.Core.Presentation.JQueryDataTable;
using Fly01.Core.ViewModels;
using Fly01.OrdemServico.ViewModel;
using Fly01.uiJS.Classes;
using Fly01.uiJS.Classes.Elements;
using Fly01.uiJS.Defaults;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Fly01.OrdemServico.Controllers
{
    [OperationRole(ResourceKey = ResourceHashConst.OrdemServico)]
    public class OrdemServicoItemServicoController : BaseController<OrdemServicoItemServicoVM>
    {
        public OrdemServicoItemServicoController()
        {
            ExpandProperties = "servico";
        }

        public override Func<OrdemServicoItemServicoVM, object> GetDisplayData()
        {
            return x => new
            {
                id = x.Id.ToString(),
                servico_descricao = x.Servico.Descricao,
                quantidade = x.Quantidade.ToString("N", AppDefaults.CultureInfoDefault),
                valor = x.Valor.ToString("C", AppDefaults.CultureInfoDefault),
                desconto = x.Desconto.ToString("C", AppDefaults.CultureInfoDefault),
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
                Id = "fly01mdlfrmOrdemServicoItemServico",
                ReadyFn = "fnFormReadyOrdemServicoItemServico",
                Functions = new List<string>() { "fnChangeTotalServico" }
            };

            config.Elements.Add(new InputHiddenUI { Id = "id" });
            config.Elements.Add(new InputHiddenUI { Id = "ordemServicoId" });

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
            }, ResourceHashConst.OrdemServicoCadastrosServicos));


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

            return Content(JsonConvert.SerializeObject(config, JsonSerializerSetting.Front), "application/json");
        }

        public override ContentResult List() { throw new NotImplementedException(); }

        [OperationRole(PermissionValue = EPermissionValue.Read)]
        public JsonResult GetOrdemServicoItemServicos(string id)
        {
            Dictionary<string, string> filters = new Dictionary<string, string>
            {
                { "ordemServicoId eq", string.IsNullOrEmpty(id) ? Guid.NewGuid().ToString() : id }
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