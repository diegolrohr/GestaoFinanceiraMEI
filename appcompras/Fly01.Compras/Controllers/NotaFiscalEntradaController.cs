using Fly01.Compras.ViewModel;
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
using Fly01.Core.ViewModels;
using Fly01.Core.ViewModels.Presentation.Commons;
using Fly01.uiJS.Enums;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using Fly01.Core.Mensageria;
using Fly01.Compras.Helpers;

namespace Fly01.Compras.Controllers
{
    [OperationRole(ResourceKey = ResourceHashConst.ComprasComprasNotasFiscais)]
    public class NotaFiscalEntradaController : BaseController<NotaFiscalEntradaVM>
    {
        public bool HasErrorDownload { get; set; }
        public NotaFiscalEntradaController()
        {
            ExpandProperties = "fornecedor($select=id,nome,email),ordemCompraOrigem($select=id,numero),categoria($select=id,descricao),serieNotaFiscal($select=id,serie),centroCusto";
        }

        public override Dictionary<string, string> GetQueryStringDefaultGridLoad()
        {
            var queryString = base.GetQueryStringDefaultGridLoad();
            queryString.Add("$select", "id,tipoNotaFiscal,status,data,tipoCompra,numNotaFiscal");
            return queryString;
        }

        public override Func<NotaFiscalEntradaVM, object> GetDisplayData()
        {
            return x => new
            {
                id = x.Id.ToString(),
                tipoNotaFiscal = x.TipoNotaFiscal,
                tipoNotaFiscalDescription = EnumHelper.GetDescription(typeof(TipoNotaFiscal), x.TipoNotaFiscal),
                tipoNotaFiscalCssClass = EnumHelper.GetCSS(typeof(TipoNotaFiscal), x.TipoNotaFiscal),
                tipoNotaFiscalValue = EnumHelper.GetValue(typeof(TipoNotaFiscal), x.TipoNotaFiscal),
                status = x.Status,
                statusDescription = EnumHelper.GetDescription(typeof(StatusNotaFiscal), x.Status),
                statusCssClass = EnumHelper.GetCSS(typeof(StatusNotaFiscal), x.Status),
                statusValue = EnumHelper.GetValue(typeof(StatusNotaFiscal), x.Status),
                data = x.Data.ToString("dd/MM/yyyy"),
                fornecedor_nome = x.Fornecedor.Nome,
                fornecedor_email = x.Fornecedor.Email,
                ordemCompraOrigem_numero = x.OrdemCompraOrigem?.Numero.ToString(),
                tipoCompra = x.TipoCompra,
                tipoCompraDescription = EnumHelper.GetDescription(typeof(TipoCompraVenda), x.TipoCompra),
                tipoCompraCssClass = EnumHelper.GetCSS(typeof(TipoCompraVenda), x.TipoCompra),
                tipoCompraValue = EnumHelper.GetValue(typeof(TipoCompraVenda), x.TipoCompra),
                categoria_descricao = x.Categoria != null ? x.Categoria.Descricao : "",
                numNotaFiscal = x.NumNotaFiscal,
                serieNotaFiscal_serie = x.SerieNotaFiscal != null ? x.SerieNotaFiscal.Serie : ""
            };
        }

        protected override ContentUI FormJson() { throw new NotImplementedException(); }

        public override ContentResult List()
            => ListNotaFiscal();

        public List<HtmlUIButton> GetListButtonsOnHeaderCustom(string buttonLabel, string buttonOnClick)
        {
            var target = new List<HtmlUIButton>();

            if (UserCanWrite)
            {
                target.Add(new HtmlUIButton { Id = "baixarXmls", Label = "Baixar Todos Xmls", OnClickFn = "fnBaixarTodosXMLNFeZip", Position = HtmlUIButtonPosition.Out });
                target.Add(new HtmlUIButton { Id = "baixarTodosXmls", Label = "Baixar Xmls", OnClickFn = "fnBaixarXMLNFeZip", Position = HtmlUIButtonPosition.Out });
                target.Add(new HtmlUIButton { Id = "atualizarStatus", Label = "Atualizar Status", OnClickFn = "fnAtualizarStatus",Position = HtmlUIButtonPosition.Main });
                target.Add(new HtmlUIButton { Id = "new", Label = "Novo Pedido", OnClickFn = "fnNovoPedido" });
                target.Add(new HtmlUIButton { Id = "filterGrid", Label = buttonLabel, OnClickFn = buttonOnClick });
                target.Add(new HtmlUIButton { Id = "newNFInutilizada", Label = "Inutilizar Nota Fiscal", OnClickFn = "fnNotaFiscalInutilizadaList" });
            }

            return target;
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

            var cfg = new ContentUIBase(Url.Action("Sidebar", "Home"))
            {
                History = new ContentUIHistory { Default = Url.Action("Index") },
                Header = new HtmlUIHeader
                {
                    Title = "Notas Fiscais",
                    Buttons = new List<HtmlUIButton>(GetListButtonsOnHeaderCustom(buttonLabel, buttonOnClick))
                },
                UrlFunctions = Url.Action("Functions") + "?fns=",
                Functions = new List<string>() { "fnFormReadyNotasFiscais" }
            };

            var cfgForm = new FormUI
            {
                Id = "fly01frm",
                UrlFunctions = Url.Action("Functions") + "?fns=",
                ReadyFn = gridLoad == "GridLoad" ? "" : "fnChangeInput",
                Elements = new List<BaseUI>()
                {
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

            if (gridLoad == "GridLoad")
            {
                cfgForm.Elements.Add(new PeriodPickerUI()
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
                });
                cfgForm.ReadyFn = "fnUpdateDataFinal";
            }

            cfg.Content.Add(cfgForm);
            var config = new DataTableUI
            {
                Id = "fly01dt",
                UrlGridLoad = Url.Action(gridLoad),
                Parameters = new List<DataTableUIParameter>
                {
                    new DataTableUIParameter() {Id = "dataInicial", Required = (gridLoad == "GridLoad") },
                    new DataTableUIParameter() {Id = "dataFinal", Required = (gridLoad == "GridLoad") }
                },
                UrlFunctions = Url.Action("Functions") + "?fns=",
                Functions = new List<string>() { "fnRenderEnum" },
                Options = new DataTableUIConfig
                {
                    Select = new { style = "multi" },
                    OrderColumn = 6,
                    OrderDir = "desc"
                }
            };

            config.Actions.AddRange(GetActionsInGrid(new List<DataTableUIAction>()
            {
                new DataTableUIAction { OnClickFn = "fnVisualizarNFe", Label = "Visualizar", ShowIf = "(row.tipoNotaFiscal == 'NFe')" },
                new DataTableUIAction { OnClickFn = "fnVisualizarRetornoSefaz", Label = "Mensagem SEFAZ", ShowIf = "(row.status != 'NaoTransmitida' && row.tipoNotaFiscal == 'NFe')" },
                new DataTableUIAction { OnClickFn = "fnTransmitirNFe", Label = "Transmitir", ShowIf = "((row.status == 'NaoAutorizada' || row.status == 'NaoTransmitida' || row.status == 'FalhaTransmissao') && row.tipoNotaFiscal == 'NFe')" },
                new DataTableUIAction { OnClickFn = "fnExcluirNFe", Label = "Excluir", ShowIf = "((row.status == 'NaoAutorizada' || row.status == 'NaoTransmitida' || row.status == 'FalhaTransmissao') && row.tipoNotaFiscal == 'NFe')" },
                new DataTableUIAction { OnClickFn = "fnBaixarXMLNFe", Label = "Baixar XML", ShowIf = "((row.status == 'NaoAutorizada' || row.status == 'Autorizada'|| row.status == 'Transmitida') && row.tipoNotaFiscal == 'NFe')" },
                new DataTableUIAction { OnClickFn = "fnBaixarPDFNFe", Label = "Baixar PDF", ShowIf = "(row.status == 'Autorizada' && row.tipoNotaFiscal == 'NFe')" },
                new DataTableUIAction { OnClickFn = "fnCancelarNFe", Label = "Cancelar", ShowIf = "((row.status == 'Autorizada' || row.status == 'FalhaNoCancelamento') && row.tipoNotaFiscal == 'NFe')" },
                new DataTableUIAction { OnClickFn = "fnFormCartaCorrecao", Label = "Carta de Correção", ShowIf = "(row.status == 'Autorizada')" },
                new DataTableUIAction { OnClickFn = "fnEnviarEmailNFe", Label = "Enviar por e-mail", ShowIf = "(row.status == 'Autorizada') && (row.tipoNotaFiscal == 'NFe')" }
            }));

            config.Columns.Add(new DataTableUIColumn { DataField = "serieNotaFiscal_serie", DisplayName = "Série", Priority = 1 });
            config.Columns.Add(new DataTableUIColumn { DataField = "numNotaFiscal", DisplayName = "Número NF", Priority = 2, Type = "numbers" });
            config.Columns.Add(new DataTableUIColumn
            {
                DataField = "status",
                DisplayName = "Status",
                Priority = 3,
                Options = new List<SelectOptionUI>(SystemValueHelper.GetUIElementBase(typeof(StatusNotaFiscal))),
                RenderFn = "fnRenderEnum(full.statusCssClass, full.statusDescription)"
            });
            config.Columns.Add(new DataTableUIColumn
            {
                DataField = "tipoNotaFiscal",
                DisplayName = "Tipo",
                Priority = 4,
                Options = new List<SelectOptionUI>(SystemValueHelper.GetUIElementBase(typeof(TipoNotaFiscal))),
                RenderFn = "fnRenderEnum(full.tipoNotaFiscalCssClass, full.tipoNotaFiscalDescription)"
            });
            config.Columns.Add(new DataTableUIColumn
            {
                DataField = "tipoCompra",
                DisplayName = "Finalidade",
                Priority = 5,
                Options = new List<SelectOptionUI>(SystemValueHelper.GetUIElementBase(typeof(TipoCompraVenda))),
                RenderFn = "fnRenderEnum(full.tipoCompraCssClass, full.tipoCompraDescription)"
            });
            config.Columns.Add(new DataTableUIColumn { DataField = "fornecedor_nome", DisplayName = "Fornecedor", Priority = 6 });
            config.Columns.Add(new DataTableUIColumn { DataField = "data", DisplayName = "Data", Priority = 7, Type = "date" });
            config.Columns.Add(new DataTableUIColumn { DataField = "ordemCompraOrigem_numero", DisplayName = "Pedido Origem", Searchable = false, Priority = 8 });

            cfg.Content.Add(config);

            return Content(JsonConvert.SerializeObject(cfg, JsonSerializerSetting.Front), "application/json");
        }

        public override JsonResult GridLoad(Dictionary<string, string> filters = null)
        {
            if (filters == null)
                filters = new Dictionary<string, string>();

            if (Request.QueryString["dataFinal"] != "")
                filters.Add("data le ", Request.QueryString["dataFinal"]);
            if (Request.QueryString["dataInicial"] != "")
                filters.Add(" and data ge ", Request.QueryString["dataInicial"]);

            return base.GridLoad(filters);
        }

        public JsonResult GridLoadNoFilter()
        {
            return GridLoad();
        }

        [OperationRole(PermissionValue = EPermissionValue.Read)]
        [HttpGet]
        public JsonResult TotalNotaFiscal(string id)
        {
            try
            {
                var resource = string.Format("CalculaTotalNotaFiscal?&notaFiscalId={0}", id);
                var response = RestHelper.ExecuteGetRequest<TotalPedidoNotaFiscalVM>(resource, queryString: null);

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

        [OperationRole(PermissionValue = EPermissionValue.Write)]
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

        private byte[] GetXMLFile(NotaFiscalEntradaVM notafiscal)
        {

            try
            {
                var resourceById = string.Format("NotaFiscalXML?&id={0}", notafiscal.Id);
                var response = RestHelper.ExecuteGetRequest<JObject>(resourceById);

                string fileName = notafiscal.TipoNotaFiscal + response.Value<string>("numNotaFiscal") + ".xml";
                string xml = response.Value<string>("xml");
                xml = xml.Replace("\\", "");

                byte[] bytes = Encoding.ASCII.GetBytes(xml);

                return bytes;
            }
            catch (Exception ex)
            {
                ErrorInfo error = JsonConvert.DeserializeObject<ErrorInfo>(ex.Message);
                throw new Exception(error.Message);
            }
        }

        [OperationRole(PermissionValue = EPermissionValue.Read)]
        private byte[] GetPDFFile(NotaFiscalEntradaVM notafiscal)
        {
            try
            {
                var resourceById = string.Format("NotaFiscalPDF?&id={0}", notafiscal.Id);
                var response = RestHelper.ExecuteGetRequest<JObject>(resourceById);

                string fileName = "Danfe - NFe" + response.Value<string>("numNotaFiscal") + ".pdf";
                string fileBase64 = response.Value<string>("pdf");

                byte[] bytes = Convert.FromBase64String(fileBase64);

                return bytes;
            }
            catch (Exception ex)
            {
                ErrorInfo error = JsonConvert.DeserializeObject<ErrorInfo>(ex.Message);
                throw new Exception(error.Message);
            }
        }

        [OperationRole(PermissionValue = EPermissionValue.Read)]
        public JsonResult EnviarEmailNFe(string id)
        {
            try
            {
                var empresa = GetDadosEmpresa();
                var notaFiscal = Get(Guid.Parse(id));

                if (notaFiscal.Fornecedor == null)
                {
                    return JsonResponseStatus.GetFailure("Nenhum fornecedor foi encontrado.");
                }

                if (string.IsNullOrEmpty(notaFiscal.Fornecedor.Email))
                {
                    return JsonResponseStatus.GetFailure("Não foi encontrado um email válido para este fornecedor.");
                }

                if (string.IsNullOrEmpty(empresa.Email))
                {
                    return JsonResponseStatus.GetFailure("Você ainda não configurou um email válido para sua empresa.");
                }

                var pdf = File(GetPDFFile(notaFiscal), "application/pdf");
                var xml = File(GetXMLFile(notaFiscal), ".xml");
                var tituloEmail = $"{empresa.NomeFantasia} {notaFiscal.TipoNotaFiscal} - Nº {notaFiscal.NumNotaFiscal}".ToUpper();
                var mensagemPrincipal = $"Você está recebendo uma cópia do XML e Danfe da sua {notaFiscal.TipoNotaFiscal}.".ToUpper();
                var conteudoEmail = Mail.FormataMensagem(EmailFilesHelper.GetTemplate("Templates.OrdemCompra.html").Value, tituloEmail, mensagemPrincipal, empresa.Email);
                var arquivoPdf = new FileStreamResult(new MemoryStream(pdf.FileContents), pdf.ContentType);
                var arquivoXml = new FileStreamResult(new MemoryStream(xml.FileContents), xml.ContentType);

                Stream[] anexos = new[] { arquivoPdf.FileStream, arquivoXml.FileStream };
                string[] tiposAnexos = new[] { arquivoPdf.ContentType, arquivoXml.ContentType };

                Mail.SendMultipleAttach(notaFiscal.Fornecedor.Email, empresa.NomeFantasia, tituloEmail, conteudoEmail, anexos, tiposAnexos);

                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var error = JsonConvert.DeserializeObject<ErrorInfo>(ex.Message);
                return JsonResponseStatus.GetFailure(error.Message);
            }
        }

        [OperationRole(PermissionValue = EPermissionValue.Read)]
        [HttpGet]
        public ActionResult BaixarXMLs(string idsXML)
        {
            try
            {
                var ids = idsXML.Split(',');
                var response = new List<JObject>();

                foreach (var item in ids)
                {
                    try
                    {
                        var resourceById = string.Format("NotaFiscalXML?&id={0}", item);
                        var res = RestHelper.ExecuteGetRequest<JObject>(resourceById);
                        if (res != null)
                            response.Add(res);
                    }
                    catch (Exception)
                    {
                        HasErrorDownload = true;
                        continue;
                    }
                }

                if (response.Count == 0)
                    return JsonResponseStatus.GetFailure("Os XMLs solicitados não estão disponíveis para download");

                Session["responseValue"] = JsonConvert.SerializeObject(response);

                return JsonResponseStatus.GetJson(new { downloadAddress = Url.Action("DownloadXMLs", new { idsXML = idsXML }), hasError = HasErrorDownload });
            }
            catch (Exception ex)
            {
                ErrorInfo error = JsonConvert.DeserializeObject<ErrorInfo>(ex.Message);
                return JsonResponseStatus.GetFailure(error.Message);
            }
        }

        [OperationRole(PermissionValue = EPermissionValue.Read)]
        [HttpGet]
        public ActionResult BaixarTodosXMLs(DateTime dataInicial, DateTime dataFinal)
        {
            try
            {
                string idsXML = "";
                var response = new List<NotaFiscalEntradaVM>();
                try
                {
                    var resourceById = string.Format("NotaFiscalXML?&dataInicial={0}&dataFinal={1}", dataInicial.ToString("yyyy-MM-dd"), dataFinal.ToString("yyyy-MM-dd"));
                    var res = RestHelper.ExecuteGetRequest<List<NotaFiscalEntradaVM>>(resourceById);
                    if (res != null)
                    {
                        response = res;
                    }
                }
                catch (Exception)
                {
                    HasErrorDownload = true;
                }

                foreach (NotaFiscalEntradaVM item in response)
                {
                    if (item.Id != null)
                    {
                        idsXML += item.Id + ",";
                    }
                }

                if (response.Count == 0)
                    return JsonResponseStatus.GetFailure("Os XMLs solicitados não estão disponíveis para download");

                Session["responseValue"] = JsonConvert.SerializeObject(response);

                return JsonResponseStatus.GetJson(new { downloadAddress = Url.Action("DownloadXMLs", new { idsXML = idsXML }), hasError = HasErrorDownload });
            }
            catch (Exception ex)
            {
                ErrorInfo error = JsonConvert.DeserializeObject<ErrorInfo>(ex.Message);
                return JsonResponseStatus.GetFailure(error.Message);
            }
        }

        [OperationRole(NotApply = true)]
        [HttpGet]
        public ActionResult DownloadXMLs(string idsXML)
        {
            var sessionValue = Session["responseValue"];
            var response = JsonConvert.DeserializeObject<List<JObject>>(sessionValue.ToString());

            var fileName = "";
            using (var memoryStream = new MemoryStream())
            {
                using (var ziparchive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                {
                    response.ToList().ForEach(item =>
                    {
                        fileName = item.Value<string>("tipoNotaFiscal") + item.Value<string>("numNotaFiscal");

                        string xml = item.Value<string>("xml");
                        xml = xml.Replace("\\", "");
                        Session.Add(fileName, xml);
                        byte[] data = Convert.FromBase64String(Base64Helper.CodificaBase64(Session[fileName].ToString()));

                        AddToArchive(ziparchive, fileName + ".xml", data);
                    });
                }
                return File(memoryStream.ToArray(), "application/zip", "arquivosXML.zip");
            }
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

    }
}