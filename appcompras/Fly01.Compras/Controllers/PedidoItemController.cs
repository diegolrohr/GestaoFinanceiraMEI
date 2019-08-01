using Fly01.Compras.ViewModel;
using Fly01.Core;
using Fly01.uiJS.Classes;
using Fly01.uiJS.Classes.Elements;
using Fly01.uiJS.Defaults;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Fly01.Core.Presentation;
using Fly01.Core.Presentation.Commons;
using Fly01.uiJS.Classes.Helpers;
using Fly01.Core.Presentation.JQueryDataTable;

namespace Fly01.Compras.Controllers
{
    [OperationRole(ResourceKey = ResourceHashConst.ComprasComprasOrcamentoPedido)]
    public class PedidoItemController : BaseController<PedidoItemVM>
    {
        public PedidoItemController()
        {
            ExpandProperties = "produto($select=id,descricao),grupoTributario($select=id,descricao,tipoTributacaoICMS)";
        }

        public override Func<PedidoItemVM, object> GetDisplayData()
        {
            return x => new
            {
                id = x.Id.ToString(),
                produto_descricao = x.Produto.Descricao,
                grupoTributario_descricao = x.GrupoTributario != null ? x.GrupoTributario.Descricao : "",
                quantidade = x.Quantidade.ToString("R", AppDefaults.CultureInfoDefault),
                valor = x.Valor.ToString("C", AppDefaults.CultureInfoDefault),
                desconto = x.Desconto.ToString("C", AppDefaults.CultureInfoDefault),
                total = x.Total.ToString("C", AppDefaults.CultureInfoDefault),
            };      
                
        }

        public ContentResult Modal()
        {
            ModalUIForm config = new ModalUIForm()
            {
                Title = "Produto",
                UrlFunctions = @Url.Action("Functions") + "?fns=",
                ConfirmAction = new ModalUIAction() { Label = "Salvar" },
                CancelAction = new ModalUIAction() { Label = "Cancelar" },
                Action = new FormUIAction
                {
                    Create = @Url.Action("Create"),
                    Edit = @Url.Action("Edit"),
                    Get = @Url.Action("Json") + "/",
                    List = @Url.Action("List")
                },
                Id = "fly01mdlfrmPedidoItem",
                ReadyFn = "fnFormReadyPedidoItem",
                Functions = new List<string>() { "fnChangeTotalProduto" }
            };

            config.Elements.Add(new InputHiddenUI { Id = "id" });
            config.Elements.Add(new InputHiddenUI { Id = "pedidoId" });
            config.Elements.Add(new InputHiddenUI { Id = "grupoTributarioTipoTributacaoICMS" });

            config.Elements.Add(ElementUIHelper.GetAutoComplete(new AutoCompleteUI
            {
                Id = "produtoId",
                Class = "col s12 m6",
                Label = "Produto",
                Required = true,
                DataUrl = Url.Action("ProdutoOrdem", "AutoComplete"),
                DataUrlPostModal = Url.Action("FormModal", "Produto"),
                DataPostField = "descricao",
                LabelId = "produtoDescricao",
                DomEvents = new List<DomEventUI> { new DomEventUI { DomEvent = "autocompleteselect", Function = "fnChangeProduto" } }
            }, ResourceHashConst.ComprasCadastrosProdutos));

            config.Elements.Add(ElementUIHelper.GetAutoComplete(new AutoCompleteUI
            {
                Id = "grupoTributarioIdProduto",
                Class = "col s12 m6",
                Label = "Grupo Tributário",
                Name = "grupoTributarioId",
                DataUrl = Url.Action("GrupoTributario", "AutoComplete"),
                LabelId = "grupoTributarioDescricaoProduto",
                LabelName = "grupoTributarioDescricao",
                DataUrlPostModal = Url.Action("FormModal", "GrupoTributario"),
                DataPostField = "descricao",
                DomEvents = new List<DomEventUI> { new DomEventUI { DomEvent = "autocompleteselect", Function = "fnChangeGrupoTribProduto" } }
            }, ResourceHashConst.ComprasCadastrosGrupoTributario));

            config.Elements.Add(new InputFloatUI
            {
                Id = "quantidade",
                Class = "col s12 l6 numeric",
                Label = "Quantidade",
                Value = "1",
                Required = true,
                Digits = 3,
                DomEvents = new List<DomEventUI>()
                {
                    new DomEventUI() {DomEvent = "change", Function = "fnChangeTotal" }
                }
            });

            config.Elements.Add(new InputCurrencyUI
            {
                Id = "valor",
                Class = "col s12 l6 numeric",
                Label = "Valor",
                Value = "0",
                Required = false
            });

            config.Elements.Add(new InputCurrencyUI
            {
                Id = "desconto",
                Class = "col s12 l6",
                Label = "Desconto",
                Value = "0"
            });

            config.Elements.Add(new InputCurrencyUI { Id = "total", Class = "col s12 l6", Label = "Total", Disabled = true, Required = true });

            config.Elements.Add(new InputCurrencyUI
            {
                Id = "valorCreditoICMS",
                Class = "col s12 l6 numeric",
                Label = "Crédito ICMS",
                Value = "0"
            });

            config.Elements.Add(new InputCurrencyUI
            {
                Id = "valorBCSTRetido",
                Class = "col s12 l6 numeric",
                Label = "Base Cálculo ST Retido",
                Value = "0"
            });

            config.Elements.Add(new InputCurrencyUI
            {
                Id = "valorICMSSTRetido",
                Class = "col s12 l6 numeric",
                Label = "ICMS ST Retido",
                Value = "0"
            });

            config.Elements.Add(new InputCurrencyUI
            {
                Id = "valorBCFCPSTRetidoAnterior",
                Class = "col s12 l6",
                Label = "Base FCP ST Retido Anterior",
                Value = "0",
            });

            config.Elements.Add(new InputCurrencyUI
            {
                Id = "valorFCPSTRetidoAnterior",
                Class = "col s12 l6 numeric",
                Label = "Valor FCP ST Retido Anterior",
                Value = "0"
            });

            config.Elements.Add(new InputCustommaskUI
            {
                Id = "percentualReducaoBC",
                Class = "col s12 l6 numeric",
                Label = "Percentual Redução Base Cálculo",
                Value = "0",
                Data = new { inputmask = "'mask': '9{1,3}[,9{1,2}] %', 'alias': 'decimal', 'autoUnmask': true, 'suffix': ' %', 'radixPoint': ',' " }
            });

            config.Elements.Add(new InputCustommaskUI
            {
                Id = "percentualReducaoBCST",
                Class = "col s12 l6 numeric",
                Label = "Percentual Redução Base Cálculo ST.",
                Value = "0",
                Data = new { inputmask = "'mask': '9{1,3}[,9{1,2}] %', 'alias': 'decimal', 'autoUnmask': true, 'suffix': ' %', 'radixPoint': ',' " }
            });

            #region Helpers 
            config.Helpers.Add(new TooltipUI
            {
                Id = "percentualReducaoBCST",
                Tooltip = new HelperUITooltip()
                {
                    Text = "Percentual da Redução da Base de Cálculo ST(Substituição Tributária). Se o pedido gerar nota fiscal com CST 70, conforme cadastro do Grupo Tributário, este dado pode ser informado."
                }
            });
            config.Helpers.Add(new TooltipUI
            {
                Id = "percentualReducaoBC",
                Tooltip = new HelperUITooltip()
                {
                    Text = "Percentual da Redução da Base de Cálculo. Se o pedido gerar nota fiscal com CST 20 ou 70, conforme cadastro do Grupo Tributário, este dado deve ser informado."
                }
            });
            config.Helpers.Add(new TooltipUI
            {
                Id = "icms",
                Tooltip = new HelperUITooltip()
                {
                    Text = "Ao adicionar o produto, a alíquota será carregada dos parâmetros tributários, se necessário altere a alíquota por produto. Os impostos são calculados de acordo com as configurações do grupo Tributário."
                }
            });
            config.Helpers.Add(new TooltipUI
            {
                Id = "fcp",
                Tooltip = new HelperUITooltip()
                {
                    Text = "Ao adicionar o produto, a alíquota será carregada dos parâmetros tributários, se necessário altere a alíquota por produto. Os impostos são calculados de acordo com as configurações do grupo Tributário."
                }
            });
            config.Helpers.Add(new TooltipUI
            {
                Id = "valorCreditoICMS",
                Tooltip = new HelperUITooltip()
                {
                    Text = "Valor de Crédito do ICMS. Se o pedido gerar nota fiscal com CSOSN 101, 201 ou 900, conforme cadastro do Grupo Tributário, este dado deve ser informado. Alíquota calculada pelo valor bruto versus o crédito informado." +
                    " Os valores informados servirão de base para as informações complementares no xml, de acordo ao ARTIGO 23 DA LC 123."
                }
            });
            config.Helpers.Add(new TooltipUI
            {
                Id = "valorICMSSTRetido",
                Tooltip = new HelperUITooltip()
                {
                    Text = "Valor de Substituição Tributária retida. Se o pedido gerar nota fiscal com CSOSN 500, conforme cadastro do Grupo Tributário, este dado deve ser informado."
                }
            });
            config.Helpers.Add(new TooltipUI
            {
                Id = "valorBCSTRetido",
                Tooltip = new HelperUITooltip()
                {
                    Text = "Base de cálculo de Substituição Tributária retida. Se o pedido gerar nota fiscal com CSOSN 500, conforme cadastro do Grupo Tributário, este dado deve ser informado."
                }
            });
            config.Helpers.Add(new TooltipUI
            {
                Id = "valorBCFCPSTRetidoAnterior",
                Tooltip = new HelperUITooltip()
                {
                    Text = "Valor Base do Fundo de Combate à Pobreza retido anteriormente. O percentual será calculado conforme base e valor informados. Se o pedido gerar nota fiscal com CSOSN 500, conforme cadastro do Grupo Tributário, este dado deve ser informado."
                }
            });
            config.Helpers.Add(new TooltipUI
            {
                Id = "valorFCPSTRetidoAnterior",
                Tooltip = new HelperUITooltip()
                {
                    Text = "Valor do Fundo de Combate à Pobreza retido anteriormente. O percentual será calculado conforme base e valor informados. Se o pedido gerar nota fiscal com CSOSN 500, conforme cadastro do Grupo Tributário, este dado deve ser informado."
                }
            });
            #endregion

            return Content(JsonConvert.SerializeObject(config, JsonSerializerSetting.Front), "application/json");
        }

        public override ContentResult List()
        {
            throw new NotImplementedException();
        }

        [OperationRole(NotApply = true)]
        public JsonResult GetOrdemCompraProdutos(string id)
        {
            Dictionary<string, string> filters = new Dictionary<string, string>
            {
                { "pedidoId eq", string.IsNullOrEmpty(id) ? Guid.NewGuid().ToString() : id }
            };
            return GridLoad(filters);
        }

        protected override ContentUI FormJson()
        {
            throw new NotImplementedException();
        }

        protected override List<JQueryDataTableParamsColumn> GetParamsColumns()
        {
            throw new NotImplementedException();
        }
    }
}