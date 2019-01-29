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
using Fly01.Core.ViewModels;
using System.Dynamic;

namespace Fly01.Compras.Controllers
{
    [OperationRole(ResourceKey = ResourceHashConst.ComprasComprasNFeImportacao)]
    public class NFeImportacaoController : BaseController<NFeImportacaoVM>
    {
        public NFeImportacaoController()
        {
            //TODO: Select properties
            ExpandProperties = "fornecedor($select=nome),pedido,transportadora,condicaoParcelamento,formaPagamento,categoria";
        }

        public override Func<NFeImportacaoVM, object> GetDisplayData()
        {
            return x => new
            {
                id = x.Id,
                status = x.Status,
                statusDescription = EnumHelper.GetDescription(typeof(Status), x.Status),
                statusCssClass = EnumHelper.GetCSS(typeof(Status), x.Status),
                statusValue = EnumHelper.GetValue(typeof(Status), x.Status),
                valorTotal = x.ValorTotal.ToString("C", AppDefaults.CultureInfoDefault),
                dataEmissao = x.DataEmissao.ToString("dd/MM/yyyy"),
                fornecedor_nome = (x.Fornecedor != null ? x.Fornecedor.Nome : "Vinculação pendente"),
                serie = x.Serie,
                numero = x.Numero
            };
        }

        public List<HtmlUIButton> GetListButtonsOnHeaderCustom(string buttonLabel, string buttonOnClick)
        {
            var target = new List<HtmlUIButton>();

            if (UserCanWrite)
            {
                target.Add(new HtmlUIButton { Id = "new", Label = "Novo", OnClickFn = "fnNovo", Position = HtmlUIButtonPosition.Main });
                target.Add(new HtmlUIButton { Id = "filterGrid", Label = buttonLabel, OnClickFn = buttonOnClick, Position = HtmlUIButtonPosition.In });
            }

            return target;
        }

        public override ContentResult List()
                => ListNFeImportacao();

        public ContentResult ListNFeImportacao(string gridLoad = "GridLoad")
        {
            var buttonLabel = "Mostrar todas as importações";
            var buttonOnClick = "fnRemoveFilter";

            if (Request.QueryString["action"] == "GridLoadNoFilter")
            {
                gridLoad = Request.QueryString["action"];
                buttonLabel = "Mostrar importações do mês atual";
                buttonOnClick = "fnAddFilter";
            }

            var cfg = new ContentUIBase(Url.Action("Sidebar", "Home"))
            {
                History = new ContentUIHistory { Default = Url.Action("Index") },
                Header = new HtmlUIHeader
                {
                    Title = "Importação XML",
                    Buttons = new List<HtmlUIButton>(GetListButtonsOnHeaderCustom(buttonLabel, buttonOnClick))
                },
                UrlFunctions = Url.Action("Functions") + "?fns="
            };

            var cfgForm = new FormUI
            {
                Id = "fly01frm",
                UrlFunctions = Url.Action("Functions", "NFeImportacao") + "?fns=",
                ReadyFn = gridLoad == "GridLoad" ? "" : "fnChangeInput",
                Elements = new List<BaseUI>()
                {
                    new InputHiddenUI()
                    {
                        Id = "dataFinal",
                        Name = "dataFinal"
                    },
                    new InputHiddenUI()
                    {
                        Id = "dataInicial",
                        Name = "dataInicial"
                    }
                }
            };

            if (gridLoad == "GridLoad")
            {
                cfgForm.Elements.Add(new PeriodPickerUI()
                {
                    Label = "Selecione o período",
                    Id = "mesPicker",
                    Name = "mesPicker",
                    Class = "col s12 m6 offset-m3 l4 offset-l4",
                    DomEvents = new List<DomEventUI>()
                    {
                        new DomEventUI()
                        {
                            DomEvent = "change",
                            Function = "fnUpdateDataFinal"
                        }
                    }
                });
                cfgForm.ReadyFn = "fnUpdateDataFinal";
            }

            cfg.Content.Add(cfgForm);

            var config = new DataTableUI
            {
                Id = "fly01dt",
                UrlGridLoad = Url.Action("GridLoad"),
                UrlFunctions = Url.Action("Functions") + "?fns=",
                Functions = new List<string>() { "fnRenderEnum" },
                Parameters = new List<DataTableUIParameter>
                {
                    new DataTableUIParameter() {Id = "dataInicial", Required = (gridLoad == "GridLoad") },
                    new DataTableUIParameter() {Id = "dataFinal", Required = (gridLoad == "GridLoad") }
                },
                Options = new DataTableUIConfig
                {
                    OrderColumn = 0,
                    OrderDir = "desc"
                }
            };

            config.Actions.AddRange(GetActionsInGrid(new List<DataTableUIAction>()
            {
                new DataTableUIAction { OnClickFn = "fnEditar", Label = "Editar" },
                new DataTableUIAction { OnClickFn = "fnVisualizar", Label = "Visualizar" },
                new DataTableUIAction { OnClickFn = "fnExcluir", Label = "Excluir" },
                new DataTableUIAction { OnClickFn = "fnBaixarXML", Label = "Baixar XML" }
            }));

            config.Columns.Add(new DataTableUIColumn { DataField = "dataEmissao", DisplayName = "Data Emissão", Priority = 1, Type = "date" });
            config.Columns.Add(new DataTableUIColumn { DataField = "fornecedor_nome", DisplayName = "Fornecedor", Priority = 3 });
            config.Columns.Add(new DataTableUIColumn { DataField = "serie", DisplayName = "Série", Priority = 6 });
            config.Columns.Add(new DataTableUIColumn { DataField = "numero", DisplayName = "Número", Priority = 5 });
            config.Columns.Add(new DataTableUIColumn
            {
                DataField = "status",
                DisplayName = "Status",
                Priority = 4,
                Options = new List<SelectOptionUI>(SystemValueHelper.GetUIElementBase(typeof(Status))),
                RenderFn = "fnRenderEnum(full.statusCssClass, full.statusDescription)"
            });
            config.Columns.Add(new DataTableUIColumn { DataField = "valorTotal", DisplayName = "Valor Total Nota", Priority = 2 });

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
                Steps = new List<FormWizardUIStep>()
                {
                    new FormWizardUIStep()
                    {
                        Title = "Importar arquivo",
                        Id = "stepImportarArquivo",
                        Quantity = 8,
                    },
                    new FormWizardUIStep()
                    {
                        Title = "Fornecedor",
                        Id = "stepFornecedor",
                        Quantity = 8,
                    },
                    new FormWizardUIStep()
                    {
                        Title = "Transportadora",
                        Id = "stepTransportadora",
                        Quantity = 8,
                    },
                    new FormWizardUIStep()
                    {
                        Title = "Pendências",
                        Id = "stepProdutosPendencias",
                        Quantity = 1,
                    },
                    new FormWizardUIStep()
                    {
                        Title = "Produtos",
                        Id = "stepProdutos",
                        Quantity = 2,
                    },
                    new FormWizardUIStep()
                    {
                        Title = "Resumo/Finalizar",
                        Id = "stepResumoFinalizar",
                        Quantity = 3,
                    }
                },
                Rule = isEdit ? "parallel" : "linear",
                ShowStepNumbers = true,
            };

            #region stepImportação
            config.Elements.Add(new InputHiddenUI { Id = "id" });
            config.Elements.Add(new InputHiddenUI { Id = "serie" });
            config.Elements.Add(new InputHiddenUI { Id = "numero" });
            config.Elements.Add(new InputHiddenUI { Id = "dataEmissao" });
            config.Elements.Add(new InputHiddenUI { Id = "status" });
            config.Elements.Add(new InputHiddenUI { Id = "tipo" });
            config.Elements.Add(new InputHiddenUI { Id = "tipoFrete" });
            config.Elements.Add(new InputFileUI { Id = "arquivoXML", Class = "col s12 m12", Label = "Arquivo de importação (.xml)", Required = true, Accept = ".xml" });
            #endregion

            #region step Fornecedor

            config.Elements.Add(ElementUIHelper.GetAutoComplete(new AutoCompleteUI
            {
                Id = "fornecedorId",
                Class = "col s12 m6",
                Label = "Fornecedor",
                DataUrl = Url.Action("FornecedorXML", "AutoComplete"),
                LabelId = "fornecedorNome",
                DataUrlPost = Url.Action("PostFornecedor"),
                Required = true
            }, null));

            config.Elements.Add(new InputCheckboxUI
            {
                Id = "atualizaDadosFornecedor",
                Class = "col s12 m6",
                Label = "Atualizar dados do fornecedor encontrado",
                DomEvents = new List<DomEventUI>
                {
                    new DomEventUI { DomEvent = "change", Function = "" }
                }
            });
            config.Helpers.Add(new TooltipUI
            {
                Id = "atualizaDadosFornecedor",
                Tooltip = new HelperUITooltip()
                {
                    Text = "Marque a opção, caso deseje atualizar o fornecedor existente com os dados da importação."
                }
            });
            config.Elements.Add(new InputCheckboxUI
            {
                Id = "novoFornecedor",
                Class = "col s12 m6",
                Label = "Cadastrar novo fornecedor ao fim do processo",
                DomEvents = new List<DomEventUI>
                {
                    new DomEventUI { DomEvent = "change", Function = "" }
                }
            });
            config.Elements.Add(new LabelSetUI { Id = "labelSetFornecedor", Class = "col s12", Label = "Dados Fornecedor XML" });
            config.Elements.Add(new InputTextUI { Id = "fornecedorNomeXml", Class = "col s12 m6", Label = "Nome", MaxLength = 60, Readonly = true });
            config.Elements.Add(new InputTextUI { Id = "fornecedorCnpjXml", Class = "col s12 m6", Label = "CNPJ", MaxLength = 60, Readonly = true });
            config.Elements.Add(new InputTextUI { Id = "fornecedorInscEstadualXml", Class = "col s12 m6", Label = "Inscrição estadual", MaxLength = 60, Readonly = true });
            config.Elements.Add(new InputTextUI { Id = "fornecedorRazaoSocialXml", Class = "col s12 m6", Label = "Razão social", MaxLength = 60, Readonly = true });

            #endregion

            #region step Tansportadora
            config.Elements.Add(ElementUIHelper.GetAutoComplete(new AutoCompleteUI
            {
                Id = "transportadoraId",
                Class = "col s12 m6",
                Label = "Transportadora",
                DataUrl = Url.Action("TransportadoraXML", "AutoComplete"),
                LabelId = "transportadoraNome",
                DataUrlPost = Url.Action("PostTransportadora"),
                Required = false
            }, null));

            config.Elements.Add(new InputCheckboxUI
            {
                Id = "atualizaDadosTransportadora",
                Class = "col s12 m6",
                Label = "Atualizar dados da transportadora encontrada",
                DomEvents = new List<DomEventUI>
                {
                    new DomEventUI { DomEvent = "change", Function = "" }
                }
            });

            config.Elements.Add(new InputCheckboxUI
            {
                Id = "novaTransportadora",
                Class = "col s12 m6",
                Label = "Cadastrar nova transportadora ao fim do processo",
                DomEvents = new List<DomEventUI>
                {
                    new DomEventUI { DomEvent = "change", Function = "" }
                }
            });
            config.Helpers.Add(new TooltipUI
            {
                Id = "novaTransportadora",
                Tooltip = new HelperUITooltip()
                {
                    Text = "Marque a opção, caso deseje atualizar a transportadora existente com os dados da importação."
                }
            });

            config.Elements.Add(new LabelSetUI { Id = "labelSetTransportadora", Class = "col s12", Label = "Dados Transpotadora XML" });
            config.Elements.Add(new InputTextUI { Id = "transportadoraRazaoSocialXml", Class = "col s12 m6", Label = "Nome", Readonly = true });
            config.Elements.Add(new InputTextUI { Id = "transportadorCNPJXml", Class = "col s12 m6", Label = "CNPJ", Readonly = true });
            config.Elements.Add(new InputTextUI { Id = "transportadoraInscEstadualXml", Class = "col s12 m6", Label = "Inscrição Estadual", Readonly = true });
            config.Elements.Add(new InputTextUI { Id = "transportadoraUFXml", Class = "col s12 m6", Label = "UF", Readonly = true });

            #endregion

            #region step Produtos Pendências
            config.Elements.Add(new DivElementUI { Id = "produtosPendencias", Class = "col s12 visible" });
            #endregion

            #region step Produtos

            config.Elements.Add(new ButtonUI
            {
                Id = "alterarSelecionados",                
                Class = "col s12 m3 right",
                ClassBtn = "btn-narrow",
                Label = "",
                Value = "Alterar Selecionados",
                DomEvents = new List<DomEventUI>
                {
                    new DomEventUI { DomEvent = "click", Function = "fnModalAlterarProdutos" }
                }
            });

            config.Elements.Add(new DivElementUI { Id = "produtosResolvidos", Class = "col s12 visible" });

            #endregion

            cfg.Content.Add(config);
            cfg.Content.Add(GetDtProdutosPendenciasCfg());
            cfg.Content.Add(GetDtProdutosResolvidosfg());
            return cfg;
        }

        public ContentResult FormNFeImportacao(bool isEdit = false)
            => Content(JsonConvert.SerializeObject(FormNFeImportacaoJson(isEdit), JsonSerializerSetting.Front), "application/json");

        protected NFeImportacaoFormVM GetNFeImportacaoForm(Guid id)
        {
            string resourceName = ResourceName;
            string resourceById = String.Format("{0}/{1}", ResourceName, id);

            if (string.IsNullOrEmpty(ExpandProperties))
            {
                return RestHelper.ExecuteGetRequest<NFeImportacaoFormVM>(resourceById);
            }
            else
            {
                var queryString = new Dictionary<string, string> {
                    { "$expand", ExpandProperties }
                };
                return RestHelper.ExecuteGetRequest<NFeImportacaoFormVM>(resourceById, queryString);
            }
        }

        public override ContentResult Json(Guid id)
        {
            try
            {
                var entity = GetNFeImportacaoForm(id);

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(Base64Helper.DecodificaBase64(entity.XML));

                XmlElement xelRoot = doc.DocumentElement;
                XmlNode tagNFe = xelRoot.FirstChild;
                if (tagNFe.Name == "NFe")
                {
                    XmlSerializer ser = new XmlSerializer(typeof(NFeVM));
                    StringReader sr = new StringReader(tagNFe.OuterXml);
                    var NFe = (NFeVM)ser.Deserialize(sr);
                    if (NFe != null && NFe.InfoNFe != null)
                    {
                        if (NFe.InfoNFe.Emitente != null)
                        {
                            entity.FornecedorNomeXml = NFe.InfoNFe.Emitente?.NomeFantasia;
                            entity.FornecedorCnpjXml = NFe.InfoNFe.Emitente?.Cnpj;
                            entity.FornecedorRazaoSocialXml = NFe.InfoNFe.Emitente?.NomeFantasia;
                            entity.FornecedorInscEstadualXml = NFe.InfoNFe.Emitente?.InscricaoEstadual;
                        }
                        if (NFe.InfoNFe.Transporte != null && NFe.InfoNFe.Transporte.Transportadora != null)
                        {
                            entity.TransportadoraRazaoSocialXml = NFe.InfoNFe.Transporte.Transportadora?.RazaoSocial;
                            entity.TransportadorCNPJXml = NFe.InfoNFe.Transporte.Transportadora?.CNPJ;
                            entity.TransportadoraInscEstadualXml = NFe.InfoNFe.Transporte.Transportadora?.IE;
                            entity.TransportadoraUFXml = NFe.InfoNFe.Transporte.Transportadora?.UF;
                        }
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

        //estava bloqueando como conteúdo desconhecido
        [HttpPost]
        [ValidateInput(false)]
        public JsonResult ImportaArquivoXML(string conteudo)
        {
            try
            {
                dynamic nfeImportacao = new ExpandoObject();
                nfeImportacao.xml = Base64Helper.CodificaBase64(conteudo);
                nfeImportacao.xmlMd5 = Base64Helper.CalculaMD5Hash(conteudo);
                nfeImportacao.status = Status.Aberto.ToString();

                var postResponse = RestHelper.ExecutePostRequest(ResourceName, JsonConvert.SerializeObject(nfeImportacao, JsonSerializerSetting.Default));
                NFeImportacaoVM postResult = JsonConvert.DeserializeObject<NFeImportacaoVM>(postResponse);
                var response = new JsonResult
                {
                    Data = new { success = true, message = AppDefaults.EditSuccessMessage, id = postResult.Id.ToString(), tipoFrete = postResult.TipoFrete.ToString() }
                };
                return response;
            }
            catch (Exception ex)
            {
                var error = JsonConvert.DeserializeObject<ErrorInfo>(ex.Message);
                return JsonResponseStatus.GetFailure(error.Message);
            }
        }

        [OperationRole(PermissionValue = EPermissionValue.Read)]
        public ActionResult BaixarXML(Guid id)
        {
            try
            {
                var NFeImportacao = GetNFeImportacaoForm(id);

                string fileName = "NFeEntrada" + NFeImportacao.Numero.ToString() + ".xml";
                string xml = Base64Helper.DecodificaBase64(NFeImportacao.XML);
                xml = xml.Replace("\\", "");
                Session.Add(fileName, xml);

                return JsonResponseStatus.GetJson(new { downloadAddress = Url.Action("DownloadXMLString", new { fileName = fileName }) });
            }
            catch (Exception ex)
            {
                ErrorInfo error = JsonConvert.DeserializeObject<ErrorInfo>(ex.Message);
                return JsonResponseStatus.GetFailure(error.Message);
            }
        }

        public override JsonResult GridLoad(Dictionary<string, string> filters = null)
        {
            if (filters == null)
                filters = new Dictionary<string, string>();

            if (Request.QueryString["dataFinal"] != "")
                filters.Add("dataEmissao le ", Request.QueryString["dataFinal"]);
            if (Request.QueryString["dataInicial"] != "")
                filters.Add(" and dataEmissao ge ", Request.QueryString["dataInicial"]);

            return base.GridLoad(filters);
        }

        public JsonResult GridLoadNoFilter()
        {
            return GridLoad();
        }

        protected DataTableUI GetDtProdutosPendenciasCfg()
        {
            DataTableUI dtProdutosPendenciasCfg = new DataTableUI
            {
                Parent = "produtosPendenciasField",
                Id = "dtProdutosPendencias",
                UrlGridLoad = Url.Action("GetNFeImportacaoProdutosPendencia", "NFeImportacaoProduto"),
                UrlFunctions = Url.Action("Functions") + "?fns=",
                Options = new DataTableUIConfig
                {
                    PageLength = -1
                },
                Parameters = new List<DataTableUIParameter>
                {
                    new DataTableUIParameter { Id = "id", Required = true }
                },
                Callbacks = new DataTableUICallbacks()
                {
                    FooterCallback = "fnFooterCallbackProdutosPendencias",
                    DrawCallback = "fnHidePaginate"
                },
                Functions = new List<string>() { "fnFooterCallbackProdutosPendencias", "fnHidePaginate" }
            };

            dtProdutosPendenciasCfg.Columns.Add(new DataTableUIColumn() { DataField = "descricao", DisplayName = "Produto", Priority = 1 });
            dtProdutosPendenciasCfg.Columns.Add(new DataTableUIColumn() { DataField = "codigoBarras", DisplayName = "GTIN", Priority = 1 });
            dtProdutosPendenciasCfg.Columns.Add(new DataTableUIColumn() { DataField = "quantidade", DisplayName = "Quantidade", Priority = 2, Type = "float" });
            dtProdutosPendenciasCfg.Columns.Add(new DataTableUIColumn() { DataField = "valor", DisplayName = "Valor", Priority = 3, Type = "float" });

            return dtProdutosPendenciasCfg;
        }

        protected DataTableUI GetDtProdutosResolvidosfg()
        {
            DataTableUI dtProdutosResolvidosCfg = new DataTableUI
            {
                Parent = "produtosResolvidosField",
                Id = "dtProdutosResolvidos",
                UrlGridLoad = Url.Action("GetNFeImportacaoProdutos", "NFeImportacaoProduto"),
                UrlFunctions = Url.Action("Functions") + "?fns=",
                Functions = new List<string> { "fnRenderCheck", "fnRenderButton", "fnRenderText" },
                Parameters = new List<DataTableUIParameter>
                {
                    new DataTableUIParameter { Id = "id", Required = true }
                },
            };

            dtProdutosResolvidosCfg.Columns.Add(new DataTableUIColumn() { DataField = "descricao", DisplayName = "Produto", Priority = 1 });
            dtProdutosResolvidosCfg.Columns.Add(new DataTableUIColumn() { DataField = "quantidadeString", DisplayName = "Quant.", Priority = 2, Type = "float", Searchable = false, Orderable = false });
            dtProdutosResolvidosCfg.Columns.Add(new DataTableUIColumn() { DataField = "valorString", DisplayName = "Valor", Priority = 3, Type = "float", Searchable = false, Orderable = false });
            dtProdutosResolvidosCfg.Columns.Add(new DataTableUIColumn() { DataField = "valorVenda", DisplayName = "Valor Venda", Priority = 4, Searchable = false, Orderable = false, RenderFn = "fnValorVenda", Class = "dt-center" });
            dtProdutosResolvidosCfg.Columns.Add(new DataTableUIColumn() { DataField = "unidadeMedidaAbreviacao", DisplayName = "Saldo Estoque", Priority = 5, Type = "float", Searchable = false, Orderable = false });

            dtProdutosResolvidosCfg.Columns.Add(new DataTableUIColumn() { DataField = "movimentaEstoque", DisplayName = "Movimentar Estoque", Priority = 6,  Searchable = false, Orderable = false, RenderFn = "fnMovimentaEstoque", Class = "dt-center" });
            dtProdutosResolvidosCfg.Columns.Add(new DataTableUIColumn() { DataField = "atualizaDadosProduto", DisplayName = "Atualizar Produto", Priority = 7, Searchable = false, Orderable = false, RenderFn = "fnAtualizaProduto", Class = "dt-center" });
            dtProdutosResolvidosCfg.Columns.Add(new DataTableUIColumn() { DataField = "atualizaValorVenda", DisplayName = "Atualizar Valor Venda", Priority = 8, Searchable = false, Orderable = false, RenderFn = "fnAtualizaVlCompras", Class = "dt-center" });

            dtProdutosResolvidosCfg.Columns.Add(new DataTableUIColumn() { DataField = "excluirItem", DisplayName = "", Priority = 9, Searchable = false, Orderable = false, RenderFn = "fnRenderButton", Class = "dt-center" });


            
            return dtProdutosResolvidosCfg;
        }

        public ContentResult ModalAlterarProdutos()
        {
            ModalUIForm config = new ModalUIForm()
            {
                Title = "Alterar valor de venda",
                UrlFunctions = @Url.Action("Functions") + "?fns=",
                ConfirmAction = new ModalUIAction() { Label = "Salvar" },
                CancelAction = new ModalUIAction() { Label = "Cancelar" },
                Action = new FormUIAction
                {
                    Create = @Url.Action("Create"),
                    Edit = @Url.Action("Edit"),
                    Get = @Url.Action("Json") + "/"
                },
                Id = "fly01mdlfrmProdutoValorVendas",
                //ReadyFn = "fnFormReadyModalKitItens"
            };

            config.Elements.Add(new ButtonGroupUI()
            {
                Id = "fly01btngrpFinalidade",
                Class = "col s12 m12",
                OnClickFn = "fnChangeFinalidade",
                Label = "Tipo do pedido",
                Options = new List<ButtonGroupOptionUI>
                {
                    new ButtonGroupOptionUI { Id = "btnPercent", Label = "% Percentual"},
                    new ButtonGroupOptionUI { Id = "btnValor", Label = "Valor"},
                }
            });

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
    }
}
