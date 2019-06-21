using Fly01.Core.Entities.Domains.Enum;
using Fly01.Core.Helpers;
using Fly01.Core.Presentation.Commons;
using Fly01.Core.Rest;
using Fly01.Core.ViewModels.Presentation.Commons;
using Fly01.EmissaoNFE.Domain.Enums;
using Fly01.uiJS.Classes;
using Fly01.uiJS.Classes.Elements;
using Fly01.uiJS.Classes.Helpers;
using Fly01.uiJS.Defaults;
using Fly01.uiJS.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Fly01.Core.Presentation.Controllers
{
    public class ProdutoBaseController<T> : BaseController<T> where T : ProdutoVM
    {
        protected Func<T, object> GetDisplayDataSelect { get; set; }
        protected string SelectProperties { get; set; }
        private string GrupoProdutoResourceHash { get; set; }

        public ProdutoBaseController(string grupoProdutoResourceHash)
        {
            GrupoProdutoResourceHash = grupoProdutoResourceHash;
            ExpandProperties = "grupoProduto($select=id,descricao),unidadeMedida($select=id,descricao),ncm($select=id,descricao),cest($select=id,descricao,codigo),enquadramentoLegalIPI($select=id,codigo,grupoCST,descricao)";
            SelectProperties = "id,codigoProduto,descricao,grupoProdutoId,tipoProduto,registroFixo";
            GetDisplayDataSelect = x => new
            {
                id = x.Id,
                codigoProduto = x.CodigoProduto,
                codigoBarras = x.CodigoBarras,
                descricao = x.Descricao.Substring(0, x.Descricao.Length <= 60 ? x.Descricao.Length : 60),
                grupoProdutoId = x.GrupoProdutoId,
                grupoProduto_descricao = x.GrupoProduto != null ? x.GrupoProduto.Descricao : "",
                tipoProduto = EnumHelper.GetValue(typeof(TipoProduto), x.TipoProduto),
                tipoProdutoCSS = EnumHelper.GetCSS(typeof(TipoProduto), x.TipoProduto),
                tipoProdutoDescricao = EnumHelper.GetDescription(typeof(TipoProduto), x.TipoProduto),
                registroFixo = x.RegistroFixo
            };
        }

        public override Func<T, object> GetDisplayData()
            => GetDisplayDataSelect;

        public override Dictionary<string, string> GetQueryStringDefaultGridLoad()
        {
            var customFilters = base.GetQueryStringDefaultGridLoad();
            customFilters.AddParam("$expand", ExpandProperties);
            customFilters.AddParam("$select", SelectProperties);
            customFilters.AddParam("$filter", $"objetoDeManutencao eq {AppDefaults.APIEnumResourceName}ObjetoDeManutencao'Nao'");

            return customFilters;
        }

        public override List<HtmlUIButton> GetFormButtonsOnHeader()
        {
            var target = new List<HtmlUIButton>();

            if (UserCanWrite)
            {
                target.Add(new HtmlUIButton { Id = "cancel", Label = "Cancelar", OnClickFn = "fnCancelar", Position = HtmlUIButtonPosition.Out });
                target.Add(new HtmlUIButton { Id = "saveNew", Label = "Salvar e Novo", OnClickFn = "fnSalvar", Type = "submit", Position = HtmlUIButtonPosition.Out });
                target.Add(new HtmlUIButton { Id = "save", Label = "Salvar", OnClickFn = "fnSalvar", Type = "submit", Position = HtmlUIButtonPosition.Main });
            }

            return target;
        }

        public override List<HtmlUIButton> GetListButtonsOnHeader()
        {
            var target = new List<HtmlUIButton>();

            if (UserCanWrite)
            {
                target.Add(new HtmlUIButton { Id = "new", Label = "Novo", OnClickFn = "fnNovo", Position = HtmlUIButtonPosition.Main });
                target.Add(new HtmlUIButton { Id = "import", Label = "Importar Produtos", OnClickFn = "fnImportarCadastro", Position = HtmlUIButtonPosition.Out });
            }

            return target;
        }

        public override ContentResult List()
        {
            var cfg = new ContentUIBase(Url.Action("Sidebar", "Home"))
            {
                History = new ContentUIHistory { Default = Url.Action("Index") },
                Header = new HtmlUIHeader
                {
                    Title = "Produtos",
                    Buttons = new List<HtmlUIButton>(GetListButtonsOnHeader())
                },
                UrlFunctions = Url.Action("Functions") + "?fns=",
                Functions = new List<string>() { "fnRenderEnum" }
            };
            var config = new DataTableUI { Id = "fly01dt", UrlGridLoad = Url.Action("GridLoad"), UrlFunctions = Url.Action("Functions") + "?fns=" };

            config.Actions.AddRange(GetActionsInGrid(new List<DataTableUIAction>()
            {
                new DataTableUIAction { OnClickFn = "fnEditar", Label = "Editar", ShowIf = "row.registroFixo == 0" },
                new DataTableUIAction { OnClickFn = "fnExcluir", Label = "Excluir", ShowIf = "row.registroFixo == 0" }
            }));

            config.Columns.Add(new DataTableUIColumn { DataField = "codigoProduto", DisplayName = "Código", Priority = 1 });
            config.Columns.Add(new DataTableUIColumn { DataField = "descricao", DisplayName = "Descrição", Priority = 2 });
            config.Columns.Add(new DataTableUIColumn { DataField = "grupoProduto_descricao", DisplayName = "Grupo", Priority = 3 });
            config.Columns.Add(new DataTableUIColumn
            {
                DataField = "tipoProduto",
                DisplayName = "Tipo",
                Priority = 4,
                Options = new List<SelectOptionUI>(SystemValueHelper.GetUIElementBase(typeof(TipoProduto))),
                RenderFn = "fnRenderEnum(full.tipoProdutoCSS, full.tipoProdutoDescricao)"
            });

            config.Columns.Add(new DataTableUIColumn { DataField = "codigoBarras", DisplayName = "Código barras", Priority = 4 });

            cfg.Content.Add(config);

            return Content(JsonConvert.SerializeObject(cfg, JsonSerializerSetting.Default), "application/json");
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
                    Title = "Dados do Produto",
                    Buttons = new List<HtmlUIButton>(GetFormButtonsOnHeader())
                },
                UrlFunctions = Url.Action("Functions") + "?fns="
            };

            var config = new FormUI
            {
                Id = "fly01frm",
                Action = new FormUIAction
                {
                    Create = @Url.Action("Create"),
                    Edit = @Url.Action("Edit"),
                    Get = @Url.Action("Json") + "/",
                    List = Url.Action("List"),
                    Form = Url.Action("Form")
                },
                UrlFunctions = Url.Action("Functions") + "?fns=",
                ReadyFn = "fnFormReady"
            };

            config.Elements.Add(new InputHiddenUI { Id = "id" });
            config.Elements.Add(new InputHiddenUI { Id = "saldoProduto", Value = "0" });

            config.Elements.Add(new InputFileUI
            {
                Id = "imageProduto",
                Class = "col s12 m3",
                Label = "Imagem",
                Accept = "image/png",
                Image = true
            });
            config.Elements.Add(new InputTextUI { Id = "descricao", Class = "col s12 m9", Label = "Descrição", Required = true });
            config.Elements.Add(new InputTextUI { Id = "codigoProduto", Class = "col s12 m3", Label = "Código" });

            config.Elements.Add(new InputTextUI { Id = "codigoBarras", Class = "col s12 m3", Label = "Código de barras", Value = "SEM GTIN" });

            config.Elements.Add(new SelectUI
            {
                Id = "tipoProduto",
                Class = "col s12 m3",
                Label = "Tipo",
                Required = true,
                Options = new List<SelectOptionUI>(SystemValueHelper.GetUIElementBase(typeof(TipoProduto)).ToList().FindAll(x => "ProdutoFinal,Insumo,Outros".Contains(x.Value)).OrderByDescending(x => x.Label)),
                DomEvents = new List<DomEventUI>() { new DomEventUI() { DomEvent = "change", Function = "fnChangeTipoProduto" } }
            });

            config.Elements.Add(ElementUIHelper.GetAutoComplete(new AutoCompleteUI
            {
                Id = "grupoProdutoId",
                Class = "col s12 m3",
                Label = "Grupo",
                DataUrl = @Url.Action("GrupoProduto", "AutoComplete"),
                DataUrlPost = @Url.Action("NovoGrupoProduto"),
                LabelId = "grupoProdutoDescricao",
                PreFilter = "tipoProduto",
                DomEvents = new List<DomEventUI> { new DomEventUI { DomEvent = "autocompleteselect", Function = "fnChangeGrupoProduto" } }
            }, GrupoProdutoResourceHash));

            config.Elements.Add(new AutoCompleteUI
            {
                Id = "unidadeMedidaId",
                Class = "col s12 m3",
                Label = "Unidade de medida",
                Required = true,
                DataUrl = @Url.Action("UnidadeMedida", "AutoComplete"),
                LabelId = "unidadeMedidaDescricao"
            });
            config.Elements.Add(new InputCustommaskUI
            {
                Id = "aliquotaIpi",
                Class = "col s12 m3",
                Label = "Alíquota IPI",
                MaxLength = 5,
                Data = new { inputmask = "'mask': '9{1,3}[,9{1,2}] %', 'alias': 'numeric', 'suffix': ' %', 'autoUnmask': true, 'radixPoint': ',' " }
            });

            config.Elements.Add(new AutoCompleteUI
            {
                Id = "ncmId",
                Class = "col s12 m10",
                Label = "NCM",
                DataUrl = @Url.Action("Ncm", "AutoComplete"),
                LabelId = "ncmDescricao",
                DomEvents = new List<DomEventUI> { new DomEventUI { DomEvent = "autocompleteselect", Function = "fnChangeNCM" } }
            });
            config.Elements.Add(new InputTextUI { Id = "extipi", Class = "col s12 m2", Label = "EX TIPI", MaxLength = 3 });

            config.Elements.Add(new AutoCompleteUI
            {
                Id = "cestId",
                Class = "col s12",
                Label = "CEST (Escolha um NCM antes)",
                DataUrl = @Url.Action("Cest", "AutoComplete"),
                LabelId = "cestDescricao",
                PreFilter = "ncmId"
            });

            config.Elements.Add(new InputFloatUI { Id = "saldoMinimo", Class = "col s12 m3", Label = "Saldo Mínimo", Digits = 3 });
            config.Elements.Add(new InputFloatUI
            {
                Id = "saldoProdutoField",
                Class = "col s12 m3",
                Label = "Saldo Atual",
                Digits = 3,
                Disabled = true,
                DomEvents = new List<DomEventUI> { new DomEventUI { DomEvent = "blur", Function = "fnChangeSaldoProduto" } },

            });
            config.Elements.Add(new InputCurrencyUI { Id = "valorCusto", Class = "col s12 m3", Label = "Valor Custo" });
            config.Elements.Add(new InputCurrencyUI { Id = "valorVenda", Class = "col s12 m3", Label = "Valor Venda" });

            config.Elements.Add(new AutoCompleteUI()
            {
                Id = "enquadramentoLegalIPIId",
                Class = "col s12",
                Label = "Enquadramento Legal do IPI",
                DataUrl = @Url.Action("EnquadramentoLegalIPI", "AutoComplete"),
                LabelId = "enquadramentoLegalIPIDescricao"
            });

            config.Elements.Add(new SelectUI
            {
                Id = "origemMercadoria",
                Class = "col s12",
                Label = "Origem Mercadoria",
                Required = true,
                Options = new List<SelectOptionUI>(SystemValueHelper.GetUIElementBase(typeof(OrigemMercadoria)))
            });

            config.Elements.Add(new TextAreaUI { Id = "observacao", Class = "col s12", Label = "Observação", MaxLength = 200 });

            List<TooltipUI> tooltips = GetHelpers();

            if (tooltips != null)
                config.Helpers.AddRange(tooltips);

            config.Helpers.Add(new TooltipUI
            {
                Id = "extipi",
                Tooltip = new HelperUITooltip()
                {
                    Text = "Informe se for necessário para nota fiscal de exportação. Informar de acordo com o código EX da TIPI se houver para o NCM do produto."
                }
            });
            cfg.Content.Add(config);

            return cfg;
        }

        public virtual List<TooltipUI> GetHelpers()
            => null;

        public ContentResult FormImportacao()
        {
            var cfg = new ContentUIBase(Url.Action("Sidebar", "Home"))
            {
                History = new ContentUIHistory()
                {
                    Default = Url.Action("ImportarProduto"),
                },
                Header = new HtmlUIHeader()
                {
                    Title = $"Importar Produtos",
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
                ReadyFn = "fnImportarProdutoFormReady",
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

        public List<HtmlUIButton> GetFormImportacaoButtonsOnHeader()
        {
            var target = new List<HtmlUIButton>();

            if (UserCanWrite)
            {
                target.Add(new HtmlUIButton() { Id = "cancel", Label = "Voltar", OnClickFn = "fnCancelar" });
                target.Add(new HtmlUIButton() { Id = "save", Label = "Importar", OnClickFn = "fnCarregarArquivo", Type = "submit" });
            }

            return target;
        }

        public JsonResult ImportaArquivo(string pConteudo)
        {
            try
            {
                var arquivoVM = ImportacaoArquivoHelper.ImportaProduto($"Cadastro de Produtos", pConteudo);
                return JsonResponseStatus.GetJson(arquivoVM);

            }
            catch (Exception ex)
            {
                var error = JsonConvert.DeserializeObject<ErrorInfo>(ex.Message);
                return JsonResponseStatus.GetFailure(error.Message);
            }
        }

        public virtual ActionResult ImportarProduto()
        {
            return View();
        }

        #region onDemand

        [HttpPost]
        public JsonResult NovoGrupoProduto(string term)
        {
            try
            {
                var tipoProduto = Request.QueryString["tipo"];

                var entity = new GrupoProdutoVM
                {
                    Descricao = term,
                    TipoProduto = tipoProduto
                };

                var resourceName = AppDefaults.GetResourceName(typeof(GrupoProdutoVM));
                var data = RestHelper.ExecutePostRequest<GrupoProdutoVM>(resourceName, entity, AppDefaults.GetQueryStringDefault());

                return JsonResponseStatus.Get(new ErrorInfo() { HasError = false }, Operation.Create, data.Id);
            }
            catch (Exception ex)
            {
                var error = JsonConvert.DeserializeObject<ErrorInfo>(ex.Message);
                return JsonResponseStatus.GetFailure(error.Message);
            }

        }

        public ContentResult FormModal()
        {
            ConfiguracaoPersonalizacaoVM personalizacao = null;
            try
            {
                personalizacao = RestHelper.ExecuteGetRequest<ResultBase<ConfiguracaoPersonalizacaoVM>>("ConfiguracaoPersonalizacao", queryString: null)?.Data?.FirstOrDefault();
            }
            catch (Exception)
            {
            }
            var emiteNotaFiscal = personalizacao != null ? personalizacao.EmiteNotaFiscal : true;
            ModalUIForm config = new ModalUIForm()
            {
                Title = "Adicionar produto",
                ConfirmAction = new ModalUIAction() { Label = "Salvar" },
                CancelAction = new ModalUIAction() { Label = "Cancelar" },
                Action = new FormUIAction
                {
                    Create = @Url.Action("Create"),
                    Edit = @Url.Action("Edit"),
                    Get = @Url.Action("Json") + "/",
                },
                Id = "fly01mdlfrmProduto",
                UrlFunctions = Url.Action("Functions") + "?fns=",
                ReadyFn = "fnFormReadyModal"
            };

            config.Elements.Add(new InputHiddenUI { Id = "id" });
            config.Elements.Add(new InputHiddenUI { Id = "emiteNotaFiscal", Value = emiteNotaFiscal.ToString() });
            config.Elements.Add(new InputHiddenUI { Id = "valorCusto", Value = "0" });
            config.Elements.Add(new InputHiddenUI { Id = "valorVenda", Value = "0" });

            config.Elements.Add(new InputTextUI { Id = "descricao", Class = "col s12 m9", Label = "Descrição", Required = true });

            config.Elements.Add(new SelectUI
            {
                Id = "tipoProduto",
                Class = "col s12 m3",
                Label = "Tipo",
                Required = true,
                Options = new List<SelectOptionUI>(SystemValueHelper.GetUIElementBase(typeof(TipoProduto)).ToList().FindAll(x => "ProdutoFinal,Insumo,Outros".Contains(x.Value)).OrderByDescending(x => x.Label)),
                DomEvents = new List<DomEventUI>() { new DomEventUI() { DomEvent = "change", Function = "fnChangeTipoProduto" } },
            });

            config.Elements.Add(ElementUIHelper.GetAutoComplete(new AutoCompleteUI
            {
                Id = "grupoProdutoId",
                Class = "col s12 m7",
                Label = "Grupo",
                DataUrl = @Url.Action("GrupoProduto", "AutoComplete"),
                DataUrlPost = @Url.Action("NovoGrupoProduto"),
                LabelId = "grupoProdutoDescricao",
                PreFilter = "tipoProduto",
                DomEvents = new List<DomEventUI> { new DomEventUI { DomEvent = "autocompleteselect", Function = "fnChangeGrupoProduto" } }
            }, GrupoProdutoResourceHash));

            config.Elements.Add(new AutoCompleteUI
            {
                Id = "unidadeMedidaId",
                Class = "col s12 m3",
                Label = "Unidade de medida",
                Required = true,
                DataUrl = @Url.Action("UnidadeMedida", "AutoComplete"),
                LabelId = "unidadeMedidaDescricao"
            });
                      
            config.Elements.Add(new InputFloatUI
            {
                Id = "saldoProduto",
                Class = "col s12 m2",
                Label = "Saldo atual",
                Value = "0",
                Digits = 3
            });

            if (emiteNotaFiscal)
            {
                config.Helpers.Add(new TooltipUI
                {
                    Id = "codigoBarras",
                    Tooltip = new HelperUITooltip()
                    {
                        Text = "Informe códigos GTIN (8, 12, 13, 14), de acordo com o NCM e CEST. Para produtos que não possuem código de barras, informe o literal “SEM GTIN”, se utilizar este produto para emitir notas fiscais."
                    }
                });
                config.Elements.Add(new AutoCompleteUI
                {
                    Id = "ncmId",
                    Class = "col s12 m7",
                    Label = "NCM",
                    DataUrl = @Url.Action("Ncm", "AutoComplete"),
                    LabelId = "ncmDescricao",
                    DomEvents = new List<DomEventUI> { new DomEventUI { DomEvent = "autocompleteselect", Function = "fnChangeNCM" } }
                });
                config.Elements.Add(new InputTextUI { Id = "codigoBarras", Class = "col s12 m2", Label = "Código de barras", Value = "SEM GTIN" });

                config.Elements.Add(new InputCustommaskUI
                {
                    Id = "aliquotaIpi",
                    Class = "col s12 m3",
                    Label = "Alíquota IPI",
                    MaxLength = 5,
                    Data = new { inputmask = "'mask': '9{1,3}[,9{1,2}] %', 'alias': 'numeric', 'suffix': ' %', 'autoUnmask': true, 'radixPoint': ',' " }
                });

                config.Elements.Add(new AutoCompleteUI
                {
                    Id = "cestId",
                    Class = "col s12 m7",
                    Label = "CEST (Escolha um NCM antes)",
                    DataUrl = @Url.Action("Cest", "AutoComplete"),
                    LabelId = "cestDescricao",
                    PreFilter = "ncmId"
                });

                config.Elements.Add(new SelectUI
                {
                    Id = "origemMercadoria",
                    Class = "col s12 m3",
                    Label = "Origem Mercadoria",
                    Required = true,
                    Options = new List<SelectOptionUI>(SystemValueHelper.GetUIElementBase(typeof(OrigemMercadoria)))
                });

                config.Helpers.Add(new TooltipUI
                {
                    Id = "extipi",
                    Tooltip = new HelperUITooltip()
                    {
                        Text = "Informe se for necessário para nota fiscal de exportação. Informar de acordo com o código EX da TIPI se houver para o NCM do produto."
                    }
                });

                config.Elements.Add(new InputTextUI { Id = "extipi", Class = "col s12 m2", Label = "EX TIPI", MaxLength = 3 });

                config.Helpers.Add(new TooltipUI
                {
                    Id = "enquadramentoLegalIPIId",
                    Tooltip = new HelperUITooltip()
                    {
                        Text = "Informe o enquadramento legal do IPI, se utilizar este produto com um grupo tributário que calcula IPI ao emitir notas fiscais."
                    }
                });

                config.Elements.Add(new AutoCompleteUI()
                {
                    Id = "enquadramentoLegalIPIId",
                    Class = "col s12",
                    Label = "Enquadramento Legal do IPI",
                    DataUrl = @Url.Action("EnquadramentoLegalIPI", "AutoComplete"),
                    LabelId = "enquadramentoLegalIPIDescricao"
                });
            }
            return Content(JsonConvert.SerializeObject(config, JsonSerializerSetting.Front), "application/json");
        }
        #endregion
    }
}