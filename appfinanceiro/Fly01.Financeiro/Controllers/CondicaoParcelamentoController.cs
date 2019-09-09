using Fly01.Core;
using Fly01.Core.Entities.Domains.Enum;
using Fly01.Core.Helpers;
using Fly01.Core.Presentation;
using Fly01.Core.Presentation.Commons;
using Fly01.Core.Presentation.JQueryDataTable;
using Fly01.Core.Rest;
using Fly01.Core.ViewModels.Presentation.Commons;
using Fly01.uiJS.Classes;
using Fly01.uiJS.Classes.Elements;
using Fly01.uiJS.Defaults;
using Fly01.uiJS.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Fly01.Financeiro.Controllers
{
    public class CondicaoParcelamentoController<T> : BaseController<T> where T : CondicaoParcelamentoVM
    {
        [OperationRole(NotApply = true)]
        public JsonResult GridLoadSimulacao(string valorPrevisto, DateTime dataVencimento, string condicoesParcelamento, int? qtdParcelas)
        {
            JsonResult jsonResponse = new JsonResult();

            try
            {
                var dadosReferenciaSimulacao = new
                {
                    valorReferencia = valorPrevisto.Replace(",", "."),
                    dataReferencia = dataVencimento.ToString("yyyy-MM-dd"),
                    condicoesParcelamento = condicoesParcelamento,
                    qtdParcelas = qtdParcelas != null ? qtdParcelas.Value : 0
                };

                var listCondicoesParcelamento = RestHelper.ExecutePostRequest<ResponseSimulacaoVM>("condicaoparcelamentosimulacao", dadosReferenciaSimulacao);

                return Json(new
                {
                    success = true,
                    data = listCondicoesParcelamento.Items.Select(GetDisplayDataSimulacao()),
                    recordsFiltered = listCondicoesParcelamento.Items.Count,
                    recordsTotal = listCondicoesParcelamento.Items.Count
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = string.Format("Ocorreu um erro ao carregar dados: {0}", ex.Message) }, JsonRequestBehavior.AllowGet);
            }
        }

        public override Func<T, object> GetDisplayData()
        {
            return x => new
            {
                id = x.Id,
                descricao = x.Descricao,
                qtdParcelas = x.QtdParcelas,
                condicoesParcelamento = x.CondicoesParcelamento,
                valorReferencia = "",
                dataReferencia = DateTime.Now
            };
        }

        public Func<CondicaoParcelamentoParcelaVM, object> GetDisplayDataSimulacao()
        {
            return x => new
            {
                descricaoParcela = x.DescricaoParcela,
                dataVencimentoString = x.DataVencimentoString,
                valor = x.Valor.ToString("C", AppDefaults.CultureInfoDefault)
            };
        }

        public JsonResult PostCondicaoParcelamento(string term)
        {
            var entity = new CondicaoParcelamentoVM
            {
                Descricao = term,
                QtdParcelas = 1,
            };
            try
            {
                var resourceName = AppDefaults.GetResourceName(typeof(CondicaoParcelamentoVM));
                var data = RestHelper.ExecutePostRequest<PessoaVM>(resourceName, entity, AppDefaults.GetQueryStringDefault());

                return JsonResponseStatus.Get(new ErrorInfo() { HasError = false }, Operation.Create, data.Id);
            }
            catch (Exception ex)
            {
                var error = JsonConvert.DeserializeObject<ErrorInfo>(ex.Message);
                return JsonResponseStatus.GetFailure(error.Message);
            }
        }

        public override ContentResult List()
        {
            var cfg = new ContentUIBase(Url.Action("Sidebar", "Home"))
            {
                History = new ContentUIHistory { Default = Url.Action("Index") },
                Header = new HtmlUIHeader
                {
                    Title = "Condições de Parcelamento",
                    Buttons = new List<HtmlUIButton>(GetListButtonsOnHeader())
                },
                UrlFunctions = Url.Action("Functions", "CondicaoParcelamento", null, Request.Url.Scheme) + "?fns="
            };

            DataTableUI config = new DataTableUI { Id = "fly01dt", UrlGridLoad = Url.Action("GridLoad"), UrlFunctions = Url.Action("Functions", "CondicaoParcelamento", null, Request.Url.Scheme) + "?fns=" };

            config.Actions.AddRange(GetActionsInGrid(new List<DataTableUIAction>()
            {
                new DataTableUIAction { OnClickFn = "fnEditar", Label = "Editar", ShowIf = "row.registroFixo == 0" },
                new DataTableUIAction { OnClickFn = "fnExcluir", Label = "Excluir", ShowIf = "row.registroFixo == 0" }
            }));

            config.Columns.Add(new DataTableUIColumn { DataField = "descricao", DisplayName = "Descrição", Priority = 1 });

            cfg.Content.Add(config);

            return Content(JsonConvert.SerializeObject(cfg, JsonSerializerSetting.Front), "application/json");
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
                    Title = "Dados da condição de parcelamento",
                    Buttons = new List<HtmlUIButton>(GetFormButtonsOnHeader())                    
                },
                UrlFunctions = Url.Action("Functions", "CondicaoParcelamento", null, Request.Url.Scheme) + "?fns="
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
                ReadyFn = "fnFormReady",
                UrlFunctions = Url.Action("Functions", "CondicaoParcelamento", null, Request.Url.Scheme) + "?fns="
            };

            config.Elements.Add(new InputHiddenUI { Id = "id" });
            config.Elements.Add(new InputTextUI { Id = "descricao", Class = "col s6 l9", Label = "Descrição", Required = true });

            config.Elements.Add(new SelectUI
            {
                Id = "TipoSimulacao",
                Class = "col s6 l3",
                Label = "Tipo de Simulação",
                Required = true,
                MaxLength = 1,
                Options = new List<SelectOptionUI>(SystemValueHelper.GetUIElementBase(typeof(TipoSimulacao))),
                DomEvents = new List<DomEventUI>
                {
                    new DomEventUI { DomEvent = "change", Function = "fnChangeTipoSimulacao" }
                }
            });
            config.Elements.Add(new InputNumbersUI { Id = "qtdParcelas", Class = "col s6 l4", Label = "Quantidade de Parcelas", Disabled = true });

            config.Elements.Add(new InputTextUI
            {
                Id = "condicoesParcelamento",
                Class = "col s6 l8",
                Label = "Intervalo de dias",
                Disabled = true,
                DomEvents = new List<DomEventUI>
                {
                    new DomEventUI { DomEvent = "keyup", Function = "fnChangeCondicoesParcelamento" }
                }
            });

            config.Elements.Add(new LabelSetUI { Id = "simulatorLabel", Class = "col s12", Label = "Simular Condição de Parcelamento" });

            config.Elements.Add(new InputCurrencyUI { Id = "valorReferencia", Class = "col s6 l5", Label = "Valor Referência" });
            config.Elements.Add(new InputDateUI { Id = "dataReferencia", Class = "col s6 l5", Label = "Data Referência" });
            config.Elements.Add(new ButtonUI
            {
                Id = "btnRefresh",
                Class = "col s12 l2",
                Value = "Simular",
                DomEvents = new List<DomEventUI>
                {
                    new DomEventUI { DomEvent = "click", Function = "fnSimulaParcelamento" }
                }
            });

            config.Elements.Add(new TableUI
            {
                Id = "simulatorDataTable",
                Class = "col s12",
                Label = "Simulação",
                Options = new List<OptionUI>
                {
                    new OptionUI { Label = "Parcela", Value = "0"},
                    new OptionUI { Label = "Data", Value = "1"},
                    new OptionUI { Label = "Valor",Value = "2"}
                }
            });

            cfg.Content.Add(config);

            return cfg;
        }

        public ContentResult FormModal(string readyFn = "")
        {
            ModalUIForm config = new ModalUIForm()
            {
                Title = "Adicionar condição de parcelamento",
                ConfirmAction = new ModalUIAction() { Label = "Salvar" },
                CancelAction = new ModalUIAction() { Label = "Cancelar" },
                Action = new FormUIAction
                {
                    Create = @Url.Action("Create"),
                    Edit = @Url.Action("Edit"),
                    Get = @Url.Action("Json") + "/",
                },
                Id = "fly01mdlfrmCondicaoParcelamento",
                ReadyFn = string.IsNullOrEmpty(readyFn) ? "fnFormReady" : readyFn,
                UrlFunctions = Url.Action("Functions", "CondicaoParcelamento", null, Request.Url.Scheme) + "?fns="
            };

            config.Elements.Add(new InputHiddenUI { Id = "id" });
            config.Elements.Add(new InputTextUI { Id = "descricao", Class = "col s6 l6", Label = "Descrição", Required = true });

            config.Elements.Add(new SelectUI
            {
                Id = "TipoSimulacao",
                Class = "col s6 l6",
                Label = "Tipo de Simulação",
                Required = true,
                MaxLength = 1,
                Options = new List<SelectOptionUI>(SystemValueHelper.GetUIElementBase(typeof(TipoSimulacao))),
                DomEvents = new List<DomEventUI>
                {
                    new DomEventUI { DomEvent = "change", Function = "fnChangeTipoSimulacao" }
                }
            });
            config.Elements.Add(new InputNumbersUI { Id = "qtdParcelas", Class = "col s6 l4", Label = "Quantidade de Parcelas", Disabled = true });

            config.Elements.Add(new InputTextUI
            {
                Id = "condicoesParcelamento",
                Class = "col s6 l8",
                Label = "Intervalo de dias",
                Disabled = true,
                DomEvents = new List<DomEventUI>
                {
                    new DomEventUI { DomEvent = "keyup", Function = "fnChangeCondicoesParcelamento" }
                }
            });

            return Content(JsonConvert.SerializeObject(config, JsonSerializerSetting.Front), "application/json");
        }
    }
}
