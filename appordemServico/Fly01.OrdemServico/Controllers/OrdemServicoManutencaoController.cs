
using Fly01.Core;
using Fly01.Core.Presentation;
using Fly01.Core.Presentation.Commons;
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
    public class OrdemServicoManutencaoController : BaseController<OrdemServicoManutencaoVM>
    {
        public OrdemServicoManutencaoController()
        {
            ExpandProperties = "produto";
        }

        public override Func<OrdemServicoManutencaoVM, object> GetDisplayData()
        {
            return x => new
            {
                id = x.Id.ToString(),
                produto_descricao = x.Produto.Descricao,
                quantidade = x.Quantidade.ToString("R", AppDefaults.CultureInfoDefault)
            };
        }

        public ContentResult Modal()
        {
            ModalUIForm config = new ModalUIForm()
            {
                Title = "Objeto de Manutenção",
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
                Id = "fly01mdlfrmOrdemServicoManutencao",
                ReadyFn = "fnFormReadyOrdemServicoManutencao",
                Functions = new List<string>() { "fnChangeTotalProduto" }
            };

            config.Elements.Add(new InputHiddenUI { Id = "id" });
            config.Elements.Add(new InputHiddenUI { Id = "ordemServicoId" });

            config.Elements.Add(ElementUIHelper.GetAutoComplete(new AutoCompleteUI
            {
                Id = "produtoId",
                Class = "col s12 m6",
                Label = "Produto do cliente",
                Required = true,
                DataUrl = Url.Action("ItemManutencao", "AutoComplete"),
                DataUrlPostModal = Url.Action("FormModal", "Produto"),
                DataPostField = "descricao",
                LabelId = "produtoCliente",
                DomEvents = new List<DomEventUI> { new DomEventUI { DomEvent = "autocompleteselect", Function = "fnChangeManutencao" } }
            }, ResourceHashConst.FaturamentoCadastrosProdutos));

            config.Elements.Add(new InputFloatUI
            {
                Id = "quantidade",
                Class = "col s12 l6 numeric",
                Label = "Quantidade",
                Digits = 3,
                Value = "1",
                Required = false
            });

            return Content(JsonConvert.SerializeObject(config, JsonSerializerSetting.Front), "application/json");
        }

        public override ContentResult List() { throw new NotImplementedException(); }

        [OperationRole(PermissionValue = EPermissionValue.Read)]
        public JsonResult GetOrdemServicoManutencao(string id)
        {
            Dictionary<string, string> filters = new Dictionary<string, string>
            {
                { "ordemServicoId eq", string.IsNullOrEmpty(id) ? new Guid().ToString() : id }
            };
            return GridLoad(filters);
        }

        protected override ContentUI FormJson()
        {
            throw new NotImplementedException();
        }
    }
}