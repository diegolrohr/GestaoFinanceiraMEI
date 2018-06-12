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
using Fly01.Core.ViewModels.Presentation;
using Fly01.Core.Rest;
using Boleto2Net;

namespace Fly01.Financeiro.Controllers
{
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

        [HttpPost]
        public ActionResult GeraArquivoRemessa(List<Guid> ids)
        {
            try
            {
                var listaArquivosGerados = new List<string>();

                var listaBancos = GetListBancos();
                var dictBoletos = GetListaBoletos(ids);

                foreach (var item in dictBoletos.GroupBy(x => x.Key).OrderByDescending(x => x.Key).ToList())
                {
                    var lstBoletos = dictBoletos.Where(x => x.Key == item.Key).Select(x => x.Value).ToList();
                    var banco = lstBoletos.FirstOrDefault().Banco;
                    var codigoBanco = banco.Codigo.ToString("000");
                    var total = (double)lstBoletos.Sum(x => x.Boleto.ValorTitulo);
                    var boletos = new Boletos() { Banco = banco };
                    boletos.AddRange(lstBoletos.Select(x => x.Boleto));

                    var arquivoRemessa = new ArquivoRemessa(banco, ValidaDadosBancoVM.GetTipoCnab(banco.Codigo), 1); // tem que avaliar os dados passados(tipoArquivo, NumeroArquivo)
                    var nomeArquivo = $"{banco.Codigo}-{DateTime.Now.ToString("ddMMyyyyHHmmss")}";
                    Session[nomeArquivo] = arquivoRemessa.GerarArquivoRemessa(boletos);

                    if (Session[nomeArquivo] != null)
                    {
                        var dadosBanco = listaBancos.FirstOrDefault(x => x.Codigo.Contains(codigoBanco));
                        if (dadosBanco != null)
                        {
                            listaArquivosGerados.Add(nomeArquivo);
                        }
                    }
                }
                return Json(new { success = true, FileGuid = listaArquivosGerados });
            }
            catch (Exception e)
            {
                return JsonResponseStatus.GetFailure($"Ocorreu um erro: {e.Message}");
            }
        }

        [HttpPost]
        public ActionResult GetQtdArquivos(List<Guid> ids)
        {
            try
            {
                List<KeyValuePair<Guid?, Boleto2Net.Boleto>> dictContasEBoletos = MontarBoletos(ids);
                return Json(new { success = true, FileGuid = dictContasEBoletos.GroupBy(x => x.Key).OrderByDescending(x => x.Key).ToList().Count() });
            }
            catch (Exception e)
            {
                return JsonResponseStatus.GetFailure($"Ocorreu um erro: {e.Message}");
            }
        }

        [HttpGet]
        public ActionResult DownloadArquivoRemessa(List<string> ids)
        {
            var idsCnabToSave = ids[0].Split(',');
            List<string> arquivosGerados = SalvarArquivoRemessa(idsCnabToSave);

            if (arquivosGerados.Count > 1)
            {
                using (var memoryStream = new MemoryStream())
                {
                    using (var ziparchive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                    {
                        foreach (var item in arquivosGerados)
                        {
                            AddToArchive(ziparchive, item.ToString() + ".REM", (byte[])Session[item]);
                        }
                    }
                    return File(memoryStream.ToArray(), "application/zip", "LoteArquivosRemessa.zip");
                }
            }
            else
            {
                if (Session[arquivosGerados.FirstOrDefault().ToString()] != null)
                {
                    var arquivoDownload = File((byte[])Session[arquivosGerados.FirstOrDefault()], MediaTypeNames.Application.Octet, arquivosGerados.FirstOrDefault().ToString() + ".REM");
                    Session[arquivosGerados.FirstOrDefault().ToString()] = null;
                    return arquivoDownload;
                }
            }

            return null;
        }

        private List<String> SalvarArquivoRemessa(string[] idsCnabToSave)
        {
            List<string> ArquivosGeradosPorBanco = new List<string>();
            List<BancoVM> Bancos = GetBancosEmiteBoletos();
            List<KeyValuePair<Guid?, Boleto2Net.Boleto>> dictContasEBoletos = MontarBoletos(idsCnabToSave.Select(Guid.Parse).ToList());

            foreach (var item in dictContasEBoletos.GroupBy(x => x.Key).OrderByDescending(x => x.Key).ToList())
            {
                var lstBoletos = dictBoletos.Where(x => x.Key == item.Key).Select(x => x.Value).ToList();
                var banco = lstBoletos.FirstOrDefault().Banco;
                var codigoBanco = banco.Codigo.ToString("000");
                var total = (double)lstBoletos.Sum(x => x.ValorTitulo);
                var boletos = new Boleto2Net.Boletos()
                {
                    Banco = banco
                };
                boletos.AddRange(lstBoletos);

                string nomeArquivo;
                GerarArquivoPorBanco(Boletos, out nomeArquivo);

                var bancoId = Bancos.FirstOrDefault(x => x.Codigo.Contains(Boletos.FirstOrDefault().Banco.Codigo.ToString("000")));
                var Cnabs = GetCnab(idsCnabToSave.Select(Guid.Parse).ToList());
                Save(Cnabs.Where(x => x.ContaBancariaCedenteId == item.Key).Select(x => x.Id).ToList(), bancoId.Id, nomeArquivo, Boletos.Count(), total);
                ArquivosGeradosPorBanco.Add(nomeArquivo);
            }
            return ArquivosGeradosPorBanco;
        }

        private void Save(List<Guid> ids, Guid bancoId, string nomeArquivo, int qtdBoletos, double valorBoletos)
        {
            var arquivoRemessa = new ArquivoRemessaVM()
            {
                Descricao = $"{nomeArquivo}.REM",
                TotalBoletos = qtdBoletos,
                StatusArquivoRemessa = StatusArquivoRemessa.AguardandoRetorno.ToString(),
                ValorTotal = valorBoletos,
                BancoId = bancoId
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
            config.Elements.Add(new InputHiddenUI { Id = "descricao" });
            config.Elements.Add(new InputHiddenUI { Id = "dataExportacao" });
            config.Elements.Add(new InputHiddenUI { Id = "statusArquivoRemessa" });
            cfg.Content.Add(config);

            cfg.Content.Add(new CardUI
            {
                Class = "col s12",
                Color = "blue",
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
                    new DataTableUIColumn { DataField = "statusArquivoRemessa", DisplayName = "Status", Priority = 1, Options = new List<SelectOptionUI>(SystemValueHelper.GetUIElementBase(typeof(StatusCnab))), RenderFn ="function(data, type, full, meta) { return fnRenderEnum(full.statusCssClass, full.statusDescription, full.statusTooltip); }"},
                    new DataTableUIColumn { DataField = "nossoNumero", DisplayName = "Nosso numero", Priority = 6 },
                    new DataTableUIColumn { DataField = "pessoa_nome", DisplayName = "Cliente", Priority = 3, Orderable = false, Searchable = false },
                    new DataTableUIColumn { DataField = "dataVencimento", DisplayName = "Data Vencimento", Priority = 6, Orderable = false, Searchable = false, Type = "date" },
                    new DataTableUIColumn { DataField = "valorBoleto", DisplayName = "Valor", Priority = 4, Orderable = false, Searchable = false, Type = "currency" },
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
                RenderFn = "function(data, type, full, meta) { return fnRenderEnum(full.statusCssClass, full.statusDescription, full.statusTooltip); }"
            });
            config.Columns.Add(new DataTableUIColumn { DataField = "descricao", DisplayName = "Arquivo", Priority = 2 });
            config.Columns.Add(new DataTableUIColumn { DataField = "banco_nome", DisplayName = "Banco", Priority = 2 });
            config.Columns.Add(new DataTableUIColumn { DataField = "dataExportacao", DisplayName = "Dt. Exportação", Priority = 3, Type = "date" });
            config.Columns.Add(new DataTableUIColumn { DataField = "totalBoletos", DisplayName = "Qtd. Boletos", Priority = 4, Orderable = false, Searchable = false });
            config.Columns.Add(new DataTableUIColumn { DataField = "valorTotal", DisplayName = "Valor Total", Priority = 5, Orderable = false, Searchable = false });

            cfg.Content.Add(config);

            return Content(JsonConvert.SerializeObject(cfg, JsonSerializerSetting.Front), "application/json");
        }
    }
}