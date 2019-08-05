using Fly01.Core.Entities.Domains.Enum;
using Fly01.Core.Helpers;
using Fly01.Core.Presentation;
using Fly01.Core.Presentation.Commons;
using Fly01.Core.Rest;
using Fly01.Core.ViewModels;
using Fly01.Compras.ViewModel;
using Fly01.uiJS.Classes;
using Fly01.uiJS.Classes.Elements;
using Fly01.uiJS.Defaults;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Fly01.Core.Presentation.JQueryDataTable;

namespace Fly01.Compras.Controllers
{
    [OperationRole(ResourceKey = ResourceHashConst.ComprasComprasNotasFiscais)]
    public class NFeEntradaController : BaseController<NFeEntradaVM>
    {
        //NFeVM e NFSeVM na mesma controller notaFiscal, direcionado as controller via javaScript
        public NFeEntradaController()
        {
            ExpandProperties = "ordemCompraOrigem($select=numero),fornecedor($select=id,nome),transportadora($select=id,nome),estadoPlacaVeiculo,condicaoParcelamento,formaPagamento,categoria,serieNotaFiscal,centroCusto,certificadoDigital($select=cnpj,inscricaoEstadual,uf,entidadeHomologacao,entidadeProducao)";
        }

        public override Func<NFeEntradaVM, object> GetDisplayData()
        {
            throw new NotImplementedException();
        }

        public ContentResult Modal()
        {
            ModalUIForm config = new ModalUIForm()
            {
                Title = "Visualizar NF-e",
                UrlFunctions = @Url.Action("Functions") + "?fns=",
                CancelAction = new ModalUIAction() { Label = "Cancelar" },
                Action = new FormUIAction
                {
                    Create = @Url.Action("Create"),
                    Edit = @Url.Action("Edit"),
                    Get = @Url.Action("Json") + "/",
                    List = @Url.Action("List", "NotaFiscal")
                },
                ReadyFn = "fnFormReadyVisualizarNFe",
                Id = "fly01mdlfrmVisualizarNFe"
            };

            config.Elements.Add(new InputHiddenUI { Id = "id" });
            config.Elements.Add(new InputNumbersUI { Id = "ordemCompraOrigemNumero", Class = "col s12 m6 l2", Label = "Pedido Origem", Disabled = true });
            config.Elements.Add(new SelectUI
            {
                Id = "tipoCompra",
                Class = "col s12 m6 l3",
                Label = "Tipo Compra",
                Disabled = true,
                Options = new List<SelectOptionUI>(SystemValueHelper.GetUIElementBase(typeof(TipoCompraVenda)).
                ToList().FindAll(x => "Normal,Devolucao,Complementar".Contains(x.Value)))
            });
            config.Elements.Add(new SelectUI
            {
                Id = "tipoNfeComplementar",
                Class = "col s12 m6 l3",
                Label = "Tipo Complemento",
                Disabled = true,
                Options = new List<SelectOptionUI>(SystemValueHelper.GetUIElementBase(typeof(TipoNfeComplementar)).ToList())
            });
            config.Elements.Add(new SelectUI
            {
                Id = "status",
                Class = "col s12 m6 l4",
                Label = "Status",
                Disabled = true,
                Options = new List<SelectOptionUI>(SystemValueHelper.GetUIElementBase(typeof(StatusNotaFiscal)))
            });
            config.Elements.Add(new AutoCompleteUI
            {
                Id = "serieNotaFiscalIdNFe",
                Class = "col s12 m6",
                Label = "Série",
                Required = true,
                Name = "serieNotaFiscalId",
                Disabled = true,
                DataUrl = Url.Action("SerieNFe", "AutoComplete"),
                LabelId = "serieNotaFiscalSerieNFe",
                LabelName = "serieNotaFiscalSerie"
            });
            config.Elements.Add(new InputNumbersUI { Id = "numNotaFiscalNFe", Class = "col s12 m6", Label = "Número Nota Fiscal", Required = true, MinLength = 1, MaxLength = 9, Name = "numNotaFiscal", Disabled = true });
            config.Elements.Add(new InputDateUI { Id = "data", Class = "col s12 m4", Label = "Data", Disabled = true });
            config.Elements.Add(new AutoCompleteUI
            {
                Id = "fornecedorId",
                Class = "col s12 m8",
                Label = "Fornecedor",
                Disabled = true,
                DataUrl = Url.Action("Fornecedor", "AutoComplete"),
                LabelId = "fornecedorNome"
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
            config.Elements.Add(new SelectUI
            {
                Id = "tipoFrete",
                Class = "col s12 m6",
                Label = "Tipo Frete",
                Disabled = true,
                Options = new List<SelectOptionUI>(SystemValueHelper.GetUIElementBase(typeof(TipoFrete))),
            });
            config.Elements.Add(new AutoCompleteUI
            {
                Id = "transportadoraId",
                Class = "col s12",
                Label = "Transportadora",
                Disabled = true,
                DataUrl = Url.Action("Transportadora", "AutoComplete"),
                LabelId = "transportadoraNome"
            });
            config.Elements.Add(new InputCustommaskUI
            {
                Id = "placaVeiculo",
                Class = "col s12 m4",
                Label = "Placa Veículo",
                Disabled = true,
                Data = new { inputmask = "'mask':'AAA[-9999]|[9A99]', 'showMaskOnHover': false, 'autoUnmask':true, 'greedy':true" }
            });
            config.Elements.Add(new AutoCompleteUI
            {
                Id = "estadoPlacaVeiculoId",
                Class = "col s12 m4",
                Label = "UF Placa Veículo",
                Disabled = true,
                DataUrl = Url.Action("Estado", "AutoComplete"),
                LabelId = "estadoPlacaVeiculoNome"
            });
            config.Elements.Add(new InputCurrencyUI { Id = "valorFrete", Class = "col s12 m4", Label = "Valor Frete", Disabled = true });
            config.Elements.Add(new InputFloatUI { Id = "pesoBruto", Class = "col s12 m4", Label = "Peso Bruto", Digits = 3, Disabled = true });
            config.Elements.Add(new InputTextUI { Id = "marca", Class = "col s12 m4", Label = "Marca", MaxLength = 60, Disabled = true});
            config.Elements.Add(new InputFloatUI { Id = "pesoLiquido", Class = "col s12 m4", Label = "Peso Líquido", Digits = 3, Disabled = true });
            config.Elements.Add(new InputNumbersUI { Id = "quantidadeVolumes", Class = "col s12 m4", Label = "Quantidade Volumes", Disabled = true });
            config.Elements.Add(new InputTextUI { Id = "tipoEspecie", Class = "col s12 m4", Label = "Tipo Espécie", MaxLength = 60, Disabled = true});
            config.Elements.Add(new InputTextUI { Id = "numeracaoVolumesTrans", Class = "col s12 m4", Label = "Numeração", MaxLength = 60, Disabled = true});

            config.Elements.Add(new LabelSetUI { Id = "labelSetProdutos", Class = "col s12", Label = "Produtos" });
            config.Elements.Add(new TableUI
            {
                Id = "nfeProdutosDataTable",
                Class = "col s12",
                Disabled = true,
                Options = new List<OptionUI>
                {
                    new OptionUI { Label = "Produto", Value = "0"},
                    new OptionUI { Label = "GrupoTributário", Value = "0"},
                    new OptionUI { Label = "Quant.", Value = "1"},
                    new OptionUI { Label = "Valor",Value = "2"},
                    new OptionUI { Label = "Desconto",Value = "3"},
                    new OptionUI { Label = "Total",Value = "4"},
                }
            });

            config.Elements.Add(new LabelSetUI { Id = "labelSetTotais", Class = "col s12", Label = "Totais" });
            config.Elements.Add(new InputCurrencyUI { Id = "totalProdutos", Class = "col s12 m6", Label = "Total produtos", Readonly = true });
            config.Elements.Add(new InputCurrencyUI { Id = "totalFrete", Class = "col s12 m6", Label = "Frete a pagar", Readonly = true });
            config.Elements.Add(new InputCurrencyUI { Id = "totalImpostosProdutos", Class = "col s12 m6", Label = "Total de impostos incidentes", Readonly = true });
            config.Elements.Add(new InputCurrencyUI { Id = "totalImpostosProdutosNaoAgrega", Class = "col s12 m6", Label = "Total de impostos não incidentes", Readonly = true });
            config.Elements.Add(new InputCurrencyUI { Id = "totalNotaFiscal", Class = "col s12", Label = "Total (produtos + impostos incidentes + frete)", Readonly = true });

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

        [OperationRole(PermissionValue = EPermissionValue.Read)]
        public ContentResult FormRetornoSefaz()
        {
            ModalUIForm config = new ModalUIForm()
            {
                Title = "Mensagem SEFAZ",
                UrlFunctions = @Url.Action("Functions") + "?fns=",
                CancelAction = new ModalUIAction() { Label = "Cancelar" },
                Action = new FormUIAction
                {
                    Create = @Url.Action("Create"),
                    Edit = @Url.Action("Edit"),
                    Get = @Url.Action("Json") + "/",
                    List = @Url.Action("List", "NotaFiscal")
                },
                Id = "fly01mdlfrmVisualizarRetornoSefaz"
            };

            config.Elements.Add(new InputHiddenUI { Id = "id" });            
            config.Elements.Add(new TextAreaUI { Id = "mensagem", Class = "col s12", Label = "Mensagem", Disabled = true });
            config.Elements.Add(new TextAreaUI { Id = "recomendacao", Class = "col s12", Label = "Recomendação", Disabled = true });

            return Content(JsonConvert.SerializeObject(config, JsonSerializerSetting.Front), "application/json");
        }

        public override ContentResult List()
        {
            throw new NotImplementedException();
        }

        [OperationRole(PermissionValue = EPermissionValue.Write)]
        public ContentResult ModalTransmitir()
        {
            ModalUIForm config = new ModalUIForm()
            {
                Title = "Transmitir NF-e",
                UrlFunctions = @Url.Action("Functions") + "?fns=",
                CancelAction = new ModalUIAction() { Label = "Cancelar" },
                ConfirmAction = new ModalUIAction() { Label = "Confirmar" },
                Action = new FormUIAction
                {
                    Edit = @Url.Action("Transmitir"),
                    Get = @Url.Action("Json") + "/",
                    //List = @Url.Action("List", "NotaFiscal")
                },
                Id = "fly01mdlfrmTransmitirNFe",
                ReadyFn = "fnFormReadyTransmitirNFe",
            };

            config.Elements.Add(new InputHiddenUI { Id = "idNFe", Name = "id" });
            config.Elements.Add(new SelectUI
            {
                Id = "tipoCompraNFe",
                Class = "col s12 m6",
                Label = "Tipo Compra",
                Disabled = true,
                Name = "tipoCompra",
                Options = new List<SelectOptionUI>(SystemValueHelper.GetUIElementBase(typeof(TipoCompraVenda)).
                ToList().FindAll(x => "Normal,Devolucao,Complementar".Contains(x.Value)))
            });
            config.Elements.Add(new InputDateUI { Id = "dataNFe", Class = "col s12 m6", Label = "Data", Disabled = true, Name = "data" });
            config.Elements.Add(new AutoCompleteUI
            {
                Id = "fornecedorIdNFe",
                Class = "col s12",
                Label = "Fornecedor",
                Disabled = true,
                Name = "fornecedorId",
                DataUrl = Url.Action("Fornecedor", "AutoComplete"),
                LabelId = "fornecedorNomeNFe",
                LabelName = "fornecedorNome"
            });
            config.Elements.Add(new InputHiddenUI { Id = "tipoFreteNFe", Name = "tipoFrete" });
            config.Elements.Add(new InputHiddenUI { Id = "valorFreteNFe", Name = "valorFrete" });
            config.Elements.Add(new InputCurrencyUI { Id = "totalProdutos", Class = "col s12 m6", Label = "Total produtos", Readonly = true });
            config.Elements.Add(new InputCurrencyUI { Id = "totalFreteNFe", Class = "col s12 m6", Label = "Frete a pagar", Readonly = true, Name = "totalFrete" });
            config.Elements.Add(new InputCurrencyUI { Id = "totalImpostosProdutos", Class = "col s12 m6", Label = "Total de impostos incidentes", Readonly = true });
            config.Elements.Add(new InputCurrencyUI { Id = "totalImpostosProdutosNaoAgrega", Class = "col s12 m6", Label = "Total de impostos não incidentes", Readonly = true });
            config.Elements.Add(new InputCurrencyUI { Id = "totalNotaFiscalNFe", Class = "col s12", Label = "Total (produtos + impostos incidentes + frete)", Readonly = true, Name = "totalNotaFiscal" });

            config.Elements.Add(new AutoCompleteUI
            {
                Id = "serieNotaFiscalIdNFe",
                Class = "col s12 m6",
                Label = "Série",
                Required = true,
                Name = "serieNotaFiscalId",
                DataUrl = Url.Action("SerieNFe", "AutoComplete"),
                LabelId = "serieNotaFiscalSerieNFe",
                LabelName = "serieNotaFiscalSerie",
                DomEvents = new List<DomEventUI>
                {
                    new DomEventUI { DomEvent = "autocompleteselect", Function = "fnChangeSerieNFe" }
                },
                DataUrlPostModal = Url.Action("FormModalNFe", "SerieNotaFiscal"),
                DataPostField = "serie"
            });

            config.Elements.Add(new InputNumbersUI { Id = "numNotaFiscalNFe", Class = "col s12 m6", Label = "Número Nota Fiscal", Required = true, MinLength = 1, MaxLength = 9, Name = "numNotaFiscal" });

            return Content(JsonConvert.SerializeObject(config, JsonSerializerSetting.Front), "application/json");
        }

        [OperationRole(PermissionValue = EPermissionValue.Write)]
        [HttpPost]
        public JsonResult Transmitir(NFeEntradaVM entity)
        {
            try
            {
                dynamic NFe = new
                {
                    status = "Transmitida",
                    serieNotaFiscalId = entity.SerieNotaFiscalId,
                    numNotaFiscal = entity.NumNotaFiscal
                };

                var resourceNamePut = $"{"NFeEntrada"}/{entity.Id}";
                RestHelper.ExecutePutRequest(resourceNamePut, JsonConvert.SerializeObject(NFe));

                return JsonResponseStatus.GetSuccess("NF-e transmitida com sucesso");
            }
            catch (Exception ex)
            {
                var error = JsonConvert.DeserializeObject<ErrorInfo>(ex.Message);
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

                string fileName = "NFe" + response.Value<string>("numNotaFiscal") + ".xml";
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

                string fileName = "NFe" + response.Value<string>("numNotaFiscal") + ".pdf";
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

        protected override List<JQueryDataTableParamsColumn> GetParamsColumns(string ResourceName = "")
        {
            throw new NotImplementedException();
        }
    }
}