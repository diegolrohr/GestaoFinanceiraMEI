using Fly01.Core.Defaults;
using Fly01.Core.Presentation;
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

namespace Fly01.Financeiro.Controllers
{
    //[OperationRole(ResourceKey = ResourceHashConst.FinanceiroCadastrosCategoria)]
    public class MinhaContaController : BaseController<MinhaContaVM>
    {

        public override ContentResult List()
        {
            var dataInicialFiltroDefault = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            var dataFinalFiltroDefault = DateTime.Now;

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
                        new HtmlUIButton { Id = "new", Label = "Novo", OnClickFn = "fnNovo" }
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
                        Class = "col s6 m3 l6",
                        Label = "Data Inicial",
                        Value = dataInicialFiltroDefault.ToString("dd/MM/yyyy"),
                        //DomEvents = new List<DomEventUI> { new DomEventUI {DomEvent = "change", Function = "fnAtualizar"} },
                        Max = true,
                        Min = -180
                    },
                    new InputDateUI
                    {
                        Id = "dataFinal",
                        Class = "col s6 m3 l6",
                        Label = "Data Final",
                        Value = dataFinalFiltroDefault.ToString("dd/MM/yyyy"),
                       // DomEvents = new List<DomEventUI> { new DomEventUI {DomEvent = "change", Function = "fnAtualizar"} },
                        Max = true,
                        Min = -180
                    },
                }
            });

            var config = new DataTableUI
            {
                Id = "fly01dt",
                UrlGridLoad = Url.Action("LoadMinhasContas"),
                UrlFunctions = Url.Action("Functions") + "?fns=",
                Parameters = new List<DataTableUIParameter>
                {
                    new DataTableUIParameter() {Id = "dataInicial"},
                    new DataTableUIParameter() {Id = "dataFinal" }
                },
            };

            config.Actions.AddRange(GetActionsInGrid(new List<DataTableUIAction>()
            {
                new DataTableUIAction { OnClickFn = "fnEditar", Label = "Editar" },
                new DataTableUIAction { OnClickFn = "fnExcluir", Label = "Excluir" }
            }));

            config.Columns.Add(new DataTableUIColumn { DataField = "emicao", DisplayName = "Emissão", Priority = 1 });
            config.Columns.Add(new DataTableUIColumn { DataField = "numero", DisplayName = "Numero", Priority = 2 });
            config.Columns.Add(new DataTableUIColumn { DataField = "valor", DisplayName = "Valor", Priority = 3 });
            config.Columns.Add(new DataTableUIColumn { DataField = "vencimento", DisplayName = "Vencimento", Priority = 4 });
            config.Columns.Add(new DataTableUIColumn { DataField = "descricao", DisplayName = "Descrição", Priority = 5 });
            config.Columns.Add(new DataTableUIColumn { DataField = "observacao", DisplayName = "Observação", Priority = 4 });

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

                Dictionary<string, string> queryString = new Dictionary<string, string>
                {
                        { "codigoMaxime", "T94517"},
                        { "vencimentoInicial", "2018-10-15"},
                        { "vencimentoFinal", "2019-04-15"},
                        { "posicao", "TODOS"}
                };

                var minhasContas = RestHelper.ExecutePostRequest<List<MinhaContaVM>>("minhaConta", queryString);

                return Json(new
                {
                    success = true,
                    recordsTotal = minhasContas.Count,
                    recordsFiltered = minhasContas.Count,
                    data = minhasContas.Select(GetDisplayMinhaConta())
                }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return JsonResponseStatus.GetFailure(ex.Message);
            }
        }

        protected Func<MinhaContaVM, object> GetDisplayMinhaConta()
        {
            return x => new
            {
                id = x.Id,
                descricao = x.Descricao,
                valor = Math.Round(x.Valor, 2, MidpointRounding.AwayFromZero),
                emissao = x.DataEmissao,
                observacao = x.Observacao,
                vencimento = x.Vencimento,
                numero = x.Numero
            };
        }

        public override Func<MinhaContaVM, object> GetDisplayData()
        {
            throw new NotImplementedException();
        }
    }
}