using Fly01.Compras.ViewModel;
using Fly01.Core;
using Fly01.Core.Defaults;
using Fly01.Core.Entities.Domains.Enum;
using Fly01.Core.Helpers;
using Fly01.Core.Presentation;
using Fly01.Core.Presentation.Commons;
using Fly01.Core.Presentation.JQueryDataTable;
using Fly01.Core.Rest;
using Fly01.Core.ViewModels;
using Fly01.uiJS.Classes;
using Fly01.uiJS.Classes.Elements;
using Fly01.uiJS.Classes.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Web.Mvc;

namespace Fly01.Compras.Controllers
{
    [OperationRole(ResourceKey = ResourceHashConst.ComprasComprasNFeImportacao)]
    public class NFeImportacaoProdutoController : BaseController<NFeImportacaoProdutoVM>
    {
        public NFeImportacaoProdutoController()
        {
            ExpandProperties = "produto($expand=unidadeMedida),unidadeMedida";
        }

        public override Func<NFeImportacaoProdutoVM, object> GetDisplayData()
        {
            return x => new
            {
                id = x.Id,
                nfeImportacaoId = x.NFeImportacaoId,
                novoProduto = x.NovoProduto,
                produtoId = x.ProdutoId,
                codigo = x.Codigo,
                codigoBarras = x.CodigoBarras,
                descricao = x.Descricao.Substring(0, x.Descricao.Length > 30 ? 30 : x.Descricao.Length),
                quantidade = x.Quantidade.ToString("R", AppDefaults.CultureInfoDefault),
                quantidadeCurrency = x.Quantidade,
                valor = x.Valor.ToString("C", AppDefaults.CultureInfoDefault),
                valorCurrency = x.Valor,
                unidadeMedida_abreviacao = x.UnidadeMedida.Abreviacao,
                produto_unidadeMedida_abreviacao = x.Produto?.UnidadeMedida.Abreviacao ?? "",
                produto_descricao = x.Produto?.Descricao ?? "",                
                fatorConversao = x.FatorConversao.ToString("R", AppDefaults.CultureInfoDefault),
                fatorConversaoCurrency = x.FatorConversao,
                tipoFatorConversao = x.TipoFatorConversao,
                tipoFatorConversaoDescription = EnumHelper.GetDescription(typeof(TipoFatorConversao), x.TipoFatorConversao),
                tipoFatorConversaoCssClass = EnumHelper.GetCSS(typeof(TipoFatorConversao), x.TipoFatorConversao),
                tipoFatorConversaoValue = EnumHelper.GetValue(typeof(TipoFatorConversao), x.TipoFatorConversao),
                movimentaEstoque = x.MovimentaEstoque,
                atualizaDadosProduto = x.AtualizaDadosProduto,
                atualizaValorVenda = x.AtualizaValorVenda,
                valorVenda = x.ValorVenda.ToString("C", AppDefaults.CultureInfoDefault),
                valorVendaCurrency = x.ValorVenda
            };
        }

        public override ContentResult List()
        {
            throw new NotImplementedException();
        }

        protected override ContentUI FormJson()
        {
            throw new NotImplementedException();
        }

        public JsonResult GetNFeImportacaoProdutos(string id)
        {
            Dictionary<string, string> filters = new Dictionary<string, string>
            {
                { "nFeImportacaoId eq", string.IsNullOrEmpty(id) ? Guid.NewGuid().ToString() : id }
            };
            return GridLoad(filters);
        }

        public JsonResult GetNFeImportacaoProdutosPendencia(string id)
        {
            Dictionary<string, string> filters = new Dictionary<string, string>
            {
                { "(nFeImportacaoId eq", (string.IsNullOrEmpty(id) ? Guid.NewGuid().ToString() : id)+")" },
                { " and (fatorConversao eq", "0)" },
                { " and ((produtoId eq", "null)" },
                { " or (produto ne null and produto/unidadeMedida/abreviacao ne", "unidadeMedida/abreviacao))" },
            };
            return GridLoad(filters);
        }

        [HttpGet]
        public ContentResult ModalFatorConversao(string id, string unidadeXml, string unidadeCadastro, double valor, double quantidade, double fatorConversao, string tipoFatorConversao)
        {
            ModalUIForm config = new ModalUIForm()
            {
                Title = "Fator de Conversão",
                UrlFunctions = @Url.Action("Functions") + "?fns=",
                ConfirmAction = new ModalUIAction() { Label = "Salvar", OnClickFn = "fnSalvarModalFatorConversao" },
                CancelAction = new ModalUIAction() { Label = "Cancelar", OnClickFn = "fnCancelarModalFatorConversao" },
                Action = new FormUIAction
                {
                    Create = @Url.Action("SalvarProdutoPendencia"),
                    Edit = @Url.Action("SalvarProdutoPendencia"),
                    Get = @Url.Action("Json") + "/",
                    List = @Url.Action("List", "NFeImportacao")
                },
                Id = "fly01mdlfrmFatorConversao",
                ReadyFn = "fnFormReadyFatorConversao",
                Functions = new List<string>() { "fnChangeTipoFatorConversao" }
            };

            config.Elements.Add(new InputHiddenUI { Id = "id", Value = id });
            config.Elements.Add(new InputHiddenUI { Id = "tipoFatorConversao", Value = tipoFatorConversao });
            config.Elements.Add(new ButtonGroupUI()
            {
                Id = "fly01btngrpTipoFatorConversao",
                Class = "col s12 m12",
                OnClickFn = "fnChangeTipoFatorConversao",
                Label = "Tipo do fator de conversão",
                Options = new List<ButtonGroupOptionUI>
                {
                    new ButtonGroupOptionUI { Id = "btnMultiplicar", Value = "Multiplicar", Label = "Multiplicar (X)"},
                    new ButtonGroupOptionUI { Id = "btnDividir", Value = "Dividir", Label = "Dividir (/)"},
                }
            });
            config.Elements.Add(new LabelSetUI { Id = "labelSetXml", Label = "Dados do xml" });
            config.Elements.Add(new InputTextUI
            {
                Id = "unidadeXml",
                Class = "col s12 l2",
                Label = "Unidade Xml",
                Value = unidadeXml,
                Disabled = true
            });

            config.Elements.Add(new InputFloatUI
            {
                Id = "quantidadeXml",
                Class = "col s12 l3 numeric",
                Label = "Quantidade",
                Digits = 2,
                Value = quantidade.ToString(),
                Disabled = true
            });

            config.Elements.Add(new InputCurrencyUI
            {
                Id = "valorXml",
                Class = "col s12 l3 numeric",
                Label = "Valor Unitário",
                Value = valor.ToString(),
                Disabled = true
            });
            config.Elements.Add(new InputCurrencyUI
            {
                Id = "valorTotalXml",
                Class = "col s12 l4 numeric",
                Label = "Valor Total",
                Value = (quantidade * valor).ToString(),
                Disabled = true
            });

            config.Elements.Add(new LabelSetUI { Id = "labelSetXml", Label = "Dados para cadastro" });
            config.Elements.Add(new InputTextUI
            {
                Id = "unidadeCadastro",
                Class = "col s12 l2",
                Label = "Unidade Cadastro",
                Value = unidadeCadastro,
                Disabled = true
            });
            config.Elements.Add(new InputFloatUI
            {
                Id = "quantidadeConvertida",
                Class = "col s12 l3 numeric",
                Label = "Quantidade Convertida",
                Digits = 2,
                Disabled = true
            });
            config.Elements.Add(new InputCurrencyUI
            {
                Id = "valorConvertido",
                Class = "col s12 l3 numeric",
                Label = "Valor Unitário Convertido",
                Disabled = true
            });
            config.Elements.Add(new InputFloatUI
            {
                Id = "fatorConversao",
                Class = "col s12 l4 numeric",
                Label = "Fator de Conversão",
                Digits = 2,
                Value = fatorConversao.ToString(),
                Required = true,
                DomEvents = new List<DomEventUI>()
                {
                    new DomEventUI()
                    {
                        DomEvent = "change",
                        Function = "fnChangeFatorConversao"
                    }
                }
            });

            #region Helpers 
            config.Helpers.Add(new TooltipUI
            {
                Id = "unidadeCadastro",
                Tooltip = new HelperUITooltip()
                {
                    Text = "Unidade de medida cadastrada no produto vinculado."
                }
            });
            config.Helpers.Add(new TooltipUI
            {
                Id = "fatorConversao",
                Tooltip = new HelperUITooltip()
                {
                    Text = "Fator de conversão a ser utilizado para movimentar estoque e atualizar o valor de compra/venda no cadastro do produto."
                }
            });
            config.Helpers.Add(new TooltipUI
            {
                Id = "quantidadeConvertida",
                Tooltip = new HelperUITooltip()
                {
                    Text = "Quantidade final convertida pelo fator de conversão."
                }
            });
            config.Helpers.Add(new TooltipUI
            {
                Id = "valorConvertido",
                Tooltip = new HelperUITooltip()
                {
                    Text = "Valor unitário final convertido pelo fator de conversão."
                }
            });
            #endregion

            return Content(JsonConvert.SerializeObject(config, JsonSerializerSetting.Front), "application/json");
        }

        [OperationRole(PermissionValue = EPermissionValue.Write)]
        [HttpPost]
        public virtual JsonResult SalvarProdutoPendencia(NFeImportacaoProdutoVM entityVM)
        {
            try
            {
                dynamic nfeImportacaoProduto = new ExpandoObject();
                nfeImportacaoProduto.id = entityVM.Id;
                if(entityVM.ProdutoId != null)
                    nfeImportacaoProduto.produtoId = entityVM.ProdutoId.Value;
                nfeImportacaoProduto.novoProduto = entityVM.NovoProduto;
                nfeImportacaoProduto.fatorConversao = entityVM.FatorConversao;
                nfeImportacaoProduto.tipoFatorConversao = entityVM.TipoFatorConversao;

                var resourceNamePut = $"{ResourceName}/{entityVM.Id}";
                RestHelper.ExecutePutRequest(resourceNamePut, JsonConvert.SerializeObject(nfeImportacaoProduto, JsonSerializerSetting.Edit));

                return JsonResponseStatus.Get(new ErrorInfo { HasError = false }, Operation.Edit, entityVM.Id);
            }
            catch (Exception ex)
            {
                var error = JsonConvert.DeserializeObject<ErrorInfo>(ex.Message);
                return JsonResponseStatus.GetFailure(error.Message);
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public JsonResult EditProdutoImportacao(NFeImportacaoProdutoVM entityVM)
        {
            try
            {
                dynamic nfeImportacaoProduto = new ExpandoObject();
                nfeImportacaoProduto.atualizaDadosProduto = entityVM.AtualizaDadosProduto;
                nfeImportacaoProduto.movimentaEstoque = entityVM.MovimentaEstoque;
                nfeImportacaoProduto.atualizaValorVenda = entityVM.AtualizaValorVenda;
                nfeImportacaoProduto.valorVenda = entityVM.ValorVenda;

                var resourceNamePut = $"{ResourceName}/{entityVM.Id}";
                RestHelper.ExecutePutRequest(resourceNamePut, JsonConvert.SerializeObject(nfeImportacaoProduto, JsonSerializerSetting.Edit));

                return JsonResponseStatus.Get(new ErrorInfo { HasError = false }, Operation.Edit);
            }
            catch (Exception ex)
            {
                var error = JsonConvert.DeserializeObject<ErrorInfo>(ex.Message);
                return JsonResponseStatus.GetFailure(error.Message);
            }
        }

        protected override List<JQueryDataTableParamsColumn> GetParamsColumns()
        {
            throw new NotImplementedException();
        }
    }
}
