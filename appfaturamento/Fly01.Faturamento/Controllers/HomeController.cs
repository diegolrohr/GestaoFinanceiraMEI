using System.Linq;
using System.Collections.Generic;
using System.Web.Mvc;
using Newtonsoft.Json;
using Fly01.uiJS.Classes;
using Fly01.Core.Config;
using Fly01.Core;
using Fly01.uiJS.Defaults;
using Fly01.Faturamento.ViewModel;
using Fly01.Core.Rest;
using System.Configuration;
using Fly01.uiJS.Classes.Widgets;
using Fly01.Core.Presentation;

namespace Fly01.Faturamento.Controllers
{
    public class HomeController : Core.Presentation.Controllers.HomeController
    {
        protected override ContentUI HomeJson()
        {
            if (!UserCanPerformOperation(ResourceHashConst.FaturamentoFaturamentoVisaoGeral))
                return new ContentUI { SidebarUrl = @Url.Action("Sidebar") };

            var cfg = new ContentUI
            {
                History = new ContentUIHistory
                {
                    Default = Url.Action("Index")
                },
                Header = new HtmlUIHeader()
                {
                    Title = "Visão Geral",
                    Buttons = new List<HtmlUIButton>()
                },
                UrlFunctions = Url.Action("Functions", "Home", null, Request.Url.Scheme) + "?fns=",
                SidebarUrl = @Url.Action("Sidebar")
            };

            cfg.Content.Add(new FormUI
            {
                ReadyFn = "fnFormReady",
                UrlFunctions = Url.Action("Functions", "Home", null, Request.Url.Scheme) + "?fns=",
                Class = "col s12"
            });

            cfg.Content.Add(new CardUI
            {
                Class = "col s12",
                Color = "totvs-blue",
                Id = "cardNotaFiscal",
                Title = "Nota Fiscal",
                Placeholder = "Número de Notas Fiscais não transmitidas",
                Action = new LinkUI
                {
                    Label = "Ver mais",
                    OnClick = Url.Action("List", "NotaFiscal") + "?action=GridLoadNoFilter"
                }
            });

            cfg.Content.Add(new AppUI()
            {
                Id = "nfenormal",
                Class = "col s12 m4",
                Title = "NF-e Normal",
                Icon = "https://mpn.azureedge.net/img/icon/nfe/normal.svg",
                Target = new LinkUI
                {
                    Go = Url.Action("Form", "Pedido")
                }
            });

            cfg.Content.Add(new AppUI()
            {
                Id = "nfedevolucao",
                Class = "col s12 m4",
                Title = "NF-e Devolução",
                Icon = "https://mpn.azureedge.net/img/icon/nfe/devolucao.svg",
                Target = new LinkUI
                {
                    Go = Url.Action("Form", "Pedido")
                }
            });

            cfg.Content.Add(new AppUI()
            {
                Id = "nfecomplemento",
                Class = "col s12 m4",
                Title = "NF-e Complemento",
                Icon = "https://mpn.azureedge.net/img/icon/nfe/complemento.svg",
                Target = new LinkUI
                {
                    Go = Url.Action("Form", "Pedido")
                }
            });            

            return cfg;
        }

        public override ContentResult Sidebar()
        {
            var config = new SidebarUI() { Id = "nav-bar", AppName = "Faturamento", Parent = "header" };

            #region MenuItems

            var menuItems = new List<SidebarUIMenu>()
            {
                new SidebarUIMenu()
                {
                    Class = ResourceHashConst.FaturamentoFaturamento,
                    Label = "Faturamento",
                    Items = new List<LinkUI>
                    {
                        new LinkUI() { Class = ResourceHashConst.FaturamentoFaturamentoVisaoGeral, Label = "Visão Geral", OnClick = @Url.Action("List")},
                        new LinkUI() { Class = ResourceHashConst.FaturamentoFaturamentoVendas, Label = "Vendas", OnClick = @Url.Action("List", "OrdemVenda")},
                        new LinkUI() { Class = ResourceHashConst.FaturamentoFaturamentoNotasFiscais, Label = "Notas Fiscais", OnClick = @Url.Action("List", "NotaFiscal")},
                    }
                },
                new SidebarUIMenu()
                {
                    Class = ResourceHashConst.FaturamentoCadastros,
                    Label = "Cadastros",
                    Items = new List<LinkUI>
                    {
                        new LinkUI() { Class = ResourceHashConst.FaturamentoCadastrosClientes, Label = "Clientes", OnClick = @Url.Action("List", "Cliente") },
                        new LinkUI() { Class = ResourceHashConst.FaturamentoCadastrosFornecedores, Label = "Fornecedores", OnClick = @Url.Action("List", "Fornecedor") },
                        new LinkUI() { Class = ResourceHashConst.FaturamentoCadastrosTransportadoras, Label = "Transportadoras", OnClick = @Url.Action("List", "Transportadora") },
                        new LinkUI() { Class = ResourceHashConst.FaturamentoCadastrosGrupoTributario, Label = "Grupo Tributário", OnClick = @Url.Action("List", "GrupoTributario") },
                        new LinkUI() { Class = ResourceHashConst.FaturamentoCadastrosProdutos, Label = "Produtos", OnClick = @Url.Action("List", "Produto") },
                        new LinkUI() { Class = ResourceHashConst.FaturamentoCadastrosGrupoProdutos, Label = "Grupo de Produtos", OnClick = @Url.Action("List", "GrupoProduto") },
                        new LinkUI() { Class = ResourceHashConst.FaturamentoCadastrosServicos, Label = "Serviços", OnClick = @Url.Action("List", "Servico") },
                        new LinkUI() { Class = ResourceHashConst.FaturamentoCadastrosCondicoesParcelamento, Label = "Condição Parcelamento", OnClick = @Url.Action("List", "CondicaoParcelamento") },
                        new LinkUI() { Class = ResourceHashConst.FaturamentoCadastrosFormasPagamento, Label = "Forma Pagamento", OnClick = @Url.Action("List", "FormaPagamento") },
                        new LinkUI() { Class = ResourceHashConst.FaturamentoCadastrosCategoria, Label = "Categoria", OnClick = @Url.Action("List", "Categoria") },
                        new LinkUI() { Class = ResourceHashConst.FaturamentoCadastrosSubstituicaoTributaria, Label = "Substituição Tributária", OnClick = @Url.Action("List", "SubstituicaoTributaria") }
                    }
                },
                new SidebarUIMenu()
                {
                    Class = ResourceHashConst.FaturamentoConfiguracoes,
                    Label = "Configurações",
                    Items = new List<LinkUI>
                    {
                        new LinkUI() { Class = ResourceHashConst.FaturamentoConfiguracoesCertificadoDigital, Label = "Certificado Digital", OnClick = @Url.Action("Form", "CertificadoDigital") },
                        new LinkUI() { Class = ResourceHashConst.FaturamentoConfiguracoesParametrosTributarios, Label = "Parâmetros Tributários", OnClick = @Url.Action("Form", "ParametroTributario") },
                        new LinkUI() { Class = ResourceHashConst.FaturamentoConfiguracoesSerieNotasFiscais, Label = "Série de Notas Fiscais", OnClick = @Url.Action("List", "SerieNotaFiscal")},
                        new LinkUI() { Class = ResourceHashConst.FaturamentoConfiguracoesNotasFiscaisInutilizadas, Label = "Notas Fiscais Inutilizadas", OnClick = @Url.Action("List", "NotaFiscalInutilizada") }
                    }
                },
                new SidebarUIMenu()
                {
                    Class = ResourceHashConst.FaturamentoAjuda,
                    Label = "Ajuda",
                    Items = new List<LinkUI>
                    {
                        new LinkUI() { Class = ResourceHashConst.FaturamentoAjudaAssistenciaRemota, Label = "Assistência Remota", Link = "https://secure.logmeinrescue.com/customer/code.aspx" }
                    }
                },
                new SidebarUIMenu() { Class = ResourceHashConst.FaturamentoAvalieAplicativo, Label = "Avalie o Aplicativo", OnClick = @Url.Action("List", "AvaliacaoApp") }
            };

            config.MenuItems.AddRange(ProcessMenuRoles(menuItems));
            #endregion

            #region User Menu Items

            config.UserMenuItems.Add(new LinkUI() { Label = "Sair", Link = @Url.Action("Logoff", "Account") });

            #endregion

            #region Lista de aplicativos do usuário

            config.MenuApps.AddRange(AppsList());

            #endregion

            config.Name = SessionManager.Current.UserData.TokenData.Username;
            config.Email = SessionManager.Current.UserData.PlatformUser;

            config.Widgets = new WidgetsUI
            {
                Conpass = new ConpassUI(),
                Droz = new DrozUI(),
                Zendesk = new ZendeskUI()
                {
                    AppName = "Fly01 Gestão",
                    AppTag = "chat_fly01_gestao",
                }
            };
            if (Request.Url.ToString().Contains("fly01.com.br"))
                config.Widgets.Insights = new InsightsUI { Key = ConfigurationManager.AppSettings["InstrumentationKeyAppInsights"] };

            return Content(JsonConvert.SerializeObject(config, JsonSerializerSetting.Front), "application/json");
        }

        private int GetCertificado()
        {
            ResultBase<NotaFiscalVM> response = RestHelper.ExecuteGetRequest<ResultBase<NotaFiscalVM>>("notafiscal",
                AppDefaults.GetQueryStringDefault());

            return response != null && response.Data != null
                ? response.Data.Count(x => x.Status.Equals("NaoTransmitida"))
                : 0;
        }

        public JsonResult StatusCard()
        {
            var numeroNFNaoTransmitida = GetCertificado();

            if (numeroNFNaoTransmitida == 0)
                return Json(new { numeroNFNaoTransmitidas = 0 }, JsonRequestBehavior.AllowGet);

            return Json(new
            {
                numeroNFNaoTransmitidas = numeroNFNaoTransmitida
            }, JsonRequestBehavior.AllowGet);
        }

    }
}