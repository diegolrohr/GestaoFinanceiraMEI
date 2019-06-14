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
using Fly01.Core.Helpers;
using Fly01.Core.ViewModels.Presentation.Commons;
using System;
using Fly01.uiJS.Classes.Elements;

namespace Fly01.Faturamento.Controllers
{
    public class HomeController : Core.Presentation.Controllers.HomeController
    {
        protected override ContentUI HomeJson()
        {
            if (!UserCanPerformOperation(ResourceHashConst.FaturamentoFaturamentoVisaoGeral))
                return new ContentUI { SidebarUrl = @Url.Action("Sidebar") };

            ConfiguracaoPersonalizacaoVM personalizacao = null;
            try
            {
                personalizacao = RestHelper.ExecuteGetRequest<ResultBase<ConfiguracaoPersonalizacaoVM>>("ConfiguracaoPersonalizacao", queryString: null)?.Data?.FirstOrDefault();
            }
            catch (Exception){}

            var emiteNotaFiscal = personalizacao != null ? personalizacao.EmiteNotaFiscal : true;

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
                Id = "fly01frm",
                ReadyFn = "fnFormReady",
                UrlFunctions = Url.Action("Functions", "Home", null, Request.Url.Scheme) + "?fns=",
                Class = "col s12",
                Elements = new List<BaseUI>()
                {
                    new InputHiddenUI() { Id = "emiteNotaFiscal", Value = emiteNotaFiscal.ToString() }
                }
            });

            if (emiteNotaFiscal)
            {
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

                var classCard = "col s12 m4";
                var empresa = ApiEmpresaManager.GetEmpresa(SessionManager.Current.UserData.PlatformUrl);
                var cidadeHomologadaTss = (!string.IsNullOrEmpty(empresa?.Cidade?.CodigoIbge) && NFSeTssHelper.IbgesCidadesHomologadasTssNFSe.Contains(empresa?.Cidade?.CodigoIbge));
                if (cidadeHomologadaTss)
                {
                    classCard = "col s12 m3";
                    cfg.Content.Add(new AppUI()
                    {
                        Id = "nfsenormal",
                        Class = classCard,
                        Title = "NFS-e Serviço",
                        Icon = "https://mpn.azureedge.net/img/icon/nfe/servico.svg",
                        Target = new LinkUI
                        {
                            Go = Url.Action("FormPedido", "Pedido", new { isEdit = "false", tipoVenda = "Normal" })
                        }
                    });
                }

                cfg.Content.Add(new AppUI()
                {
                    Id = "nfenormal",
                    Class = classCard,
                    Title = "NF-e Normal",
                    Icon = "https://mpn.azureedge.net/img/icon/nfe/normal.svg",
                    Target = new LinkUI
                    {
                        Go = Url.Action("FormPedido", "Pedido", new { isEdit = "false", tipoVenda = "Normal" })
                    }
                });

                cfg.Content.Add(new AppUI()
                {
                    Id = "nfedevolucao",
                    Class = classCard,
                    Title = "NF-e Devolução",
                    Icon = "https://mpn.azureedge.net/img/icon/nfe/devolucao.svg",
                    Target = new LinkUI
                    {
                        Go = Url.Action("FormPedido", "Pedido", new { isEdit = "false", tipoVenda = "Devolucao" })
                    }
                });

                cfg.Content.Add(new AppUI()
                {
                    Id = "nfecomplemento",
                    Class = classCard,
                    Title = "NF-e Complemento",
                    Icon = "https://mpn.azureedge.net/img/icon/nfe/complemento.svg",
                    Target = new LinkUI
                    {
                        Go = Url.Action("FormPedido", "Pedido", new { isEdit = "false", tipoVenda = "Complementar" })
                    }
                });

            }
            else
            {
                //redirect no JavaScript até Fraga decidir um dashboard
            }

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
                        new LinkUI() { Class = ResourceHashConst.FaturamentoCadastrosSubstituicaoTributaria, Label = "Substituição Tributária", OnClick = @Url.Action("List", "SubstituicaoTributaria") },
                        new LinkUI() { Class = ResourceHashConst.FaturamentoCadastrosKit, Label = "Kit Produtos/Serviços", OnClick = @Url.Action("List", "Kit") },
                        new LinkUI() { Class = ResourceHashConst.FaturamentoCadastrosCentroCustos, Label = "Centro de Custos", OnClick = @Url.Action("List", "CentroCusto") }
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
                        //Personalizar Sistema não vai ter hash especifico de permissão, segundo Fraga
                        new LinkUI() { Class = ResourceHashConst.FaturamentoConfiguracoes, Label = "Personalizar Sistema", OnClick = @Url.Action("Form", "ConfiguracaoPersonalizacao") },
                        new LinkUI() { Class = ResourceHashConst.FaturamentoConfiguracoesNotasFiscaisInutilizadas, Label = "Notas Fiscais Inutilizadas", OnClick = @Url.Action("List", "NotaFiscalInutilizada") }
                    }
                },
                new SidebarUIMenu()
                {
                    Class = ResourceHashConst.FaturamentoAjuda,
                    Label = "Ajuda",
                    Items = new List<LinkUI>
                    {
                        new LinkUI() { Class = ResourceHashConst.FaturamentoAjudaAssistenciaRemota, Label =  "Assistência Remota", OnClick = @Url.Action("Form", "AssistenciaRemota") },
                        new LinkUI() { Class = ResourceHashConst.FaturamentoAjuda,Label = "Manual do Usuário", Link = "https://centraldeatendimento.totvs.com/hc/pt-br/categories/360000364572" }
                    }
                },
                new SidebarUIMenu() { Class = ResourceHashConst.FaturamentoAvalieAplicativo, Label = "Avalie o Aplicativo", OnClick = @Url.Action("List", "AvaliacaoApp") }
            };

            config.MenuItems.AddRange(ProcessMenuRoles(menuItems));
            #endregion

            #region User Menu Items
            if (!string.IsNullOrEmpty(SessionManager.Current.UserData.TokenData.CodigoMaxime))
                config.UserMenuItems.Add(new LinkUI() { Label = "Minha Conta", OnClick = @Url.Action("List", "MinhaConta") });
            config.UserMenuItems.Add(new LinkUI() { Label = "Sair", Link = @Url.Action("Logoff", "Account") });

            #endregion

            #region Lista de aplicativos do usuário

            config.MenuApps.AddRange(AppsList());

            #endregion

            config.Name = SessionManager.Current.UserData.TokenData.Username;
            config.Email = SessionManager.Current.UserData.PlatformUser;
            config.Notification = new SidebarUINotification()
            {
                Channel = AppDefaults.AppId + "_" + SessionManager.Current.UserData.PlatformUrl,
                JWT = @Url.Action("NotificationJwt"),
                SocketServer = AppDefaults.UrlNotificationSocket
            };


            config.Widgets = new WidgetsUI
            {
                Conpass = new ConpassUI(),
                Droz = new DrozUI(),
                Zendesk = new ZendeskUI()
                {
                    AppName = "Bemacash Gestão",
                    AppTag = "chat_fly01_gestao",
                }
            };
            if (Request.Url.ToString().Contains("bemacash.com.br"))
                config.Widgets.Insights = new InsightsUI { Key = ConfigurationManager.AppSettings["InstrumentationKeyAppInsights"] };

            return Content(JsonConvert.SerializeObject(config, JsonSerializerSetting.Front), "application/json");
        }

        private string GenerateJWT()
        {
            var payload = new Dictionary<string, string>()
                {
                    {  "platformUrl", SessionManager.Current.UserData.PlatformUrl },
                    {  "clientId", AppDefaults.AppId },
                };
            var token = JWTHelper.Encode(payload,"https://meu.bemacash.com.br/", DateTime.Now.AddMinutes(60));
            return token;
        }

        public JsonResult NotificationJwt()
        {
            return Json(new
            {
                token = GenerateJWT()
            }, JsonRequestBehavior.AllowGet);
        }

        private int GetNotasNaoTransmitidas()
        {
            Dictionary<string, string> queryString = AppDefaults.GetQueryStringDefault();
            queryString.AddParam("$filter", $"status eq {AppDefaults.APIEnumResourceName}StatusNotaFiscal'NaoTransmitida'");
            queryString.AddParam("$select", "id");

            ResultBase<NotaFiscalVM> response = RestHelper.ExecuteGetRequest<ResultBase<NotaFiscalVM>>("notafiscal",
                queryString);

            return response != null && response.Data != null
                ? response.Data.Count()
                : 0;
        }
                
        public JsonResult StatusCard()
        {
            var numeroNFNaoTransmitida = GetNotasNaoTransmitidas();

            if (numeroNFNaoTransmitida == 0)
                return Json(new { numeroNFNaoTransmitidas = 0 }, JsonRequestBehavior.AllowGet);

            return Json(new
            {
                numeroNFNaoTransmitidas = numeroNFNaoTransmitida
            }, JsonRequestBehavior.AllowGet);
        }

    }
}