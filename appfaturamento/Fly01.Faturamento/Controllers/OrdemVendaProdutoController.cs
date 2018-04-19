using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Fly01.Faturamento.Controllers.Base;
using Fly01.Faturamento.ViewModel;
using Fly01.Core;
using Newtonsoft.Json;
using Fly01.uiJS.Classes;
using Fly01.uiJS.Classes.Elements;
using Fly01.uiJS.Defaults;
using Fly01.uiJS.Classes.Helpers;

namespace Fly01.Faturamento.Controllers
{
    public class OrdemVendaProdutoController : BaseController<OrdemVendaProdutoVM>
    {
        public OrdemVendaProdutoController()
        {
            ExpandProperties = "produto,grupoTributario";
        }

        public override Func<OrdemVendaProdutoVM, object> GetDisplayData()
        {
            return x => new
            {
                id = x.Id.ToString(),
                produto_descricao = x.Produto.Descricao,
                grupoTributario_descricao = x.GrupoTributario.Descricao,
                quantidade = x.Quantidade.ToString("C", AppDefaults.CultureInfoDefault).Replace("R$", "").Replace("R$ ", ""),
                valor = x.Valor.ToString("C", AppDefaults.CultureInfoDefault),
                desconto = x.Desconto.ToString("C", AppDefaults.CultureInfoDefault),
                total = x.Total.ToString("C", AppDefaults.CultureInfoDefault),
            };
        }

        public override ContentResult Form()
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
                Id = "fly01mdlfrmOrdemVendaProduto",
                ReadyFn = "fnFormReadyOrdemVendaProduto",
                Functions = new List<string>() { "fnChangeTotalProduto" }
            };

            config.Elements.Add(new InputHiddenUI { Id = "id" });
            config.Elements.Add(new InputHiddenUI { Id = "ordemVendaId" });
            config.Elements.Add(new InputHiddenUI { Id = "grupoTributarioTipoTributacaoICMS" });

            config.Elements.Add(new AutocompleteUI
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
            });
            config.Elements.Add(new AutocompleteUI
            {
                Id = "grupoTributarioIdProduto",
                Class = "col s12 m6",
                Label = "Grupo Tributário",
                Required = true,
                Name = "grupoTributarioId",
                DataUrl = Url.Action("GrupoTributario", "AutoComplete"),
                LabelId = "grupoTributarioDescricaoProduto",
                LabelName = "grupoTributarioDescricao",
                DataUrlPostModal = Url.Action("FormModal", "GrupoTributario"),
                DataPostField = "descricao",
                DomEvents = new List<DomEventUI> { new DomEventUI { DomEvent = "autocompleteselect", Function = "fnChangeGrupoTribProduto" } }
            });

            config.Elements.Add(new InputFloatUI
            {
                Id = "quantidade",
                Class = "col s12 l6 numeric",
                Label = "Quantidade",
                Value = "1",
                Required = true
            });

            config.Elements.Add(new InputCurrencyUI
            {
                Id = "valor",
                Class = "col s12 l6 numeric",
                Label = "Valor",
                Value = "0",
                Required = true
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
                Id = "valorICMSSTRetido",
                Class = "col s12 l6 numeric",
                Label = "ICMS ST Retido",
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

            #region Helpers 
            config.Helpers.Add(new TooltipUI
            {
                Id = "valorCreditoICMS",
                Tooltip = new HelperUITooltip()
                {
                    Text = "Valor de Crédito do ICMS. Se o pedido gerar nota fiscal com CSOSN 101, 201 ou 900, conforme cadastro do Grupo Tributário, este dado deve ser informado."
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
                    Text = "Valor Base do Fundo de Combate à Pobreza retido anteriormente. Se o pedido gerar nota fiscal com CSOSN 500, conforme cadastro do Grupo Tributário, este dado deve ser informado."
                }
            });
            config.Helpers.Add(new TooltipUI
            {
                Id = "valorFCPSTRetidoAnterior",
                Tooltip = new HelperUITooltip()
                {
                    Text = "Valor do Fundo de Combate à Pobreza retido anteriormente. Se o pedido gerar nota fiscal com CSOSN 500, conforme cadastro do Grupo Tributário, este dado deve ser informado."
                }
            });
            #endregion

            return Content(JsonConvert.SerializeObject(config, JsonSerializerSetting.Front), "application/json");
        }

        public override ContentResult List()
        {
            throw new NotImplementedException();
        }

        public JsonResult GetOrdemVendaProdutos(string id)
        {
            Dictionary<string, string> filters = new Dictionary<string, string>
            {
                { "ordemVendaId eq", string.IsNullOrEmpty(id) ? new Guid().ToString() : id }
            };
            return GridLoad(filters);
        }
    }
}