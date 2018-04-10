using Fly01.Faturamento.Controllers.Base;
using Fly01.Faturamento.Entities.ViewModel;
using Fly01.uiJS.Classes;
using Fly01.uiJS.Classes.Elements;
using Fly01.uiJS.Defaults;
using Fly01.Core.Helpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Fly01.Core.Presentation.Commons;
using Fly01.Core.Rest;

namespace Fly01.Faturamento.Controllers
{
    public class NFeController : BaseController<NFeVM>
    {
        //NFeVM e NFSeVM na mesma controller notaFiscal, direcionado as controller via javaScript
        public NFeController()
        {
            ExpandProperties = "ordemVendaOrigem($select=numero),cliente($select=id,nome),transportadora($select=id,nome),estadoPlacaVeiculo,condicaoParcelamento,formaPagamento,categoria,serieNotaFiscal";
        }

        public override Func<NFeVM, object> GetDisplayData()
        {
            throw new NotImplementedException();
        }

        public override ContentResult Form()
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
            config.Elements.Add(new InputNumbersUI { Id = "ordemVendaOrigemNumero", Class = "col s12 m6", Label = "Pedido Origem", Disabled = true });
            config.Elements.Add(new InputNumbersUI { Id = "numero", Class = "col s12 m6", Label = "Número sistema", Disabled = true });
            config.Elements.Add(new SelectUI
            {
                Id = "tipoVenda",
                Class = "col s12 m4",
                Label = "Tipo Venda",
                Disabled = true,
                Options = new List<SelectOptionUI>(SystemValueHelper.GetUIElementBase("TipoVenda", true, false))
            });
            config.Elements.Add(new SelectUI
            {
                Id = "status",
                Class = "col s12 m8",
                Label = "Status",
                Disabled = true,
                Options = new List<SelectOptionUI>(SystemValueHelper.GetUIElementBase("StatusNotaFiscal", true, false))
            });
            config.Elements.Add(new AutocompleteUI
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
            config.Elements.Add(new AutocompleteUI
            {
                Id = "clienteId",
                Class = "col s12 m8",
                Label = "Cliente",
                Disabled = true,
                DataUrl = Url.Action("Cliente", "AutoComplete"),
                LabelId = "clienteNome"
            });
            config.Elements.Add(new TextareaUI { Id = "observacao", Class = "col s12", Label = "Observação", Disabled = true, MaxLength = 200 });
            config.Elements.Add(new InputTextUI { Id = "naturezaOperacao", Class = "col s12", Label = "Natureza de Operação", Disabled = true });
            config.Elements.Add(new InputTextUI { Id = "sefazId", Class = "col s12", Label = "Sefaz chave de acesso", Disabled = true });

            config.Elements.Add(new AutocompleteUI
            {
                Id = "formaPagamentoId",
                Class = "col s12 m6",
                Label = "Forma Pagamento",
                Disabled = true,
                DataUrl = Url.Action("FormaPagamento", "AutoComplete"),
                LabelId = "formaPagamentoDescricao"
            });
            config.Elements.Add(new AutocompleteUI
            {
                Id = "condicaoParcelamentoId",
                Class = "col s12 m6",
                Label = "Condição Parcelamento",
                Disabled = true,
                DataUrl = Url.Action("CondicaoParcelamento", "AutoComplete"),
                LabelId = "condicaoParcelamentoDescricao"
            });
            config.Elements.Add(new AutocompleteUI
            {
                Id = "categoriaId",
                Class = "col s12 m6",
                Label = "Categoria",
                Disabled = true,
                DataUrl = @Url.Action("Categoria", "AutoComplete"),
                LabelId = "categoriaDescricao",
            });
            config.Elements.Add(new InputDateUI { Id = "dataVencimento", Class = "col s12 m6", Label = "Data Vencimento", Disabled = true });
            config.Elements.Add(new AutocompleteUI
            {
                Id = "transportadoraId",
                Class = "col s12",
                Label = "Transportadora",
                Disabled = true,
                DataUrl = Url.Action("Transportadora", "AutoComplete"),
                LabelId = "transportadoraNome"
            });
            config.Elements.Add(new SelectUI
            {
                Id = "tipoFrete",
                Class = "col s12 m5",
                Label = "Tipo Frete",
                Disabled = true,
                Options = new List<SelectOptionUI>(SystemValueHelper.GetUIElementBase("TipoFrete", true, false)),
            });
            config.Elements.Add(new InputCustommaskUI
            {
                Id = "placaVeiculo",
                Class = "col s12 m2",
                Label = "Placa Veículo",
                Disabled = true,
                Data = new { inputmask = "'mask':'AAA-9999', 'showMaskOnHover': false, 'autoUnmask':true" }
            });
            config.Elements.Add(new AutocompleteUI
            {
                Id = "estadoPlacaVeiculoId",
                Class = "col s12 m5",
                Label = "UF Placa Veículo",
                Disabled = true,
                DataUrl = Url.Action("Estado", "AutoComplete"),
                LabelId = "estadoPlacaVeiculoNome"
            });
            config.Elements.Add(new InputCurrencyUI { Id = "valorFrete", Class = "col s12 m6", Label = "Valor Frete", Disabled = true });
            config.Elements.Add(new InputFloatUI { Id = "pesoBruto", Class = "col s12 m6", Label = "Peso Bruto", Disabled = true });
            config.Elements.Add(new InputFloatUI { Id = "pesoLiquido", Class = "col s12 m6", Label = "Peso Líquido", Disabled = true });
            config.Elements.Add(new InputNumbersUI { Id = "quantidadeVolumes", Class = "col s12 m6", Label = "Quantidade Volumes", Disabled = true });

            config.Elements.Add(new LabelsetUI { Id = "labelSetTotais", Class = "col s12", Label = "Totais" });
            config.Elements.Add(new InputCurrencyUI { Id = "totalProdutos", Class = "col s12 m6", Label = "Total produtos", Readonly = true });
            config.Elements.Add(new InputCurrencyUI { Id = "totalImpostosProdutos", Class = "col s12 m6", Label = "Total impostos produtos", Readonly = true });
            config.Elements.Add(new InputCurrencyUI { Id = "totalFrete", Class = "col s12 m6", Label = "Frete fornecedor paga (CIF/Remetente)", Readonly = true });
            config.Elements.Add(new InputCurrencyUI { Id = "totalNotaFiscal", Class = "col s12 m6", Label = "Total (produtos + impostos + frete)", Readonly = true });

            config.Elements.Add(new LabelsetUI { Id = "labelSetProdutos", Class = "col s12", Label = "Produtos" });
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
            config.Elements.Add(new LabelsetUI { Id = "labelSetTransmissao", Class = "col s12", Label = "Transmissão" });
            config.Elements.Add(new TextareaUI { Id = "mensagem", Class = "col s12", Label = "Mensagem", Disabled = true });
            config.Elements.Add(new TextareaUI { Id = "recomendacao", Class = "col s12", Label = "Recomendação", Disabled = true });

            return Content(JsonConvert.SerializeObject(config, JsonSerializerSetting.Front), "application/json");
        }

        public override ContentResult List()
        {
            throw new NotImplementedException();
        }

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
                    List = @Url.Action("List", "NotaFiscal")
                },
                Id = "fly01mdlfrmTransmitirNFe",
                ReadyFn = "fnFormReadyTransmitirNFe",
            };

            config.Elements.Add(new InputHiddenUI { Id = "idNFe", Name = "id" });
            config.Elements.Add(new SelectUI
            {
                Id = "tipoVendaNFe",
                Class = "col s12 m6",
                Label = "Tipo Venda",
                Disabled = true,
                Name = "tipoVenda",
                Options = new List<SelectOptionUI>(SystemValueHelper.GetUIElementBase("TipoVenda", true, false))
            });
            config.Elements.Add(new InputDateUI { Id = "dataNFe", Class = "col s12 m6", Label = "Data", Disabled = true, Name = "data" });
            config.Elements.Add(new AutocompleteUI
            {
                Id = "clienteIdNFe",
                Class = "col s12",
                Label = "Cliente",
                Disabled = true,
                Name = "clienteId",
                DataUrl = Url.Action("Cliente", "AutoComplete"),
                LabelId = "clienteNomeNFe",
                LabelName = "clienteNome"
            });
            config.Elements.Add(new InputHiddenUI { Id = "tipoFreteNFe", Name = "tipoFrete" });
            config.Elements.Add(new InputHiddenUI { Id = "valorFreteNFe", Name = "valorFrete" });
            config.Elements.Add(new InputCurrencyUI { Id = "totalProdutos", Class = "col s12 m6", Label = "Total produtos", Readonly = true });
            config.Elements.Add(new InputCurrencyUI { Id = "totalImpostosProdutos", Class = "col s12 m6", Label = "Total impostos produtos", Readonly = true });
            config.Elements.Add(new InputCurrencyUI { Id = "totalFreteNFe", Class = "col s12 m6", Label = "Frete fornecedor paga (CIF/Remetente)", Readonly = true, Name = "totalFrete" });
            config.Elements.Add(new InputCurrencyUI { Id = "totalNotaFiscalNFe", Class = "col s12 m6", Label = "Total (produtos + impostos + frete)", Readonly = true, Name = "totalNotaFiscal" });

            config.Elements.Add(new AutocompleteUI
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
                DataUrlPostModal = Url.Action("FormModal", "SerieNotaFiscal"),
                DataPostField = "serie"
            });
            config.Elements.Add(new InputNumbersUI { Id = "numNotaFiscalNFe", Class = "col s12 m6", Label = "Número Nota Fiscal", Required = true, MinLength = 1, MaxLength = 9, Name = "numNotaFiscal" });


            return Content(JsonConvert.SerializeObject(config, JsonSerializerSetting.Front), "application/json");
        }

        [HttpPost]
        public JsonResult Transmitir(NFeVM entity)
        {
            try
            {
                dynamic NFe = new
                {
                    status = "Transmitida",
                    serieNotaFiscalId = entity.SerieNotaFiscalId,
                    numNotaFiscal = entity.NumNotaFiscal
                };

                var resourceNamePut = $"{"NFe"}/{entity.Id}";
                RestHelper.ExecutePutRequest(resourceNamePut, JsonConvert.SerializeObject(NFe));

                return JsonResponseStatus.GetSuccess("NF-e transmitida com sucesso");
            }
            catch (Exception ex)
            {
                var error = JsonConvert.DeserializeObject<ErrorInfo>(ex.Message);
                return JsonResponseStatus.GetFailure(error.Message);
            }
        }

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
    }
}