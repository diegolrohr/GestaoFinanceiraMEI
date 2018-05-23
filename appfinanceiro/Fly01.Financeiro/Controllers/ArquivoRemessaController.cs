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
                valorTotal = x.ValorTotal.ToString("C", AppDefaults.CultureInfoDefault),
                dataExportacao = x.DataExportacao.ToString("dd/MM/yyyy"),
                totalBoletos = x.TotalBoletos,
                status = x.StatusArquivoRemessa
            };
        }

        private List<CnabVM> GetContasReceber(Guid? idArquivo, int pageNo)
        {
            var queryString = new Dictionary<string, string>
            {
                { "arquivoRemessaId", idArquivo.ToString()},
                { "pageNo", pageNo.ToString() },
                { "pageSize", "10"}
            };

            var boletos = RestHelper.ExecuteGetRequest<PagedResult<CnabVM>>("cnab", queryString);

            return boletos.Data.Where(x => x.ArquivoRemessaId == idArquivo).ToList();
        }

        private List<CnabVM> GetCnab(Guid? idArquivo)
        {
            var queryString = AppDefaults.GetQueryStringDefault();
            queryString.AddParam("$filter", $"arquivoRemessaId eq {idArquivo}");

            var boletos = RestHelper.ExecuteGetRequest<ResultBase<CnabVM>>("cnab", queryString);

            return boletos.Data;
        }

        public JsonResult LoadGridBoletos()
        {
            var Id = Guid.Parse(Request.UrlReferrer.Segments.Last());

            var param = JQueryDataTableParams.CreateFromQueryString(Request.QueryString);
            var pageNo = param.Start > 0 ? (param.Start / 10) + 1 : 1;

            var response = GetCnab(Id);
            return Json(new
            {
                recordsTotal = response.Count,
                recordsFiltered = response.Count,
                data = response.Select(item => new
                {
                    nossoNumero = item.NossoNumero,
                    pessoa_nome = item.ContaReceber?.Pessoa?.Nome,
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
                    WithParams = Url.Action("Edit"),
                },
                Header = new HtmlUIHeader()
                {
                    Title = "Lista de boletos do arquivo",
                    Buttons = new List<HtmlUIButton>()
                    {
                        new HtmlUIButton() { Id = "cancel", Label = "Voltar", OnClickFn = "fnCancelar" }
                    }
                },
                UrlFunctions = Url.Action("Functions") + "?fns=",
                Functions = new List<string>() { "fnFormReady" }
            };

            var config = new FormUI
            {
                Action = new FormUIAction
                {
                    Edit = @Url.Action("Edit"),
                    Get = @Url.Action("Json") + "/",
                    List = @Url.Action("List")
                },
                ReadyFn = "fnFormReady"
            };

            cfg.Content.Add(config);

            var arquivo = "honatel.Rem";
            var metodoPagamento = "Boleto bancário";
            var dataImportacao = "01/02/1987";
            var status = "Aguardando aprovação";

            cfg.Content.Add(new CardUI
            {
                Class = "col s12",
                Color = "blue",
                Id = "cardObs",
                Title = "Transmissão de arquivo",
                Functions = new List<string>() { "fnFormReady" },
                Placeholder = $"Arquivo {arquivo} com o metodo de pagamento {metodoPagamento} dataImoportação {dataImportacao} e status {status}, Esta aguardando a sua aprovação manual.",
                Action = new LinkUI()
                {
                    Label = "",
                    OnClick = ""
                }
            });

            cfg.Content.Add(new DataTableUI
            {
                Id = "dtListcnab",
                Class = "col s12",
                UrlGridLoad = Url.Action("LoadGridBoletos"),
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
                Header = new HtmlUIHeader
                {
                    Title = "Arquivos remessa",
                    Buttons = new List<HtmlUIButton>
                    {
                        new HtmlUIButton { Id = "btnViewBoletos", Label = "Visualizar boletos", OnClickFn = "fnListContasArquivo" }
                    }
                },
                UrlFunctions = Url.Action("Functions") + "?fns=",
                Functions = new List<string>() { "fnEditar" }
            };

            var config = new DataTableUI
            {
                Id = "dtRemessa",
                UrlGridLoad = Url.Action("GridLoad"),
                UrlFunctions = Url.Action("Functions") + "?fns=",
                Functions = new List<string> { "fnFormReadyRemessa" },
                Options = new DataTableUIConfig()
                {
                    Select = new { style = "multi" }
                }
            };
            config.Columns.Add(new DataTableUIColumn { DataField = "descricao", DisplayName = "Arquivo", Priority = 2 });
            config.Columns.Add(new DataTableUIColumn { DataField = "dataExportacao", DisplayName = "Dt. Exportação", Priority = 3, Type = "date" });
            config.Columns.Add(new DataTableUIColumn { DataField = "totalBoletos", DisplayName = "Qtd. Boletos", Priority = 4, Orderable = false, Searchable = false });
            config.Columns.Add(new DataTableUIColumn { DataField = "valorTotal", DisplayName = "Valor Total", Priority = 4, Orderable = false, Searchable = false });
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