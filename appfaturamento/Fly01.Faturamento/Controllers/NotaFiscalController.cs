using Fly01.Faturamento.Controllers.Base;
using Fly01.Faturamento.Entities.ViewModel;
using Fly01.uiJS.Classes;
using Fly01.uiJS.Defaults;
using Fly01.Core.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Fly01.uiJS.Classes.Elements;
using Newtonsoft.Json.Linq;
using Fly01.Core.API;
using Fly01.Core.Presentation.Commons;
using Fly01.Core.Rest;

namespace Fly01.Faturamento.Controllers
{
    public class NotaFiscalController : BaseController<NotaFiscalVM>
    {
        public NotaFiscalController()
        {
            ExpandProperties = "cliente($select=nome),ordemVendaOrigem($select=id,numero),categoria,serieNotaFiscal";
        }

        //NFeVM e NFSeVM na mesma controller notaFiscal, direcionado as controller via javaScript
        public override Func<NotaFiscalVM, object> GetDisplayData()
        {
            return x => new
            {
                id = x.Id.ToString(),
                numero = x.Numero.ToString(),
                tipoNotaFiscal = x.TipoNotaFiscal,
                tipoNotaFiscalDescription = EnumHelper.SubtitleDataAnotation("TipoNotaFiscal", x.TipoNotaFiscal).Description,
                tipoNotaFiscalCssClass = EnumHelper.SubtitleDataAnotation("TipoNotaFiscal", x.TipoNotaFiscal).CssClass,
                tipoNotaFiscalValue = EnumHelper.SubtitleDataAnotation("TipoNotaFiscal", x.TipoNotaFiscal).Value,
                status = x.Status,
                statusDescription = EnumHelper.SubtitleDataAnotation("StatusNotaFiscal", x.Status).Description,
                statusCssClass = EnumHelper.SubtitleDataAnotation("StatusNotaFiscal", x.Status).CssClass,
                statusValue = EnumHelper.SubtitleDataAnotation("StatusNotaFiscal", x.Status).Value,
                data = x.Data.ToString("dd/MM/yyyy"),
                cliente_nome = x.Cliente.Nome,
                ordemVendaOrigem_numero = x.OrdemVendaOrigem.Numero.ToString(),
                tipoVenda = x.TipoVenda,
                tipoVendaDescription = EnumHelper.SubtitleDataAnotation("TipoVenda", x.TipoVenda).Description,
                tipoVendaCssClass = EnumHelper.SubtitleDataAnotation("TipoVenda", x.TipoVenda).CssClass,
                tipoVendaValue = EnumHelper.SubtitleDataAnotation("TipoVenda", x.TipoVenda).Value,
                categoria_descrica = x.Categoria != null ? x.Categoria.Descricao : "",
                numNotaFiscal = x.NumNotaFiscal,
                serieNotaFiscal_serie = x.SerieNotaFiscal != null ? x.SerieNotaFiscal.Serie : ""
            };
        }

        public override ContentResult Form()
        {
            throw new NotImplementedException();
        }

        public override ContentResult List()
        {
            return ListNotaFiscal();
        }

        public ContentResult ListNotaFiscal(string gridLoad = "GridLoad")
        {
            var buttonLabel = "Mostrar todas as notas";
            var buttonOnClick = "fnRemoveFilter";

            if (Request.QueryString["action"] == "GridLoadNoFilter")
            {
                gridLoad = Request.QueryString["action"];
                buttonLabel = "Mostrar notas do mês atual";
                buttonOnClick = "fnAddFilter";
            }

            var cfg = new ContentUI
            {
                History = new ContentUIHistory { Default = Url.Action("Index") },
                Header = new HtmlUIHeader
                {
                    Title = "Notas Fiscais",
                    Buttons = new List<HtmlUIButton>
                    {
                        new HtmlUIButton { Id = "atualizarStatus", Label = "Atualizar Status", OnClickFn = "fnAtualizarStatus" },
                        new HtmlUIButton { Id = "new", Label = "Novo Pedido", OnClickFn = "fnNovoPedido" },
                        new HtmlUIButton { Id = "filterGrid", Label = buttonLabel, OnClickFn = buttonOnClick },
                    }
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
                        new PeriodpickerUI
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
                UrlFunctions = Url.Action("Functions") + "?fns="
            };

            config.Actions.Add(new DataTableUIAction { OnClickFn = "fnVisualizarNFe", Label = "Visualizar", ShowIf = "(row.tipoNotaFiscal == 'NFe')" });
            config.Actions.Add(new DataTableUIAction { OnClickFn = "fnVisualizarRetornoSefaz", Label = "Mensagem SEFAZ", ShowIf = "(row.status != 'NaoTransmitida' && row.tipoNotaFiscal == 'NFe')" });
            config.Actions.Add(new DataTableUIAction { OnClickFn = "fnVisualizarNFSe", Label = "Visualizar", ShowIf = "(row.tipoNotaFiscal == 'NFSe')" });
            config.Actions.Add(new DataTableUIAction { OnClickFn = "fnTransmitirNFe", Label = "Transmitir", ShowIf = "((row.status == 'NaoAutorizada' || row.status == 'NaoTransmitida' || row.status == 'FalhaTransmissao') && row.tipoNotaFiscal == 'NFe')" });
            config.Actions.Add(new DataTableUIAction { OnClickFn = "fnTransmitirNFSe", Label = "Transmitir", ShowIf = "((row.status == 'NaoAutorizada' || row.status == 'NaoTransmitida' || row.status == 'FalhaTransmissao') && row.tipoNotaFiscal == 'NFSe')" });
            config.Actions.Add(new DataTableUIAction { OnClickFn = "fnBaixarXMLNFe", Label = "Baixar XML", ShowIf = "((row.status == 'NaoAutorizada' || row.status == 'Autorizada') && row.tipoNotaFiscal == 'NFe')" });
            config.Actions.Add(new DataTableUIAction { OnClickFn = "fnBaixarPDFNFe", Label = "Baixar PDF", ShowIf = "(row.status == 'Autorizada' && row.tipoNotaFiscal == 'NFe')" });
            config.Actions.Add(new DataTableUIAction { OnClickFn = "fnBaixarXMLNFSe", Label = "Baixar XML", ShowIf = "((row.status == 'NaoAutorizada' || row.status == 'Autorizada') && row.tipoNotaFiscal == 'NFSe')" });
            config.Actions.Add(new DataTableUIAction { OnClickFn = "fnBaixarPDFNFSe", Label = "Baixar PDF", ShowIf = "(row.status == 'Autorizada' && row.tipoNotaFiscal == 'NFSe')" });
            config.Actions.Add(new DataTableUIAction { OnClickFn = "fnCancelarNFe", Label = "Cancelar", ShowIf = "((row.status == 'Autorizada' || row.status == 'FalhaNoCancelamento') && row.tipoNotaFiscal == 'NFe')" });
            config.Actions.Add(new DataTableUIAction { OnClickFn = "fnCancelarNFSe", Label = "Cancelar", ShowIf = "((row.status == 'Autorizada' || row.status == 'FalhaNoCancelamento') && row.tipoNotaFiscal == 'NFSe')" });

            config.Columns.Add(new DataTableUIColumn { DataField = "serieNotaFiscal_serie", DisplayName = "Série", Priority = 1 });
            config.Columns.Add(new DataTableUIColumn { DataField = "numNotaFiscal", DisplayName = "Número NF", Priority = 2, Type = "numbers" });
            config.Columns.Add(new DataTableUIColumn
            {
                DataField = "status",
                DisplayName = "Status",
                Priority = 3,
                Options = new List<SelectOptionUI>(SystemValueHelper.GetUIElementBase("StatusNotaFiscal", true, false)),
                RenderFn = "function(data, type, full, meta) { return \"<span class=\\\"new badge \" + full.statusCssClass + \" left\\\" data-badge-caption=\\\" \\\">\" + full.statusDescription + \"</span>\" }"
            });
            config.Columns.Add(new DataTableUIColumn
            {
                DataField = "tipoNotaFiscal",
                DisplayName = "Tipo",
                Priority = 4,
                Options = new List<SelectOptionUI>(SystemValueHelper.GetUIElementBase("TipoNotaFiscal", true, false)),
                RenderFn = "function(data, type, full, meta) { return \"<span class=\\\"new badge \" + full.tipoNotaFiscalCssClass + \" left\\\" data-badge-caption=\\\" \\\">\" + full.tipoNotaFiscalDescription + \"</span>\" }"
            });
            config.Columns.Add(new DataTableUIColumn { DataField = "cliente_nome", DisplayName = "Cliente", Priority = 5 });
            config.Columns.Add(new DataTableUIColumn { DataField = "data", DisplayName = "Data", Priority = 6, Type = "date" });
            config.Columns.Add(new DataTableUIColumn { DataField = "ordemVendaOrigem_numero", DisplayName = "Pedido Origem", Searchable = false, Priority = 7 });//numero int e pesquisa string
            //config.Columns.Add(new DataTableUIColumn
            //{
            //    DataField = "tipoVenda",
            //    DisplayName = "Tipo Venda",
            //    Priority = 7,
            //    Options = new List<SelectOptionUI>(SystemValueHelper.GetUIElementBase("TipoVenda", true, false)),
            //    RenderFn = "function(data, type, full, meta) { return \"<span class=\\\"new badge \" + full.tipoVendaCssClass + \" left\\\" data-badge-caption=\\\" \\\">\" + full.tipoVendaDescription + \"</span>\" }"
            //});
            config.Columns.Add(new DataTableUIColumn { DataField = "categoria_descricao", DisplayName = "Categoria", Priority = 8 });

            cfg.Content.Add(config);

            return Content(JsonConvert.SerializeObject(cfg, JsonSerializerSetting.Front), "application/json");
        }

        public override Dictionary<string, string> GetQueryStringDefaultGridLoad()
        {
            var customFilters = base.GetQueryStringDefaultGridLoad();
            customFilters.AddParam("$orderby", "data");

            return customFilters;
        }

        public override JsonResult GridLoad(Dictionary<string, string> filters = null)
        {
            if (filters == null)
                filters = new Dictionary<string, string>();

            filters.Add("data le ", Request.QueryString["dataFinal"]);
            filters.Add(" and data ge ", Request.QueryString["dataInicial"]);

            return base.GridLoad(filters);
        }

        public JsonResult GridLoadNoFilter()
        {
            return base.GridLoad();
        }

        [HttpGet]
        public JsonResult TotalNotaFiscal(string id, double? valorFreteCIF = 0)
        {
            try
            {
                var resource = string.Format("CalculaTotalNotaFiscal?&notaFiscalId={0}&valorFreteCIF={1}", id, valorFreteCIF.ToString().Replace(",", "."));
                var response = RestHelper.ExecuteGetRequest<TotalNotaFiscalVM>(resource, queryString: null);

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

        [HttpGet]
        public JsonResult AtualizaStatus()
        {
            try
            {
                var response = RestHelper.ExecuteGetRequest<JObject>("NotaFiscalAtualizaStatus", queryString: null);

                return Json(
                    new { success = true },
                    JsonRequestBehavior.AllowGet
                );
            }
            catch (Exception ex)
            {
                var error = JsonConvert.DeserializeObject<ErrorInfo>(ex.Message);
                return JsonResponseStatus.GetFailure(error.Message);
            }
        }

    }
}