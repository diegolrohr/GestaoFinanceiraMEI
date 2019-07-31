using Fly01.Core.Entities.Domains.Enum;
using Fly01.Core.Helpers;
using Fly01.Core.Presentation.Commons;
using Fly01.Core.Presentation.JQueryDataTable;
using Fly01.Core.Rest;
using Fly01.Core.ViewModels;
using Fly01.Core.ViewModels.Presentation.Commons;
using Fly01.uiJS.Classes;
using Fly01.uiJS.Classes.Elements;
using Fly01.uiJS.Classes.Helpers;
using Fly01.uiJS.Defaults;
using Fly01.uiJS.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace Fly01.Core.Presentation.Controllers
{
    public class KitItemBaseController<T> : BaseController<T> where T : KitItemVM
    {
        public KitItemBaseController()
        {
            ExpandProperties = "produto($select=descricao,codigoProduto,valorVenda,valorCusto),servico($select=descricao,codigoServico,valorServico)";
        }

        public override Func<T, object> GetDisplayData()
        {
            return x => new
            {
                id = x.Id,
                produtoServicoCodigo = x.ProdutoServicoCodigo,
                produtoServicoDescricao = x.ProdutoServicoDescricao,
                quantidade = x.Quantidade.ToString("N", AppDefaults.CultureInfoDefault),
                tipoItem = x.TipoItem,
                tipoItemDescription = EnumHelper.GetDescription(typeof(TipoItem), x.TipoItem),
                tipoItemCssClass = EnumHelper.GetCSS(typeof(TipoItem), x.TipoItem),
                tipoItemValue = EnumHelper.GetValue(typeof(TipoItem), x.TipoItem),
                valorVenda = x.Produto?.ValorVenda.ToString("N", AppDefaults.CultureInfoDefault),
                valorCusto = x.Produto?.ValorCusto.ToString("N", AppDefaults.CultureInfoDefault),
                valorServico = x.Servico?.ValorServico.ToString("N", AppDefaults.CultureInfoDefault)
            };
        }

        public virtual JsonResult GridLoadKitItem(string id)
        {
            Dictionary<string, string> filters = new Dictionary<string, string>
            {
                { "kitId eq", string.IsNullOrEmpty(id) ? Guid.NewGuid().ToString() : id }
            };
            return GridLoad(filters);
        }

        public override ContentResult List()
        {
            throw new NotImplementedException();
        }
        
        protected override ContentUI FormJson()
        {
            throw new NotImplementedException();
        }

        [OperationRole(PermissionValue = EPermissionValue.Write)]
        [HttpPost]
        public override JsonResult Edit(T entityVM)
        {
            try
            {
                var resourceNamePut = $"{ResourceName}/{entityVM.Id}";

                //Does not support untyped value in non-open type.
                dynamic KitItem = new
                {
                    id = entityVM.Id,
                    kitId = entityVM.KitId,
                    produtoId = entityVM.ProdutoId,
                    servicoId = entityVM.ServicoId,
                    quantidade = entityVM.Quantidade,
                    tipoItem = entityVM.TipoItem
                };

                RestHelper.ExecutePutRequest(resourceNamePut, JsonConvert.SerializeObject(KitItem, JsonSerializerSetting.Default));
                
                return JsonResponseStatus.Get(new ErrorInfo { HasError = false }, Operation.Edit);
            }
            catch (Exception ex)
            {
                var error = JsonConvert.DeserializeObject<ErrorInfo>(ex.Message);
                return JsonResponseStatus.GetFailure(error.Message);
            }
        }

        public ContentResult Modal()
        {
            ModalUIForm config = new ModalUIForm()
            {
                Title = "Produto/Serviço",
                UrlFunctions = @Url.Action("Functions") + "?fns=",
                ConfirmAction = new ModalUIAction() { Label = "Salvar" },
                CancelAction = new ModalUIAction() { Label = "Cancelar" },
                Action = new FormUIAction
                {
                    Create = @Url.Action("Create"),
                    Edit = @Url.Action("Edit"),
                    Get = @Url.Action("Json") + "/"
                },
                Id = "fly01mdlfrmKitItens",
                ReadyFn = "fnFormReadyModalKitItens"
            };

            config.Elements.Add(new InputHiddenUI { Id = "id" });
            config.Elements.Add(new InputHiddenUI { Id = "kitId" });
            config.Elements.Add(new InputHiddenUI { Id = "tipoItem" });
            config.Elements.Add(new InputHiddenUI { Id = "produtoId" });
            config.Elements.Add(new InputHiddenUI { Id = "servicoId" });

            config.Elements.Add(new InputTextUI { Id = "produtoServicoCodigo", Class = "col s12 m2", Label = "Código", Disabled = true });
            config.Elements.Add(new InputTextUI { Id = "produtoServicoDescricao", Class = "col s12 m6", Label = "Descrição", Disabled = true });
            config.Elements.Add(new InputTextUI { Id = "tipoItemDescricao", Class = "col s12 m2", Label = "Tipo", Disabled = true });

            config.Elements.Add(new InputFloatUI
            {
                Id = "quantidade",
                Class = "col s12 m2",
                Label = "Quantidade",
                Value = "1",
                Required = true
            });

            return Content(JsonConvert.SerializeObject(config, JsonSerializerSetting.Front), "application/json");
        }

        protected override List<JQueryDataTableParamsColumn> GetParamsColumns()
        {
            throw new NotImplementedException();
        }
    }
}