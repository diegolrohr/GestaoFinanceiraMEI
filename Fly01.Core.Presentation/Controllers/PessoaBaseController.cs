using Fly01.Core.Entities.Domains.Enum;
using Fly01.Core.Helpers;
using Fly01.Core.Presentation.Commons;
using Fly01.Core.Presentation.JQueryDataTable;
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
    public class PessoaBaseController<T> : BaseController<T> where T : PessoaVM
    {
        protected virtual string ResourceTitle { get; set; }
        protected virtual string LabelTitle { get; set; }
        protected virtual string Filter { get; set; }

        public PessoaBaseController()
        {
            ExpandProperties = "estado($select=id,nome,sigla,codigoIbge),cidade($select=id,nome,estadoId,codigoIbge)";
        }

        public override Dictionary<string, string> GetQueryStringDefaultGridLoad()
        {
            var tempExpand = ExpandProperties;
            ExpandProperties = string.Empty;

            var customFilters = base.GetQueryStringDefaultGridLoad();
            ExpandProperties = tempExpand;

            customFilters.AddParam("$filter", Filter);
            customFilters.AddParam("$select", "id,nome,cpfcnpj,email,telefone,dataInclusao");

            return customFilters;
        }

        public override Func<T, object> GetDisplayData()
        {
            return x => new
            {
                id = x.Id,
                nome = x.Nome,
                cpfcnpj = FormatterUtils.FormatDocument(x.CPFCNPJ),
                contato = x.Contato,
                telefone = string.IsNullOrEmpty(x.Telefone)
                            ? ""
                            : Regex.Replace(x.Telefone, x.Telefone.Length == 10 ? @"(\d{2})(\d{4})(\d{4})" : @"(\d{2})(\d{4})(\d{5})", "($1) $2-$3")
            };
        }

        public override JsonResult Create(T entityVM)
        {
            NormarlizarEntidade(ref entityVM);
            return base.Create(entityVM);
        }

        [HttpPost]
        public override JsonResult Edit(T entityVM)
        {
            NormarlizarEntidade(ref entityVM);

            return base.Edit(entityVM);
        }

        protected virtual void NormarlizarEntidade(ref T entityVM)
        {
            const string regexSomenteDigitos = @"[^\d]";

            entityVM.CPFCNPJ = Regex.Replace(entityVM.CPFCNPJ ?? "", regexSomenteDigitos, "");
            entityVM.TipoDocumento = GetTipoDocumento(entityVM.CPFCNPJ ?? "");
            entityVM.Celular = Regex.Replace(entityVM.Celular ?? "", regexSomenteDigitos, "");
            entityVM.Telefone = Regex.Replace(entityVM.Telefone ?? "", regexSomenteDigitos, "");
            entityVM.CEP = Regex.Replace(entityVM.CEP ?? "", regexSomenteDigitos, "");
        }

        private string GetTipoDocumento(string documento)
        {
            if (documento.Length <= 11)
                return "F";
            if (documento.Length > 11)
                return "J";

            return null;
        }

        public override List<HtmlUIButton> GetListButtonsOnHeader()
        {
            var target = new List<HtmlUIButton>();

            target.Add(new HtmlUIButton { Id = "new", Label = "Novo", OnClickFn = "fnNovo", Position = HtmlUIButtonPosition.Main });
            target.Add(new HtmlUIButton { Id = "import", Label = $"Importar {LabelTitle}", OnClickFn = "fnImportarCadastro", Position = HtmlUIButtonPosition.Out });

            return target;
        }

        public override ContentResult List()
        {
            var cfg = new ContentUIBase(Url.Action("Sidebar", "Home"))
            {
                History = new ContentUIHistory { Default = Url.Action("Index") },
                Header = new HtmlUIHeader
                {
                    Title = LabelTitle,
                    Buttons = new List<HtmlUIButton>(GetListButtonsOnHeader())
                },
                UrlFunctions = Url.Action("Functions") + "?fns="
            };
            var config = new DataTableUI
            {
                Id = "fly01dt",
                UrlGridLoad = Url.Action("GridLoad"),
                UrlFunctions = Url.Action("Functions") + "?fns=",
                Options = new DataTableUIConfig
                {
                    OrderColumn = 0,
                    OrderDir = "asc"
                }
            };

            config.Actions.AddRange(GetActionsInGrid(new List<DataTableUIAction>()
            {
                new DataTableUIAction { OnClickFn = "fnEditar", Label = "Editar" },
                new DataTableUIAction { OnClickFn = "fnExcluir", Label = "Excluir" }
            }));

            config.Columns.Add(new DataTableUIColumn { DataField = "nome", DisplayName = ResourceTitle, Priority = 1 });
            config.Columns.Add(new DataTableUIColumn { DataField = "contato", DisplayName = "Contato", Priority = 2 });
            config.Columns.Add(new DataTableUIColumn { DataField = "telefone", DisplayName = "Telefone", Priority = 3, Type = "tel" });
            config.Columns.Add(new DataTableUIColumn { DataField = "cpfcnpj", DisplayName = "CPF / CNPJ", Priority = 4, Type = "cpfcnpj" });

            cfg.Content.Add(config);

            return Content(JsonConvert.SerializeObject(cfg, JsonSerializerSetting.Front), "application/json");
        }

        public override List<HtmlUIButton> GetFormButtonsOnHeader()
        {
            var target = new List<HtmlUIButton>();

            target.Add(new HtmlUIButton { Id = "cancel", Label = "Cancelar", OnClickFn = "fnCancelar", Position = HtmlUIButtonPosition.Out });
            target.Add(new HtmlUIButton { Id = "saveNew", Label = "Salvar e Novo", OnClickFn = "fnSalvar", Type = "submit", Position = HtmlUIButtonPosition.Out });
            target.Add(new HtmlUIButton { Id = "save", Label = "Salvar", OnClickFn = "fnSalvar", Type = "submit", Position = HtmlUIButtonPosition.Main });

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
                    Title = ResourceTitle,
                    Buttons = new List<HtmlUIButton>(GetFormButtonsOnHeader())
                },
                UrlFunctions = Url.Action("Functions") + "?fns="
            };

            var config = new FormUI
            {
                Id = "fly01frm",
                Action = new FormUIAction
                {
                    Create = Url.Action("Create"),
                    Edit = Url.Action("Edit"),
                    Get = Url.Action("Json") + "/",
                    List = Url.Action("List"),
                    Form = Url.Action("Form")
                },
                UrlFunctions = Url.Action("Functions") + "?fns=",
                ReadyFn = "fnFormReady"
            };

            config.Elements.Add(new InputHiddenUI { Id = "id" });
            config.Elements.Add(new InputHiddenUI { Id = "estadoCodigoIbge" });
            config.Elements.Add(new InputHiddenUI { Id = "cidadeCodigoIbge" });

            config.Elements.Add(new InputCpfcnpjUI { Id = "cpfcnpj", Class = "col s12 l3", Label = "CPF / CNPJ", MaxLength = 18 });
            config.Elements.Add(new InputTextUI { Id = "nome", Class = "col s12 l5", Label = "Razão Social / Nome Completo", Required = true, MaxLength = 180 });
            config.Elements.Add(new InputTextUI { Id = "nomeComercial", Class = "col s12 l4", Label = "Nome Comercial", MaxLength = 180 });

            config.Elements.Add(new InputTextUI { Id = "idEstrangeiro", Class = "col s12 l3", Label = "Identificação Estrangeiro", MaxLength = 20 });
            config.Elements.Add(new InputEmailUI { Id = "email", Class = "col s12 l5", Label = "E-mail", MaxLength = 100 });

            config.Elements.Add(new InputTextUI { Id = "contato", Class = "col s6 l4", Label = "Pessoa de Contato", MaxLength = 60 });
            config.Elements.Add(new InputTelUI { Id = "celular", Class = "col s6 l4", Label = "Celular", MaxLength = 15 });
            config.Elements.Add(new InputTelUI { Id = "telefone", Class = "col s6 l4", Label = "Telefone", MaxLength = 15 });

            config.Elements.Add(new InputCepUI
            {
                Id = "cep",
                Class = "col s6 l2",
                Label = "CEP",
                MaxLength = 9,
                DomEvents = new List<DomEventUI>() { new DomEventUI { DomEvent = "keyup", Function = "fnBuscaCEP" } }
            });

            config.Elements.Add(new AutoCompleteUI
            {
                Id = "paisId",
                Class = "col s12 l2",
                Label = "País",
                MaxLength = 35,
                DataUrl = Url.Action("Pais", "AutoComplete"),
                LabelId = "paisNome"
            });

            config.Elements.Add(new AutoCompleteUI
            {
                Id = "estadoId",
                Class = "col s12 l4",
                Label = "Estado",
                MaxLength = 35,
                DataUrl = Url.Action("Estado", "AutoComplete"),
                LabelId = "estadoNome",
                DomEvents = new List<DomEventUI>
                {
                    new DomEventUI() { DomEvent = "autocompleteselect", Function = "fnStateSelect" }
                }
            });

            config.Elements.Add(new AutoCompleteUI
            {
                Id = "cidadeId",
                Class = "col s12 l4",
                Label = "Cidade (Escolha o estado antes)",
                MaxLength = 35,
                DataUrl = Url.Action("Cidade", "AutoComplete"),
                LabelId = "cidadeNome",
                PreFilter = "estadoId",
                DomEvents = new List<DomEventUI>
                {
                    new DomEventUI() { DomEvent = "autocompleteselect", Function = "fnCitySelect" }
                }
            });

            config.Elements.Add(new InputTextUI { Id = "bairro", Class = "col s12 l3", Label = "Bairro", MaxLength = 200 });
            config.Elements.Add(new InputTextUI { Id = "endereco", Class = "col s12 l7", Label = "Endereço", MaxLength = 180 });
            config.Elements.Add(new InputTextUI { Id = "numero", Class = "col s12 l2", Label = "Número", MaxLength = 50 });
            config.Elements.Add(new TextAreaUI { Id = "complemento", Class = "col s12", Label = "Complemento", MaxLength = 500 });

            config.Elements.Add(new TextAreaUI { Id = "observacao", Class = "col s12", Label = "Observação", MaxLength = 500 });


            List<InputCheckboxUI> checkboxes = GetCheckBboxes();
            if (checkboxes != null)
                config.Elements.AddRange(checkboxes);

            List<TooltipUI> tooltips = GetHelpers();
            if (tooltips != null)
                config.Helpers.AddRange(tooltips);

            config.Helpers.Add(new TooltipUI
            {
                Id = "idEstrangeiro",
                Tooltip = new HelperUITooltip()
                {
                    Text = "Informe se for necessário para nota fiscal de exportação. Informar com o número do passaporte ou outro documento legal para identificar a pessoa estrangeira."
                }
            });

            config.Helpers.Add(new TooltipUI
            {
                Id = "paisNome",
                Tooltip = new HelperUITooltip()
                {
                    Text = "Informe se for necessário para nota fiscal de exportação."
                }
            });

            cfg.Content.Add(config);

            return cfg;
        }

        public virtual ActionResult ImportaCadastro()
        {
            return View();
        }

        public List<HtmlUIButton> GetFormImportacaoButtonsOnHeader()
        {
            var target = new List<HtmlUIButton>();

            target.Add(new HtmlUIButton() { Id = "cancel", Label = "Voltar", OnClickFn = "fnCancelar" });
            target.Add(new HtmlUIButton() { Id = "save", Label = "Importar", OnClickFn = "fnCarregarArquivo", Type = "submit" });

            return target;
        }

        public ContentResult FormImportacao()
        {
            var cfg = new ContentUIBase(Url.Action("Sidebar", "Home"))
            {
                History = new ContentUIHistory()
                {
                    Default = Url.Action("ImportaCadastro"),
                },
                Header = new HtmlUIHeader()
                {
                    Title = $"Importar {ResourceTitle}",
                    Buttons = new List<HtmlUIButton>(GetFormImportacaoButtonsOnHeader())
                },
                UrlFunctions = Url.Action("Functions") + "?fns="
            };

            var config = new FormUI
            {
                Id = "fly01frm",
                Action = new FormUIAction()
                {
                    Create = Url.Action("ImportaCadastro"),
                    Edit = Url.Action("ImportaCadastro"),
                    Get = Url.Action("Json") + "/ImportarCadastro",
                    List = @Url.Action("List")
                },
                ReadyFn = "fnImportaCadastroFormReady",
                UrlFunctions = Url.Action("Functions") + "?fns="
            };

            config.Elements.Add(new InputFileUI { Id = "arquivo", Class = "col s12", Label = "Arquivo de importação em lotes (.csv)", Required = true, Accept = ".csv" });

            config.Elements.Add(new TextAreaUI { Id = "observacao", Class = "col s12", Label = "Observação", Readonly = true });

            cfg.Content.Add(config);

            cfg.Content.Add(new CardUI()
            {
                Class = "col s12",
                Color = "blue",
                Id = "cardDuvidas",
                Title = "Dúvidas",
                Placeholder = "Se preferir você pode baixar um arquivo modelo de importação.",
                Action = new LinkUI()
                {
                    Label = "Baixar arquivo modelo"
                }

            });

            return Content(JsonConvert.SerializeObject(cfg, JsonSerializerSetting.Front), "application/json");
        }

        protected virtual List<TooltipUI> GetHelpers()
        {
            return null;
        }

        protected virtual List<InputCheckboxUI> GetCheckBboxes()
            => null;
    }
}