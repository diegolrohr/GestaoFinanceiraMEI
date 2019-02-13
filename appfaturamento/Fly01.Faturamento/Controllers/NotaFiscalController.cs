﻿using Fly01.Faturamento.ViewModel;
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
using Fly01.Faturamento.Helpers;
using Fly01.Core.Mensageria;
using System.IO;
using System.Text;

namespace Fly01.Faturamento.Controllers
{
    [OperationRole(ResourceKey = ResourceHashConst.FaturamentoFaturamentoNotasFiscais)]
    public class NotaFiscalController : BaseController<NotaFiscalVM>
    {
        public NotaFiscalController()
        {
            ExpandProperties = "cliente($select=nome, email),ordemVendaOrigem($select=id,numero),categoria,serieNotaFiscal";
        }

        public override Dictionary<string, string> GetQueryStringDefaultGridLoad()
        {
            var queryString = base.GetQueryStringDefaultGridLoad();
            queryString.Add("$select", "id,tipoNotaFiscal,status,data,tipoVenda,numNotaFiscal");
            return queryString;
        }

        //NFeVM e NFSeVM na mesma controller notaFiscal, direcionado as controller via javaScript
        public override Func<NotaFiscalVM, object> GetDisplayData()
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
                cliente_nome = x.Cliente.Nome,
                cliente_email = x.Cliente?.Email,
                ordemVendaOrigem_numero = x.OrdemVendaOrigem?.Numero.ToString(),
                tipoVenda = x.TipoVenda,
                tipoVendaDescription = EnumHelper.GetDescription(typeof(TipoCompraVenda), x.TipoVenda),
                tipoVendaCssClass = EnumHelper.GetCSS(typeof(TipoCompraVenda), x.TipoVenda),
                tipoVendaValue = EnumHelper.GetValue(typeof(TipoCompraVenda), x.TipoVenda),
                categoria_descrica = x.Categoria != null ? x.Categoria.Descricao : "",
                numNotaFiscal = x.NumNotaFiscal,
                serieNotaFiscal_serie = x.SerieNotaFiscal != null ? x.SerieNotaFiscal.Serie : "",
                selected = false
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
                target.Add(new HtmlUIButton { Id = "baixarTodosXmls", Label = "Baixar Xmls", OnClickFn = "fnBaixarXMLNFeZip", Position = HtmlUIButtonPosition.Out });
                target.Add(new HtmlUIButton { Id = "atualizarStatus", Label = "Atualizar Status", OnClickFn = "fnAtualizarStatus", Position = HtmlUIButtonPosition.Main });
                target.Add(new HtmlUIButton { Id = "new", Label = "Novo Pedido", OnClickFn = "fnNovoPedido" });
                target.Add(new HtmlUIButton { Id = "filterGrid", Label = buttonLabel, OnClickFn = buttonOnClick });
                target.Add(new HtmlUIButton { Id = "newNFInutilizada", Label = "Inutilizar Nota Fiscal", OnClickFn = "fnNotaFiscalInutilizadaList" });
            }

            return target;
        }

        private byte[] GetXMLFile(NotaFiscalVM notafiscal)
        {

            try
            {
                var resourceById = string.Format("NotaFiscalXML?&id={0}", notafiscal.Id);
                var response = RestHelper.ExecuteGetRequest<JObject>(resourceById);

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
        private byte[] GetPDFFile(NotaFiscalVM notafiscal)
        {
            try
            {
                var resourceById = string.Format("NotaFiscalPDF?&id={0}", notafiscal.Id);
                var response = RestHelper.ExecuteGetRequest<JObject>(resourceById);

                string fileBase64 = response.Value<string>("pdf");

                byte[] bytes = Encoding.ASCII.GetBytes(fileBase64);

                return bytes;
            }
            catch (Exception ex)
            {
                ErrorInfo error = JsonConvert.DeserializeObject<ErrorInfo>(ex.Message);
                throw new Exception(error.Message);
            }
        }

        [OperationRole(PermissionValue = EPermissionValue.Read)]
        public JsonResult EnviaEmailNotaFiscal(string id)
        {
            try
            {
                var empresa = GetDadosEmpresa();
                var notaFiscal = Get(Guid.Parse(id));

                if (notaFiscal.Cliente == null)
                {
                    return JsonResponseStatus.GetFailure("Nenhum cliente foi encontrado.");
                }

                if (string.IsNullOrEmpty(notaFiscal.Cliente.Email))
                {
                    return JsonResponseStatus.GetFailure("Não foi encontrado um email válido para este cliente.");
                }

                if (string.IsNullOrEmpty(empresa.Email))
                {
                    return JsonResponseStatus.GetFailure("Você ainda não configurou um email válido para sua empresa.");
                }
              
                var anexo = File(GetXMLFile(notaFiscal), ".xml");// File(GetPDFFile(ordemVenda), "application/pdf");// XML e Danfe
                var anexo2 = File(GetPDFFile(notaFiscal), ".pdf");
                var tituloEmail = $"{empresa.NomeFantasia} {notaFiscal.TipoNotaFiscal} - Nº {notaFiscal.NumNotaFiscal}".ToUpper();
                var mensagemPrincipal = $"Você está recebendo uma cópia do XML e Danfe da sua {notaFiscal.TipoNotaFiscal}.".ToUpper();
                var conteudoEmail = Mail.FormataMensagem(EmailFilesHelper.GetTemplate("Templates.OrdemVenda.html").Value, tituloEmail, mensagemPrincipal, empresa.Email);
                var arquivoAnexo = new FileStreamResult(new MemoryStream(anexo.FileContents), ".xml");


                Mail.Send(empresa.NomeFantasia, notaFiscal.Cliente.Email, tituloEmail, conteudoEmail, arquivoAnexo.FileStream, arquivoAnexo.ContentType);

                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var error = JsonConvert.DeserializeObject<ErrorInfo>(ex.Message);
                return JsonResponseStatus.GetFailure(error.Message);
            }
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
                    OrderDir = "desc",
                    NoExportButtons = true
                }
            };

            config.Actions.AddRange(GetActionsInGrid(new List<DataTableUIAction>()
            {
                new DataTableUIAction { OnClickFn = "fnVisualizarNFe", Label = "Visualizar", ShowIf = "(row.tipoNotaFiscal == 'NFe')" },
                new DataTableUIAction { OnClickFn = "fnVisualizarRetornoSefaz", Label = "Mensagem SEFAZ", ShowIf = "(row.status != 'NaoTransmitida' && row.tipoNotaFiscal == 'NFe')" },
                new DataTableUIAction { OnClickFn = "fnVisualizarNFSe", Label = "Visualizar", ShowIf = "(row.tipoNotaFiscal == 'NFSe')" },
                new DataTableUIAction { OnClickFn = "fnVisualizarRetornoValidacao", Label = "Mensagem Validação", ShowIf = "(row.status != 'NaoTransmitida' && row.tipoNotaFiscal == 'NFSe')" },
                new DataTableUIAction { OnClickFn = "fnTransmitirNFe", Label = "Transmitir", ShowIf = "((row.status == 'NaoAutorizada' || row.status == 'NaoTransmitida' || row.status == 'FalhaTransmissao') && row.tipoNotaFiscal == 'NFe')" },
                new DataTableUIAction { OnClickFn = "fnTransmitirNFSe", Label = "Transmitir", ShowIf = "((row.status == 'NaoAutorizada' || row.status == 'NaoTransmitida' || row.status == 'FalhaTransmissao') && row.tipoNotaFiscal == 'NFSe')" },
                new DataTableUIAction { OnClickFn = "fnExcluirNFe", Label = "Excluir", ShowIf = "((row.status == 'NaoAutorizada' || row.status == 'NaoTransmitida' || row.status == 'FalhaTransmissao') && row.tipoNotaFiscal == 'NFe')" },
                new DataTableUIAction { OnClickFn = "fnExcluirNFSe", Label = "Excluir", ShowIf = "((row.status == 'NaoAutorizada' || row.status == 'NaoTransmitida' || row.status == 'FalhaTransmissao') && row.tipoNotaFiscal == 'NFSe')" },
                new DataTableUIAction { OnClickFn = "fnBaixarXMLNFe", Label = "Baixar XML", ShowIf = "((row.status == 'NaoAutorizada' || row.status == 'Autorizada' || row.status == 'Transmitida' || row.status == 'FalhaTransmissao') && row.tipoNotaFiscal == 'NFe')" },
                new DataTableUIAction { OnClickFn = "fnBaixarPDFNFe", Label = "Baixar PDF", ShowIf = "(row.status == 'Autorizada' && row.tipoNotaFiscal == 'NFe')" },
                new DataTableUIAction { OnClickFn = "fnBaixarXMLNFSe", Label = "Baixar XML", ShowIf = "((row.status == 'FalhaTransmissao' || row.status == 'NaoAutorizada' || row.status == 'Autorizada' || row.status == 'Transmitida') && row.tipoNotaFiscal == 'NFSe')" },
                new DataTableUIAction { OnClickFn = "fnBaixarXMLUnicoNFSe", Label = "Baixar XML TSS", ShowIf = "((row.status == 'NaoAutorizada' || row.status == 'Transmitida' || row.status == 'FalhaTransmissao') && row.tipoNotaFiscal == 'NFSe')" },
                new DataTableUIAction { OnClickFn = "fnCancelarNFe", Label = "Cancelar", ShowIf = "((row.status == 'Autorizada' || row.status == 'FalhaNoCancelamento') && row.tipoNotaFiscal == 'NFe')" },
                new DataTableUIAction { OnClickFn = "fnCancelarNFSe", Label = "Cancelar", ShowIf = "((row.status == 'Autorizada' || row.status == 'FalhaNoCancelamento') && row.tipoNotaFiscal == 'NFSe')" },
                new DataTableUIAction { OnClickFn = "fnFormCartaCorrecao", Label = "Carta de Correção", ShowIf = "(row.status == 'Autorizada')" },
                new DataTableUIAction { OnClickFn = "fnEnviarEmailNotaFiscal", Label = "Enviar por e-mail", ShowIf = "(row.status != 'NaoTransmitida')" }
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
                DataField = "tipoVenda",
                DisplayName = "Finalidade",
                Priority = 5,
                Options = new List<SelectOptionUI>(SystemValueHelper.GetUIElementBase(typeof(TipoCompraVenda))),
                RenderFn = "fnRenderEnum(full.tipoVendaCssClass, full.tipoVendaDescription)"
            });
            config.Columns.Add(new DataTableUIColumn { DataField = "cliente_nome", DisplayName = "Cliente", Priority = 6 });
            config.Columns.Add(new DataTableUIColumn { DataField = "data", DisplayName = "Data", Priority = 7, Type = "date" });
            config.Columns.Add(new DataTableUIColumn { DataField = "ordemVendaOrigem_numero", DisplayName = "Pedido Origem", Searchable = false, Priority = 8 });//numero int e pesquisa string
            config.Columns.Add(new DataTableUIColumn { DataField = "categoria_descricao", DisplayName = "Categoria", Priority = 9 });

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
    }
}