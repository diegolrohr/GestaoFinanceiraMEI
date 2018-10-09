﻿using Fly01.Core;
using Fly01.Core.Config;
using Fly01.Core.Entities.Domains.Enum;
using Fly01.Core.Helpers;
using Fly01.Core.Mensageria;
using Fly01.Core.Presentation;
using Fly01.Core.Presentation.Commons;
using Fly01.Core.Rest;
using Fly01.Core.ViewModels;
using Fly01.Core.ViewModels.Presentation.Commons;
using Fly01.OrdemServico.Helpers;
using Fly01.OrdemServico.Models.Reports;
using Fly01.OrdemServico.ViewModel;
using Fly01.uiJS.Classes;
using Fly01.uiJS.Classes.Elements;
using Fly01.uiJS.Defaults;
using Fly01.uiJS.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace Fly01.OrdemServico.Controllers
{
    public class OrdemServicoController : BaseController<OrdemServicoVM>
    {
        //private bool _novaOS;

        public OrdemServicoController()
        {
            ExpandProperties = "cliente($select=id,nome,email;$expand=cidade($select=nome),estado($select=sigla))";
        }

        private JsonResult GetJson(object data)
            => Json(data, JsonRequestBehavior.AllowGet);

        [OperationRole(PermissionValue = EPermissionValue.Read)]
        public List<OrdemServicoItemProdutoVM> GetObjetosManutencao(Guid id)
        {
            var queryString = new Dictionary<string, string>();
            queryString.AddParam("$filter", $"ordemServicoId eq {id}");
            queryString.AddParam("$expand", "produto");

            return RestHelper.ExecuteGetRequest<ResultBase<OrdemServicoItemProdutoVM>>("OrdemServicoItemProduto", queryString).Data;
        }

        [OperationRole(PermissionValue = EPermissionValue.Read)]
        public List<OrdemServicoItemProdutoVM> GetProdutos(Guid id)
        {
            var queryString = new Dictionary<string, string>();
            queryString.AddParam("$filter", $"ordemServicoId eq {id}");
            queryString.AddParam("$expand", "produto");

            return RestHelper.ExecuteGetRequest<ResultBase<OrdemServicoItemProdutoVM>>("OrdemServicoItemProduto", queryString).Data;
        }

        [OperationRole(PermissionValue = EPermissionValue.Read)]
        public List<OrdemServicoItemServicoVM> GetServicos(Guid id)
        {
            var queryString = new Dictionary<string, string>();
            queryString.AddParam("$filter", $"ordemServicoId eq {id}");
            queryString.AddParam("$expand", "servico");

            return RestHelper.ExecuteGetRequest<ResultBase<OrdemServicoItemServicoVM>>("OrdemServicoItemServico", queryString).Data;
        }

        protected DataTableUI GetDtOrdemServicoManutencaoCfg()
        {
            DataTableUI dtOrdemServicoManutencaoCfg = new DataTableUI
            {
                Parent = "ordemServicoManutencaoField",
                Id = "dtOrdemServicoManutencao",
                UrlGridLoad = Url.Action("GetOrdemServicoManutencao", "OrdemServicoManutencao"),
                UrlFunctions = Url.Action("Functions", "OrdemServicoManutencao") + "?fns=",
                Parameters = new List<DataTableUIParameter>
                {
                    new DataTableUIParameter { Id = "id", Required = true }
                }
            };

            dtOrdemServicoManutencaoCfg.Actions.AddRange(new List<DataTableUIAction>()
            {
                new DataTableUIAction { OnClickFn = "fnEditarOrdemServicoManutencao", Label = "Editar" },
                new DataTableUIAction { OnClickFn = "fnExcluirOrdemServicoManutencao", Label = "Excluir" }
            });

            dtOrdemServicoManutencaoCfg.Columns.Add(new DataTableUIColumn() { DataField = "produto_descricao", DisplayName = "Produto", Priority = 1, Searchable = false, Orderable = false });
            dtOrdemServicoManutencaoCfg.Columns.Add(new DataTableUIColumn() { DataField = "quantidade", DisplayName = "Quantidade", Priority = 2, Type = "float", Searchable = false, Orderable = false });
            dtOrdemServicoManutencaoCfg.Columns.Add(new DataTableUIColumn() { DataField = "total", DisplayName = "Total", Priority = 3, Type = "float", Searchable = false, Orderable = false });

            return dtOrdemServicoManutencaoCfg;
        }

        protected DataTableUI GetDtOrdemServicoItemProdutosCfg()
        {
            DataTableUI dtOrdemServicoItemProdutosCfg = new DataTableUI
            {
                Parent = "ordemServicoItemProdutosField",
                Id = "dtOrdemServicoItemProdutos",
                UrlGridLoad = Url.Action("GetOrdemServicoItemProdutos", "OrdemServicoItemProduto"),
                UrlFunctions = Url.Action("Functions", "OrdemServicoItemProduto") + "?fns=",
                Parameters = new List<DataTableUIParameter>
                {
                    new DataTableUIParameter { Id = "id", Required = true }
                }
            };

            dtOrdemServicoItemProdutosCfg.Actions.AddRange(new List<DataTableUIAction>()
            {
                new DataTableUIAction { OnClickFn = "fnEditarOrdemServicoItemProduto", Label = "Editar" },
                new DataTableUIAction { OnClickFn = "fnExcluirOrdemServicoItemProduto", Label = "Excluir" }
            });

            dtOrdemServicoItemProdutosCfg.Columns.Add(new DataTableUIColumn() { DataField = "produto_descricao", DisplayName = "Produto", Priority = 1, Searchable = false, Orderable = false });
            dtOrdemServicoItemProdutosCfg.Columns.Add(new DataTableUIColumn() { DataField = "quantidade", DisplayName = "Quantidade", Priority = 2, Type = "float", Searchable = false, Orderable = false });
            dtOrdemServicoItemProdutosCfg.Columns.Add(new DataTableUIColumn() { DataField = "valor", DisplayName = "Valor", Priority = 3, Type = "currency", Searchable = false, Orderable = false });
            dtOrdemServicoItemProdutosCfg.Columns.Add(new DataTableUIColumn() { DataField = "desconto", DisplayName = "Desconto", Priority = 4, Type = "currency", Searchable = false, Orderable = false });
            dtOrdemServicoItemProdutosCfg.Columns.Add(new DataTableUIColumn() { DataField = "total", DisplayName = "Total", Priority = 5, Type = "currency", Searchable = false, Orderable = false });

            return dtOrdemServicoItemProdutosCfg;
        }

        protected DataTableUI GetDtOrdemServicoItemServicosCfg()
        {
            DataTableUI dtOrdemServicoItemServicosCfg = new DataTableUI
            {
                Parent = "ordemServicoItemServicosField",
                Id = "dtOrdemServicoItemServicos",
                UrlGridLoad = Url.Action("GetOrdemServicoItemServicos", "OrdemServicoItemServico"),
                UrlFunctions = Url.Action("Functions", "OrdemServicoItemServico") + "?fns=",
                Parameters = new List<DataTableUIParameter>
                {
                    new DataTableUIParameter { Id = "id", Required = true }
                }
            };

            dtOrdemServicoItemServicosCfg.Actions.AddRange(new List<DataTableUIAction>()
            {
                new DataTableUIAction { OnClickFn = "fnEditarOrdemServicoItemServico", Label = "Editar" },
                new DataTableUIAction { OnClickFn = "fnExcluirOrdemServicoItemServico", Label = "Excluir" }
            });

            dtOrdemServicoItemServicosCfg.Columns.Add(new DataTableUIColumn() { DataField = "servico_descricao", DisplayName = "Serviço", Priority = 1, Searchable = false, Orderable = false });
            dtOrdemServicoItemServicosCfg.Columns.Add(new DataTableUIColumn() { DataField = "quantidade", DisplayName = "Quantidade", Priority = 2, Type = "float", Searchable = false, Orderable = false });
            dtOrdemServicoItemServicosCfg.Columns.Add(new DataTableUIColumn() { DataField = "valor", DisplayName = "Valor", Priority = 3, Type = "currency", Searchable = false, Orderable = false });
            dtOrdemServicoItemServicosCfg.Columns.Add(new DataTableUIColumn() { DataField = "desconto", DisplayName = "Desconto", Priority = 4, Type = "currency", Searchable = false, Orderable = false });
            dtOrdemServicoItemServicosCfg.Columns.Add(new DataTableUIColumn() { DataField = "total", DisplayName = "Total", Priority = 5, Type = "currency", Searchable = false, Orderable = false });

            return dtOrdemServicoItemServicosCfg;
        }

        public override Func<OrdemServicoVM, object> GetDisplayData()
        {
            return x => new
            {
                id = x.Id.ToString(),
                numero = x.Numero.ToString(),
                dataEmissao = x.DataEmissao.ToString("dd/MM/yyyy"),
                dataEntrega = x.DataEntrega.ToString("dd/MM/yyyy"),
                status = x.Status,
                statusDescription = EnumHelper.GetDescription(typeof(StatusOrdemServico), x.Status),
                statusCssClass = EnumHelper.GetCSS(typeof(StatusOrdemServico), x.Status),
                statusValue = EnumHelper.GetValue(typeof(StatusOrdemServico), x.Status),
                cliente_nome = x.Cliente.Nome,
                geraOrdemVenda = x.GeraOrdemVenda
            };
        }

        public override ContentResult List()
            => ListOrdemServico();

        public List<HtmlUIButton> GetListButtonsOnHeaderCustom(string buttonLabel, string buttonOnClick)
        {
            var target = new List<HtmlUIButton>();

            //if (UserCanWrite)
            //{
            target.Add(new HtmlUIButton { Id = "new", Label = "Novo", OnClickFn = "fnNovaOS", Position = HtmlUIButtonPosition.Main });
            target.Add(new HtmlUIButton { Id = "filterGrid1", Label = buttonLabel, OnClickFn = buttonOnClick });

            //}

            return target;
        }

        public override List<HtmlUIButton> GetFormButtonsOnHeader()
        {
            var target = new List<HtmlUIButton>();

            target.Add(new HtmlUIButton { Id = "cancel", Label = "Cancelar", OnClickFn = "fnCancelarNovaOrdem" });

            return target;
        }

        public ContentResult ListOrdemServico(string gridLoad = "GridLoad")
        {
            var buttonLabel = "Mostrar todos as ordens de serviço";
            var buttonOnClick = "fnRemoveFilter";

            if (Request.QueryString["action"] == "GridLoadNoFilter")
            {
                gridLoad = Request.QueryString["action"];
                buttonLabel = "Mostrar ordens de serviço do mês atual";
                buttonOnClick = "fnAddFilter";
            }

            var cfg = new ContentUI
            {
                History = new ContentUIHistory { Default = Url.Action("Index") },
                Header = new HtmlUIHeader
                {
                    Title = "Ordem de Serviço",
                    Buttons = new List<HtmlUIButton>(GetListButtonsOnHeaderCustom(buttonLabel, buttonOnClick))
                },
                UrlFunctions = Url.Action("Functions") + "?fns="
            };

            if (gridLoad == "GridLoad")
            {
                var cfgForm = new FormUI
                {
                    ReadyFn = "fnUpdateDataFinal",
                    UrlFunctions = Url.Action("Functions") + "?fns=",
                    Elements = new List<BaseUI>()
                    {
                        new PeriodPickerUI()
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
                        },
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

                cfg.Content.Add(cfgForm);
            }

            var config = new DataTableUI
            {
                UrlGridLoad = Url.Action(gridLoad),
                Parameters = new List<DataTableUIParameter>
                {
                    new DataTableUIParameter() {Id = "dataInicial", Required = (gridLoad == "GridLoad") },
                    new DataTableUIParameter() {Id = "dataFinal", Required = (gridLoad == "GridLoad") }
                },
                UrlFunctions = Url.Action("Functions") + "?fns=",
                Functions = { "fnRenderEnum", "fnExecutarOrdem", "fnExecutarOrdem", "fnCancelarOrdem", "fnConcluirOrdem" }
            };

            config.Actions.AddRange(new List<DataTableUIAction>()
            {
                new DataTableUIAction { OnClickFn = "fnVisualizar", Label = "Visualizar" },
                new DataTableUIAction { OnClickFn = "fnEditar", Label = "Editar", ShowIf = $"(row.status == '{StatusOrdemServico.EmAberto}' || row.status == '{StatusOrdemServico.EmAndamento}' || row.status == '{StatusOrdemServico.EmPreenchimento}')" },
                new DataTableUIAction { OnClickFn = "fnExcluir", Label = "Excluir", ShowIf = $"(row.status == '{StatusOrdemServico.EmAberto}' || row.status == '{StatusOrdemServico.EmPreenchimento}')" },
                new DataTableUIAction { OnClickFn = "fnImprimirOrdemServico", Label = "Imprimir", ShowIf = $"(row.status != '{StatusOrdemServico.EmPreenchimento}')" },
                new DataTableUIAction { OnClickFn = "fnEnviarEmailOS", Label = "Enviar por e-mail", ShowIf = $"(row.status != '{StatusOrdemServico.EmPreenchimento}')" },
                new DataTableUIAction { OnClickFn = "fnExecutarOrdem", Label = "Executar", ShowIf = $"(row.status == '{StatusOrdemServico.EmAberto}')" },
                new DataTableUIAction { OnClickFn = "fnCancelarOrdem", Label = "Cancelar", ShowIf = $"(row.status == '{StatusOrdemServico.EmAberto}' || row.status == '{StatusOrdemServico.EmAndamento}')" },
                new DataTableUIAction { OnClickFn = "fnConcluirOrdem", Label = "Concluir", ShowIf = $"(row.status == '{StatusOrdemServico.EmAndamento}' && !row.geraOrdemVenda)" },
                new DataTableUIAction { OnClickFn = "fnConcluirGerarOrdem", Label = "Concluir & Gerar Pedido de Venda", ShowIf = $"(row.status == '{StatusOrdemServico.EmAndamento}')" }
            });

            config.Columns.Add(new DataTableUIColumn { DataField = "numero", DisplayName = "Número OS", Priority = 1, Type = "numbers" });
            config.Columns.Add(new DataTableUIColumn
            {
                DataField = "status",
                DisplayName = "Status",
                Priority = 2,
                Options = new List<SelectOptionUI>(SystemValueHelper.GetUIElementBase(typeof(StatusOrdemServico))),
                RenderFn = "fnRenderEnum(full.statusCssClass, full.statusDescription)"
            });
            config.Columns.Add(new DataTableUIColumn { DataField = "cliente_nome", DisplayName = "Cliente", Priority = 3 });
            config.Columns.Add(new DataTableUIColumn { DataField = "dataEmissao", DisplayName = "Data de Emissão", Priority = 4, Type = "date" });
            config.Columns.Add(new DataTableUIColumn { DataField = "dataEntrega", DisplayName = "Data de Entrega", Priority = 5, Type = "date" });

            cfg.Content.Add(config);

            return Content(JsonConvert.SerializeObject(cfg, JsonSerializerSetting.Front), "application/json");
        }

        [OperationRole(PermissionValue = EPermissionValue.Read)]
        public JsonResult EnviaEmail(string id)
        {
            try
            {
                var empresa = GetDadosEmpresa();
                var ordem = Get(Guid.Parse(id));

                var ResponseError = ValidarDadosEmail(id, empresa, ordem);
                if (ResponseError != null) return ResponseError; // tem que arrumar uma solução para o retorno nulo

                MailSend(empresa, ordem);

                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var error = JsonConvert.DeserializeObject<ErrorInfo>(ex.Message);
                return JsonResponseStatus.GetFailure(error.Message);
            }
        }

        private JsonResult ValidarDadosEmail(string id, ManagerEmpresaVM empresa, OrdemServicoVM pedido)
        {

            if (pedido.Cliente == null) return JsonResponseStatus.GetFailure("Nenhum Cliente foi encontrado.");
            if (string.IsNullOrEmpty(pedido.Cliente.Email)) return JsonResponseStatus.GetFailure("Não foi encontrado um email válido para este Cliente.");
            if (string.IsNullOrEmpty(empresa.Email)) return JsonResponseStatus.GetFailure("Você ainda não configurou um email válido para sua empresa.");

            return null;
        }

        private void MailSend(ManagerEmpresaVM empresa, OrdemServicoVM ordem)
        {
            var anexo = File(GetPDFFile(ordem), "application/pdf");
            var mensagemPrincipal = "VOCÊ ESTÁ RECEBENDO UMA CÓPIA DA SUA ORDEM DE SERVIÇO.";
            var tituloEmail = $"{empresa.NomeFantasia} ORDEM DE SERVIÇO - Nº {ordem.Numero}".ToUpper();
            var conteudoEmail = Mail.FormataMensagem(EmailFilesHelper.GetTemplate("Templates.OrdemServico.html").Value, tituloEmail, mensagemPrincipal, empresa.Email);
            var arquivoAnexo = new FileStreamResult(new MemoryStream(anexo.FileContents), anexo.ContentType);

            Mail.Send(empresa.NomeFantasia, ordem.Cliente.Email, tituloEmail, conteudoEmail, arquivoAnexo.FileStream);
        }

        public ContentResult FormOrdemServico(bool isEdit = false)
            => Content(JsonConvert.SerializeObject(FormOrdemServicoJson(isEdit), JsonSerializerSetting.Front), "application/json");


        public ContentUI FormOrdemServicoJson(bool isEdit = false)
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
                    Title = "Ordem de Serviços",
                    Buttons = new List<HtmlUIButton>(GetFormButtonsOnHeader())
                },
                UrlFunctions = Url.Action("Functions") + "?fns="
            };

            var config = new FormWizardUI
            {
                Action = new FormUIAction
                {
                    Create = @Url.Action("Create"),
                    Edit = @Url.Action("Edit"),
                    Get = @Url.Action("Json") + "/",
                    List = @Url.Action("List", "OrdemServico")
                },
                ReadyFn = "fnFormReadyOrdemServico",
                UrlFunctions = Url.Action("Functions") + "?fns=",
                Functions = new List<string> { "fnChangeEstado" },
                Steps = new List<FormWizardUIStep>()
                {
                    new FormWizardUIStep()
                    {
                        Title = "Cadastro",
                        Id = "stepCadastro",
                        Quantity = 8,
                    },
                    new FormWizardUIStep()
                    {
                        Title = "Itens do Cliente",
                        Id = "stepItensCliente",
                        Quantity = 2,
                    },
                    new FormWizardUIStep()
                    {
                        Title = "Produtos",
                        Id = "stepProdutos",
                        Quantity = 2,
                    },
                    new FormWizardUIStep()
                    {
                        Title = "Serviços",
                        Id = "stepServicos",
                        Quantity = 2,
                    },
                    new FormWizardUIStep()
                    {
                        Title = "Finalizar",
                        Id = "stepFinalizar",
                        Quantity = 5,
                    }
                },
                Rule = isEdit ? "parallel" : "linear",
                ShowStepNumbers = true
            };

            #region step Cadastro

            config.Elements.Add(new InputHiddenUI { Id = "id" });
            config.Elements.Add(new InputHiddenUI { Id = "status" });
            config.Elements.Add(new InputNumbersUI { Id = "numero", Class = "col s12 m2", Label = "Número OS", Readonly = true });

            config.Elements.Add(new InputDateUI { Id = "dataEmissao", Class = "col s12 m3", Label = "Data de Emissão", Required = true });
            config.Elements.Add(new InputDateUI { Id = "dataEntrega", Class = "col s12 m3", Label = "Data de Entrega", Required = true });

            config.Elements.Add(ElementUIHelper.GetAutoComplete(new AutoCompleteUI
            {
                Id = "responsavelPadraoId",
                Class = "col s12",
                Label = "Responsável padrão",
                Required = false,
                DataUrl = Url.Action("Vendedor", "AutoComplete"),
                LabelId = "responsavelPadraoNome",
                LabelName = "responsavelPadraoNome"
            }, ResourceHashConst.FaturamentoCadastrosClientes));

            config.Elements.Add(ElementUIHelper.GetAutoComplete(new AutoCompleteUI
            {
                Id = "clienteId",
                Class = "col s12",
                Label = "Cliente",
                Required = true,
                DataUrl = Url.Action("Cliente", "AutoComplete"),
                LabelId = "clienteNome",
                DataUrlPost = Url.Action("PostCliente")
            }, ResourceHashConst.FaturamentoCadastrosClientes));

            config.Elements.Add(new TextAreaUI { Id = "descricao", Class = "col s12", Label = "Descrição", MaxLength = 1000 });


            #endregion

            #region Itens do Cliente
            config.Elements.Add(new ButtonUI
            {
                Id = "btnOrdemServicoManutencao",
                Class = "col s12 m2",
                Label = "",
                Value = "Adicionar Item",
                DomEvents = new List<DomEventUI>
                    {
                        new DomEventUI { DomEvent = "click", Function = "fnModalOrdemServicoManutencao" }
                    }
            });
            config.Elements.Add(new DivElementUI { Id = "ordemServicoManutencao", Class = "col s12" });
            #endregion

            #region step Produtos
            config.Elements.Add(new ButtonUI
            {
                Id = "btnOrdemServicoItemProduto",
                Class = "col s12 m2",
                Label = "",
                Value = "Adicionar produto",
                DomEvents = new List<DomEventUI>
                    {
                        new DomEventUI { DomEvent = "click", Function = "fnModalOrdemServicoItemProduto" }
                    }
            });
            config.Elements.Add(new DivElementUI { Id = "ordemServicoItemProdutos", Class = "col s12" });
            #endregion

            #region step Serviços
            config.Elements.Add(new ButtonUI
            {
                Id = "btnOrdemServicoItemServico",
                Class = "col s12 m2",
                Label = "",
                Value = "Adicionar serviço",
                DomEvents = new List<DomEventUI>
                    {
                        new DomEventUI { DomEvent = "click", Function = "fnModalOrdemServicoItemServico" }
                    }
            });
            config.Elements.Add(new DivElementUI { Id = "ordemServicoItemServicos", Class = "col s12" });
            #endregion

            #region step Finalizar
            config.Elements.Add(new InputNumbersUI { Id = "quantidadeItensCliente", Class = "col s12 m3", Label = "Qtd. Itens do Cliente", Readonly = true });
            config.Elements.Add(new InputCurrencyUI { Id = "totalProdutos", Class = "col s12 m3", Label = "Total produtos", Readonly = true });
            config.Elements.Add(new InputCurrencyUI { Id = "totalServicos", Class = "col s12 m3", Label = "Total serviços", Readonly = true });
            config.Elements.Add(new InputCurrencyUI { Id = "totalOrdemServico", Class = "col s12 m3", Label = "Total (produtos + serviços)", Readonly = true });
            config.Elements.Add(new InputCheckboxUI { Id = "geraOrdemVenda", Class = "col s12 m3", Label = "Gerar Pedido de Venda" });
            #endregion

            cfg.Content.Add(config);
            cfg.Content.Add(GetDtOrdemServicoManutencaoCfg());
            cfg.Content.Add(GetDtOrdemServicoItemProdutosCfg());
            cfg.Content.Add(GetDtOrdemServicoItemServicosCfg());
            return cfg;

        }

        [OperationRole(PermissionValue = EPermissionValue.Write)]
        public virtual ContentResult FormNew()
        {
            // _novaOS = true;
            return Form();
        }

        protected override ContentUI FormJson()
            => FormOrdemServicoJson();

        public override JsonResult GridLoad(Dictionary<string, string> filters = null)
        {
            if (filters == null)
                filters = new Dictionary<string, string>();

            filters.Add("dataEmissao le ", Request.QueryString["dataFinal"]);
            filters.Add(" and dataEmissao ge ", Request.QueryString["dataInicial"]);

            return base.GridLoad(filters);
        }

        public JsonResult GridLoadNoFilter()
            => base.GridLoad();

        [HttpPost]
        public override JsonResult Create(OrdemServicoVM entityVM)
        {
            try
            {
                var postResponse = RestHelper.ExecutePostRequest(ResourceName, JsonConvert.SerializeObject(entityVM, JsonSerializerSetting.Default));
                OrdemServicoVM postResult = JsonConvert.DeserializeObject<OrdemServicoVM>(postResponse);
                var response = new JsonResult
                {
                    Data = new { success = true, message = AppDefaults.EditSuccessMessage, id = postResult.Id.ToString(), numero = postResult.Numero.ToString() }
                };
                return (response);
            }
            catch (Exception ex)
            {
                var error = JsonConvert.DeserializeObject<ErrorInfo>(ex.Message);
                return JsonResponseStatus.GetFailure(error.Message);
            }
        }

        public ContentResult Visualizar()
        {
            ModalUIForm config = new ModalUIForm()
            {
                Title = "Visualizar",
                UrlFunctions = @Url.Action("Functions", "OrdemServico") + "?fns=",
                CancelAction = new ModalUIAction() { Label = "Cancelar" },
                Action = new FormUIAction
                {
                    Create = @Url.Action("Create"),
                    Edit = @Url.Action("Edit"),
                    Get = @Url.Action("Json") + "/",
                    List = @Url.Action("List")
                },
                ReadyFn = "fnFormReadyVisualizarOrdemServico",
                Id = "fly01mdlfrmVisualizarOrdemServico"
            };

            config.Elements.Add(new InputHiddenUI { Id = "id" });
            config.Elements.Add(new InputNumbersUI { Id = "numero", Class = "col s12 m6 l2", Label = "Número", Disabled = true });
            config.Elements.Add(new InputDateUI { Id = "dataEmissao", Class = "col s12 m6 l2", Label = "Data de Emissão", Disabled = true });
            config.Elements.Add(new InputDateUI { Id = "dataEntrega", Class = "col s12 m6 l2", Label = "Data de Entrega", Disabled = true });
            config.Elements.Add(new AutoCompleteUI
            {
                Id = "clienteId",
                Class = "col s12 m6",
                Label = "Cliente",
                Disabled = true,
                DataUrl = Url.Action("Cliente", "AutoComplete"),
                LabelId = "clienteNome"
            });
            config.Elements.Add(new TextAreaUI
            {
                Id = "descricao",
                Class = "col s12",
                Label = "Descrição",
                MaxLength = 1000,
                Disabled = true
            });
            config.Elements.Add(new LabelSetUI { Id = "labelSetItensCliente", Class = "col s12", Label = "Itens do Cliente" });
            config.Elements.Add(new TableUI
            {
                Id = "ordemServicoManutencaoDataTable",
                Class = "col s12",
                Disabled = true,
                Options = new List<OptionUI>
                {
                    new OptionUI { Label = "Item do Cliente", Value = "0"},
                    new OptionUI { Label = "Quant.", Value = "1"}
                }
            });
            config.Elements.Add(new LabelSetUI { Id = "labelSetProdutos", Class = "col s12", Label = "Produtos" });
            config.Elements.Add(new TableUI
            {
                Id = "ordemServicoItemProdutosDataTable",
                Class = "col s12",
                Disabled = true,
                Options = new List<OptionUI>
                {
                    new OptionUI { Label = "Produto", Value = "0"},
                    new OptionUI { Label = "Quant.", Value = "1"},
                    new OptionUI { Label = "Valor",Value = "2"},
                    new OptionUI { Label = "Desconto",Value = "3"},
                    new OptionUI { Label = "Total",Value = "4"},
                }
            });
            config.Elements.Add(new LabelSetUI { Id = "labelSetServico", Class = "col s12", Label = "Serviços" });
            config.Elements.Add(new TableUI
            {
                Id = "ordemServicoItemServicosDataTable",
                Class = "col s12",
                Disabled = true,
                Options = new List<OptionUI>
                {
                    new OptionUI { Label = "Serviço", Value = "0"},
                    new OptionUI { Label = "Quant.", Value = "1"},
                    new OptionUI { Label = "Valor",Value = "2"},
                    new OptionUI { Label = "Desconto",Value = "3"},
                    new OptionUI { Label = "Total",Value = "4"},
                }
            });
            return Content(JsonConvert.SerializeObject(config, JsonSerializerSetting.Front), "application/json");
        }

        [OperationRole(PermissionValue = EPermissionValue.Read)]
        [HttpGet]
        public JsonResult TotalOrdemServico(string id)
        {
            try
            {
                var resource = string.Format("CalculaTotalOrdemServico?ordemServicoId={0}&onList={1}", id, false);
                var response = RestHelper.ExecuteGetRequest<TotalOrdemServicoVM>(resource, queryString: null);
                return Json(
                    new { success = true, total = response },
                    JsonRequestBehavior.AllowGet
                );
            }
            catch (Exception ex)
            {
                var error = JsonConvert.DeserializeObject<ErrorInfo>(ex.Message);
                return JsonResponseStatus.GetFailure(error.Message);
            }
        }

        [OperationRole(PermissionValue = EPermissionValue.Read)]
        [HttpGet]
        public JsonResult GetInformacoesComplementares()
        {
            try
            {
                var response = RestHelper.ExecuteGetRequest<ResultBase<ParametroTributarioVM>>("parametrotributario");

                return Json(
                    new { success = true, infcomp = response.Data.FirstOrDefault()?.MensagemPadraoNota },
                    JsonRequestBehavior.AllowGet
                );
            }
            catch (Exception ex)
            {
                var error = JsonConvert.DeserializeObject<ErrorInfo>(ex.Message);
                return JsonResponseStatus.GetFailure(error.Message);
            }
        }

        private List<ImprimirOrdemServicoVM> GetDadosOrcamentoPedido(string id, OrdemServicoVM os)
        {
            var manut = GetObjetosManutencao(Guid.Parse(id));
            var produtos = GetProdutos(Guid.Parse(id));
            var servicos = GetServicos(Guid.Parse(id));
            var total = produtos.Sum(x => x.Total) + servicos.Sum(x => x.Total);

            var reportItems = new List<ImprimirOrdemServicoVM>();
            foreach (OrdemServicoItemProdutoVM itemManutencao in manut)
                reportItems.Add(PreencherItemBase(os, total, "Objetos de manutenção", itemManutencao, itemManutencao.ProdutoId, itemManutencao.Produto?.Descricao));

            foreach (OrdemServicoItemServicoVM OrdemServico in servicos)
                reportItems.Add(PreencherItemBase(os, total, "Serviços", OrdemServico, OrdemServico.ServicoId, OrdemServico.Servico?.Descricao));

            foreach (OrdemServicoItemProdutoVM OrdemProduto in produtos)
                reportItems.Add(PreencherItemBase(os, total, "Produtos", OrdemProduto, OrdemProduto.ProdutoId, OrdemProduto.Produto?.Descricao));

            if (!produtos.Any() && !servicos.Any())
                reportItems.Add(GetEntidadeImpressaoBase(os, total));

            return reportItems;
        }

        private static ImprimirOrdemServicoVM GetEntidadeImpressaoBase(OrdemServicoVM os, double total) => new ImprimirOrdemServicoVM
        {
            Id = os.Id.ToString(),
            ClienteNome = os.Cliente?.Nome,
            ClienteCPF = os.Cliente?.CPFCNPJ,
            ClienteCelular = os.Cliente?.Celular,
            ClienteTelefone = os.Cliente?.Telefone,
            ClienteEndereco = GetEndereco(os.Cliente),
            ClientEmail = os.Cliente?.Email,
            DataEmissao = os.DataEmissao.ToString(),
            DataEntrega = os.DataEntrega.ToString(),
            Status = os.Status.ToString(),
            Numero = os.Numero.ToString(),
            Descricao = os.Descricao,
            Total = total
        };

        private static ImprimirOrdemServicoVM PreencherItemBase(OrdemServicoVM os, double total, string itemTipo, OrdemServicoItemVM OrdemProduto, Guid itemId, string itemNome)
        {
            var ent = GetEntidadeImpressaoBase(os, total);
            ent.ItemTipo = itemTipo;
            ent.ItemId = itemId;
            ent.ItemNome = itemNome;
            ent.ItemQtd = OrdemProduto.Quantidade;
            ent.ItemValor = OrdemProduto.Valor;
            ent.ItemDesconto = OrdemProduto.Desconto;
            ent.ItemTotal = OrdemProduto.Total;
            ent.ItemObservacao = OrdemProduto.Observacao;

            return ent;
        }

        private static string GetEndereco(PessoaVM cliente) =>
                cliente == null || string.IsNullOrEmpty(cliente.Endereco) ?
                "" :
                $"{cliente.Endereco}, {cliente.Cidade?.Nome} - {cliente.Estado?.Sigla}. CEP: {cliente.CEP}";

        [OperationRole(PermissionValue = EPermissionValue.Read)]
        public virtual JsonResult ImprimirOrdemServico(string id)
        {
            try
            {
                var ordemServico = Get(Guid.Parse(id));
                var fileName = "OrdemServico" + ordemServico.Numero.ToString() + ".pdf";
                var fileBase64 = Convert.ToBase64String(GetPDFFile(ordemServico));

                Session.Add(fileName, fileBase64);


                return JsonResponseStatus.GetJson(new { downloadAddress = Url.Action("DownloadPDF", new { fileName }) });
            }
            catch (Exception ex)
            {
                var error = JsonConvert.DeserializeObject<ErrorInfo>(ex.Message);
                return JsonResponseStatus.GetFailure(error.Message);
            }
        }

        private byte[] GetPDFFile(OrdemServicoVM ordemServico)
        {
            var reportViewer = new WebReportViewer<ImprimirOrdemServicoVM>(ReportOrdemServico.Instance);
            return reportViewer.Print(GetDadosOrcamentoPedido(ordemServico.Id.ToString(), ordemServico), SessionManager.Current.UserData.PlatformUrl);
        }

        private JsonResult MudarStatus(string id, StatusOrdemServico status, bool gerarOrdemVenda = false)
        {
            dynamic pedido = new ExpandoObject();
            pedido.status = status.ToString();
            if (gerarOrdemVenda)
                pedido.geraOrdemVenda = true;
            return ExecutePut(id, pedido);
        }

        [OperationRole(PermissionValue = EPermissionValue.Write)]
        [HttpPost]
        public JsonResult ExecutarOrdem(string id) => MudarStatus(id, StatusOrdemServico.EmAndamento);

        [OperationRole(PermissionValue = EPermissionValue.Write)]
        [HttpPost]
        public JsonResult CancelarOrdem(string id) => MudarStatus(id, StatusOrdemServico.Cancelado);

        [OperationRole(PermissionValue = EPermissionValue.Write)]
        [HttpPost]
        public JsonResult ConcluirOrdem(string id, bool geraOrdemVenda = false) => MudarStatus(id, StatusOrdemServico.Concluido, geraOrdemVenda);


        [OperationRole(PermissionValue = EPermissionValue.Write)]
        [HttpPost]
        public JsonResult GerarOrdem(string id)
        {
            dynamic pedido = new ExpandoObject();
            pedido.geraOrdemServico = true;
            return ExecutePut(id, pedido);
        }

        private static JsonResult ExecutePut(string id, object pedido)
        {
            try
            {
                var resourceNamePut = $"OrdemServico/{id}";
                RestHelper.ExecutePutRequest(resourceNamePut, JsonConvert.SerializeObject(pedido, JsonSerializerSetting.Edit));

                return JsonResponseStatus.Get(new ErrorInfo { HasError = false }, Operation.Edit);
            }
            catch (Exception ex)
            {
                var error = JsonConvert.DeserializeObject<ErrorInfo>(ex.Message);
                return JsonResponseStatus.GetFailure(error.Message);
            }
        }

        #region OnDemmand
        [HttpPost]
        public JsonResult NovaCategoria(string term)
        {
            try
            {
                var tipoCategoria = "";
                var tipoCarteira = Request.QueryString["tipo"];

                if (tipoCarteira == "Receita")
                    tipoCategoria = "1";
                else
                    tipoCategoria = "2";

                var entity = new CategoriaVM
                {
                    Descricao = term,
                    TipoCarteira = tipoCategoria
                };

                var resourceName = AppDefaults.GetResourceName(typeof(CategoriaVM));
                var data = RestHelper.ExecutePostRequest<CategoriaVM>(resourceName, entity, AppDefaults.GetQueryStringDefault());

                return JsonResponseStatus.Get(new ErrorInfo() { HasError = false }, Operation.Create, data.Id);
            }
            catch (Exception ex)
            {
                var error = JsonConvert.DeserializeObject<ErrorInfo>(ex.Message);
                return JsonResponseStatus.GetFailure(error.Message);
            }
        }
        #endregion
    }
}