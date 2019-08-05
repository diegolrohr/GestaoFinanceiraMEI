using Fly01.Core.Config;
using Fly01.Core.Defaults;
using Fly01.Core.Helpers;
using Fly01.Core.Presentation.Commons;
using Fly01.Core.Presentation.JQueryDataTable;
using Fly01.Core.Rest;
using Fly01.Core.ViewModels.Presentation.Commons;
using Fly01.uiJS.Classes;
using Fly01.uiJS.Classes.Elements;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Fly01.Core.Presentation.Controllers
{
    public class MinhaContaBaseController : BaseController<MinhaContaVM>
    {
        public override Func<MinhaContaVM, object> GetDisplayData()
        {
            return x => new
            {
                id = (x.Numero + x.Vencimento.ToString("ddMMyyyy")),
                descricao = x.Descricao,
                valor = x.Valor.ToString("C", AppDefaults.CultureInfoDefault),
                emissao = x.Emissao.ToString("dd/MM/yyyy"),
                observacao = x.Observacao,
                vencimento = x.Vencimento.ToString("dd/MM/yyyy"),
                boletoVencido = x.Vencimento.Date <= DateTime.Now.Date,
                numero = x.Numero,
                situacao = x.Situacao,
                urlBoleto = x.UrlBoleto,
                urlSegundaVia = "https://www.itau.com.br/servicos/boletos/atualizar/",
                nfe = x.NFE,
                parcela = x.Parcela,
                codigoBarrasFormatado = x.CodigoBarrasFormatado
            };
        }

        public override ContentResult List()
        {
            var dateNow = DateTime.Now;
            var dataInicialFiltroDefault = dateNow.AddMonths(-1);
            var dataFinalFiltroDefault = dateNow.AddMonths(1);

            var cfg = new ContentUIBase(Url.Action("Sidebar", "Home"))
            {
                History = new ContentUIHistory
                {
                    Default = Url.Action("Index")
                },
                Header = new HtmlUIHeader
                {
                    Title = "Minha Conta",
                    Buttons = new List<HtmlUIButton>
                    {
                        new HtmlUIButton { Id = "new", Label = "Atualizar", OnClickFn = "fnAtualizar" }
                    }
                },
                UrlFunctions = Url.Action("Functions") + "?fns="
            };

            cfg.Content.Add(new FormUI
            {
                Elements = new List<BaseUI>
                {
                    new InputHiddenUI {Id = "contaBancariaId"},
                    new InputDateUI
                    {
                        Id = "dataInicial",
                        Class = "col s12 m6",
                        Label = "Data Inicial",
                        Value = dataInicialFiltroDefault.ToString("dd/MM/yyyy")
                    },
                    new InputDateUI
                    {
                        Id = "dataFinal",
                        Class = "col s12 m6",
                        Label = "Data Final",
                        Value = dataFinalFiltroDefault.ToString("dd/MM/yyyy")
                    }
                }
            });

            var config = new DataTableUI
            {
                Id = "fly01dtminhaconta",
                UrlGridLoad = Url.Action("LoadMinhasContas"),
                UrlFunctions = Url.Action("Functions") + "?fns=",
                Parameters = new List<DataTableUIParameter>
                {
                    new DataTableUIParameter() { Id = "dataInicial" },
                    new DataTableUIParameter() { Id = "dataFinal" }
                },
            };

            config.Columns.Add(new DataTableUIColumn { DataField = "emissao", DisplayName = "Emissão", Priority = 8, Orderable = false, Searchable = false });
            config.Columns.Add(new DataTableUIColumn { DataField = "numero", DisplayName = "Número", Priority = 7, Orderable = false, Searchable = false });
            config.Columns.Add(new DataTableUIColumn { DataField = "nfe", DisplayName = "NFe", Priority = 9, Orderable = false, Searchable = false });
            config.Columns.Add(new DataTableUIColumn { DataField = "descricao", DisplayName = "Descrição", Priority = 5, Orderable = false, Searchable = false });
            config.Columns.Add(new DataTableUIColumn { DataField = "vencimento", DisplayName = "Vencimento", Priority = 3, Orderable = false, Searchable = false, RenderFn = "fnRenderVencimento" });
            config.Columns.Add(new DataTableUIColumn { DataField = "valor", DisplayName = "Valor", Priority = 1, Orderable = false, Searchable = false });
            config.Columns.Add(new DataTableUIColumn { DataField = "parcela", DisplayName = "Parcela", Priority = 10, Orderable = false, Searchable = false });
            config.Columns.Add(new DataTableUIColumn { DisplayName = "Situação", Priority = 6, Searchable = false, Orderable = false, RenderFn = "fnRenderSituacao" });
            config.Columns.Add(new DataTableUIColumn { DisplayName = "Ações", Priority = 2, Searchable = false, Orderable = false, RenderFn = "fnRenderBoletos", Width = "20%" });

            cfg.Content.Add(config);

            return Content(JsonConvert.SerializeObject(cfg, JsonSerializerSetting.Front), "application/json");
        }

        protected override ContentUI FormJson()
        {
            throw new NotImplementedException();
        }

        public JsonResult LoadMinhasContas(DateTime dataInicial, DateTime dataFinal)
        {
            try
            {
                var param = JQueryDataTableParams.CreateFromQueryString(Request.QueryString);

                var configuracao = new MinhaContaConfiguracaoVM()
                {
                    //CodigoMaxime = "T94517",
                    CodigoMaxime = SessionManager.Current.UserData.TokenData.CodigoMaxime,
                    VencimentoInicial = dataInicial,
                    VencimentoFinal = dataFinal,
                    Posicao = "TODOS"
                };

                var minhasContas = RestHelper.ExecutePostRequest<List<MinhaContaVM>>("minhaConta", configuracao);

                return Json(new
                {
                    success = true,
                    recordsTotal = minhasContas.Count,
                    recordsFiltered = minhasContas.Count,
                    data = minhasContas.Select(GetDisplayData())
                }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(new
                {
                    queryStringFilter = string.Empty,
                    recordsTotal = 0,
                    recordsFiltered = 0,
                    data = new { },
                    success = false,
                    message = string.Format("Ocorreu um erro ao carregar dados: {0}", ex.Message)
                }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult CodigoBarras(string urlBoleto, string id)
        {
            try
            {
                //http://www.secretgeek.net/uri_enconding
                var queryStringCodigo = new Dictionary<string, string>
                {
                    { "urlBoleto", Uri.EscapeUriString(urlBoleto)}
                };
                var response = RestHelper.ExecuteGetRequest<MinhaContaCodigoBarrasResponseVM>(AppDefaults.UrlGatewayNew, "boletos/codigobarras", queryStringCodigo)?.Data;
                return Json(new
                {
                    success = true,
                    id = id,
                    codigoBarras = response.CodigoBarras,
                    codigoBarrasFormatado = response.CodigoBarrasFormatado,
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new
                {
                    success = false,
                    numero = string.Empty,
                    codigoBarras = string.Empty,
                    codigoBarrasFormatado = string.Empty,
                }, JsonRequestBehavior.AllowGet);
            }
        }

        protected override List<JQueryDataTableParamsColumn> GetParamsColumns(string ResourceName = "")
        {
            throw new NotImplementedException();
        }
    }
}