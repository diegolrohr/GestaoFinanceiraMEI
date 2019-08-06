using Fly01.Core.Helpers;
using Fly01.Core.Presentation.Commons;
using Fly01.Core.Presentation.JQueryDataTable;
using Fly01.Core.Rest;
using Fly01.Core.ViewModels.Presentation.Commons;
using Fly01.uiJS.Classes;
using Fly01.uiJS.Classes.Elements;
using Fly01.uiJS.Classes.Helpers;
using Fly01.uiJS.Defaults;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Fly01.Core.Presentation.Controllers
{
    public class ConfiguracaoPersonalizacaoBaseController<T> : BaseController<T> where T : ConfiguracaoPersonalizacaoVM
    {
        public override Func<T, object> GetDisplayData()
        {
            throw new NotImplementedException();
        }

        public override ContentResult List()
        {
            throw new NotImplementedException();
        }

        public override ActionResult Index()
        {
            return View("Index");
        }

        protected override ContentUI FormJson()
        {
            var cfg = new ContentUIBase(Url.Action("Sidebar", "Home"))
            {
                History = new ContentUIHistory
                {
                    Default = Url.Action("Index")
                },
                Header = new HtmlUIHeader
                {
                    Title = "Personalizar Sistema",
                    Buttons = new List<HtmlUIButton>
                    {
                        new HtmlUIButton {Id = "save", Label = "Salvar", OnClickFn = "fnSalvar", Type = "submit"}
                    }
                },
                UrlFunctions = Url.Action("Functions", "ConfiguracaoPersonalizacao", null, Request.Url.Scheme) + "?fns=",
            };

            var config = new FormUI
            {
                Id = "fly01frm",
                Action = new FormUIAction
                {
                    Create = Url.Action("Edit"),
                    Edit = Url.Action("Edit"),
                    Get = Url.Action("Json") + "/",
                    List = Url.Action("Form")
                },
                ReadyFn = "fnFormReady",
                UrlFunctions = Url.Action("Functions", "ConfiguracaoPersonalizacao", null, Request.Url.Scheme) + "?fns=",
            };

            config.Elements.Add(new InputHiddenUI { Id = "id" });
            config.Elements.Add(new InputCheckboxUI { Id = "emiteNotaFiscal", Class = "col s12 m6 l4", Label = "Habilitar emissão de notas fiscais" });

            config.Elements.Add(new LabelSetUI() { Id = "lblFaturamento", Label = "Aplicativo Faturamento" });
            config.Elements.Add(new InputCheckboxUI { Id = "exibirStepProdutosVendas", Class = "col s12 m6 l4", Label = "Habilitar venda de produtos" });
            config.Elements.Add(new InputCheckboxUI { Id = "exibirStepServicosVendas", Class = "col s12 m6 l4", Label = "Habilitar prestação de serviços" });
            config.Elements.Add(new InputCheckboxUI { Id = "exibirStepTransportadoraVendas", Class = "col s12 m6 l4", Label = "Habilitar uso de transporte" });

            config.Elements.Add(new LabelSetUI() { Id = "lblCompras", Label = "Aplicativo Compras" });
            config.Elements.Add(new InputCheckboxUI { Id = "exibirStepTransportadoraCompras", Class = "col s12 m6 l4", Label = "Habilitar uso de transporte" });

            #region Helpers            
            config.Helpers.Add(new TooltipUI
            {
                Id = "emiteNotaFiscal",
                Tooltip = new HelperUITooltip()
                {
                    Text = "Se desmarcar, diversos campos referente a emissão de notas fiscais, deixam de ser obrigatórios e não são mais exibidos no cadastro das Vendas e no cadastro dos Pedidos de Compra. Exemplo: grupo tributário dos produtos/serviços, alíquotas e cálculos de impostos, opção de faturar o pedido."
                }
            });
            config.Helpers.Add(new TooltipUI
            {
                Id = "exibirStepProdutosVendas",
                Tooltip = new HelperUITooltip()
                {
                    Text = "Se desmarcar, deixa de exibir a aba de adicionar Produtos no cadastro das Vendas(Orçamento/Pedido)."
                }
            });
            config.Helpers.Add(new TooltipUI
            {
                Id = "exibirStepServicosVendas",
                Tooltip = new HelperUITooltip()
                {
                    Text = "Se desmarcar, deixa de exibir a aba de adicionar Serviços no cadastro das Vendas(Orçamento/Pedido)."
                }
            });
            config.Helpers.Add(new TooltipUI
            {
                Id = "exibirStepTransportadoraVendas",
                Tooltip = new HelperUITooltip()
                {
                    Text = "Se desmarcar, deixa de exibir a aba de adicionar Transportadora e informações de frete, no cadastro das Vendas(Orçamento/Pedido)."
                }
            });
            config.Helpers.Add(new TooltipUI
            {
                Id = "exibirStepTransportadoraCompras",
                Tooltip = new HelperUITooltip()
                {
                    Text = "Se desmarcar, deixa de exibir a aba de adicionar Transportadora e informações de frete, no cadastro dos Pedidos de Compra."
                }
            });
            #endregion

            cfg.Content.Add(config);

            return cfg;
        }

        [HttpPost]
        public override JsonResult Edit(T entityVM)
        {
            if(entityVM?.Id != null && entityVM?.Id != default(Guid))
            {
                return base.Edit(entityVM);
            }
            else
            {
                return base.Create(entityVM);
            }
        }

        public override ContentResult Json(Guid id)
        {
            try
            {
                var entity = RestHelper.ExecuteGetRequest<ResultBase<ConfiguracaoPersonalizacaoVM>>("ConfiguracaoPersonalizacao", queryString: null)?.Data?.FirstOrDefault();

                entity = (entity != null && entity.Id != default(Guid))
                    ? entity
                    : new ConfiguracaoPersonalizacaoVM()
                    {
                        EmiteNotaFiscal  = true,                        
                        ExibirStepProdutosVendas = true,
                        ExibirStepServicosVendas = true,
                        ExibirStepTransportadoraCompras = true,
                        ExibirStepTransportadoraVendas = true,
                        Id = default(Guid)
                    };
                return Content(JsonConvert.SerializeObject(entity, JsonSerializerSetting.Front), "application/json");
            }
            catch (Exception ex)
            {
                var error = JsonConvert.DeserializeObject<ErrorInfo>(ex.Message);
                return Content(JsonConvert.SerializeObject(JsonResponseStatus.GetFailure(error.Message).Data), "application/json");
            }
        }

        protected override List<JQueryDataTableParamsColumn> GetParamsColumns(string ResourceName = "")
        {
            throw new NotImplementedException();
        }
    }
}