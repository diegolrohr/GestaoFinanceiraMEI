using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Fly01.Compras.Controllers.Base;
using Fly01.Compras.Entities.ViewModel;
using Fly01.Core;
using Fly01.Core.Helpers;
using Fly01.uiJS.Classes;
using Newtonsoft.Json;
using Fly01.uiJS.Defaults;
using Fly01.uiJS.Classes.Elements;

namespace Fly01.Compras.Controllers
{
    public class CondicaoParcelamentoController : BaseController<CondicaoParcelamentoVM>
    {
        public JsonResult GridLoadSimulacao(string pValorReferencia, DateTime pDataReferencia, string pCondicoesParcelamento, int? pQtdParcelas)
        {
            JsonResult jsonResponse = new JsonResult();

            try
            {
                var dadosReferenciaSimulacao = new
                {
                    valorReferencia = pValorReferencia.Replace(",", "."),
                    dataReferencia = pDataReferencia.ToString("yyyy-MM-dd"),
                    condicoesParcelamento = pCondicoesParcelamento,
                    qtdParcelas = pQtdParcelas != null ? pQtdParcelas.Value : 0
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

        public override Func<CondicaoParcelamentoVM, object> GetDisplayData()
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

        public override ContentResult List()
        {
            var cfg = new ContentUI
            {
                History = new ContentUIHistory { Default = Url.Action("Index") },
                Header = new HtmlUIHeader
                {
                    Title = "Condições de Parcelamento",
                    Buttons = new List<HtmlUIButton>
                    {
                            new HtmlUIButton { Id = "new", Label = "Novo", OnClickFn = "fnNovo" }
                        }
                },
                UrlFunctions = Url.Action("Functions", "CondicaoParcelamento", null, Request.Url.Scheme) + "?fns="
            };

            DataTableUI config = new DataTableUI { UrlGridLoad = Url.Action("GridLoad"), UrlFunctions = Url.Action("Functions", "CondicaoParcelamento", null, Request.Url.Scheme) + "?fns=" };

            config.Actions.Add(new DataTableUIAction { OnClickFn = "fnEditar", Label = "Editar" });
            config.Actions.Add(new DataTableUIAction { OnClickFn = "fnExcluir", Label = "Excluir" });

            config.Columns.Add(new DataTableUIColumn { DataField = "descricao", DisplayName = "Descrição", Priority = 1 });

            cfg.Content.Add(config);

            return Content(JsonConvert.SerializeObject(cfg, JsonSerializerSetting.Front), "application/json");
        }

        public override ContentResult Form()
        {
            var cfg = new ContentUI
            {
                History = new ContentUIHistory
                {
                    Default = Url.Action("Create"),
                    WithParams = Url.Action("Edit")
                },
                Header = new HtmlUIHeader
                {
                    Title = "Dados da condição de parcelamento",
                    Buttons = new List<HtmlUIButton>
                    {
                        new HtmlUIButton { Id = "cancel", Label = "Cancelar", OnClickFn = "fnCancelar" },
                        new HtmlUIButton { Id = "save", Label = "Salvar", OnClickFn = "fnSalvar", Type = "submit" }
                    }
                },
                UrlFunctions = Url.Action("Functions", "CondicaoParcelamento", null, Request.Url.Scheme) + "?fns="
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
                Options = new List<SelectOptionUI>(SystemValueHelper.GetUIElementBase("TipoSimulacao", true, false)),
                DomEvents = new List<DomEventUI>
                {
                    new DomEventUI { DomEvent = "change", Function = "fnChangeTipoSimulacao" }
                }
            });
            config.Elements.Add(new InputNumbersUI { Id = "qtdParcelas", Class = "col s6 l4", Label = "Quantidade de Parcelas", Disabled = true });
            //(Informe as condições separadas por vírgula. Exemplo (30,60,90))
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

            config.Elements.Add(new LabelsetUI { Id = "simulatorLabel", Class = "col s12", Label = "Simular Condição de Parcelamento" });

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

            return Content(JsonConvert.SerializeObject(cfg, JsonSerializerSetting.Front), "application/json");
        }

        public ContentResult FormModal()
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
                ReadyFn = "fnFormReady",
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
                Options = new List<SelectOptionUI>(SystemValueHelper.GetUIElementBase("TipoSimulacao", true, false)),
                DomEvents = new List<DomEventUI>
                {
                    new DomEventUI { DomEvent = "change", Function = "fnChangeTipoSimulacao" }
                }
            });
            config.Elements.Add(new InputNumbersUI { Id = "qtdParcelas", Class = "col s6 l4", Label = "Quantidade de Parcelas", Disabled = true });
            //(Informe as condições separadas por vírgula. Exemplo (30,60,90))
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