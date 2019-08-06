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
using Fly01.Compras.ViewModel;
using Fly01.Core.Presentation.JQueryDataTable;

namespace Fly01.Compras.Controllers
{
    [OperationRole(ResourceKey = ResourceHashConst.ComprasComprasNFeImportacao)]
    public class NFeImportacaoController : BaseController<NFeImportacaoVM>
    {
        public NFeImportacaoController()
        {
            ExpandProperties = "fornecedor($select=nome),transportadora($select=nome),condicaoParcelamento,formaPagamento,categoria,centroCusto";
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

        public override Dictionary<string, string> GetQueryStringDefaultGridLoad()
        {
            var queryString = base.GetQueryStringDefaultGridLoad();
            queryString.Add("$select", "id,status,valorTotal,dataEmissao,serie,numero");
            return queryString;
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
                new DataTableUIAction { OnClickFn = "fnEditar", Label = "Editar" , ShowIf = "(row.status != 'Finalizado')" },
                new DataTableUIAction { OnClickFn = "fnVisualizar", Label = "Visualizar" },
                new DataTableUIAction { OnClickFn = "fnExcluirNFeImportacao", Label = "Excluir" },
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

        public override List<HtmlUIButton> GetFormButtonsOnHeader()
        {
            var target = new List<HtmlUIButton>();

            if (UserCanWrite)
            {
                target.Add(new HtmlUIButton { Id = "btnCancelar", Label = "Cancelar", OnClickFn = "fnCancelar" });
            }

            return target;
        }

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
                        Quantity = 3,
                    },
                    new FormWizardUIStep()
                    {
                        Title = "Produtos",
                        Id = "stepProdutos",
                        Quantity = 3,
                    },
                    new FormWizardUIStep()
                    {
                        Title = "Financeiro",
                        Id = "stepFinanceiro",
                        Quantity = 9,
                    },
                    new FormWizardUIStep()
                    {
                        Title = "Resumo/Finalizar",
                        Id = "stepResumoFinalizar",
                        Quantity = 10,
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
            config.Elements.Add(new InputCpfcnpjUI { Id = "fornecedorCnpjXml", Class = "col s12 m6", Label = "CNPJ", MaxLength = 60, Readonly = true });
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


            config.Elements.Add(new LabelSetUI { Id = "labelSetTransportadora", Class = "col s12", Label = "Dados Transportadora XML" });
            config.Elements.Add(new InputTextUI { Id = "transportadoraRazaoSocialXml", Class = "col s12 m6", Label = "Nome", Readonly = true });
            config.Elements.Add(new InputCpfcnpjUI { Id = "transportadorCNPJXml", Class = "col s12 m6", Label = "CNPJ", MaxLength = 60, Readonly = true });
            config.Elements.Add(new InputTextUI { Id = "transportadoraInscEstadualXml", Class = "col s12 m6", Label = "Inscrição Estadual", Readonly = true });
            config.Elements.Add(new InputTextUI { Id = "transportadoraUFXml", Class = "col s12 m6", Label = "UF", Readonly = true });

            #endregion

            #region step Produtos Pendências
            config.Elements.Add(new DivElementUI { Id = "infoProdutosPendencias", Class = "col s12 text-justify visible", Label = "Informações" });
            config.Elements.Add(new ButtonUI
            {
                Id = "salvarTodos",
                Class = "col s12 m3 right",
                ClassBtn = "btn right",
                Label = "",
                Value = "Salvar Todos",
                OnClickFn = "fnSalvarTodosProdutos"
            });
            config.Elements.Add(new DivElementUI { Id = "produtosPendencias", Class = "col s12 visible" });
            #endregion

            #region step Produtos
            config.Elements.Add(new ButtonUI
            {
                Id = "alterarSelecionados",
                Class = "col s12 m3 right",
                ClassBtn = "btn-narrow",
                Label = "",
                Value = "Alterar valor venda",
                DomEvents = new List<DomEventUI>
                {
                    new DomEventUI { DomEvent = "click", Function = "fnModalAlterarProdutos" }
                }
            });
            config.Elements.Add(new DivElementUI { Id = "infoProdutos" });

            config.Elements.Add(new DivElementUI { Id = "produtosResolvidos", Class = "col s12 visible" });

            #endregion

            #region step Financeiro
            config.Elements.Add(new InputCheckboxUI
            {
                Id = "geraFinanceiro",
                Class = "col s12 m4",
                Label = "Gerar financeiro",
                DomEvents = new List<DomEventUI>
                {
                    new DomEventUI { DomEvent = "change", Function = "fnValidaCamposGeraFinanceiro" }
                }
            });
            config.Elements.Add(new InputCheckboxUI
            {
                Id = "geraContasXml",
                Class = "col s12 m4",
                Label = "Gerar contas xml",
                DomEvents = new List<DomEventUI>
                {
                    new DomEventUI { DomEvent = "change", Function = "fnChkGeraContasXml" }
                }
            });
            config.Elements.Add(ElementUIHelper.GetAutoComplete(new AutoCompleteUI
            {
                Id = "formaPagamentoId",
                Class = "col s12 m4",
                Label = "Forma Pagamento",
                DataUrl = Url.Action("FormaPagamento", "AutoComplete"),
                LabelId = "formaPagamentoDescricao",
                DataUrlPostModal = Url.Action("FormModal", "FormaPagamento"),
                DataPostField = "descricao"
            }, ResourceHashConst.ComprasCadastrosFormaPagamento));

            config.Elements.Add(ElementUIHelper.GetAutoComplete(new AutoCompleteUI
            {
                Id = "condicaoParcelamentoId",
                Class = "col s12 m4",
                Label = "Condição Parcelamento",
                DataUrl = Url.Action("CondicaoParcelamento", "AutoComplete"),
                LabelId = "condicaoParcelamentoDescricao",
                DataUrlPostModal = Url.Action("FormModal", "CondicaoParcelamento"),
                DataPostField = "descricao"
            }, ResourceHashConst.ComprasCadastrosCondicoesParcelamento));

            config.Elements.Add(ElementUIHelper.GetAutoComplete(new AutoCompleteUI
            {
                Id = "categoriaId",
                Class = "col s12 m4",
                Label = "Categoria",
                PreFilter = "tipoCarteira",
                DataUrl = @Url.Action("Categoria", "AutoComplete"),
                LabelId = "categoriaDescricao",
                DataUrlPost = @Url.Action("NovaCategoriaDespesa")
            }, ResourceHashConst.ComprasCadastrosCategoria));

            config.Elements.Add(ElementUIHelper.GetAutoComplete(new AutoCompleteUI
            {
                Id = "centroCustoId",
                Class = "col s12 m4",
                Label = "Centro de Custo",
                DataUrl = Url.Action("CentroCusto", "AutoComplete"),
                LabelId = "centroCustoDescricao",
                DataUrlPostModal = Url.Action("FormModal", "CentroCusto"),
                DataPostField = "descricao"
            }, ResourceHashConst.ComprasCadastrosCentroCustos));

            config.Elements.Add(new InputDateUI { Id = "dataVencimento", Class = "col s12 m4", Label = "Data Vencimento" });
            config.Elements.Add(new LabelSetUI { Id = "labelSetCobrancas", Label = "Cobranças importadas XML", Class = "col s12" });
            config.Elements.Add(new DivElementUI { Id = "cobrancas", Class = "col s12 visible" });
            #endregion

            #region step Finalizar

            config.Elements.Add(new LabelSetUI { Id = "labelImpostos", Class = "col s12", Label = "Impostos" });
            config.Elements.Add(new InputCurrencyUI { Id = "somatorioICMSST", Class = "col s12 m4", Label = "ICMSST", Readonly = true });
            config.Elements.Add(new InputCurrencyUI { Id = "somatorioIPI", Class = "col s12 m4", Label = "IPI", Readonly = true });
            config.Elements.Add(new InputCurrencyUI { Id = "somatorioFCPST", Class = "col s12 m4", Label = "FCPST", Readonly = true });

            config.Elements.Add(new LabelSetUI { Id = "labelValores", Class = "col s12", Label = "Valores" });
            config.Elements.Add(new InputCurrencyUI { Id = "valorFrete", Class = "col s12 m4", Label = "Valor Frete", Readonly = true });
            config.Elements.Add(new InputCurrencyUI { Id = "somatorioDesconto", Class = "col s12 m4", Label = "Descontos", Readonly = true });
            config.Elements.Add(new InputCurrencyUI { Id = "valorTotal", Class = "col s12 m4", Label = "Valor Total", Readonly = true });

            config.Elements.Add(new InputCheckboxUI
            {
                Id = "novoPedido",
                Class = "col s12 m4 l4",
                Label = "Incluir novo pedido"
            });
            config.Elements.Add(new InputCheckboxUI { Id = "finalizarImportacao", Class = "col s12 m4", Label = "Salvar e Finalizar" });

            #endregion

            #region Helpers
            config.Helpers.Add(new TooltipUI
            {
                Id = "novoPedido",
                Tooltip = new HelperUITooltip()
                {
                    Text = "Se marcar Incluir novo Pedido, será gerado um novo Pedido com o status de Finalizado."
                }
            });
            config.Helpers.Add(new TooltipUI
            {
                Id = "finalizarImportacao",
                Tooltip = new HelperUITooltip()
                {
                    Text = "Resolva todas as pendências para finalizar a importação. Ao finalizar serão efetivadas as opções marcadas (Gerar financeiro, movimentar estoque, novo fornecedor/transportadora, atualização dos dados dos cadastros). Não será mais possível editar."
                }
            });
            #endregion

            cfg.Content.Add(config);
            cfg.Content.Add(GetDtProdutosPendenciasCfg());
            cfg.Content.Add(GetDtProdutosResolvidosCfg());
            cfg.Content.Add(GetDtCobrancasCfg());
            return cfg;
        }

        public ContentResult FormNFeImportacao(bool isEdit = false)
            => Content(JsonConvert.SerializeObject(FormNFeImportacaoJson(isEdit), JsonSerializerSetting.Front), "application/json");

        protected NFeImportacaoFormVM GetNFeImportacaoForm(Guid id)
        {
            string resourceName = "nfeimportacaovinculacao";
            string resourceById = String.Format("{0}?id={1}", resourceName, id);

            var response = RestHelper.ExecuteGetRequest<NFeImportacaoFormVM>(resourceById);
            return response;
        }

        protected NFeVM DeserializeXmlToNFe(string xml)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(Base64Helper.DecodificaBase64(xml));

            XmlElement xelRoot = doc.DocumentElement;
            XmlNode tagNFe = xelRoot.FirstChild;
            if (tagNFe.Name == "NFe")
            {
                XmlSerializer ser = new XmlSerializer(typeof(NFeVM));
                StringReader sr = new StringReader(tagNFe.OuterXml);
                return (NFeVM)ser.Deserialize(sr);
            }
            return null;
        }

        public override ContentResult Json(Guid id)
        {
            try
            {
                var entity = GetNFeImportacaoForm(id);

                var NFe = DeserializeXmlToNFe(entity.XML);
                if (NFe != null && NFe.InfoNFe != null)
                {
                    if (NFe.InfoNFe.Emitente != null)
                    {
                        entity.FornecedorNomeXml = NFe.InfoNFe.Emitente?.Nome ?? NFe.InfoNFe.Emitente?.NomeFantasia;
                        entity.FornecedorCnpjXml = NFe.InfoNFe.Emitente?.Cnpj;
                        entity.FornecedorRazaoSocialXml = NFe.InfoNFe.Emitente?.NomeFantasia ?? NFe.InfoNFe.Emitente?.Nome;
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
                NFeImportacaoFormVM postResult = JsonConvert.DeserializeObject<NFeImportacaoFormVM>(postResponse);
                var NFe = DeserializeXmlToNFe(postResult.XML);
                var hasTagTransportadora = (NFe != null && NFe.InfoNFe != null && NFe.InfoNFe.Transporte != null && NFe.InfoNFe.Transporte.Transportadora != null && NFe.InfoNFe.Transporte.Transportadora?.RazaoSocial != null);

                var response = new JsonResult
                {
                    Data = new { success = true, message = AppDefaults.EditSuccessMessage, id = postResult.Id.ToString(), tipoFrete = postResult.TipoFrete.ToString(), hasTagTransportadora = hasTagTransportadora }
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

        public ContentResult Visualizar()
        {
            ModalUIForm config = new ModalUIForm()
            {
                Title = "Visualizar",
                UrlFunctions = @Url.Action("Functions", "NFeImportacao") + "?fns=",
                CancelAction = new ModalUIAction() { Label = "Cancelar" },
                Action = new FormUIAction
                {
                    Create = @Url.Action("Create"),
                    Edit = @Url.Action("Edit"),
                    Get = @Url.Action("Json") + "/",
                    List = @Url.Action("List")
                },
                Functions = new List<string>{ "fnFormReadyVisualizarImportacao" },
                ReadyFn = "fnFormReadyVisualizarImportacao",
                Id = "fly01mdlfrmVisualizarImportacao"
            };

            config.Elements.Add(new InputHiddenUI { Id = "id" });

            #region Fornecedor
            config.Elements.Add(new LabelSetUI { Id = "labelSetFornecedor", Class = "col s12", Label = "Dados Fornecedor XML" });
            config.Elements.Add(ElementUIHelper.GetAutoComplete(new AutoCompleteUI
            {
                Id = "fornecedorId",
                Class = "col s12 m6",
                Label = "Fornecedor",
                DataUrl = Url.Action("FornecedorXML", "AutoComplete"),
                LabelId = "fornecedorNome",
                DataUrlPost = Url.Action("PostFornecedor"),
                Readonly = true
            }, null));
            config.Elements.Add(new InputTextUI { Id = "fornecedorNomeXml", Class = "col s12 m6", Label = "Nome", MaxLength = 60, Readonly = true });
            config.Elements.Add(new InputCpfcnpjUI { Id = "fornecedorCnpjXml", Class = "col s12 m6", Label = "CNPJ", MaxLength = 60, Readonly = true });
            config.Elements.Add(new InputTextUI { Id = "fornecedorInscEstadualXml", Class = "col s12 m6", Label = "Inscrição estadual", MaxLength = 60, Readonly = true });
            config.Elements.Add(new InputTextUI { Id = "fornecedorRazaoSocialXml", Class = "col s12 m6", Label = "Razão social", MaxLength = 60, Readonly = true });
            #endregion

            #region Transportadora
            config.Elements.Add(new LabelSetUI { Id = "labelSetTransportadora", Class = "col s12", Label = "Dados Transportadora XML" });
            config.Elements.Add(ElementUIHelper.GetAutoComplete(new AutoCompleteUI
            {
                Id = "transportadoraId",
                Class = "col s12 m6",
                Label = "Transportadora",
                DataUrl = Url.Action("TransportadoraXML", "AutoComplete"),
                LabelId = "transportadoraNome",
                DataUrlPost = Url.Action("PostTransportadora"),
                Readonly = true
            }, null));

            config.Elements.Add(new InputTextUI { Id = "transportadoraRazaoSocialXml", Class = "col s12 m6", Label = "Nome", Readonly = true });
            config.Elements.Add(new InputCpfcnpjUI { Id = "transportadorCNPJXml", Class = "col s12 m6", Label = "CNPJ", MaxLength = 60, Readonly = true });
            config.Elements.Add(new InputTextUI { Id = "transportadoraInscEstadualXml", Class = "col s12 m6", Label = "Inscrição Estadual", Readonly = true });
            config.Elements.Add(new InputTextUI { Id = "transportadoraUFXml", Class = "col s12 m6", Label = "UF", Readonly = true });
            #endregion

            #region Produtos

            config.Elements.Add(new TableUI
            {
                Id = "produtosImportacaoDataTable",
                Class = "col s12",
                Disabled = true,
                Options = new List<OptionUI>
                {
                    new OptionUI { Label = "Produto", Value = "0"},
                    new OptionUI { Label = "Quant.", Value = "1"},
                    new OptionUI { Label = "Un.", Value = "1"},
                    new OptionUI { Label = "Valor",Value = "2"},
                    new OptionUI { Label = "Valor Venda" ,Value = "2"},
                    new OptionUI { Label = "Movimenta Estoque",Value = "3"},
                    new OptionUI { Label = "Atualiza Produto",Value = "5"},
                    new OptionUI { Label = "Atualiza Valor Venda",Value = "4"},
                }
            });

            #endregion

            #region Financeiro
            config.Elements.Add(new LabelSetUI { Id = "labelSetCobrancas", Label = "Cobranças importadas XML", Class = "col s12" });
            config.Elements.Add(ElementUIHelper.GetAutoComplete(new AutoCompleteUI
            {
                Id = "formaPagamentoId",
                Class = "col s12 m4",
                Label = "Forma Pagamento",
                DataUrl = Url.Action("FormaPagamento", "AutoComplete"),
                LabelId = "formaPagamentoDescricao",
                DataUrlPostModal = Url.Action("FormModal", "FormaPagamento"),
                DataPostField = "descricao",
                Readonly = true
            }, ResourceHashConst.ComprasCadastrosFormaPagamento));

            config.Elements.Add(ElementUIHelper.GetAutoComplete(new AutoCompleteUI
            {
                Id = "condicaoParcelamentoId",
                Class = "col s12 m4",
                Label = "Condição Parcelamento",
                DataUrl = Url.Action("CondicaoParcelamento", "AutoComplete"),
                LabelId = "condicaoParcelamentoDescricao",
                DataUrlPostModal = Url.Action("FormModal", "CondicaoParcelamento"),
                DataPostField = "descricao",
                Readonly = true
            }, ResourceHashConst.ComprasCadastrosCondicoesParcelamento));

            config.Elements.Add(ElementUIHelper.GetAutoComplete(new AutoCompleteUI
            {
                Id = "categoriaId",
                Class = "col s12 m4",
                Label = "Categoria",
                PreFilter = "tipoCarteira",
                DataUrl = @Url.Action("Categoria", "AutoComplete"),
                LabelId = "categoriaDescricao",
                DataUrlPost = @Url.Action("NovaCategoriaDespesa"),
                Readonly = true
            }, ResourceHashConst.ComprasCadastrosCategoria));
            config.Elements.Add(new InputDateUI { Id = "dataVencimento", Class = "col s12 m4", Label = "Data Vencimento", Readonly = true, Disabled = true });
            config.Elements.Add(new DivElementUI { Id = "cobrancas", Class = "col s12 visible" });

            config.Elements.Add(new TableUI
            {
                Id = "cobrancasImportacaoDataTable",
                Class = "col s12",
                Disabled = true,
                Options = new List<OptionUI>
                {
                    new OptionUI { Label = "Número", Value = "0"},
                    new OptionUI { Label = "Valor.", Value = "1"},
                    new OptionUI { Label = "Vencimento", Value = "1"},
                }
            });
            #endregion


            return Content(JsonConvert.SerializeObject(config, JsonSerializerSetting.Front), "application/json");
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
                    WithoutRowMenu = true,
                    PageLength = -1,
                    NoExportButtons = true,
                },
                Parameters = new List<DataTableUIParameter>
                {
                    new DataTableUIParameter { Id = "id", Required = true }
                },
                Callbacks = new DataTableUICallbacks()
                {
                    FooterCallback = "fnFooterCallbackProdutosPendencias",
                    DrawCallback = "fnDrawCallbackProdutosPendencias"
                },
                Functions = new List<string>() { "fnFooterCallbackProdutosPendencias", "fnDrawCallbackProdutosPendencias" }
            };
            dtProdutosPendenciasCfg.Columns.Add(new DataTableUIColumn() { Priority = 1, Searchable = false, Orderable = false, DataField = "descricao", DisplayName = "Produto" });
            dtProdutosPendenciasCfg.Columns.Add(new DataTableUIColumn() { Priority = 2, Searchable = false, Orderable = false, DataField = "codigoBarras", DisplayName = "GTIN" });
            dtProdutosPendenciasCfg.Columns.Add(new DataTableUIColumn() { Priority = 5, Searchable = false, Orderable = false, DataField = "quantidade", DisplayName = "Quant.", Type = "float" });
            dtProdutosPendenciasCfg.Columns.Add(new DataTableUIColumn() { Priority = 4, Searchable = false, Orderable = false, DataField = "valor", DisplayName = "Valor", Type = "float" });
            dtProdutosPendenciasCfg.Columns.Add(new DataTableUIColumn() { Priority = 6, Searchable = false, Orderable = false, DataField = "unidadeMedida_abreviacao", DisplayName = "Un." });
            dtProdutosPendenciasCfg.Columns.Add(new DataTableUIColumn() { Priority = 2, Searchable = false, Orderable = false, RenderFn = "fnRenderSalvarProdutoPendencia", DisplayName = "Vincular Produtos", Class = "dt-center", Width = "35%" });
            dtProdutosPendenciasCfg.Columns.Add(new DataTableUIColumn() { Priority = 3, Searchable = false, Orderable = false, RenderFn = "fnRenderExcluirProdutoPendencia", Class = "dt-center" });

            return dtProdutosPendenciasCfg;
        }

        protected DataTableUI GetDtProdutosPendenciasConfig()
        {
            DataTableUI dtProdutosPendenciasCfg = new DataTableUI
            {
                Parent = "produtosPendenciasField",
                Id = "dtProdutosPendencias",
                UrlGridLoad = Url.Action("GetNFeImportacaoProdutosPendencia", "NFeImportacaoProduto"),
                UrlFunctions = Url.Action("Functions") + "?fns=",
                Options = new DataTableUIConfig
                {
                    WithoutRowMenu = true,
                    PageLength = -1,
                    NoExportButtons = true,
                },
                Parameters = new List<DataTableUIParameter>
                {
                    new DataTableUIParameter { Id = "id", Required = true }
                },
                Callbacks = new DataTableUICallbacks()
                {
                    FooterCallback = "fnFooterCallbackProdutosPendencias",
                    DrawCallback = "fnDrawCallbackProdutosPendencias"
                },
                Functions = new List<string>() { "fnFooterCallbackProdutosPendencias", "fnDrawCallbackProdutosPendencias" }
            };
            dtProdutosPendenciasCfg.Columns.Add(new DataTableUIColumn() { Priority = 1, Searchable = false, Orderable = false, DataField = "descricao", DisplayName = "Produto" });
            dtProdutosPendenciasCfg.Columns.Add(new DataTableUIColumn() { Priority = 2, Searchable = false, Orderable = false, DataField = "codigoBarras", DisplayName = "GTIN" });
            dtProdutosPendenciasCfg.Columns.Add(new DataTableUIColumn() { Priority = 5, Searchable = false, Orderable = false, DataField = "quantidade", DisplayName = "Quant.", Type = "float" });
            dtProdutosPendenciasCfg.Columns.Add(new DataTableUIColumn() { Priority = 4, Searchable = false, Orderable = false, DataField = "valor", DisplayName = "Valor", Type = "float" });
            dtProdutosPendenciasCfg.Columns.Add(new DataTableUIColumn() { Priority = 6, Searchable = false, Orderable = false, DataField = "unidadeMedida_abreviacao", DisplayName = "Un." });
            dtProdutosPendenciasCfg.Columns.Add(new DataTableUIColumn() { Priority = 2, Searchable = false, Orderable = false, RenderFn = "fnRenderSalvarProdutoPendencia", DisplayName = "Vincular Produtos", Class = "dt-center", Width = "35%" });
            dtProdutosPendenciasCfg.Columns.Add(new DataTableUIColumn() { Priority = 3, Searchable = false, Orderable = false, RenderFn = "fnRenderExcluirProdutoPendencia", Class = "dt-center" });

            return dtProdutosPendenciasCfg;
        }

        protected DataTableUI GetDtProdutosResolvidosCfg()
        {
            DataTableUI dtProdutosResolvidosCfg = new DataTableUI
            {
                Parent = "produtosResolvidosField",
                Id = "dtProdutosResolvidos",
                UrlGridLoad = Url.Action("GetNFeImportacaoProdutos", "NFeImportacaoProduto"),
                UrlFunctions = Url.Action("Functions") + "?fns=",
                Functions = new List<string> { "fnRenderCheck", "fnRenderButtonExcluirProdutos", "fnRenderText", "fnFooterCallbackProdutos" },
                Parameters = new List<DataTableUIParameter>
                {
                    new DataTableUIParameter { Id = "id", Required = true }
                },
                Callbacks = new DataTableUICallbacks
                {
                    DrawCallback = "fnDrawCallback",
                    FooterCallback = "fnFooterCallbackProdutos",
                },
                Options = new DataTableUIConfig
                {
                    WithoutRowMenu = true,
                    PageLength = -1,
                    NoExportButtons = true
                }
            };

            dtProdutosResolvidosCfg.Columns.Add(new DataTableUIColumn() { DataField = "descricao", DisplayName = "Produto", Searchable = false, Orderable = false, Priority = 1 });
            dtProdutosResolvidosCfg.Columns.Add(new DataTableUIColumn() { DataField = "quantidade", DisplayName = "Quant.", Priority = 2, Type = "float", Searchable = false, Orderable = false });
            dtProdutosResolvidosCfg.Columns.Add(new DataTableUIColumn() { Priority = 3, Searchable = false, Orderable = false, DataField = "unidadeMedida_abreviacao", DisplayName = "Un." });
            dtProdutosResolvidosCfg.Columns.Add(new DataTableUIColumn() { DataField = "valor", DisplayName = "Valor", Priority = 4, Type = "float", Searchable = false, Orderable = false });
            dtProdutosResolvidosCfg.Columns.Add(new DataTableUIColumn() { DataField = "valorVenda", DisplayName = "Valor Venda", Priority = 5, Searchable = false, Orderable = false, RenderFn = "fnValorVenda", Class = "dt-center" });

            dtProdutosResolvidosCfg.Columns.Add(new DataTableUIColumn() { DataField = "movimentaEstoque", DisplayName = "Movimentar Estoque", Priority = 6, Searchable = false, Orderable = false, RenderFn = "fnMovimentaEstoque", Class = "dt-center" });
            dtProdutosResolvidosCfg.Columns.Add(new DataTableUIColumn() { DataField = "atualizaDadosProduto", DisplayName = "Atualizar Produto", Priority = 7, Searchable = false, Orderable = false, RenderFn = "fnAtualizaProduto", Class = "dt-center" });
            dtProdutosResolvidosCfg.Columns.Add(new DataTableUIColumn() { DataField = "atualizaValorVenda", DisplayName = "Atualizar Valor Venda", Priority = 8, Searchable = false, Orderable = false, RenderFn = "fnAtualizaVlCompras", Class = "dt-center" });

            dtProdutosResolvidosCfg.Columns.Add(new DataTableUIColumn() { DisplayName = "Excluir", Priority = 9, Searchable = false, Orderable = false, RenderFn = "fnRenderButtonExcluirProdutos", Class = "dt-center" });

            return dtProdutosResolvidosCfg;
        }

        protected DataTableUI GetDtCobrancasCfg()
        {
            DataTableUI dtCobrancasCfg = new DataTableUI
            {
                Parent = "cobrancasField",
                Id = "dtCobrancas",
                UrlGridLoad = Url.Action("GetNFeImportacaoCobrancas", "NFeImportacaocobranca"),
                UrlFunctions = Url.Action("Functions") + "?fns=",
                Parameters = new List<DataTableUIParameter>
                {
                    new DataTableUIParameter { Id = "id", Required = true }
                },
                Functions = new List<string> { "fnFooterCallbackCobrancas", "fnDrawCallbackCobrancas" },
                Callbacks = new DataTableUICallbacks
                {
                    FooterCallback = "fnFooterCallbackCobrancas",
                    DrawCallback = "fnDrawCallbackCobrancas"
                },
                Options = new DataTableUIConfig
                {
                    PageLength = -1,
                    NoExportButtons = true
                }
            };

            dtCobrancasCfg.Actions.Add(new DataTableUIAction() { Label = "Excluir", OnClickFn = "fnExcluirCobranca" });

            dtCobrancasCfg.Columns.Add(new DataTableUIColumn() { DataField = "numero", DisplayName = "Número", Searchable = false, Orderable = false, Priority = 3 });
            dtCobrancasCfg.Columns.Add(new DataTableUIColumn() { DataField = "valor", DisplayName = "Valor", Priority = 1, Type = "float", Searchable = false, Orderable = false });
            dtCobrancasCfg.Columns.Add(new DataTableUIColumn() { DataField = "dataVencimento", DisplayName = "Vencimento", Searchable = false, Orderable = false, Priority = 2, Type = "date" });

            return dtCobrancasCfg;
        }

        public ContentResult ModalAlterarProdutos()
        {
            ModalUIForm config = new ModalUIForm()
            {
                Title = "Alterar valor de venda",
                UrlFunctions = @Url.Action("Functions") + "?fns=",
                ConfirmAction = new ModalUIAction() { Label = "Salvar", OnClickFn = "fnSalvarProduto" },
                CancelAction = new ModalUIAction() { Label = "Cancelar" },
                Action = new FormUIAction
                {
                    //Create = @Url.Action("PostProdutos"),
                    //Edit = @Url.Action("Edit"),
                    Get = @Url.Action("Json") + "/"
                },
                Id = "fly01mdlfrmProdutoValorVendas",
                ReadyFn = "fnFormReadyModal",
            };

            config.Elements.Add(new ButtonGroupUI()
            {
                Id = "fly01btngrpFinalidade",
                Class = "col s12 m12",
                OnClickFn = "",
                Label = "Tipo do pedido",
                Options = new List<ButtonGroupOptionUI>
                {
                    new ButtonGroupOptionUI { Id = "btnPercent", Label = "% Percentual"},
                    new ButtonGroupOptionUI { Id = "btnValor", Label = "Valor"},
                }
            });

            config.Elements.Add(new InputCustommaskUI
            {
                Id = "percentualId",
                Class = "col s12 m6",
                Label = "Percentual",
                Data = new { inputmask = "'mask': '9{1,3}[,9{1,2}] %', 'alias': 'decimal', 'autoUnmask': true, 'suffix': ' %', 'radixPoint': ',' " }
            });

            config.Elements.Add(new InputCurrencyUI
            {
                Id = "valorId",
                Class = "col s12 m6",
                Label = "Valor"
            });
            return Content(JsonConvert.SerializeObject(config, JsonSerializerSetting.Front), "application/json");
        }

        #region OnDemand

        [HttpPost]
        public JsonResult NovaCategoriaDespesa(string term)
        {
            try
            {
                var resourceName = AppDefaults.GetResourceName(typeof(CategoriaVM));
                var data = RestHelper.ExecutePostRequest<CategoriaVM>(
                    resourceName,
                    new CategoriaVM { Descricao = term, TipoCarteira = "2" },
                    AppDefaults.GetQueryStringDefault()
                );

                return JsonResponseStatus.Get(new ErrorInfo() { HasError = false }, Operation.Create, data.Id);
            }
            catch (Exception ex)
            {
                var error = JsonConvert.DeserializeObject<ErrorInfo>(ex.Message);
                return JsonResponseStatus.GetFailure(error.Message);
            }
        }

        protected override List<JQueryDataTableParamsColumn> GetParamsColumns(string ResourceName = "")
        {
            throw new NotImplementedException();
        }

        #endregion

    }
}
