using Fly01.Compras.ViewModel;
using Fly01.Core;
using Fly01.uiJS.Classes;
using Fly01.uiJS.Classes.Elements;
using Fly01.uiJS.Defaults;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Fly01.Core.Presentation;
using Fly01.Core.Presentation.Commons;
using Fly01.Core.Presentation.JQueryDataTable;

namespace Fly01.Compras.Controllers
{
    [OperationRole(ResourceKey = ResourceHashConst.ComprasComprasOrcamentoPedido)]
    public class OrcamentoItemController : BaseController<OrcamentoItemVM>
    {
        public OrcamentoItemController()
        {
            ExpandProperties = "produto($select=id,descricao),fornecedor($select=id,nome)";
        }

        public override Func<OrcamentoItemVM, object> GetDisplayData()
        {
            return x => new
            {
                id = x.Id.ToString(),
                fornecedor_nome = x.Fornecedor.Nome,
                produto_descricao = x.Produto.Descricao,
                quantidade = x.Quantidade.ToString("R", AppDefaults.CultureInfoDefault),
                valor = x.Valor.ToString("C", AppDefaults.CultureInfoDefault),
                desconto = x.Desconto.ToString("C", AppDefaults.CultureInfoDefault),
                total = x.Total.ToString("C", AppDefaults.CultureInfoDefault),
            };
        }

        public ContentResult Modal()
        {
            ModalUIForm config = new ModalUIForm()
            {
                Title = "Adicionar Produto",
                UrlFunctions = @Url.Action("Functions", "OrcamentoItem", null, Request.Url.Scheme) + "?fns=",
                ConfirmAction = new ModalUIAction() { Label = "Salvar" },
                CancelAction = new ModalUIAction() { Label = "Cancelar" },
                Action = new FormUIAction
                {
                    Create = @Url.Action("Create"),
                    Edit = @Url.Action("Edit"),
                    Get = @Url.Action("Json") + "/",
                    List = @Url.Action("List")
                },
                ReadyFn = "fnFormReadyOrcamentoItem",
                Id = "fly01mdlfrmOrcamentoItem"
            };

            config.Elements.Add(new InputHiddenUI { Id = "id" });
            config.Elements.Add(new InputHiddenUI { Id = "orcamentoId" });
            config.Elements.Add(new InputHiddenUI { Id = "fornecedorNome" });

            config.Elements.Add(ElementUIHelper.GetAutoComplete(new AutoCompleteUI {
                Id = "fornecedorId",
                Class = "col s12 l6",
                Label = "Fornecedor",
                Required = true,
                DataUrl = Url.Action("Fornecedor", "AutoComplete"),
                LabelId = "itemFornecedorNome",
                DataUrlPost = Url.Action("PostFornecedor", "Orcamento"),
                DomEvents = new List<DomEventUI> { new DomEventUI { DomEvent = "autocompleteselect", Function = "fnChangeNomeFornecedor" } }
            }, ResourceHashConst.ComprasCadastrosFornecedores));

            config.Elements.Add(ElementUIHelper.GetAutoComplete(new AutoCompleteUI {
                Id = "produtoId",
                Class = "col s12 l6",
                Label = "Produto",
                Required = true,
                DataUrl = Url.Action("Produto", "AutoComplete"),
                LabelId = "produtoDescricao",
                DataUrlPostModal = Url.Action("FormModal", "Produto"),
                DataPostField = "descricao",
                DomEvents = new List<DomEventUI> { new DomEventUI { DomEvent = "autocompleteselect", Function = "fnChangeProduto" } }
            }, ResourceHashConst.ComprasCadastrosProdutos));

            config.Elements.Add(new InputFloatUI
            {
                Id = "quantidade",
                Class = "col s12 l6 numeric",
                Label = "Quantidade",
                Value = "1",
                Required = true,
                Digits = 3,
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

            config.Elements.Add(new InputCurrencyUI { Id = "total", Class = "col s12 l6", Label = "Total", Value = "0", Disabled = true, Required = true });

            return Content(JsonConvert.SerializeObject(config, JsonSerializerSetting.Front), "application/json");
        }

        public override ContentResult List()
        {
            throw new NotImplementedException();
        }

        [OperationRole(NotApply = true)]
        public JsonResult GetOrcamentoItens(string id)
        {
            Dictionary<string, string> filters = new Dictionary<string, string>
            {
                { "orcamentoId eq", string.IsNullOrWhiteSpace(id) ? Guid.NewGuid().ToString() : id }
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