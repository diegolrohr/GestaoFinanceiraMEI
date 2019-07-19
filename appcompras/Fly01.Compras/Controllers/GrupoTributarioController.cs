using Fly01.Core.ViewModels.Presentation.Commons;
using Fly01.Core.Presentation.Controllers;
using Fly01.Core.Presentation;
using System.Web.Mvc;
using Fly01.uiJS.Classes;
using Newtonsoft.Json;
using Fly01.Core.Defaults;
using Fly01.uiJS.Classes.Elements;

namespace Fly01.Compras.Controllers
{
    [OperationRole(ResourceKey = ResourceHashConst.ComprasCadastrosGrupoTributario)]
    public class GrupoTributarioController : GrupoTributarioBaseController<GrupoTributarioVM>
    {
        public ContentResult FormModal()
        {
            ModalUIForm config = new ModalUIForm()
            {
                Title = "Adicionar Grupo Tributário",
                ConfirmAction = new ModalUIAction() { Label = "Salvar" },
                CancelAction = new ModalUIAction() { Label = "Cancelar" },
                Action = new FormUIAction
                {
                    Create = @Url.Action("Create"),
                    Edit = @Url.Action("Edit"),
                    Get = @Url.Action("Json") + "/",
                },
                Id = "fly01mdlfrmModalGrupoTributario"
            };

            config.Elements.Add(new InputHiddenUI { Id = "id" });
            config.Elements.Add(new InputHiddenUI { Id = "calculaIcms", Value = "false" });
            config.Elements.Add(new InputHiddenUI { Id = "calculaIpi", Value = "false" });
            config.Elements.Add(new InputHiddenUI { Id = "calculaPis", Value = "false" });
            config.Elements.Add(new InputHiddenUI { Id = "calculaCofins", Value = "false" });
            config.Elements.Add(new InputHiddenUI { Id = "calculaIss", Value = "false" });
            config.Elements.Add(new InputHiddenUI { Id = "calculaCSLL", Value = "false" });
            config.Elements.Add(new InputHiddenUI { Id = "calculaINSS", Value = "false" });
            config.Elements.Add(new InputHiddenUI { Id = "calculaImpostoRenda", Value = "false" });
            config.Elements.Add(new InputHiddenUI { Id = "tipoTributacaoICMS", Value = "101" });
            config.Elements.Add(new InputHiddenUI { Id = "tipoTributacaoIPI", Value = "0" });
            config.Elements.Add(new InputHiddenUI { Id = "tipoTributacaoPIS", Value = "01" });
            config.Elements.Add(new InputHiddenUI { Id = "tipoTributacaoCOFINS", Value = "01" });
            config.Elements.Add(new InputHiddenUI { Id = "tipoTributacaoISS", Value = "1" });
            config.Elements.Add(new InputHiddenUI { Id = "tipoPagamentoImpostoISS", Value = "1" });
            config.Elements.Add(new InputHiddenUI { Id = "tipoCFPS", Value = "1" });

            config.Elements.Add(new InputTextUI { Id = "descricao", Class = "col s12 l12", Label = "Descrição", Required = true, MaxLength = 40 });

            config.Elements.Add(new AutoCompleteUI
            {
                Id = "cfopId",
                Class = "col s12 l12",
                Label = "Código Fiscal de Operação (CFOP)",
                Required = true,
                DataUrl = @Url.Action("Cfop", "AutoComplete"),
                LabelId = "cfopDescricaoModal"
            });

            return Content(JsonConvert.SerializeObject(config, JsonSerializerSetting.Front), "application/json");
        }
    }
}