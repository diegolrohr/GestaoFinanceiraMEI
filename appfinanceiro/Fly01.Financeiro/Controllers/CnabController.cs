using Fly01.Core.Entities.Domains.Commons;
using Fly01.Core.Entities.Domains.Enum;
using Fly01.Core.Helpers;
using Fly01.Core.Presentation.Commons;
using Fly01.Core.ViewModels.Presentation.Commons;
using Fly01.Financeiro.Controllers.Base;
using Fly01.Financeiro.ViewModel;
using Fly01.uiJS.Classes;
using Fly01.uiJS.Classes.Elements;
using Fly01.uiJS.Defaults;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Fly01.Financeiro.Controllers
{
    public class CnabController : BaseController<CnabVM>
    {
        public override Func<CnabVM, object> GetDisplayData()
        {
            return x => new
            {
                id = x.Id,
                numeroBoleto = x.NumeroBoleto,
                banco = x.BancoCedente.Nome,
                valorBoleto = x.ValorBoleto,
                sacado = x.Pessoa.Nome,
                status = EnumHelper.SubtitleDataAnotation(typeof(StatusCnab), x.Status).Value,
                dataEmissao = x.DataEmissao,
                dataVencimento = x.DataVencimento
            };
        }

        public override ContentResult Form()
        {
            var cfg = new ContentUI
            {
                History = new ContentUIHistory
                {
                    Default = Url.Action("Create", "Cnab"),
                    WithParams = Url.Action("Edit", "Cnab"),
                },
                Header = new HtmlUIHeader
                {
                    Title = "Dados para emissão de boleto",
                    Buttons = new List<HtmlUIButton>
                    {
                        new HtmlUIButton { Id = "cancel", Label = "Cancelar", OnClickFn = "fnCancelar" },
                        new HtmlUIButton { Id = "save", Label = "Salvar", OnClickFn = "fnSalvar", Type = "submit" }
                    }
                },
                UrlFunctions = Url.Action("Functions") + "?fns="
            };

            var config = new FormUI
            {
                Action = new FormUIAction
                {
                    Create = @Url.Action("Create"),
                    Edit = @Url.Action("Edit"),
                    Get = @Url.Action("Json") + "/",
                    List = @Url.Action("List")
                },
                UrlFunctions = Url.Action("Functions") + "?fns="
            };

            config.Elements.Add(new InputHiddenUI { Id = "id" });
            config.Elements.Add(new AutocompleteUI
            {
                Id = "bancoId",
                Class = "col s12 m12 12",
                Label = "Banco",
                Required = true,
                DataUrl = @Url.Action("Banco", "AutoComplete"),
                LabelId = "bancoNome"
            });
            config.Elements.Add(new InputTextUI { Id = "cpfcnpj", Class = "col s8 l8", Label = "CPF/CNPJ", Required = true });
            config.Elements.Add(new InputTextUI { Id = "valor", Class = "col s8 l8", Label = "Valor boleto", Required = true });

            config.Elements.Add(new LabelsetUI { Id = "simulatorLabel", Class = "col s12", Label = "Gerar boleto a partir de conta existente" });

            config.Elements.Add(new AutocompleteUI
            {
                Id = "contaReceberId",
                Class = "col s12 m12 12",
                Label = "Banco",
                Required = true,
                DataUrl = @Url.Action("ContaReceber", "AutoComplete"),
                LabelId = "conta"
            });

            cfg.Content.Add(config);

            return Content(JsonConvert.SerializeObject(cfg, JsonSerializerSetting.Front), "application/json");
        }

        public override ContentResult List()
        {
            var cfg = new ContentUI
            {
                History = new ContentUIHistory() { Default = Url.Action("Index", "Cnab") },
                Header = new HtmlUIHeader()
                {
                    Title = "Boletos",
                    Buttons = new List<HtmlUIButton>
                    {
                        new HtmlUIButton { Id = "new", Label = "Novo", OnClickFn = "fnNovo" }
                    }
                },
                UrlFunctions = Url.Action("Functions") + "?fns="
            };

            var config = new DataTableUI() { UrlGridLoad = Url.Action("GridLoad"), UrlFunctions = Url.Action("Functions") + "?fns=" };

            config.Actions.Add(new DataTableUIAction { OnClickFn = "fnEditar", Label = "Editar" });
            config.Actions.Add(new DataTableUIAction { OnClickFn = "fnExcluir", Label = "Excluir" });
            config.Columns.Add(new DataTableUIColumn
            {
                DataField = "status",
                DisplayName = "Status",
                Priority = 1,
                Options = new List<SelectOptionUI>(SystemValueHelper.GetUIElementBase(typeof(StatusCnab)))
            });

            config.Columns.Add(new DataTableUIColumn { DataField = "numeroBoleto", DisplayName = "Nº Boleto", Priority = 2 });
            config.Columns.Add(new DataTableUIColumn { DataField = "banco", DisplayName = "Banco", Priority = 3 });
            config.Columns.Add(new DataTableUIColumn { DataField = "sacado", DisplayName = "Sacado", Priority = 4 });
            config.Columns.Add(new DataTableUIColumn { DataField = "valorBoleto", DisplayName = "Valor", Priority = 5 });
            config.Columns.Add(new DataTableUIColumn { DataField = "dataEmissao", DisplayName = "Dt. Emissão", Priority = 6 });
            config.Columns.Add(new DataTableUIColumn { DataField = "dataVencimento", DisplayName = "Dt. Vencimento", Priority = 7 });

            cfg.Content.Add(config);
            return Content(JsonConvert.SerializeObject(cfg, JsonSerializerSetting.Front), "application/json");
        }
    }
}