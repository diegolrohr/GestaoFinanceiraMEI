using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Fly01.Financeiro.ViewModel;
using Fly01.Core;
using Fly01.Core.Helpers;
using Fly01.uiJS.Classes;
using Fly01.uiJS.Classes.Elements;
using Newtonsoft.Json;
using Fly01.uiJS.Defaults;
using Fly01.Core.Presentation.JQueryDataTable;
using Fly01.Core.Presentation.Commons;
using Fly01.Core.Rest;
using Fly01.Core.ViewModels.Presentation.Commons;
using Fly01.Core.Config;
using Fly01.Core.Presentation;
using Fly01.uiJS.Enums;
using Fly01.Core.ViewModels;
using Fly01.Financeiro.Models.Reports;
using System.Data;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.IO;
using System.Text;

namespace Fly01.Financeiro.Controllers
{
    [OperationRole(ResourceKey = ResourceHashConst.FinanceiroFinanceiroExtrato)]
    public class ExtratoController : BaseController<MovimentacaoVM>
    {
        public JsonResult LoadContasBancarias()
        {
            try
            {
                var response = RestHelper.ExecuteGetRequest<ResponseExtratoContaSaldoVM>("extrato/saldos");
                if (response == null)
                    return Json(new { }, JsonRequestBehavior.AllowGet);

                var saldoAbsolutoTodasContas = response.Values.Where(x => x.ContaBancariaId != Guid.Empty).Sum(x => Math.Abs(x.SaldoConsolidado));
                var responseToView = response.Values.Select(item => new
                {
                    contaBancariaId = item.ContaBancariaId.ToString() == Guid.Empty.ToString()
                        ? string.Empty
                        : item.ContaBancariaId.ToString(),
                    contaBancariaDescricao = item.ContaBancariaDescricao,
                    contaSaldo = item.SaldoConsolidado.ToString("C", AppDefaults.CultureInfoDefault),
                    progressSaldo = item.ContaBancariaId == Guid.Empty || saldoAbsolutoTodasContas == default(double)
                        ? 100
                        : Math.Round(Math.Abs(item.SaldoConsolidado) / saldoAbsolutoTodasContas * 100, 2)
                }).OrderByDescending(x => x.progressSaldo).ToList();

                return Json(new
                {
                    recordsTotal = responseToView.Count,
                    recordsFiltered = responseToView.Count,
                    data = responseToView
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return JsonResponseStatus.GetFailure(ex.Message);
            }
        }

        public JsonResult LoadChart(DateTime dataInicial, DateTime dataFinal, string contaBancariaId)
        {
            try
            {
                Dictionary<string, string> queryString = new Dictionary<string, string>
                {
                    { "dataInicial", dataInicial.ToString("yyyy-MM-dd") },
                    { "dataFinal", dataFinal.ToString("yyyy-MM-dd") },
                    { "contaBancariaId", contaBancariaId ?? string.Empty }
                };

                var responseChart = RestHelper.ExecuteGetRequest<ResponseExtratoHistoricoSaldoVM>("extrato/historicosaldos", queryString);
                var dataChartToView = new
                {
                    success = true,
                    currency = true,
                    labels = responseChart.Value.Saldos.Select(x => x.Data.ToString("dd/MM")).ToArray(),
                    datasets = new object[] {
                        new {
                                type = "line",
                                label = "Saldo Consolidado",
                                backgroundColor = "rgb(44, 55, 57)",
                                borderColor = "rgb(44, 55, 57)",
                                data = responseChart.Value.Saldos.Select(x => Math.Round(x.SaldoConsolidado, 2)).ToArray(),
                                fill = false
                            },
                        new {
                                label = "Recebimentos",
                                fill = false,
                                backgroundColor = "rgb(0, 178, 121)",
                                borderColor = "rgb(0, 178, 121)",
                                data = responseChart.Value.Saldos.Select(x => Math.Round(x.TotalRecebimentos, 2)).ToArray()
                            },
                        new {
                                label = "Pagamentos",
                                fill = false,
                                backgroundColor = "rgb(239, 100, 97)",
                                borderColor = "rgb(239, 100, 97)",
                                data = responseChart.Value.Saldos.Select(x => Math.Round(x.TotalPagamentos * -1, 2)).ToArray()
                        }
                    }
                };

                return Json(dataChartToView, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return JsonResponseStatus.GetFailure(ex.Message);
            }

        }

        public JsonResult LoadExtratoDetalhes(DateTime dataInicial, DateTime dataFinal, string contaBancariaId, int length)
        {
            try
            {
                if (length == default(int))
                    length = 50;

                var param = JQueryDataTableParams.CreateFromQueryString(Request.QueryString);
                var fileType = (Request.QueryString.AllKeys.Contains("fileType")) ? Request.QueryString.Get("fileType") : "";
                var pageNo = param.Start > 0 ? (param.Start / length) + 1 : 1;

                Dictionary<string, string> queryString = new Dictionary<string, string>
                {
                    { "dataInicial", dataInicial.ToString("yyyy-MM-dd") },
                    { "dataFinal", dataFinal.ToString("yyyy-MM-dd") },
                    { "contaBancariaId", contaBancariaId ?? string.Empty },
                    { "pageNo", pageNo.ToString() },
                    { "pageSize", length.ToString() }
                };

                var responseExtratoDetalhe = RestHelper.ExecuteGetRequest<PagedResult<ExtratoDetalheVM>>("extrato/extratodetalhe", queryString);

                if (!string.IsNullOrWhiteSpace(fileType))
                {
                    if (responseExtratoDetalhe.Data.Count.Equals(0))
                        throw new Exception("Não existem registros para exportar");
                    DataTable dataTable = GridToDataTable(responseExtratoDetalhe, param);
                    switch (fileType.ToLower())
                    {
                        case "pdf":
                            GridToPDF(dataTable);
                            break;
                        case "doc":
                            GridToDOC(dataTable);
                            break;
                        case "xls":
                            GridToXLS(dataTable);
                            break;
                        case "csv":
                            GridToCSV(dataTable);
                            break;
                    }

                }

                return Json(new
                {
                    recordsTotal = responseExtratoDetalhe.Paging.TotalRecordCount,
                    recordsFiltered = responseExtratoDetalhe.Paging.TotalRecordCount,
                    data = responseExtratoDetalhe.Data.Select(item => new
                    {
                        data = item.DataMovimento.ToString("dd/MM/yyyy"),
                        descricaoLancamento = item.DescricaoLancamento,
                        pessoaNome = item.PessoaNome,
                        contaBancariaDescricao = item.ContaBancariaDescricao,
                        valorLancamento = item.ValorLancamento.ToString("C", AppDefaults.CultureInfoDefault)
                    })
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return JsonResponseStatus.GetFailure(ex.Message);
            }
        }

        [HttpPost]
        public virtual JsonResult NovoPagamento(MovimentacaoVM pagamento)
        {
            try
            {
                RestHelper.ExecutePostRequest("movimentacao", JsonConvert.SerializeObject(pagamento, JsonSerializerSetting.Default));
                return JsonResponseStatus.Get(new ErrorInfo { HasError = false }, Operation.Create);
            }
            catch (Exception ex)
            {
                var error = JsonConvert.DeserializeObject<ErrorInfo>(ex.Message);
                return JsonResponseStatus.GetFailure(error.Message);
            }
        }

        [HttpPost]
        public virtual JsonResult NovoRecebimento(MovimentacaoVM recebimento)
        {
            try
            {
                RestHelper.ExecutePostRequest("movimentacao", JsonConvert.SerializeObject(recebimento, JsonSerializerSetting.Default));
                return JsonResponseStatus.Get(new ErrorInfo { HasError = false }, Operation.Create);
            }
            catch (Exception ex)
            {
                var error = JsonConvert.DeserializeObject<ErrorInfo>(ex.Message);
                return JsonResponseStatus.GetFailure(error.Message);
            }
        }

        [HttpPost]
        public virtual JsonResult NovaTransferencia(TransferenciaCadastroVM transferenciaCadastro)
        {
            try
            {
                var transferencia = new TransferenciaVM
                {
                    MovimentacaoOrigem = new MovimentacaoVM
                    {
                        CategoriaId = transferenciaCadastro.CategoriaId,
                        ContaBancariaOrigemId = transferenciaCadastro.ContaBancariaOrigemId,
                        Data = transferenciaCadastro.Data,
                        Descricao = transferenciaCadastro.Descricao,
                        Valor = (transferenciaCadastro.Valor * -1)
                    },
                    MovimentacaoDestino = new MovimentacaoVM
                    {
                        CategoriaId = transferenciaCadastro.CategoriaDestinoId,
                        ContaBancariaDestinoId = transferenciaCadastro.ContaBancariaDestinoId,
                        Data = transferenciaCadastro.Data,
                        Descricao = transferenciaCadastro.DescricaoDestino,
                        Valor = transferenciaCadastro.Valor
                    }
                };

                RestHelper.ExecutePostRequest("transferencia", JsonConvert.SerializeObject(transferencia, JsonSerializerSetting.Default));
                return JsonResponseStatus.Get(new ErrorInfo { HasError = false }, Operation.Create);
            }
            catch (Exception ex)
            {
                var error = JsonConvert.DeserializeObject<ErrorInfo>(ex.Message);
                return JsonResponseStatus.GetFailure(error.Message);
            }
        }

        public override Func<MovimentacaoVM, object> GetDisplayData()
        {
            throw new NotImplementedException();
        }

        public override List<HtmlUIButton> GetListButtonsOnHeader()
        {
            var target = new List<HtmlUIButton>();

            if (UserCanRead)
            {
                target.Add(new HtmlUIButton { Id = "save", Label = "Atualizar", OnClickFn = "fnAtualizar", Position = HtmlUIButtonPosition.Main });
                target.Add(new HtmlUIButton { Id = "prnt", Label = "Imprimir", OnClickFn = "fnImprimirExtrato", Position = HtmlUIButtonPosition.Out });
            }

            if (UserCanWrite)
            {
                target.Add(new HtmlUIButton { Id = "pgto", Label = "Novo Pagamento", OnClickFn = "fnNovoPagamento", Position = HtmlUIButtonPosition.In });
                target.Add(new HtmlUIButton { Id = "rcto", Label = "Novo Recebimento", OnClickFn = "fnNovoRecebimento", Position = HtmlUIButtonPosition.In });
                target.Add(new HtmlUIButton { Id = "trnf", Label = "Nova Transferência", OnClickFn = "fnNovaTransferencia", Position = HtmlUIButtonPosition.In });
            }

            return target;
        }

        public override ContentResult List()
        {
            if (!UserCanRead)
                return Content(JsonConvert.SerializeObject(new ContentUIBase(Url.Action("Sidebar", "Home")), JsonSerializerSetting.Default), "application/json");

            var response = ApiEmpresaManager.GetEmpresa(SessionManager.Current.UserData.PlatformUrl);
            var responseCidade = response.Cidade != null ? response.Cidade.Nome : string.Empty;
            var dataInicialFiltroDefault = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            var dataFinalFiltroDefault = DateTime.Now;

            var cfg = new ContentUIBase(Url.Action("Sidebar", "Home"))
            {
                History = new ContentUIHistory { Default = Url.Action("Index") },
                Header = new HtmlUIHeader
                {
                    Title = "Extrato",
                    Buttons = new List<HtmlUIButton>(GetListButtonsOnHeader())
                },
                UrlFunctions = Url.Action("Functions", "Extrato", null, Request.Url.Scheme) + "?fns="
            };
            cfg.Content.Add(new CardUI
            {
                Class = "col s12 m8 offset-m2 printinfo",
                Color = "totvs-blue",
                Id = "fly01cardCabecalho",
                Placeholder = response.RazaoSocial + " | CNPJ: " + response.CNPJ +
                              " | Endereço: " + response.Endereco + ", " + response.Numero +
                              " | Bairro: " + response.Bairro + " | CEP: " + response.CEP +
                              " | Cidade: " + responseCidade + " | Email: " + response.Email,
                Action = new LinkUI
                {
                    Label = "",
                    OnClick = ""
                }
            });
            cfg.Content.Add(new FormUI
            {
                Id = "fly01frm",
                ReadyFn = "fnFormReady",
                UrlFunctions = Url.Action("Functions") + "?fns=",
                Functions = new List<string> { "__format" },
                Class = "col s12",
                Elements = new List<BaseUI>
                {
                    new InputHiddenUI {Id = "contaBancariaId"},
                    new InputDateUI
                    {
                        Id = "dataInicial",
                        Class = "col s6 m3 l4",
                        Label = "Data Inicial",
                        Value = dataInicialFiltroDefault.ToString("dd/MM/yyyy"),
                        DomEvents = new List<DomEventUI> { new DomEventUI {DomEvent = "change", Function = "fnAtualizar"} },
                        //Max = true,
                        Min = -2000
                    },
                    new InputDateUI
                    {
                        Id = "dataFinal",
                        Class = "col s6 m3 l4",
                        Label = "Data Final",
                        Value = dataFinalFiltroDefault.ToString("dd/MM/yyyy"),
                        DomEvents = new List<DomEventUI> { new DomEventUI {DomEvent = "change", Function = "fnAtualizar"} },
                        //Max = true,
                        Min = -2000
                    },
                    new ButtonGroupUI
                    {
                        Id = "fly01btngrp",
                        Class = "col s12 m6 l4 hide-on-print",
                        Label = "Selecione o período",
                        OnClickFn = "fnAtualizarPeriodo",
                        Options = new List<ButtonGroupOptionUI>
                        {
                            new ButtonGroupOptionUI { Id = "btnDia", Value = "0", Label = "Dia", Class = "col s4" },
                            new ButtonGroupOptionUI { Id = "btnSemana", Value = "6", Label = "Semana", Class = "col s4" },
                            new ButtonGroupOptionUI { Id = "btnMes", Value = "30", Label = "Mês", Class = "col s4" }
                        }
                    }
                }
            });
            cfg.Content.Add(new DataTableUI
            {
                Id = "contasBancariasList",
                Class = "col s12 m12 l4",
                UrlGridLoad = @Url.Action("LoadContasBancarias"),
                Columns = new List<DataTableUIColumn>
                {
                    new DataTableUIColumn
                    {
                        DataField = "contaBancariaId",
                        DisplayName = "Contas Bancárias - Saldo Atual",
                        Orderable = false,
                        Searchable = false,
                        RenderFn = "fnRenderContasBancarias"
                    }
                },
                Options = new DataTableUIConfig
                {
                    ScrollLength = 300,
                    WithoutRowMenu = true
                },
                Callbacks = new DataTableUICallbacks
                {
                    DrawCallback = "fnContaBancariaDrawCB"
                },
                UrlFunctions = Url.Action("Functions") + "?fns=",
                Functions = new List<string> { "fnContaBancariaDrawCB" }
            });
            cfg.Content.Add(new ChartUI
            {
                Id = "fly01chart",
                Options = new
                {
                    title = new
                    {
                        display = true,
                        text = "Saldo do Período",
                        fontSize = 15,
                        fontFamily = "Roboto",
                        fontColor = "#555"
                    },
                    tooltips = new
                    {
                        mode = "label",
                        bodySpacing = 10,
                        cornerRadius = 0,
                        titleMarginBottom = 15
                    },
                    legend = new { position = "bottom" },
                    global = new
                    {
                        responsive = false,
                        maintainAspectRatio = false
                    },
                    scales = new
                    {
                        xAxes = new object[]
                        {
                            new {
                                stacked = true
                            }
                        },
                        yAxes = new object[]
                        {
                            new {
                                stacked = true
                            }
                        }
                    }
                },
                UrlData = @Url.Action("LoadChart"),
                Class = "col s12 m12 l8",
                Parameters = new List<ChartUIParameter>
                {
                    new ChartUIParameter { Id = "dataInicial" },
                    new ChartUIParameter { Id = "dataFinal" },
                    new ChartUIParameter { Id = "contaBancariaId" }
                }
            });
            cfg.Content.Add(new DivUI
            {
                Class = "col s12",
                Elements = new List<BaseUI>
                {
                    new LabelSetUI { Id =  "lab", Class = "col s12", Label = "Detalhes do Extrato"}
                }
            });
            cfg.Content.Add(new DataTableUI
            {
                Id = "dtExtratoDetalhe",
                Class = "col s12",
                UrlGridLoad = Url.Action("LoadExtratoDetalhes"),
                Parameters = new List<DataTableUIParameter>
                {
                    new DataTableUIParameter { Id = "dataInicial" },
                    new DataTableUIParameter { Id = "dataFinal" },
                    new DataTableUIParameter { Id = "contaBancariaId" }
                },
                Options = new DataTableUIConfig()
                {
                    PageLength = 50,
                    LengthChange = true
                },
                Columns = new List<DataTableUIColumn>
                {
                    new DataTableUIColumn { DataField = "data", DisplayName = "Data", Priority = 1, Orderable = false, Searchable = false, Type = "date" },
                    new DataTableUIColumn { DataField = "pessoaNome", DisplayName = "Cliente/Fornecedor", Priority = 2, Orderable = false, Searchable = false },
                    new DataTableUIColumn { DataField = "contaBancariaDescricao", DisplayName = "Conta Bancária", Priority = 3, Orderable = false, Searchable = false },
                    new DataTableUIColumn { DataField = "valorLancamento", DisplayName = "Valor", Priority = 4, Orderable = false, Searchable = false, Type = "currency" },
                    new DataTableUIColumn { DataField = "descricaoLancamento", DisplayName = "Lançamento", Priority = 5, Orderable = false, Searchable = false },           
        }
            });
            return Content(JsonConvert.SerializeObject(cfg, JsonSerializerSetting.Front), "application/json");
        }

        protected override ContentUI FormJson()
        {
            throw new NotImplementedException();
        }

        [OperationRole(PermissionValue = EPermissionValue.Write)]
        public ContentResult ModalExtratoNovaTransferencia()
        {
            var config = new ModalUIForm()
            {
                Title = "Nova Transferência",
                UrlFunctions = @Url.Action("Functions", "Extrato", null, Request.Url.Scheme) + "?fns=",
                Id = "fly01frmNovaTransf",
                ConfirmAction = new ModalUIAction { Label = "Confirmar", OnClickFn = "fnSalvarNovaTransferencia" },
                CancelAction = new ModalUIAction { Label = "Cancelar" },
                Action = new FormUIAction
                {
                    Create = "",
                    Edit = "",
                    List = ""
                }
            };

            config.Elements.Add(new InputHiddenUI { Id = "descricaoDestino" });
            config.Elements.Add(new InputHiddenUI { Id = "descricao" });

            config.Elements.Add(ElementUIHelper.GetAutoComplete(new AutoCompleteUI
            {
                Id = "contaBancariaOrigemIdTransf",
                Class = "col s12 m6",
                Label = "Conta Bancária Origem",
                Required = true,
                Name = "contaBancariaOrigemId",
                DataUrl = @Url.Action("ContaBancariaBanco", "AutoComplete"),
                LabelId = "contaBancariaOrigemNomeContaTransf",
                LabelName = "contaBancariaOrigemNomeConta",
                DataUrlPostModal = @Url.Action("FormModal", "ContaBancaria"),
                DataPostField = "nomeConta",
                DomEvents = new List<DomEventUI>
                {
                    new DomEventUI {DomEvent = "autocompletechange", Function = "fnChangeContaOrigemTransferencia"}
                }
            }, ResourceHashConst.FinanceiroCadastrosContasBancarias));

            config.Elements.Add(ElementUIHelper.GetAutoComplete(new AutoCompleteUI
            {
                Id = "contaBancariaDestinoIdTransf",
                Class = "col s12 m6",
                Label = "Conta Bancária Destino",
                Required = true,
                Name = "contaBancariaDestinoId",
                DataUrl = @Url.Action("ContaBancariaBanco", "AutoComplete"),
                LabelId = "contaBancariaDestinoNomeContaTransf",
                LabelName = "contaBancariaDestinoNomeConta",
                DataUrlPostModal = @Url.Action("FormModal", "ContaBancaria"),
                DataPostField = "nomeConta",
                DomEvents = new List<DomEventUI>
                {
                    new DomEventUI { DomEvent = "autocompletechange", Function = "fnChangeContaDestinoTransferencia" }
                }
            }, ResourceHashConst.FinanceiroCadastrosContasBancarias));

            config.Elements.Add(ElementUIHelper.GetAutoComplete(new AutoCompleteUI
            {
                Id = "categoriaIdTransf",
                Class = "col s12 m6",
                Label = "Categoria Origem",
                Required = true,
                Name = "categoriaId",
                DataUrl = @Url.Action("CategoriaCP", "AutoComplete"),
                LabelId = "categoriaDescricaoTransf",
                LabelName = "categoriaDescricao",
                DataUrlPost = Url.Action("NovaCategoriaDespesa"),
            }, ResourceHashConst.FinanceiroCadastrosCategoria));

            config.Elements.Add(ElementUIHelper.GetAutoComplete(new AutoCompleteUI
            {
                Id = "categoriaDestinoIdTransf",
                Class = "col s12 m6",
                Label = "Categoria Destino",
                Required = true,
                Name = "categoriaDestinoId",
                DataUrl = @Url.Action("CategoriaCR", "AutoComplete"),
                LabelId = "categoriaDestinoDescricaoTransf",
                LabelName = "categoriaDestinoDescricao",
                DataUrlPost = Url.Action("NovaCategoriaReceita")
            }, ResourceHashConst.FinanceiroCadastrosCategoria));

            config.Elements.Add(new InputDateUI
            {
                Id = "dataTransf",
                Class = "col s12 m6",
                Label = "Data",
                Required = true,
                Name = "data",
                Max = true
            });

            config.Elements.Add(new InputCurrencyUI { Id = "valorTransf", Class = "col s12 m6", Label = "Valor", Required = true, Name = "valor" });

            config.Elements.Add(new TextAreaUI {
                Id = "observacao",
                Class = "col s12",
                Label = "Descrição",
                Name = "observacao",
                MaxLength = 200,
                DomEvents = new List<DomEventUI>
                {
                    new DomEventUI { DomEvent = "change", Function = "fnChangeObservacao" }
                }
            });

            return Content(JsonConvert.SerializeObject(config, JsonSerializerSetting.Front), "application/json");
        }

        [OperationRole(PermissionValue = EPermissionValue.Write)]
        public ContentResult ModalExtratoNovoRecebimento()
        {
            var config = new ModalUIForm()
            {
                Title = "Novo Recebimento",
                UrlFunctions = @Url.Action("Functions", "Extrato", null, Request.Url.Scheme) + "?fns=",
                Id = "fly01frmNovoReceb",
                ConfirmAction = new ModalUIAction { Label = "Confirmar", OnClickFn = "fnSalvarNovoRecebimento" },
                CancelAction = new ModalUIAction { Label = "Cancelar" },
                Action = new FormUIAction
                {
                    Create = "",
                    Edit = "",
                    List = ""
                }
            };

            config.Elements.Add(ElementUIHelper.GetAutoComplete(new AutoCompleteUI
            {
                Id = "contaBancariaDestinoIdReceb",
                Class = "col s12",
                Label = "Conta Bancária",
                Required = true,
                Name = "contaBancariaDestinoId",
                DataUrl = @Url.Action("ContaBancariaBanco", "AutoComplete"),
                LabelId = "contaBancariaDestinoNomeContaReceb",
                LabelName = "contaBancariaDestinoNomeConta",
                DataUrlPostModal = @Url.Action("FormModal", "ContaBancaria"),
                DataPostField = "nomeConta"
            }, ResourceHashConst.FinanceiroCadastrosContasBancarias));

            config.Elements.Add(ElementUIHelper.GetAutoComplete(new AutoCompleteUI
            {
                Id = "categoriaIdReceb",
                Class = "col s12",
                Label = "Categoria",
                Required = true,
                Name = "categoriaId",
                DataUrl = @Url.Action("CategoriaCR", "AutoComplete"),
                LabelId = "categoriaReceb",
                LabelName = "categoriaDescricao",
                DataUrlPost = Url.Action("NovaCategoriaReceita"),
                DomEvents = new List<DomEventUI>
                {
                    new DomEventUI { DomEvent = "autocompleteselect", Function = "fnChangeCategoriaRecebimento" }
                }
            }, ResourceHashConst.FinanceiroCadastrosCategoria));

            config.Elements.Add(new InputDateUI
            {
                Id = "dataReceb",
                Class = "col s12 m6",
                Label = "Data",
                Required = true,
                Name = "data",
                Max = true
            });

            config.Elements.Add(new InputCurrencyUI { Id = "valorReceb", Class = "col s12 m6", Label = "Valor", Required = true, Name = "valor" });
            config.Elements.Add(new TextAreaUI { Id = "descricaoReceb", Class = "col s12", Label = "Descrição", Required = true, Name = "descricao", MaxLength = 200 });

            return Content(JsonConvert.SerializeObject(config, JsonSerializerSetting.Front), "application/json");
        }

        [OperationRole(PermissionValue = EPermissionValue.Write)]
        public ContentResult ModalExtratoNovoPagamento()
        {
            var config = new ModalUIForm()
            {
                Title = "Novo Pagamento",
                UrlFunctions = @Url.Action("Functions", "Extrato", null, Request.Url.Scheme) + "?fns=",
                Id = "fly01frmNovoPgt",
                ConfirmAction = new ModalUIAction { Label = "Confirmar", OnClickFn = "fnSalvarNovoPagamento" },
                CancelAction = new ModalUIAction { Label = "Cancelar" },
                Action = new FormUIAction
                {
                    Create = "",
                    Edit = "",
                    List = ""
                }
            };

            config.Elements.Add(ElementUIHelper.GetAutoComplete(new AutoCompleteUI
            {
                Id = "contaBancariaOrigemIdPgto",
                Class = "col s12",
                Label = "Conta Bancária",
                Required = true,
                Name = "contaBancariaOrigemId",
                DataUrl = @Url.Action("ContaBancariaBanco", "AutoComplete"),
                LabelId = "contaBancariaOrigemNomeContaPgto",
                LabelName = "contaBancariaOrigemNomeConta",
                DataUrlPostModal = @Url.Action("FormModal", "ContaBancaria"),
                DataPostField = "nomeConta"
            }, ResourceHashConst.FinanceiroCadastrosContasBancarias));

            config.Elements.Add(ElementUIHelper.GetAutoComplete(new AutoCompleteUI
            {
                Id = "categoriaIdPgto",
                Class = "col s12",
                Label = "Categoria",
                Required = true,
                Name = "categoriaId",
                DataUrl = @Url.Action("CategoriaCP", "AutoComplete"),
                LabelId = "categoriaPgto",
                LabelName = "categoriaDescricao",
                DataUrlPost = Url.Action("NovaCategoriaDespesa"),
                DomEvents = new List<DomEventUI>
                {
                    new DomEventUI { DomEvent = "autocompleteselect", Function = "fnChangeCategoriaPagamento" }
                }
            }, ResourceHashConst.FinanceiroCadastrosCategoria));

            config.Elements.Add(new InputDateUI
            {
                Id = "dataPgto",
                Class = "col s12 m6",
                Label = "Data",
                Required = true,
                Name = "data",
                Max = true
            });

            config.Elements.Add(new InputCurrencyUI { Id = "valorPgto", Class = "col s12 m6", Label = "Valor", Required = true, Name = "valor" });
            config.Elements.Add(new TextAreaUI { Id = "descricaoPgto", Class = "col s12", Label = "Descrição", Required = true, Name = "descricao", MaxLength = 200 });

            return Content(JsonConvert.SerializeObject(config, JsonSerializerSetting.Front), "application/json");
        }

        [OperationRole(PermissionValue = EPermissionValue.Write)]
        [HttpPost]
        public JsonResult NovaCategoriaDespesa(string term)
        {
            return NovaCategoria(new CategoriaVM { Descricao = term, TipoCarteira = "2" });
        }

        [OperationRole(PermissionValue = EPermissionValue.Write)]
        [HttpPost]
        public JsonResult NovaCategoriaReceita(string term)
        {
            return NovaCategoria(new CategoriaVM { Descricao = term, TipoCarteira = "1" });
        }

        private JsonResult NovaCategoria(CategoriaVM entity)
        {
            try
            {
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

        public virtual ActionResult ImprimirExtrato(DateTime? dataInicial, DateTime? dataFinal, string contaBancariaId)
        {
            try
            {
                List<ImprimirExtratoBancarioVM> reportItems = new List<ImprimirExtratoBancarioVM>();

                var contasbancarias = GetContasBancarias();

                var contabancaria = (!string.IsNullOrEmpty(contaBancariaId) ? contasbancarias.Where(x => x.ContaBancariaId == Guid.Parse(contaBancariaId)).FirstOrDefault() :
                    contasbancarias.Where(x => x.ContaBancariaDescricao == "Todas as Contas").FirstOrDefault());
                                  
                var extratodetalhes = GetExtratoDetalhe(dataInicial, dataFinal, contaBancariaId);

                MontarExtratoParaPrint(contabancaria, reportItems,dataInicial, dataFinal);
                MontarDetalhesExtratoParaPrint(extratodetalhes, reportItems);

                var reportViewer = new WebReportViewer<ImprimirExtratoBancarioVM>(ReportExtrato.Instance);
                return File(reportViewer.Print(reportItems, SessionManager.Current.UserData.PlatformUrl), "application/pdf");
            }
            catch (Exception ex)
            {
                var error = JsonConvert.DeserializeObject<ErrorInfo>(ex.Message);
                return JsonResponseStatus.GetFailure(error.Message);
            }
        }

        [OperationRole(PermissionValue = EPermissionValue.Read)]
        public List<ExtratoContaSaldoVM> GetContasBancarias()
        {
            return RestHelper.ExecuteGetRequest<List<ExtratoContaSaldoVM>>("extrato/impressaosaldos");
        }   

        [OperationRole(PermissionValue = EPermissionValue.Read)]
        public List<ExtratoDetalheVM> GetExtratoDetalhe(DateTime? dataInicial, DateTime? dataFinal, string contaBancariaId)
        {
            Dictionary<string, string> queryString = new Dictionary<string, string>
                {
                    { "dataInicial", dataInicial?.ToString("yyyy-MM-dd") },
                    { "dataFinal", dataFinal?.ToString("yyyy-MM-dd") },
                    { "contaBancariaId", contaBancariaId ?? string.Empty },
                };

            return RestHelper.ExecuteGetRequest<List<ExtratoDetalheVM>>("extrato/impressaoextratodetalhe", queryString);
        }

        private static void MontarExtratoParaPrint(ExtratoContaSaldoVM contabancaria, List<ImprimirExtratoBancarioVM> reportItems, DateTime? dataInicial, DateTime? dataFinal)
        {         
                reportItems.Add(new ImprimirExtratoBancarioVM
                {
                    ContaBancariaDescricao = contabancaria.ContaBancariaDescricao,
                    SaldoConsolidado = contabancaria.SaldoConsolidado,
                    DataInicial = dataInicial,
                    DataFinal = dataFinal
                });
        }

        private static void MontarDetalhesExtratoParaPrint(List<ExtratoDetalheVM> extratodetalhes, List<ImprimirExtratoBancarioVM> reportItems)
        {
            foreach (ExtratoDetalheVM itens in extratodetalhes)
            {
                reportItems.Add(new ImprimirExtratoBancarioVM
                {
                   Data = itens.DataMovimento,
                   ClienteFornecedor = itens.PessoaNome,
                   Valor = itens.ValorLancamento,
                   Lancamento = itens.DescricaoLancamento,
                   ContaBancaria = itens.ContaBancariaDescricao
                });
            }
        }

        #region ExportGrid 
      
        protected DataTable GridToDataTable(PagedResult<ExtratoDetalheVM> responseGrid, JQueryDataTableParams param)
        {
            DataTable dt = new DataTable();
            dt.Clear();

            param.Columns.ForEach(x =>
            {
                if (!string.IsNullOrWhiteSpace(x.Name))
                    dt.Columns.Add(x.Name);
            });

            var data = responseGrid.Data.Select(item => new
            {
                data = item.DataMovimento.ToString("dd/MM/yyyy"),
                descricaoLancamento = item.DescricaoLancamento,
                pessoaNome = item.PessoaNome,
                contaBancariaDescricao = item.ContaBancariaDescricao,
                valorLancamento = item.ValorLancamento.ToString("C", AppDefaults.CultureInfoDefault)
            }).ToList();
            Type o = data.FirstOrDefault().GetType();
            data.ForEach(x =>
            {
                DataRow dtr = dt.NewRow();
                param.Columns.ForEach(y =>
                {
                    if (!string.IsNullOrWhiteSpace(y.Name))
                        dtr[y.Name] = o.GetProperty(y.Data).GetValue(x, null);
                });
                dt.Rows.Add(dtr);
            });
            dt.Columns.Cast<DataColumn>().ToList().ForEach(column =>
            {
                if (dt.AsEnumerable().All(dr => dr.IsNull(column) || dr[column].ToString().Equals("")))
                    dt.Columns.Remove(column);
            });

            return dt;
        }

        protected override List<JQueryDataTableParamsColumn> GetParamsColumns(string ResourceName = "")
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}