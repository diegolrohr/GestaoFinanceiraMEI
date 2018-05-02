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

namespace Fly01.Faturamento.Controllers
{
    public class HomeController : Core.Presentation.Controllers.HomeController
    {
        protected override ContentUI HomeJson(bool withSidebarUrl = false)
        {
            var config = new ContentUI
            {
                History = new ContentUIHistory { Default = Url.Action("Index") },
                SidebarUrl = Url.Action("Sidebar"),
                Header = new HtmlUIHeader()
                {
                    Title = "Visão Geral",
                    Buttons = new List<HtmlUIButton>()
                },
                UrlFunctions = Url.Action("Functions", "Home", null, Request.Url.Scheme) + "?fns="
            };

            config.Content.Add(new FormUI
            {
                ReadyFn = "fnFormReady",
                UrlFunctions = Url.Action("Functions", "Home", null, Request.Url.Scheme) + "?fns=",
                Class = "col s12"
            });

            config.Content.Add(new CardUI
            {
                Class = "col s12",
                Color = "orange",
                Id = "cardNotaFiscal",
                Title = "Nota Fiscal",
                Placeholder = "Número de Notas Fiscais não transmitidas",
                Action = new LinkUI
                {
                    Label = "Ver mais",
                    OnClick = Url.Action("List", "NotaFiscal") + "?action=GridLoadNoFilter"
                }
            });

            if (withSidebarUrl)
                config.SidebarUrl = Url.Action("Sidebar", "Home", null, Request.Url.Scheme);

            return config;
        }

        public override ContentResult Sidebar()
        {
            var config = new SidebarUI() {Id = "nav-bar", AppName = "Faturamento", Parent = "header"};

            #region MenuItems

            config.MenuItems.Add(new SidebarMenuUI()
            {
                Label = "Faturamento",
                Items = new List<LinkUI>
                {
                    new LinkUI() {Label = "Vendas", OnClick = @Url.Action("List", "OrdemVenda")},
                    new LinkUI() {Label = "Notas Fiscais", OnClick = @Url.Action("List", "NotaFiscal")},
                }
            });

            config.MenuItems.Add(new SidebarMenuUI()
            {
                Label = "Cadastros",
                Items = new List<LinkUI>
                {
                    new LinkUI() {Label = "Clientes", OnClick = @Url.Action("List", "Cliente")},
                    new LinkUI() {Label = "Fornecedores", OnClick = @Url.Action("List", "Fornecedor")},
                    new LinkUI() {Label = "Transportadoras", OnClick = @Url.Action("List", "Transportadora")},
                    new LinkUI() {Label = "Grupo Tributário", OnClick = @Url.Action("List", "GrupoTributario")},
                    new LinkUI() {Label = "Produtos", OnClick = @Url.Action("List", "Produto")},
                    new LinkUI() {Label = "Grupo de Produtos", OnClick = @Url.Action("List", "GrupoProduto")},
                    new LinkUI() {Label = "Serviços", OnClick = @Url.Action("List", "Servico")},
                    new LinkUI()
                    {
                        Label = "Condição Parcelamento",
                        OnClick = @Url.Action("List", "CondicaoParcelamento")
                    },
                    new LinkUI() {Label = "Forma Pagamento", OnClick = @Url.Action("List", "FormaPagamento")},
                    new LinkUI() {Label = "Categoria", OnClick = @Url.Action("List", "Categoria")},
                    new LinkUI()
                    {
                        Label = "Substituição Tributária",
                        OnClick = @Url.Action("List", "SubstituicaoTributaria")
                    }
                }
            });

            config.MenuItems.Add(new SidebarMenuUI()
            {
                Label = "Configurações",
                Items = new List<LinkUI>
                {
                    new LinkUI() {Label = "Certificado Digital", OnClick = @Url.Action("Form", "CertificadoDigital")},
                    new LinkUI()
                    {
                        Label = "Parâmetros Tributários",
                        OnClick = @Url.Action("Form", "ParametroTributario")
                    },
                    new LinkUI() {Label = "Série de Notas Fiscais", OnClick = @Url.Action("List", "SerieNotaFiscal")},
                    //new LinkUI() //TODO falta api emissao
                    //{
                    //    Label = "Notas Fiscais Inutilizadas",
                    //    OnClick = @Url.Action("List", "SerieNotaFiscalInutilizada")
                    //}
                }
            });


            config.MenuItems.Add(new SidebarMenuUI()
            {
                Label = "Ajuda",
                Items = new List<LinkUI>
                {
                    new LinkUI()
                    {
                        Label = "Assistência Remota",
                        Link = "https://secure.logmeinrescue.com/customer/code.aspx"
                    }
                }
            });

            #endregion

            #region User Menu Items

            config.UserMenuItems.Add(new LinkUI() {Label = "Sair", Link = @Url.Action("Logoff", "Account")});

            #endregion

            #region Lista de aplicativos do usuário

            config.MenuApps.AddRange(AppsList());

            #endregion

            config.Name = SessionManager.Current.UserData.TokenData.Username;
            config.Email = SessionManager.Current.UserData.PlatformUser;

            config.Widgets = new WidgetsUI();
            config.Widgets.Conpass = new ConpassUI();
            config.Widgets.Zendesk = new ZendeskUI()
            {
                AppName = "Fly01 Faturamento",
                AppTag = "fly01_manufatura",
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
                return Json(new {numeroNFNaoTransmitidas = 0}, JsonRequestBehavior.AllowGet);

            return Json(new
            {
                numeroNFNaoTransmitidas = numeroNFNaoTransmitida
            }, JsonRequestBehavior.AllowGet);
        }

    }
}