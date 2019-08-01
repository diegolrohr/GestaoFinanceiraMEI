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
using Fly01.Core;
using Fly01.Core.Helpers;
using System.IO;
using System.IO.Compression;
using System.Net.Mime;
using System.Linq;
using Fly01.Core.Rest;
using Fly01.Core.Presentation.JQueryDataTable;
using Fly01.Core.Presentation;
using Fly01.uiJS.Enums;

namespace Fly01.Financeiro.Controllers
{
    [OperationRole(ResourceKey = ResourceHashConst.FinanceiroCobrancaArquivosRemessa)]
    public class ArquivoRemessaController : BoletoController<ArquivoRemessaVM>
    {
        public ArquivoRemessaController()
        {
            ExpandProperties = "banco($select=nome)";
        }

        public override Func<ArquivoRemessaVM, object> GetDisplayData()
        {
            return x => new
            {
                id = x.Id,
                statusArquivoRemessa = x.StatusArquivoRemessa,
                bancoId = x.BancoId,
                banco_nome = x.Banco?.Nome,
                statusCssClass = EnumHelper.GetCSS(typeof(StatusArquivoRemessa), x.StatusArquivoRemessa),
                statusDescription = EnumHelper.GetDescription(typeof(StatusArquivoRemessa), x.StatusArquivoRemessa),
                statusTooltip = EnumHelper.GetTooltipHint(typeof(StatusArquivoRemessa), x.StatusArquivoRemessa),
                descricao = x.Descricao,
                dataExportacao = x.DataExportacao.ToString("dd/MM/yyyy"),
                totalBoletos = x.TotalBoletos,
                valorTotal = x.ValorTotal.ToString("C", AppDefaults.CultureInfoDefault),
            };
        }

        [OperationRole(NotApply = true)]
        [HttpPost]
        public ActionResult GetQtdArquivos(List<Guid> ids)
        {
            try
            {
                var dictContasEBoletos = GetListaBoletos(ids);

                return Json(new
                {
                    success = true,
                    FileGuid = dictContasEBoletos
                    .GroupBy(x => x.ContaBancariaCedenteId)
                    .OrderByDescending(x => x.Key)
                    .ToList()
                    .Count()
                });
            }
            catch (Exception e)
            {
                return JsonResponseStatus.GetFailure($"Ocorreu um erro: {e.Message}");
            }
        }

        [OperationRole(NotApply = true)]
        [HttpGet]
        public ActionResult DownloadArquivoRemessa(List<string> ids, string idArquivo = "")
        {
            var idsCnabToSave = ids[0].Split(',');
            var arquivosGerados = SalvarArquivoRemessa(idsCnabToSave, idArquivo);

            if (arquivosGerados.Count > 1)
            {
                using (var memoryStream = new MemoryStream())
                {
                    using (var ziparchive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                    {
                        arquivosGerados.ToList().ForEach(item =>
                        {
                            var nomeCorreto = item;
                            var indexString = item.IndexOf("-");
                            item = item.Substring(indexString + 1);

                            AddToArchive(ziparchive, item.Remove(8, 6) + ".REM", (byte[])Session[nomeCorreto]);
                        });
                    }

                    return File(memoryStream.ToArray(), "application/zip", "LoteArquivosRemessa.zip");
                }
            }
            else
            {
                var nomeArquivo = arquivosGerados.FirstOrDefault();
                var nomeCorreto = nomeArquivo;

                if (Session[nomeArquivo] != null)
                {
                    var indexString = nomeArquivo.IndexOf("-");
                    nomeArquivo = nomeArquivo.Substring(indexString + 1);
                    nomeArquivo = nomeArquivo.Remove(8, 6);

                    var arquivoDownload = File((byte[])Session[nomeCorreto], MediaTypeNames.Application.Octet, nomeArquivo + ".REM");
                    Session[nomeArquivo] = null;

                    return arquivoDownload;
                }
            }

            return null;
        }

        [OperationRole(NotApply = true)]
        [HttpGet]
        public ActionResult DownloadArquivoRemessabyId(Guid idArquivo)
        {
            var cnabs = GetArquivo(idArquivo);
            var value = "";

            List<string> ids = new List<string>();
            foreach (var item in cnabs)
                value = value + item.Id.ToString() + ",";

            value = value.Substring(0, value.Length - 1);
            ids.Add(value);

            return DownloadArquivoRemessa(ids, idArquivo.ToString());   
        }

        protected List<CnabVM> GetArquivo(Guid id)
        {
            var queryString = new Dictionary<string, string>();
            queryString.AddParam("$filter", $"arquivoRemessaId eq {id}");
            queryString.AddParam("$select", "id");

            var cnabs = RestHelper.ExecuteGetRequest<ResultBase<CnabVM>>("cnab", queryString);

            return cnabs.Data.ToList();
        }

        private List<String> SalvarArquivoRemessa(string[] idsCnabToSave, string idArquivo = "")
        {
            var arquivosGeradosPorBanco = new List<string>();
            var bancos = GetBancosEmiteBoletos();
            var dictContasEBoletos = GetListaBoletos(idsCnabToSave.Select(Guid.Parse).ToList(), true);

            dictContasEBoletos.GroupBy(x => x.CodigoBanco).OrderByDescending(x => x.Key).ToList().ForEach(item =>
            {
                var dadosArquivoRemessa = item.FirstOrDefault();
                var codigoBanco = dadosArquivoRemessa.CodigoBanco.ToString("000");
                var nomeArquivo = $"{codigoBanco}-{DateTime.Now.ToString("ddMMyyyyHHmmss")}";
                Session[nomeArquivo] = dadosArquivoRemessa.ConteudoArquivoRemessa;

                var banco = bancos.FirstOrDefault(x => x.Codigo.Contains(codigoBanco));
                var cnabs = GetCnab(idsCnabToSave.Select(Guid.Parse).ToList())
                    .Where(x => x.ContaBancariaCedenteId == dadosArquivoRemessa.ContaBancariaCedenteId)
                    .Select(x => x.Id).ToList();

                arquivosGeradosPorBanco.Add(nomeArquivo);

                if (idArquivo == "")                
                    Save(cnabs, banco.Id, nomeArquivo, dadosArquivoRemessa.TotalBoletosGerados, dadosArquivoRemessa.ValorTotalArquivoRemessa, dadosArquivoRemessa.NumeroArquivoRemessa);
                
            });

            return arquivosGeradosPorBanco;
        }

        private void Save(List<Guid> ids, Guid bancoId, string nomeArquivo, int qtdBoletos, double valorBoletos, int NumeroArquivoRemessa)
        {
            var arquivoRemessa = new ArquivoRemessaVM()
            {
                Descricao = $"{nomeArquivo}.REM",
                TotalBoletos = qtdBoletos,
                StatusArquivoRemessa = StatusArquivoRemessa.AguardandoRetorno.ToString(),
                ValorTotal = valorBoletos,
                BancoId = bancoId, 
                NumeroArquivoRemessa = NumeroArquivoRemessa
            };

            var result = RestHelper.ExecutePostRequest<ArquivoRemessaVM>("arquivoremessa", JsonConvert.SerializeObject(arquivoRemessa, JsonSerializerSetting.Default));
            UpdateCnab(ids, result);
        }

        private void AddToArchive(ZipArchive ziparchive, string fileName, byte[] attach)
        {
            var zipEntry = ziparchive.CreateEntry(fileName, CompressionLevel.Optimal);
            using (var zipStream = zipEntry.Open())
            using (var streamIn = new MemoryStream(attach))
            {
                streamIn.CopyTo(zipStream);
            }
        }

        private void UpdateCnab(List<Guid> ids, ArquivoRemessaVM result)
        {
            var status = ((int)StatusCnab.AguardandoRetorno).ToString();

            ids.ForEach(x =>
            {
                var resource = $"cnab/{x}";
                RestHelper.ExecutePutRequest(resource, JsonConvert.SerializeObject(new
                {
                    arquivoRemessaId = result.Id,
                    status = status
                }));
            });
        }

        public override List<HtmlUIButton> GetFormButtonsOnHeader()
        {
            var target = new List<HtmlUIButton>();

            if (UserCanWrite)
            {
                target.Add(new HtmlUIButton { Id = "save", Label = "Salvar", OnClickFn = "fnSalvar", Type = "submit", Position = HtmlUIButtonPosition.Main });
                target.Add(new HtmlUIButton() { Id = "cancel", Label = "Voltar", OnClickFn = "fnCancelar" });
            }

            return target;
        }

        protected override ContentUI FormJson()
        {
            var cfg = new ContentUIBase(Url.Action("Sidebar", "Home"))
            {
                History = new ContentUIHistory()
                {
                    WithParams = Url.Action("Edit"),
                },
                Header = new HtmlUIHeader()
                {
                    Title = "Lista de boletos do arquivo",
                    Buttons = new List<HtmlUIButton>(GetFormButtonsOnHeader())
                    {
                        
                    }
                },
                UrlFunctions = Url.Action("Functions") + "?fns=",
                Functions = new List<string>() { "fnFormReady" }
            };

            var config = new FormUI
            {
                Id = "fly01frm",
                Action = new FormUIAction
                {
                    Edit = @Url.Action("Edit"),
                    Get = @Url.Action("Json") + "/",
                    List = @Url.Action("List")
                },
                ReadyFn = "fnFormReady"
            };
            config.Elements.Add(new InputHiddenUI { Id = "descricao" });
            config.Elements.Add(new InputHiddenUI { Id = "dataExportacao" });
            config.Elements.Add(new InputHiddenUI { Id = "statusArquivoRemessa" });
            cfg.Content.Add(config);

            cfg.Content.Add(new CardUI
            {
                Class = "col s12",
                Color = "green",
                Id = "cardObs",
                Title = "Transmissão de arquivo",
                Functions = new List<string>() { "fnFormReady" },
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
                UrlFunctions = Url.Action("Functions") + "?fns=",
                Functions = new List<string> { "fnFormReadyRemessa", "fnRenderEnum" },
                Options = new DataTableUIConfig()
                {
                    PageLength = 10
                },
                Columns = new List<DataTableUIColumn>
                {
                    new DataTableUIColumn { DataField = "statusArquivoRemessa", DisplayName = "Status", Priority = 1, Options = new List<SelectOptionUI>(SystemValueHelper.GetUIElementBase(typeof(StatusCnab))), RenderFn ="fnRenderEnum(full.statusCssClass, full.statusDescription, full.statusTooltip)"},
                    new DataTableUIColumn { DataField = "nossoNumeroFormatado", DisplayName = "Nosso numero", Priority = 6 },
                    new DataTableUIColumn { DataField = "pessoa_nome", DisplayName = "Cliente", Priority = 3, Orderable = false, Searchable = false },
                    new DataTableUIColumn { DataField = "dataVencimento", DisplayName = "Data Vencimento", Priority = 6, Orderable = false, Searchable = false, Type = "date" },
                    new DataTableUIColumn { DataField = "valorBoleto", DisplayName = "Valor", Priority = 4, Orderable = false, Searchable = false, Type = "currency" },
                }
            });

            return cfg;
        }

        public override List<HtmlUIButton> GetListButtonsOnHeader()
        {
            var target = new List<HtmlUIButton>()
            {
                new HtmlUIButton { Id = "btnViewBoletos", Label = "Visualizar boletos", OnClickFn = "fnListContasArquivo", Position = HtmlUIButtonPosition.Out  },
                new HtmlUIButton { Id = "btnDownload", Label = "Download Arquivo", OnClickFn = "fnDownloadArquivo", Position = HtmlUIButtonPosition.Out }
            };

            if (UserCanWrite)
            {
                target.Add(new HtmlUIButton { Id = "btnGerarArqRemessa", Label = "GERAR ARQ. REMESSA", OnClickFn = "fnGerarArquivo", Position = HtmlUIButtonPosition.Main });
            }

            return target;                    
        }

        public override ContentResult List()
        {
            var cfg = new ContentUIBase(Url.Action("Sidebar", "Home"))
            {
                History = new ContentUIHistory { Default = Url.Action("Index") },
                Header = new HtmlUIHeader
                {
                    Title = "Arquivos remessa",
                    Buttons = new List<HtmlUIButton>(GetListButtonsOnHeader())
                },
                UrlFunctions = Url.Action("Functions") + "?fns=",
                Functions = new List<string>() { "fnEditar" }
            };

            var config = new DataTableUI
            {
                Id = "dtRemessa",
                UrlGridLoad = Url.Action("GridLoad"),
                UrlFunctions = Url.Action("Functions") + "?fns=",
                Functions = new List<string> { "fnFormReadyRemessa", "fnRenderEnum" },
                Options = new DataTableUIConfig()
                {
                    Select = new { style = "single" }
                }
            };

            config.Columns.Add(new DataTableUIColumn
            {
                DataField = "statusArquivoRemessa",
                DisplayName = "Status",
                Options = new List<SelectOptionUI>(SystemValueHelper.GetUIElementBase(typeof(StatusArquivoRemessa))),
                Priority = 1,
                Width = "12%",
                RenderFn = "fnRenderEnum(full.statusCssClass, full.statusDescription, full.statusTooltip)"
            });
            config.Columns.Add(new DataTableUIColumn { DataField = "descricao", DisplayName = "Arquivo", Priority = 2 });
            config.Columns.Add(new DataTableUIColumn { DataField = "banco_nome", DisplayName = "Banco", Priority = 2 });
            config.Columns.Add(new DataTableUIColumn { DataField = "dataExportacao", DisplayName = "Dt. Exportação", Priority = 3, Type = "date" });
            config.Columns.Add(new DataTableUIColumn { DataField = "totalBoletos", DisplayName = "Qtd. Boletos", Priority = 4, Orderable = false, Searchable = false });
            config.Columns.Add(new DataTableUIColumn { DataField = "valorTotal", DisplayName = "Valor Total", Priority = 5, Orderable = false, Searchable = false });

            cfg.Content.Add(config);

            return Content(JsonConvert.SerializeObject(cfg, JsonSerializerSetting.Front), "application/json");
        }

        [OperationRole(NotApply = true)]
        public JsonResult LoadGridBoletos()
        {
            var Id = Guid.Parse(Request.UrlReferrer.Segments.Last());

            var param = JQueryDataTableParams.CreateFromQueryString(Request.QueryString);
            var pageNo = param.Start > 0 ? (param.Start / 10) + 1 : 1;

            var response = GetCnab($"arquivoRemessaId eq {Id}");
            return Json(new
            {
                recordsTotal = response.Count,
                recordsFiltered = response.Count,
                data = response.Select(item => new
                {
                    nossoNumero = item.NossoNumero,
                    nossoNumeroFormatado = item.NossoNumeroFormatado,
                    pessoa_nome = item.ContaReceber?.Pessoa?.Nome,
                    valorBoleto = item.ValorBoleto.ToString("C", AppDefaults.CultureInfoDefault),
                    dataEmissao = item.DataEmissao.ToString("dd/MM/yyyy"),
                    dataVencimento = item.DataVencimento.ToString("dd/MM/yyyy"),
                    statusArquivoRemessa = item.Status,
                    statusCssClass = EnumHelper.GetCSS(typeof(StatusCnab), item.Status),
                    statusDescription = EnumHelper.GetDescription(typeof(StatusCnab), item.Status),
                    statusTooltip = EnumHelper.GetTooltipHint(typeof(StatusCnab), item.Status),
                })

            }, JsonRequestBehavior.AllowGet);
        }

        protected override List<JQueryDataTableParamsColumn> GetParamsColumns()
        {
            throw new NotImplementedException();
        }
    }
}