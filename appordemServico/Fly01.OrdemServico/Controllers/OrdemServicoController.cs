using Fly01.Core.Defaults;
using Fly01.Core.Presentation;
using Fly01.Faturamento.ViewModel;
using Fly01.uiJS.Classes;
using Fly01.uiJS.Classes.Elements;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Fly01.OrdemServico.Controllers
{

    public class OrdemServicoController : BaseController<OrdemServicoVM>
    {
        public OrdemServicoController()
        {

        }

        public override Func<OrdemServicoVM, object> GetDisplayData()
        {
            throw new NotImplementedException();
        }

        public override ContentResult List()
            => ListOrdemServico();

        public List<HtmlUIButton> GetListButtonsOnHeaderCustom(string buttonLabel, string buttonOnClick)
        {
            var target = new List<HtmlUIButton>();

            if (UserCanWrite)
            {
                target.Add(new HtmlUIButton { Id = "new", Label = "Nova Ordem de Serviço", OnClickFn = "fnNovaOrdemServico" });
                target.Add(new HtmlUIButton { Id = "newNFInutilizada", Label = "Imprimir", OnClickFn = "fnImprimirOS" });
            }

            return target;
        }

        public ContentResult ListOrdemServico(string gridLoad = "GridLoad")
        {
            var buttonLabel = "Mostrar todas as notas";
            var buttonOnClick = "fnRemoveFilter";

            if (Request.QueryString["action"] == "GridLoadNoFilter")
            {
                gridLoad = Request.QueryString["action"];
                buttonLabel = "Mostrar notas do mês atual";
                buttonOnClick = "fnAddFilter";
            }

            var cfg = new ContentUI
            {
                History = new ContentUIHistory { Default = Url.Action("Index") },
                Header = new HtmlUIHeader
                {
                    Title = "Notas Fiscais",
                    Buttons = new List<HtmlUIButton>(GetListButtonsOnHeaderCustom(buttonLabel, buttonOnClick))
                },
                UrlFunctions = Url.Action("Functions") + "?fns="
            };

            if (gridLoad == "GridLoad")
            {
                var cfgForm = new FormUI
                {
                    ReadyFn = "fnUpdateDataFinal",
                    UrlFunctions = Url.Action("Functions") + "?fns=",
                    Elements = new List<BaseUI>()
                    {
                        new PeriodPickerUI
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
                        },
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

                cfg.Content.Add(cfgForm);
            }

            var config = new DataTableUI
            {
                UrlGridLoad = Url.Action(gridLoad),
                Parameters = new List<DataTableUIParameter>
                {
                    new DataTableUIParameter() {Id = "dataInicial", Required = (gridLoad == "GridLoad") },
                    new DataTableUIParameter() {Id = "dataFinal", Required = (gridLoad == "GridLoad") }
                },
                UrlFunctions = Url.Action("Functions") + "?fns=",
                Functions = new List<string>() { "fnRenderEnum" }
            };

            config.Actions.AddRange(GetActionsInGrid(new List<DataTableUIAction>()
            {
                new DataTableUIAction { OnClickFn = "fnVisualizarNFe", Label = "Visualizar", ShowIf = "(row.tipoNotaFiscal == 'NFe')" },
                new DataTableUIAction { OnClickFn = "fnVisualizarRetornoSefaz", Label = "Mensagem SEFAZ", ShowIf = "(row.status != 'NaoTransmitida' && row.tipoNotaFiscal == 'NFe')" },
                new DataTableUIAction { OnClickFn = "fnVisualizarNFSe", Label = "Visualizar", ShowIf = "(row.tipoNotaFiscal == 'NFSe')" },
                new DataTableUIAction { OnClickFn = "fnTransmitirNFe", Label = "Transmitir", ShowIf = "((row.status == 'NaoAutorizada' || row.status == 'NaoTransmitida' || row.status == 'FalhaTransmissao') && row.tipoNotaFiscal == 'NFe')" },
                new DataTableUIAction { OnClickFn = "fnTransmitirNFSe", Label = "Transmitir", ShowIf = "((row.status == 'NaoAutorizada' || row.status == 'NaoTransmitida' || row.status == 'FalhaTransmissao') && row.tipoNotaFiscal == 'NFSe')" },
                new DataTableUIAction { OnClickFn = "fnExcluirNFe", Label = "Excluir", ShowIf = "((row.status == 'NaoAutorizada' || row.status == 'NaoTransmitida' || row.status == 'FalhaTransmissao') && row.tipoNotaFiscal == 'NFe')" },
                new DataTableUIAction { OnClickFn = "fnExcluirNFSe", Label = "Excluir", ShowIf = "((row.status == 'NaoAutorizada' || row.status == 'NaoTransmitida' || row.status == 'FalhaTransmissao') && row.tipoNotaFiscal == 'NFSe')" },
                new DataTableUIAction { OnClickFn = "fnBaixarXMLNFe", Label = "Baixar XML", ShowIf = "((row.status == 'NaoAutorizada' || row.status == 'Autorizada') && row.tipoNotaFiscal == 'NFe')" },
                new DataTableUIAction { OnClickFn = "fnBaixarPDFNFe", Label = "Baixar PDF", ShowIf = "(row.status == 'Autorizada' && row.tipoNotaFiscal == 'NFe')" },
                new DataTableUIAction { OnClickFn = "fnBaixarXMLNFSe", Label = "Baixar XML", ShowIf = "((row.status == 'NaoAutorizada' || row.status == 'Autorizada') && row.tipoNotaFiscal == 'NFSe')" },
                new DataTableUIAction { OnClickFn = "fnBaixarPDFNFSe", Label = "Baixar PDF", ShowIf = "(row.status == 'Autorizada' && row.tipoNotaFiscal == 'NFSe')" },
                new DataTableUIAction { OnClickFn = "fnCancelarNFe", Label = "Cancelar", ShowIf = "((row.status == 'Autorizada' || row.status == 'FalhaNoCancelamento') && row.tipoNotaFiscal == 'NFe')" },
                new DataTableUIAction { OnClickFn = "fnCancelarNFSe", Label = "Cancelar", ShowIf = "((row.status == 'Autorizada' || row.status == 'FalhaNoCancelamento') && row.tipoNotaFiscal == 'NFSe')" },
                new DataTableUIAction { OnClickFn = "fnFormCartaCorrecao", Label = "Carta de Correção", ShowIf = "(row.status == 'Autorizada')" }
            }));

            config.Columns.Add(new DataTableUIColumn { DataField = "serieNotaFiscal_serie", DisplayName = "Série", Priority = 1 });
            config.Columns.Add(new DataTableUIColumn { DataField = "numNotaFiscal", DisplayName = "Número NF", Priority = 2, Type = "numbers" });
            //config.Columns.Add(new DataTableUIColumn
            //{
            //    DataField = "status",
            //    DisplayName = "Status",
            //    Priority = 3,
            //    Options = new List<SelectOptionUI>(SystemValueHelper.GetUIElementBase(typeof(StatusNotaFiscal))),
            //    RenderFn = "fnRenderEnum(full.statusCssClass, full.statusDescription)"
            //});
            //config.Columns.Add(new DataTableUIColumn
            //{
            //    DataField = "tipoNotaFiscal",
            //    DisplayName = "Tipo",
            //    Priority = 4,
            //    Options = new List<SelectOptionUI>(SystemValueHelper.GetUIElementBase(typeof(TipoNotaFiscal))),
            //    RenderFn = "fnRenderEnum(full.tipoNotaFiscalCssClass, full.tipoNotaFiscalDescription)"
            //});
            //config.Columns.Add(new DataTableUIColumn
            //{
            //    DataField = "tipoVenda",
            //    DisplayName = "Finalidade",
            //    Priority = 5,
            //    Options = new List<SelectOptionUI>(SystemValueHelper.GetUIElementBase(typeof(TipoVenda))),
            //    RenderFn = "fnRenderEnum(full.tipoVendaCssClass, full.tipoVendaDescription)"
            //});
            //config.Columns.Add(new DataTableUIColumn { DataField = "fornecedor_nome", DisplayName = "Fornecedor", Priority = 6 });
            //config.Columns.Add(new DataTableUIColumn { DataField = "data", DisplayName = "Data", Priority = 7, Type = "date" });
            //config.Columns.Add(new DataTableUIColumn { DataField = "ordemVendaOrigem_numero", DisplayName = "Pedido Origem", Searchable = false, Priority = 8 });//numero int e pesquisa string
            //config.Columns.Add(new DataTableUIColumn { DataField = "categoria_descricao", DisplayName = "Categoria", Priority = 9 });

            cfg.Content.Add(config);

            return Content(JsonConvert.SerializeObject(cfg, JsonSerializerSetting.Front), "application/json");
        }

        protected override ContentUI FormJson()
        {
            throw new NotImplementedException();
        }
    }

}