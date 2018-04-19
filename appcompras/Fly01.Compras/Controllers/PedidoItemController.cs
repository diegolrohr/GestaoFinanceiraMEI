using Fly01.Compras.Controllers.Base;
using Fly01.Compras.ViewModel;
using Fly01.uiJS.Classes;
using Fly01.uiJS.Classes.Elements;
using Fly01.uiJS.Defaults;
using Fly01.Core;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Fly01.Compras.Controllers
{
    public class PedidoItemController : BaseController<PedidoItemVM>
    {
        public PedidoItemController()
        {
            ExpandProperties = "produto";
        }

        public override Func<PedidoItemVM, object> GetDisplayData()
        {
            return x => new
            {
                id = x.Id.ToString(),
                produto_descricao = x.Produto.Descricao,
                quantidade = x.Quantidade.ToString("C", AppDefaults.CultureInfoDefault).Replace("R$", "").Replace("R$ ", ""),
                valor = x.Valor.ToString("C", AppDefaults.CultureInfoDefault),
                desconto = x.Desconto.ToString("C", AppDefaults.CultureInfoDefault),
                total = x.Total.ToString("C", AppDefaults.CultureInfoDefault),
            };
        }

        public override ContentResult Form()
        {
            ModalUIForm config = new ModalUIForm()
            {
                Title = "Adicionar Produto",
                UrlFunctions = @Url.Action("Functions", "PedidoItem", null, Request.Url.Scheme) + "?fns=",
                ConfirmAction = new ModalUIAction() { Label = "Salvar" },
                CancelAction = new ModalUIAction() { Label = "Cancelar" },
                Action = new FormUIAction
                {
                    Create = @Url.Action("Create"),
                    Edit = @Url.Action("Edit"),
                    Get = @Url.Action("Json") + "/",
                    List = @Url.Action("List")
                },
                Id = "fly01mdlfrmPedidoItem",
                ReadyFn = "fnFormReadyPedidoItem"
            };

            config.Elements.Add(new InputHiddenUI { Id = "id" });
            config.Elements.Add(new InputHiddenUI { Id = "pedidoId" });

            config.Elements.Add(new AutocompleteUI
            {
                Id = "produtoId",
                Class = "col s12",
                Label = "Produto",
                Required = true,
                DataUrl = Url.Action("Produto", "AutoComplete"),
                LabelId = "produtoDescricao",
                DataUrlPostModal = Url.Action("FormModal", "Produto"),
                DataPostField = "descricao",
                DomEvents = new List<DomEventUI> { new DomEventUI { DomEvent = "autocompleteselect", Function = "fnChangeProduto" } }
            });

            config.Elements.Add(new InputFloatUI
            {
                Id = "quantidade",
                Class = "col s12 l6 numeric",
                Label = "Quantidade",
                Value = "1",
                Required = true,
                DomEvents = new List<DomEventUI>()
                {
                    new DomEventUI() {DomEvent = "change", Function = "fnChangeTotal" }
                }
            });

            config.Elements.Add(new InputCurrencyUI
            {
                Id = "valor",
                Class = "col s12 l6 numeric",
                Label = "Valor",
                Required = true,
                DomEvents = new List<DomEventUI>()
                {
                    new DomEventUI() {DomEvent = "change", Function = "fnChangeTotal" }
                }
            });

            config.Elements.Add(new InputCurrencyUI
            {
                Id = "desconto",
                Class = "col s12 l6",
                Label = "Desconto",
                DomEvents = new List<DomEventUI>()
                {
                    new DomEventUI() {DomEvent = "change", Function = "fnChangeTotal" }
                }
            });

            config.Elements.Add(new InputCurrencyUI { Id = "total", Class = "col s12 l6", Label = "Total", Disabled = true, Required = true });


            return Content(JsonConvert.SerializeObject(config, JsonSerializerSetting.Front), "application/json");
        }

        public override ContentResult List()
        {
            throw new NotImplementedException();
        }

        public JsonResult GetPedidoItens(string id)
        {
            Dictionary<string, string> filters = new Dictionary<string, string>
            {
                { "pedidoId eq", string.IsNullOrEmpty(id) ? new Guid().ToString() : id }
            };
            return GridLoad(filters);
        }
    }
}