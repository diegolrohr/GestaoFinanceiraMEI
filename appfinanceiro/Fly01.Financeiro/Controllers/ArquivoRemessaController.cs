using System;
using System.Web.Mvc;
using Fly01.Financeiro.Controllers.Base;
using Fly01.Financeiro.ViewModel;
using Fly01.uiJS.Classes;
using System.Collections.Generic;
using Newtonsoft.Json;
using Fly01.uiJS.Defaults;
using Fly01.uiJS.Classes.Elements;
using Fly01.Core.Presentation.Commons;
using Fly01.Core.Entities.Domains.Enum;
using Fly01.Core.Presentation.JQueryDataTable;
using Fly01.Core.Helpers;
using Fly01.Core.Rest;
using System.Linq;
using Fly01.Core;

namespace Fly01.Financeiro.Controllers
{
    public class ArquivoRemessaController : BaseController<ArquivoRemessaVM>
    {
        public override Func<ArquivoRemessaVM, object> GetDisplayData()
        {
            return x => new
            {
                id = x.Id,
                descricao = x.Descricao,
                valorTotal = x.ValorTotal,
                dataExportacao = x.DataExportacao.ToString("dd/MM/yyyy"),
                totalBoletos = x.TotalBoletos,
                status = x.StatusArquivoRemessa
            };
        }

        private PagedResult<CnabVM> GetContasReceber(Guid idArquivo, int pageNo)
        {
            var queryString = new Dictionary<string, string>
            {
                { "arquivoRemessaId", idArquivo.ToString()},
                { "pageNo", pageNo.ToString() },
                { "pageSize", "10"}
            };

            return RestHelper.ExecuteGetRequest<PagedResult<CnabVM>>("cnab/contasReceberarquivo", queryString);
        }

        public JsonResult LoadGridBoletos(Guid IdArquivo)
        {
            var param = JQueryDataTableParams.CreateFromQueryString(Request.QueryString);
            var pageNo = param.Start > 0 ? (param.Start / 10) + 1 : 1;

            var response = GetContasReceber(IdArquivo, pageNo);

            return Json(new
            {
                recordsTotal = response.Paging.TotalRecordCount,
                recordsFiltered = response.Paging.TotalRecordCount,
                data = response.Data.Select(item => new
                {
                    nossoNumero = item.NossoNumero,
                    pessoa_nome = item.ContaReceber.Pessoa.Nome,
                    valorBoleto = item.ValorBoleto.ToString("C", AppDefaults.CultureInfoDefault),
                    dataEmissao = item.DataEmissao.ToString("dd/MM/yyyy"),
                    dataVencimento = item.DataVencimento.ToString("dd/MM/yyyy"),
                    statusArquivoRemessa = item.Status
                })

            }, JsonRequestBehavior.AllowGet);
        }

        public override ContentResult Form()
        {
            var cfg = new ContentUI
            {
                History = new ContentUIHistory()
                {
                    Default = Url.Action("Create"),// Tem que revisar(Colocar Edit)
                },
                Header = new HtmlUIHeader()
                {
                    Title = "Lista de boletos do arquivo",
                    Buttons = new List<HtmlUIButton>()
                    {
                        new HtmlUIButton() { Id = "cancel", Label = "Voltar", OnClickFn = "fnCancelar" }
                    }
                },
                UrlFunctions = Url.Action("Functions") + "?fns="
            };

            var config = new FormUI
            {
                Action = new FormUIAction
                {
                    Edit = @Url.Action("Edit"),
                    Get = @Url.Action("Json") + "/",
                    List = @Url.Action("List")
                },
                //ReadyFn = "fnFormReady",
                //UrlFunctions = Url.Action("Functions", "ContaReceber", null, Request.Url.Scheme) + "?fns="
            };

            config.Elements.Add(new InputHiddenUI { Id = "id" });
            //config.Elements.Add(new LabelsetUI)  Verificar elemento Label  para inserir dados.

            cfg.Content.Add(new DataTableUI
            {
                Class = "col s12",
                UrlGridLoad = Url.Action("LoadGridBoletos", "Cnab"),
                Parameters = new List<DataTableUIParameter>
                {
                    new DataTableUIParameter { Id = "Id" }
                },
                Options = new DataTableUIConfig()
                {
                    PageLength = 10
                },
                Columns = new List<DataTableUIColumn>
                {
                    new DataTableUIColumn { DataField = "numero", DisplayName = "Nº", Priority = 1, Orderable = false, Searchable = false, Type = "number" },
                    new DataTableUIColumn { DataField = "pessoa_nome", DisplayName = "Cliente", Priority = 2, Orderable = false, Searchable = false },
                    new DataTableUIColumn { DataField = "valorBoleto", DisplayName = "Valor", Priority = 3, Orderable = false, Searchable = false, Type = "currency" },
                    new DataTableUIColumn { DataField = "dataEmissao", DisplayName = "Data emissão", Priority = 4, Orderable = false, Searchable = false, Type = "date" },
                    new DataTableUIColumn { DataField = "dataVencimento", DisplayName = "Data Vencimento", Priority = 5, Orderable = false, Searchable = false, Type = "date" },
                    new DataTableUIColumn { DataField = "statusArquivoRemessa", DisplayName = "Status", Priority = 6, Options = new List<SelectOptionUI>(SystemValueHelper.GetUIElementBase(typeof(StatusArquivoRemessa)))}
                }
            });

            return Content(JsonConvert.SerializeObject(cfg, JsonSerializerSetting.Front), "application/json");
        }

        public override ContentResult List()
        {
            var cfg = new ContentUI
            {
                History = new ContentUIHistory { Default = Url.Action("Index") },
                Header = new HtmlUIHeader { Title = "Arquivos remessa" },
                UrlFunctions = Url.Action("Functions") + "?fns="
            };
            var config = new DataTableUI { UrlGridLoad = Url.Action("GridLoad"), UrlFunctions = Url.Action("Functions") + "?fns=" };

            config.Actions.Add(new DataTableUIAction { OnClickFn = "fnEditar", Label = "Listar Boletos" });

            config.Columns.Add(new DataTableUIColumn { DataField = "descricao", DisplayName = "Arquivo", Priority = 2 });
            config.Columns.Add(new DataTableUIColumn { DataField = "dataExportacao", DisplayName = "Vencimento", Priority = 3, Type = "date" });
            config.Columns.Add(new DataTableUIColumn { DataField = "totalBoletos", DisplayName = "Valor Total", Priority = 4, Orderable = false, Searchable = false });
            config.Columns.Add(new DataTableUIColumn
            {
                DataField = "statusArquivoRemessa",
                DisplayName = "Status",
                Priority = 5,
                Options = new List<SelectOptionUI>(SystemValueHelper.GetUIElementBase(typeof(StatusArquivoRemessa)))
            });

            cfg.Content.Add(config);

            return Content(JsonConvert.SerializeObject(cfg, JsonSerializerSetting.Front), "application/json");
        }
    }
}