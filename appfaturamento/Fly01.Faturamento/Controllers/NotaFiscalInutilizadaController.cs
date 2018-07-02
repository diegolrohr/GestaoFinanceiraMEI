using Fly01.Faturamento.ViewModel;
using Fly01.uiJS.Classes;
using Fly01.uiJS.Defaults;
using Fly01.Core.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Fly01.uiJS.Classes.Elements;
using Newtonsoft.Json.Linq;
using Fly01.Core.Presentation.Commons;
using Fly01.Core.Rest;
using Fly01.Core.Entities.Domains.Enum;
using Fly01.Core.Presentation;

namespace Fly01.Faturamento.Controllers
{
    public class NotaFiscalInutilizadaController : BaseController<NotaFiscalInutilizadaVM>
    {
        public override Func<NotaFiscalInutilizadaVM, object> GetDisplayData()
        {
            return x => new
            {
                id = x.Id.ToString(),
                serie = x.Serie,
                numNotaFiscal = x.NumNotaFiscal,
                status = x.Status,
                statusDescription = EnumHelper.GetDescription(typeof(StatusNotaFiscal), x.Status),
                statusCssClass = EnumHelper.GetCSS(typeof(StatusNotaFiscal), x.Status),
                statusValue = EnumHelper.GetValue(typeof(StatusNotaFiscal), x.Status),
                data = x.Data.ToString("dd/MM/yyyy"),
                sefazChaveAcesso = x.SefazChaveAcesso,
                mensagem = x.Mensagem,
                recomendacao = x.Recomendacao
            };
        }

        public override ContentResult Form()
        {
            throw new NotImplementedException();
        }

        public override ContentResult List()
        {
            var cfg = new ContentUI
            {
                History = new ContentUIHistory { Default = Url.Action("Index") },
                Header = new HtmlUIHeader
                {
                    Title = "Notas Fiscais Inutilizadas",
                    Buttons = new List<HtmlUIButton>
                    {
                        new HtmlUIButton { Id = "atualizarStatusInutilizada", Label = "Atualizar Status", OnClickFn = "fnAtualizarStatusInutilizada" },
                        new HtmlUIButton { Id = "new", Label = "Nova Inutilização", OnClickFn = "fnNovo" },
                    }
                },
                UrlFunctions = Url.Action("Functions") + "?fns="
            };

            //    var cfgForm = new FormUI
            //    {
            //        ReadyFn = "fnUpdateDataFinal",
            //        UrlFunctions = Url.Action("Functions") + "?fns=",
            //        Elements = new List<BaseUI>()
            //        {
            //            new PeriodPickerUI
            //            {
            //                Label = "Selecione o período",
            //                Id = "mesPicker",
            //                Name = "mesPicker",
            //                Class = "col s12 m6 offset-m3 l4 offset-l4",
            //                DomEvents = new List<DomEventUI>()
            //                {
            //                    new DomEventUI()
            //                    {
            //                        DomEvent = "change",
            //                        Function = "fnUpdateDataFinal"
            //                    }
            //                }
            //            },
            //            new InputHiddenUI()
            //            {
            //                Id = "dataFinal",
            //                Name = "dataFinal"
            //            },
            //            new InputHiddenUI()
            //            {
            //                Id = "dataInicial",
            //                Name = "dataInicial"
            //            }
            //        }

            //    };

            //cfg.Content.Add(cfgForm);

            //var config = new DataTableUI
            //{
            //    UrlGridLoad = Url.Action(),
            //    Parameters = new List<DataTableUIParameter>
            //    {
            //        new DataTableUIParameter() {Id = "dataInicial", Required = (gridLoad == "GridLoad") },
            //        new DataTableUIParameter() {Id = "dataFinal", Required = (gridLoad == "GridLoad") }
            //    },
            //    UrlFunctions = Url.Action("Functions") + "?fns=",
            //    Functions = new List<string>() { "fnRenderEnum" }
            //};

            var config = new DataTableUI { UrlGridLoad = Url.Action("GridLoad"), UrlFunctions = Url.Action("Functions") + "?fns=" };

            //config.Actions.Add(new DataTableUIAction { OnClickFn = "fnExcluir", Label = "Cancelar", ShowIf = "((row.status == 'Autorizada' || row.status == 'FalhaNoCancelamento') && row.tipoNotaFiscal == 'NFSe')" });
            //TODO: Diego retransmitir

            config.Columns.Add(new DataTableUIColumn { DataField = "serie", DisplayName = "Série", Priority = 1 });
            config.Columns.Add(new DataTableUIColumn { DataField = "numNotaFiscal", DisplayName = "Número NF", Priority = 2, Type = "numbers" });
            config.Columns.Add(new DataTableUIColumn
            {
                DataField = "status",
                DisplayName = "Status",
                Priority = 3,
                Options = new List<SelectOptionUI>(SystemValueHelper.GetUIElementBase(typeof(StatusNotaFiscal))),
                RenderFn = "function(data, type, full, meta) { return fnRenderEnum(full.statusCssClass, full.statusDescription); }"
            });
            config.Columns.Add(new DataTableUIColumn { DataField = "sefazChaveAcesso", DisplayName = "Sefaz Chave Acesso", Priority = 4 });
            config.Columns.Add(new DataTableUIColumn { DataField = "data", DisplayName = "Data", Priority = 5, Type = "date" });

            cfg.Content.Add(config);

            return Content(JsonConvert.SerializeObject(cfg, JsonSerializerSetting.Front), "application/json");
        }

        //public override JsonResult GridLoad(Dictionary<string, string> filters = null)
        //{
        //    if (filters == null)
        //        filters = new Dictionary<string, string>();

        //    filters.Add("data le ", Request.QueryString["dataFinal"]);
        //    filters.Add(" and data ge ", Request.QueryString["dataInicial"]);

        //    return base.GridLoad(filters);
        //}

        //public JsonResult GridLoadNoFilter()
        //{
        //    return base.GridLoad();
        //}

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