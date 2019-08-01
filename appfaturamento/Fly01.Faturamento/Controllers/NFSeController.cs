using Fly01.Core;
using Fly01.Core.Config;
using Fly01.Core.Entities.Domains.Enum;
using Fly01.Core.Helpers;
using Fly01.Core.Presentation;
using Fly01.Core.Presentation.Commons;
using Fly01.Core.Presentation.JQueryDataTable;
using Fly01.Core.Rest;
using Fly01.Core.ViewModels;
using Fly01.Faturamento.ViewModel;
using Fly01.uiJS.Classes;
using Fly01.uiJS.Classes.Elements;
using Fly01.uiJS.Defaults;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Fly01.Faturamento.Controllers
{
    [OperationRole(ResourceKey = ResourceHashConst.FaturamentoFaturamentoNotasFiscais)]
    public class NFSeController : BaseController<NFSeVM>
    {
        //NFeVM e NFSeVM na mesma controller notaFiscal, direcionado as controller via javaScript
        public NFSeController()
        {
            ExpandProperties = "ordemVendaOrigem($select=numero),cliente($select=id,nome),transportadora($select=id,nome),estadoPlacaVeiculo,condicaoParcelamento,formaPagamento,categoria,serieNotaFiscal,centroCusto,certificadoDigital($select=cnpj,inscricaoEstadual,uf,entidadeHomologacao,entidadeProducao)";
        }

        public override Func<NFSeVM, object> GetDisplayData() { throw new NotImplementedException(); }

        public ContentResult Modal()
        {
            ModalUIForm config = new ModalUIForm()
            {
                Title = "Visualizar NFS-e",
                UrlFunctions = Url.Action("Functions") + "?fns=",
                CancelAction = new ModalUIAction() { Label = "Cancelar" },
                Action = new FormUIAction
                {
                    Create = @Url.Action("Create"),
                    Edit = @Url.Action("Edit"),
                    Get = @Url.Action("Json") + "/",
                    List = @Url.Action("List", "NotaFiscal")
                },
                ReadyFn = "fnFormReadyVisualizarNFSe",
                Id = "fly01mdlfrmVisualizarNFSe"
            };

            config.Elements.Add(new InputHiddenUI { Id = "id" });
            config.Elements.Add(new InputNumbersUI { Id = "ordemVendaOrigemNumero", Class = "col s12 m2", Label = "Pedido Origem", Disabled = true });
            config.Elements.Add(new SelectUI
            {
                Id = "tipoVenda",
                Class = "col s12 m4",
                Label = "Tipo Venda",
                Disabled = true,
                Options = new List<SelectOptionUI>(SystemValueHelper.GetUIElementBase(typeof(TipoCompraVenda)).
                ToList().FindAll(x => "Normal,Devolucao,Complementar".Contains(x.Value)))
            });
            config.Elements.Add(new SelectUI
            {
                Id = "status",
                Class = "col s12 m6",
                Label = "Status",
                Disabled = true,
                Options = new List<SelectOptionUI>(SystemValueHelper.GetUIElementBase(typeof(StatusNotaFiscal)))
            });
            config.Elements.Add(new AutoCompleteUI
            {
                Id = "serieNotaFiscalIdNFSe",
                Class = "col s12 m6",
                Label = "Série",
                Required = true,
                Name = "serieNotaFiscalId",
                Disabled = true,
                DataUrl = Url.Action("SerieNFSe", "AutoComplete"),
                LabelId = "serieNotaFiscalSerieNFSe",
                LabelName = "serieNotaFiscalSerie"
            });
            config.Elements.Add(new InputNumbersUI { Id = "numNotaFiscalNFSe", Class = "col s12 m6", Label = "Número Nota Fiscal", Required = true, MinLength = 1, MaxLength = 9, Name = "numNotaFiscal", Disabled = true });
            config.Elements.Add(new InputDateUI { Id = "data", Class = "col s12 m4", Label = "Data", Disabled = true });
            config.Elements.Add(new AutoCompleteUI
            {
                Id = "clienteId",
                Class = "col s12 m8",
                Label = "Cliente",
                Disabled = true,
                DataUrl = Url.Action("Cliente", "AutoComplete"),
                LabelId = "clienteNome"
            });
            config.Elements.Add(new TextAreaUI { Id = "observacao", Class = "col s12", Label = "Observação", Disabled = true, MaxLength = 200 });
            config.Elements.Add(new InputTextUI { Id = "naturezaOperacao", Class = "col s12", Label = "Natureza de Operação", Disabled = true });
            config.Elements.Add(new InputTextUI { Id = "sefazId", Class = "col s12", Label = "Sefaz chave de acesso", Disabled = true });

            config.Elements.Add(new AutoCompleteUI
            {
                Id = "formaPagamentoId",
                Class = "col s12 m6",
                Label = "Forma Pagamento",
                Disabled = true,
                DataUrl = Url.Action("FormaPagamento", "AutoComplete"),
                LabelId = "formaPagamentoDescricao"
            });
            config.Elements.Add(new AutoCompleteUI
            {
                Id = "condicaoParcelamentoId",
                Class = "col s12 m6",
                Label = "Condição Parcelamento",
                Disabled = true,
                DataUrl = Url.Action("CondicaoParcelamento", "AutoComplete"),
                LabelId = "condicaoParcelamentoDescricao"
            });
            config.Elements.Add(new AutoCompleteUI
            {
                Id = "categoriaId",
                Class = "col s12 m6",
                Label = "Categoria",
                Disabled = true,
                DataUrl = @Url.Action("Categoria", "AutoComplete"),
                LabelId = "categoriaDescricao",
            });
            config.Elements.Add(new AutoCompleteUI
            {
                Id = "centroCustoId",
                Class = "col s12 m6",
                Label = "Centro de Custo",
                Disabled = true,
                DataUrl = @Url.Action("CentroCusto", "AutoComplete"),
                LabelId = "centroCustoDescricao",
            });
            config.Elements.Add(new InputDateUI { Id = "dataVencimento", Class = "col s12 m6", Label = "Data Vencimento", Disabled = true });

            config.Elements.Add(new LabelSetUI { Id = "labelSetTotais", Class = "col s12", Label = "Totais" });
            config.Elements.Add(new InputCurrencyUI { Id = "totalServicos", Class = "col s12 m6", Label = "Total serviços", Readonly = true });
            config.Elements.Add(new InputCurrencyUI { Id = "totalRetencoesServicos", Class = "col s12 m6", Label = "Total retenções serviços", Readonly = true });
            config.Elements.Add(new InputCurrencyUI { Id = "totalImpostosServicosNaoAgrega", Class = "col s12 m6", Label = "Total de impostos não incidentes", Readonly = true });
            config.Elements.Add(new InputCurrencyUI { Id = "totalNotaFiscal", Class = "col s12 m6", Label = "Total (serviços - retenções)", Readonly = true });

            config.Elements.Add(new LabelSetUI { Id = "labelSetServicos", Class = "col s12", Label = "Serviços" });
            config.Elements.Add(new TableUI
            {
                Id = "nfseServicosDataTable",
                Class = "col s12",
                Disabled = true,
                Options = new List<OptionUI>
                {
                    new OptionUI { Label = "Serviço", Value = "0"},
                    new OptionUI { Label = "GrupoTributário", Value = "0"},
                    new OptionUI { Label = "Quant.", Value = "1"},
                    new OptionUI { Label = "Valor",Value = "2"},
                    new OptionUI { Label = "Desconto",Value = "3"},
                    new OptionUI { Label = "Outras Retenções",Value = "5"},
                    new OptionUI { Label = "Total",Value = "4"},
                }
            });
            config.Elements.Add(new LabelSetUI { Id = "labelSetTransmissao", Class = "col s12", Label = "Transmissão" });
            config.Elements.Add(new TextAreaUI { Id = "mensagem", Class = "col s12", Label = "Mensagem", Disabled = true });
            config.Elements.Add(new TextAreaUI { Id = "recomendacao", Class = "col s12", Label = "Recomendação", Disabled = true });

            config.Elements.Add(new LabelSetUI { Id = "labelSetDadosTransmissao", Class = "col s12", Label = "Dados da Transmissão" });
            config.Elements.Add(new SelectUI
            {
                Id = "tipoAmbiente",
                Class = "col s12 m6 l4",
                Label = "Ambiente",
                Disabled = true,
                Options = new List<SelectOptionUI>(SystemValueHelper.GetUIElementBase(typeof(TipoAmbiente)).ToList())
            });
            config.Elements.Add(new InputTextUI { Id = "certificadoDigitalEntidadeHomologacao", Class = "col s12 m6 l4", Label = "Entidade TSS homologação", Disabled = true });
            config.Elements.Add(new InputTextUI { Id = "certificadoDigitalEntidadeProducao", Class = "col s12 m6 l4", Label = "Entidade TSS produção", Disabled = true });
            config.Elements.Add(new InputCpfcnpjUI { Id = "certificadoDigitalCnpj", Class = "col s12 m6 l4", Label = "CNPJ empresa", Disabled = true });
            config.Elements.Add(new InputTextUI { Id = "certificadoDigitalInscricaoEstadual", Class = "col s12 m6 l4", Label = "I.E. empresa", Disabled = true });
            config.Elements.Add(new InputTextUI { Id = "certificadoDigitalUf", Class = "col s12 m6 l4", Label = "UF empresa", Disabled = true });

            return Content(JsonConvert.SerializeObject(config, JsonSerializerSetting.Front), "application/json");
        }

        public override ContentResult List() { throw new NotImplementedException(); }

        [OperationRole(PermissionValue = EPermissionValue.Write)]
        public ContentResult ModalTransmitir()
        {
            ModalUIForm config = new ModalUIForm()
            {
                Title = "Transmitir NFS-e",
                UrlFunctions = Url.Action("Functions") + "?fns=",
                CancelAction = new ModalUIAction() { Label = "Cancelar" },
                Action = new FormUIAction
                {
                    Edit = @Url.Action("Transmitir"),
                    Get = @Url.Action("Json") + "/",
                    //List = @Url.Action("List", "NotaFiscal")
                },
                ReadyFn = "fnFormReadyTransmitirNFSe",
                Id = "fly01mdlfrmTransmitirNFSe"
            };
            var empresa = ApiEmpresaManager.GetEmpresa(SessionManager.Current.UserData.PlatformUrl);
            var cidadeHomologadaTss = (!string.IsNullOrEmpty(empresa?.Cidade?.CodigoIbge) && NFSeTssHelper.IbgesCidadesHomologadasTssNFSe.Contains(empresa?.Cidade?.CodigoIbge));
            if (cidadeHomologadaTss)
            {
                config.ConfirmAction = new ModalUIAction() { Label = "Confirmar" };
            }

            config.Elements.Add(new InputHiddenUI { Id = "cidadeHomologadaTss", Value = cidadeHomologadaTss.ToString() });
            config.Elements.Add(new InputHiddenUI { Id = "idNFSe", Name = "id" });
            config.Elements.Add(new SelectUI
            {
                Id = "tipoVendaNFSe",
                Class = "col s12 m6",
                Label = "Tipo Venda",
                Disabled = true,
                Name = "tipoVenda",
                Options = new List<SelectOptionUI>(SystemValueHelper.GetUIElementBase(typeof(TipoCompraVenda)).
                ToList().FindAll(x => "Normal,Devolucao,Complementar".Contains(x.Value)))
            });
            config.Elements.Add(new InputDateUI { Id = "dataNFSe", Class = "col s12 m6", Label = "Data", Disabled = true, Name = "data" });
            config.Elements.Add(new AutoCompleteUI
            {
                Id = "clienteIdNFSe",
                Class = "col s12",
                Label = "Cliente",
                Disabled = true,
                Name = "clienteId",
                DataUrl = Url.Action("Cliente", "AutoComplete"),
                LabelId = "clienteNomeNFSe",
                LabelName = "clienteNome"
            });

            config.Elements.Add(new InputCurrencyUI { Id = "totalServicos", Class = "col s12 m6", Label = "Total serviços", Readonly = true });
            config.Elements.Add(new InputCurrencyUI { Id = "totalRetencoesServicos", Class = "col s12 m6", Label = "Total retenções serviços", Readonly = true });
            config.Elements.Add(new InputCurrencyUI { Id = "totalImpostosServicosNaoAgrega", Class = "col s12 m6", Label = "Total de impostos não incidentes", Readonly = true });
            config.Elements.Add(new InputCurrencyUI { Id = "totalNotaFiscalNFSe", Class = "col s12 m6", Label = "Total (serviços - retenções)", Readonly = true, Name = "totalNotaFiscal" });

            if (cidadeHomologadaTss)
            {
                config.Elements.Add(new AutoCompleteUI
                {
                    Id = "serieNotaFiscalIdNFSe",
                    Class = "col s12 m6",
                    Label = "Série",
                    Required = true,
                    Name = "serieNotaFiscalId",
                    DataUrl = Url.Action("SerieNFSe", "AutoComplete"),
                    LabelId = "serieNotaFiscalSerieNFSe",
                    LabelName = "serieNotaFiscalSerie",
                    DomEvents = new List<DomEventUI>
                {
                    new DomEventUI { DomEvent = "autocompleteselect", Function = "fnChangeSerieNFSe" }
                },
                    DataUrlPostModal = Url.Action("FormModalNFSe", "SerieNotaFiscal"),
                    DataPostField = "serie"
                });
                config.Elements.Add(new InputNumbersUI { Id = "numNotaFiscalNFSe", Class = "col s12 m6", Label = "Número Nota Fiscal", Required = true, MinLength = 1, MaxLength = 9, Name = "numNotaFiscal" });
            }
            else
            {
                config.Elements.Add(new DivElementUI { Id = "infoCidadeHomologada", Class = "col s12 text-justify visible", Label = "Informação" });
            }

            return Content(JsonConvert.SerializeObject(config, JsonSerializerSetting.Front), "application/json");
        }

        [OperationRole(PermissionValue = EPermissionValue.Write)]
        [HttpPost]
        public JsonResult Transmitir(NFSeVM entity)
        {
            try
            {
                dynamic NFSe = new
                {
                    status = "Transmitida",
                    serieNotaFiscalId = entity.SerieNotaFiscalId,
                    numNotaFiscal = entity.NumNotaFiscal
                };

                var resourceNamePut = $"{"NFSe"}/{entity.Id}";
                RestHelper.ExecutePutRequest(resourceNamePut, JsonConvert.SerializeObject(NFSe));

                return JsonResponseStatus.GetSuccess("NFS-e transmitida com sucesso");
            }
            catch (Exception ex)
            {
                var error = JsonConvert.DeserializeObject<ErrorInfo>(ex.Message);
                return JsonResponseStatus.GetFailure(error.Message);
            }
        }

        [OperationRole(PermissionValue = EPermissionValue.Read)]
        [HttpGet]
        public JsonResult TotalNFSeServicos(string id)
        {
            try
            {
                Dictionary<string, string> queryString = new Dictionary<string, string>();
                queryString.AddParam("$filter", $"notaFiscalId eq {id} and ativo eq true");

                var total = RestHelper.ExecuteGetRequest<ResultBase<NFSeServicoVM>>("NFSeServico", queryString).Data.Sum(x => x.Total);

                return Json(
                    new { totalNFSeServicos = total, success = true },
                    JsonRequestBehavior.AllowGet
                );
            }
            catch (Exception ex)
            {
                var error = JsonConvert.DeserializeObject<ErrorInfo>(ex.Message);
                return JsonResponseStatus.GetFailure(error.Message);
            }
        }

        [OperationRole(PermissionValue = EPermissionValue.Read)]
        public ActionResult BaixarXMLUnico(Guid id)
        {
            try
            {
                var response = base.Get(id);

                string fileName = "NFSeTSS" + response.NumNotaFiscal + ".xml";
                string xml = response.XMLUnicoTSS;
                xml = xml.Replace("\\", "");
                Session.Add(fileName, xml);

                return JsonResponseStatus.GetJson(new { downloadAddress = Url.Action("DownloadXMLString", new { fileName }) });
            }
            catch (Exception ex)
            {
                ErrorInfo error = JsonConvert.DeserializeObject<ErrorInfo>(ex.Message);
                return JsonResponseStatus.GetFailure(error.Message);
            }
        }

        [OperationRole(PermissionValue = EPermissionValue.Read)]
        public ActionResult BaixarXML(Guid id)
        {
            try
            {
                var resourceById = string.Format("NotaFiscalXML?&id={0}", id);
                var response = RestHelper.ExecuteGetRequest<JObject>(resourceById);

                string fileName = "NFSe" + response.Value<string>("numNotaFiscal") + ".xml";
                string xml = response.Value<string>("xml");
                xml = xml.Replace("\\", "");
                Session.Add(fileName, xml);

                return JsonResponseStatus.GetJson(new { downloadAddress = Url.Action("DownloadXMLString", new { fileName = fileName }) });
            }
            catch (Exception ex)
            {
                ErrorInfo error = JsonConvert.DeserializeObject<ErrorInfo>(ex.Message);
                return JsonResponseStatus.GetFailure(error.Message);
            }
        }

        [OperationRole(PermissionValue = EPermissionValue.Read)]
        public ActionResult BaixarPDF(Guid id)
        {
            try
            {
                var resourceById = string.Format("NotaFiscalPDF?&id={0}", id);
                var response = RestHelper.ExecuteGetRequest<JObject>(resourceById);

                string fileName = "NFSe" + response.Value<string>("numNotaFiscal") + ".pdf";
                string fileBase64 = response.Value<string>("pdf");
                Session.Add(fileName, fileBase64);

                return JsonResponseStatus.GetJson(new { downloadAddress = Url.Action("DownloadPDF", new { fileName = fileName }) });
            }
            catch (Exception ex)
            {
                ErrorInfo error = JsonConvert.DeserializeObject<ErrorInfo>(ex.Message);
                return JsonResponseStatus.GetFailure(error.Message);
            }
        }

        [OperationRole(PermissionValue = EPermissionValue.Write)]
        [HttpPost]
        public JsonResult Cancelar(Guid id)
        {
            try
            {
                var resourceById = string.Format("NotaFiscalCancelar?&id={0}", id);
                RestHelper.ExecutePostRequest(resourceById, string.Empty);

                return JsonResponseStatus.GetSuccess("Cancelamento solicitado com sucesso");
            }
            catch (Exception ex)
            {
                var error = JsonConvert.DeserializeObject<ErrorInfo>(ex.Message);
                return JsonResponseStatus.GetFailure(error.Message);
            }
        }

        protected override ContentUI FormJson()
        {
            throw new NotImplementedException();
        }

        [OperationRole(PermissionValue = EPermissionValue.Read)]
        public ContentResult FormRetornoValidacao()
        {
            ModalUIForm config = new ModalUIForm()
            {
                Title = "Mensagem Validação",
                UrlFunctions = @Url.Action("Functions") + "?fns=",
                CancelAction = new ModalUIAction() { Label = "Cancelar" },
                Action = new FormUIAction
                {
                    Create = @Url.Action("Create"),
                    Edit = @Url.Action("Edit"),
                    Get = @Url.Action("Json") + "/",
                    List = @Url.Action("List", "NotaFiscal")
                },
                Id = "fly01mdlfrmVisualizarMensagemValidacao"
            };

            config.Elements.Add(new InputHiddenUI { Id = "id" });
            config.Elements.Add(new TextAreaUI { Id = "mensagem", Class = "col s12", Label = "Mensagem", Disabled = true });
            config.Elements.Add(new TextAreaUI { Id = "recomendacao", Class = "col s12", Label = "Recomendação", Disabled = true });

            return Content(JsonConvert.SerializeObject(config, JsonSerializerSetting.Front), "application/json");
        }

        protected override List<JQueryDataTableParamsColumn> GetParamsColumns()
        {
            throw new NotImplementedException();
        }
    }
}