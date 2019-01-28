using Fly01.Core;
using Fly01.Core.Defaults;
using Fly01.Core.Entities.Domains.Enum;
using Fly01.Core.Helpers;
using Fly01.Core.Presentation;
using Fly01.Core.Presentation.Commons;
using Fly01.Core.Rest;
using Fly01.Core.ViewModels.Presentation.Commons;
using Fly01.uiJS.Classes;
using Fly01.uiJS.Classes.Elements;
using Fly01.uiJS.Classes.Helpers;
using Fly01.uiJS.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Serialization;
using Fly01.EmissaoNFE.Domain.Entities.NFe;

namespace Fly01.Compras.Controllers
{
    //TODO:  [OperationRole(ResourceKey = ResourceHashConst.ComprasComprasNFeImportada)]
    public class NFeImportacaoController : BaseController<NFeImportacaoVM>
    {
        public override Func<NFeImportacaoVM, object> GetDisplayData()
        {
            return x => new
            {
                id = x.Id,
                fornecedor_nome = x.Fornecedor?.Nome,
                status = x.Status,
                statusDescription = EnumHelper.GetDescription(typeof(Status), x.Status),
                statusCssClass = EnumHelper.GetCSS(typeof(Status), x.Status),
                statusValue = EnumHelper.GetValue(typeof(Status), x.Status),
                valorTotal = x.ValorTotal.ToString("C", AppDefaults.CultureInfoDefault),
                dataEmissao = x.DataEmissao.ToString("dd/MM/yyyy")
            };
        }

        public override List<HtmlUIButton> GetListButtonsOnHeader()
        {
            var target = new List<HtmlUIButton>();

            //if TODO: (UserCanWrite)
            //{
                target.Add(new HtmlUIButton { Id = "new", Label = "Novo", OnClickFn = "fnNovo", Position = HtmlUIButtonPosition.Main });
            //}

            return target;
        }

        public override ContentResult List()
        {
            var cfg = new ContentUIBase(Url.Action("Sidebar", "Home"))
            {
                History = new ContentUIHistory { Default = Url.Action("Index") },
                Header = new HtmlUIHeader
                {
                    Title = "Importação de notas fiscais de entrada",
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
                new DataTableUIAction { OnClickFn = "fnEditar", Label = "Editar", ShowIf = "row.registroFixo == 0" },
                new DataTableUIAction { OnClickFn = "fnEditar", Label = "Visualizar", ShowIf = "row.registroFixo == 0" },
                new DataTableUIAction { OnClickFn = "fnExcluir", Label = "Excluir", ShowIf = "row.registroFixo == 0" },
                new DataTableUIAction { OnClickFn = "fnEditar", Label = "Baixar XML", ShowIf = "row.registroFixo == 0" }
            }));

            config.Columns.Add(new DataTableUIColumn { DataField = "fornecedor_nome", DisplayName = "Fornecedor", Priority = 1 });
            config.Columns.Add(new DataTableUIColumn { DataField = "dataEmissao", DisplayName = "Data", Priority = 2 });
            config.Columns.Add(new DataTableUIColumn
            {
                DataField = "status",
                DisplayName = "Status",
                Priority = 3,
                Options = new List<SelectOptionUI>(SystemValueHelper.GetUIElementBase(typeof(Status))),
                RenderFn = "fnRenderEnum(full.statusCssClass, full.statusDescription)"
            });
            config.Columns.Add(new DataTableUIColumn { DataField = "valorTotal", DisplayName = "Valor Total", Priority = 4, Type = "tel" });

            cfg.Content.Add(config);

            return Content(JsonConvert.SerializeObject(cfg, JsonSerializerSetting.Front), "application/json");
        }

        protected override ContentUI FormJson()
            => FormNFeImportacaoJson();

        protected ContentUI FormNFeImportacaoJson(bool isEdit = false)
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
                    Title = "Importação XML",
                    Buttons = new List<HtmlUIButton>(GetFormButtonsOnHeader())
                },
                UrlFunctions = Url.Action("Functions") + "?fns="
            };

            var config = new FormWizardUI
            {
                Id = "fly01frm",

                Action = new FormUIAction
                {
                    Create = @Url.Action("Create"),
                    Edit = @Url.Action("Edit"),
                    Get = @Url.Action("Json") + "/",
                    List = @Url.Action("List")

                },
                UrlFunctions = Url.Action("Functions") + "?fns=",
                ReadyFn = "fnFormReady",
                //Functions = new List<string> { "fnChangeEstado" },
                Steps = new List<FormWizardUIStep>()
                {
                    new FormWizardUIStep()
                    {
                        Title = "Importar arquivo",
                        Id = "stepImportarArquivo",
                        Quantity = 2,
                    },
                    new FormWizardUIStep()
                    {
                        Title = "Fornecedor",
                        Id = "stepFornecedor",
                        Quantity = 11,
                    },
                    new FormWizardUIStep()
                    {
                        Title = "Transportadora",
                        Id = "stepTransportadora",
                        Quantity = 3,
                    },
                    new FormWizardUIStep()
                    {
                        Title = "Pendências",
                        Id = "stepProdutosPendentes",
                        Quantity = 5,
                    },
                    new FormWizardUIStep()
                    {
                        Title = "Produtos",
                        Id = "stepProdutos",
                        Quantity = 11,
                    },
                    new FormWizardUIStep()
                    {
                        Title = "Resumo/Finalizar",
                        Id = "stepResumoFinalizar",
                        Quantity = 15,
                    }
                },
                Rule = isEdit ? "parallel" : "linear",
                ShowStepNumbers = true,
            };

            #region stepImportação
            config.Elements.Add(new InputHiddenUI { Id = "id" });
            config.Elements.Add(new InputFileUI { Id = "arquivoXML", Class = "col s12 m12", Label = "Arquivo de importação (.xml)", Required = false, Accept = ".xml" });
            #endregion

            #region step Fornecedor

            config.Elements.Add(ElementUIHelper.GetAutoComplete(new AutoCompleteUI
            {
                Id = "fornecedorId",
                Class = "col s12 m6",
                Label = "Fornecedor",
                DataUrl = Url.Action("Fornecedor", "AutoComplete"),
                LabelId = "fornecedorNome",
                DataUrlPost = Url.Action("PostFornecedor"),
                Required = true
            }, null));

            config.Elements.Add(new InputCheckboxUI
            {
                Id = "atualizarFornecedor",
                Class = "col s12 m6",
                Label = "Atualizar dados do fornecedor encontrado.",
                DomEvents = new List<DomEventUI>
                {
                    new DomEventUI { DomEvent = "change", Function = "" }
                }
            });
            config.Helpers.Add(new TooltipUI
            {
                Id = "atualizarFornecedor",
                Tooltip = new HelperUITooltip()
                {
                    Text = "Marque a opção, caso deseje atualizar o fornecedor existente com os dados da importação."
                }
            });
            config.Elements.Add(new InputCheckboxUI
            {
                Id = "criarNovoFornecedor",
                Class = "col s12 m6",
                Label = "Cadastrar novo fornecedor ao fim do processo.",
                DomEvents = new List<DomEventUI>
                {
                    new DomEventUI { DomEvent = "change", Function = "" }
                }
            });
            config.Elements.Add(new LabelSetUI { Id = "labelSetFornecedor", Class = "col s12", Label = "Dados Fornecedor XMl" });
            config.Elements.Add(new InputTextUI { Id = "fornecedorNome", Class = "col s12 m6", Label = "Nome", MaxLength = 60 , Readonly = true});
            config.Elements.Add(new InputTextUI { Id = "fornecedorCnpj", Class = "col s12 m6", Label = "Cnpj", MaxLength = 60 , Readonly = true});
            config.Elements.Add(new InputTextUI { Id = "inscEstadual", Class = "col s12 m6", Label = "Inscrição estadual", MaxLength = 60 , Readonly = true });
            config.Elements.Add(new InputTextUI { Id = "razaoSocial", Class = "col s12 m6", Label = "Razão social", MaxLength = 60 , Readonly = true });
            config.Elements.Add(new InputTextUI { Id = "cnpj", Class = "col s12 m6", Label = "CNPJ", MaxLength = 60, Readonly = true });
                                                                                   
            #endregion

            cfg.Content.Add(config);
            return cfg;
        }

        public ContentResult FormNFeImportacao(bool isEdit = false)
            => Content(JsonConvert.SerializeObject(FormNFeImportacaoJson(isEdit), JsonSerializerSetting.Front), "application/json");

        public override ContentResult Json(Guid id)
        {
            try
            {
                NFeImportacaoVM entity = Get(id);

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(Base64Helper.DecodificaBase64(entity.XML));

                XmlElement xelRoot = doc.DocumentElement;
                XmlNode tagNFe = xelRoot.FirstChild;
                if (tagNFe.Name == "NFe")
                {
                    XmlSerializer ser = new XmlSerializer(typeof(NFeVM));
                    StringReader sr = new StringReader(tagNFe.OuterXml);
                    var NFe = (NFeVM)ser.Deserialize(sr);
                    if(NFe != null && NFe.InfoNFe != null && NFe.InfoNFe.Emitente != null)
                    {
                        entity.FornecedorNome = NFe.InfoNFe.Emitente.NomeFantasia;
                        entity.FornecedorCnpj = NFe.InfoNFe.Emitente.Cnpj;
                    }
                }

                var x = Content(JsonConvert.SerializeObject(entity, JsonSerializerSetting.Front), "application/json");
                return x;
            }
            catch (Exception ex)
            {
                var error = JsonConvert.DeserializeObject<ErrorInfo>(ex.Message);
                return Content(JsonConvert.SerializeObject(JsonResponseStatus.GetFailure(error.Message).Data), "application/json");
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public JsonResult ImportaArquivoXML(string conteudo)
        {
            try
            {
                var nota = new NFeImportacaoVM
                {
                    XML = Base64Helper.CodificaBase64(conteudo),
                    XmlMd5 = Base64Helper.CalculaMD5Hash(conteudo),
                    Status = Status.Aberto.ToString()
                };
                return Create(nota);
            }
            catch (Exception ex)
            {
                var error = JsonConvert.DeserializeObject<ErrorInfo>(ex.Message);
                return JsonResponseStatus.GetFailure(error.Message);
            }
        }

        [HttpPost]
        public override JsonResult Create(NFeImportacaoVM entityVM)
        {
            try
            {
                var postResponse = RestHelper.ExecutePostRequest(ResourceName, JsonConvert.SerializeObject(entityVM, JsonSerializerSetting.Default));
                NFeImportacaoVM postResult = JsonConvert.DeserializeObject<NFeImportacaoVM>(postResponse);
                var response = new JsonResult
                {
                    Data = new { success = true, message = AppDefaults.EditSuccessMessage, id = postResult.Id.ToString(), tipoFrete = postResult.TipoFrete.ToString() }
                };
                return (response);
            }
            catch (Exception ex)
            {
                var error = JsonConvert.DeserializeObject<ErrorInfo>(ex.Message);
                return JsonResponseStatus.GetFailure(error.Message);
            }
        }
    }
}
