using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Fly01.Faturamento.Controllers.Base;
using Fly01.Faturamento.ViewModel;
using Fly01.Core;
using Fly01.uiJS.Classes;
using Fly01.uiJS.Classes.Elements;
using Newtonsoft.Json;
using Fly01.uiJS.Defaults;

namespace Fly01.Faturamento.Controllers
{
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
                grupoTributario_descricao = x.GrupoTributario.Descricao,
                quantidade = x.Quantidade.ToString("N", AppDefaults.CultureInfoDefault),
                valor = x.Valor.ToString("C", AppDefaults.CultureInfoDefault),
                desconto = x.Desconto.ToString("C", AppDefaults.CultureInfoDefault),
                total = x.Total.ToString("C", AppDefaults.CultureInfoDefault),
            };
        }

        public override ContentResult Form()
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

            config.Elements.Add(new AutocompleteUI
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
            });
            config.Elements.Add(new AutocompleteUI
            {
                Id = "grupoTributarioIdServico",
                Class = "col s12 m6",
                Label = "Grupo Tributário",
                Required = true,
                Name = "grupoTributarioId",
                DataUrl = Url.Action("GrupoTributario", "AutoComplete"),
                LabelId = "grupoTributarioDescricaoServico",
                LabelName = "grupoTributarioDescricao",
                DataUrlPostModal = Url.Action("FormModal", "GrupoTributario"),
                DataPostField = "descricao"
            });

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

        public override ContentResult List()
        {
            throw new NotImplementedException();
        }

        public JsonResult GetOrdemVendaServicos(string id)
        {
            Dictionary<string, string> filters = new Dictionary<string, string>
            {
                { "ordemVendaId eq", string.IsNullOrEmpty(id) ? new Guid().ToString() : id }
            };
            return GridLoad(filters);
        }
    }
}